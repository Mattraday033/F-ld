using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ScriptOnLocationEntryList
{
    private static Dictionary<string, List<PlayerInteractionScript>> scriptOnLocationEntryDict;

    public static List<PlayerInteractionScript> getScriptsOnLocationEntry()
    {
        if (!scriptOnLocationEntryDict.ContainsKey(AreaManager.locationName))
        {
            return new List<PlayerInteractionScript>();
        }
        
        return scriptOnLocationEntryDict[AreaManager.locationName];
    }

    [RuntimeInitializeOnLoadMethod]
    public static void initialize()
    {
        scriptOnLocationEntryDict = new Dictionary<string, List<PlayerInteractionScript>>();
        List<PlayerInteractionScript> list;

        #region Body Pile
        list = new List<PlayerInteractionScript>();

        list.Add(new EnteredBodyPile());

        scriptOnLocationEntryDict.Add(LocationNameList.bodyPile, list);
        #endregion

        #region Manse Camp
        list = new List<PlayerInteractionScript>();

        list.Add(new EnteredCampManse());

        scriptOnLocationEntryDict.Add(LocationNameList.campManse, list);
        #endregion

        #region Camp North West
        list = new List<PlayerInteractionScript>();

        list.Add(new EnteredCampNorthWest());

        scriptOnLocationEntryDict.Add(LocationNameList.campNorthWest, list);
        #endregion

        #region Manse-1F-1a
        list = new List<PlayerInteractionScript>();

        list.Add(new EnteredManse1F1a());

        scriptOnLocationEntryDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section1a, list);
        #endregion

        #region Manse-2F-1a
        list = new List<PlayerInteractionScript>();

        list.Add(new EnteredManse2F1a());

        scriptOnLocationEntryDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section1a, list);
        #endregion

        #region MineLvl_1-1a
        list = new List<PlayerInteractionScript>();

        list.Add(new EnteredMineLvl1());

        scriptOnLocationEntryDict.Add(ZoneKeyList.mineLvl1 + LocationNameList.section1a, list);
        #endregion

        #region MineLvl_2-1a
        list = new List<PlayerInteractionScript>();

        list.Add(new EnteredMineLvl2());

        scriptOnLocationEntryDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section1a, list);
        #endregion

        #region MineLvl_2-2a
        list = new List<PlayerInteractionScript>();

        list.Add(new EnteredMineLvl2_2a());

        scriptOnLocationEntryDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section2a, list);
        #endregion

        #region MineLvl_3-1a
        list = new List<PlayerInteractionScript>();

        list.Add(new EnteredMineLvl3());

        scriptOnLocationEntryDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section1a, list);
        #endregion
    }

}

public class EnteredManse2F1a : PlayerInteractionScript
{
    private const string manse2F1AGateKey = ZoneKeyList.manseSecondFloor+LocationNameList.section1a+NPCNameList.gate;
    private const string manse2F1cShelfIndexZeroKey = ZoneKeyList.manseSecondFloor+LocationNameList.section1c+Chest.chestKeyMarker+Constants.zeroRating;
    private const string manse2F1cShelfIndexOneKey = ZoneKeyList.manseSecondFloor+LocationNameList.section1c+Chest.chestKeyMarker+"1";

    public override void runScript(GameObject target = null)
    {
        if ((Flags.getFlag(FlagNameList.revoltStarted) || 
            AreaList.getArea(ZoneKeyList.manseSecondFloor+LocationNameList.section1a).isHostile()) &&
            !Flags.getFlag(FlagNameList.manse2F1aGatesOpened))
        {
            GateAndChestManager.addKey(manse2F1AGateKey);

            GateAndChestManager.removeKey(manse2F1cShelfIndexZeroKey);
            GateAndChestManager.addKey(manse2F1cShelfIndexOneKey);

            Flags.setFlag(FlagNameList.manse2F1aGatesOpened, true);
        } 
        
        if(!Flags.getFlag(FlagNameList.revoltStarted) && 
            !Flags.getFlag(FlagNameList.taborRoomShelfSetToNonHostile))
        {
            GateAndChestManager.addKey(manse2F1cShelfIndexZeroKey);
            GateAndChestManager.removeKey(manse2F1cShelfIndexOneKey);
            
            Flags.setFlag(FlagNameList.taborRoomShelfSetToNonHostile,true);
        }
    }
}

public class EnteredManse1F1a : PlayerInteractionScript
{
    private const string manse1F1AGateKey = ZoneKeyList.manseFirstFloor+LocationNameList.section1a+NPCNameList.gate;

    public override void runScript(GameObject target = null)
    {
        if ((Flags.getFlag(FlagNameList.revoltStarted) || 
            AreaList.getArea(ZoneKeyList.manseFirstFloor+LocationNameList.section1a).isHostile()) &&
            !Flags.getFlag(FlagNameList.manse1F1aGatesOpened))
        {
            GateAndChestManager.addKey(manse1F1AGateKey);
            Flags.setFlag(FlagNameList.manse1F1aGatesOpened, true);
        }
    }
}

public class EnteredCampManse : PlayerInteractionScript
{
    private const string manseFrontGateKey = LocationNameList.campManse+NPCNameList.manseFrontDoor;

    public override void runScript(GameObject target = null)
    {
        if ((Flags.getFlag(FlagNameList.revoltStarted) || 
            AreaList.getArea(LocationNameList.campManse).isHostile()) &&
            !Flags.getFlag(FlagNameList.manseFrontDoorsClosed))
        {
            GateAndChestManager.removeKey(manseFrontGateKey);
            Flags.setFlag(FlagNameList.manseFrontDoorsClosed, true);
        }
    }
}

public class EnteredBodyPile : PlayerInteractionScript
{
    public override void runScript(GameObject target = null)
    {
        if (Flags.getFlag(FlagNameList.orderedIntoBodyPile) &&
            !Flags.getFlag(FlagNameList.givenBodyPileEntryQuestStep))
        {
            QuestList.activateQuestStep(QuestNameList.combTheBodiesQuestTitle, QuestNameList.combTheBodiesStepTitleOne);
            Flags.setFlag(FlagNameList.givenBodyPileEntryQuestStep, true);
        }
    }
}

public class EnteredCampNorthWest : PlayerInteractionScript
{
    public override void runScript(GameObject target = null)
    {       
        if (Flags.getFlag(FlagNameList.foundThiefsRing) &&
            !Flags.getFlag(FlagNameList.enteredCampNorthWestAfterBodyPile))
        {
            DialogueManager.getInstance().StartCoroutine(waitTwoFramesThenEnterDialogue());
        }
    }

    private IEnumerator waitTwoFramesThenEnterDialogue()
    {
        yield return null;
        yield return null;

        DialogueManager.getInstance().startDialogue(DialogueList.getDialogue(LocationNameList.campNorthWest, NPCNameList.chiefTabor + 5));
        Flags.setFlag(FlagNameList.enteredCampNorthWestAfterBodyPile, true);
    }
}