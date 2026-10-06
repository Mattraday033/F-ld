using System.Diagnostics;
using System.Text;
using UnityEngine;

//Editor and development builds only. Every call to it is compiled out of a release build.
//Times one opening of a screen or map, counts the work done for it, and writes one line to the console two frames later
public static class ScreenOpenProbe
{
    private const string editor = "UNITY_EDITOR";
    private const string developmentBuild = "DEVELOPMENT_BUILD";

    //the frame the screen was opened in and the one after it, which is where the work a screen puts off until the end of the frame lands
    private const int framesMeasured = 2;

    private const float notTimed = -1f;

    private static bool measuring;
    private static string label;
    private static int startFrame;

    private static readonly Stopwatch stepStopwatch = new Stopwatch();
    private static readonly Stopwatch canvasStopwatch = new Stopwatch();
    private static readonly StringBuilder steps = new StringBuilder();

    //a canvas pass is the rebuild Unity does before drawing, and every Canvas.ForceUpdateCanvases on top of it
    private static readonly double[] canvasMilliseconds = new double[framesMeasured];
    private static readonly int[] canvasPasses = new int[framesMeasured];
    private static readonly float[] frameMilliseconds = new float[framesMeasured];

    private static int gridFills;
    private static int gridRowsMade;
    private static int descriptionBuilds;
    private static int descriptionRowsMade;
    private static int forcedCanvasRebuilds;
    private static int offOnToggles;
    private static int sliderChecks;
    private static int spriteLoads;

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        measuring = false;
    }

    [Conditional(editor), Conditional(developmentBuild)]
    public static void begin(string label)
    {
        //opened again before the last one was written out, so that one is written with what it has
        if (measuring)
        {
            report();
        }

        ScreenOpenProbe.label = label;

        measuring = true;
        startFrame = Time.frameCount;

        steps.Clear();

        for (int frame = 0; frame < framesMeasured; frame++)
        {
            canvasMilliseconds[frame] = 0;
            canvasPasses[frame] = 0;
            frameMilliseconds[frame] = notTimed;
        }

        gridFills = 0;
        gridRowsMade = 0;
        descriptionBuilds = 0;
        descriptionRowsMade = 0;
        forcedCanvasRebuilds = 0;
        offOnToggles = 0;
        sliderChecks = 0;
        spriteLoads = 0;

        //added again each time so that it is the last to hear a pass finish, after the UI's own rebuild has run
        Canvas.preWillRenderCanvases -= onCanvasPassStarted;
        Canvas.preWillRenderCanvases += onCanvasPassStarted;

        Canvas.willRenderCanvases -= onCanvasPassFinished;
        Canvas.willRenderCanvases += onCanvasPassFinished;

        stepStopwatch.Restart();
    }

    //how long it took to get here from begin, or from the step before
    [Conditional(editor), Conditional(developmentBuild)]
    public static void step(string name)
    {
        if (!measuring)
        {
            return;
        }

        if (steps.Length > 0)
        {
            steps.Append(", ");
        }

        steps.Append(name).Append(" ").Append(inMilliseconds(stepStopwatch.Elapsed.TotalMilliseconds));

        stepStopwatch.Restart();
    }

    //for something worth knowing about this opening that isn't a time or a count
    [Conditional(editor), Conditional(developmentBuild)]
    public static void note(string text)
    {
        if (measuring)
        {
            label += " (" + text + ")";
        }
    }

    [Conditional(editor), Conditional(developmentBuild)]
    public static void countGridFill(int rowsMade)
    {
        if (measuring)
        {
            gridFills++;
            gridRowsMade += rowsMade;
        }
    }

    [Conditional(editor), Conditional(developmentBuild)]
    public static void countDescriptionBuild(int rowsMade)
    {
        if (measuring)
        {
            descriptionBuilds++;
            descriptionRowsMade += rowsMade;
        }
    }

    [Conditional(editor), Conditional(developmentBuild)]
    public static void countForcedCanvasRebuild()
    {
        if (measuring)
        {
            forcedCanvasRebuilds++;
        }
    }

    [Conditional(editor), Conditional(developmentBuild)]
    public static void countOffOnToggle()
    {
        if (measuring)
        {
            offOnToggles++;
        }
    }

    [Conditional(editor), Conditional(developmentBuild)]
    public static void countSliderCheck()
    {
        if (measuring)
        {
            sliderChecks++;
        }
    }

    [Conditional(editor), Conditional(developmentBuild)]
    public static void countSpriteLoad()
    {
        if (measuring)
        {
            spriteLoads++;
        }
    }

    private static void onCanvasPassStarted()
    {
        if (measuring)
        {
            canvasStopwatch.Restart();
        }
    }

    private static void onCanvasPassFinished()
    {
        if (!measuring)
        {
            return;
        }

        int frame = Time.frameCount - startFrame;

        //the first pass of a frame is the earliest this hears that the frame before it has ended
        if (frame >= 1 && frame <= framesMeasured && frameMilliseconds[frame - 1] == notTimed)
        {
            frameMilliseconds[frame - 1] = Time.unscaledDeltaTime * 1000f;
        }

        if (frame >= framesMeasured)
        {
            report();

            return;
        }

        canvasMilliseconds[frame] += canvasStopwatch.Elapsed.TotalMilliseconds;
        canvasPasses[frame]++;
    }

    private static void report()
    {
        measuring = false;

        StringBuilder line = new StringBuilder();

        line.Append("[ScreenOpen] ").Append(label).Append(": ").Append(steps);

        line.Append(" | canvas ");

        for (int frame = 0; frame < framesMeasured; frame++)
        {
            if (frame > 0)
            {
                line.Append(", then ");
            }

            line.Append(inMilliseconds(canvasMilliseconds[frame])).Append(" in ").Append(canvasPasses[frame]).Append(" passes");
        }

        line.Append(" | frame ");

        for (int frame = 0; frame < framesMeasured; frame++)
        {
            if (frame > 0)
            {
                line.Append(", then ");
            }

            line.Append(frameMilliseconds[frame] == notTimed ? "not timed" : inMilliseconds(frameMilliseconds[frame]));
        }

        line.Append(" | grid fills ").Append(gridFills).Append(" (").Append(gridRowsMade).Append(" rows made)");
        line.Append(", description builds ").Append(descriptionBuilds).Append(" (").Append(descriptionRowsMade).Append(" rows made)");
        line.Append(", forced canvas rebuilds ").Append(forcedCanvasRebuilds);
        line.Append(", off/on toggles ").Append(offOnToggles);
        line.Append(", slider checks ").Append(sliderChecks);
        line.Append(", sprite loads ").Append(spriteLoads);

        UnityEngine.Debug.Log(line.ToString());
    }

    private static string inMilliseconds(double milliseconds)
    {
        return milliseconds.ToString("0.0") + " ms";
    }
}
