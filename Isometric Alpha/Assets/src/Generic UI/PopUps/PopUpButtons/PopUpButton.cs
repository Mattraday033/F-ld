using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Events;
using UnityEngine;

public enum PopUpType	{
							FormationEditor = 1, 
							UseItemOnPartyMember = 2, 
							BinaryPanel = 3, 
							DialogueTrackerWithoutChoices = 4, 
							FullEditAbilityWheel = 5, 
							LoadOnlyScreen = 6, 
							SingleEditAbilityWheel = 7, 
							DialogueTrackerWithChoices = 8, 
							CombatResults = 9,
							HoverPanel = 10,
							LevelUp = 11,
							CharacterCreation = 12,
							Book = 13,
							Shop = 14,
							GameOver = 15,
                            Tutorial = 16,
							Notification = 17,
							Map = 18,
						}

public class PopUpButton : MonoBehaviour 
{
	public PopUpType type;
	private PopUpWindow popUpWindow;
	
	public PopUpButton(PopUpType type)
	{
		this.type = type;
	}

	public PopUpWindow getPopUpWindow()
	{
		return popUpWindow;
	}

    public void setPopUpWindow(PopUpWindow popUpWindow)
    {
        this.popUpWindow = popUpWindow;
    }

    public virtual GameObject getCurrentPopUpGameObject()
    {
		throw new IOException("Base version of getCurrentPopUpGameObject() called erronously");
    }

    public virtual void spawnPopUp()
	{
		PopUpBlocker.spawnPopUpScreenBlocker();
		Instantiate(Resources.Load<GameObject>(getPopUpPrefabName(type)), PopUpBlocker.getPopUpParent());

		popUpWindow = getCurrentPopUpGameObject().GetComponent<PopUpWindow>(); 
		
		popUpWindow.setProgenitor(this);
		
		EscapeStack.addEscapableObject(popUpWindow);
    }
	


	public virtual void destroyPopUp()
	{
		if (getCurrentPopUpGameObject() != null && !(getCurrentPopUpGameObject() is null))
		{
			DestroyImmediate(getCurrentPopUpGameObject());
			EscapeStack.removeTopObjectFromStack();
		}
		else
		{
			EscapeStack.removeAllNullObjectsFromStack();
		}

		PopUpBlocker.destroyPopUpScreenBlocker();

		if(shouldReturnToWalkingMode())
		{
			PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
		}
	}
	
	public virtual bool shouldReturnToWalkingMode()
	{ 
        if (OverallUIManager.currentScreenManager == null && OverallUIManager.currentScreenManager is null && EscapeStack.getEscapableObjectsCount() == 0 && !TutorialSequence.currentlyInTutorialSequence())
		{
			//Helpers.debugNullCheck("OverallUIManager.currentScreen", OverallUIManager.currentScreen);
			return true;
		} else
		{
			return false; 
		}
	}
	
	public static string getPopUpPrefabName(PopUpType type)
	{
		switch(type)
		{
			case PopUpType.FormationEditor:
				return PrefabNames.formationEditorPanel;
				
			case PopUpType.UseItemOnPartyMember:
				return PrefabNames.outOfCombatHealPartyMemberPopUp;
				
			case PopUpType.BinaryPanel:
				return PrefabNames.binaryDecisionPanel;

			case PopUpType.DialogueTrackerWithoutChoices:				
				return PrefabNames.dialogueTrackerWindowPopUp;
				
			case PopUpType.FullEditAbilityWheel:
				return PrefabNames.abilityWheelEditorFull;
			
			case PopUpType.LoadOnlyScreen:
				return PrefabNames.loadOnlyPopUp;
				
			case PopUpType.SingleEditAbilityWheel:
				return PrefabNames.singleEditAbilityWheelPopUp;
				
			case PopUpType.DialogueTrackerWithChoices:
				return PrefabNames.dialogueTrackerWindowWithChoicesPopUp;
				
			case PopUpType.CombatResults:
				return PrefabNames.combatResultsPopUp;
				
			case PopUpType.HoverPanel:
				return PrefabNames.hoverPanelPopUpWindow;

			case PopUpType.LevelUp:
				return PrefabNames.levelUpPopUpWindow;

            case PopUpType.CharacterCreation:
                return PrefabNames.characterCreationPopUpWindow;

            case PopUpType.Book:
                return PrefabNames.bookPopUpWindow;

            case PopUpType.Shop:
                return PrefabNames.shopPopUpWindow;

            case PopUpType.GameOver:
                return PrefabNames.gameOverPopUpWindow;

            case PopUpType.Tutorial:
                return PrefabNames.tutorialPopUpWindow;
			
            case PopUpType.Notification:
                return PrefabNames.notificationPopUpWindow;
				
			case PopUpType.Map:
				return PrefabNames.mapPopUpWindow;

            default:
				throw new IOException("Unknown PopUpType: " + type.ToString());
		}
	}
}
