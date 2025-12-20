using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{

    public const int sizeZero = 0;
    public const int sizeOne = 1;
    public const int sizeTwo = 2;
    public const int sizeThree = 3;
    public const int sizeFour = 4;
    public const int sizeFive = 5;
    public const int sizeSix = 6;
    public const int sizeSeven = 7;

    public const int indexZero = 0;
    public const int indexOne = 1;
    public const int indexTwo = 2;
    public const int indexThree = 3;
    public const int indexFour = 4;
    public const int indexFive = 5;
    public const int indexSix = 6;
    public const int indexSeven = 7;
    public const int indexEight = 8;
    public const int indexNine = 9;
    public const int indexTen = 10;
    public const int indexEleven = 11;
    public const int indexTwelve = 12;

    public const int difficultyTwo = 2;
    public const int difficultyThree = 3;
    public const int difficultyFour = 4;
    public const int difficultyFive = 5;
    public const int difficultySix = 6;
    public const int difficultySeven = 7;
    public const int difficultyEight = 8;
    public const int difficultyNine = 9;
    public const int difficultyTen = 10;

	public const float detectionSize = .075f;

    public const int statLevelOne = 1;
    public const int statLevelTwo = 2;
    public const int statLevelThree = 3;
    public const int statLevelFour = 4;
    public const int statLevelFive = 5;
    public const int statLevelSix = 6;
    public const int statLevelSeven = 7;
    public const int statLevelEight = 8;
    public const int statLevelNine = 9;
    public const int statLevelTen = 10;

    public const float onTableHeightOffset = 0.05f;

    public const string STRDesignator = "STR";
    public const string DEXDesignator = "DEX";
    public const string WISDesignator = "WIS";
    public const string CHADesignator = "CHA";

    public const string seperatorChar = "/";

    public const string zeroRating = "0";

    public const string emptyString = "";

    public const bool reveal = true;
    public const bool removeReveal = false;

    public readonly static Vector3 flippedXScale = new Vector3(-1f, 1f, 1f);

    public const string strDiffVarName = "strDifficulty";
    public const string dexDiffVarName = "dexDifficulty";
    public const string wisDiffVarName = "wisDifficulty";
    public const string chaDiffVarName = "chaDifficulty";

}

public static class StatDifficultyList
{
    public readonly static KeyValuePair<string, int> strengthDifficultyTwo = new KeyValuePair<string, int>(Constants.strDiffVarName, Constants.difficultyTwo);
    public readonly static KeyValuePair<string, int> strengthDifficultyThree = new KeyValuePair<string, int>(Constants.strDiffVarName, Constants.difficultyThree);
    public readonly static KeyValuePair<string, int> strengthDifficultyFour = new KeyValuePair<string, int>(Constants.strDiffVarName, Constants.difficultyFour);
    public readonly static KeyValuePair<string, int> strengthDifficultyFive = new KeyValuePair<string, int>(Constants.strDiffVarName, Constants.difficultyFive);
    public readonly static KeyValuePair<string, int> strengthDifficultySix = new KeyValuePair<string, int>(Constants.strDiffVarName, Constants.difficultySix);
    public readonly static KeyValuePair<string, int> strengthDifficultySeven = new KeyValuePair<string, int>(Constants.strDiffVarName, Constants.difficultySeven);
    public readonly static KeyValuePair<string, int> strengthDifficultyEight = new KeyValuePair<string, int>(Constants.strDiffVarName, Constants.difficultyEight);
    public readonly static KeyValuePair<string, int> strengthDifficultyNine = new KeyValuePair<string, int>(Constants.strDiffVarName, Constants.difficultyNine);
    public readonly static KeyValuePair<string, int> strengthDifficultyTen = new KeyValuePair<string, int>(Constants.strDiffVarName, Constants.difficultyTen);

