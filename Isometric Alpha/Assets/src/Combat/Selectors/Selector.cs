using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Selector : ScriptableObject, ICloneable
{
	private const bool shouldIncludeIllegalCoords = true;
	
	public string name;
	
    public static Color secondaryColor = Color.yellow;
	private Color originalColor = Color.clear;
	
	public bool selfTargeting = false;
	
	public bool singleTile; //true if you don't want to look for anything beyond the initial tile
	
	public int startUpperBounds;	
	public int startRightBounds;
	public int startLeftBounds;
	public int startLowerBounds;
	
	public int upperBounds;	
	public int rightBounds;
	public int leftBounds;
	public int lowerBounds;
	
	public int startRow;
	public int startCol;
	
	public int currentRow;
	public int currentCol;
	
	public GridCoords[] childTileAdjustments; //should be empty if singleTile is true
	
	private GameObject selectorObject;
	private CapsuleCollider2D collider;
	
	void Start()
	{
		selectorObject = Resources.Load<GameObject>(name);
		collider = selectorObject.GetComponent<CapsuleCollider2D>();
	}
	
	public virtual GameObject getSelectorObject()
	{
		if(selectorObject == null)
		{
			selectorObject = Instantiate(Resources.Load<GameObject>(name), CombatGrid.fullCombatGrid[startRow][startCol], Quaternion.identity);
		}
		
		return selectorObject;
	}
	
	//usually only useful if a single target selector
	public GridCoords getCoords()
	{
		return new GridCoords(currentRow, currentCol);
	}
	
	public virtual GridCoords[] getChildTileAdjustments()
	{
		return childTileAdjustments;
	}
	
	public GridCoords[] getAllSelectorCoords()
	{
		return getAllSelectorCoords(false);
	}
	
	public GridCoords[] getAllSelectorCoords(bool includeIllegalCoords)
	{
		GridCoords[] allSelectorCoords = new GridCoords[0];
		ArrayList allTileAdjustments = new ArrayList();
		allTileAdjustments.Add(new GridCoords(0, 0)); //parent Selector Coord
		
		if(getChildTileAdjustments() != null)
		{
			allTileAdjustments.AddRange(getChildTileAdjustments());
		}
		
		foreach(GridCoords adjustment in allTileAdjustments)
		{
            GridCoords currentGridCoord = new GridCoords(currentRow + adjustment.row, currentCol + adjustment.col);
			
			if((getCoords().isWithinEnemySection() && !currentGridCoord.isWithinEnemySection()) || 
				(getCoords().isWithinAllySection() && !currentGridCoord.isWithinAllySection()))
			{
				if(includeIllegalCoords)
				{
					allSelectorCoords = Helpers.appendArray<GridCoords>(allSelectorCoords, currentGridCoord);
				} 
			} else
			{
				allSelectorCoords = Helpers.appendArray<GridCoords>(allSelectorCoords, currentGridCoord);
			}
		}

		return allSelectorCoords;
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
	
	public virtual bool wasGenerated()
	{
		return false;
	}
	
	public bool allTilesAreLegal()
	{
		GridCoords[] allSelectorCoords = getAllSelectorCoords(shouldIncludeIllegalCoords);
	
		foreach(GridCoords selectorCoords in allSelectorCoords)
		{
			if(selectorCoords.row > CombatGrid.rowLowerBounds || selectorCoords.col < CombatGrid.colLeftBounds ||
				selectorCoords.row < CombatGrid.rowUpperBounds || selectorCoords.col > CombatGrid.colRightBounds || crossesBattlefieldDivide())
			{	
				return false;
			}
		}
		
		return true;
	}
	
	private bool crossesBattlefieldDivide()
	{
		if(singleTile)
		{
			return false;
		}
		
		if((currentRow <= CombatGrid.enemyRowLowerBounds && lowerBounds >= CombatGrid.allyRowUpperBounds) ||
		   (currentRow >= CombatGrid.allyRowUpperBounds && upperBounds <= CombatGrid.enemyRowLowerBounds))
		{
			return true;
		} else
		{
			return false;
		}
	}
	
	public bool containsTarget(Stats target)
	{		
		return containsTarget(target.position);
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
	
	public CapsuleCollider2D getCollider()
	{
		if(selectorObject == null)
		{ //Instantiate(CombatGrid.getCombatantAtCoords(x,y).combatSprite, CombatGrid.fullCombatGrid[x][y] + CombatGrid.getCombatantAtCoords(x,y).adjustment, Quaternion.identity);
			selectorObject = Instantiate(Resources.Load<GameObject>(name), CombatGrid.fullCombatGrid[startRow][startCol],Quaternion.identity);
			collider = selectorObject.GetComponent<CapsuleCollider2D>();
		} else if(collider == null)
		{
			collider = selectorObject.GetComponent<CapsuleCollider2D>();
		} 
		
		return collider;
	}
	
	public ArrayList getAllTileColliders()
	{
		ArrayList allTileColliders = new ArrayList();
		
		allTileColliders.Add(getCollider());
		
		for(int childIndex = 0; childIndex < getSelectorObject().transform.childCount; childIndex++)
		{
			allTileColliders.Add(getSelectorObject().transform.GetChild(childIndex).GetComponent<Collider2D>());
		}
		
		return allTileColliders;
	}

    public ArrayList getAllTileChildren()
    {
        ArrayList allTileChildren = new ArrayList();

        for (int childIndex = 0; childIndex < getSelectorObject().transform.childCount; childIndex++)
        {
            allTileChildren.Add(getSelectorObject().transform.GetChild(childIndex).gameObject);
        }

        return allTileChildren;
    }

	public void setToStartLocation()
	{
		currentRow = startRow;
		currentCol = startCol;

		if (!singleTile)
		{
			upperBounds = startUpperBounds;
			rightBounds = startRightBounds;
			leftBounds = startLeftBounds;
			lowerBounds = startLowerBounds;
		}
		else
		{
			upperBounds = startRow;
			rightBounds = startCol;
			leftBounds = startCol;
			lowerBounds = startRow;
		}

		getSelectorObject().transform.position = CombatGrid.fullCombatGrid[currentRow][currentCol];
	}

	public void setToClosestLegalLocation(GridCoords coords)
	{
		setToLocation(SelectorManager.findLegalCoordsContainingMandatoryTarget(this, coords));
	}

	public void setToLocation(GridCoords coords)
	{
		setToLocation(coords, true);
	}
	
	public void setToLocation(GridCoords coords, bool moveGameObject)
	{
        int rowDifference = currentRow - coords.row;
		int colDifference = currentCol - coords.col;
		
		currentRow -= rowDifference;
		currentCol -= colDifference;
	
		if(!singleTile)
		{
			upperBounds -= rowDifference;
			lowerBounds -= rowDifference;
			
			rightBounds -= colDifference;
			leftBounds -= colDifference;
			
		} else
		{
			upperBounds = currentRow;	
			rightBounds = currentCol;
			leftBounds = currentCol;
			lowerBounds = currentRow;
		}

		if (moveGameObject)
		{
			getSelectorObject().transform.position = CombatGrid.fullCombatGrid[currentRow][currentCol];

			if (this == SelectorManager.getCurrentSelector())
			{
				Helpers.updateColliderPosition(getSelectorObject());
			}
		}
	}
	
	public bool onEnemySide()
	{
		return getCoords().isWithinEnemySection();
	}
	
	public bool onAllySide()
	{
		return getCoords().isWithinAllySection();
	}
	
	//always returns lowercase for easier comparisons
	public string getTag()
	{
		return selectorObject.tag.ToLower();
	}
	
	public void SetActive(bool active)
	{
		if(selectorObject == null)
		{
			selectorObject = Instantiate(Resources.Load<GameObject>(name), CombatGrid.fullCombatGrid[startRow][startCol],Quaternion.identity);
			collider = selectorObject.GetComponent<CapsuleCollider2D>();
		} 
		
		selectorObject.SetActive(active);
	}
	
	public bool hasAtLeastOneTarget(string[] tagCriteria)
	{
		ArrayList allTargets = getAllTargets();

		foreach (Stats stats in allTargets)
		{
			GameObject combatSprite = stats.combatSprite;

			if (Helpers.tagMatchesCriteria(combatSprite, tagCriteria))
			{
				return true;
			}
		}

        return false;
	}
	
	public bool hasAtLeastOneLivingTarget(string[] tagCriteria)
	{
		ArrayList allTargets = getAllTargets();

		foreach (Stats stats in allTargets)
		{
			if (stats.isDead)
			{
				continue;
			}

			GameObject combatSprite = stats.combatSprite;

			foreach (string tag in tagCriteria)
			{
				if (combatSprite.gameObject.tag.Equals(tag))
				{
					return true;
				}
			}
		}

		return false;
	}
	
	public bool hasAtLeastOneMandatoryTarget()
	{
		GridCoords[] targetTileCoords = getAllSelectorCoords();

		foreach (GridCoords targetTileCoord in targetTileCoords)
		{
			Stats targetCombatant = CombatGrid.getCombatantAtCoords(targetTileCoord);

			if (targetCombatant != null && Helpers.hasQuality<Trait>(targetCombatant.traits, t => t.isMandatoryTarget()))
			{
				return true;
			}
		}

		return false;
	}
	
	public ArrayList getAllTargets()
	{
        GridCoords[] targetTileCoords = getAllSelectorCoords();
        ArrayList allActionTargets = new ArrayList();

        foreach (GridCoords targetTileCoord in targetTileCoords)
        {
			if(CombatGrid.getCombatantAtCoords(targetTileCoord) != null)
			{
                allActionTargets.Add(CombatGrid.getCombatantAtCoords(targetTileCoord));
            }
        }

		return allActionTargets;
    }

    public ArrayList getAllTargetClones()
    {
        ArrayList allActionTargets = getAllTargets();
		ArrayList cloneTargets = new ArrayList();

        foreach (Stats target in allActionTargets)
        {
			cloneTargets.Add(target.clone());
        }

        return cloneTargets;
    }

    public void setToCurrentSelector()
	{
		SetActive(true);
		SelectorManager.currentSelector = this;
	}
	
	public void moveBoundsUp() //moves all bounds up
	{
		upperBounds--;
		lowerBounds--;
	}
	
	public void moveBoundsRight() //moves all bounds right
	{
		leftBounds++;
		rightBounds++;
	}
	
	public void moveBoundsLeft() //moves all bounds left
	{
		leftBounds--;
		rightBounds--;
	}
	
	public void moveBoundsDown() //moves all bounds down
	{
		upperBounds++;
		lowerBounds++;
	}
	
	//currently only sets first/parent tile
	public void setToSecondaryColor()
	{
        SpriteRenderer selectorSprite = selectorObject.GetComponent<SpriteRenderer>();
        
		if (!selectorSprite.color.Equals(secondaryColor))
		{
			originalColor = selectorSprite.color; 
		}

        setToColor(secondaryColor, selectorSprite);
	}
 	
	//currently only sets first/parent tile
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
		setToColor(newColor, null);
    }

    private void setToColor(Color newColor, SpriteRenderer selectorSprite)
	{
		if(selectorSprite == null)
		{
            selectorSprite = selectorObject.GetComponent<SpriteRenderer>();
        }
        
        selectorSprite.color = newColor;

		ArrayList listOfTileChildren = getAllTileChildren();


        foreach (GameObject gObject in listOfTileChildren)
		{
            selectorSprite = gObject.GetComponent<SpriteRenderer>();
            selectorSprite.color = newColor;
        }
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

			if(Helpers.hasQuality<Trait>(target.traits, t => t.isImmobile())) 
			{
				return true;
			}
		}
		
		return false;
	}
	public void deselectSelectorGameObject()
	{
		selectorObject = null;
	}

	public Selector clone()
	{
		Selector selectorClone = (Selector)Clone();

		selectorClone.childTileAdjustments = new GridCoords[childTileAdjustments.Length];

		for(int index = 0; index < childTileAdjustments.Length; index++)
		{
			selectorClone.childTileAdjustments[index] = childTileAdjustments[index].clone();
		}

		return selectorClone;
	}
	
	public object Clone()
    {
        return this.MemberwiseClone();
    }
} 
