using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

[System.Serializable]
public class Selector : ICloneable
{
	private const bool shouldIncludeIllegalCoords = true;
	
	public string name;
	
    public readonly static Color secondaryColor = Color.yellow;
	public Color originalColor = Color.red;
	
	public bool selfTargeting = false;
	
    private GridCoords _StartingCoords;
    public GridCoords startingCoords
    {
        private set
        {
            _StartingCoords = value.clone();
        }
        get
        {
            return _StartingCoords.clone();
        }
    }
    private Rect rect;
	
	private bool[,] spaces;
	
    // public int startUpperBounds { private set{} get { return startingCoords.row; }}
    // public int startLowerBounds { private set{} get { return startingCoords.row + (int) rect.height - 1; }}

    // public int startLeftBounds { private set{} get { return startingCoords.col; }}
    // public int startRightBounds { private set{} get { return startingCoords.col + (int) rect.width - 1; }}

    public int upperBounds { private set{} get { return (int) rect.y; }}
    public int lowerBounds { private set{} get { return (int) rect.y + (int) rect.height - 1; }}

    public int leftBounds { private set{} get { return (int) rect.x; }}
    public int rightBounds { private set{} get { return (int) rect.x + (int) rect.width - 1; }}

    public int currentRow { private set{} get { return (int) rect.y; }}
    public int currentCol { private set{} get { return (int) rect.x; }}

	public Selector(
		string name,
        int width,
        int height,
		GridCoords startingCoords,
		bool[,] spaces,
        Color originalColor = default)
	{
		this.name = name;

		this.startingCoords = startingCoords;

        this.rect = new Rect(x: startingCoords.col, y: startingCoords.row, width: width, height: height);

		this.spaces = spaces;

        if(!originalColor.Equals(default))
        {
            this.originalColor = originalColor;
        }
	}

    public bool singleTile()
    {
        return rect.width == 1 && rect.height == 1;
    }

	public GridCoords getCoords()
	{
		return new GridCoords(rect.y, rect.x);
	}
	
	public GridCoords[] getAllSelectorCoords(bool includeIllegalCoords = false)
	{
		List<GridCoords> allSelectorCoords = new List<GridCoords>();
		
		for(int row = 0; row < spaces.GetLength(0); row++)
		{
            for(int col = 0; col < spaces.GetLength(1); col++)
            {
                if(spaces[row,col])
                {
                    GridCoords currentGridCoord = new GridCoords(rect.y + row, rect.x + col);
                
                    if(includeIllegalCoords || 
                        !((getCoords().isWithinEnemySection() && !currentGridCoord.isWithinEnemySection()) || 
                        (getCoords().isWithinAllySection() && !currentGridCoord.isWithinAllySection())))
                    {
                        allSelectorCoords.Add(currentGridCoord);
                    }
                }
            }
		}

		return allSelectorCoords.ToArray();
	}
	
	public override bool Equals(object obj)
	{
		Selector other = (Selector) obj;
		
		if(other.getAllSelectorCoords().Equals(getAllSelectorCoords()))
		{
			return true;
		}

		return false;
	}
	
	public override int GetHashCode()
	{
		return name.GetHashCode();
	}

	public virtual bool wasGenerated()
	{
		return false;
	}
	
	public bool allTilesAreLegal()
	{
        return rect.y >= CombatGrid.rowUpperBounds && rect.y + rect.height-1 <= CombatGrid.allyRowLowerBounds && 
                rect.x >= CombatGrid.colLeftBounds && rect.x + rect.width-1 <= CombatGrid.colRightBounds && !crossesBattlefieldDivide();
	}
	
	private bool crossesBattlefieldDivide()
	{
		if(singleTile() || rect.height == 1)
		{
			return false;
		}
		
        return CombatGrid.positionIsOnEnemySide(getCoords()) && rect.y + (rect.height-1) > CombatGrid.enemyRowLowerBounds;
	}
	
	public bool containsTarget(Stats target)
	{
		return target.positions.Any(p => containsTarget(p));
	}
	
	public bool containsTarget(GridCoords target)
	{
		GridCoords[] allSelectorCoords = getAllSelectorCoords();

		foreach(GridCoords coord in allSelectorCoords)
		{
			if(target.Equals(coord))	
			{
				return true;
			}
		}
		
		return false;
	}
	
	public void setToStartLocation()
	{
        setToLocation(startingCoords);
	}

	public void setToClosestLegalLocation(GridCoords coords)
	{
		setToLocation(SelectorManager.findLegalCoordsContainingMandatoryTarget(this, coords));
	}

	public void setToLocation(GridCoords coords)
	{
        rect.x = coords.col;
        rect.y = coords.row;

        SelectorManager.declareSelectors();
	}
	
	public bool onEnemySide()
	{
		return getCoords().isWithinEnemySection();
	}
	
	public bool onAllySide()
	{
		return getCoords().isWithinAllySection();
	}
	
