using Newtonsoft.Json;
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
    private const string characterSpriteExampleName = "CharGen";

    private static CharacterCreationPopUpWindow instance;
    public readonly static string[] portraitSpriteNameList = new string[]{ NPCNameList.protagPrefix+1, NPCNameList.protagPrefix+2 };

    public TMP_InputField nameField;

    public AllyStats currentStats;

    public Button[] pageButtons;

    public ThreeRingButton plusButton;
    public ThreeRingButton minusButton;

    public int pointsToSpend;
    public int pointsSpent;

    public TextMeshProUGUI pointsToSpendDisplay;
    public TextMeshProUGUI statPageTitle;
    public TextMeshProUGUI statAmount;

    public TextMeshProUGUI statCombatDescription;
    public TextMeshProUGUI statDialogueDescription;
    public TextMeshProUGUI statMobilityDescription;

    public SlotIconHover statIcon;

    public PrimaryStat currentPrimaryStatPage = PrimaryStat.Strength;

    public PrimaryStatsPanel primaryStatsPanel;

    public int portraitNameIndex = 0;
    public int spriteNameIndex = 0;

    public Image portraitImage;
    public Image spriteImage;

    public Canvas windowCanvas;

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
        AudioManager.playChangeScreenSFX();
    }

    void Update() //here for Key Input
    {
        KeyPressManager.updateKeyBools();

        if ((Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Return)) && !KeyPressManager.handlingPrimaryKeyPress && inNameInputField())
        {
            KeyPressManager.handlingPrimaryKeyPress = true;

            handleESCPress();
        }
    }

    private void handleESCPress()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void selectNameField()
    {
        EventSystem.current.SetSelectedGameObject(nameField.gameObject);
    }

    public void setPage(int primaryStat)
    {
        currentPrimaryStatPage = (PrimaryStat) primaryStat;

        populate();
    }

    public void incrementStat()
    {
        pointsToSpend--;
        pointsSpent++;

        switch (currentPrimaryStatPage)
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
            default:
                currentStats.charisma = currentStats.getCharisma() + 1;
                break;
        }

        populate();
    }

    public void decrementStat()
    {
        pointsToSpend++;
        pointsSpent--;

        switch (currentPrimaryStatPage)
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
            default:
                currentStats.charisma = currentStats.getCharisma() - 1;
                break;
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
        setAcceptButtonInteractability();

        setInteractability();

        pointsToSpendDisplay.text = "" + pointsToSpend;

        updatePortraitSpriteImages();

        statPageTitle.text = currentPrimaryStatPage.ToString() + ":";
        statAmount.text = "" + getStatAmount();

        showPageDetails();

        statIcon.setHoverMessage(currentPrimaryStatPage.ToString(), HoverMessageList.getMessage(currentPrimaryStatPage.ToString()));
        statIcon.iconImage.sprite = Helpers.loadSpriteFromResources(currentPrimaryStatPage.ToString());

        primaryStatsPanel.updateStatsPanel(currentStats);
    }

    public void showPageDetails()
    {
        statCombatDescription.text = currentPrimaryStatPage.getPrimaryStatCharGenCombatDescription();
        statDialogueDescription.text = currentPrimaryStatPage.getPrimaryStatCharGenDialogueDescription();
        statMobilityDescription.text = currentPrimaryStatPage.getPrimaryStatCharGenMobilityDescription();
    }

    private void updatePortraitSpriteImages()
    {
        portraitImage.sprite = getPortrait();
        spriteImage.sprite = getSprite();
    }

    public void setInteractability()
    {
        setPageButtonInteractability();

        setPlusMinusButtonInteractability();
    }

    public void setPageButtonInteractability()
    {
        for(int i = 0; i < pageButtons.Length; i++)
        {
            pageButtons[i].interactable = i != (int) currentPrimaryStatPage;
        }
    }

    public void setPlusMinusButtonInteractability()
    {
        plusButton.interactable = pointsToSpend > 0;

        minusButton.interactable = getStatAmount() > 1;
    }

    public int getStatAmount()
    {
        int[] statsAsArray = currentStats.getStatsAsArray();

        return statsAsArray[(int) currentPrimaryStatPage];
    }

    private const float newGameFadeOutDuration = 6f;
    private bool isFadingToMonologue;

    public void newGameSetCharacterNameAndStats()
    {
        if (isFadingToMonologue)
        {
            return;
        }

        isFadingToMonologue = true;
        StartCoroutine(fadeOutThenStartMonologue());
    }

    private IEnumerator fadeOutThenStartMonologue()
    {
        // if(Application.isEditor)
        // {
        //     applyCharacterChoicesAndStartMonologue();
        //     yield break;
        // }

        FadeToBlackManager.StopFade(FadeType.Screen);
        FadeToBlackManager.StopFade(FadeType.Music);

        FadeToBlackTransition fadeOut = new(skipFadeIn: true);
        fadeOut.fadeTime = 4.5f;
        FadeToBlackManager.createFade(fadeOut);

        AudioManager.setMusicSourceVolume(0f);

        AudioManager.playGongSFX();

        yield return null;

        hideWindow();

        yield return new WaitForSeconds(5.5f);

        applyCharacterChoicesAndStartMonologue();
    }

    private void applyCharacterChoicesAndStartMonologue()
    {
        string name = nameField.text;

        if (name.Equals(""))
        {
            name = SaveDefaultValues.defaultPlayerName;
        }

        PartyMember player = new PartyMember(new AllyStats(name + PartyManager.playerMarker, currentStats.getStrength(), currentStats.getDexterity(), currentStats.getWisdom(), currentStats.getCharisma()));
        player.canJoinParty = true;
        player.stats.combatActionArray = new CombatActionArray(player.stats, getStartingActions(player.stats));

        PartyManager.resetPartyMembers();
        PartyManager.addPlayerStatsToDict(player.stats);

        SaveBlueprint cleanSaveBlueprint = SaveHandler.getCleanSlateSave();
        cleanSaveBlueprint.playerPortraitName = portraitSpriteNameList[portraitNameIndex];
        cleanSaveBlueprint.playerSpriteName = portraitSpriteNameList[spriteNameIndex];
        cleanSaveBlueprint.partyMemberStats = new StatsWrapper[]{ new StatsWrapper(player)};

        Dictionary<string, bool> startingFlags = new Dictionary<string, bool>();
        startingFlags[Flags.getStatTutorialFlag(player.stats)] = true;
        cleanSaveBlueprint.currentFlags = JsonConvert.SerializeObject(startingFlags, Formatting.Indented);

        LoadSaveFile loadSaveFile = new LoadSaveFile(cleanSaveBlueprint, OOCActivity.inDialogue, showMonologueFirst: true);
        loadSaveFile.execute();
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
        AudioManager.playChangeScreenSFX();
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
        return Helpers.loadSpriteFromResources(EnemyTypeFolderPathList.getEnemyTypeFolderPath(portraitSpriteNameList[spriteNameIndex])+characterSpriteExampleName);
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

    public static void hideWindow()
    {
        if(instance != null && instance.windowCanvas != null)
        {
            instance.windowCanvas.enabled = false;
        }
    }
}
