using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageNumberPopupQueue : MonoBehaviour
{
    private static DamageNumberPopupQueue instance;

    public Dictionary<GridCoords, List<DamageNumberPopup>> dictionaryOfQueues = new Dictionary<GridCoords, List<DamageNumberPopup>>();

    private void Awake()
    {
        instance = this;
        HeartBeatManager.MediumHeartBeat.AddListener(revealNextWaveOfPopups);
    }

    private void OnDestroy()
    {
        HeartBeatManager.MediumHeartBeat.RemoveListener(revealNextWaveOfPopups);
    }

    public static void addDamageNumberToQueue(GridCoords coords, DamageNumberPopup popUp)
    {
        if(!instance.dictionaryOfQueues.ContainsKey(coords))
        {
            instance.dictionaryOfQueues[coords] = new List<DamageNumberPopup>();
        }

        instance.dictionaryOfQueues[coords].Add(popUp);
    }

    public static void revealNextWaveOfPopups(int row)
    {
        if(row > CombatGrid.enemyRowLowerBounds)
        {
            return;
        }

        List<GridCoords> queuesToRemove = new List<GridCoords>();

        foreach(KeyValuePair<GridCoords,List<DamageNumberPopup>> kvp in instance.dictionaryOfQueues)
        {
            bool emptyQueue = setNextPopUpInQueueVisible(kvp.Value);

            if(emptyQueue)
            {
                queuesToRemove.Add(kvp.Key);
            }
        }

        foreach(GridCoords coords in queuesToRemove)
        {
            instance.dictionaryOfQueues.Remove(coords);
        }
    }

    public static bool setNextPopUpInQueueVisible(List<DamageNumberPopup> queue)
    {
        foreach(DamageNumberPopup popUp in queue)
        {
            if(popUp == null)
            {
                continue;
            }

            if(!popUp.isVisible())
            {
                popUp.setToVisible();
                return false;
            }
        }

        return true;
    }

}
