using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Linq;

// [System.Serializable]
public class SaveHandler : ScreenManager, IEscapable
{
	private static Dictionary<string, SaveBlueprint> saveGameList;

    public const int saveNameCharacterLimit = 20;

	public const string autoSave1Name = "Autosave 1";
    public const string autoSave2Name = "Autosave 2";
    public const string autoSave3Name = "Autosave 3";
	public const string quickSaveName = "Quicksave";

	public const string cleanSlateSaveName = "cleanSlateSave";

    private static readonly Regex sWhitespace = new Regex(@"\s+");
	
	public static string ReplaceWhitespace(string input, string replacement) 
	{
		return sWhitespace.Replace(input, replacement);
	}

	private static string fileExtension = ".json";
    private static string fileExtensionWithoutPeriod = "json";

    //[SerializeField] 
    public TMP_InputField saveNameField;
    //[SerializeField] 
    public GameObject newSaveInputPanel; //panel behind saveNameField
	//[SerializeField] 
    public Button saveButton;
	
	public BinaryPanelPopUpButton overwriteButton;

	private static SaveHandler instance;
	
	public static SaveHandler getInstance()
	{
		return instance;
	}

	private void Awake()
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

		if (CombatStateManager.inCombat)
		{
			OverallUIManager.setCurrentScreenType(this);
			HealthBarCanvas.disableHealthBarCanvas();
		}
	}

	void Update()
	{
		KeyPressManager.updateKeyBools();

		if (Input.GetKey(KeyBindingList.acceptInputKey) && !KeyPressManager.handlingPrimaryKeyPress)
		{
			KeyPressManager.handlingPrimaryKeyPress = true;
			saveButtonPress();
		}

    }

	public static bool saveNameFieldIsSelected()
	{
		return EventSystem.current != null && getInstance() != null &&
				EventSystem.current.currentSelectedGameObject == getInstance().saveNameField.gameObject;
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

	public static bool ignoreNavigationKeyPresseDuringInputFieldSelection()
	{
		if(getInstance() == null || EventSystem.current == null)
		{
			return false;
		}

		return EventSystem.current.currentSelectedGameObject == getInstance().saveNameField.gameObject;
	}

	public void setSaveButtonInteractibility()
	{
		if (saveNameField.text.Length <= 0)
		{
			saveButton.interactable = false;
		}
		else
		{
			saveButton.interactable = true;
		}
	}

	public void saveButtonPress()
	{
		if(saveNameField.text.Length <= 0 || 
			saveNameField.text.Length > saveNameCharacterLimit)
		{
			return;
		}
		
		if(grids[0].contains(saveNameField.text))
		{
			overwriteButton.spawnPopUp(new OverwriteSaveFile(saveNameField.text));
		} else
		{
			save(saveNameField.text);
			
			populateAllGridsEnableAllRows();
			hideCurrentDescriptionPanel();
			
			saveNameField.text = "";
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

		ArrayList autosaveBlueprints = new ArrayList();

		foreach(KeyValuePair<string, SaveBlueprint> kvp in saveGameList)
		{
			if(kvp.Value.isAutosave())
			{
				autosaveBlueprints.Add(kvp.Value);
			}
		}

        SaveBlueprint oldestAutosave = (SaveBlueprint) autosaveBlueprints[0];

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
		int saveNumber = getHighestSaveNumber() + 1;

        saveNumber *= -1;

        Vector3 position = AreaManager.getMasterGrid().GetCellCenterWorld(transition.getOutPutCellCoords());
		SaveBlueprint blueprint = SaveBlueprint.build(determineCurrentAutosaveName(), saveNumber);

		blueprint.playerPosition = new float[] { position.x, position.y, position.z};
        blueprint.playerFacing = (int) CharacterFacing.getOpposingFacing(transition.playerSpawnDirection);

        createSave(blueprint);
    }

	public static SaveBlueprint save(string saveName)
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

		//Flags.printAll();

		SaveBlueprint blueprint = SaveBlueprint.build(saveName, saveNumber);

		createSave(blueprint);

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

	public void removeOldSaveInfoFromScreen() //used when checking for nonexistant save names in save input field
	{
		string possibleSaveName = saveNameField.text;

		if(!saveExists(possibleSaveName))
		{
			hideCurrentDescriptionPanel();
		}
	}

    public static string getCurrentSaveName()
	{
		return getInstance().descriptionPanelSlots[0].getCurrentDescribables()[0].getName();
	}
	
	public void setInputFieldToSaveName(TextMeshProUGUI saveNameText)
	{
		saveNameField.text = saveNameText.text;
	}

	public static void createSave(SaveBlueprint blueprint)
	{
		string json = JsonConvert.SerializeObject(blueprint);

		File.WriteAllText(Application.persistentDataPath + "/" + blueprint.saveName + fileExtension, json);

		createSavedGameList();
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

		new LoadSaveFile(topSave.saveName).execute();
	}	

	public static SaveBlueprint getDataFromSaveFile(string saveName)
	{
		// if this method is ever edited to somehow return the wrong save file, it will torpedo every single save file in the save folder

		if (File.Exists(Application.persistentDataPath + "/" + saveName + fileExtension))
		{
			string jsonString = File.ReadAllText(Application.persistentDataPath + "/" + saveName + fileExtension);

			return JsonConvert.DeserializeObject<SaveBlueprint>(jsonString);

		}
		else
		{
			Debug.LogError("Save File not found: " + Application.persistentDataPath + "/" + saveName + fileExtension);
			return null;
		}
	}

    public static void deleteSaveFile(string saveFileName)
    {
        File.Delete(Application.persistentDataPath + "/" + saveFileName + fileExtension);

        saveGameList.Remove(saveFileName);
	}
	
	public static ArrayList getSaveGameList()
	{
		ArrayList saveGameArrayList = new ArrayList();

		if (saveGameList == null)
		{
			createSavedGameList();
		}

		foreach (KeyValuePair<string, SaveBlueprint> kvp in saveGameList)
		{
			saveGameArrayList.Add(kvp.Value);
		}

		return saveGameArrayList;
	}

	public static void createSavedGameList() //side effect: will update all saveblueprint.saveName's
	{
		string[] saveFiles = Directory.GetFiles(Application.persistentDataPath + "/");
		saveGameList = new Dictionary<string, SaveBlueprint>();

		foreach (string saveFilePath in saveFiles)
		{
			if (!String.Equals(saveFilePath.Split(".")[1], fileExtensionWithoutPeriod, StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}

			string[] saveFilePathParts = saveFilePath.Split("/");
			string saveFileName = saveFilePathParts[saveFilePathParts.Length - 1];

			SaveBlueprint blueprint = getDataFromSaveFile(saveFileName.Split(".")[0]);

			blueprint.saveName = saveFileName.Replace(fileExtension, ""); 

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

			saveGameList.Add(blueprint.saveName, blueprint);
		}
	}
	
	public override void revealDescriptionPanelSet(IDescribable objectToDescribe)
	{
		base.revealDescriptionPanelSet(objectToDescribe);
		
		saveNameField.text = objectToDescribe.getName();			
	}

    public void handleEscapePress()
	{
		if (Flags.isInNewGameMode() && OverallUIManager.UIParentPanel && gameObject)
		{
            OverallUIManager.UIParentPanel.SetActive(false);
            Destroy(gameObject);
		}
	}
}
