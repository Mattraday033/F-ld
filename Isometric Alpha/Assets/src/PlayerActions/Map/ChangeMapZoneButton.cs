using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeMapZoneButton : MonoBehaviour, IQuestListSource
{

	private string zoneKey;

	public QuestCounter questCounter;

	public Button button;

	public TextMeshProUGUI label;

	private void Awake()
	{
		questCounter.setQuestListSource(this);

		if (getNumberOfQuests() > 0)
		{
			questCounter.gameObject.SetActive(true);
		}

	}

	private void setInteractability()
	{
		if (MapZone.hasBeenDiscovered(zoneKey))
		{
			button.interactable = true;
		}
		else
		{
			button.interactable = false;
		}
	}

	public void setToZoneMap()
	{
		MapPopUpWindow.getInstance().populate(zoneKey);
	}

	public void setZoneKey(string zoneKey)
	{
		this.zoneKey = zoneKey;

		gameObject.SetActive(true);

		label.text = "To " + MapObjectList.getMapObject(zoneKey).getMapUIDisplayName();

		setInteractability();

		questCounter.setQuestStepGridVisibility(false);
	}

	public void deactivate()
	{
		gameObject.SetActive(false);
	}

    //IQuestListSource methods
    public string getListKey()
    {
        return zoneKey;
    }

    public bool highlightOnHover()
    {
        return false;
    }

    public int getNumberOfQuests()
    {
        return QuestList.getNumberOfActiveUnfinishedQuestsInZone(zoneKey);
    }

	public ArrayList getListOfQuestsForDisplay()
	{
		return QuestList.getActiveUnfinishedQuestStepsInZone(zoneKey);
	}
}
