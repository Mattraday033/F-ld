using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPartyEditor
{
	public void updateSlotTracker();
	public void populateFormationGrid(); //populates Formation UI

	public void readInCurrentFormation(); //Reads Formation info from State.formation, incase an interim formation is used
	public void writeFormation();   //Writes from interirm formation to State.formation

	public void addCharacterToFormation(AllyStats characterToAdd, int row, int col);
	public void removeCharacter(AllyStats characterToRemove);

	public AllyStats getSelectedPartyMember();

	public Formation getFormation();
}

public class PartyMemberDragAndDrop : DragAndDropUIObject, IDragAndDropContainer
{
    private static PartyMemberDragAndDrop instance;

    public IPartyEditor partyEditor;

    public static PartyMemberDragAndDrop getInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instance of PartyMemberDragAndDrop exist erroneously");
        }

        instance = this;
        partyEditor = (IPartyEditor)OverallUIManager.currentScreenManager;
    }

    public override void checkForTargetObject()
    {
        GridCoords droppedGridCoords = determineDroppedGridCoords();

        if (!droppedGridCoords.Equals(GridCoords.getDefaultCoords()))
        {
            partyEditor.addCharacterToFormation(Stats.convertIDescribableToStats(objectBeingDragged), droppedGridCoords.row, droppedGridCoords.col);
        }
    }

    private GridCoords determineDroppedGridCoords()
    {
        Dictionary<GridCoords, int> hitsPerCoord = new Dictionary<GridCoords, int>();

        foreach (Collider2D collider in colliders)
        {
            List<GridCoords> hitCoords = getGridCoordsOfHits(collider);

            foreach (GridCoords hitCoord in hitCoords)
            {
                incrementGridCoordsHit(hitsPerCoord, hitCoord);
            }
        }

        if (hitsPerCoord.Count <= 0)
        {
            return GridCoords.getDefaultCoords();
        }
        else
        {
            return getCoordsWithMostHits(hitsPerCoord);
        }
    }

    private static GridCoords getCoordsWithMostHits(Dictionary<GridCoords, int> hitsPerCoord)
    {
        GridCoords mostHitCoord = GridCoords.getDefaultCoords();

        foreach (KeyValuePair<GridCoords, int> kvp in hitsPerCoord)
        {
            if (mostHitCoord.Equals(GridCoords.getDefaultCoords()))
            {
                mostHitCoord = kvp.Key.clone();
            }
            else if (kvp.Value > hitsPerCoord[mostHitCoord])
            {
                mostHitCoord = kvp.Key;
            }
        }

        return mostHitCoord;
    }

    private static List<GridCoords> getGridCoordsOfHits(Collider2D collider)
    {
        List<GridCoords> hitCoords = new List<GridCoords>();

        Collider2D[] collisions = Helpers.getCollisions(collider);

        foreach (Collider2D collision in collisions)
        {
            if (collision != null)
            {
                PartyPositionGridSquare gridSquare = collision.GetComponent<PartyPositionGridSquare>();

                if (gridSquare != null)
                {
                    hitCoords.Add(gridSquare.getCoords()); ;
                }
            }
        }

        return hitCoords;
    }

    private static void incrementGridCoordsHit(Dictionary<GridCoords, int> hitDictionary, GridCoords gridCoords)
    {
        if (hitDictionary.ContainsKey(gridCoords))
        {
            hitDictionary[gridCoords]++;
        }
        else
        {
            hitDictionary.Add(gridCoords, 0);
        }
    }

}
