using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//the GameObject side of a combatant: everything Stats used to reach into the combat prefab for.
//Added to the Creature prefab by CombatantSpawnDetails, one per GameObject, so a MultiAnimationEnemyStats has one per cell
public class Combatant : MonoBehaviour, INameSource
{
    private const string healthBarCanvasName = "Health Bar Canvas";
    private const string promptParentName = "Prompt Parent";
    private const string healthBarSortingLayerName = "DialogueBox";
    private const int healthBarSortingOrder = 1;

    public Stats stats { get; private set; }
    public GridCoords cell { get; private set; }

    //the primary owns the health bar and listens to the Stats; the others only draw their own cell
    public bool isPrimary { get; private set; }
    public bool isPlaceholder { get; private set; }

    public NewAnimationManager animationManager { get; private set; }
    public SpriteLayerRendererList rendererList { get; private set; }
    public HealthBarManager healthBarManager { get; private set; }
    public Transform promptParent { get; private set; }

    //added by the aesthetic spawn behaviours, which run before this is initialized
    public CombatantHover hover { get; private set; }

    private bool listening;

    #region INameSource

    public string displayName { get { return stats != null ? stats.displayName : ""; } }
    public string uniqueName { get { return stats != null ? stats.uniqueName : ""; } }

    #endregion

    #region Initialization

    public void initialize(Stats stats, GridCoords cell, bool isPrimary, bool isPlaceholder = false)
    {
        this.stats = stats;
        this.cell = cell;
        this.isPrimary = isPrimary;
        this.isPlaceholder = isPlaceholder;

        rendererList = GetComponent<SpriteLayerRendererList>();
        animationManager = GetComponent<NewAnimationManager>();
        hover = GetComponentInChildren<CombatantHover>(true);
        promptParent = createChild(promptParentName).transform;

        if(animationManager != null)
        {
            rendererList.setFlipX(animationManager.characterFacing.flipSprite());
        }

        if(hover != null)
        {
            hover.linkedStats = stats;
        }

        if(isPlaceholder)
        {
            rendererList.setAlpha(RepositionPlaceholderGenerator.placeHolderSpriteOpaqueness);

            //a reposition clone is drawn by its placeholder, but a combatant that already has a GameObject keeps it
            if(stats.combatant != null)
            {
                return;
            }
        }

        stats.combatants.Add(this);

        if(!isPrimary)
        {
            return;
        }

        stats.combatant = this;

        if(!isPlaceholder)
        {
            createHealthBar();
        }

        addListeners();

        if(stats.isDead())
        {
            setToDeadIdle();
        }
    }

    private GameObject createChild(string childName)
    {
        GameObject child = new GameObject(childName, typeof(RectTransform));

        child.transform.SetParent(transform, false);

        return child;
    }

    //recreates the world space Canvas the old combat prefabs carried, since the health bar is UI
    private void createHealthBar()
    {
        GameObject canvasObject = createChild(healthBarCanvasName);

        RectTransform canvasRect = canvasObject.GetComponent<RectTransform>();
        canvasRect.anchorMin = Vector2.zero;
        canvasRect.anchorMax = Vector2.one;
        canvasRect.pivot = new Vector2(.5f, 1f);
        canvasRect.offsetMin = Vector2.zero;
        canvasRect.offsetMax = Vector2.zero;

        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.overrideSorting = true;
        canvas.sortingLayerName = healthBarSortingLayerName;
        canvas.sortingOrder = healthBarSortingOrder;

        healthBarManager = HealthBarManager.createHealthBar(stats, canvasRect);

        updateHealthBar();
    }

    #endregion

    #region Stats Listeners

    private void addListeners()
    {
        stats.OnPositionsChanged += updatePosition;
        stats.OnHealthBarUpdate += updateHealthBar;
        stats.OnDeath += onDeath;
        stats.OnRevive += onRevive;
        stats.OnTraitAdded += onTraitAdded;
        stats.OnTraitRemoved += onTraitRemoved;

        listening = true;
    }

