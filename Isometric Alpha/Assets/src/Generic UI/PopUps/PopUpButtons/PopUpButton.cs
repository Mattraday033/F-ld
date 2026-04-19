using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Events;
using UnityEngine;

public enum PopUpType	{
							FormationEditor = 1, 
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
							WorldMap = 19,
                            CombatEscapeMenu = 20,
							SettingsScreen = 21 
						}

public abstract class PopUpButton : MonoBehaviour 
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

    public abstract GameObject getCurrentPopUpGameObject();

    public virtual void spawnPopUp()
	{
		PopUpScreenBlockerManager.spawnPopUpScreenBlocker();
		Instantiate(Resources.Load<GameObject>(getPopUpPrefabName(type)), PopUpScreenBlockerManager.getPopUpParent());

		popUpWindow = getCurrentPopUpGameObject().GetComponent<PopUpWindow>(); 
		
		popUpWindow.setProgenitor(this);
		
		EscapeStack.addEscapableObject(popUpWindow);

        if(CombatStateManager.inCombat && CombatStateManager.whoseTurn != WhoseTurn.Lost)
        {
            TutorialSequenceStepTargetUIObject.createCutOutMask(popUpWindow.transform);
        }

        playOpenSFX(type);
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

		PopUpScreenBlockerManager.destroyPopUpScreenBlocker();

		if(shouldReturnToWalkingMode())
		{
			PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
		}
	}
	
	public virtual bool shouldReturnToWalkingMode()
	{ 
        if (OverallUIManager.currentScreenManager == null && OverallUIManager.currentScreenManager is null && EscapeStack.getEscapableObjectsCount() == 0 && !TutorialSequence.currentlyInTutorialSequence() && PlayerOOCStateManager.currentActivity != OOCActivity.Defeat)
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
				
			case PopUpType.BinaryPanel:
				return PrefabNames.binaryDecisionPanel;

			case PopUpType.LoadOnlyScreen:
				return PrefabNames.saveScreen;
				
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

			case PopUpType.WorldMap:
				return PrefabNames.worldMapPopUpWindow;

            case PopUpType.CombatEscapeMenu:
                return PrefabNames.combatEscapeMenu;
            case PopUpType.SettingsScreen:
                return PrefabNames.settingsScreen;
            default:
				throw new IOException("Unknown PopUpType: " + type.ToString());
		}
	}

    private static void playOpenSFX(PopUpType type)
    {
        switch(type)
        {
            case PopUpType.DialogueTrackerWithChoices:
            case PopUpType.BinaryPanel:
                return;
            default:
                AudioManager.playChangeScreenSFX();
                return;
        }

    }
}
