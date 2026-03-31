using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CharacterCreationPopUpWindow : PopUpWindow
{
    private static CharacterCreationPopUpWindow instance;
    public readonly static string[] portraitSpriteNameList = new string[]{ NPCNameList.thatch, NPCNameList.nandor };

    public TMP_InputField nameField;

    public AllyStats currentStats;

    public Button[] incrementButtons;
    public Button[] decrementButtons;

    public int pointsToSpend;
    public int pointsSpent;

    public TextMeshProUGUI pointsToSpendDisplay;

    public PrimaryStatsPanel primaryStatsPanel;

    public int portraitNameIndex = 0;
    public int spriteNameIndex = 0;

    public Image portraitImage;
    public Image spriteImage;

    [RuntimeInitializeOnLoadMethod]
    private static void intitializeCharacterCreationWindow()
    {
        instance = null;
    }


    public static CharacterCreationPopUpWindow getInstanceCC()
    {
        return instance;
    }

    public static bool inNameInputField()
    {
        return EventSystem.current != null && getInstanceCC() != null &&
                EventSystem.current.currentSelectedGameObject == getInstanceCC().nameField.gameObject;
    }


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Duplicate instances of CharacterCreationPopUpWindow exist erroneously");
        }

        instance = this;

        currentStats = new AllyStats("", 1, 1, 1, 1);
        pointsToSpend = 1;

        populate();
    }

    void Update() //here for Key Input
    {
        KeyPressManager.updateKeyBools();

        if ((Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Return)) && !KeyPressManager.handlingPrimaryKeyPress && inNameInputField())
        {
            KeyPressManager.handlingPrimaryKeyPress = true;

            handleESCPress();
        }

        // if (Input.GetKey(KeyCode.Return) && !KeyPressManager.handlingPrimaryKeyPress && inNameInputField())
        // {
        //     KeyPressManager.handlingPrimaryKeyPress = true;

        // 	handleESCPress();
        // }
    }

    private void handleESCPress()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void selectNameField()
    {
        EventSystem.current.SetSelectedGameObject(nameField.gameObject);
    }

    public void incrementStat(EnumButtonPasser passer)
    {
        pointsToSpend--;
        pointsSpent++;

        switch (passer.PrimaryStat)
        {
            case PrimaryStat.Strength:
                currentStats.strength = currentStats.getStrength() + 1;
                break;
            case PrimaryStat.Dexterity:
                currentStats.dexterity = currentStats.getDexterity() + 1;
                break;
            case PrimaryStat.Wisdom:
                currentStats.wisdom = currentStats.getWisdom() + 1;
                break;
            case PrimaryStat.Charisma:
                currentStats.charisma = currentStats.getCharisma() + 1;
                break;
            default:
                throw new IOException("Unknown PrimaryStat: " + passer.PrimaryStat.ToString());
        }

        populate();
    }

    public void decrementStat(EnumButtonPasser passer)
    {
        pointsToSpend++;
        pointsSpent--;

        switch (passer.PrimaryStat)
        {
            case PrimaryStat.Strength:
                currentStats.strength = currentStats.getStrength() - 1;
                break;
            case PrimaryStat.Dexterity:
                currentStats.dexterity = currentStats.getDexterity() - 1;
                break;
            case PrimaryStat.Wisdom:
                currentStats.wisdom = currentStats.getWisdom() - 1;
                break;
            case PrimaryStat.Charisma:
                currentStats.charisma = currentStats.getCharisma() - 1;
                break;
            default:
                throw new IOException("Unknown PrimaryStat: " + passer.PrimaryStat.ToString());
        }

        populate();
    }

    public void setAcceptButtonInteractability()
    {
        if (pointsToSpend == 0)
        {
            acceptButton.interactable = true;
        }
        else
        {
            acceptButton.interactable = false;
        }
    }

    public void populate()
    {
        primaryStatsPanel.updateStatsPanel(currentStats);
        setAcceptButtonInteractability();

        setInteractability();

        pointsToSpendDisplay.text = "" + pointsToSpend;

        updatePortraitSpriteImages();
    }

    private void updatePortraitSpriteImages()
    {
        portraitImage.sprite = getPortrait();
        spriteImage.sprite = getSprite();
    }

    public void setInteractability()
    {
        for (int buttonIndex = 0; buttonIndex < incrementButtons.Length && buttonIndex < decrementButtons.Length; buttonIndex++)
        {
            if (pointsToSpend > 0)
            {
                incrementButtons[buttonIndex].gameObject.SetActive(true);
            }
            else
            {
                incrementButtons[buttonIndex].gameObject.SetActive(false);
            }

            switch ((PrimaryStat) buttonIndex)
            {
                case PrimaryStat.Strength:
                    decrementButtons[buttonIndex].gameObject.SetActive(currentStats.getStrength() > 1);
                    break;
                case PrimaryStat.Dexterity:
                    decrementButtons[buttonIndex].gameObject.SetActive(currentStats.getDexterity() > 1);
                    break;
                case PrimaryStat.Wisdom:
                    decrementButtons[buttonIndex].gameObject.SetActive(currentStats.getWisdom() > 1);
                    break;
                case PrimaryStat.Charisma:
                    decrementButtons[buttonIndex].gameObject.SetActive(currentStats.getCharisma() > 1);
                    break;
            }

        }
    }

    public void newGameSetCharacterNameAndStats()
    {
        string name = nameField.text;

        if (name.Equals(""))
        {
            name = SaveDefaultValues.defaultPlayerName;
        }

        LoadSaveFile.loadCleanSlateSaveFile();

        AllyStats playerStats = new AllyStats(name + PartyManager.playerMarker, currentStats.getStrength(), currentStats.getDexterity(), currentStats.getWisdom(), currentStats.getCharisma());

        playerStats.combatActionArray = new CombatActionArray(playerStats, getStartingActions(playerStats));

        PartyManager.resetPartyMembers();
        PartyManager.addPlayerStatsToDict(playerStats);

        State.formation = new Formation();
        State.playerPortraitName = portraitSpriteNameList[portraitNameIndex];
        State.playerSpriteName = portraitSpriteNameList[spriteNameIndex];

        PlayerOOCStateManager.setCurrentActivity(OOCActivity.inDialogue);
        OverallUIManager.resetScreenStates();
    }

    public static CombatAction[] getStartingActions(AllyStats stats)
    {
        PrimaryStat chosenStat = stats.getHighestPrimaryStats()[0];

        switch (chosenStat)
        {
            case PrimaryStat.Strength:
                return Strength.getStartingActions(stats);
            case PrimaryStat.Dexterity:
                return Dexterity.getStartingActions(stats);
            case PrimaryStat.Wisdom:
                return Wisdom.getStartingActions(stats);
            case PrimaryStat.Charisma:
                return Charisma.getStartingActions(stats);
            default:
                return new CombatAction[] { new FistAttack(stats), null, null, null, null, null, null, null, null, null, null, null };
        }
    }
    public override void handleEscapePress()
    {
        destroyWindow();
    }

    public override void closeButtonPress()
    {
        base.closeButtonPress();
        StartingMenuManager.getInstance().revertToMainMenu();
    }

    public void removeInvalidNameCharacters()
    {
        string characterName = nameField.text;

        if(characterName.Length == 0)
        {
            return;
        }

        switch(characterName[characterName.Length-1])
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
            case ',':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
            case '0':
            case '!':
            case '@':
            case '#':
            case '$':
            case '%':
            case '^':
            case '&':
            case '(':
            case ')':
            case '-':
            case '_':
            case '+':
            case '=':
            case '{':
            case '}':
            case '[':
            case ']':
            case ';':
            case '\'':
            nameField.text = nameField.text.Substring(0, nameField.text.Length-1);
                break;
            default:
                return;
        }
    }

    private Sprite getSprite()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(EnemyTypeFolderPathList.getEnemyTypeFolderPath(portraitSpriteNameList[spriteNameIndex])+CharacterAnimationType.Idle_Front.ToString());

        return sprites[Constants.indexZero];
    }

    private Sprite getPortrait()
    {
        return Helpers.loadSpriteFromResources(PrefabNames.portraitFolder + portraitSpriteNameList[portraitNameIndex]);
    }

    public void incrementSpriteIndex()
    {
        if(spriteNameIndex >= portraitSpriteNameList.Length-1)
        {
            spriteNameIndex = 0;
        } else
        {
            spriteNameIndex++;
        }

        updatePortraitSpriteImages();
    }

    public void decrementSpriteIndex()
    {
        if(spriteNameIndex <= 0)
        {
            spriteNameIndex = portraitSpriteNameList.Length-1;
        } else
        {
            spriteNameIndex--;
        }

        updatePortraitSpriteImages();
    }

    public void incrementPortraitIndex()
    {
        if(portraitNameIndex >= portraitSpriteNameList.Length-1)
        {
            portraitNameIndex = 0;
        } else
        {
            portraitNameIndex++;
        }

        updatePortraitSpriteImages();
    }

    public void decrementPortraitIndex()
    {
        if(portraitNameIndex <= 0)
        {
            portraitNameIndex = portraitSpriteNameList.Length-1;
        } else
        {
            portraitNameIndex--;
        }

        updatePortraitSpriteImages();
    }
}