    private void removeListeners()
    {
        if(!listening)
        {
            return;
        }

        stats.OnPositionsChanged -= updatePosition;
        stats.OnHealthBarUpdate -= updateHealthBar;
        stats.OnDeath -= onDeath;
        stats.OnRevive -= onRevive;
        stats.OnTraitAdded -= onTraitAdded;
        stats.OnTraitRemoved -= onTraitRemoved;

        listening = false;
    }

    private void OnDestroy()
    {
        if(stats == null)
        {
            return;
        }

        removeListeners();

        stats.combatants.Remove(this);

        if(ReferenceEquals(stats.combatant, this))
        {
            stats.combatant = null;
        }
    }

    //a multi cell combatant keeps one GameObject on each of its cells, so only a single sprite follows the Stats around
    public void updatePosition()
    {
        if(stats.positions.Count <= 0 || stats.combatants.Count > 1)
        {
            return;
        }

        cell = CombatantSpawnDetails.getSpriteCell(stats.positions);
        transform.position = CombatGrid.getPositionAt(cell);
    }

    public void updateHealthBar()
    {
        if(healthBarManager == null)
        {
            return;
        }

        healthBarManager.setLinkedStats(stats);
        healthBarManager.setTotalHealth(stats.getTotalHealth());
        healthBarManager.setMissingHealth(stats.getMissingHealth());
        healthBarManager.resetPreviewHealth();
    }

    //removedFromCombat is true when the combatant can't be brought back, so its GameObjects go with it
    private void onDeath(bool removedFromCombat)
    {
        if(removedFromCombat)
        {
            despawn();
        } else if(healthBarManager != null)
        {
            healthBarManager.hide();
        }

        if(CombatStateManager.whoseTurn == WhoseTurn.Start)
        {
            setToDeadIdle();
        }
    }

    private void onRevive()
    {
        if(healthBarManager != null)
        {
            healthBarManager.show();
        }

        foreach(NewAnimationManager manager in allAnimationManagers)
        {
            manager.setToDefaultIdle();
            manager.playSpawnAnimation();
        }
    }

    private void onTraitAdded(Trait trait)
    {
        if(!CombatStateManager.inCombat)
        {
            return;
        }

        foreach(NewAnimationManager manager in allAnimationManagers)
        {
            manager.setIdleAnimationOnTraitApplication(trait);
        }
    }

    private void onTraitRemoved(Trait trait)
    {
        if(stats.isDead())
        {
            return;
        }

        foreach(NewAnimationManager manager in allAnimationManagers)
        {
            manager.setIdleAnimationOnTraitRemoval(trait);
        }
    }

    #endregion

    #region Visuals

    //every GameObject drawing this combatant, which is just this one unless the Stats spans several cells
    private IEnumerable<Combatant> allCombatants
    {
        get
        {
            if(stats != null && stats.combatants.Contains(this))
            {
                return stats.combatants.ToList();
            }

            return new List<Combatant>() { this };
        }
    }

    private IEnumerable<NewAnimationManager> allAnimationManagers
    {
        get
        {
            foreach(Combatant combatant in allCombatants)
            {
                if(combatant.animationManager != null)
                {
                    yield return combatant.animationManager;
                }
            }
        }
    }

    public void setOutline()
    {
        setOutline(stats.getOutlineColor());
    }

    public void setOutline(byte alpha)
    {
        Color32 color = stats.getOutlineColor();

        color.a = alpha;

        setOutline((Color) color);
    }

    private void setOutline(Color color)
    {
        foreach(Combatant combatant in allCombatants)
        {
            combatant.rendererList.createOutline(color);
        }
    }

    public void removeOutline()
    {
        if(stats.isDead() && stats.notResurrectable())
        {
            return;
        }

        foreach(Combatant combatant in allCombatants)
        {
            combatant.rendererList.removeOutline();
        }
    }

    public void disablePolygonCollider()
    {
        foreach(Combatant combatant in allCombatants)
        {
            combatant.rendererList.setBodyColliderEnabled(false);
        }
    }

