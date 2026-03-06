using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum PopUpMoveDirection { Up, UpLeft, UpRight }

public class DamageNumberPopup : MonoBehaviour
{
	public TextMeshProUGUI damageNumberTMP;
	
	private int frameDelay = 0;
	
    public PopUpMoveDirection direction = PopUpMoveDirection.UpRight;

	private float disappearTimer = .7f;
	private float disappearSpeed = 3f;
	private const float moveSpeedY = 1f;
	private const float moveSpeedX = .4f;
	private Color textColor;
	
	void Update() //here for Animation 
	{	
		if(frameDelay > 0)
		{
			frameDelay--;
			return;
		} else if(frameDelay == 0)
		{
			frameDelay--;
		}
	
		transform.position += new Vector3(getMoveSpeedX(), moveSpeedY) * Time.deltaTime;
		
		disappearTimer -= Time.deltaTime;
		
		if(disappearTimer < 0)
		{
			textColor.a -= disappearSpeed * Time.deltaTime;
			damageNumberTMP.color = textColor;
			
			if(textColor.a < 0)
			{
				Destroy(gameObject);
			}
		}
	}

    public bool isVisible()
    {
        return gameObject != null && gameObject.activeInHierarchy;
    }

    public void setToVisible()
    {
        gameObject.SetActive(true);
        enabled = true;
    }

    public float getMoveSpeedX()
    {
        switch(direction)
        {
            case PopUpMoveDirection.UpRight:
                return moveSpeedX;
            case PopUpMoveDirection.UpLeft:
                return moveSpeedX*-1f;
            default:
                return 0f;
        }
    }

	public void populate(string damageAmount)
	{
		damageNumberTMP.text = damageAmount;
		textColor = damageNumberTMP.color;
		textColor.a = 1f;
	}
	
	public void moveTo(Vector3 newPosition)
	{
		gameObject.transform.position = newPosition;
	}
	
	public void setFrameDelay(int frameDelay)
	{
		this.frameDelay = frameDelay;
	}
	
	public static DamageNumberPopup createResistPopUp(GridCoords targetCoords, Vector3 newPosition, Transform canvas)
	{
		return create(targetCoords, Constants.resist, newPosition, PopUpMoveDirection.Up, canvas, false, false);
	}
    
	public static DamageNumberPopup create(GridCoords targetCoords, int damageAmount, Vector3 newPosition, PopUpMoveDirection direction, Transform canvas, bool crit, bool healsTarget)
	{
		return create(targetCoords, damageAmount.ToString(), newPosition, direction, canvas, crit, healsTarget);
	}

	public static DamageNumberPopup create(GridCoords targetCoords, string damageAmount, Vector3 newPosition, PopUpMoveDirection direction, Transform canvas, bool crit, bool healsTarget)
	{
		GameObject damageNumberObject;
		
		if(healsTarget)
		{
			damageNumberObject = Instantiate(Resources.Load<GameObject>(PrefabNames.healingNumbersFont), canvas).gameObject;
		} else if(crit)
		{
			damageNumberObject = Instantiate(Resources.Load<GameObject>(PrefabNames.critNumbersFont), canvas).gameObject;
		} else
		{
			damageNumberObject = Instantiate(Resources.Load<GameObject>(PrefabNames.damageNumbersFont), canvas).gameObject;
		}

		DamageNumberPopup popup = damageNumberObject.GetComponent<DamageNumberPopup>();
		popup.populate(damageAmount);
		popup.moveTo(newPosition);
        popup.direction = direction;
        popup.enabled = false;

		damageNumberObject.SetActive(false);
        DamageNumberPopupQueue.addDamageNumberToQueue(targetCoords, popup);
		
		return popup;
	}
	
	public static DamageNumberPopup create(GridCoords targetCoords, int damageAmount, Vector3 newPosition, PopUpMoveDirection direction, Transform canvas, bool crit, bool healsTarget, int frameDelay)
	{
		return create(targetCoords, damageAmount.ToString(), newPosition, direction, canvas, crit, healsTarget, frameDelay);
	}

	public static DamageNumberPopup create(GridCoords targetCoords, string damageAmount, Vector3 newPosition, PopUpMoveDirection direction, Transform canvas, bool crit, bool healsTarget, int frameDelay)
	{
		DamageNumberPopup popup = create(targetCoords, damageAmount, newPosition, direction, canvas, crit, healsTarget);
		
		popup.setFrameDelay(frameDelay);
		
		return popup;
	}

    public static PopUpMoveDirection getDirectionByTargetCoords(GridCoords targetCoords)
    {
        bool allySide = CombatGrid.positionIsOnAlliedSide(targetCoords);

        if(allySide)
        {
            return PopUpMoveDirection.UpLeft;
        } else
        {
            return PopUpMoveDirection.UpRight;
        }
    }
}

