using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class EnvironmentGrid
{
    [UnityEditor.InitializeOnLoadMethod]
    private static void CreateEnvironmentGridListener()
    {
        PrefabStage.prefabStageOpened -= HandlePrefabStageOpened;
        PrefabStage.prefabStageOpened += HandlePrefabStageOpened;
    }

    private static void HandlePrefabStageOpened(PrefabStage prefabStage)
    {
        if (prefabStage == null)
        {
            return;
        }

        foreach (GameObject rootObject in prefabStage.scene.GetRootGameObjects())
        {
            if(rootObject.name.Contains("Environment"))
            {
                Grid grid = rootObject.GetComponent<Grid>();

                if(grid == null)
                {
                    continue;
                }
                
                grid.transform.localScale = new Vector3(1.012393f, 0.864012301f, 1f);

                grid.cellSize = new Vector3(1f, .5f, 1f);
                grid.cellLayout = GridLayout.CellLayout.IsometricZAsY;
            }
        }
    }
}
