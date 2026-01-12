using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FormationDisplayUI : MonoBehaviour, ICounter
{
	public PartyPositionGridRow[] formationUIGrid;
	
    #region ICounter

    private void Awake()
    {
        addListeners();
    }

    private void OnDestroy()
    {
        removeListeners();
    }

    public void addListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.AddListener(updateCounter);
        }
    }
    public void removeListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach(UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.RemoveListener(updateCounter);
        }
    }

    public virtual void updateCounter()
    {
        populate(State.formation);
    }

    public List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        if(OverallUIManager.currentScreenManager != null)
        {
            listOfEvents.AddRange(OverallUIManager.currentScreenManager.getUpdateEvents());
        }

        return listOfEvents;
    }

    #endregion


	public void setColorOfGridSquare(GridCoords coords, Color color)
	{
		formationUIGrid[coords.row].cells[coords.col].image.color = color;
	}
	
	public void setToReadOnly()
	{
		for(int rowIndex = 0; rowIndex < formationUIGrid.Length; rowIndex++)
		{
			for(int colIndex = 0; colIndex < formationUIGrid[rowIndex].cells.Length; colIndex++)
			{
				formationUIGrid[rowIndex].cells[colIndex].button.enabled = false;
			}
		}
	}

	public void setEmptySquaresToInteractable(Formation formation)
	{
		setEmptySquaresInterability(formation, true);
	}
	
	public void setEmptySquaresToUninteractable(Formation formation)
	{
		setEmptySquaresInterability(formation, false);
	}

	private void setEmptySquaresInterability(Formation formation, bool interactable)
	{
		for (int rowIndex = 0; rowIndex < formationUIGrid.Length; rowIndex++)
		{
			for (int colIndex = 0; colIndex < formationUIGrid[rowIndex].cells.Length; colIndex++)
			{
				if (formation.getGrid()[rowIndex][colIndex] == null)
				{
					formationUIGrid[rowIndex].cells[colIndex].button.interactable = interactable;
				}
			}
		}
	}
	
	public void populate(Formation formation)
	{
		for (int rowIndex = 0; rowIndex < formation.getGrid().Length; rowIndex++)
		{
			for (int colIndex = 0; colIndex < formation.getGrid()[rowIndex].Length; colIndex++)
			{
				PartyPositionGridSquare gridSquare = getGridSquareAtPosition(rowIndex, colIndex);

				if (formation.getGrid()[rowIndex][colIndex] != null)
				{
					gridSquare.populate(formation.getGrid()[rowIndex][colIndex]);
				}
				else
				{
					gridSquare.populate();
				}

				gridSquare.determineButtonEnabled();
			}
		}
	}
	
	public PartyPositionGridSquare getGridSquareAtPosition(int row, int col)
	{
		return formationUIGrid[row].cells[col];
	}
	
}
