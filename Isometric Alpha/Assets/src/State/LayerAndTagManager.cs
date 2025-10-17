using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerAndTagManager
{

	private static readonly int playerLayer = LayerMask.NameToLayer("Player");
	private static readonly int colliderLayer = LayerMask.NameToLayer("Collider");
	private static readonly int enemyBoundaryLayer = LayerMask.NameToLayer("EnemyBoundary");
	private static readonly int npcLayer = LayerMask.NameToLayer("NPC");
	private static readonly int objectLayer = LayerMask.NameToLayer("Object");
	private static readonly int movableObjectLayer = LayerMask.NameToLayer("MovableObject");
	private static readonly int enemyLayer = LayerMask.NameToLayer("Enemy");
	private static readonly int cunningableObjectLayer = LayerMask.NameToLayer("CunningableObject");
	private static readonly int openableDoorLayer = LayerMask.NameToLayer("OpenableDoor");
	private static readonly int terrainLayer = LayerMask.NameToLayer("Terrain");
	private static readonly int trainLayer = LayerMask.NameToLayer("Train");
	private static readonly int chestLayer = LayerMask.NameToLayer("Chest");
	private static readonly int transitionLayer = LayerMask.NameToLayer("Transition");
	private static readonly int tutorialLayer = LayerMask.NameToLayer("Tutorial");
	private static readonly int movableObjectBlockerLayer = LayerMask.NameToLayer("MovableObjectBlocker");
	private static readonly int UILayer = LayerMask.NameToLayer("UI");

	public const string firstSortingLayerName = "First";
	public const string thirdSortingLayerName = "Third";

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


	// public static LayerMask allInteractableLayers;
	public readonly static LayerMask playerLayerMask = initializePlayerLayerMask();
	public readonly static LayerMask npcLayerMask = initializeNPCLayerMask();
	public readonly static LayerMask chestLayerMask = initializeChestLayerMask();
	public readonly static LayerMask transitionLayerMask = initializeTransitionLayerMask();
	public readonly static LayerMask tutorialLayerMask = initializeTutorialLayerMask();
    public readonly static LayerMask terrainLayerMask = initializeTerrainLayerMask();
	public readonly static LayerMask movableObjectLayerMask = initializeMovableObjectLayerMask();
	public readonly static LayerMask uiLayerMask = initializeUILayerMask();


	public readonly static LayerMask blocksPlayerMovementLayerMask = initializeBlocksPlayerMovementLayerMask();
	public readonly static LayerMask blocksMovableObjectLayerMask = initializeBlocksMovableObjectLayerMask();
	public readonly static LayerMask blocksEnemyMovementLayerMask = initializeBlocksEnemyMovementLayerMask();

	public readonly static LayerMask blocksObservationLayerMask = initializeBlocksObservationLayerMask();

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

	private static LayerMask initializeMovableObjectLayerMask()
	{
		LayerMask movableObjectLayerMask = new LayerMask();
        movableObjectLayerMask |= (1 << movableObjectLayer);
        return movableObjectLayerMask;
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

        return blocksPlayerMovementLayerMask;
	}

	private static LayerMask initializeBlocksEnemyMovementLayerMask()
	{
		LayerMask blocksEnemyMovementLayerMask = new LayerMask();
		blocksEnemyMovementLayerMask |= (1 << colliderLayer);
		blocksEnemyMovementLayerMask |= (1 << npcLayer);
		blocksEnemyMovementLayerMask |= (1 << objectLayer);
		blocksEnemyMovementLayerMask |= (1 << enemyLayer);
		blocksEnemyMovementLayerMask |= (1 << cunningableObjectLayer);
		blocksEnemyMovementLayerMask |= (1 << openableDoorLayer);
		blocksEnemyMovementLayerMask |= (1 << chestLayer);
        blocksEnemyMovementLayerMask |= (1 << enemyBoundaryLayer);

        return blocksEnemyMovementLayerMask;
	}


	private static LayerMask initializeBlocksMovableObjectLayerMask()
	{
		LayerMask blocksMovableObjectLayerMask = new LayerMask();
		blocksMovableObjectLayerMask |= (1 << movableObjectLayer);
		blocksMovableObjectLayerMask |= (1 << colliderLayer);
		blocksMovableObjectLayerMask |= (1 << npcLayer);
		blocksMovableObjectLayerMask |= (1 << objectLayer);
		blocksMovableObjectLayerMask |= (1 << enemyLayer);
		blocksMovableObjectLayerMask |= (1 << cunningableObjectLayer);
		blocksMovableObjectLayerMask |= (1 << openableDoorLayer);
		blocksMovableObjectLayerMask |= (1 << chestLayer);
        blocksMovableObjectLayerMask |= (1 << movableObjectBlockerLayer);

        return blocksMovableObjectLayerMask;
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
