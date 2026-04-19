using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//b4 Claude

public interface IFloorButton
{
	
	public void evaluate();
	public void declareButton();
	
}

public class FloorButton : MonoBehaviour, INameSource
{
    public string secretDoorFlag;

	public Collider2D collider;
    public SpriteRenderer spriteRenderer;

    public int index;

    public int weight = 1;

    public int charismaRequirement = 1;

    public bool previousIsPressed = false;

    private void Awake()
    {
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
        if(this.secretDoorFlag != null && this.secretDoorFlag.Equals(discoveredSecretDoorFlag))
        {
            gameObject.SetActive(true);
        }
    }

    void Start()
    {
        checkCharismaRequirement();
    }

    public string getName()
    {
        return NPCNameList.button;
    }

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

            spriteRenderer.sprite = Resources.Load<Sprite>(PrefabNames.buttonDownStoneFolderPath);            
        } else
        {
            if(withSFX && isPressed() != previousIsPressed)
            {
                AudioManager.playButtonOffSFX();
            }

            spriteRenderer.sprite = Resources.Load<Sprite>(PrefabNames.buttonUpStoneFolderPath);      
        }

        previousIsPressed = isPressed();
    }

    private void OnEnable()
    {
        ButtonLogicScript.OnButtonDataRequest.AddListener(giveData);
        MovementManager.OnMoveFinished.AddListener(setSprite);        
    }

    private void OnDisable()
    {
        ButtonLogicScript.OnButtonDataRequest.RemoveListener(giveData);
        MovementManager.OnMoveFinished.RemoveListener(setSprite); 
    }
}
