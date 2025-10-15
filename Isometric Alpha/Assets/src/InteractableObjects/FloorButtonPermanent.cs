using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorButtonPermanent : MonoBehaviour, IFloorButton
{
	public bool openGatePermanently;
	private CapsuleCollider2D collider;
	
	public bool deactivateTargetOnPress;
	
	public ContactFilter2D filterPlayer;
	public ContactFilter2D filterMovableObject;
	
	public GameObject target;
	public GameObject[] targets;
	
	//[SerializeField]
	public IFloorButtonScript script;
	
	void Start()
	{
		collider = gameObject.GetComponent<CapsuleCollider2D>();
		script = GetComponent<IFloorButtonScript>();
		
		declareButton();
	}

    public void evaluate()
    {
		if((Helpers.hasCollision(collider, filterPlayer) || Helpers.hasCollision(collider, filterMovableObject)) && 
			!gameObject.GetComponent<SpriteRenderer>().color.Equals(Color.green))
		{
			handleButtonPress();
		} 
    }
	
	public void declareButton()
	{
		MovementManager.getInstance().addFloorButton(this);
	}
	
	public void handleButtonPress()
	{
		handleButtonPress(false);
	}
	
	public void handleButtonPress(bool skipKeyHandling)
	{
		gameObject.GetComponent<SpriteRenderer>().color = Color.green;

		if (script != null)
		{
			script.activate();
			return;
		}
			
		if(deactivateTargetOnPress)
		{
			if(target == null)
			{
				if(targets != null)
				{
					foreach(GameObject targetToDeactivate in targets)
					{
						deactivateTargets(targetToDeactivate);
					}
				}
			} else
			{
				deactivateTargets(target);
			}
		}
		
		if(!skipKeyHandling)
		{
			TrapAndButtonStateManager.addKey(getKey());
		}
	}
	
	private void deactivateTargets(GameObject targetToDeactivate)
	{
		if (targetToDeactivate == null)
		{
			return;
		}

		if (openGatePermanently)
		{
			targetToDeactivate.GetComponent<ButtonTarget>().setToOpenedPermanently();
		}
		else
		{
			targetToDeactivate.GetComponent<ButtonTarget>().setToOpened();
		}
	}

	public string getKey()
	{
		float x = Mathf.Round(gameObject.transform.position.x * 10f) / 10f;
		float y = Mathf.Round(gameObject.transform.position.y * 10f) / 10f;

		return SceneManager.GetActiveScene().name + "_PB_" + x + "_" + y;
	}
}
