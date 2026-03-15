using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrefabNames
{
    #region UI
    public const string screenOutline = "Screen Outline";

    public const string inventoryScreen = "Inventory Screen Revision"; //Inventory Screen
    public const string characterScreen = "Character Screen Revision"; // Character Screen
    public const string partyScreen = "Party Screen";
    public const string journalScreen = "Journal Screen";
    public const string saveScreen = "Save Screen";
    public const string settingsScreen = "Settings Screen";

    public const string healingNumbersFont = "Healing Numbers PF";
    public const string critNumbersFont = "Critical Damage Numbers PF";
    public const string damageNumbersFont = "Damage Numbers PF";

    public const string armorDescPanelFull = "Armor Description Panels";
    public const string weaponDescPanelFull = "Weapon Description Panels";
    public const string offHandWeaponDescPanelFull = "Off Hand Weapon Description Panels";
    public const string treasureEssentialDescPanelFull = "Treasure_Essential Item Description Panels";
    public const string useItemDescPanelFull = "Use Item Description Panels";
    public const string combatUsableUseItemDescPanelFull = "Combat Usable Use Item Description Panels";

    public const string offhandHoverDescriptionPanel = "Off Hand Hover Description Panel";
    public const string actionHoverDescriptionPanel = "Action Hover Description Panel";
    public const string harmlessCombatActionHoverDescriptionPanel = "Harmless Action Hover Description Panel";

    public const string inventoryRow = "Inventory Row";
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

    public const string dragAndDropCombatActionIcon = "Drag And Drop Action Icon";
    public const string slotIcon = "Slot Icon";

    public const string saveRow = "Save Row";
    public const string saveLoadPanelFull = "SaveDescriptionPanel";
    public const string loadOverwriteDeleteDecisionPanel = "Save Decision Panel";
    public const string loadDecisionPanel = "Load Only Decision Panel";

    public const string bookPopUpWindow = "Book PopUp Window";

    public const string partyMemberRow = "Party Member Row";
    public const string partyMemberSpriteRow = "Party Member Sprite Row";
    public const string party2x3GridSection = "2x3 Party Grid Section";
    public const string formationEditorRow = "Party Member Formation Editor Row";
    public const string partyMemberDescriptionPanel = "Party Member Description Panel";

    public const string glossaryCategoryNameFull = "Glossary Category Name Full";
    public const string glossaryCategoryRow = "Glossary Category Row";
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

    public const string hoverPanelPopUpWindow = "Combatant Hover Panel";
    public const string statsDescriptionPanel = "Stats Description Panel";
    public const string partyMemberStatsScreenDescPanel = "Party Member Screen Stats Description Panel";
    public const string partyScreenMainDescPanel = "Party Screen Main Description Panel";

    public const string levelUpPopUpWindow = "LevelUp PopUp Window";
    public const string characterCreationPopUpWindow = "Character Creation PopUp Revision";
    public const string actionLevelUpDescriptionPanels = "Action LevelUp Description Panels";
    public const string skillLevelUpDescriptionPanels = "Skill LevelUp Description Panels";

    public const string shopPopUpWindow = "Shop PopUp Window";

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
    public const string questListDropDownGrid = "Quest List Drop Down Grid";


    public const string traitSquareRowPanel = "Trait Square Row Panel";
    public const string stackableTraitSquareRowPanel = "Stackable Trait Square Row Panel";
    public const string multiStackableTraitSquareRowPanel = "MultiStackable Trait Square Row Panel";
    public const string multiStackableTraitHoverDescriptionPanel = "MultiStackable Trait Hover Description Panel";

    public const string characterGenerationStatDescriptionPanel = "Chargen Mouse Hover";

    public const string combatResultsPopUp = "Combat Results PopUp";
    public const string star = "Star";

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
    public const string worldMapPopUpWindow = "World Map PopUp Window";
    public const string worldMapLandmark = "World Map Landmark";

    public const string descriptionPanelBuilder = "Description Panel Builder";
    public const string hoverDescriptionPanelBuilder = "Hover Description Panel Builder";
    public const string combatStatsHoverDescriptionPanelBuilder = "Combat Stats Hover Description Panel Builder";
    public const string oocStatsHoverDescriptionPanelBuilder = "OOC Stats Hover Description Panel Builder";
    public const string combatActionHoverDescriptionPanelBuilder = "Combat Action Hover Description Panel Builder";
    public const string statsDescriptionPanelBuilder = "Stats Description Panel Builder";
    public const string statsUpgradeDescriptionPanelBuilder = "Stats Upgrade Description Panel Builder";
    public const string playerSideStatsDescriptionPanelBuilder = "Player Side Stats Description Panel Builder";

    public const string combatEscapeMenu = "Combat Escape Menu";

    public const string descriptionPanelBuildingBlockName = "Name Building Block";
    public const string descriptionPanelBuildingBlockIcon = "Icon Building Block";
    public const string descriptionPanelBuildingBlockText = "Text Building Block";
    public const string descriptionPanelBuildingBlockLargeText = "Large Text Building Block";
    public const string descriptionPanelBuildingBlockPrimaryStat = "Primary Stat Building Block";
    public const string descriptionPanelBuildingBlockRange = "Range Building Block";
    public const string descriptionPanelBuildingBlockDamageText = "Damage Text Building Block";
    public const string descriptionPanelBuildingBlockBonusDamageText = "Bonus Damage Text Building Block";
    public const string descriptionPanelBuildingBlockItem = "Combat Results Description Panel";

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

    public const string outlineMaterial = "Sprite-Outline-Material";

    #endregion

    public readonly static string rootGameFolder = Application.persistentDataPath + "/";

    #region Saves
    public readonly static string savesFolder = rootGameFolder + "Saves/";

    #endregion

    #region Config
    public readonly static string configFolder = rootGameFolder + "Config/";
    public readonly static string configFile = configFolder + "Config" + Constants.jsonFileExtension;

    #endregion

    #region UI

    public const string UIFolder = "UI/";

    public const string fadeFolder = UIFolder + "Fade/";
    public const string circleTransitionObject = fadeFolder + "Circle Transition";

    public const string UITexturesFolder = UIFolder + "UI Textures/";

    public const string blankTexture = UITexturesFolder + "Blank";

    public const string UIBubble = UITexturesFolder + "Bubble";

    public const string portraitFolder = UIFolder + "Portraits/";
    #endregion

    public const string playerPrefab = "PlayerOOC";

    #region Interactable Game Objects
    public const string interactablesFolder = "Interactables/";
    public const string NPC = interactablesFolder + "NPC";
    public const string placedPartyMember = interactablesFolder + "PlacedPartyMember";
    public const string partyMemberFollower = interactablesFolder + "PartyMemberFollower";
    public const string npcExtraSpace = interactablesFolder + "NPC Extra Space";
    public const string transitionSpace = interactablesFolder + "Transition Space";
    public const string vaultableObject = interactablesFolder + "VaultableObject";
    public const string chest = interactablesFolder + "Chest";
    public const string oocMonster = interactablesFolder + "OOC Monster";
    public const string oocObstacle = interactablesFolder + "OOC Obstacle";
    public const string spikes = interactablesFolder + "Spikes";
    public const string floorButton = interactablesFolder + "Floor Button";
    public const string movableObject = interactablesFolder + "Movable Object";
    public const string secretDoor = interactablesFolder + "Secret Door";
    public const string tutorialCollider = interactablesFolder + "Tutorial Collider";
    public const string cunningBlocker = interactablesFolder + "Cunning Blocker";
    public const string book = interactablesFolder + "Book";

    public const string commonComponentsFolder = interactablesFolder + "CommonComponents/";

    #endregion

    #region Combat

    public const string combatFolder = "Combat/";

    public const string enemySprite = combatFolder + "Enemy Sprite";

    // public const string enemyWithAnimations = charactersFolder + "Single_Tile_Enemy";

    public const string healthBar = combatFolder + "Health Bar";

    public const string allyCombatSpriteName = combatFolder + "AllySprite";

    public const string projectile = combatFolder + "Projectile";
    public const string effect = combatFolder + "Effect";

    public const string placeHolderObject = combatFolder + "RepositionPlaceholder";

    #endregion

    #region Sprite Maps
    
    public const string spriteMapFolder = "SpriteMaps/";

    public const string combatBackgroundFolderPath = spriteMapFolder + "Combat Backgrounds/";
    public const string OOCBackgroundFolderPath = spriteMapFolder + "Backgrounds/";
    public const string backgroundTilemap = OOCBackgroundFolderPath + "BackgroundTilemap";

    public const string ground = "Ground";

    #endregion

    #region Sprites
    public const string spriteFolder = "Sprites/";

    public const string plantFolder = spriteFolder + "Plants/";
    public const string forestFolder = plantFolder + "Forest/";
    public const string leafPile = forestFolder + "LeafPile";

    public const string abilityEffectFolderPath = spriteFolder + "Ability Effects/";

    public const string buttonsFolderPath = spriteFolder + "Buttons/";
    public const string buttonUpStoneFolderPath = buttonsFolderPath + "Button_Up_Stone";
    public const string buttonDownStoneFolderPath = buttonsFolderPath + "Button_Down_Stone";

    public const string cratesAndBarrelsFolder = spriteFolder + "CratesAndBarrels/";
    public const string vaultableBarrels = cratesAndBarrelsFolder + "VaultableBarrels";
    public const string destroyableBarricade = cratesAndBarrelsFolder + "DestroyableBarricade";
    public const string squareCratesSmall = cratesAndBarrelsFolder + "Square Crates Small";
    public const string pushableCrate = cratesAndBarrelsFolder + "Crate";
    public const string tripleBarrel = cratesAndBarrelsFolder + "TripleBarrels";

    public const string charactersFolder = spriteFolder + "Characters/";
    public const string humansFolder = charactersFolder + "Humans/";
    public const string defaultNPCSprite = humansFolder + "NPC Sprite";

    public const string bookFolder = spriteFolder + "Books/";
    public const string note = bookFolder + "Note";

    public const string spikesFolderPath = spriteFolder + "Spikes/";
    public const string spikesUp = spikesFolderPath + "Spikes_Up";
    public const string spikesDown = spikesFolderPath + "Spikes_Down";

    public const string stalagmiteFolder = spriteFolder + "Stalagmites/";
    public const string singleStalagmite = stalagmiteFolder + "Single Stalagmite";
    public const string tripleStalagmite = stalagmiteFolder + "Triple Stalagmite";
    public const string mediumBushStalagmite = stalagmiteFolder + "Medium Bush Stalagmite";
    public const string lowStalagmite = stalagmiteFolder + "Low Stalagmite";

    public const string furnitureFolder = spriteFolder + "Furniture/";

    public const string bedFolder = furnitureFolder + "Beds/"; // Assets/Resources/Sprites/Furniture/Beds/Hay/SlaveBed.png

    public const string hayBedFolder = bedFolder + "Hay/";

    public const string slaveBed = hayBedFolder + "SlaveBed";

    public const string chestsFolder = furnitureFolder + "Chests/";
    public const string chestBackClosed = chestsFolder + "Chest_Back_Closed";
    public const string chestBackOpenFilled = chestsFolder + "Chest_Back_Opened_Filled";
    public const string chestBackOpenEmpty = chestsFolder + "Chest_Back_Opened_Empty";
    public const string chestFrontClosed = chestsFolder + "Chest_Front_Closed";
    public const string chestFrontOpenFilled = chestsFolder + "Chest_Front_Opened_Filled";
    public const string chestFrontOpenEmpty = chestsFolder + "Chest_Front_Opened_Empty";

    public const string storageFolder = furnitureFolder + "Storage/";
    public const string itemContainersFolder = storageFolder + "Containers/";

    public const string shelfFrontFull = itemContainersFolder + "Shelf_Front_Full";
    public const string shelfFrontEmpty = itemContainersFolder + "Shelf_Front_Empty";

    
    public const string axeRack = itemContainersFolder + "Axe Rack";
    public const string hammerRack = itemContainersFolder + "Hammer Rack";
    public const string emptyShortRack = itemContainersFolder + "Empty Short Rack";
    
    public const string swordTable = itemContainersFolder + "Sword Table";
    public const string pickaxeTable = itemContainersFolder + "Pickaxe Table";
    public const string emptyWeaponTable = itemContainersFolder + "Empty Weapon Table";

    public const string spearRack = itemContainersFolder + "Spear Rack";
    public const string shovelRack = itemContainersFolder + "Shovel Rack";
    public const string emptyPolearmRack = itemContainersFolder + "Empty Polearm Rack";

    public const string mattockRack = itemContainersFolder + "Mattock Rack";
    public const string emptyMattockRack = itemContainersFolder + "Empty Mattock Rack";


    public const string waterFolder = spriteFolder + "Water/";
    public const string water = waterFolder + "Water";

    public const string statueFolder = furnitureFolder + "Statues/";
    public const string directorStatueSpriteName = "DirectorStatue";
    public const string directorStatuePath = statueFolder + directorStatueSpriteName;
    public const string brokenDirectorStatuePath = statueFolder + "Broken"+directorStatueSpriteName;

    public const string cunningObjectsFolder = spriteFolder + "CunningObjects/";
    public const string crankSW = cunningObjectsFolder + "Crank_SW";
    public const string crankSE = cunningObjectsFolder + "Crank_SE";

    public const string secretDoorsFolder = spriteFolder + "SecretDoors/";
    public const string mineLvl2WallSecretDoor = secretDoorsFolder + "MineLvl_2 Wall";
    public const string mineLvl3WallSecretDoor = secretDoorsFolder + "MineLvl_3-SecretDoor";
    public const string mineLvl3GroundSecretDoor = secretDoorsFolder + "MineLvl_3-Ground";
    public const string manseWallSecretDoor = secretDoorsFolder + "Manse Wall";
    public const string manseHalfWallSecretDoor = secretDoorsFolder + "Manse Half Wall";
    public const string secretShelfNWSecretDoor = secretDoorsFolder + "Secret Shelf NW";
    public const string wallPatch = secretDoorsFolder + "WallPatch";
    public const string wallPatchTall = secretDoorsFolder + "WallPatchTall";

/*
WallPatch
WallPatchTall
*/

    public const string miscFolder = spriteFolder + "Misc/";
    public const string controlPanel = miscFolder + NPCNameList.controlPanel;
    public const string unstablePillar = miscFolder + NPCNameList.unstablePillar;

    public const string tilesFolder = spriteFolder + "Tiles/";
    public const string halfWallsFolder = tilesFolder + "Half Walls/";
    public const string stoneHalfWallsFolder = halfWallsFolder + "Stone/";
    public const string lavaHalfWallsFolder = halfWallsFolder + "Lava/";
    public const string lavaVaultableGapHalf = lavaHalfWallsFolder + "Lava Vaultable Gap";
    public const string shackWallHalf = stoneHalfWallsFolder + "Shack Wall Half";
    public const string brickHalfWallsFolder = halfWallsFolder + "Brick/";
    public const string mineLvl2WallCunningObstacle = brickHalfWallsFolder + "Dark_Brick_Cunning_Obstacle";

    public const string tallWallsFolder = tilesFolder + "Tall Walls/";

    public const string groundFolder = tilesFolder + "Ground/";
    public const string stoneGroundFolder = groundFolder + "Stone/";
    public const string stoneVaultableGap = stoneGroundFolder + "Stone Vaultable Gap";

    public const string rubbleFolder = spriteFolder + "Rubble/";
    public const string southDescendingRubble = rubbleFolder + "South Descending Rubble";
    public const string northWestDescendingRubble = rubbleFolder + "NW Descending Rubble";
    public const string southWestDescendingRubble = rubbleFolder + "SW Descending Rubble";
    public const string blockRubble = rubbleFolder + "Block Rubble";
    public const string lowRubble = rubbleFolder + "Low Rubble";
    public const string vaultableRocks = rubbleFolder + "Vaultable Rocks";
    public const string tutorialRubble = rubbleFolder + "Tutorial Rubble";

    public const string doorsFolder =  spriteFolder + "Doors/";
    public const string XAxisDoor = doorsFolder + "XAxisDoor";
    public const string YAxisDoor = doorsFolder + "YAxisDoor";

    public const string portcullis1x1SpriteName = "1x1Portcullis";
    public const string portcullis1x1Path =  doorsFolder + portcullis1x1SpriteName;
    public const string portcullis2x1SpriteName = "2x1Portcullis";
    public const string portcullis2x1Path =  doorsFolder + portcullis2x1SpriteName;
    public const string portcullis3x1SpriteName = "3x1Portcullis";
    public const string portcullis3x1Path =  doorsFolder + portcullis3x1SpriteName;

    private const string ladderFolder = spriteFolder + "Ladders/";
    public const string ladderShortNE = ladderFolder + "Ladder_Short_NE";
    public const string ladderTallNE = ladderFolder + "Ladder_Tall_NE";
    public const string ladderTallSW = ladderFolder + "Ladder_Tall_SW";

    private const string shadowFolder = charactersFolder + "Shadows/";
    public const string shadow256x256 = shadowFolder + "256_Shadow";
    public const string shadow512x512 = shadowFolder + "512_Shadow";

    #endregion
}


