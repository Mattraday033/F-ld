using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public readonly static Vector3 scaleChange = new Vector3(0.987758756f,1.15740716f);
    public readonly static Vector3 reverseScaleChange = new Vector3(1.012393f,0.864000201f);

    public const int tierZero = 0;
    public const int tierOne = 1;
    public const int tierTwo = 2;
    public const int tierThree = 3;
    public const int tierFour = 4;

    public const int sizeZero = 0;
    public const int sizeOne = 1;
    public const int sizeTwo = 2;
    public const int sizeThree = 3;
    public const int sizeFour = 4;
    public const int sizeFive = 5;
    public const int sizeSix = 6;
    public const int sizeSeven = 7;
    public const int sizeEight = 8;
    public const int sizeNine = 9;
    public const int sizeTen = 10;

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

	public const float detectionSize = .05f;

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

	public const int endOfRoundDuration = 1; //Trait duration will always tick down once before the player/enemy gets to exploit
	public const int oneRoundDuration = 2; //so a 1 round duration is really just for the rest of that turn's resolution
	public const int twoRoundDuration = 3;
	public const int threeRoundDuration = 4;
	public const int fourRoundDuration = 5;

    public const int zeroStacksAtStart = 0;
	public const int oneStackAtStart = 1;
	public const int twoStacksAtStart = 2;
	public const int threeStacksAtStart = 3;
	public const int fourStacksAtStart = 4;

	public const int oneStackPerApplication = 1;

    public const float onTableHeightOffset = 0.05f;
    public const float onGroundHeightOffset = 0.3f;

    public const string STRDesignator = "STR";
    public const string DEXDesignator = "DEX";
    public const string WISDesignator = "WIS";
    public const string CHADesignator = "CHA";

    public const string resist = "Resist";

    public const string seperatorChar = "/";

    public const double autoSuccess = 1.0;
    public const string zeroRating = "0";
    public const string zeroBonus = "+0";

    public const string emptyString = "";

    public const bool skipAutosave = true;

    public const bool reveal = true;
    public const bool removeReveal = false;

    public const bool flipX = true;
    public const bool flipY = true;

    public readonly static Vector3 flippedXScale = new Vector3(-1f, 1f, 1f);

    public const int resistanceBonusPerTier = 3;

    public const float minFontSize = 12f;
	public const float maxOpacity = 255f;

    public const string femaleMarker = " (F)";
    public const string maleMarker = " (M)";

    #region Creature Amounts
    public const int oneCreature = 1;
    public const int twoCreatures = 2;
    public const int threeCreatures = 3;
    public const int fourCreatures = 4;
    public const int fiveCreatures = 5;
    public const int sixCreatures = 6;
    public const int sevenCreatures = 7;
    public const int eightCreatures = 8;
    public const int nineCreatures = 9;
    public const int tenCreatures = 10;
    public const int elevenCreatures = 11;
    public const int twelveCreatures = 12;
    public const int tooManyCreatures = 20;
    #endregion

    public const int perfectVolleyAccuracy = 100;

    #region Creature Armor Values

    public const int zeroArmor = 0;
    public const int fiveArmor = 5;
    public const int tenArmor = 10;
    public const int fifteenArmor = 15;
    public const int twentyArmor = 20;
    public const int twentyFiveArmor = 25;
    public const int thirtyArmor = 30;
    public const int thirtyFiveArmor = 35;
    public const int fortyArmor = 40;
    public const int fortyFiveArmor = 45;
    public const int fiftyArmor = 50;
    public const int fiftyFiveArmor = 55;
    public const int sixtyArmor = 60;
    public const int sixtyFiveArmor = 65;
    public const int seventyArmor = 70;
    public const int seventyFiveArmor = 75;
    public const int eightyArmor = 80;
    public const int eightyFiveArmor = 85;
    public const int ninetyArmor = 90;

    #endregion
}

public static class StatDifficultyList
{
    public readonly static KeyValuePair<string, int> strengthDifficultyTwo = new KeyValuePair<string, int>(InkVariableNameList.strDiffVarName, Constants.difficultyTwo);
    public readonly static KeyValuePair<string, int> strengthDifficultyThree = new KeyValuePair<string, int>(InkVariableNameList.strDiffVarName, Constants.difficultyThree);
    public readonly static KeyValuePair<string, int> strengthDifficultyFour = new KeyValuePair<string, int>(InkVariableNameList.strDiffVarName, Constants.difficultyFour);
    public readonly static KeyValuePair<string, int> strengthDifficultyFive = new KeyValuePair<string, int>(InkVariableNameList.strDiffVarName, Constants.difficultyFive);
    public readonly static KeyValuePair<string, int> strengthDifficultySix = new KeyValuePair<string, int>(InkVariableNameList.strDiffVarName, Constants.difficultySix);
    public readonly static KeyValuePair<string, int> strengthDifficultySeven = new KeyValuePair<string, int>(InkVariableNameList.strDiffVarName, Constants.difficultySeven);
    public readonly static KeyValuePair<string, int> strengthDifficultyEight = new KeyValuePair<string, int>(InkVariableNameList.strDiffVarName, Constants.difficultyEight);
    public readonly static KeyValuePair<string, int> strengthDifficultyNine = new KeyValuePair<string, int>(InkVariableNameList.strDiffVarName, Constants.difficultyNine);
    public readonly static KeyValuePair<string, int> strengthDifficultyTen = new KeyValuePair<string, int>(InkVariableNameList.strDiffVarName, Constants.difficultyTen);

