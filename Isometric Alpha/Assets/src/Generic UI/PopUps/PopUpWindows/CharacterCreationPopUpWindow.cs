using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

[Serializable] public enum StatType {Str = 1, Dex = 2, Wis = 3, Cha = 4}

public class CharacterCreationPopUpWindow : PopUpWindow
{
    private static Color32 disabledColor = new Color32(125, 125, 125, 255);
    private static CharacterCreationPopUpWindow instance;

    public TextMeshProUGUI placeHolderNameText;
    public TMP_InputField nameField;

    public Image statTitleImage;
    public TextMeshProUGUI statTitle;

    public Image statDescriptionImage;
    public TextMeshProUGUI statDescription;

    public ScrollableUIElement statDescriptionScroll;

    public AllyStats currentStats;

    public Button[] incrementButtons;
    public Button[] decrementButtons;

    public int pointsToSpend;
    public int pointsSpent;

    public TextMeshProUGUI pointsToSpendDisplay;
    public TextMeshProUGUI oldLevelDisplay;
    public TextMeshProUGUI newLevelDisplay;

    public PrimaryStatsPanel primaryStatsPanel;
    public SecondaryStatsPanel secondaryStatsPanel;

    public ScrollableUIElement newAbilityGrid;

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

        switch (passer.statType)
        {
            case StatType.Str:
                currentStats.strength = currentStats.getStrength() + 1;
                break;
            case StatType.Dex:
                currentStats.dexterity = currentStats.getDexterity() + 1;
                break;
            case StatType.Wis:
                currentStats.wisdom = currentStats.getWisdom() + 1;
                break;
            case StatType.Cha:
                currentStats.charisma = currentStats.getCharisma() + 1;
                break;
            default:
                throw new IOException("Unknown StatType: " + passer.statType.ToString());
        }

        populate();
    }

    public void decrementStat(EnumButtonPasser passer)
    {
        pointsToSpend++;
        pointsSpent--;

        switch (passer.statType)
        {
            case StatType.Str:
                currentStats.strength = currentStats.getStrength() - 1;
                break;
            case StatType.Dex:
                currentStats.dexterity = currentStats.getDexterity() - 1;
                break;
            case StatType.Wis:
                currentStats.wisdom = currentStats.getWisdom() - 1;
                break;
            case StatType.Cha:
                currentStats.charisma = currentStats.getCharisma() - 1;
                break;
            default:
                throw new IOException("Unknown StatType: " + passer.statType.ToString());
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

        // if(pointsToSpend == 1)
        // {
        //     statTitleImage.color = disabledColor;
        //     statDescriptionImage.color = disabledColor;
        //     statTitle.text = "";
        //     statDescription.text = "";
        // } else
        // {
        //     statTitleImage.color = Color.white;
        //     statDescriptionImage.color = Color.white;
        //     statTitle.text = getStatTitle();
        //     statDescription.text = getStatDescription();
        // }

        // statDescriptionScroll.disableScrollCheck();
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

            switch ((StatType)buttonIndex + 1)
            {
                case StatType.Str:
                    decrementButtons[buttonIndex].gameObject.SetActive(currentStats.getStrength() > 1);
                    break;
                case StatType.Dex:
                    decrementButtons[buttonIndex].gameObject.SetActive(currentStats.getDexterity() > 1);
                    break;
                case StatType.Wis:
                    decrementButtons[buttonIndex].gameObject.SetActive(currentStats.getWisdom() > 1);
                    break;
                case StatType.Cha:
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

        AllyStats playerStats = new AllyStats(name, currentStats.getStrength(), currentStats.getDexterity(), currentStats.getWisdom(), currentStats.getCharisma());

        playerStats.combatActionArray = new CombatActionArray(playerStats, getStartingActions(playerStats));

        PartyManager.resetPartyMembers();
        PartyManager.addPlayerStatsToDict(playerStats);

        State.formation = new Formation();

        PlayerOOCStateManager.setCurrentActivity(OOCActivity.inDialogue);
        OverallUIManager.resetScreenStates();
    }

    private string getStatTitle()
    {
        PrimaryStat chosenStat = currentStats.getHighestPrimaryStats()[0];

        switch (chosenStat)
        {
            case PrimaryStat.Strength:
                return "Strength";
            case PrimaryStat.Dexterity:
                return "Dexterity";
            case PrimaryStat.Wisdom:
                return "Wisdom";
            case PrimaryStat.Charisma:
                return "Charisma";
            default:
                return "";
        }
    }

    private string getStatDescription()
    {
        PrimaryStat chosenStat = currentStats.getHighestPrimaryStats()[0];

        switch (chosenStat)
        {
            case PrimaryStat.Strength:
                return Strength.getDescription();
            case PrimaryStat.Dexterity:
                return Dexterity.getDescription();
            case PrimaryStat.Wisdom:
                return Wisdom.getDescription();
            case PrimaryStat.Charisma:
                return Charisma.getDescription();
            default:
                return "";
        }
    }

    public static CombatAction[] getStartingActions(AllyStats stats)
    {
        PrimaryStat chosenStat = stats.getHighestPrimaryStats()[0];

        switch (chosenStat)
        {
            case PrimaryStat.Strength:
                return Strength.getStartingActions();
            case PrimaryStat.Dexterity:
                return Dexterity.getStartingActions();
            case PrimaryStat.Wisdom:
                return Wisdom.getStartingActions();
            case PrimaryStat.Charisma:
                return Charisma.getStartingActions();
            default:
                return new CombatAction[] { new FistAttack(), null, null, null, null, null, null, null, null, null, null, null };
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
}
