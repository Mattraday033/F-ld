using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScreenButton : PopUpButton
{

	public LoadScreenButton():
	base(PopUpType.LoadOnlyScreen)
	{
		
	}

    // Update is called once per frame
    void Update()
    {
		KeyPressManager.updateKeyBools();
		
		if(KeyBindingList.eitherBackoutKeyIsPressed() && !KeyPressManager.handlingPrimaryKeyPress)
		{
			EscapeStack.handleEscapePress();
		}
    }
	
	private void Awake()
	{
        SceneChange.addOOCUIScene();
	}

    private void OnDestroy()
    {
        EscapeStack.escapeAll();

        OverallUIManager.setCurrentScreenType(null);
    }

    public override void spawnPopUp()
	{
        if (CombatStateManager.inCombat && CombatStateManager.whoseTurn == WhoseTurn.Lost)
        {
            SceneManager.UnloadSceneAsync("Combat UI"); 
        }

        if(!CombatStateManager.inCombat)
        {
		    OverallUIManager.UIParentPanel.SetActive(true); 
        }
		
		Instantiate(Resources.Load<GameObject>(PrefabNames.saveScreen), PopUpScreenBlockerManager.getPopUpParent(PopUpType.LoadOnlyScreen)); 
		
		OverallUIManager.setCurrentScreenType(getCurrentPopUpGameObject().GetComponent<SaveHandler>());
		
		EscapeStack.addEscapableObject(getCurrentPopUpGameObject().GetComponent<SaveHandler>()); 
	}

    public override GameObject getCurrentPopUpGameObject()
    {
        if (SaveHandler.getInstance() != null && !(SaveHandler.getInstance() is null))
        {
            return SaveHandler.getInstance().gameObject;
        }
        else
        {
            return null;
        }
    }
}