public static class MapTileSpriteList
{
    public const string mapTilesFolder = PrefabNames.tilesFolder + "MapTiles/";

    public const string manMadeFolder = mapTilesFolder + "manmade/";
    public const string bridgeTwoMapTile = "bridge (2)";
    public const string campWithManseMapTile = "Camp With Manse";
    public const string campMapTile = "Camp";
    public const string mapMineMapTile = "Map Mine";
    public const string worldMapMineMapTile = "World Map Mine";
    public const string towerOneMapTile = "Tower 1";
    public const string ruinOneMapTile = "Ruin 1";
    public const string ruinTwoMapTile = "Ruin 2";
    public const string manseWallMapTile = "Manse Wall Map Tile";
    public const string shelvesMapTile = "Shelves Map Tile";
    public const string largeWoodenStairsMapTile = "Large Wooden Stairs";
    public const string stockroomMapTile = "Stockroom Map Tile";
    public const string gardenMapTile = "Garden Map Tile";
    public const string tableMapTile = "Table Map Tile";
    public const string blueCarpetMapTile = "Blue Carpet";
    public const string greenCarpetMapTile = "Green Carpet";
    public const string redCarpetMapTile = "Red Carpet";
        

    public const string propsFolder = mapTilesFolder + "props/";

    public const string pitTile = "Pit";

