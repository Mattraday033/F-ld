using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChoice : MonoBehaviour
{
	private DialogueManager dialogueManager;

	private int number;
	
	void Start()
	{
		dialogueManager = DialogueManager.getInstance();
	}
	
	public void setChoiceNumber(int choiceNumber)
	{
		number = choiceNumber;
	}
	
	public int getChoiceNumber()
	{
		return number;
	}
	
	public void makeChoice()
	{
		dialogueManager.makeChoice(number);
	}
}
