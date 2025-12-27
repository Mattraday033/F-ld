using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeartBeatManager : MonoBehaviour
{

    public const float beatLengthSeconds = 1.2f;
    private static bool beatIsEven = false;
    private static int currentRow = 0;
    private static float timestamp = 0f;

    private static Dictionary<int, int> beatsPerRow;

    public readonly static UnityEvent<int, bool> HeartBeat = new UnityEvent<int, bool>();

    private void Awake()
    {
        timestamp = 0f;

        beatsPerRow = new Dictionary<int, int>();

        for(int rowIndex = 0; rowIndex <= CombatGrid.allyRowLowerBounds; rowIndex++)
        {
            beatsPerRow[rowIndex] = 0;
        }
    }

    void Update()
    {
        timestamp += Time.deltaTime;

        if(timestamp > beatLengthSeconds)
        {
            timestamp -= beatLengthSeconds;
            invokeHeartBeat();
        }
    }

    private static void trackBeatsPerRow(int row)
    {
        beatsPerRow[row]++;
        beatsPerRow[currentRow+CombatGrid.allyRowUpperBounds]++;
    }

    private static void invokeHeartBeat()
    {
        trackBeatsPerRow(currentRow);

        HeartBeat.Invoke(currentRow, beatIsEven);
        HeartBeat.Invoke(currentRow+CombatGrid.allyRowUpperBounds, beatIsEven);

        beatIsEven = !beatIsEven;
        currentRow++;

        if(currentRow > CombatGrid.enemyRowLowerBounds)
        {
           currentRow = CombatGrid.enemyRowUpperBounds; 
        }
    }

    public static int getBeatsSentToRow(int row)
    {
        if(beatsPerRow == null)
        {
            return 0;
        }

        return beatsPerRow[row];
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
            Debug.LogError("no "+animationType.ToString()+" entry for "+monsterName);    

            foreach(KeyValuePair<KeyValuePair<string, CharacterAnimationType>, Sprite[]> kvp in idleDict)
            {
                if(kvp.Key.Key.Equals(monsterName))
                {
                    Debug.LogError(monsterName + " has entry for " + kvp.Key.Value.ToString());    
                }
            }

            return null;
        }

        Sprite[] currentIdleSprites = idleDict[new KeyValuePair<string, CharacterAnimationType>(monsterName, animationType)];

        int beats = HeartBeatManager.getBeatsSentToRow(row);

        int currentSpriteIndex = beats % currentIdleSprites.Length;

        return currentIdleSprites[currentSpriteIndex];
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