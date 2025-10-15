using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public interface IRevealable : IPointerEnterHandler,
	IPointerExitHandler
{
	public void createListeners();

	public void destroyListeners();

	public void onReveal();

	public Color getRevealColor();

	public void spawnTargetCanvas();

	public void createHoverTag();
}

public static class RevealManager
{
	public static bool currentlyRevealed = false;

	public static UnityEvent OnReveal = new UnityEvent();

	public static Color attacksOnSight = Color.red;
	public static Color canBeInteractedWith = Color.green;
	public static Color canBePushed = Color.blue;
	public static Color canBeCunninged = Color.yellow;
	public static Color defaultWhenNotRevealed = Color.clear;
	public static Color tutorialDefault = Color.cyan;

	public static void setRevealForGameObject(GameObject gameObject, Color outlineColor)
	{
		if (gameObject == null)
		{
			return;
		}

		SpriteOutline outlineComponent = gameObject.GetComponent<SpriteOutline>();

		if (outlineComponent == null || outlineComponent is null)
		{
			return;
		}

		if (currentlyRevealed || CombatStateManager.inCombat)
		{
			setOutlineColor(outlineComponent, outlineColor);
		}
		else
		{
			setOutlineColorToDefault(outlineComponent);
		}
	}

	public static void setOutlineColor(GameObject gameObject, Color outlineColor)
	{
		if (gameObject == null)
		{
			return;
		}

		setOutlineColor(gameObject.GetComponent<SpriteOutline>(), outlineColor);
	}
	public static void setOutlineColor(SpriteOutline outlineComponent, Color outlineColor)
	{
		if (outlineComponent == null)
		{
			return;
		}

		outlineComponent.color = outlineColor;
		resetOutlineComponent(outlineComponent);
	}

	public static void setOutlineColorToDefault(GameObject gameObject)
	{
		if (gameObject == null)
		{
			return;
		}

		setOutlineColorToDefault(gameObject.GetComponent<SpriteOutline>());
	}

	public static void setOutlineColorToDefault(SpriteOutline outlineComponent)
	{
		if (outlineComponent == null)
		{
			return;
		}

		outlineComponent.color = defaultWhenNotRevealed;
		resetOutlineComponent(outlineComponent);
	}

	private static void resetOutlineComponent(SpriteOutline outlineComponent)
	{
		if (outlineComponent == null)
		{
			return;
		}

		outlineComponent.enabled = false;
		outlineComponent.enabled = true;
	}

	public static void manuallyRevealGameObject(GameObject gameObject, Color outlineColor)
	{
		SpriteOutline outlineComponent = gameObject.GetComponent<SpriteOutline>();

		if (outlineComponent == null || outlineComponent is null)
		{
			return;
		}

		outlineComponent.color = outlineColor;

		Helpers.updateColliderPosition(gameObject);
	}

	public static void manuallyUnrevealGameObject(GameObject gameObject)
	{
		SpriteOutline outlineComponent = gameObject.GetComponent<SpriteOutline>();

		if (outlineComponent == null || outlineComponent is null)
		{
			return;
		}

		outlineComponent.color = defaultWhenNotRevealed;
		
		Helpers.updateColliderPosition(gameObject);
	}

	public static void toggleReveal()
	{
		currentlyRevealed = !currentlyRevealed;

		revealAllObjects();
	}

	public static void revealAllObjects()
	{
		OnReveal.Invoke();
	}
}
