using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class ListenerButtonManager : MonoBehaviour 
{
    private const string finishedFlag = "wisdomPuzzleMineLvl3Completed";

    private static ListenerButtonManager instance;

    public static int codeIndex = 0;
    public static WallType previousWallType = WallType.None;

    public ListenerButton[] buttons;

    public static WallType[] code = new WallType[5] { WallType.RoundRubble, WallType.TripleStalagmite, WallType.SingleStalagmite, WallType.BushRock, WallType.SingleStalagmite };
    public static UnityEvent<bool> FinishedWithListenerButtons = new UnityEvent<bool>();

    public GameObject[] codeGameObjects;

    public GameObject bridge;
    public GameObject water;

    public MonsterPack monsterToSpawn;

    public static bool correctWallType(WallType wallType)
    {
        return wallType == code[codeIndex];
    }

    public static void activateNextCodeObject()
    {
        if (codeIndex >= ListenerButtonManager.getInstance().codeGameObjects.Length)
        {
            return;
        }

        ListenerButtonManager.getInstance().codeGameObjects[codeIndex + 1].SetActive(true);
    }

    public static void incrementCodeIndex()
    {
        codeIndex++;
    }

    public static void resetCodeGameObjects()
    {
        codeIndex = 0;

        for (int i = 1; i < ListenerButtonManager.getInstance().codeGameObjects.Length; i++)
        {
            ListenerButtonManager.getInstance().codeGameObjects[i].SetActive(false);
        }

        Flags.setFlag(FlagNameList.wisdomPuzzleMineLvl3TwoObjects, false);
        Flags.setFlag(FlagNameList.wisdomPuzzleMineLvl3ThreeObjects, false);
        Flags.setFlag(FlagNameList.wisdomPuzzleMineLvl3FourObjects, false);
        Flags.setFlag(FlagNameList.wisdomPuzzleMineLvl3FiveObjects, false);

        if (previousWallType == WallType.RoundRubble)
        {
            previousWallType = WallType.None;
        }
    }

    public void setActiveButton(WallType wallType)
    {
        if (wallType == WallType.None)
        {
            return;
        }

        int activeButtonIndex = ((int)wallType) - 1;

        buttons[activeButtonIndex].setActive(true);
    }

    public static void activateBridge()
    {
        getInstance().water.SetActive(false);
        getInstance().bridge.SetActive(true);
    }

    public static bool isFinished()
    {
        return Flags.getFlag(finishedFlag);
    }

    public static void markAsFinished()
    {
        Flags.setFlag(finishedFlag, true);
    }

    public void activateCodeGameObjects(int amount)
    {
        foreach (GameObject codeObject in codeGameObjects)
        {
            codeObject.SetActive(false);
        }

        for (int i = 0; i < amount; i++)
        {
            codeGameObjects[i].SetActive(true);
        }
    }

    public static ListenerButtonManager getInstance()
    {
        return instance;
    }

    public static void setSaveFlag()
    {
        Flags.setFlag(FlagNameList.wisdomPuzzleMineLvl3TwoObjects, false);
        Flags.setFlag(FlagNameList.wisdomPuzzleMineLvl3ThreeObjects, false);
        Flags.setFlag(FlagNameList.wisdomPuzzleMineLvl3FourObjects, false);
        Flags.setFlag(FlagNameList.wisdomPuzzleMineLvl3FiveObjects, false);

        switch (codeIndex)
        {
            case 1:
                Flags.setFlag(FlagNameList.wisdomPuzzleMineLvl3TwoObjects, true);
                break;
            case 2:
                Flags.setFlag(FlagNameList.wisdomPuzzleMineLvl3ThreeObjects, true);
                break;
            case 3:
                Flags.setFlag(FlagNameList.wisdomPuzzleMineLvl3FourObjects, true);
                break;
            case 4:
                Flags.setFlag(FlagNameList.wisdomPuzzleMineLvl3FiveObjects, true);
                break;
        }
    }

    public static void setPreviousWallType(WallType wallType)
    {
        previousWallType = wallType;

		Flags.setFlag(FlagNameList.prevWTRoundRubble, false);
		Flags.setFlag(FlagNameList.prevWTSingleStalagmite, false);
		Flags.setFlag(FlagNameList.prevWTTripleStalagmite, false);
		Flags.setFlag(FlagNameList.prevWTBushRock, false);

        switch (wallType)
        {
            case WallType.RoundRubble:
                Flags.setFlag(FlagNameList.prevWTRoundRubble, true);
                break;
            case WallType.SingleStalagmite:
                Flags.setFlag(FlagNameList.prevWTSingleStalagmite, true);
                break;
            case WallType.TripleStalagmite:
                Flags.setFlag(FlagNameList.prevWTTripleStalagmite, true);
                break;
            case WallType.BushRock:
                Flags.setFlag(FlagNameList.prevWTBushRock, true);
                break;
        }
    }

    public static void getPreviousWallTypeFromFlags()
    {
        if (Flags.getFlag(FlagNameList.prevWTRoundRubble))
        {
            previousWallType = WallType.RoundRubble;
        }
        else if (Flags.getFlag(FlagNameList.prevWTSingleStalagmite))
        {
            previousWallType = WallType.SingleStalagmite;
        }
        else if (Flags.getFlag(FlagNameList.prevWTTripleStalagmite))
        {
            previousWallType = WallType.TripleStalagmite;
        }
        else if (Flags.getFlag(FlagNameList.prevWTBushRock))
        {
            previousWallType = WallType.BushRock;
        }
        else
        {
            previousWallType = WallType.None;
        }

        getInstance().setActiveButton(previousWallType);
    }

    public void setCodeGameObjectsToMirrorSaveFlag()
    {
        if (Flags.getFlag(FlagNameList.wisdomPuzzleMineLvl3TwoObjects))
        {
            codeIndex = 1;
        }
        else if (Flags.getFlag(FlagNameList.wisdomPuzzleMineLvl3ThreeObjects))
        {
            codeIndex = 2;
        }
        else if (Flags.getFlag(FlagNameList.wisdomPuzzleMineLvl3FourObjects))
        {
            codeIndex = 3;
        }
        else if (Flags.getFlag(FlagNameList.wisdomPuzzleMineLvl3FiveObjects))
        {
            codeIndex = 4;
        }
        else
        {
            codeIndex = 0;
        }

        activateCodeGameObjects(codeIndex + 1);
    }

    public void spawnMonster()
    {
        if (monsterToSpawn.monsterSprite != null && monsterToSpawn.monsterSprite.activeSelf)
        {
            return;
        }
        else if (monsterToSpawn.monsterSprite != null && !monsterToSpawn.monsterSprite.activeSelf)
        {
            DestroyImmediate(monsterToSpawn.monsterSprite);
        }
        
        monsterToSpawn.facingDirection = Facing.NorthEast;

        monsterToSpawn = MonsterPackListManager.getInstance().instantiateMonsterSpriteAtStartingPosition(monsterToSpawn.index, monsterToSpawn);

        MovementManager.getInstance().addEnemySprite(monsterToSpawn.monsterSprite.transform, monsterToSpawn.index + 1);

        FadeToBlackManager.getInstance().setAndStartFadeBackIn();
    }

    private void Awake()
    {
        if (instance != null)
        {
            throw new IOException("There is already an instance of ListenerButtonManager");
        }

        instance = this;

        if (isFinished())
        {
            activateCodeGameObjects(codeGameObjects.Length);
            activateBridge();
        }
        else
        {
            setCodeGameObjectsToMirrorSaveFlag();
        }
    }

    void Start()
    {
        if (State.currentMonsterPackList != null && State.currentMonsterPackList.monsterPacks != null && State.currentMonsterPackList.monsterPacks.Length > 0)
        {
            monsterToSpawn = State.currentMonsterPackList.monsterPacks[0];
        }
        
        getPreviousWallTypeFromFlags();
    }
}
