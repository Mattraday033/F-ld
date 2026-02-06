using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public struct SpawnDetails
{
	public bool hasSpawnDetails;
	public bool dontSpawnWhenSurprised;
	
	public GridCoords[] allSpawnPositions; //every coords that has a reference to the enemy's stats
	public GridCoords baseStatsPosition;   //the coords put into the "position" of the base class
	public GridCoords spritePosition;      //the coords that the sprite is placed at on the grid
	
	public SpawnDetails(GridCoords[] allSpawnPositions, GridCoords baseStatsPosition, GridCoords spritePosition, bool dontSpawnWhenSurprised)
	{
		this.allSpawnPositions = allSpawnPositions;
		this.baseStatsPosition = baseStatsPosition;
		this.spritePosition = spritePosition;
		this.dontSpawnWhenSurprised = dontSpawnWhenSurprised;
		
		this.hasSpawnDetails = true;
	}
}

public class LargeEnemyStats : EnemyStats
{

    #region Global Variables
    public SpawnDetails spawnDetails;

    public Dictionary<GridCoords, GameObject> combatSprites;
    public Dictionary<GridCoords, SpriteRenderer> spriteRenderers;
    public Dictionary<GridCoords, AnimationManager> animationManagers;
    public Dictionary<GridCoords, SpriteOutline> outlines;
    public Dictionary<GridCoords, CombatantHover> combatantHovers;

    #endregion

    #region Unity Events
    public readonly static UnityEvent OnLargeEnemySpawn = new UnityEvent();
    #endregion


    #region Constructors

    //There is a difference between multi-sprite enemies (barricades) and large single sprited enemies (large worms, horses, etc). 
    //Currently set up for multi-sprited enemies, will eventually need to decouple when re-implementing worms/horses
    public LargeEnemyStats(string key, int armor, int tHP, Trait[] traits, SpawnDetails spawnDetails) :
    base(key, armor, tHP, traits: traits)
    {
        this.spawnDetails = spawnDetails;
        position = spawnDetails.baseStatsPosition;
    }

    #endregion

    #region Sprite and GameObject

    public override GameObject instantiateCombatSprite(GridCoords coords)
    {
        return instantiateCombatSprite();
    }

    private GameObject instantiateCombatSprite()
    {
        combatSprites = new Dictionary<GridCoords, GameObject>();
        animationManagers = new Dictionary<GridCoords, AnimationManager>();
        spriteRenderers = new Dictionary<GridCoords, SpriteRenderer>();
        outlines = new Dictionary<GridCoords, SpriteOutline>();
        combatantHovers = new Dictionary<GridCoords, CombatantHover>();

        foreach(GridCoords coords in spawnDetails.allSpawnPositions)
        {
            combatSprites[coords] = Instantiate(Resources.Load<GameObject>(getCombatSpriteName()), CombatStateManager.getCreatureParent());

            if(getName().Contains(NPCNameList.barricade))
            {
                combatSprites[coords].transform.localScale = Constants.reverseScaleChange;
            } else
            {
                combatSprites[coords].transform.localScale = Vector3.one;
            }

            combatSprites[coords].transform.position = CombatGrid.getPositionAt(coords);

            ComponentList componentList = combatSprites[coords].GetComponent<ComponentList>();

            if(coords.Equals(spawnDetails.baseStatsPosition))
            {
                setUpComponents(componentList);
            } else
            {
                componentList.healthBarManager.hide();
            }

            setUpComponents(coords, componentList);

            CombatGrid.setCombatantAtCoords(coords, this);
        }

        foreach(AnimationManager animationManager in animationManagers.Values)
        {
            animationManager.healthBarManager = healthBarManager;
            animationManager.setAnimations(getName());
        }

        combatSprite = combatSprites[spawnDetails.baseStatsPosition];

        OnLargeEnemySpawn.Invoke();

        return combatSprite;
    }

    public override void setUpComponents(ComponentList list)
    {
        healthBarManager = list.healthBarManager;
        updateHealthBar();
    }

    public void setUpComponents(GridCoords coords, ComponentList list)
    {
        animationManagers[coords] = list.animationManager;
        
        spriteRenderers[coords] = list.spriteRenderer;

        outlines[coords] = new SpriteOutline();
        outlines[coords].setSpriteRenderer(spriteRenderers[coords]);

        combatantHovers[coords] = list.combatantHover;
        combatantHovers[coords].linkedStats = this;
    }

    public override void destroyCombatSprite()
    {
        foreach(GameObject gameObject in combatSprites.Values)
        {
            Destroy(gameObject);
        }
    }

    public override void removeFromGrid()
    {
        foreach(GridCoords coords in spawnDetails.allSpawnPositions)
        {
            CombatGrid.setCombatantAtCoords(coords, null);
        }
    }

    public override GridCoords getPositionToHit(Selector selector, int skips)
    {
        GridCoords[] allSelectorCoords = selector.getAllSelectorCoords();
        List<GridCoords> allCompatabilePositions = new List<GridCoords>();

        foreach (GridCoords coords in allSelectorCoords)
        {
            if (spawnDetails.allSpawnPositions.Contains(coords))
            {
                allCompatabilePositions.Add(coords);
            }
        }

        if (allCompatabilePositions.Count == 0 || skips >= allCompatabilePositions.Count)
        {
            return position.clone();
        }
        else
        {
            return allCompatabilePositions[skips];
        }
    }

    public override void setOutline()
    {
        foreach(SpriteOutline outline in outlines.Values)
        {
            outline.createOutline(getOutlineColor());
        }
    }
    public override void removeOutline()
    {
        foreach(SpriteOutline outline in outlines.Values)
        {
            outline.removeOutline();
        }
    }

    public override bool isInsideCoordinates(GridCoords coords)
    {
        return spawnDetails.allSpawnPositions.Contains(coords);
    }

    public override bool isInsideCoordinates(GridCoords[] coords)
    {
        return coords.Any(x => spawnDetails.allSpawnPositions.Contains(x));
    }

    #endregion

    #region AnimationManager

    public override void playAnimationOnDamage()
    {
        if (isDead())
        {
            foreach(AnimationManager animationManager in animationManagers.Values)
            {
                animationManager.playDeathAnimation();
            }

            healthBarManager.hide();
        } else
        {
            foreach(AnimationManager animationManager in animationManagers.Values)
            {
                animationManager.playWoundedAnimation();
            }
        }
    }

    #endregion

    #region Health

    #endregion

    #region Combat and Actions

    #endregion

    #region Traits

    public override bool isLarge()
    {
        return true;
    }

    #endregion

}
