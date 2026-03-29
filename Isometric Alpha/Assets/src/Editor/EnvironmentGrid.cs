using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class EnvironmentGrid
{
    private const string EnvironmentObjectName = "Environment Grid";

    [UnityEditor.InitializeOnLoadMethod]
    private static void CreateEnvironmentGridListener()
    {
        PrefabStage.prefabStageOpened -= HandlePrefabStageOpened;
        PrefabStage.prefabStageOpened += HandlePrefabStageOpened;

        CreateEnvironmentObjectIfNeeded(PrefabStageUtility.GetCurrentPrefabStage());
    }

    private static void HandlePrefabStageOpened(PrefabStage prefabStage)
    {
        CreateEnvironmentObjectIfNeeded(prefabStage);
    }

    private static void CreateEnvironmentObjectIfNeeded(PrefabStage prefabStage)
    {
        if (prefabStage == null)
        {
            return;
        }

        foreach (GameObject rootObject in prefabStage.scene.GetRootGameObjects())
        {
            if (rootObject.name == EnvironmentObjectName)
            {
                return;
            }
        }

        GameObject environmentObject = new GameObject(EnvironmentObjectName)
        {
            hideFlags = HideFlags.DontSaveInEditor
        };

        SceneManager.MoveGameObjectToScene(environmentObject, prefabStage.scene);
    }
}
