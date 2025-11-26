using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    private void Awake()
    {
        SecretDoorFlags.OnSecretDoorDiscovery.AddListener(show);
    }
    private void OnDestroy()
    {
        SecretDoorFlags.OnSecretDoorDiscovery.RemoveListener(show);
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
        StartCoroutine(waitThreeFramesThenSetSprite());
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

        setSprite(Constants.indexZero);
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

    private void setSprite(int movementIndex)
    {
        if(isPressed())
        {
            spriteRenderer.sprite = Resources.Load<Sprite>(PrefabNames.buttonDownStoneFolderPath);            
            // spriteRenderer.color = Color.green;
        } else
        {
            spriteRenderer.sprite = Resources.Load<Sprite>(PrefabNames.buttonUpStoneFolderPath);      
            // spriteRenderer.color = Color.white;            
        }
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