	public bool hasAtLeastOneTarget(string[] tagCriteria)
	{
		List<Stats> allTargets = getAllTargets();

        List<Stats> targetsQueuedToMove = new List<Stats>();

		foreach (Stats stats in allTargets)
		{
            if(stats == null)
            {
                continue;
            }

			GameObject combatSprite = stats.combatSprite;

			if (Helpers.tagMatchesCriteria(combatSprite, tagCriteria) && !stats.queuedToMove())
			{
				return true;
			} else if(stats.queuedToMove())
            {
                targetsQueuedToMove.Add(stats);
            }
		}

        foreach(Stats target in targetsQueuedToMove)
        {
            CombatantHover hover = target.repositionClone.combatSprite.GetComponent<CombatantHover>();

            hover.createOutlineAndStartFade();
        }

        return false;
	}
	
	public bool hasAtLeastOneLivingTarget(string[] tagCriteria)
	{
		List<Stats> allTargets = getAllTargets();
        List<Stats> targetsQueuedToMove = new List<Stats>();

		foreach (Stats stats in allTargets)
		{
			if (stats.isDead())
			{
				continue;
			}

			GameObject combatSprite = stats.combatSprite;

			if (Helpers.tagMatchesCriteria(combatSprite, tagCriteria) && !stats.queuedToMove())
			{
				return true;
			}else if(stats.queuedToMove())
            {
                targetsQueuedToMove.Add(stats);
            }
		}

        foreach(Stats target in targetsQueuedToMove)
        {
            CombatantHover hover = target.repositionClone.combatSprite.GetComponent<CombatantHover>();

            hover.createOutlineAndStartFade();
        }

		return false;
	}
	
	public bool hasAtLeastOneMandatoryTarget()
	{
		GridCoords[] targetTileCoords = getAllSelectorCoords();

		foreach (GridCoords targetTileCoord in targetTileCoords)
		{
			Stats targetCombatant = CombatGrid.getCombatantAtCoords(targetTileCoord);

			if (targetCombatant != null && Helpers.hasQuality<Trait>(targetCombatant.traitContainer, t => t.isMandatoryTarget())
                    && !targetCombatant.queuedToMove())
			{
				return true;
			}
		}

		return false;
	}
	
	public GridCoords getFirstCombatantCoords()
	{
		GridCoords[] allSelectorCoords = getAllSelectorCoords();

		foreach (GridCoords coord in allSelectorCoords)
		{
			if (CombatGrid.getCombatantAtCoords(coord) != null)
			{
				return coord;
			}
		}

		return getCoords();
	}

	public List<Stats> getAllTargets()
	{
        GridCoords[] targetTileCoords = getAllSelectorCoords();
        List<Stats> allActionTargets = new List<Stats>();

        foreach (GridCoords targetTileCoord in targetTileCoords)
        {
			if(CombatGrid.getCombatantAtCoords(targetTileCoord) != null)
			{
                allActionTargets.Add(CombatGrid.getCombatantAtCoords(targetTileCoord));
            }
        }

		return allActionTargets;
    }

    public List<Stats> getAllPreviewTargetClones()
    {
        List<Stats> allActionTargets = getAllTargets();
		List<Stats> cloneTargets = new List<Stats>();

        foreach (Stats target in allActionTargets)
        {
			cloneTargets.Add(target.getPreviewClone());
        }

        return cloneTargets;
    }

    public int countHealthBarOccurances(HealthBarManager healthBar)
    {
        List<Stats> allActionTargets = getAllTargets();
        int healthBarOccurances = 0;

        foreach (Stats target in allActionTargets)
        {
			if(target.healthBarManager == healthBar)
            {
                healthBarOccurances++;
            }
        }

        return healthBarOccurances;
    }

    public void setToCurrentSelector()
	{
		SelectorManager.currentSelector = this;
        SelectorManager.declareSelectors();
	}
	
	public void setToSecondaryColor()
	{
        setToColor(secondaryColor);
	}
 	
	public bool setToOriginalColor()
	{
		if(originalColor.Equals(Color.clear))
		{
			return false;
		}

        setToColor(originalColor);

		return true;
	}

    private void setToColor(Color newColor)
    {
		this.setTilesToColor(newColor);
    }

	public bool targetsImmobileTarget()
	{
		GridCoords[] allSelectorCoords = getAllSelectorCoords();
		
		foreach(GridCoords coords in allSelectorCoords)
		{
			Stats target = CombatGrid.getCombatantAtCoords(coords);
			
			if(target == null)
			{
				return false;
			}

			if(Helpers.hasQuality<Trait>(target.traitContainer, t => t.isImmobile())) 
			{
				return true;
			}
		}
		
		return false;
	}

	public Selector clone()
	{
		Selector selectorClone = (Selector) Clone();

        selectorClone.startingCoords = startingCoords.clone();

		return selectorClone;
	}
	
	public object Clone()
    {
        return this.MemberwiseClone();
    }
} 
