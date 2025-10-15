using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Transition
{
    public int index;
    public string currentAreaName;
    public string destinationAreaName;

    public Facing playerSpawnDirection;

    public Vector3Int cellCoords;

    public int outputMultiplier;

    public Transition(string currentAreaName, string destinationAreaName, Vector3Int cellCoords, int index, Facing playerSpawnDirection)
    {
        this.currentAreaName = currentAreaName;
        this.destinationAreaName = destinationAreaName;
        this.cellCoords = cellCoords;
        this.index = index;
        this.playerSpawnDirection = playerSpawnDirection;
        
        this.outputMultiplier = 1;
    }

    public Transition(string currentAreaName, string destinationAreaName, Vector3Int cellCoords, int index, Facing playerSpawnDirection, int outputMultiplier)
    {
        this.currentAreaName = currentAreaName;
        this.destinationAreaName = destinationAreaName;
        this.cellCoords = cellCoords;
        this.index = index;
        this.playerSpawnDirection = playerSpawnDirection;
        this.outputMultiplier = outputMultiplier;
    }


    public bool sharesHash(Transition transition)
    {
        return currentAreaName.Equals(transition.destinationAreaName) &&
            destinationAreaName.Equals(transition.currentAreaName) &&
            transition.index == index;
    }

    public Vector3Int getOutPutCellCoords()
    {
        switch(playerSpawnDirection)
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

    public Transition transition;


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