    public readonly static KeyValuePair<string, int> dexterityDifficultyTwo = new KeyValuePair<string, int>(Constants.dexDiffVarName, Constants.difficultyTwo);
    public readonly static KeyValuePair<string, int> dexterityDifficultyThree = new KeyValuePair<string, int>(Constants.dexDiffVarName, Constants.difficultyThree);
    public readonly static KeyValuePair<string, int> dexterityDifficultyFour = new KeyValuePair<string, int>(Constants.dexDiffVarName, Constants.difficultyFour);
    public readonly static KeyValuePair<string, int> dexterityDifficultyFive = new KeyValuePair<string, int>(Constants.dexDiffVarName, Constants.difficultyFive);
    public readonly static KeyValuePair<string, int> dexterityDifficultySix = new KeyValuePair<string, int>(Constants.dexDiffVarName, Constants.difficultySix);
    public readonly static KeyValuePair<string, int> dexterityDifficultySeven = new KeyValuePair<string, int>(Constants.dexDiffVarName, Constants.difficultySeven);
    public readonly static KeyValuePair<string, int> dexterityDifficultyEight = new KeyValuePair<string, int>(Constants.dexDiffVarName, Constants.difficultyEight);
    public readonly static KeyValuePair<string, int> dexterityDifficultyNine = new KeyValuePair<string, int>(Constants.dexDiffVarName, Constants.difficultyNine);
    public readonly static KeyValuePair<string, int> dexterityDifficultyTen = new KeyValuePair<string, int>(Constants.dexDiffVarName, Constants.difficultyTen);

    public readonly static KeyValuePair<string, int> wisdomDifficultyTwo = new KeyValuePair<string, int>(Constants.wisDiffVarName, Constants.difficultyTwo);
    public readonly static KeyValuePair<string, int> wisdomDifficultyThree = new KeyValuePair<string, int>(Constants.wisDiffVarName, Constants.difficultyThree);
    public readonly static KeyValuePair<string, int> wisdomDifficultyFour = new KeyValuePair<string, int>(Constants.wisDiffVarName, Constants.difficultyFour);
    public readonly static KeyValuePair<string, int> wisdomDifficultyFive = new KeyValuePair<string, int>(Constants.wisDiffVarName, Constants.difficultyFive);
    public readonly static KeyValuePair<string, int> wisdomDifficultySix = new KeyValuePair<string, int>(Constants.wisDiffVarName, Constants.difficultySix);
    public readonly static KeyValuePair<string, int> wisdomDifficultySeven = new KeyValuePair<string, int>(Constants.wisDiffVarName, Constants.difficultySeven);
    public readonly static KeyValuePair<string, int> wisdomDifficultyEight = new KeyValuePair<string, int>(Constants.wisDiffVarName, Constants.difficultyEight);
    public readonly static KeyValuePair<string, int> wisdomDifficultyNine = new KeyValuePair<string, int>(Constants.wisDiffVarName, Constants.difficultyNine);
    public readonly static KeyValuePair<string, int> wisdomDifficultyTen = new KeyValuePair<string, int>(Constants.wisDiffVarName, Constants.difficultyTen);

    public readonly static KeyValuePair<string, int> charismaDifficultyTwo = new KeyValuePair<string, int>(Constants.chaDiffVarName, Constants.difficultyTwo);
    public readonly static KeyValuePair<string, int> charismaDifficultyThree = new KeyValuePair<string, int>(Constants.chaDiffVarName, Constants.difficultyThree);
    public readonly static KeyValuePair<string, int> charismaDifficultyFour = new KeyValuePair<string, int>(Constants.chaDiffVarName, Constants.difficultyFour);
    public readonly static KeyValuePair<string, int> charismaDifficultyFive = new KeyValuePair<string, int>(Constants.chaDiffVarName, Constants.difficultyFive);
    public readonly static KeyValuePair<string, int> charismaDifficultySix = new KeyValuePair<string, int>(Constants.chaDiffVarName, Constants.difficultySix);
    public readonly static KeyValuePair<string, int> charismaDifficultySeven = new KeyValuePair<string, int>(Constants.chaDiffVarName, Constants.difficultySeven);
    public readonly static KeyValuePair<string, int> charismaDifficultyEight = new KeyValuePair<string, int>(Constants.chaDiffVarName, Constants.difficultyEight);
    public readonly static KeyValuePair<string, int> charismaDifficultyNine = new KeyValuePair<string, int>(Constants.chaDiffVarName, Constants.difficultyNine);
    public readonly static KeyValuePair<string, int> charismaDifficultyTen = new KeyValuePair<string, int>(Constants.chaDiffVarName, Constants.difficultyTen);

}