using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddStatPoint : IDecision
{
    private AllyStats targetStats;

    public string getMessage()
    {
        return "Are you sure you want to raise "+targetStats.getName()+"'s " + PrimaryStatIncreaseButton.currentButton.getStatName() + " by 1? This is permanent and costs 1000 Experience Points.";
    }

	public AddStatPoint(Stats targetStats)
	{
		this.targetStats = targetStats as AllyStats;
	}

    public void execute()
    {
        string currentStatSymbol = PrimaryStatIncreaseButton.currentButton.getStatSymbol();

        switch (currentStatSymbol)
        {
            case Strength.symbolChar:
                targetStats.incrementStrength();
                break;
            case Dexterity.symbolChar:
                targetStats.incrementDexterity();
                break;
            case Wisdom.symbolChar:
                targetStats.incrementWisdom();
                break;
            case Charisma.symbolChar:
                targetStats.incrementCharisma();
                break;
        }

        targetStats.removeXPFromLevelUpOnce();
        targetStats.incrementLevel();
        targetStats.fullHeal();

        Stats.OnStatsChange.Invoke();

        if (!Flags.getFlag(TutorialSequenceList.addingAbilitiesTutorialSeenFlag))
        {
            TutorialSequence.startTutorialSequence(TutorialSequenceList.addingAbilitiesTutorialSequenceKey);
        }
    }

    public void backOut()
    {

    }


}
