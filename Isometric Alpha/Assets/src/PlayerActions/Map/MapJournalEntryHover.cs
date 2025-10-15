using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class MapJournalEntryHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static UnityEvent<string, bool> OnQuestStarHover = new UnityEvent<string, bool>();

    public TextMeshProUGUI[] highlightableText;

    public DescriptionPanel descriptionPanel;

    private void OnEnable()
    {
        if (highlightableText != null && highlightableText.Length > 0)
        {
            OnQuestStarHover.AddListener(highlightText);
        }
    }

    private void OnDisable()
    {
        if (highlightableText != null && highlightableText.Length > 0)
        {
            OnQuestStarHover.RemoveListener(highlightText);
        }
    }

    private void highlightText(string sceneName, bool entering)
    {
        if (!shouldHighlightText(sceneName))
        {
            return;       
        }

        foreach (TextMeshProUGUI text in highlightableText)
        {
            if (entering)
            {
                text.color = Color.yellow;
            }
            else
            {
                text.color = Color.white;
            }
        }
    }

    private bool shouldHighlightText(string sceneName)
    {
        if (descriptionPanel.getObjectBeingDescribed() == null)
        {
            return false;
        }

        QuestStep questStep = descriptionPanel.getObjectBeingDescribed() as QuestStep;

        if (!questStep.hasTargetLocation())
        {
            return false;
        }

        if (questStep.mapLocation.Equals(sceneName))
        {
            return true;
        }

        IMapObject mapObject = MapObjectList.getMapObject(questStep.mapLocation);

        if (mapObject.isInterior() && mapObject.getExteriorSceneName().Equals(sceneName))
        {
            return true;
        }

        return false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        QuestStep step = (QuestStep)descriptionPanel.getObjectBeingDescribed();

        MapPopUpWindow.showJournalEntryDescription(step);
        MapPopUpWindow.highlightQuestStar(step.mapLocation);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        QuestStep step = (QuestStep)descriptionPanel.getObjectBeingDescribed();

        MapPopUpWindow.hideJournalEntryDescription();
        MapPopUpWindow.unhighlightQuestStar(step.mapLocation);
    }
}