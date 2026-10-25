using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Logs game-wide events that have no single owning class (scene loads, the app quitting, a zone transition
/// finishing) to the HeadlessLogger. Only subscribes when the game runs headless, so a normal run pays nothing.
/// Nothing here knows about tests: the same lines appear in any headless run that has a log file.
/// </summary>
public static class HeadlessGameEvents
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void subscribe()
    {
        if(!HeadlessLogger.IsHeadless)
        {
            return;
        }

        SceneManager.sceneLoaded -= onSceneLoaded;
        SceneManager.sceneLoaded += onSceneLoaded;

        SceneManager.sceneUnloaded -= onSceneUnloaded;
        SceneManager.sceneUnloaded += onSceneUnloaded;

        Application.quitting -= onQuitting;
        Application.quitting += onQuitting;

        TransitionManager.AfterTransition.RemoveListener(onAfterTransition);
        TransitionManager.AfterTransition.AddListener(onAfterTransition);
    }

    private static void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HeadlessLogger.LogEvent("Scene.Loaded", "scene", scene.name, "mode", mode);
    }

    private static void onSceneUnloaded(Scene scene)
    {
        HeadlessLogger.LogEvent("Scene.Unloaded", "scene", scene.name);
    }

    private static void onQuitting()
    {
        HeadlessLogger.LogEvent("App.Quitting");
    }

    //AfterTransition fires once the new area is built and the player placed, before the fade back in
    private static void onAfterTransition()
    {
        HeadlessLogger.LogEvent("Transition.Finished", "area", AreaManager.locationName);
    }
}
