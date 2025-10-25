using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOCSpawnDetails
{

    private const string gameObjectNameSuffix = "'s GameObject";
    private const string gameObjectPlaceHolderName = "PlaceHolder GameObject";

    public string npcName = "";
    public Vector3Int cellCoords;

    public OOCSpawnDetails(string npcName, Vector3Int cellCoords)
    {
        this.npcName = npcName;
        this.cellCoords = cellCoords;
    }

    public virtual string getSpriteName()
    {
        return null;
    }

    public virtual string getPrefabName()
    {
        return null;
    }

    public virtual bool determineSpriteAtSpawn()
    {
        return true;
    }

    public virtual Transform getParent()
    {
        return null;
    }

    public virtual void setGameObjectName(GameObject gameObject)
    {
        if (npcName.Length > 0)
        {
            gameObject.name = npcName + gameObjectNameSuffix;
        }
        else
        {
            gameObject.name = gameObjectPlaceHolderName;
        }
    }

    public virtual void spawnActions(GameObject interactable)
    {
        SpriteRenderer spriteRenderer = interactable.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Helpers.loadSpriteFromResources(getSpriteName());
    }

}

public class ObstacleSpawnDetails : OOCSpawnDetails
{

    private string spriteName;

    public ObstacleSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName) :
    base(npcName, cellCoords)
    {
        this.spriteName = spriteName;
    }

    public override string getSpriteName()
    {
        return spriteName;
    }

    public override string getPrefabName()
    {
        return PrefabNames.oocObstacle;
    }

    public override bool determineSpriteAtSpawn()
    {
        return true;
    }

    public override Transform getParent()
    {
        return AreaManager.getNPCParent();
    }

    public override void spawnActions(GameObject interactable)
    {
        Obstacle obstacle = interactable.GetComponent<Obstacle>();
        obstacle.setObstacleName(npcName);

        SpriteRenderer spriteRenderer = interactable.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Helpers.loadSpriteFromResources(getSpriteName());
    }

}

public class NPCSpawnDetails : OOCSpawnDetails
{

    public const string extraSpaceNameSuffix = "'s Extra Space GameObject";

    public bool activated;
    public Vector3Int[] extraSpaces = new Vector3Int[0];
    public Dialogue dialogue;
    public SpeakAtStartScript speakAtStartScript;

    public NPCSpawnDetails(string npcName, Vector3Int cellCoords) :
    base(npcName, cellCoords)
    {
        this.activated = true;

        this.dialogue = DialogueList.getDialogue(npcName);
    }

    public NPCSpawnDetails(string npcName, Vector3Int cellCoords, string areaName) :
    base(npcName, cellCoords)
    {
        this.activated = true;
        this.dialogue = DialogueList.getDialogue(npcName, areaName);
    }

    public NPCSpawnDetails(string npcName, Vector3Int cellCoords, string areaName, Vector3Int[] extraSpaces) :
    base(npcName, cellCoords)
    {
        this.activated = true;
        this.dialogue = DialogueList.getDialogue(npcName, areaName);

        this.extraSpaces = extraSpaces;
    }

    public NPCSpawnDetails(string npcName, Vector3Int cellCoords, string areaName, SpeakAtStartScript speakAtStartScript) :
    base(npcName, cellCoords)
    {
        this.activated = true;
        this.dialogue = DialogueList.getDialogue(npcName, areaName);
        this.speakAtStartScript = speakAtStartScript;
    }

    public NPCSpawnDetails(string npcName, Vector3Int cellCoords, bool activated) :
    base(npcName, cellCoords)
    {
        this.activated = activated;
        this.dialogue = null;
    }

    public NPCSpawnDetails(string npcName, Vector3Int cellCoords, bool activated, Dialogue dialogue) :
    base(npcName, cellCoords)
    {
        this.activated = activated;
        this.dialogue = dialogue;
    }

    public NPCSpawnDetails(string npcName, Vector3Int cellCoords, bool activated, string areaName) :
    base(npcName, cellCoords)
    {
        this.activated = activated;
        this.dialogue = DialogueList.getDialogue(npcName, areaName);
    }

    public virtual bool interactable()
    {
        return dialogue != null;
    }

    public override string getSpriteName()
    {
        return PrefabNames.defaultNPCSprite;
    }

    public override string getPrefabName()
    {
        return PrefabNames.NPC;
    }

    public override Transform getParent()
    {
        return AreaManager.getNPCParent();
    }

    public override void spawnActions(GameObject npc)
    {
        base.spawnActions(npc);

        npc.SetActive(activated);

        DialogueTrigger dialogueTrigger = npc.GetComponent<DialogueTrigger>();

        if (interactable())
        {
            dialogueTrigger.dialogue = dialogue;
            dialogueTrigger.speakAtStartScript = speakAtStartScript;
        }
        else
        {
            dialogueTrigger.dialogue = new Dialogue(npcName, npc);
        }

        spawnActions(dialogueTrigger);
    }

