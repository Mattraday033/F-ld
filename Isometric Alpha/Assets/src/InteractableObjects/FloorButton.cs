using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloorButton : MonoBehaviour, INameSource
{
    public string secretDoorFlag;

	public Collider2D collider;

    [SerializeField]
    private SpriteLayerRendererList _RendererList;
    public SpriteLayerRendererList rendererList
    {
        get
        {
            return _RendererList;
        }
        set
        {
            _RendererList = value;
        }
    }

    //the button is a single sprite, so like SpriteDescription appearances it draws to the body layer
    private SpriteRenderer buttonRenderer { get { return _RendererList[SpriteLayer.Body]; } }

    public int index;

    public int weight = 1;

    public int charismaRequirement = 1;

    public bool previousIsPressed = false;

    private void Awake()
    {
        if(_RendererList == null)
        {
            _RendererList = GetComponent<SpriteLayerRendererList>();
        }

        Formation.OnFormationChange.AddListener(checkCharismaRequirement);
        SecretDoorFlags.OnSecretDoorDiscovery.AddListener(show);
        
    }
    private void OnDestroy()
    {
        Formation.OnFormationChange.RemoveListener(checkCharismaRequirement);
        SecretDoorFlags.OnSecretDoorDiscovery.RemoveListener(show);
    }

    private void checkCharismaRequirement()
    {
        gameObject.SetActive(PartyStats.getHighestCharisma() >= charismaRequirement);

        if(gameObject.activeInHierarchy)
        {
            StartCoroutine(waitThreeFramesThenSetSprite());
        }
    }

    private void show(string discoveredSecretDoorFlag)
    {
        if(secretDoorFlag != null && secretDoorFlag.Equals(discoveredSecretDoorFlag))
        {
            gameObject.SetActive(true);
        }
    }

    void Start()
    {
        checkCharismaRequirement();
    }

    public string displayName { get { return NPCNameList.button; } }

    //the index tells floor buttons apart, the same way it does for ButtonSpawnDetails
    public string uniqueName { get { return NPCNameList.button + index; } }

    private IEnumerator waitThreeFramesThenSetSprite()
    {
        yield return null;

        yield return null;

        yield return null;

        setSprite(Constants.indexZero, false);
    }

    public string getKey()
    {
        return AreaManager.locationName + index;
    }

    public void giveData(ButtonLogicScript buttonLogicScript)
    {
        buttonLogicScript.getFloorButtonStatus(this);
    }

    public bool isPressed()
    {
        return Helpers.hasCollision(collider, LayerAndTagManager.pressesButtonsLayerMask); 
    }

    public void setSprite(int movementIndex)
    {
        setSprite(movementIndex, true);
    }

    public void setSprite(int movementIndex, bool withSFX)
    {
        if(isPressed())
        {
            if(withSFX && isPressed() != previousIsPressed)
            {
                AudioManager.playButtonOnSFX();
            }

            buttonRenderer.sprite = SpriteUtil.loadSpriteFromResources(PrefabNames.buttonDownStoneFolderPath);
        } else
        {
            if(withSFX && isPressed() != previousIsPressed)
            {
                AudioManager.playButtonOffSFX();
            }

            buttonRenderer.sprite = SpriteUtil.loadSpriteFromResources(PrefabNames.buttonUpStoneFolderPath);
        }

        previousIsPressed = isPressed();
    }

    //OnStepFinished keeps the sprite current after every step, since OnMoveFinished now only fires once a mover stops.
    //OnMoveFinished is still needed for the places that invoke it by hand, like a movable object snapping back.
    //setSprite only plays a sound when the pressed state changes, so both firing on the final step is harmless
    private void OnEnable()
    {
        ButtonLogicScript.OnButtonDataRequest.AddListener(giveData);
        MovementManager.OnStepFinished.AddListener(setSprite);
        MovementManager.OnMoveFinished.AddListener(setSprite);
    }

    private void OnDisable()
    {
        ButtonLogicScript.OnButtonDataRequest.RemoveListener(giveData);
        MovementManager.OnStepFinished.RemoveListener(setSprite);
        MovementManager.OnMoveFinished.RemoveListener(setSprite);
    }
}
