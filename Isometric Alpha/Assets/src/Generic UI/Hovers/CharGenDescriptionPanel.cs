using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharGenDescriptionPanel : MonoBehaviour
{

    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    public Transform dialogueStarParent;
    public Transform combatStarParent;
    public Transform mobilityStarParent;

    private void Awake()
    {
        title.text = getTitle();
        description.text = getDescription();
        
        spawnStars();
    }

    private void spawnStars()
    {
        for(int starIndex = 0; starIndex < getDialogueStarAmount(); starIndex++)
        {
            Instantiate(Resources.Load<GameObject>(PrefabNames.star), dialogueStarParent);
        }

        for(int starIndex = 0; starIndex < getCombatStarAmount(); starIndex++)
        {
            Instantiate(Resources.Load<GameObject>(PrefabNames.star), combatStarParent);
        }

        for(int starIndex = 0; starIndex < getMobilityStarAmount(); starIndex++)
        {
            Instantiate(Resources.Load<GameObject>(PrefabNames.star), mobilityStarParent);
        }
    }

    private static string getTitle()
    {
        return CharGenStatHover.currentPrimaryStat.ToString();
    }

    private static string getDescription()
    {
        switch(CharGenStatHover.currentPrimaryStat)
        {
            case PrimaryStat.Strength:
                return Strength.startingDescription;
            case PrimaryStat.Dexterity:
                return Dexterity.startingDescription;
            case PrimaryStat.Wisdom:
                return Wisdom.startingDescription;
            default:
                return Charisma.startingDescription;
        }
    }

    private static int getDialogueStarAmount()
    {
        switch(CharGenStatHover.currentPrimaryStat)
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

    private static int getCombatStarAmount()
    {
        switch(CharGenStatHover.currentPrimaryStat)
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

    private static int getMobilityStarAmount()
    {
        switch(CharGenStatHover.currentPrimaryStat)
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
