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

    public CurrentActivity exitActivity;
    public SaveBlueprint saveBlueprint;
    public bool showMonologueFirst;

    public LoadSaveFile(SaveBlueprint saveBlueprint, CurrentActivity exitActivity = CurrentActivity.Walking, bool showMonologueFirst = false)
    {
        this.saveBlueprint = saveBlueprint;

        this.exitActivity = exitActivity;
        this.showMonologueFirst = showMonologueFirst;
    }

    public string getMessage()
    {
        return loadLostProgressMessageStart + saveBlueprint.displayName + loadLostProgressMessageEnd;
    }

    //true while the screen fades out ahead of a load, so a second press doesn't start the load twice
    private static bool fadingOutToLoad = false;

    //statics survive between play sessions with domain reload disabled
    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        fadingOutToLoad = false;
        SceneManager.sceneLoaded -= clearFadeUnderLoadingScreen;
    }

    //The scene change blocks for a few frames, and the last frame drawn before it would otherwise be the UI half torn down,
    //so the screen goes black first. A screen that's already black, as after character creation, goes straight on
    public void execute()
    {
        if(fadingOutToLoad)
        {
            return;
        }

        bool fadingOut = !FadeToBlackManager.isBlack() && FadeToBlackManager.getInstance() != null;

        fadingOutToLoad = true;

        FadeToBlackManager.quickFadeToBlackThen(() =>
        {
            fadingOutToLoad = false;

            //the black was only there to cover the freeze, so it's cleared once the loading screen is up to take over
            if(fadingOut && !showMonologueFirst)
            {
                SceneManager.sceneLoaded -= clearFadeUnderLoadingScreen;
                SceneManager.sceneLoaded += clearFadeUnderLoadingScreen;
            }

            changeScene();
        });
    }

    private static void clearFadeUnderLoadingScreen(Scene scene, LoadSceneMode mode)
    {
        if(!scene.name.Equals(SceneNameList.loadingScreen))
        {
            return;
        }

        SceneManager.sceneLoaded -= clearFadeUnderLoadingScreen;

        FadeToBlackManager.setToClear();
    }

    private void changeScene()
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

        if(PlayerStateManager.currentActivity != CurrentActivity.InTutorialSequence)
        {
            PlayerStateManager.setCurrentActivity(exitActivity);
        }

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
