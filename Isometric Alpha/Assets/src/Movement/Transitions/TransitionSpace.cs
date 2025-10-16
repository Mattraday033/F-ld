using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Transition
{
    public int index;
    public string currentAreaName;
    public string destinationAreaName;

    public Facing playerSpawnDirection;

    public Vector3Int cellCoords;

    public int outputMultiplier;
    public bool usableForFastTravel;

    //used in fast travelling
    public Transition(string currentAreaName, string destinationAreaName)
    {
        this.currentAreaName = currentAreaName;
        this.destinationAreaName = destinationAreaName;
        this.usableForFastTravel = true;

        this.cellCoords = PlayerMovement.getMovementGridCoords();

        this.index = 0;
        this.playerSpawnDirection = CharacterFacing.getOpposingFacing(State.playerFacing.getFacing());
        this.outputMultiplier = 1;
    }

    public Transition(string currentAreaName, string destinationAreaName, Vector3Int cellCoords, int index, Facing playerSpawnDirection, bool usableForFastTravel, int outputMultiplier)
    {
        this.currentAreaName = currentAreaName;
        this.destinationAreaName = destinationAreaName;

        this.cellCoords = cellCoords;
        this.index = index;

        this.playerSpawnDirection = playerSpawnDirection;

        this.usableForFastTravel = usableForFastTravel;

        this.outputMultiplier = outputMultiplier;
    }


    public bool sharesHash(Transition transition)
    {
        return currentAreaName.Equals(transition.destinationAreaName) &&
            destinationAreaName.Equals(transition.currentAreaName) &&
            transition.index == index;
    }

    public virtual bool fastTravelCapable()
    {
        return usableForFastTravel;
    }

    public Vector3Int getOutPutCellCoords()
    {
        switch (playerSpawnDirection)
        {
            case Facing.NorthEast:
                return cellCoords + MovementManager.distance1TileNorthEastGrid * outputMultiplier;
            case Facing.NorthWest:
                return cellCoords + MovementManager.distance1TileNorthWestGrid * outputMultiplier;
            case Facing.SouthEast:
                return cellCoords + MovementManager.distance1TileSouthEastGrid * outputMultiplier;
            default:
                return cellCoords + MovementManager.distance1TileSouthWestGrid * outputMultiplier;
        }
    }

}

public class TransitionSpace : MonoBehaviour, ICounter
{

    [SerializeField]
    private Transition transition;
    public Collider2D collider;

    public Transition getTransition()
    {
        return transition;
    }

    public void setTransition(Transition transition)
    {
        this.transition = transition;

        if (transition.fastTravelCapable())
        {
            collider.enabled = false;
        }
    }


    private void OnEnable()
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

    public void updateCounter()
    {
        TransitionManager.addTransition(transition);
    }

    public List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        listOfEvents.Add(TransitionManager.OnTransitionUpdate);

        return listOfEvents;
    }

}