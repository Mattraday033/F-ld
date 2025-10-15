using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public interface IDescribable
{
	public string getName();
	//used to show if it's a valid option, like in the grid for abilities by stats if you have enough 
	public bool ineligible();   //of that stat this would return false, if you didn't have enough of that stat it would return true. 
								//Probably need a better name for it than just ineligible
	public GameObject getRowType(RowType rowType);

	public GameObject getDescriptionPanelFull();

	public GameObject getDescriptionPanelFull(PanelType type);

	public GameObject getDecisionPanel();

	public bool withinFilter(string[] filterParameters);

	public void describeSelfFull(DescriptionPanel panel);

	public void describeSelfRow(DescriptionPanel panel);

	public void setUpDecisionPanel(IDecisionPanel descisionPanel);

	public ArrayList getRelatedDescribables();

	public bool buildableWithBlocks(); // temporary measure for checking if you should be building with IDescribableInBlocks instead

	public bool buildableWithBlocksRows(); // temporary measure for checking if you should be building with IDescribableInBlocks instead
}

public enum PanelType { Standard = 1, AbilityEditor = 2, GlossaryDescription = 3, Combat = 4, CombatHover = 5, Notification = 6, TutorialUITarget = 7, TutorialUITargetUltraWide = 8, Builder = 9, PartyScreenStats = 10, PartyScreenMain = 11 }

public class DescriptionPanel : MonoBehaviour
{
	private IDescribable objectBeingDescribed;

    public string amountPrefix = "";

	public DescriptionPanelSlot[] additionalSlots;

	public bool showingShopkeeperInventory = false;

	public Image iconBackgroundPanel;
	public Image iconPanel;
	
	public Image slotIconPanel;
	public Image slotIconBackgroundPanel;
	
	public Image typeIconPanel;
	public Image typeIconBackgroundPanel;

	public TextMeshProUGUI nameText;
    public TextMeshProUGUI notificationNameText;
    public TextMeshProUGUI secondaryNameText;
	public TextMeshProUGUI inputText;
	public TextMeshProUGUI hpText;
	public TextMeshProUGUI typeText;
	public TextMeshProUGUI amountText;
	public TextMeshProUGUI statText;
	public TextMeshProUGUI locationText;
	public TextMeshProUGUI loreDescriptionText;
	public TextMeshProUGUI useDescriptionText;
    public TextMeshProUGUI contentsText;
	public TextMeshProUGUI worthText;
	public TextMeshProUGUI slotText;
	public TextMeshProUGUI weaponNameText;
	public TextMeshProUGUI rangeText;
	public TextMeshProUGUI armorRatingText;
	public TextMeshProUGUI damageText;
	public TextMeshProUGUI offHandDamageText;
	public TextMeshProUGUI critRatingText;
	public TextMeshProUGUI offHandCritRatingText;
	public TextMeshProUGUI timerText;
	public TextMeshProUGUI levelText;
	public TextMeshProUGUI experienceText;
	public TextMeshProUGUI affinityText;
	public TextMeshProUGUI partyText;
	public TextMeshProUGUI strText;
	public TextMeshProUGUI dexText;
	public TextMeshProUGUI wisText;
	public TextMeshProUGUI chaText;
	
	public DescriptionPanel nestedDescriptionPanel;
	
	public Canvas gameObjectDisplayCanvas;
	public int[] displayAdjustmentCoords = new int[2];
	private GameObject gameObjectToDisplay;

	public static void setImage(Image image, Sprite sprite)
	{
		if(image != null && !(image is null))
		{
			image.sprite = sprite;
		}
	}

	public static void setImageColor(Image image, Color color)
	{
		if(image != null && !(image is null))
		{
            if (image.color.Equals(Color.clear) || color.a == 0f)
			{
				image.enabled = false;
			}
			else
			{
				image.enabled = true;
				image.color = color;
			}
		}
	}

	public static void setText(TextMeshProUGUI tmpTextObject, string newText)
	{
		if(tmpTextObject != null && !(tmpTextObject is null))
		{
			tmpTextObject.text = newText;
		}
	}
	
	public static void setText(TextMeshProUGUI tmpTextObject, int newText)
	{
		if(tmpTextObject != null && !(tmpTextObject is null))
		{
			tmpTextObject.text = "" + newText;
		}
	}

    public static void setTextColor(TextMeshProUGUI tmpTextObject, Color color)
    {
        if (tmpTextObject != null && !(tmpTextObject is null))
        {
            tmpTextObject.color = color;
        }
    }

	public virtual void setObjectBeingDescribed(IDescribable describable)
	{
		objectBeingDescribed = describable;

		foreach (DescriptionPanelSlot slot in additionalSlots)
		{
			slot.setPrimaryDescribable(describable);
		}
	}
	
	public Item getItemBeingDescribed()
	{
		return (Item) objectBeingDescribed;
	}
	
	public IDescribable getObjectBeingDescribed()
	{
		return objectBeingDescribed;
	}		
	
	public static GameObject getDescriptionPanel(string panelTypeName)
	{
		GameObject descriptionPanelGameObject = Resources.Load<GameObject>(panelTypeName); 
		
		if(descriptionPanelGameObject == null || descriptionPanelGameObject is null)
		{
			Debug.LogError("Game Object Prefab (" +panelTypeName+ ") does not exist");
		}
		
		return descriptionPanelGameObject;
	}
	
	public DescriptionPanel getNestedDescriptionPanel()
	{
		return nestedDescriptionPanel;
	}
}
