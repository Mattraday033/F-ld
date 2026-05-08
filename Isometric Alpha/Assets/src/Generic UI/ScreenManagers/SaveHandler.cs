using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine.Events;

[System.Serializable]
public class SaveHandler : ScreenManager, IEscapable
{
    public readonly static UnityEvent<IDescribable> OnSaveCreated = new UnityEvent<IDescribable>();
    public readonly static UnityEvent<IDescribable> OnSaveDeleted = new UnityEvent<IDescribable>();

	private static Dictionary<string, SaveBlueprint> saveGameList;

    public const int saveNameCharacterLimit = 20;

	public const string autoSave1Name = "Autosave 1";
    public const string autoSave2Name = "Autosave 2";
    public const string autoSave3Name = "Autosave 3";
	public const string quickSaveName = "Quicksave";

	public const string cleanSlateSaveName = "cleanSlateSave";

    public TMP_InputField saveNameField;
    public Button saveButton;
    public BinaryPanelPopUpButton overwriteButton;

    public SaveBlueprint currentSaveFile;

	private static SaveHandler instance;
	
	public static SaveHandler getInstance()
	{
		return instance;
	}

	public override void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Found more than one Save Manager in the scene.");
		}

		instance = this;

		if (saveGameList == null || saveGameList.Count == 0)
		{
			createSavedGameList();
		}

		if(Flags.isInNewGameMode())
        {
            OverallUIManager.setCurrentScreenType(this);
        }
        else if (CombatStateManager.inCombat)
		{
            if(CombatStateManager.whoseTurn == WhoseTurn.Lost)
            {
			    OverallUIManager.setCurrentScreenType(this);
			    HealthBarCanvas.disableHealthBarCanvas();
            } else
            {
                TutorialSequenceStepTargetUIObject.createCutOutMask(transform);
            }
		}

        saveButton.gameObject.SetActive(!Flags.isInNewGameMode() && !CombatStateManager.inCombat);

        base.Awake();

        setSaveButtonInteractibility();
	}

    protected override void Start()
    {
        //Empty On Purpose
    }

	void Update()
	{
		KeyPressManager.updateKeyBools();

		if (Input.GetKey(KeyBindingList.acceptInputKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
		{
			KeyPressManager.handlingPrimaryKeyPress = true;
			saveButtonPress();
		}

    }

	public static bool saveNameFieldIsSelected()
	{
        bool isSelected = EventSystem.current != null && getInstance() != null &&
				EventSystem.current.currentSelectedGameObject == getInstance().saveNameField.gameObject;

		return isSelected;
	}

	public static bool nameMeetsAutosaveCriteria(string saveName)
	{
		return saveName.Equals(autoSave1Name) ||
			   saveName.Equals(autoSave2Name) ||
			   saveName.Equals(autoSave3Name);
	}

	public static bool cannotSaveInCurrentState()
	{
		return Flags.getFlag(FlagNameList.newGameFlagName) || CombatStateManager.inCombat;
	}

    public void removeInvalidFileNameCharacter()
    {
        string saveName = saveNameField.text;

        if(saveName.Length == 0)
        {
            return;
        }

        switch(saveName[saveName.Length-1])
        {
            case '<':
            case '>':
            case ':':
            case '"':
            case '/':
            case '\\':
            case '|':
            case '?':
            case '*':
            case '.':
            saveNameField.text = saveNameField.text.Substring(0, saveNameField.text.Length-1);
                break;
            default:
                return;
        }
    }

	public void setSaveButtonInteractibility()
	{
		if (saveNameField.text.Length <= 0 || saveNameIsInvalid(saveNameField.text))
		{
			saveButton.interactable = false;
		}
		else
		{
			saveButton.interactable = true;
		}
	}

    public static bool saveNameIsInvalid(string saveName)
    {
        switch(saveName.ToUpperInvariant())
        {
            case "CON":
            case "PRN":
            case "AUX":
            case "NUL":
            case "COM1":
            case "COM2":
            case "COM3":
            case "COM4":
            case "COM5":
            case "COM6":
            case "COM7":
            case "COM8":
            case "COM9":
            case "COM¹":
            case "COM²":
            case "COM³":
            case "LPT1":
            case "LPT2":
            case "LPT3":
            case "LPT4":
            case "LPT5":
            case "LPT6":
            case "LPT7":
            case "LPT8":
            case "LPT9":
            case "LPT¹":
            case "LPT²":
            case "LPT³":
                return true;
            default:
                return false;
        }
    }

	public void saveButtonPress()
	{
		if(saveNameField.text.Length <= 0 || 
			saveNameField.text.Length > saveNameCharacterLimit || 
            Flags.isInNewGameMode())
		{
			return;
		}
		
        if(saveGameList.ContainsKey(saveNameField.text))
        {
            overwriteButton.spawnPopUp();
        } else
        {
            save(saveNameField.text); 
        }
	}

	private static string determineCurrentAutosaveName()
	{
        if (!saveExists(autoSave1Name))
        {
            return autoSave1Name;
        } else if (!saveExists(autoSave2Name))
		{
			return autoSave2Name;
		} else if (!saveExists(autoSave3Name))
		{
			return autoSave3Name; 
		}

		List<SaveBlueprint> autosaveBlueprints = new List<SaveBlueprint>();

		foreach(KeyValuePair<string, SaveBlueprint> kvp in saveGameList)
		{
			if(kvp.Value.isAutosave())
			{
				autosaveBlueprints.Add(kvp.Value);
			}
		}

        SaveBlueprint oldestAutosave = autosaveBlueprints[0];

		foreach(SaveBlueprint autosaveBlueprint in autosaveBlueprints)
		{
			if(Math.Abs(autosaveBlueprint.getNumber()) < Math.Abs(oldestAutosave.getNumber()))
			{
				oldestAutosave = autosaveBlueprint;
            }
		}

        return oldestAutosave.getName();
	}

    public static void autosave(Transition transition)
    {
        if(transition == null || !transition.allowAutosave)
        {
            return;
        }

        autosave(transition.getPositionOnSaveMultiplier(), CharacterFacing.getOpposingFacing(transition.playerSpawnDirection));
    }

    public static void autosave(Vector3Int cellCoords, Facing facing)
    {
		int saveNumber = getHighestSaveNumber() + 1;

        saveNumber *= -1;

        Vector3 position = AreaManager.getMasterGrid().GetCellCenterWorld(cellCoords);
		SaveBlueprint blueprint = SaveBlueprint.build(determineCurrentAutosaveName(), saveNumber);

		blueprint.playerPosition = new float[] { position.x, position.y, position.z};
        blueprint.playerFacing = (int) facing;

        createSave(blueprint);
    }

	public static SaveBlueprint save(string saveName)
	{
		return save(saveName, false);
	}

	public static SaveBlueprint save(string saveName, bool skipFileCreation)
	{
		int saveNumber;

		if (saveGameList.Count == 0)
		{
			saveNumber = 1;
		}
		else
		{
			saveNumber = getHighestSaveNumber() + 1;
		}

		SaveBlueprint blueprint = SaveBlueprint.build(saveName, saveNumber);

        if(!skipFileCreation)
        {
            createSave(blueprint);
        }

		return blueprint;
	}

	public static int getHighestSaveNumber()
	{
		int saveNumber = 0;

		if (saveGameList.Count == 0)
        {
            saveNumber = 1;
        }
        else
        {
			foreach (KeyValuePair<string, SaveBlueprint> kvp in saveGameList)
			{
				if (kvp.Value.getNumber() > saveNumber)
				{
					saveNumber = kvp.Value.getNumber();
				}
			}
        }

		return saveNumber;
	}

	public static void quickSave()
	{
		SaveBlueprint quicksave = save(quickSaveName + " " + getNextQuickSaveNumber());

		NotificationManager.spawnQuickSaveNotification(quicksave);
	}

	public static int getNextQuickSaveNumber()
	{
		int quickSaveNumber = 1;

		if (saveGameList.Count == 0)
		{
			return quickSaveNumber;
		}
		else
		{
			foreach (KeyValuePair<string, SaveBlueprint> kvp in saveGameList)
			{
				if (kvp.Value.getName().Contains(quickSaveName))
				{
					quickSaveNumber++;
				}
			}
		}

		return quickSaveNumber;
	}

	public static bool saveExists(string saveName)
	{
		return saveGameList.ContainsKey(saveName);
	}

    public static SaveBlueprint getCurrentSave()
	{
        if(instance == null)
        {
            return null;
        }

		return instance.currentSaveFile;
	}
	
	public void setInputFieldToSaveName(TextMeshProUGUI saveNameText)
	{
		saveNameField.text = saveNameText.text;
	}

	public static void createSave(SaveBlueprint blueprint)
	{
		string json = JsonConvert.SerializeObject(blueprint);

		File.WriteAllText(PrefabNames.savesFolder + blueprint.saveName + Constants.jsonFileExtension, json);

		createSavedGameList();

        OnSaveCreated.Invoke(blueprint);
	}

	public static SaveBlueprint getCleanSlateSave()
	{
		return SaveBlueprint.build(Resources.Load<TextAsset>(cleanSlateSaveName));
	}	

	public static void quickLoadTopSave()
	{
		if (saveGameList == null || saveGameList.Count == 0)
		{
			createSavedGameList();
		}

		if(saveGameList.Count == 0)
		{
			return;
		}

    	SaveBlueprint topSave = saveGameList.Values.OrderByDescending(blueprint => blueprint.getNumber()).First();

		new LoadSaveFile(topSave).execute();
	}	

    public static void deleteSaveFile(string saveFileName)
    {
        File.Delete(PrefabNames.savesFolder + saveFileName + Constants.jsonFileExtension);

        OnSaveDeleted.Invoke(saveGameList[saveFileName]);
        
        saveGameList.Remove(saveFileName);
	}
	
	public static List<IDescribable> getSaveGameList()
	{
		List<IDescribable> saveGameBluepreints = new List<IDescribable>();

		if (saveGameList == null)
		{
			createSavedGameList();
		}

		foreach (KeyValuePair<string, SaveBlueprint> kvp in saveGameList)
		{
			saveGameBluepreints.Add(kvp.Value);
		}

		return saveGameBluepreints;
	}

    private static void moveSaveFilesToSaveFolder() // Temporary
    {
        string[] filesInRootFolder = Directory.GetFiles(PrefabNames.rootGameFolder);
    
        foreach(string filePath in filesInRootFolder)
        {
            if (!Json.fileIsJson(filePath))
			{
				continue;
			}

            string[] filePathParts = filePath.Split("/");

            try
            {
                File.Move(filePath, PrefabNames.savesFolder + filePathParts[filePathParts.Length-1]);
            } catch(Exception e)
            {
                
            }
        }
    }

	public static void createSavedGameList(bool skipInvokeCall = false) //side effect: will update all saveblueprint.saveName's
	{
        moveSaveFilesToSaveFolder();

		string[] saveFiles = Directory.GetFiles(PrefabNames.savesFolder);
		saveGameList = new Dictionary<string, SaveBlueprint>();

		foreach (string saveFilePath in saveFiles)
		{
			if (!String.Equals(saveFilePath.Split(".")[1], Constants.jsonFileExtensionWithoutPeriod, StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}

			// string[] saveFilePathParts = saveFilePath.Split("/");
			// string saveFileName = saveFilePathParts[saveFilePathParts.Length - 1];

			SaveBlueprint blueprint = Json.getObjectFromJSON<SaveBlueprint>(saveFilePath);

            if(blueprint == null)
            {
                continue;
            }

			// blueprint.saveName = saveFileName.Replace(Constants.jsonFileExtension, ""); 

			if (nameMeetsAutosaveCriteria(blueprint.saveName) && blueprint.saveNumber >= 0)
			{
				blueprint.saveNumber *= -1;
			}
			else if (!nameMeetsAutosaveCriteria(blueprint.saveName))
			{
				blueprint.saveNumber = Math.Abs(blueprint.saveNumber);
			}

			/*
			int blueprintIndex;

			for(blueprintIndex = 0; blueprintIndex < saveGameList.Count; blueprintIndex++)
			{
				SaveBlueprint currentListedBlueprint = (SaveBlueprint) listOfBlueprints[blueprintIndex];

				if(blueprint.getNumber() < currentListedBlueprint.getNumber())
				{
					break;
				}
			}*/

			saveGameList[blueprint.saveName] = blueprint;
		}

        if(!skipInvokeCall)
        {
            OnScreenInteriorUpdate.Invoke();
        }
	}

    public void handleEscapePress()
	{
		if (Flags.isInNewGameMode() && OverallUIManager.UIParentPanel && gameObject)
		{
            AudioManager.playChangeScreenSFX();
            OverallUIManager.UIParentPanel.SetActive(false);
            Destroy(gameObject);
		} else if(CombatStateManager.inCombat && CombatStateManager.whoseTurn != WhoseTurn.Lost)
        {
            EscapeStack.removeTopObjectFromStack();
            Destroy(gameObject);
        }
	}

    public override void updateCounter()
    {
        saveNameField.text = "";
        GridRow.OnDescribableToDisplay.Invoke(null);
    }

    public override List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        listOfEvents.Add(OnScreenInteriorUpdate);

        return listOfEvents;
    }

    public override bool requiresPartyMemberSelectionGrid()
    {
        return false;
    }

    public override DescribableList getDefaultDescribableList()
    {
        return DescribableList.Saves;
    }

    public override void addListeners()
    {
        base.addListeners();
        GridRow.OnDescribableToDisplay.AddListener(setCurrentSaveFile);
    }
    public override void removeListeners()
    {
        base.removeListeners();
        GridRow.OnDescribableToDisplay.RemoveListener(setCurrentSaveFile);
    }

    public void setCurrentSaveFile(IDescribable describable)
    {
        if(describable as SaveBlueprint == null)
        {
            return;
        }

        currentSaveFile = describable as SaveBlueprint;
    }
    
    public override KeyCode getExitKeyCode()
    {
        return KeyBindingList.loadScreenKey.getCurrentKeyCode();
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeSaveHandler()
    {
        if(!Directory.Exists(PrefabNames.savesFolder))
        {
            Directory.CreateDirectory(PrefabNames.savesFolder);
        }
    }
}
