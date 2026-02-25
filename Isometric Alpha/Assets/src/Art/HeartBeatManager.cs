using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeartBeatManager : MonoBehaviour
{

    private static HeartBeatManager fastHeartBeatManager;
    private static HeartBeatManager slowHeartBeatManager;

    public const float slowBeatLengthSeconds = .75f;
    public const float fastBeatLengthSeconds = .05f;
    private bool beatIsEven = false;
    public int currentRow = 0;
    public float timestamp = 0f;
    public bool isFast = false;

    private Dictionary<int, int> beatsPerRow;

    public readonly static UnityEvent<int> SlowHeartBeat = new UnityEvent<int>();
    public readonly static UnityEvent<int> FastHeartBeat = new UnityEvent<int>();

    public void Awake()
    {
        timestamp = 0f;

        beatsPerRow = new Dictionary<int, int>();

        for(int rowIndex = 0; rowIndex <= CombatGrid.allyRowLowerBounds; rowIndex++)
        {
            beatsPerRow[rowIndex] = 0;
        }

        if(isFast)
        {
            fastHeartBeatManager = this;
        } else
        {
            slowHeartBeatManager = this;
        }
    }

    void Update()
    {
        float addedTime = Time.deltaTime;

        timestamp += addedTime;

        if(timestamp > getBeatLength())
        {
            timestamp -= getBeatLength();
            invokeHeartBeat();
        }
    }

    private void invokeHeartBeat()
    {
        trackBeatsPerRow(currentRow);

        UnityEvent<int> heartBeat = getHeartBeat(isFast);

        heartBeat.Invoke(currentRow);
        heartBeat.Invoke(currentRow+CombatGrid.allyRowUpperBounds);

        beatIsEven = !beatIsEven;
        currentRow++;

        if(currentRow > CombatGrid.enemyRowLowerBounds)
        {
           currentRow = CombatGrid.enemyRowUpperBounds; 
        }
    }

    private float getBeatLength()
    {
        if(isFast)
        {
            return fastBeatLengthSeconds;
        } else
        {
            return slowBeatLengthSeconds;
        }
    }

    private void trackBeatsPerRow(int row)
    {
        beatsPerRow[row]++;
        beatsPerRow[currentRow+CombatGrid.allyRowUpperBounds]++;
    }

    public static int getHeartBeatsSentToRow(string monsterName, int row)
    {
        if(useFastHeartBeatInAnimation(monsterName))
        {
            return fastHeartBeatManager.getBeatsSentToRow(row);
        } else
        {
            return slowHeartBeatManager.getBeatsSentToRow(row);
        }
    }

    private int getBeatsSentToRow(int row)
    {
        if(beatsPerRow == null)
        {
            return 0;
        }

        return beatsPerRow[row];
    }

    private static UnityEvent<int> getHeartBeat(bool isFast)
    {
        if(isFast)
        {
            return FastHeartBeat;
        }else
        {
            return SlowHeartBeat;
        }
    }
    public static UnityEvent<int> getHeartBeat(string enemyType)
    {
        return getHeartBeat(useFastHeartBeatInAnimation(enemyType));
    }

    private static bool useFastHeartBeatInAnimation(string enemyType)
    {
        switch(enemyType)
        {
            case MonsterNameList.denMother:
            case MonsterNameList.batSwarm:
            case MonsterNameList.giantBat:         
                return true;
            default:
                return false;
        }
    }
}

public static class IdleDictionary
{
    private static Dictionary<KeyValuePair<string, CharacterAnimationType>, Sprite[]> idleDict;

    public static bool idleDictContainsSprites(string monsterName, CharacterAnimationType animationType)
    {
        if(idleDict == null)
        {
            return false;
        }

        return idleDict.ContainsKey(new KeyValuePair<string, CharacterAnimationType>(monsterName, animationType));
    }

    public static void addSpritesToIdleDict(string monsterName, CharacterAnimationType animationType, Sprite[] sprites)
    {
        if(sprites == null || sprites.Length <= 0)
        {
            return;
        }

        idleDict[new KeyValuePair<string, CharacterAnimationType>(monsterName, animationType)] = sprites;
    }

    public static Sprite getCurrentIdleSprite(int row, string monsterName, CharacterAnimationType animationType)
    {
        if(!idleDictContainsSprites(monsterName, animationType))
        {
            // Debug.LogError("no "+animationType.ToString()+" entry for "+monsterName);    

            // foreach(KeyValuePair<KeyValuePair<string, CharacterAnimationType>, Sprite[]> kvp in idleDict)
            // {
            //     if(kvp.Key.Key.Equals(monsterName))
            //     {
            //         Debug.LogError(monsterName + " has entry for " + kvp.Key.Value.ToString());    
            //     }
            // }

            return null;
        }

        if(!CombatStateManager.inCombat && alwaysUseOOCIdleOutsideOfCombat(monsterName))
        {
            switch(animationType)
            {
                case CharacterAnimationType.Idle_Front:
                    animationType = CharacterAnimationType.OOC_Idle_Front;
                    break;
                case CharacterAnimationType.Idle_Back:    
                    animationType = CharacterAnimationType.OOC_Idle_Back;
                    break;
            } 
        }


        Sprite[] currentIdleSprites = idleDict[new KeyValuePair<string, CharacterAnimationType>(monsterName, animationType)];

        int beats = HeartBeatManager.getHeartBeatsSentToRow(monsterName, row);

        int currentSpriteIndex = beats % currentIdleSprites.Length;

        return currentIdleSprites[currentSpriteIndex];
    }

    private static bool alwaysUseOOCIdleOutsideOfCombat(string monsterName)
    {
        switch(monsterName)
        {
            case MonsterNameList.direWorm:
                return true;
            default:
                return false;
        }
    }

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateIdleDictionary()
    {
        idleDict = new Dictionary<KeyValuePair<string, CharacterAnimationType>, Sprite[]>();

        CombatStateManager.OnCombatStart.AddListener(purgeIdleDictionary);
    }

    private static void purgeIdleDictionary()
    {
        idleDict = new Dictionary<KeyValuePair<string, CharacterAnimationType>, Sprite[]>();
    }
}