    public const string mountainFolder = propsFolder + "mountains/";

    public const string darkMountainMapTile = "Dark Mountain";

    public const string darkPurpleMountainOneMapTile = "Dark Purple Mountain 1";
    public const string darkPurpleMountainTwoMapTile = "Dark Purple Mountain 2";

    public const string lightPurpleMountainOneMapTile = "Light Purple Mountain 1";
    public const string lightPurpleMountainTwoMapTile = "Light Purple Mountain 2";

    public const string normalMountainOneMapTile = "Normal Mountain 1";
    public const string normalMountainTwoMapTile = "Normal Mountain 2";

    public const string sandMountainOneMapTile = "Sand Mountain 1";
    public const string sandMountainTwoMapTile = "Sand Mountain 2";

    public const string snowTippedMountainOneMapTile = "Snow Tipped Mountain 1";
    public const string snowTippedMountainTwoMapTile = "Snow Tipped Mountain 2";
    
    public const string foliageFolder = propsFolder + "foliage/";
    public const string forestTreesMapTile = "foliage (7)";

    public const string groundTileFolder = mapTilesFolder + "Ground Tiles/";

    public const string dirtFolder = groundTileFolder + "Dirt/";
    public const string dirtOneTile = "Dirt 1";

    public const string grassFolder = groundTileFolder + "Grass/";
    public const string brownGrassTile = "Brown Grass";
    public const string dullGreenGrassTile = "Dull Green Grass";
    public const string forestFloorGrassTile = "Forest Floor Grass";
    public const string greenGrassTile = "Green Grass";

