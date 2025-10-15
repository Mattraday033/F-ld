using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DamageNumberPopup : MonoBehaviour
{
	public TextMeshProUGUI damageNumberTMP;
	
	private int frameDelay = 0;
	
	private float disappearTimer = .7f;
	private float disappearSpeed = 3f;
	private float moveSpeedY = 1f;
	private float moveSpeedX = .4f;
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
	
		transform.position += new Vector3(moveSpeedX, moveSpeedY) * Time.deltaTime;
		
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

	public void populate(int damageAmount)
	{
		damageNumberTMP.text = "" + damageAmount;
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
	
	public static DamageNumberPopup create(int damageAmount, Vector3 newPosition, Transform canvas, bool crit, bool healsTarget)
	{
		GameObject damageNumberObject;
		
		if(healsTarget)
		{
			damageNumberObject = Instantiate(Resources.Load<GameObject>("Healing Numbers PF"), canvas).gameObject;
		} else if(crit)
		{
			damageNumberObject = Instantiate(Resources.Load<GameObject>("Critical Damage Numbers PF"), canvas).gameObject;
		} else
		{
			damageNumberObject = Instantiate(Resources.Load<GameObject>("Damage Numbers PF"), canvas).gameObject;
		}

		DamageNumberPopup popup = damageNumberObject.GetComponent<DamageNumberPopup>();
		popup.populate(damageAmount);
		popup.moveTo(newPosition);
		
		damageNumberObject.SetActive(true);
		
		return popup;
	}
	
	public static DamageNumberPopup create(int damageAmount, Vector3 newPosition, Transform canvas, bool crit, bool healsTarget, int frameDelay)
	{
		DamageNumberPopup popup = create(damageAmount, newPosition, canvas, crit, healsTarget);
		
		popup.setFrameDelay(frameDelay);
		
		return popup;
	}
}
