using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MultiAnimationEnemyStats : LargeEnemyStats
{

    private readonly static SpawnDetails barricadeSpawnDetails = new SpawnDetails(new GridCoords[] {
                                                                        new GridCoords(Constants.indexThree, Constants.indexZero),
                                                                        new GridCoords(Constants.indexThree, Constants.indexOne),
                                                                        new GridCoords(Constants.indexThree, Constants.indexTwo),
                                                                        new GridCoords(Constants.indexThree, Constants.indexThree)
                                                                      }, true);

    #region Global Variables
    public Dictionary<GridCoords, GameObject> combatSprites;
    public Dictionary<GridCoords, SpriteRenderer> spriteRenderers;
    public Dictionary<GridCoords, AnimationManager> animationManagers;
    public Dictionary<GridCoords, SpriteOutline> outlines;
    public Dictionary<GridCoords, CombatantHover> combatantHovers;
    public Dictionary<GridCoords, TutorialSequenceStepTargetObject> tutorialTargets;

    #endregion


    #region Constructors

    public MultiAnimationEnemyStats(string key, int armor, int tHP, Trait[] traits, CombatAction combatAction = null) :
    base(key, armor, tHP, traits: traits, combatAction: combatAction)
    {
        
    }

    #endregion

    #region Sprite and GameObject

    protected override GameObject instantiateCombatSprite()
    {
        if(spawnDetails == null)
        {
            if(getName().Contains(NPCNameList.barricade))
            {
                spawnDetails = barricadeSpawnDetails;
            } else
            {
                spawnDetails = State.enemyPackInfo.getNextSpawnDetails();
            }
        }

        combatSprites = new Dictionary<GridCoords, GameObject>();
        animationManagers = new Dictionary<GridCoords, AnimationManager>();
        spriteRenderers = new Dictionary<GridCoords, SpriteRenderer>();
        outlines = new Dictionary<GridCoords, SpriteOutline>();
        combatantHovers = new Dictionary<GridCoords, CombatantHover>();
        tutorialTargets = new Dictionary<GridCoords, TutorialSequenceStepTargetObject>();

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
            animationManager.linkedStats = this;
            animationManager.healthBarManager = healthBarManager;
            animationManager.setAnimations(getName());
        }

        combatSprite = combatSprites[spawnDetails.baseStatsPosition];

        position = spawnDetails.baseStatsPosition;

        setHealthBarToAverageWorldPosition();

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

        tutorialTargets[coords] = list.tutorialTarget;
        tutorialTargets[coords].tutorialHash = getTutorialTargetHash();
    }

    public override void destroyCombatSprite()
    {
        foreach(GameObject gameObject in combatSprites.Values)
        {
            Destroy(gameObject);
        }
    }

    // public override void removeFromGrid()
    // {
    //     foreach(GridCoords coords in spawnDetails.allSpawnPositions)
    //     {
    //         CombatGrid.setCombatantAtCoords(coords, null);
    //     }
    // }

    // public override GridCoords getPositionToHit(Selector selector, int skips)
    // {
    //     GridCoords[] allSelectorCoords = selector.getAllSelectorCoords();
    //     List<GridCoords> allCompatabilePositions = new List<GridCoords>();

    //     foreach (GridCoords coords in allSelectorCoords)
    //     {
    //         if (spawnDetails.allSpawnPositions.Contains(coords))
    //         {
    //             allCompatabilePositions.Add(coords);
    //         }
    //     }

    //     if (allCompatabilePositions.Count == 0 || skips >= allCompatabilePositions.Count)
    //     {
    //         return position.clone();
    //     }
    //     else
    //     {
    //         return allCompatabilePositions[skips];
    //     }
    // }

    public override void setOutline()
    {
        foreach(SpriteOutline outline in outlines.Values)
        {
            outline.createOutline(getOutlineColor());
        }
    }

    public override void setOutline(byte alpha)
    {
        Color32 color = getOutlineColor();

        color.a = alpha;

        foreach(SpriteOutline outline in outlines.Values)
        {
            outline.createOutline(color);
        }
    }

    public override void removeOutline()
    {
        foreach(SpriteOutline outline in outlines.Values)
        {
            outline.removeOutline();
        }
    }

    public override SpriteOutline[] getOutlines()
    {
        return outlines.Values.ToArray();
    }

    // public override bool isInsideCoordinates(GridCoords coords)
    // {
    //     return spawnDetails.allSpawnPositions.Contains(coords);
    // }

    // public override bool isInsideCoordinates(GridCoords[] coords)
    // {
    //     return coords.Any(x => spawnDetails.allSpawnPositions.Contains(x));
    // }

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

    #endregion

}