    public readonly static KeyValuePair<string, int> dexterityDifficultyTwo = new KeyValuePair<string, int>(InkVariableNameList.dexDiffVarName, Constants.difficultyTwo);
    public readonly static KeyValuePair<string, int> dexterityDifficultyThree = new KeyValuePair<string, int>(InkVariableNameList.dexDiffVarName, Constants.difficultyThree);
    public readonly static KeyValuePair<string, int> dexterityDifficultyFour = new KeyValuePair<string, int>(InkVariableNameList.dexDiffVarName, Constants.difficultyFour);
    public readonly static KeyValuePair<string, int> dexterityDifficultyFive = new KeyValuePair<string, int>(InkVariableNameList.dexDiffVarName, Constants.difficultyFive);
    public readonly static KeyValuePair<string, int> dexterityDifficultySix = new KeyValuePair<string, int>(InkVariableNameList.dexDiffVarName, Constants.difficultySix);
    public readonly static KeyValuePair<string, int> dexterityDifficultySeven = new KeyValuePair<string, int>(InkVariableNameList.dexDiffVarName, Constants.difficultySeven);
    public readonly static KeyValuePair<string, int> dexterityDifficultyEight = new KeyValuePair<string, int>(InkVariableNameList.dexDiffVarName, Constants.difficultyEight);
    public readonly static KeyValuePair<string, int> dexterityDifficultyNine = new KeyValuePair<string, int>(InkVariableNameList.dexDiffVarName, Constants.difficultyNine);
    public readonly static KeyValuePair<string, int> dexterityDifficultyTen = new KeyValuePair<string, int>(InkVariableNameList.dexDiffVarName, Constants.difficultyTen);

    public readonly static KeyValuePair<string, int> wisdomDifficultyTwo = new KeyValuePair<string, int>(InkVariableNameList.wisDiffVarName, Constants.difficultyTwo);
    public readonly static KeyValuePair<string, int> wisdomDifficultyThree = new KeyValuePair<string, int>(InkVariableNameList.wisDiffVarName, Constants.difficultyThree);
    public readonly static KeyValuePair<string, int> wisdomDifficultyFour = new KeyValuePair<string, int>(InkVariableNameList.wisDiffVarName, Constants.difficultyFour);
    public readonly static KeyValuePair<string, int> wisdomDifficultyFive = new KeyValuePair<string, int>(InkVariableNameList.wisDiffVarName, Constants.difficultyFive);
    public readonly static KeyValuePair<string, int> wisdomDifficultySix = new KeyValuePair<string, int>(InkVariableNameList.wisDiffVarName, Constants.difficultySix);
    public readonly static KeyValuePair<string, int> wisdomDifficultySeven = new KeyValuePair<string, int>(InkVariableNameList.wisDiffVarName, Constants.difficultySeven);
    public readonly static KeyValuePair<string, int> wisdomDifficultyEight = new KeyValuePair<string, int>(InkVariableNameList.wisDiffVarName, Constants.difficultyEight);
    public readonly static KeyValuePair<string, int> wisdomDifficultyNine = new KeyValuePair<string, int>(InkVariableNameList.wisDiffVarName, Constants.difficultyNine);
    public readonly static KeyValuePair<string, int> wisdomDifficultyTen = new KeyValuePair<string, int>(InkVariableNameList.wisDiffVarName, Constants.difficultyTen);

    public readonly static KeyValuePair<string, int> charismaDifficultyTwo = new KeyValuePair<string, int>(InkVariableNameList.chaDiffVarName, Constants.difficultyTwo);
    public readonly static KeyValuePair<string, int> charismaDifficultyThree = new KeyValuePair<string, int>(InkVariableNameList.chaDiffVarName, Constants.difficultyThree);
    public readonly static KeyValuePair<string, int> charismaDifficultyFour = new KeyValuePair<string, int>(InkVariableNameList.chaDiffVarName, Constants.difficultyFour);
    public readonly static KeyValuePair<string, int> charismaDifficultyFive = new KeyValuePair<string, int>(InkVariableNameList.chaDiffVarName, Constants.difficultyFive);
    public readonly static KeyValuePair<string, int> charismaDifficultySix = new KeyValuePair<string, int>(InkVariableNameList.chaDiffVarName, Constants.difficultySix);
    public readonly static KeyValuePair<string, int> charismaDifficultySeven = new KeyValuePair<string, int>(InkVariableNameList.chaDiffVarName, Constants.difficultySeven);
    public readonly static KeyValuePair<string, int> charismaDifficultyEight = new KeyValuePair<string, int>(InkVariableNameList.chaDiffVarName, Constants.difficultyEight);
    public readonly static KeyValuePair<string, int> charismaDifficultyNine = new KeyValuePair<string, int>(InkVariableNameList.chaDiffVarName, Constants.difficultyNine);
    public readonly static KeyValuePair<string, int> charismaDifficultyTen = new KeyValuePair<string, int>(InkVariableNameList.chaDiffVarName, Constants.difficultyTen);

}