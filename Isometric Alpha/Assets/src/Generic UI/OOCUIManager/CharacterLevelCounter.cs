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
            
            switch(PlayerOOCStateManager.currentActivity)
            {
                case OOCActivity.walking:
                case OOCActivity.inFade:
                case OOCActivity.Loading:
                    playerLevelUpTutorialSequenceCheck();
                    return;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void playerLevelUpTutorialSequenceCheck()
    {
        if (!TutorialFlags.getFlag(TutorialSequenceList.playerLevelUpTutorialSeenFlag) && State.dialogueUponSceneLoadKey == null)
        {
            TutorialSequence.startTutorialSequence(getPlayerLevelUpTutorialSequence());
        }
    }

    private TutorialSequence getPlayerLevelUpTutorialSequence()
    {
        return TutorialSequenceList.getTutorialSequence(TutorialSequenceList.playerLevelUpTutorialSequenceKey);
    }
}