    public virtual void spawnActions(DialogueTrigger mainTrigger)
    {
        int index = 0;
        foreach (Vector3Int extraSpace in extraSpaces)
        {
            GameObject extraSpaceGameObject = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.npcExtraSpace), getParent());

            extraSpaceGameObject.name = npcName + extraSpaceNameSuffix + " #" + (index+1);

            DialogueTriggerLink linkTrigger = extraSpaceGameObject.GetComponent<DialogueTriggerLink>();

            linkTrigger.linkedDialogue = mainTrigger;

            extraSpaceGameObject.transform.position = AreaManager.getMasterGrid().GetCellCenterWorld(extraSpace);

            Helpers.updateColliderPosition(extraSpaceGameObject);

            SpawnInfoManager.addGameObject(extraSpaceGameObject);

            index++;
        }
    }
}

public class ShopkeeperSpawnDetails : NPCSpawnDetails
{

    public ShopkeeperSpawnDetails(string npcName, Vector3Int cellCoords, string areaName) :
    base(npcName, cellCoords, areaName)
    {
        
    }

    public ShopkeeperSpawnDetails(string npcName, Vector3Int cellCoords, string areaName, Vector3Int[] extraSpaces) :
    base(npcName, cellCoords, areaName, extraSpaces)
    {
        
    }

    public override bool interactable()
    {
        return true;
    }

    public override void spawnActions(GameObject npc)
    {
        base.spawnActions(npc);

        Shopkeeper shopkeeper = npc.AddComponent<Shopkeeper>();

        shopkeeper.shopkeeperInventoryKey = npcName;
    }
}

public class SecretDoorSpawnDetails : NPCSpawnDetails
{
    private SecretDoorInfo secretDoorInfo;

    public SecretDoorSpawnDetails(string npcName, Vector3Int cellCoords, string areaName, SecretDoorInfo secretDoorInfo) :
    base(npcName, cellCoords, areaName)
    {
        this.secretDoorInfo = secretDoorInfo;
    }
    public override bool interactable()
    {
        return true;
    }

    public override string getSpriteName()
    {
        return PrefabNames.defaultNPCSprite;
    }

    public override string getPrefabName()
    {
        return PrefabNames.secretDoor;
    }

    public override Transform getParent()
    {
        return AreaManager.getNPCParent();
    }

    public override void spawnActions(GameObject secretDoor)
    {
        if(secretDoorInfo.hasBeenDiscovered())
        {
            GameObject.DestroyImmediate(secretDoor);
        }

        base.spawnActions(secretDoor);

        ObservableObject observableObject = secretDoor.GetComponent<ObservableObject>();

        observableObject.secretDoorKey = secretDoorInfo.secretDoorKey;
    }

    public override void spawnActions(DialogueTrigger dialogueTrigger)
    {
        Dialogue dialogue = dialogueTrigger.dialogue;

        dialogue.variableSources.Add(secretDoorInfo);
    }
}

public class VaultableObjectSpawnDetails : NPCSpawnDetails
{

    public VaultableObject vaultableObject;


    public VaultableObjectSpawnDetails(string npcName, Vector3Int cellCoords, VaultableObject vaultableObject) :
    base(npcName, cellCoords)
    {
        this.vaultableObject = vaultableObject;

    }

    public override string getSpriteName()
    {
        switch (vaultableObject.objectName)
        {
            case VaultableObject.barrelName:
                return PrefabNames.vaultableBarrels;

            default:
                return null;
        }
    }

    public override string getPrefabName()
    {
        return PrefabNames.vaultableObject;
    }

    public override void spawnActions(DialogueTrigger dialogueTrigger)
    {
        dialogueTrigger.dialogue.variableSources.Add(vaultableObject);
    }

}

public class ChestSpawnDetails : OOCSpawnDetails
{
    private int index;
    private Facing facing;


    public ChestSpawnDetails(int index, Vector3Int cellCoords, Facing facing) :
    base(generateName(index), cellCoords)
    {
        this.index = index;
        this.facing = facing;
    }
    
    public override string getPrefabName()
    {
        return PrefabNames.chest;
    }

    private static string generateName(int index)
    {
        return NPCNameList.chest + "-" + index;
    }

    public override bool determineSpriteAtSpawn()
    {
        return false;
    }

    public override void spawnActions(GameObject chestGameObject)
    {
        Chest chest = chestGameObject.GetComponent<Chest>();

        chest.populate(index, facing);
    }
}