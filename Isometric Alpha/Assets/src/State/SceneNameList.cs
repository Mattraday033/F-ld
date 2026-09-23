using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneNameList
{

    public const string overworld = "Overworld";

    public const string combat = "Combat";
    public const string combatUI = "Combat UI";

    public const string OOCUserInterface = "OOC UI";

    public const string loadingScreen = "Loading Screen";

    public const string startMenu = "StartMenu";

    public const string endOfDemo = "EndOfDemo";

    public const string openingMonologue = "OpeningMonologue";

}

public static class SceneChange
{

    public static void changeSceneToCombat()
    {
        CombatStateManager.setReturnCell(MovementManager.getPlayerCell());

        SceneManager.LoadScene(SceneNameList.combat);
        SceneManager.LoadScene(SceneNameList.combatUI, LoadSceneMode.Additive);
    }

    public static void changeSceneToCombat(MonoBehaviour coroutineTarget)
    {

        FadeToBlackManager.startCombatTransition(coroutineTarget.transform);

        coroutineTarget.StartCoroutine(waitForFadeFinish());
    }

    private static IEnumerator waitForFadeFinish()
    {
        while(PlayerOOCStateManager.currentActivity != OOCActivity.preCombat)
        {
            yield return null;
        }

        PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
        changeSceneToCombat();
    }

    public static void changeSceneToEndOfDemo()
    {
        SceneManager.LoadScene(SceneNameList.endOfDemo);
    }

    public static void changeSceneToOpeningMonologue()
    {
        SceneManager.LoadScene(SceneNameList.openingMonologue);
    }

    public static void changeSceneToOverworld()
    {
        SceneManager.LoadScene(SceneNameList.overworld);

        addOOCUIScene();
    }

    public static void changeSceneToOverworldWithLoadingScreen()
    {
        changeSceneToOverworld();

        SceneManager.LoadScene(SceneNameList.loadingScreen, LoadSceneMode.Additive);
    }

    public static void removeLoadingScreen()
    {
        SceneManager.UnloadSceneAsync(SceneNameList.loadingScreen);
    }

    public static void changeSceneToStartMenu()
    {
        //Every route back to the start menu comes through here, so this is where the game re-enters the state
        //the newGame flag used to stand for. The bypass is needed because setCurrentActivity otherwise refuses
        //to leave inTutorialSequence for anything but walking.
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.MainMenu, tutorialSequenceCheckBypass: true);

        SceneManager.LoadScene(SceneNameList.startMenu);
    }

    public static void addOOCUIScene()
    {
        if(PlayerOOCStateManager.currentActivity == OOCActivity.Defeat)
        {
            return;
        }

        SceneManager.LoadScene(SceneNameList.OOCUserInterface, LoadSceneMode.Additive);
    }

    [RuntimeInitializeOnLoadMethod]
    private static void setSceneToStartMenu()
    {
        if(Application.isEditor && SceneManager.GetActiveScene().name.Equals(SceneNameList.startMenu))
        {
            return;
        }

        changeSceneToStartMenu();
    }

}