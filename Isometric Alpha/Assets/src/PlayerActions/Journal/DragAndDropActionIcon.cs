using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDropCombatActionIcon : MonoBehaviour
{
	private RectTransform rectTransform;
	
	public Collider2D collider;
	
	public AbilityMenuButton abilityMenuButton;

	private RectTransform dragAndDropParentRectTransform;
	private Vector2 positionMatrix;
	private bool mouseWasReleased;

	private void Awake()
	{
		dragAndDropParentRectTransform = DragAndDropParentDeclarer.dragAndDropParent.GetComponent<RectTransform>();

		positionMatrix = new Vector2(dragAndDropParentRectTransform.rect.width / Camera.main.pixelWidth, dragAndDropParentRectTransform.rect.height / Camera.main.pixelHeight);

		rectTransform = GetComponent<RectTransform>();
		Update();
	}

	void Update() //here for Animation
	{
		Vector3 mousePos = Input.mousePosition;

		rectTransform.localPosition = mousePos * positionMatrix;

		if (!Input.GetMouseButton(0))
		{
			mouseWasReleased = true;
		}

		if(mouseWasReleased && Input.GetMouseButton(0))
		{
			Collider2D collision = Helpers.getCollision(collider);

			if(collision == null || collision is null)
			{
				GameObject.Destroy(gameObject);
				return;
			}

			GameObject collisionObject = collision.gameObject;

			if(collisionObject.tag.Equals(LayerAndTagManager.abilityEditorTag))
			{
				int buttonIndex = collisionObject.GetComponent<AbilityMenuButton>().index;

				EditAbilityWheelPopUpWindow.getInstance().setAbilityAtIndex(abilityMenuButton.loadedCombatAction.clone(), buttonIndex);
			}

			GameObject.Destroy(gameObject);
		}
	}
}
