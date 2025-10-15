using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class BinaryDescisionPanel: PopUpWindow
{
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

        instance = this;
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
