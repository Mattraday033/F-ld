using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// Orders names so that "Frame_2" sorts before "Frame_10": runs of digits compare as numbers,
/// everything between them compares ordinally.
///
/// Lives in runtime code rather than beside its first caller in Assets/src/Editor because both ends
/// of the sprite pipeline have to reach the same answer - SpriteClipGenerator orders a generated
/// .anim's keyframes with it, ResourceList orders the frames Resources.LoadAll hands back with it.
/// Assembly-CSharp-Editor can see Assembly-CSharp but not the reverse, so a single shared copy can
/// only live here. Two copies would be two things to keep in step, and the failure they would
/// eventually produce - a clip and its runtime sprites disagreeing about frame order - is silent.
/// </summary>
public class NaturalNameComparer : IComparer<string>
{

    // The comparer is stateless, so one shared instance spares every caller an allocation.
    public readonly static NaturalNameComparer instance = new NaturalNameComparer();

    // A capturing split: the digit runs are kept as chunks of their own, so "Run_Back_10" becomes
    // ["Run_Back_", "10", ""] and the numeric chunk is there to be parsed.
    private const string digitRunPattern = "([0-9]+)";

    public int Compare(string left, string right)
    {
        if (left == right)
        {
            return 0;
        }

        string[] leftChunks = Regex.Split(left ?? string.Empty, digitRunPattern);
        string[] rightChunks = Regex.Split(right ?? string.Empty, digitRunPattern);

        int count = Mathf.Min(leftChunks.Length, rightChunks.Length);

        for (int i = Constants.indexZero; i < count; i++)
        {
            if (leftChunks[i] == rightChunks[i])
            {
                continue;
            }

            if (int.TryParse(leftChunks[i], out int leftNumber) &&
                int.TryParse(rightChunks[i], out int rightNumber))
            {
                return leftNumber.CompareTo(rightNumber);
            }

            return string.Compare(leftChunks[i], rightChunks[i], StringComparison.Ordinal);
        }

        return leftChunks.Length.CompareTo(rightChunks.Length);
    }
}
