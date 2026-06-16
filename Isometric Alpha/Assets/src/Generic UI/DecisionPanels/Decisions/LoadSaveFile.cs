using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public interface IDecisionPanel
{
	public GameObject getGameObject();

	public void setObjectToBeDecidedOn(IDescribable describable);

	public void setScrollableUIElement(ScrollableUIElement grid);

	public void setCollectionIndex(int currentTabCollection);

	public void updateEnabledButtons();

	public string getDescribableRowKey();
}

public delegate void OutroLogic();

public class LoadSaveFile: IDecision
{
    private const string loadLostProgressMessageStart = "Are you sure you want to load '";
    private const string loadLostProgressMessageEnd = "'? Any unsaved progress will be lost.";

    public static bool midLoad = false;
    public static bool beforeSecondStageLoad = false;

    public readonly static UnityEvent OnLoadResetData = new UnityEvent();
    public readonly static UnityEvent<SaveBlueprint> OnLoadReadBlueprint = new UnityEvent<SaveBlueprint>();

    public OOCActivity exitActivity;
    public SaveBlueprint saveBlueprint;
    public bool showMonologueFirst;

    public LoadSaveFile(SaveBlueprint saveBlueprint, OOCActivity exitActivity = OOCActivity.walking, bool showMonologueFirst = false)
    {
        this.saveBlueprint = saveBlueprint;

        this.exitActivity = exitActivity;
        this.showMonologueFirst = showMonologueFirst;
    }

    public string getMessage()
    {
        return loadLostProgressMessageStart + saveBlueprint.getName() + loadLostProgressMessageEnd;
    }

    public void execute()
    {
        midLoad = true;
        beforeSecondStageLoad = true;

        LoadingBarProgressTracker.loadSaveFile = this;

        if(showMonologueFirst)
        {
            SceneChange.changeSceneToOpeningMonologue();
        } else
        {
            SceneChange.changeSceneToOverworldWithLoadingScreen();
        }
    }

    public void resetData()
    {
        OnLoadResetData.Invoke();
    }

    public void readFromSaveBlueprint()
    {
        OnLoadReadBlueprint.Invoke(saveBlueprint);
    }

    public void performOutro()
    {
        TestScript.addTestVariables();

        FadeToBlackManager.setToMaxOpacity();

        PlayerOOCStateManager.setCurrentActivity(exitActivity);

        SceneChange.removeLoadingScreen();

        midLoad = false;
    }

    public string getPlayerSpriteNameInSave()
    {
        if(saveBlueprint == null)
        {
            return State.playerSpriteName;
        }

        return saveBlueprint.playerSpriteName;
    }

    public void backOut()
    {

    }
}
