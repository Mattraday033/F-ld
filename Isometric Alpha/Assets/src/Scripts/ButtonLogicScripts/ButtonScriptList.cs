using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ButtonScriptList
{

    private static Dictionary<string, List<ButtonLogicScript>> scriptDict;

    public static List<ButtonLogicScript> getButtonScripts(string locationName)
    {
        if (!scriptDict.ContainsKey(locationName))
        {
            return new List<ButtonLogicScript>();
        }

        return scriptDict[locationName];

    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeScriptDict()
    {
        scriptDict = new Dictionary<string, List<ButtonLogicScript>>();
        List<ButtonLogicScript> list;

        #region 6SlaveShack

        list = new List<ButtonLogicScript>();

        list.Add(new OpenGateButtonLogicScript(Constants.indexZero, Constants.sizeTwo, NPCNameList.fallenBeam));

        scriptDict.Add(LocationNameList.slaveShackSix, list);

        #endregion

        #region MineLvl_1

        #region MineLvl_1-1b

        list = new List<ButtonLogicScript>();

        list.Add(new OpenGateButtonLogicScript(Constants.indexZero, Constants.sizeTwo, NPCNameList.awkwardRubble));

        scriptDict.Add(LocationNameList.mineLvl1 + LocationNameList.section1b, list);

        #endregion

        #endregion

        #region MineLvl_2

        #region MineLvl_2-2b

        list = new List<ButtonLogicScript>();

        list.Add(new OpenGateButtonLogicScript(Constants.indexZero, Constants.sizeOne, NPCNameList.ancientPortcullis));

        scriptDict.Add(LocationNameList.mineLvl2 + LocationNameList.section2b, list);

        #endregion
        #region MineLvl_2-3a

        list = new List<ButtonLogicScript>();

        list.Add(new OpenGateButtonLogicScript(Constants.indexZero, Constants.sizeTwo, NPCNameList.awkwardRubble));
        list.Add(new OpenGateButtonLogicScript(Constants.indexOne, Constants.sizeTwo, NPCNameList.awkwardRubble));

        scriptDict.Add(LocationNameList.mineLvl2 + LocationNameList.section3a, list);

        #endregion
        #region MineLvl_2-3b

        list = new List<ButtonLogicScript>();

        list.Add(new OpenGateButtonLogicScript(Constants.indexZero, Constants.sizeThree, NPCNameList.ancientPortcullis));

        scriptDict.Add(LocationNameList.mineLvl2 + LocationNameList.section3b, list);

        #endregion

        #region MineLvl_2-6

        list = new List<ButtonLogicScript>(); //Constants.index

        int[] gatesAttachedToButtonIndexZero = new int[] { Constants.indexFive, Constants.indexSix , Constants.indexEight};
        int[] gatesAttachedToButtonIndexOne = new int[] { Constants.indexSeven, Constants.indexTwelve};
        int[] gatesAttachedToButtonIndexTwo = new int[] { Constants.indexSeven, Constants.indexEight, Constants.indexNine};
        int[] gatesAttachedToButtonIndexThree = new int[] {Constants.indexTwo, Constants.indexSeven, Constants.indexNine, Constants.indexEleven, Constants.indexTwelve};
        int[] gatesAttachedToButtonIndexFour = new int[] { Constants.indexZero, Constants.indexOne, Constants.indexFive};
        int[] gatesAttachedToButtonIndexFive = new int[] { Constants.indexFive, Constants.indexSix };
        int[] gatesAttachedToButtonIndexSix = new int[] { Constants.indexEight };
        int[] gatesAttachedToButtonIndexSeven = new int[] { Constants.indexEight, Constants.indexTen};

        int[] gatesOpenAtStart = new int[] {  Constants.indexZero, Constants.indexTwo, Constants.indexFive};

        Dictionary<int, int[]> gatesPerButton = new Dictionary<int, int[]>();

        gatesPerButton.Add(Constants.indexZero, gatesAttachedToButtonIndexZero); 
        gatesPerButton.Add(Constants.indexOne, gatesAttachedToButtonIndexOne); 
        gatesPerButton.Add(Constants.indexTwo, gatesAttachedToButtonIndexTwo); 
        gatesPerButton.Add(Constants.indexThree, gatesAttachedToButtonIndexThree); 
        gatesPerButton.Add(Constants.indexFour, gatesAttachedToButtonIndexFour); 
        gatesPerButton.Add(Constants.indexFive, gatesAttachedToButtonIndexFive); 
        gatesPerButton.Add(Constants.indexSix, gatesAttachedToButtonIndexSix); 
        gatesPerButton.Add(Constants.indexSeven, gatesAttachedToButtonIndexSeven); 

        list.Add(new OnOffButtonLogicScript(NPCNameList.ancientPortcullis, gatesPerButton, gatesOpenAtStart));

        scriptDict.Add(LocationNameList.mineLvl2 + LocationNameList.section6, list);

        #endregion

        #region MineLvl_2-7a

        list = new List<ButtonLogicScript>();

        list.Add(new OpenGateButtonLogicScript(Constants.indexZero, Constants.sizeOne, NPCNameList.ancientPortcullis));
        list.Add(new OpenGateButtonLogicScript(Constants.indexOne, Constants.sizeOne, NPCNameList.ancientPortcullis));
        list.Add(new OpenGateButtonLogicScript(Constants.indexTwo, Constants.sizeOne, NPCNameList.ancientPortcullis));
        list.Add(new OpenGateButtonLogicScript(Constants.indexThree, Constants.sizeOne, NPCNameList.ancientPortcullis));

        scriptDict.Add(LocationNameList.mineLvl2 + LocationNameList.section7a, list);

        #endregion

        #region MineLvl_2-7b

        list = new List<ButtonLogicScript>();

        list.Add(new OpenGateButtonLogicScript(Constants.indexZero, Constants.sizeTwo, NPCNameList.awkwardRubble));
        list.Add(new OpenGateButtonLogicScript(Constants.indexOne, Constants.sizeOne, NPCNameList.ancientPortcullis));

        scriptDict.Add(LocationNameList.mineLvl2 + LocationNameList.section7b, list);

        #endregion   
        #endregion      

        #region MineLvl_3

        #region MineLvl_3-1b

        list = new List<ButtonLogicScript>();

        list.Add(new ButtonOrderLogicScript(new int[]{Constants.indexZero, Constants.indexOne, Constants.indexTwo,  Constants.indexThree, Constants.indexOne, Constants.indexTwo},
                                            new Vector3Int[]{new Vector3Int(17,2), new Vector3Int(17,0), new Vector3Int(17,-2), new Vector3Int(17,-4), new Vector3Int(17,-6)},
                                            new string[]{PrefabNames.tripleStalagmite, PrefabNames.singleStalagmite, PrefabNames.mediumBushStalagmite, PrefabNames.tripleStalagmite, PrefabNames.lowStalagmite},
                                            SecretDoorKeyList.mineLvl3PuzzleFinished,
                                            ColorList.mineLvl3RubbleColor));

        scriptDict.Add(LocationNameList.mineLvl3 + LocationNameList.section1b, list);

        #endregion

        #region MineLvl_3-2b

        list = new List<ButtonLogicScript>();

        list.Add(new OpenGateButtonLogicScript(Constants.indexZero, Constants.sizeThree, NPCNameList.ancientPortcullis + 1));

        list.Add(new OpenGateButtonLogicScript(Constants.indexOne, Constants.sizeOne, NPCNameList.ancientPortcullis));

        scriptDict.Add(LocationNameList.mineLvl3 + LocationNameList.section2b, list);

        #endregion

        #region MineLvl_3-4b

        list = new List<ButtonLogicScript>();

        list.Add(new OpenGateButtonLogicScript(Constants.indexZero, Constants.sizeTwo, NPCNameList.ancientPortcullis));

        list.Add(new OpenGateButtonLogicScript(Constants.indexOne, Constants.sizeOne, NPCNameList.ancientPortcullis));

        scriptDict.Add(LocationNameList.mineLvl3 + LocationNameList.section4b, list);

        #endregion
        #region MineLvl_3-6a

        list = new List<ButtonLogicScript>();

        list.Add(new OpenGateButtonLogicScript(Constants.indexZero, Constants.sizeSeven, NPCNameList.ancientPortcullis));

        scriptDict.Add(LocationNameList.mineLvl3 + LocationNameList.section6a, list);

        #endregion
        #region MineLvl_3-7

        list = new List<ButtonLogicScript>();

        list.Add(new OpenGateButtonLogicScript(Constants.indexZero, Constants.sizeSix, NPCNameList.ancientPortcullis));

        scriptDict.Add(LocationNameList.mineLvl3 + LocationNameList.section7, list);

        #endregion
        #endregion     

    }



}
