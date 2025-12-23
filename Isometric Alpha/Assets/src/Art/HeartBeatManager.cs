using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeartBeatManager : MonoBehaviour
{

    public const float beatLength = 2f;
    private static bool beatIsEven = false;
    private static int currentRow = 0;
    private static float timestamp = 0f;

    public readonly static UnityEvent<int, bool> HeartBeat = new UnityEvent<int, bool>();

    private void Awake()
    {
        timestamp = 0f;
    }

    void Update()
    {
        timestamp += Time.deltaTime;

        if(timestamp > beatLength)
        {
            timestamp -= beatLength;
        }

        invokeHeartBeat();
    }

    private static void invokeHeartBeat()
    {
        HeartBeat.Invoke(currentRow, beatIsEven);
        HeartBeat.Invoke(currentRow+CombatGrid.allyRowUpperBounds, beatIsEven);

        beatIsEven = !beatIsEven;
        currentRow++;

        if(currentRow > CombatGrid.enemyRowLowerBounds)
        {
           currentRow = CombatGrid.enemyRowUpperBounds; 
        }
    }


    // public static float getTimestampToWaitFor(float timeToWait)
    // {
    //     if(timeToWait + timestamp > beatLength)
    //     {
    //         return timeToWait + timestamp - beatLength;
    //     } else
    //     {
    //         return timeToWait + timestamp;
    //     }
    // }

}
