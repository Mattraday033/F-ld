using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public static class PrefabNames
{
    #region UI
    public const string inventoryScreen = "Inventory Screen Revision"; //Inventory Screen
    public const string characterScreen = "Character Screen Revision"; // Character Screen
    public const string partyScreen = "Party Screen";
    public const string journalScreen = "Journal Screen";
    public const string saveScreen = "Save Screen";
    public const string settingsScreen = "Settings Screen";

    public const string armorDescPanelFull = "Armor Description Panels";
    public const string weaponDescPanelFull = "Weapon Description Panels";
    public const string offHandWeaponDescPanelFull = "Off Hand Weapon Description Panels";
    public const string treasureEssentialDescPanelFull = "Treasure_Essential Item Description Panels";
    public const string useItemDescPanelFull = "Use Item Description Panels";
    public const string combatUsableUseItemDescPanelFull = "Combat Usable Use Item Description Panels";

    public const string offhandHoverDescriptionPanel = "Off Hand Hover Description Panel";
    public const string actionHoverDescriptionPanel = "Action Hover Description Panel";
    public const string harmlessCombatActionHoverDescriptionPanel = "Harmless Action Hover Description Panel";

    public const string itemDecisionButtons = "Item Decision Buttons";
    public const string equippableDecisionButtons = "Equippable Decision Buttons";

    public const string inventoryRow = "Inventory Row";
    public const string chestDescriptionPanel = "Chest Item Description Panel";
    public const string shopRow = "Shop Item Row";
    public const string amountPanel = "Amount Panel";

    public const string actionRow = "Action Row Description Panel";
    public const string actionEditorRow = "Action Secondary Row Description Panel";
    public const string playerAbilityRow = "Player Ability Row Description Panel";
    public const string multiStackableAbilityRow = "MultiStackable Ability Row Description Panel";
    public const string companionAbilityRow = "Companion Ability Row Description Panel";
    public const string companionCombatActionDescriptionPanels = "Companion Action Description Panels";
    public const string combatCombatActionOrderRow = "Combat Action Row";

    public const string actionDescPanelFull = "Action Description Panels";
    public const string noDamageCombatActionDescPanelFull = "No Damage Action Description Panels";
    public const string noDamageCombatActionDescPanelRow = "No Damage Action Level Up Row";
    public const string multiStackableNoDamageActionLevelUpRow = "MultiStackable No Damage Action Level Up Row";
    public const string multiStackableNoDamageActionDescriptionPanels = "MultiStackable No Damage Action Description Panels";
    public const string itemCombatActionDescPanelFull = "Item Action Description Panels";
    public const string dualWieldCombatActionDescPanelFull = "Dual Wield Weapon Action Description Panels";

    public const string singleEditAbilityWheelPopUp = "Single Edit Ability Wheel PopUp";
    public const string abilityWheelEditorFull = "Full Edit Ability Wheel Popup";

    public const string outOfCombatHealPartyMemberPopUp = "Healing Party Members Screen";

    public const string dragAndDropCombatActionIcon = "Drag And Drop Action Icon";
    public const string slotIcon = "Slot Icon";

    public const string saveRow = "Save Row";
    public const string saveLoadPanelFull = "SaveDescriptionPanel";
    public const string loadOverwriteDeleteDecisionPanel = "Save Decision Panel";
    public const string loadDecisionPanel = "Load Only Decision Panel";
    public const string loadOnlyPopUp = "Load Screen PopUp";

    public const string bookPopUpWindow = "Book PopUp Window";

    public const string partyMemberRow = "Party Member Row";
    public const string partyMemberSpriteRow = "Party Member Sprite Row";
    public const string party2x3GridSection = "2x3 Party Grid Section";
    public const string formationEditorRow = "Party Member Formation Editor Row";
    public const string partyMemberDescriptionPanel = "Party Member Description Panel";

    public const string glossaryCategoryNameFull = "Glossary Category Name Full";
    public const string glossaryCategoryRow = "Glossary Category Row";
    public const string glossaryEntryRow = "Glossary Entry Row";
    public const string mapQuestObjectiveRow = "Map Quest Objective Row";
    public const string mapQuestObjectiveRowWithoutHover = "Map Quest Objective Row Without Hover";
    public const string multiStackPerkEntryRow = "MultiStack Trait Perk Entry Row";
    public const string gridGlossaryEntryFull = "Grid Glossary Entry Full";
    public const string writtenGlossaryEntryFull = "Written Glossary Entry Full";
    public const string perkDescriptionPanelFull = "Perk Description Panel";
    public const string passivePerkDescriptionPanel = "Passive Perk Description Panel";
    public const string multiStackPassivePerkDescriptionPanel = "MultiStack Passive Perk Description Panel";

    public const string dialogueLineRow = "Dialogue Line Row";
    public const string choiceRow = "Choice";
    public const string unimplementedChoice = "Unimplemented Choice";
    public const string dialogueTrackerWindowPopUp = "Dialogue Tracker Window";
    public const string dialogueTrackerWindowWithChoicesPopUp = "Dialogue Tracker Window With Choices";

    public const string areaNameDescriptionPanel = "Area Name Description Panel";
    public const string notificationDescriptionPanel = "Notification Description Panel";
    public const string questStepNotificationDescriptionPanel = "Quest Step Notification Description Panel";
    public const string questStepNotificationDescriptionPanelNoFadeIn = "Quest Step Notification Description Panel No Fade In";

    public const string hoverPanelPopUpWindow = "Combatant Hover Panel";
    public const string statsDescriptionPanel = "Stats Description Panel";
    public const string partyMemberStatsScreenDescPanel = "Party Member Screen Stats Description Panel";
    public const string partyScreenMainDescPanel = "Party Screen Main Description Panel";

    public const string levelUpPopUpWindow = "LevelUp PopUp Window";
    public const string characterCreationPopUpWindow = "Character Creation PopUp Revision";
    public const string actionLevelUpDescriptionPanels = "Action LevelUp Description Panels";
    public const string skillLevelUpDescriptionPanels = "Skill LevelUp Description Panels";

    public const string shopPopUpWindow = "Shop PopUp Window Revision";

    public const string gameOverPopUpWindow = "Game Over PopUp Window";

    public const string notificationPopUpWindow = "Notification PopUp Window";
    public const string notificationPopUpButton = "Notification PopUp Button";

    public const string tutorialPopUpWindow = "Tutorial PopUp Window";
    public const string tutorialMessageWithImage = "Tutorial Message Panel With Image";
    public const string tutorialMessageWithoutImage = "Tutorial Message Panel Without Image";
    public const string tutorialSequencePopUpDescriptionPanel = "Tutorial Sequence Pop Up Description Panel";
    public const string tutorialSequencePopUpDescriptionPanelUltraWide = "Tutorial Sequence Pop Up Description Panel Ultra Wide";
    public const string tutorialSequencePopUpDescriptionPanelUI = "UI Targeting Tutorial Sequence Pop Up Description Panel";
    public const string cutOutMask = "Cut Out Mask";
    public const string hoverIconDescriptionPanel = "Hover Icon Description Panel";
    public const string hoverIconDescriptionPanelInterior = "Hover Icon Description Panel Interior";
    public const string hoverIconCombatActionDescriptionPanel = "Hover Icon Combat Action Description Panel";

    public const string traitSquareRowPanel = "Trait Square Row Panel";
    public const string stackableTraitSquareRowPanel = "Stackable Trait Square Row Panel";
    public const string multiStackableTraitSquareRowPanel = "MultiStackable Trait Square Row Panel";
    public const string traitHoverDescriptionPanel = "Trait Hover Description Panel";
    public const string multiStackableTraitHoverDescriptionPanel = "MultiStackable Trait Hover Description Panel";

    public const string combatResultsPopUp = "Combat Results PopUp";

    public const string popUpScreenBlocker = "PopUp Screen Blocker";

    public const string binaryDecisionPanel = "Binary Decision Panel";

    public const string formationEditorPanel = "Formation Editor Panel";

    public const string intimidateTileName = "Intimidate Cunning Indicator";
    public const string cunningTileName = "Intimidate Cunning Indicator";
    public const string observationTileName = "Observation Indicator";
    public const string leadershipTileName = "Leadership Indicator";

    public const string npcNameTag = "NPC Name Tag";
    public const string oldNPCNameTag = "OLD NPC Name Tag";

    public const string mapTileName = "Map Tile";
    public const string nonInteractableMapTileName = "NonInteractable Map Tile";
    public const string mapPopUpWindow = "Map PopUp Window";

    public const string descriptionPanelBuilder = "Description Panel Builder";
    public const string hoverDescriptionPanelBuilder = "Hover Description Panel Builder";
    public const string combatStatsHoverDescriptionPanelBuilder = "Combat Stats Hover Description Panel Builder";
    public const string combatActionHoverDescriptionPanelBuilder = "Combat Action Hover Description Panel Builder";
    public const string statsDescriptionPanelBuilder = "Stats Description Panel Builder";
    public const string statsUpgradeDescriptionPanelBuilder = "Stats Upgrade Description Panel Builder";
    public const string playerSideStatsDescriptionPanelBuilder = "Player Side Stats Description Panel Builder";

    public const string descriptionPanelBuildingBlockName = "Name Building Block";
    public const string descriptionPanelBuildingBlockIcon = "Icon Building Block";
    public const string descriptionPanelBuildingBlockText = "Text Building Block";
    public const string descriptionPanelBuildingBlockLargeText = "Large Text Building Block";
    public const string descriptionPanelBuildingBlockPrimaryStat = "Primary Stat Building Block";
    public const string descriptionPanelBuildingBlockRange = "Range Building Block";
    public const string descriptionPanelBuildingBlockDamageText = "Damage Text Building Block";
    public const string descriptionPanelBuildingBlockBonusDamageText = "Bonus Damage Text Building Block";

    public const string combatPressEPrompt = "Combat Press E Prompt";

    public const string targetCanvas = "Target Canvas";
    public const string targetBox = "Target Box";
    public const string targetCombatTile = "Target Combat Tile";

    public const string mouseHoverBase = "Mouse Hover Base";
    public const string mouseHoverTag = "Mouse Hover Tag";
    public const string partyMemberSpriteDragAndDrop = "Party Member Sprite Drag And Drop";
    public const string dragAndDropActionIcon = "Drag And Drop Action Icon";
    public const string dragAndDropItemIcon = "Drag And Drop Item Icon";
    public const string dragAndDropItemShopIcon = "Drag And Drop Item Shop Icon";

    #endregion

    #region Interactable Game Objects
    public const string interactablesFolder = "Interactables/";
    public const string NPC = interactablesFolder + "NPC";
    public const string transitionSpace = interactablesFolder + "Transition Space";
    public const string vaultableObject = interactablesFolder + "VaultableObject";
    public const string chest = interactablesFolder + "Chest";
    public const string oocMonster = interactablesFolder + "OOC Monster";

    #endregion

    #region Combat

    public const string combatFolder = "Combat/";

    public const string enemySprite = combatFolder + "Enemy Sprite";
    public const string healthBar = combatFolder + "Health Bar";

    #endregion

    #region Sprites
    public const string spriteFolder = "Sprites/";

    public const string charactersFolder = "Characters/";
    public const string humansFolder = "Humans/";
    public const string defaultNPCSprite = spriteFolder + charactersFolder + humansFolder + "NPC Sprite";

    public const string barrelsFolder = "Barrels/";
    public const string vaultableBarrels = spriteFolder + barrelsFolder + "VaultableBarrels";

    public const string chestsFolder = "Chests/";

    public const string chestBackClosed = spriteFolder + chestsFolder + "Chest_Back_Closed";
    public const string chestBackOpenFilled = spriteFolder + chestsFolder + "Chest_Back_Opened_Filled";
    public const string chestBackOpenEmpty = spriteFolder + chestsFolder + "Chest_Back_Opened_Empty";
    public const string chestFrontClosed = spriteFolder + chestsFolder + "Chest_Front_Closed";
    public const string chestFrontOpenFilled = spriteFolder + chestsFolder + "Chest_Front_Opened_Filled";
    public const string chestFrontOpenEmpty = spriteFolder + chestsFolder + "Chest_Front_Opened_Empty";

    #endregion
}
