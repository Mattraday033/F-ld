using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeathStep: QuestStep {

	public string deadName;
	public int firstStep;
	public int lastStep;
	public int currentStepOnDeath = -1;
	public bool failOnActivation = false;
	public bool succeedOnActivation = false;
	
	
	public DeathStep(Quest parentQuest, bool active, int stepIndex, string stepName, string journalDescription,
					 string deadName, int firstStep, int lastStep): base(parentQuest, active, stepIndex, stepName, journalDescription)
	{
		this.deadName = deadName;
		this.firstStep = firstStep;
		this.lastStep = lastStep;
	}
	
	public DeathStep(Quest parentQuest, bool active, int stepIndex, string stepName, string journalDescription,
					 string deadName, int firstStep, int lastStep, bool failOnActivation, bool succeedOnActivation): base(parentQuest, active, stepIndex, stepName, journalDescription)
	{
		this.deadName = deadName;
		this.firstStep = firstStep;
		this.lastStep = lastStep;
		
		this.failOnActivation = failOnActivation;
		this.succeedOnActivation = succeedOnActivation;
	}
}
