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

public class NPCSpawnDetails : OOCSpawnDetails
{

    public bool activated;
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

    public bool interactable()
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

        spawnActions(dialogueTrigger.dialogue);
    }

    public virtual void spawnActions(Dialogue dialogue)
    {
        //empty on purpose
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
        switch(vaultableObject.objectName)
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

    public override void spawnActions(Dialogue dialogue)
    {
        dialogue.variableSources.Add(vaultableObject);
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