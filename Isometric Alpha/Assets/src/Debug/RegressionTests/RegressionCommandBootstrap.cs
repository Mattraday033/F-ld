using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Starts a headless regression test, but only when the game runs headless (-batchmode) and was given
/// -regressionCommands &lt;path to .commands file&gt;. Runs before the first scene loads, so the log file
/// exists before any scene's events are written.
/// </summary>
public static class RegressionCommandBootstrap
{
    public const string commandsArgument = "-regressionCommands";

    private const string buildTimestampFileName = "Build-Timestamp.txt";
    private const string runnerObjectName = "Regression Command Runner";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void start()
    {
        if(!HeadlessLogger.IsHeadless)
        {
            return;
        }

        string commandsPath = getArgumentValue(commandsArgument);

        if(String.IsNullOrWhiteSpace(commandsPath))
        {
            return;
        }

        HeadlessLogger.CreateLogFile(Path.GetFileNameWithoutExtension(commandsPath));

        //The date key lets the evaluator work out the run time even across midnight, since each line's own
        //stamp is the time of day only.
        HeadlessLogger.LogEvent("Script.Started",
                                "date", DateTime.Now.ToString("yyyy-MM-dd"),
                                "file", commandsPath,
                                "build", readBuildTimestamp());

        GameObject host = new GameObject(runnerObjectName);
        UnityEngine.Object.DontDestroyOnLoad(host);

        RegressionCommandRunner runner = host.AddComponent<RegressionCommandRunner>();
        runner.begin(commandsPath);
    }

    public static string getArgumentValue(string argumentName)
    {
        string[] arguments = Environment.GetCommandLineArgs();

        for(int index = 0; index < arguments.Length - 1; index++)
        {
            if(String.Equals(arguments[index], argumentName, StringComparison.OrdinalIgnoreCase))
            {
                return arguments[index + 1];
            }
        }

        return null;
    }

    private static string readBuildTimestamp()
    {
        try
        {
            string path = Path.Combine(Application.streamingAssetsPath, buildTimestampFileName);

            if(!File.Exists(path))
            {
                return "unknown";
            }

            return File.ReadAllText(path).Trim();
        }
        catch(Exception e) when(e is IOException || e is UnauthorizedAccessException)
        {
            return "unreadable";
        }
    }
}
