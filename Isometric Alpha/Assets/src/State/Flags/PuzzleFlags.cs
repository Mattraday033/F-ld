using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PuzzleFlags
{
    //used for when puzzles need to keep track of an index
    public static int currentPuzzleIndex;

    [RuntimeInitializeOnLoadMethod]
    private static void instantiatePuzzleFlags()
    {
        currentPuzzleIndex = 0;

        TransitionManager.BeforeTransition.AddListener(resetPuzzleFlags);
        LoadSaveFile.OnLoadReadBlueprint.AddListener(readSaveBlueprint);
    }

    private static void resetPuzzleFlags()
    {
        currentPuzzleIndex = 0;
    }

    private static void readSaveBlueprint(SaveBlueprint blueprint)
    {
        currentPuzzleIndex = blueprint.currentPuzzleIndex;
    }
}