    public const string sandFolder = groundTileFolder + "Sand/";
    public const string sandTile = "Sand";
    public const string sandWithDunesTile = "Sand With Dunes";

    public const string stoneFolder = groundTileFolder + "Stone/";
    public const string darkStoneTile = "Dark Stone";
    public const string darkPurpleStoneTile = "Dark Purple Stone";
    public const string lightPurpleStoneTile = "Light Purple Stone";
    public const string stoneOneTile = "Stone 1";
    public const string stoneTwoTile = "Stone 2";

    public const string waterFolder = groundTileFolder + "Water/";
    public const string lakeTile = "Lake";

    public const string woodFolder = groundTileFolder + "Wood/";
    public const string woodFloorTile = "Wood Floor";
    
    public static string getSpriteFullPath(string spriteFileName)
    {
        switch(spriteFileName)
        {
            case pitTile:
                return propsFolder + spriteFileName;

            case forestTreesMapTile:
                return foliageFolder + spriteFileName;

            case darkMountainMapTile:
            case darkPurpleMountainOneMapTile:
            case darkPurpleMountainTwoMapTile:
            case lightPurpleMountainOneMapTile:
            case lightPurpleMountainTwoMapTile:
            case normalMountainOneMapTile:
            case normalMountainTwoMapTile:
            case sandMountainOneMapTile:
            case sandMountainTwoMapTile:
            case snowTippedMountainOneMapTile:
            case snowTippedMountainTwoMapTile:
                return mountainFolder + spriteFileName;

            case shelvesMapTile:
            case bridgeTwoMapTile:
            case campMapTile:
            case campWithManseMapTile:
            case mapMineMapTile:
            case ruinOneMapTile:
            case ruinTwoMapTile:
            case towerOneMapTile:
            case worldMapMineMapTile:
            case manseWallMapTile:
            case largeWoodenStairsMapTile:
            case stockroomMapTile:
            case gardenMapTile:
            case tableMapTile:            
            case blueCarpetMapTile:            
            case greenCarpetMapTile:            
            case redCarpetMapTile:            
                return manMadeFolder + spriteFileName;

            case brownGrassTile:
            case dullGreenGrassTile:
            case forestFloorGrassTile:
            case greenGrassTile:
                return grassFolder + spriteFileName;

            case dirtOneTile:
                return dirtFolder + spriteFileName;

            case sandTile:
            case sandWithDunesTile:
                return sandFolder + spriteFileName;

            case darkStoneTile:            
            case darkPurpleStoneTile:            
            case lightPurpleStoneTile:            
            case stoneOneTile:            
            case stoneTwoTile:            
                return stoneFolder + spriteFileName;

            case lakeTile:            
                return waterFolder + spriteFileName;            
            
            case woodFloorTile:            
                return woodFolder + spriteFileName;            
            
            default:
                return Constants.emptyString;
        }
    }
}