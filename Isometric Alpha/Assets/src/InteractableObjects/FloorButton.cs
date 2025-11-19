using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IFloorButton
{
	
	public void evaluate();
	public void declareButton();
	
}

public class FloorButton : MonoBehaviour
{

	public Collider2D collider;
    public SpriteRenderer spriteRenderer;

    public int index;

    public int weight = 1;

    void Start()
    {
        StartCoroutine(waitThreeFramesThenSetSprite());
    }

    private IEnumerator waitThreeFramesThenSetSprite()
    {
        yield return null;

        yield return null;

        yield return null;

        setSprite();
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

    private void setSprite()
    {
        if(isPressed())
        {
            spriteRenderer.color = Color.green;
        } else
        {
            spriteRenderer.color = Color.white;            
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
