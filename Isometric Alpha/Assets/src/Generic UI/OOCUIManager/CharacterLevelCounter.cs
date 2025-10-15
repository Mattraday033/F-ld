using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterLevelCounter : MonoBehaviour
{

    public TextMeshProUGUI counterText;
    public DescriptionPanel panel;

    public void setCounter()
    {
        if (Flags.isInNewGameMode())
        {
            return;
        }

        if (PartyStats.partyMemberCanLevelUp())
        {
            gameObject.SetActive(true);
            playerLevelUpTutorialSequenceCheck();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void playerLevelUpTutorialSequenceCheck()
    {
        if (!Flags.getFlag(TutorialSequenceList.playerLevelUpTutorialSeenFlag))
        {
            TutorialSequence.startTutorialSequence(getPlayerLevelUpTutorialSequence());
        }
    }

    private TutorialSequence getPlayerLevelUpTutorialSequence()
    {
        return TutorialSequenceList.getTutorialSequence(TutorialSequenceList.playerLevelUpTutorialSequenceKey);
    }
}
