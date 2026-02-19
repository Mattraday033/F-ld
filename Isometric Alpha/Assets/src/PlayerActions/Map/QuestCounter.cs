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

    public List<QuestStep> getListOfQuestStepsForDisplay();
}

public class QuestCounter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IHoverIconSource
{
    public bool disableHover = false;

    public IQuestListSource questListSource;

    public GameObject parentObject;

    public ScrollableUIElement questStepGrid;

    public Image starOutlineImage;
    public Image starInteriorImage;

    public TextMeshProUGUI questCounterText;

    public void updateQuestCounter()
    {
        setQuestCounter(questListSource.getNumberOfQuests());
    }

    public void setQuestListSource(IQuestListSource source)
    {
        questListSource = source;
        updateQuestCounter();
    }

    public void visibilityCheck()
    {
        if(questListSource.getNumberOfQuests() <= 0)
        {
            getVisibilityGameObject().SetActive(false);
        } else
        {
            getVisibilityGameObject().SetActive(true);
        }
    }

    private void setQuestCounter(int questCount)
    {
        questCounterText.text = "" + questCount;

        if (questCount > 0)
        {
            getVisibilityGameObject().SetActive(true);
        }
        else
        {
            getVisibilityGameObject().SetActive(false);
        }
    }

    private GameObject getVisibilityGameObject()
    {
        if(parentObject != null)
        {
            return parentObject;
        } else
        {
            return gameObject;
        }
    }

    public void highlightStar()
    {
        starOutlineImage.color = Color.blue;
        starInteriorImage.color = Color.yellow;
    }

    public void unhighlightStar()
    {
        starOutlineImage.color = ColorList.grey25;
        starInteriorImage.color = ColorList.questCounterCyan;
    }

    public bool sourceIsUnsafe()
    {
        return questListSource == null || questListSource.getListKey() == null || questListSource.getListKey().Length <= 0;
    }

    public void setQuestStepGridVisibility(bool visible)
    {
        questStepGrid.transform.parent.gameObject.SetActive(visible);
    }

    private void setTextHighlight(bool highlight)
    {
        if (questListSource.highlightOnHover())
        {
            IMapObject mapLocation = MapObjectList.getMapObject(questListSource.getListKey());

            if (mapLocation.isInterior())
            {
                MapJournalEntryHover.OnQuestStarHover.Invoke((mapLocation as MapInterior).getExteriorLocationName(), highlight);
            }
            else
            {
                MapJournalEntryHover.OnQuestStarHover.Invoke(questListSource.getListKey(), highlight);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (sourceIsUnsafe() || disableHover || eventData.used)
        {
            return;
        }

        List<QuestStep> questStepsInScene = questListSource.getListOfQuestStepsForDisplay();

        if (questStepsInScene != null && questStepsInScene.Count > 0)
        {
            MouseHoverManager.startCoroutine(this, MouseHoverManager.waitToHandleDescriptionPanel(this, MouseHoverManager.shouldSpawnHoverIcon));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseHoverManager.startCoroutine(this, MouseHoverManager.waitToHandleDescriptionPanel(this, MouseHoverManager.shouldDestroyHoverIcon));
    }

    #region IHoverIconSource

    public virtual void spawnHoverIcon()
    {
        MouseHoverManager.spawnQuestListHover(questListSource, transform);
        setTextHighlight(true);
    }

    public void destroyHoverIcon()
    {
        MouseHoverManager.destroyHoverIcon();
        setTextHighlight(false);
    }
    
    public GameObject getDescriptionPanelType()
    {
        return null;
    }
    public IDescribable getObjectBeingDescribed()
    {
        return null;
    }

    #endregion
}