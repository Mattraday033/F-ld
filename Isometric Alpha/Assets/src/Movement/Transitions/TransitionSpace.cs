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

    public bool allowAutosave = false;

    public string indicatorFlag;

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

    public Transition(string currentAreaName, string destinationName, Vector3Int cellCoords, int index, Facing playerSpawnDirection, bool usableForFastTravel, int outputMultiplier, PlayerInteractionScript scriptOnTransition, bool destinationOnly = false, bool allowAutosave = true, string indicatorFlag = "")
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
        this.allowAutosave = allowAutosave;

        this.indicatorFlag = indicatorFlag;

        whichConstructor = "second";
    }

    public virtual bool preventIndicator()
    {
        return false;
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

    public override bool preventIndicator()
    {
        return true;
    }
}

public class TransitionSpace : MonoBehaviour, ICounter
{

    public string currentAreaName;
    public string destinationName;

    public Transition m_Transition;

    [SerializeField]
    public Transition transition
    {
        get { return m_Transition; }
        set
        {
            m_Transition = value;
            
            removeListeners();
            addListeners();
        }
    }
    public Collider2D collider;

    private GameObject indicator;
    private SpriteRenderer indicatorSpriteRenderer;

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

    private bool shouldShowIndicator()
    {
        return transition != null && !transition.usableForFastTravel && !transition.preventIndicator();
    }

    private void createIndicator()
    {
        indicator = Instantiate(Resources.Load<GameObject>(PrefabNames.effect), transform);

        indicator.transform.localPosition = new Vector3(0f, -.2f);
        indicator.transform.localScale = new Vector3(.4f, .4f, .4f);

        indicatorSpriteRenderer = indicator.GetComponent<SpriteRenderer>();
        indicatorSpriteRenderer.sortingLayerName = LayerAndTagManager.sixthSortingLayerName;

        if(transition.indicatorFlag != null && transition.indicatorFlag.Length > 0)
        {
            indicatorSpriteRenderer.enabled = SecretDoorFlags.secretDoorHasBeenDiscovered(transition.indicatorFlag);
        }

        EffectAnimationManager effect = indicator.GetComponent<EffectAnimationManager>();
        effect.loops = true;
        effect.setAnimations(EffectAnimationType.TransitionIndicator);
    }

    private void updateIndicatorVisibility(bool revealed)
    {
        if(revealed && indicator == null)
        {
            createIndicator();
        } else if(!revealed && indicator != null)
        {
            indicatorSpriteRenderer = null;
            Destroy(indicator);
        }
    }

    private void updateIndicatorVisibility(string secretDoorKey)
    {
        if(indicatorSpriteRenderer != null && 
            transition.indicatorFlag != null && 
            transition.indicatorFlag.Equals(secretDoorKey))

        {
            indicatorSpriteRenderer.enabled = SecretDoorFlags.secretDoorHasBeenDiscovered(transition.indicatorFlag);
        }
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

        if(shouldShowIndicator())
        {
            RevealManager.OnReveal.AddListener(updateIndicatorVisibility);
            SecretDoorFlags.OnSecretDoorDiscovery.AddListener(updateIndicatorVisibility);

            updateIndicatorVisibility(RevealManager.currentlyRevealed);
        }
    }
    public void removeListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach(UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.RemoveListener(updateCounter);
        }

        RevealManager.OnReveal.RemoveListener(updateIndicatorVisibility);
        SecretDoorFlags.OnSecretDoorDiscovery.RemoveListener(updateIndicatorVisibility);
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