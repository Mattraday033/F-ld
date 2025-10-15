using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public interface IQuestListSource
{
    public string getListKey();

    public bool highlightOnHover();

    public int getNumberOfQuests();

    public ArrayList getListOfQuestsForDisplay();
}

public class QuestCounter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool disableHover = false;

    public IQuestListSource questListSource;

    public ScrollableUIElement questStepGrid;

    public Image starOutlineImage;
    public Image starInteriorImage;

    public TextMeshProUGUI questCounterText;

    public void setQuestListSource(IQuestListSource source)
    {
        questListSource = source;
        setQuestCounter(questListSource.getNumberOfQuests());
    }

    private void setQuestCounter(int questCount)
    {
        questCounterText.text = "" + questCount;

        if (questCount > 0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void highlightStar()
    {
        starOutlineImage.color = Color.blue;
        starInteriorImage.color = Color.yellow;
    }

    public void unhighlightStar()
    {
        starOutlineImage.color = Color.black;
        starInteriorImage.color = Color.white;
    }

    public bool sourceIsUnsafe()
    {
        return questListSource == null || questListSource.getListKey() == null || questListSource.getListKey().Length <= 0;
    }

    public void setQuestStepGridVisibility(bool visible)
    {
        questStepGrid.transform.parent.gameObject.SetActive(visible);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (sourceIsUnsafe() || disableHover || eventData.used)
        {
            return;
        }

        ArrayList questStepsInScene = questListSource.getListOfQuestsForDisplay();

        questStepGrid.populatePanels(questStepsInScene);
        setQuestStepGridVisibility(true);

        setTextHighlight(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (sourceIsUnsafe() || disableHover || eventData.used)
        {
            return;
        }

        setQuestStepGridVisibility(false);

        setTextHighlight(false);
    }

    private void setTextHighlight(bool highlight)
    {
        if (questListSource.highlightOnHover())
        {
            IMapObject mapLocation = MapObjectList.getMapObject(questListSource.getListKey());

            if (mapLocation.isInterior())
            {
                MapJournalEntryHover.OnQuestStarHover.Invoke((mapLocation as MapInterior).getExteriorSceneName(), highlight);
            }
            else
            {
                MapJournalEntryHover.OnQuestStarHover.Invoke(questListSource.getListKey(), highlight);
            }
        }
    }

}
