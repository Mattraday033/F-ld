using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharGenDescriptionPanel : MonoBehaviour
{
    public Transform dialogueStarParent;
    public Transform combatStarParent;
    public Transform mobilityStarParent;

    public void spawnStars(PrimaryStat currentPrimaryStat)
    {
        destroyAllStars();

        for(int starIndex = 0; starIndex < getDialogueStarAmount(currentPrimaryStat); starIndex++)
        {
            Instantiate(Resources.Load<GameObject>(PrefabNames.star), dialogueStarParent);
        }

        for(int starIndex = 0; starIndex < getCombatStarAmount(currentPrimaryStat); starIndex++)
        {
            Instantiate(Resources.Load<GameObject>(PrefabNames.star), combatStarParent);
        }

        for(int starIndex = 0; starIndex < getMobilityStarAmount(currentPrimaryStat); starIndex++)
        {
            Instantiate(Resources.Load<GameObject>(PrefabNames.star), mobilityStarParent);
        }
    }

    private void destroyAllStars()
    {
        foreach(Transform child in combatStarParent)
        {
            Destroy(child.gameObject);
        }

        foreach(Transform child in dialogueStarParent)
        {
            Destroy(child.gameObject);
        }

        foreach(Transform child in mobilityStarParent)
        {
            Destroy(child.gameObject);
        }
    }

    private int getDialogueStarAmount(PrimaryStat currentPrimaryStat)
    {
        switch(currentPrimaryStat)
        {
            case PrimaryStat.Strength:
                return Constants.sizeThree;
            case PrimaryStat.Dexterity:
                return Constants.sizeOne;
            case PrimaryStat.Wisdom:
                return Constants.sizeThree;
            default:
                return Constants.sizeFive;
        }
    }

    private int getCombatStarAmount(PrimaryStat currentPrimaryStat)
    {
        switch(currentPrimaryStat)
        {
            case PrimaryStat.Strength:
                return Constants.sizeFive;
            case PrimaryStat.Dexterity:
                return Constants.sizeFour;
            case PrimaryStat.Wisdom:
                return Constants.sizeFour;
            default:
                return Constants.sizeThree;
        }
    }

    private int getMobilityStarAmount(PrimaryStat currentPrimaryStat)
    {
        switch(currentPrimaryStat)
        {
            case PrimaryStat.Strength:
                return Constants.sizeTwo;
            case PrimaryStat.Dexterity:
                return Constants.sizeFive;
            case PrimaryStat.Wisdom:
                return Constants.sizeThree;
            default:
                return Constants.sizeTwo;
        }
    }

}
