using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EditorConfig
{

    [UnityEditor.InitializeOnEnterPlayMode]
    private static void addCreateConfigListener()
    {
        EditorApplication.playModeStateChanged += writeConfigAfterEditorPlayMoveExit;
    }

    private static void writeConfigAfterEditorPlayMoveExit(PlayModeStateChange state)
    {
        if(state != PlayModeStateChange.ExitingPlayMode)
        {
            return;
        }

        Config.writeConfig();
        EditorApplication.playModeStateChanged -= writeConfigAfterEditorPlayMoveExit;
    }

}
