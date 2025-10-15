using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CunningObject : MonoBehaviour, ISkillTarget, IRevealable
{
	private const string tagText = "Device";

	public SpriteRenderer spriteRenderer;

	public bool flipScale;

	public bool lookingUp;
	public Sprite lookingUpSprite;
	public Sprite lookingDownSprite;
	public bool movePointActive = true;
	public GameObject movePoint;
	public GameObject blockToDelete;

	public GameObject targetCanvas;

	private void Awake()
	{
		spawnTargetCanvas();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void cunning()
	{
		cunning(false);
	}

	public void intimidate()
	{

	}

	public void cunning(bool skipKeyHandling)
	{

		if (lookingUp)
		{
			lookingUp = false;
			GetComponent<SpriteRenderer>().sprite = lookingDownSprite;
			handleDirectionChange();

			if (movePoint != null)
			{
				movePointActive = !movePointActive;
				movePoint.SetActive(movePointActive);
			}
		}
		else
		{
			lookingUp = true;
			GetComponent<SpriteRenderer>().sprite = lookingUpSprite;
			handleDirectionChange();

			if (movePoint != null)
			{
				movePointActive = !movePointActive;
				movePoint.SetActive(movePointActive);
			}
		}

		if (blockToDelete != null)
		{
			blockToDelete.SetActive(false);
		}

		if (!skipKeyHandling)
		{
			if (TrapAndButtonStateManager.contains(getKey()))
			{
				TrapAndButtonStateManager.removeKey(getKey());
			}
			else
			{
				TrapAndButtonStateManager.addKey(getKey());
			}
		}
	}

	public void handleDirectionChange()
	{
		if (flipScale)
		{
			gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * -1,
														  gameObject.transform.localScale.y,
														  gameObject.transform.localScale.z);
		}
		else
		{
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}
	}

	public string getKey()
	{
		float x = Mathf.Round(gameObject.transform.position.x * 10f) / 10f;
		float y = Mathf.Round(gameObject.transform.position.y * 10f) / 10f;

		return AreaManager.locationName + "_CT_" + x + "_" + y;
	}

	private void OnEnable()
	{
		createListeners();
	}

	private void OnDisable()
	{
		destroyListeners();
	}

	//IRevealable interface methods

	public void createListeners()
	{
		RevealManager.OnReveal.AddListener(onReveal);
	}

	public void destroyListeners()
	{
		RevealManager.OnReveal.RemoveListener(onReveal);
	}

	public void onReveal()
	{
		RevealManager.setRevealForGameObject(gameObject, getRevealColor());
	}

	public Color getRevealColor()
	{
		return RevealManager.canBeCunninged;
	}

	public void spawnTargetCanvas()
	{
		if (targetCanvas == null)
		{
			gameObject.AddComponent<RectTransform>();
			targetCanvas = Instantiate(Resources.Load<GameObject>(PrefabNames.targetBox), transform);
		}
	}

	public void createHoverTag()
	{
		MouseHoverManager.createHoverTag(tagText);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!RevealManager.currentlyRevealed)
		{
			RevealManager.setOutlineColor(gameObject, getRevealColor());
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!RevealManager.currentlyRevealed)
		{
			RevealManager.setOutlineColorToDefault(gameObject);
		}
	}
}
