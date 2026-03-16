using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class BinaryDescisionPanel: PopUpWindow
{

    public readonly static UnityEvent AcceptBinaryDecision = new UnityEvent();

	public IDecision decision;
	
	public TextMeshProUGUI message;

    private static BinaryDescisionPanel instance;

    public static BinaryDescisionPanel getInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(instance.gameObject);
        }

        TutorialSequenceStepTargetUIObject.createCutOutMask(transform);

        AcceptBinaryDecision.AddListener(acceptButtonPress);

        instance = this;
    }

    private void OnDestroy()
    {
        AcceptBinaryDecision.RemoveListener(acceptButtonPress);
    }

    public void populate(IDecision decision)
	{
		this.decision = decision;
	
		message.text = decision.getMessage();
	}
	
	public override void acceptButtonPress()
	{
		decision.execute();
		
		base.acceptButtonPress();
	}

	public override void closeButtonPress()
	{
		decision.backOut();
	
		destroyWindow();
		EscapeStack.removeAllNullObjectsFromStack();
	}
}
