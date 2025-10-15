using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IDragAndDropListener
{
    public void createListeners();
    public void destroyListeners();

    public bool draggedObjectIsFromSource(IDescribable objectBeingDragged);

    public void handleDragAndDropCreated(IDescribable objectBeingDragged);
    public void handleDragAndDropDestroyed(IDescribable objectBeingDragged);

}

public class PartyPositionSpriteGridSquare : PartyPositionGridSquare, IPointerDownHandler, IDragAndDropSource, IDragAndDropListener
{

    private static Color spaceOccupiedColor = new Color32(55, 55, 55, 255);
    private static Color spaceFreeColor = new Color32(155, 155, 155, 255);

    public Image partyMemberSprite;

    public BoxCollider2D boxCollider;
    public RectTransform rectTransform;

    private void OnEnable()
    {
        createListeners();
    }

    private void OnDisable()
    {
        destroyListeners();
    }

    void Start()
    {
        partyEditor = (IPartyEditor)OverallUIManager.currentScreenManager;

        resizeCollider();
        determineButtonEnabled();
    }

    public override void determineButtonEnabled()
    {
        if (characterInSquare != null && characterInSquare != PartyManager.getPlayerStats())
        {
            button.enabled = true;
        }
        else
        {
            button.enabled = false;
        }
    }

    private void resizeCollider()
    {
        boxCollider.size = rectTransform.sizeDelta;
    }

    private void enablePartyMemberSprite()
    {
        image.color = spaceOccupiedColor;
        partyMemberSprite.color = characterInSquare.getSpriteColor();
        partyMemberSprite.enabled = true;
    }

    private void disablePartyMemberSprite()
    {
        image.color = spaceFreeColor;
        partyMemberSprite.enabled = false;
    }

    public override void populate(AllyStats character)
    {
        base.populate(character);

        if (character != null)
        {
            enablePartyMemberSprite();
            disableNameText();
        }
        else
        {
            disablePartyMemberSprite();
        }
    }


    public override void handleButtonPress()
    {
        if (characterInSquare != null)
        {
            partyEditor.removeCharacter(characterInSquare);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (characterInSquare != null)
        {
            StartCoroutine(DragAndDropManager.waitForMouseRelease(this, PartyManager.getPartyMember(characterInSquare.getName())));
        }
    }

    //IDragAndDropSource Methods
    public string getDragAndDropPrefabName()
    {
        return PrefabNames.partyMemberSpriteDragAndDrop;
    }

    //IDragAndDropListener Methods
    public void createListeners()
    {
        DragAndDropManager.OnDragAndDropCreated.AddListener(handleDragAndDropCreated);
        DragAndDropManager.OnDragAndDropDestroyed.AddListener(handleDragAndDropDestroyed);
    }

    public void destroyListeners()
    {
        DragAndDropManager.OnDragAndDropCreated.RemoveListener(handleDragAndDropCreated);
        DragAndDropManager.OnDragAndDropDestroyed.RemoveListener(handleDragAndDropDestroyed);
    }

    public bool draggedObjectIsFromSource(IDescribable objectBeingDragged)
    {
        return Stats.convertIDescribableToStats(objectBeingDragged) == characterInSquare;
    }

    public void handleDragAndDropCreated(IDescribable objectBeingDragged)
    {
        if (draggedObjectIsFromSource(objectBeingDragged))
        {
            AllyStats statsBeingDragged = Stats.convertIDescribableToStats(objectBeingDragged);

            disablePartyMemberSprite();
            partyEditor.removeCharacter(statsBeingDragged);
            characterInSquare = statsBeingDragged;
        }
    }

    public void handleDragAndDropDestroyed(IDescribable objectBeingDragged)
    {
        if (draggedObjectIsFromSource(objectBeingDragged))
        {
            AllyStats statsBeingDragged = Stats.convertIDescribableToStats(objectBeingDragged);

            if (!partyEditor.getFormation().contains(statsBeingDragged))
            {
                populate(statsBeingDragged);
                partyEditor.addCharacterToFormation(characterInSquare, row, col);
            }
            else
            {
                if (partyEditor.getFormation().findLocationOfStats(statsBeingDragged).Equals(new GridCoords(row, col)))
                {
                    populate(statsBeingDragged);
                }
                else
                {
                    populate();
                }
            }
        }
    }
}