    public void playAnimationOnDamage()
    {
        if(stats.isDead())
        {
            if(healthBarManager != null)
            {
                healthBarManager.hide();
            }

            foreach(NewAnimationManager manager in allAnimationManagers)
            {
                manager.playDeathAnimation();
            }
        } else
        {
            foreach(NewAnimationManager manager in allAnimationManagers)
            {
                manager.playWoundedAnimation();
            }
        }
    }

    private void setToDeadIdle()
    {
        if(stats.positions.Count <= 0)
        {
            return;
        }

        foreach(NewAnimationManager manager in allAnimationManagers)
        {
            manager.setToDeadIdle(CombatGrid.positionIsOnEnemySide(stats.positions[0]));
        }
    }

    public List<Vector3> getAllWorldPositions()
    {
        return stats.positions.Select(p => CombatGrid.getPositionAt(p)).ToList();
    }

    public void despawn()
    {
        foreach(Combatant combatant in allCombatants)
        {
            Destroy(combatant.gameObject);
        }
    }

    #endregion
}

//null safe routes from a Stats to its Combatant, for the many callers that only hold the Stats.
//Each returns a real null, rather than a destroyed Unity object, when the Stats has no GameObject
public static class CombatantStatsExtensions
{
    public static Combatant getCombatant(this Stats stats)
    {
        if(stats == null || stats.combatant == null)
        {
            return null;
        }

        return stats.combatant;
    }

    public static GameObject getCombatSprite(this Stats stats)
    {
        Combatant combatant = stats.getCombatant();

        return combatant == null ? null : combatant.gameObject;
    }

    public static HealthBarManager getHealthBarManager(this Stats stats)
    {
        Combatant combatant = stats.getCombatant();

        if(combatant == null || combatant.healthBarManager == null)
        {
            return null;
        }

        return combatant.healthBarManager;
    }

    public static NewAnimationManager getAnimationManager(this Stats stats)
    {
        Combatant combatant = stats.getCombatant();

        if(combatant == null || combatant.animationManager == null)
        {
            return null;
        }

        return combatant.animationManager;
    }

    public static void setOutline(this Stats stats)
    {
        Combatant combatant = stats.getCombatant();

        if(combatant != null)
        {
            combatant.setOutline();
        }
    }

    public static void setOutline(this Stats stats, byte alpha)
    {
        Combatant combatant = stats.getCombatant();

        if(combatant != null)
        {
            combatant.setOutline(alpha);
        }
    }

    public static void removeOutline(this Stats stats)
    {
        Combatant combatant = stats.getCombatant();

        if(combatant != null)
        {
            combatant.removeOutline();
        }
    }

    public static void disablePolygonCollider(this Stats stats)
    {
        Combatant combatant = stats.getCombatant();

        if(combatant != null)
        {
            combatant.disablePolygonCollider();
        }
    }

    public static void playAnimationOnDamage(this Stats stats)
    {
        Combatant combatant = stats.getCombatant();

        if(combatant != null)
        {
            combatant.playAnimationOnDamage();
        }
    }

    public static void playSpawnAnimation(this Stats stats)
    {
        NewAnimationManager animationManager = stats.getAnimationManager();

        if(animationManager != null)
        {
            animationManager.playSpawnAnimation();
        }
    }

    public static void playAttackAnimation(this Stats stats)
    {
        NewAnimationManager animationManager = stats.getAnimationManager();

        if(animationManager != null)
        {
            animationManager.playAttackAnimation();
        }
    }

    public static void playAttackIntoFrontIdleAnimation(this Stats stats)
    {
        NewAnimationManager animationManager = stats.getAnimationManager();

        if(animationManager != null)
        {
            animationManager.playAttackIntoFrontIdleAnimation();
        }
    }

    public static void playAttackIntoSecondaryIdleAnimation(this Stats stats)
    {
        NewAnimationManager animationManager = stats.getAnimationManager();

        if(animationManager != null)
        {
            animationManager.playAttackIntoSecondaryIdleAnimation();
        }
    }

    public static void playSpecialAttackAnimation(this Stats stats)
    {
        NewAnimationManager animationManager = stats.getAnimationManager();

        if(animationManager != null)
        {
            animationManager.playSpecialAttackAnimation();
        }
    }
}
