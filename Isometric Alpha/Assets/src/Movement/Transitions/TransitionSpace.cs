using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Transition
{
    public const bool ladderTransition = false;
    public bool destinationOnly;

    public int index;
    public string currentAreaName;
    public string destinationName;

    public Facing playerSpawnDirection;

    public Vector3Int cellCoords;

    public int outputMultiplier;
    public bool usableForFastTravel;
    public PlayerInteractionScript scriptOnTransition; 

    public string whichConstructor = "";

    //used in fast traveling
    public Transition(string currentAreaName, string destinationName, bool usableForFastTravel = true)
    {
        this.currentAreaName = currentAreaName;
        this.destinationName = destinationName;
        this.usableForFastTravel = usableForFastTravel;

        this.cellCoords = PlayerMovement.getMovementGridCoords();

        this.index = 0;
        this.playerSpawnDirection = CharacterFacing.getOpposingFacing(State.playerFacing.getFacing());
        this.outputMultiplier = 0; //to prevent autosave from moving player

        whichConstructor = "first";
    }

    public Transition(string currentAreaName, string destinationName, Vector3Int cellCoords, int index, Facing playerSpawnDirection, bool usableForFastTravel, int outputMultiplier, PlayerInteractionScript scriptOnTransition, bool destinationOnly = false)
    {
        this.currentAreaName = currentAreaName;
        this.destinationName = destinationName;

        this.cellCoords = cellCoords;
        this.index = index;

        this.playerSpawnDirection = playerSpawnDirection;

        this.usableForFastTravel = usableForFastTravel;

        this.outputMultiplier = outputMultiplier;
        
        this.scriptOnTransition = scriptOnTransition;
        this.destinationOnly = destinationOnly;

        whichConstructor = "second";
    }

    public bool sharesHash(Transition transition)
    {
        return currentAreaName.Equals(transition.destinationName) &&
            destinationName.Equals(transition.currentAreaName) &&
            transition.index == index;
    }

    public void playScript()
    {
        if(scriptOnTransition == null)
        {
            return;
        }

        scriptOnTransition.runScript();
    }

    public virtual bool fastTravelCapable()
    {
        return usableForFastTravel;
    }

    public virtual Vector3Int getPositionOnSaveMultiplier()
    {
        return getOutPutCellCoords();
    }

    public virtual Vector3Int getOutPutCellCoords()
    {
        return multiplyCellCoords(outputMultiplier);
    }

    protected Vector3Int multiplyCellCoords(int multiplier)
    {
        switch (playerSpawnDirection)
        {
            case Facing.NorthEast:
                return cellCoords + MovementManager.distance1TileNorthEastGrid * multiplier;
            case Facing.NorthWest:
                return cellCoords + MovementManager.distance1TileNorthWestGrid * multiplier;
            case Facing.SouthEast:
                return cellCoords + MovementManager.distance1TileSouthEastGrid * multiplier;
            default:
                return cellCoords + MovementManager.distance1TileSouthWestGrid * multiplier;
        }
    }

}

public class LadderTransition : Transition
{
    private const bool notFastTravelCapable = false;

    public LadderTransition(string locationName, string destinationName, Vector3Int cellCoords, Facing playerSpawnDirection):
    base(locationName, destinationName, cellCoords, Constants.indexZero, playerSpawnDirection, notFastTravelCapable, Constants.sizeOne, null)
    {
        
    }

    public override Vector3Int getPositionOnSaveMultiplier()
    {
        return multiplyCellCoords(Constants.sizeZero);
    }
}

public class TransitionSpace : MonoBehaviour, ICounter
{

    public string currentAreaName;
    public string destinationName;

    [SerializeField]
    public Transition transition;
    public Collider2D collider;

    public Transition getTransition()
    {
        return transition;
    }

    public void setTransition(Transition transition)
    {
        this.transition = transition;

        currentAreaName = transition.currentAreaName;
        destinationName = transition.destinationName;

        if (transition.fastTravelCapable() || transition.destinationOnly)
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

        listOfEvents.Add(TransitionManager.CollectTransitionSpaces);

        return listOfEvents;
    }

}