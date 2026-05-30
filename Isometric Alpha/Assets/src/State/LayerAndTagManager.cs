using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerAndTagManager
{

    public readonly static int defaultLayer = LayerMask.NameToLayer("Default");
	public readonly static int playerLayer = LayerMask.NameToLayer("Player");
	public readonly static int colliderLayer = LayerMask.NameToLayer("Collider");
	public readonly static int enemyBoundaryLayer = LayerMask.NameToLayer("EnemyBoundary");
	public readonly static int npcLayer = LayerMask.NameToLayer("NPC");
	public readonly static int objectLayer = LayerMask.NameToLayer("Object");
	public readonly static int movableObjectLayer = LayerMask.NameToLayer("MovableObject");
	public readonly static int enemyLayer = LayerMask.NameToLayer("Enemy");
	public readonly static int cunningableObjectLayer = LayerMask.NameToLayer("CunningableObject");
	public readonly static int openableDoorLayer = LayerMask.NameToLayer("OpenableDoor");
	public readonly static int terrainLayer = LayerMask.NameToLayer("Terrain");
	public readonly static int trainLayer = LayerMask.NameToLayer("Train");
	public readonly static int chestLayer = LayerMask.NameToLayer("Chest");
	public readonly static int transitionLayer = LayerMask.NameToLayer("Transition");
	public readonly static int tutorialLayer = LayerMask.NameToLayer("Tutorial");
	public readonly static int movableObjectBlockerLayer = LayerMask.NameToLayer("MovableObjectBlocker");
    public readonly static int UILayer = LayerMask.NameToLayer("UI");
	public readonly static int observableLayer = LayerMask.NameToLayer("Observable");

	public const string firstSortingLayerName = "First";
	public const string thirdSortingLayerName = "Third";
	public const string fourthSortingLayerName = "Fourth";
	public const string sixthSortingLayerName = "Sixth";
    public const string mapSortingLayerName = "Map";
    public const string tutorialSequenceWindowSortingLayerName = "Tutorial Sequence Window";

	public const string playerTag = "Player";
	public const string enemyTag = "Enemy";
	public const string npcTag = "NPC";
	public const string mainCameraTag = "MainCamera";
	public const string abilityEditorTag = "AbilityEditor";
    public const string equipmentDisplayTag = "EquipmentDisplay";
    public const string itemUseTargetTag = "ItemUseTarget";
    public const string junkSlotTargetTag = "JunkSlotTarget";
	public const string cunningTargetTag = "CunningTarget";
	public const string observableTag = "Observable";
	public const string permanentButtonTag = "PermanentButton";
	public const string partyMemberTag = "PartyMember";
	public const string areaManagerTag = "AreaManager";
	public const string terrainTag = "Terrain";
	public const string transitionTag = "Transition";
	public const string placeHolderTag = "PlaceHolder";
	public const string bookTag = "Book";
	public const string shownWhileTerrainHiddenTag = "ShownWhileTerrainHidden";
    public const string mainVirtualCameraTag = "MainVirtualCamera";
    public const string musicTag = "Music";
    public const string fadeToBlackTag = "FadeToBlack";

	// public static LayerMask allInteractableLayers;
	public readonly static LayerMask playerLayerMask = initializePlayerLayerMask();
	public readonly static LayerMask npcLayerMask = initializeNPCLayerMask();
	public readonly static LayerMask chestLayerMask = initializeChestLayerMask();
	public readonly static LayerMask transitionLayerMask = initializeTransitionLayerMask();
	public readonly static LayerMask tutorialLayerMask = initializeTutorialLayerMask();
    public readonly static LayerMask terrainLayerMask = initializeTerrainLayerMask();
	public readonly static LayerMask moveableObjectLayerMask = initializemoveableObjectLayerMask();
	public readonly static LayerMask uiLayerMask = initializeUILayerMask();


	public readonly static LayerMask blocksPlayerMovementLayerMask = initializeBlocksPlayerMovementLayerMask();
	public readonly static LayerMask blocksMoveableObjectLayerMask = initializeblocksMoveableObjectLayerMask();
	public readonly static LayerMask blocksEnemyMovementLayerMask = initializeBlocksEnemyMovementLayerMask();

    public readonly static LayerMask observableLayerMask = initializeObservableLayerMask();
    public readonly static LayerMask blocksObservationLayerMask = initializeBlocksObservationLayerMask();
	public readonly static LayerMask blocksSkillsLayerMask = initializeBlocksSkillsLayerMask();

	public readonly static LayerMask pressesButtonsLayerMask = initializePressesButtonsLayerMask();

	public readonly static LayerMask cameraInputLayerMask = initializeUICameraLayerMask();


	private static LayerMask initializePlayerLayerMask()
	{
		LayerMask playerLayerMask = new LayerMask();
        playerLayerMask |= (1 << playerLayer);

        return playerLayerMask;
	}

	private static LayerMask initializeNPCLayerMask()
	{
		LayerMask npcLayerMask = new LayerMask();
        npcLayerMask |= (1 << npcLayer);

        return npcLayerMask;
	}

	private static LayerMask initializeChestLayerMask()
	{
		LayerMask chestLayerMask = new LayerMask();
        chestLayerMask |= (1 << chestLayer);

        return chestLayerMask;
	}

	private static LayerMask initializeTransitionLayerMask()
	{
		LayerMask transitionLayerMask = new LayerMask();
        transitionLayerMask |= (1 << transitionLayer);

        return transitionLayerMask;
	}

	private static LayerMask initializeTutorialLayerMask()
	{
		LayerMask tutorialLayerMask = new LayerMask();
		tutorialLayerMask |= (1 << tutorialLayer);

        return tutorialLayerMask;
	}

	private static LayerMask initializeTerrainLayerMask()
	{
		LayerMask terrainLayerMask = new LayerMask();
		terrainLayerMask |= (1 << terrainLayer);

        return terrainLayerMask;
	}

	private static LayerMask initializemoveableObjectLayerMask()
	{
		LayerMask moveableObjectLayerMask = new LayerMask();
        moveableObjectLayerMask |= (1 << movableObjectLayer);
        return moveableObjectLayerMask;
	}

	private static LayerMask initializeUILayerMask()
	{
		LayerMask uiLayerMask = new LayerMask();
        uiLayerMask |= (1 << UILayer);

        return uiLayerMask;
	}

	private static LayerMask initializeBlocksPlayerMovementLayerMask()
	{
		LayerMask blocksPlayerMovementLayerMask = new LayerMask();
		blocksPlayerMovementLayerMask |= (1 << colliderLayer);
		blocksPlayerMovementLayerMask |= (1 << npcLayer);
		blocksPlayerMovementLayerMask |= (1 << objectLayer);
		blocksPlayerMovementLayerMask |= (1 << enemyLayer);
		blocksPlayerMovementLayerMask |= (1 << cunningableObjectLayer);
		blocksPlayerMovementLayerMask |= (1 << openableDoorLayer);
        blocksPlayerMovementLayerMask |= (1 << chestLayer);
        blocksPlayerMovementLayerMask |= (1 << observableLayer);

        return blocksPlayerMovementLayerMask;
	}

	private static LayerMask initializeBlocksEnemyMovementLayerMask()
	{
		LayerMask blocksEnemyMovementLayerMask = new LayerMask();
		blocksEnemyMovementLayerMask |= (1 << colliderLayer);
		blocksEnemyMovementLayerMask |= (1 << npcLayer);
		blocksEnemyMovementLayerMask |= (1 << objectLayer);
		blocksEnemyMovementLayerMask |= (1 << movableObjectLayer);
		blocksEnemyMovementLayerMask |= (1 << enemyLayer);
		blocksEnemyMovementLayerMask |= (1 << cunningableObjectLayer);
		blocksEnemyMovementLayerMask |= (1 << openableDoorLayer);
		blocksEnemyMovementLayerMask |= (1 << chestLayer);
        blocksEnemyMovementLayerMask |= (1 << enemyBoundaryLayer);
        blocksEnemyMovementLayerMask |= (1 << observableLayer);

        return blocksEnemyMovementLayerMask;
	}


	private static LayerMask initializeblocksMoveableObjectLayerMask()
	{
		LayerMask blocksMoveableObjectLayerMask = new LayerMask();
		blocksMoveableObjectLayerMask |= (1 << movableObjectLayer);
		blocksMoveableObjectLayerMask |= (1 << colliderLayer);
		blocksMoveableObjectLayerMask |= (1 << npcLayer);
		blocksMoveableObjectLayerMask |= (1 << objectLayer);
		blocksMoveableObjectLayerMask |= (1 << enemyLayer);
		blocksMoveableObjectLayerMask |= (1 << cunningableObjectLayer);
		blocksMoveableObjectLayerMask |= (1 << openableDoorLayer);
		blocksMoveableObjectLayerMask |= (1 << chestLayer);
        blocksMoveableObjectLayerMask |= (1 << movableObjectBlockerLayer);
        blocksMoveableObjectLayerMask |= (1 << observableLayer);

        return blocksMoveableObjectLayerMask;
	}

	private static LayerMask initializeBlocksSkillsLayerMask()
	{
		LayerMask blocksSkillLayerMask = new LayerMask();
        
		blocksSkillLayerMask |= (1 << colliderLayer);
		blocksSkillLayerMask |= (1 << objectLayer);
		blocksSkillLayerMask |= (1 << cunningableObjectLayer);
        blocksSkillLayerMask |= (1 << openableDoorLayer);
		blocksSkillLayerMask |= (1 << enemyLayer);
        blocksSkillLayerMask |= (1 << observableLayer);

        return blocksSkillLayerMask;
	}

    private static LayerMask initializeObservableLayerMask()
    {
        LayerMask observationLayerMask = new LayerMask();
        observationLayerMask |= (1 << observableLayer);
        return observationLayerMask;
    }

    private static LayerMask initializeBlocksObservationLayerMask()
    {
        LayerMask blocksObservationLayerMask = new LayerMask();
        blocksObservationLayerMask |= (1 << colliderLayer);
        blocksObservationLayerMask |= (1 << objectLayer);
        blocksObservationLayerMask |= (1 << cunningableObjectLayer);
        blocksObservationLayerMask |= (1 << openableDoorLayer);
        return blocksObservationLayerMask;
    }

	private static LayerMask initializePressesButtonsLayerMask()
	{
		LayerMask pressesButtonsLayerMask = new LayerMask();
		pressesButtonsLayerMask |= (1 << movableObjectLayer);
		pressesButtonsLayerMask |= (1 << npcLayer);
        pressesButtonsLayerMask |= (1 << playerLayer);
        return pressesButtonsLayerMask;
	}
	
	private static LayerMask initializeUICameraLayerMask()
	{
		LayerMask cameraInputLayerMask = new LayerMask();
		cameraInputLayerMask |= (1 << UILayer);
        return cameraInputLayerMask;
	}

}
