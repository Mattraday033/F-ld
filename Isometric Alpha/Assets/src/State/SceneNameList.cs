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

}

public static class SceneChange
{

    public static void changeSceneToCombat()
    {
        SceneManager.LoadScene(SceneNameList.combat);
        SceneManager.LoadScene(SceneNameList.combatUI, LoadSceneMode.Additive);
    }

    public static void changeSceneToEndOfDemo()
    {
        SceneManager.LoadScene(SceneNameList.endOfDemo);
    }

    public static void changeSceneToLoadingScreen()
    {
        SceneManager.LoadScene(SceneNameList.loadingScreen);
    }

    public static void changeSceneToOverworld()
    {
        SceneManager.LoadScene(SceneNameList.overworld);

        addOOCUIScene();
    }

    public static void changeSceneToStartMenu()
    {
        SceneManager.LoadScene(SceneNameList.startMenu);
    }

    public static void addOOCUIScene()
    {
        SceneManager.LoadScene(SceneNameList.OOCUserInterface, LoadSceneMode.Additive);
    }

}