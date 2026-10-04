using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FormationDisplayUI : MonoBehaviour, ICounter
{
    public readonly static UnityEvent OnFormationDisplayUICreation = new UnityEvent();

	public PartyPositionGridRow[] formationUIGrid;
	
    #region ICounter

    //the screen this display sits in, if it sits in one. The ones built into pop-ups and description rows have none
    private ScreenManager owningScreen;

    //what addListeners subscribed to, kept so removeListeners takes off the same ones whichever screen is current by then
    private List<UnityEvent> subscribedEvents = new List<UnityEvent>();

    private void Awake()
    {
        owningScreen = GetComponentInParent<ScreenManager>(true);

        addListeners();
        OnFormationDisplayUICreation.Invoke();

        GridRow.OnDescribableToDisplay.AddListener(destroy);
        OnFormationDisplayUICreation.AddListener(destroy);
    }

    private void OnDestroy()
    {
        removeListeners();
        GridRow.OnDescribableToDisplay.RemoveListener(destroy);
        OnFormationDisplayUICreation.RemoveListener(destroy);
    }

    private void destroy()
    {
        //a row clicked or another display made while this one's screen is hidden has nothing to do with it
        if (ScreenManager.isHidden(owningScreen))
        {
            return;
        }

        Destroy(gameObject);
    }

    private void destroy(IDescribable describable)
    {
        destroy();
    }

    public void addListeners()
    {
        subscribedEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in subscribedEvents)
        {
            unityEvent.AddListener(updateCounter);
        }
    }
    public void removeListeners()
    {
        foreach(UnityEvent unityEvent in subscribedEvents)
        {
            unityEvent.RemoveListener(updateCounter);
        }
    }

    public virtual void updateCounter()
    {
        if (ScreenManager.isHidden(owningScreen))
        {
            return;
        }

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
