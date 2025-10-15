using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartyMemberUpgradeCounter : MonoBehaviour
{

    public TextMeshProUGUI counterText;

    public void setCounter()
    {
        if (Flags.isInNewGameMode())
        {
            return;
        }

        int upgradablePartyMemberCount = PartyManager.getNumberOfUpgradablePartyMembers();

        if (upgradablePartyMemberCount > 0)
        {
            counterText.text = "" + upgradablePartyMemberCount;
            gameObject.SetActive(true);
            partyMemberUpgradeTutorialSequenceCheck();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void partyMemberUpgradeTutorialSequenceCheck()
    {
        if (!Flags.getFlag(TutorialSequenceList.partyMemberUpgradeTutorialSeenFlag))
        {
            TutorialSequence.startTutorialSequence(getPartyMemberUpgradeTutorialSequence());
        }
    }

    private TutorialSequence getPartyMemberUpgradeTutorialSequence()
    {
        return TutorialSequenceList.getTutorialSequence(TutorialSequenceList.partyMemberUpgradeTutorialSequenceKey);
    }
}
