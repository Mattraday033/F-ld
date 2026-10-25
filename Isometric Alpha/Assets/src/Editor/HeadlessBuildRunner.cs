#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using Debug = UnityEngine.Debug;

/// <summary>
/// Runs the Windows build headless (-batchmode -nographics) for a set number of seconds as a startup
/// smoke test, then reports any exceptions or crashes found in the player log.
///
/// The build is rebuilt first when it was not made today. Build age comes from the Build-Timestamp
/// file that BuildTimestamp writes into the build, because the exe's own modified date only changes
/// when Unity's player stub changes.
/// </summary>
public static class HeadlessBuildRunner
{
    private const string buildFolder = @"D:\Fold Builds\Windows";
    private const string playerLogFileName = "headless-player.log";
    private const string dataFolderSuffix = "_Data";
    private const string streamingAssetsFolderName = "StreamingAssets";
    private const string globalGameManagersFileName = "globalgamemanagers";

    private const string runSecondsPrefKey = "HeadlessBuildRunner.RunSeconds";
    private const int defaultRunSeconds = 30;
    private const int killWaitMilliseconds = 5000;
    private const int maxReportedLines = 10;

    private const string logPrefix = "[HeadlessBuildRunner] ";
    private const string dialogTitle = "Headless Build";

    private const string menuRoot = "Tools/Build/";
    private const string durationMenuRoot = menuRoot + "Headless Run Duration/";
    private const string duration15Menu = durationMenuRoot + "15s";
    private const string duration30Menu = durationMenuRoot + "30s";
    private const string duration60Menu = durationMenuRoot + "60s";
    private const string duration120Menu = durationMenuRoot + "120s";

    private static readonly string[] problemMarkers = { "Exception", "Crash!!!", "Fatal error" };

    private static Process runningProcess = null;
    private static DateTime runStartTime;
    private static int runSeconds;
    private static string runLogPath;

    #region Regression tests

    //Regression tests run through RegressionTests/run_test.py, which launches the game with -regressionCommands,
    //waits for it to quit (run_test.py has its own safety timeout) and evaluates the queries. There is no kill
    //timer here: the test decides when the game exits.
    private const string regressionMenuRoot = menuRoot + "Regression Tests/";
    private const string regressionTestsFolderName = "RegressionTests";
    private const string regressionRunScriptName = "run_test.py";
    private const string newGameToFirstSaveTest = "NewGameToFirstSave";

    private static Process regressionProcess = null;
    private static string regressionTestName;
    private static DateTime regressionStartTime;
    private static readonly System.Text.StringBuilder regressionOutput = new System.Text.StringBuilder();
    private static readonly object regressionOutputLock = new object();

    [MenuItem(regressionMenuRoot + "Run " + newGameToFirstSaveTest, false, 40)]
    private static void runNewGameToFirstSave()
    {
        runRegressionTest(newGameToFirstSaveTest, false);
    }

    [MenuItem(regressionMenuRoot + "Rebuild And Run " + newGameToFirstSaveTest, false, 41)]
    private static void rebuildAndRunNewGameToFirstSave()
    {
        runRegressionTest(newGameToFirstSaveTest, true);
    }

    private static void runRegressionTest(string testName, bool forceRebuild)
    {
        if(!canStartRun())
        {
            return;
        }

        string exePath = getExePath();

        if(forceRebuild || !isBuildFromToday(exePath))
        {
            if(!buildPlayer(exePath))
            {
                return;
            }
        }

        string projectFolder = Directory.GetCurrentDirectory();
        string scriptPath = Path.Combine(projectFolder, regressionTestsFolderName, regressionRunScriptName);

        if(!File.Exists(scriptPath))
        {
            Debug.LogError(logPrefix + "No regression runner at " + scriptPath);
            return;
        }

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "python",
            Arguments = "\"" + scriptPath + "\" " + testName + " --exe \"" + exePath + "\"",
            WorkingDirectory = projectFolder,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        lock(regressionOutputLock)
        {
            regressionOutput.Clear();
        }

        try
        {
            regressionProcess = new Process { StartInfo = startInfo };
            regressionProcess.OutputDataReceived += onRegressionOutput;
            regressionProcess.ErrorDataReceived += onRegressionOutput;
            regressionProcess.Start();
            regressionProcess.BeginOutputReadLine();
            regressionProcess.BeginErrorReadLine();
        }
        catch(Exception e)
        {
            Debug.LogError(logPrefix + "Could not start python " + scriptPath + " (is python on PATH?): " + e.Message);
            regressionProcess = null;
            return;
        }

        regressionTestName = testName;
        regressionStartTime = DateTime.Now;

        EditorApplication.update += regressionTick;
        AssemblyReloadEvents.beforeAssemblyReload += onBeforeAssemblyReloadDuringRegression;

        Debug.Log(logPrefix + "Running regression test " + testName + " headless. The result appears here when the game quits.");
    }

    private static void onRegressionOutput(object sender, DataReceivedEventArgs line)
    {
        if(line.Data == null)
        {
            return;
        }

        lock(regressionOutputLock)
        {
            regressionOutput.AppendLine(line.Data);
        }
    }

    private static void regressionTick()
    {
        double elapsed = (DateTime.Now - regressionStartTime).TotalSeconds;

        try
        {
            if(regressionProcess.HasExited)
            {
                finishRegressionTest(false);
                return;
            }

            bool cancelled = EditorUtility.DisplayCancelableProgressBar(dialogTitle,
                "Regression test " + regressionTestName + ": " + elapsed.ToString("F0") + "s", 0.5f);

            if(cancelled)
            {
                finishRegressionTest(true);
            }
        }
        catch(Exception e)
        {
            Debug.LogException(e);
            finishRegressionTest(true);
        }
    }

    private static void onBeforeAssemblyReloadDuringRegression()
    {
        Debug.LogWarning(logPrefix + "Scripts are reloading, so the regression test is being stopped.");
        finishRegressionTest(true);
    }

    private static void finishRegressionTest(bool cancelled)
    {
        Process process = regressionProcess;
        regressionProcess = null;

        EditorApplication.update -= regressionTick;
        AssemblyReloadEvents.beforeAssemblyReload -= onBeforeAssemblyReloadDuringRegression;
        EditorUtility.ClearProgressBar();

        if(process == null)
        {
            return;
        }

        int exitCode = -1;

        try
        {
            if(cancelled)
            {
                killProcess(process);
                killRunningGamesSilently();
            }
            else
            {
                //flushes the asynchronous output readers
                process.WaitForExit();
                exitCode = process.ExitCode;
            }
        }
        finally
        {
            process.Dispose();
        }

        string output;

        lock(regressionOutputLock)
        {
            output = regressionOutput.ToString();
        }

        if(cancelled)
        {
            Debug.LogWarning(logPrefix + "Regression test " + regressionTestName + " was stopped.\n" + output);
        }
        else if(exitCode == 0)
        {
            Debug.Log(logPrefix + "Regression test " + regressionTestName + " PASSED.\n" + output);
        }
        else
        {
            Debug.LogError(logPrefix + "Regression test " + regressionTestName + " FAILED (exit " + exitCode + ").\n" + output);
        }
    }

    //Stopping python leaves the game it launched running, so the game is stopped too.
    private static void killRunningGamesSilently()
    {
        foreach(Process game in Process.GetProcessesByName(PlayerSettings.productName))
        {
            try
            {
                killProcess(game);
            }
            finally
            {
                game.Dispose();
            }
        }
    }

    #endregion

    [MenuItem(menuRoot + "Run Headless Build", false, 0)]
    public static void RunHeadlessBuild()
    {
        run(false);
    }

    [MenuItem(menuRoot + "Rebuild And Run Headless", false, 1)]
    public static void RebuildAndRunHeadless()
    {
        run(true);
    }

    [MenuItem(duration15Menu, false, 20)]
    private static void setDuration15() { setRunSeconds(15); }

    [MenuItem(duration15Menu, true)]
    private static bool validateDuration15() { return showDurationCheck(duration15Menu, 15); }

    [MenuItem(duration30Menu, false, 21)]
    private static void setDuration30() { setRunSeconds(30); }

    [MenuItem(duration30Menu, true)]
    private static bool validateDuration30() { return showDurationCheck(duration30Menu, 30); }

    [MenuItem(duration60Menu, false, 22)]
    private static void setDuration60() { setRunSeconds(60); }

    [MenuItem(duration60Menu, true)]
    private static bool validateDuration60() { return showDurationCheck(duration60Menu, 60); }

    [MenuItem(duration120Menu, false, 23)]
    private static void setDuration120() { setRunSeconds(120); }

    [MenuItem(duration120Menu, true)]
    private static bool validateDuration120() { return showDurationCheck(duration120Menu, 120); }

    private static int getRunSeconds()
    {
        return EditorPrefs.GetInt(runSecondsPrefKey, defaultRunSeconds);
    }

    private static void setRunSeconds(int seconds)
    {
        EditorPrefs.SetInt(runSecondsPrefKey, seconds);
    }

    private static bool showDurationCheck(string menuPath, int seconds)
    {
        Menu.SetChecked(menuPath, getRunSeconds() == seconds);

        return true;
    }

    private static void run(bool forceRebuild)
    {
        if(!canStartRun())
        {
            return;
        }

        string exePath = getExePath();

        if(forceRebuild || !isBuildFromToday(exePath))
        {
            if(!buildPlayer(exePath))
            {
                return;
            }
        }

        launch(exePath);
    }

    private static bool canStartRun()
    {
        string blocker = null;

        if(runningProcess != null || regressionProcess != null)
        {
            blocker = "A headless run is already in progress.";
        }
        else if(EditorApplication.isPlayingOrWillChangePlaymode)
        {
            blocker = "Exit Play mode before running the headless build.";
        }
        else if(EditorApplication.isCompiling || EditorApplication.isUpdating)
        {
            blocker = "Wait for Unity to finish compiling and importing, then try again.";
        }

        if(blocker == null)
        {
            return true;
        }

        EditorUtility.DisplayDialog(dialogTitle, blocker, "OK");

        return false;
    }

    private static string getExePath()
    {
        return Path.Combine(buildFolder, PlayerSettings.productName + ".exe");
    }

    private static string getDataFolder(string exePath)
    {
        return Path.Combine(Path.GetDirectoryName(exePath), Path.GetFileNameWithoutExtension(exePath) + dataFolderSuffix);
    }

    private static bool isBuildFromToday(string exePath)
    {
        if(!File.Exists(exePath))
        {
            Debug.Log(logPrefix + "No build found at " + exePath + ". Building.");
            return false;
        }

        string dataFolder = getDataFolder(exePath);
        DateTime? buildTime = readBuildTimestamp(dataFolder);

        if(buildTime == null)
        {
            string globalGameManagersPath = Path.Combine(dataFolder, globalGameManagersFileName);

            if(File.Exists(globalGameManagersPath))
            {
                buildTime = File.GetLastWriteTime(globalGameManagersPath);
            }
        }

        if(buildTime == null)
        {
            Debug.Log(logPrefix + "Could not tell when the build at " + exePath + " was made. Rebuilding.");
            return false;
        }

        string buildTimeText = buildTime.Value.ToString(Constants.buildDateAndTimeFormat);

        if(buildTime.Value.Date == DateTime.Today)
        {
            Debug.Log(logPrefix + "Build is from today (" + buildTimeText + "). Skipping rebuild.");
            return true;
        }

        Debug.Log(logPrefix + "Build is from " + buildTimeText + ", not today. Rebuilding.");

        return false;
    }

    private static DateTime? readBuildTimestamp(string dataFolder)
    {
        string timestampPath = Path.Combine(dataFolder, streamingAssetsFolderName, PrefabNames.buildTimestampFileName);

        if(!File.Exists(timestampPath))
        {
            return null;
        }

        try
        {
            string text = File.ReadAllText(timestampPath).Trim();

            if(DateTime.TryParseExact(text, Constants.buildDateAndTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime buildTime))
            {
                return buildTime;
            }
        }
        catch(IOException e)
        {
            Debug.LogWarning(logPrefix + "Could not read " + timestampPath + ": " + e.Message);
        }

        return null;
    }

    private static bool buildPlayer(string exePath)
    {
        if(!closeRunningGame())
        {
            return false;
        }

        if(!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            Debug.Log(logPrefix + "Build cancelled.");
            return false;
        }

        string[] scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();

        if(scenes.Length == 0)
        {
            Debug.LogError(logPrefix + "No scenes are enabled in Build Settings.");
            return false;
        }

        if(EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows64)
        {
            Debug.LogWarning(logPrefix + "The active build target is " + EditorUserBuildSettings.activeBuildTarget + ". Building for Windows will switch platforms and reimport assets.");
        }

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = exePath,
            target = BuildTarget.StandaloneWindows64,
            targetGroup = BuildTargetGroup.Standalone,
            options = BuildOptions.None
        };

        // BuildPlayer blocks until the build finishes and shows Unity's own progress bar.
        BuildSummary summary = BuildPipeline.BuildPlayer(options).summary;

        if(summary.result != BuildResult.Succeeded)
        {
            Debug.LogError(logPrefix + "Build " + summary.result + " with " + summary.totalErrors + " error(s). See the Console for details.");
            return false;
        }

        Debug.Log(logPrefix + "Build succeeded in " + summary.totalTime.TotalSeconds.ToString("F0") + "s.");

        return true;
    }

    /// <summary>
    /// A running copy of the game locks its files, which would make the build fail.
    /// </summary>
    private static bool closeRunningGame()
    {
        Process[] runningGames = Process.GetProcessesByName(PlayerSettings.productName);

        try
        {
            if(runningGames.Length == 0)
            {
                return true;
            }

            bool closeGame = EditorUtility.DisplayDialog(dialogTitle,
                PlayerSettings.productName + " is running. It has to be closed before the build can be replaced. Close it now?",
                "Close it", "Cancel");

            if(!closeGame)
            {
                Debug.Log(logPrefix + "Build cancelled because the game is running.");
                return false;
            }

            foreach(Process game in runningGames)
            {
                killProcess(game);
            }

            return true;
        }
        finally
        {
            foreach(Process game in runningGames)
            {
                game.Dispose();
            }
        }
    }

    private static void killProcess(Process process)
    {
        try
        {
            if(!process.HasExited)
            {
                process.Kill();
                process.WaitForExit(killWaitMilliseconds);
            }
        }
        catch(InvalidOperationException)
        {
            // The process exited between the check and the kill.
        }
        catch(System.ComponentModel.Win32Exception e)
        {
            Debug.LogWarning(logPrefix + "Could not stop process " + process.Id + ": " + e.Message);
        }
    }

    private static void launch(string exePath)
    {
        string exeFolder = Path.GetDirectoryName(exePath);
        string logPath = Path.Combine(exeFolder, playerLogFileName);

        try
        {
            if(File.Exists(logPath))
            {
                File.Delete(logPath);
            }
        }
        catch(IOException e)
        {
            Debug.LogWarning(logPrefix + "Could not delete the old player log: " + e.Message);
        }

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = exePath,
            Arguments = "-batchmode -nographics -logFile \"" + logPath + "\"",
            WorkingDirectory = exeFolder,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        try
        {
            runningProcess = Process.Start(startInfo);
        }
        catch(Exception e)
        {
            Debug.LogError(logPrefix + "Could not start " + exePath + ": " + e.Message);
            runningProcess = null;
            return;
        }

        if(runningProcess == null)
        {
            Debug.LogError(logPrefix + "Could not start " + exePath + ".");
            return;
        }

        runStartTime = DateTime.Now;
        runSeconds = getRunSeconds();
        runLogPath = logPath;

        EditorApplication.update += tick;
        AssemblyReloadEvents.beforeAssemblyReload += onBeforeAssemblyReload;

        Debug.Log(logPrefix + "Launched headless (PID " + runningProcess.Id + ") for " + runSeconds + "s. Log: " + logPath);
    }

    private static void tick()
    {
        double elapsed = getElapsedSeconds();

        try
        {
            bool exitedEarly = runningProcess.HasExited;
            bool timeUp = elapsed >= runSeconds;
            bool cancelled = false;

            if(!exitedEarly && !timeUp)
            {
                string info = "Running headless: " + elapsed.ToString("F0") + "s / " + runSeconds + "s";
                cancelled = EditorUtility.DisplayCancelableProgressBar(dialogTitle, info, (float)(elapsed / runSeconds));
            }

            if(exitedEarly || timeUp || cancelled)
            {
                finishRun(exitedEarly, cancelled, elapsed);
            }
        }
        catch(Exception e)
        {
            Debug.LogException(e);
            finishRun(false, true, elapsed);
        }
    }

    private static void onBeforeAssemblyReload()
    {
        Debug.LogWarning(logPrefix + "Scripts are reloading, so the headless run is being stopped.");
        finishRun(false, true, getElapsedSeconds());
    }

    private static double getElapsedSeconds()
    {
        return (DateTime.Now - runStartTime).TotalSeconds;
    }

    private static void finishRun(bool exitedEarly, bool cancelled, double elapsed)
    {
        Process process = runningProcess;
        runningProcess = null;

        // Unhook first so nothing below can leave the update loop or progress bar running.
        EditorApplication.update -= tick;
        AssemblyReloadEvents.beforeAssemblyReload -= onBeforeAssemblyReload;
        EditorUtility.ClearProgressBar();

        if(process == null)
        {
            return;
        }

        string elapsedText = elapsed.ToString("F0") + "s";
        string status;

        try
        {
            if(exitedEarly)
            {
                status = "exited on its own after " + elapsedText + " with code " + process.ExitCode;
            }
            else
            {
                killProcess(process);
                status = cancelled ? "was stopped early after " + elapsedText : "ran for " + elapsedText + " and was stopped";
            }
        }
        finally
        {
            process.Dispose();
        }

        report(status, exitedEarly);
    }

    private static void report(string status, bool exitedEarly)
    {
        if(!File.Exists(runLogPath))
        {
            Debug.LogError(logPrefix + "The game " + status + ", but no player log was written to " + runLogPath + ".");
            return;
        }

        List<string> problemLines = readProblemLines(runLogPath);

        if(problemLines.Count == 0 && !exitedEarly)
        {
            Debug.Log(logPrefix + "Clean run: the game " + status + ". No exceptions found. Log: " + runLogPath);
            return;
        }

        string details;

        if(problemLines.Count == 0)
        {
            details = "No exceptions in the log, but the game should keep running until stopped.";
        }
        else
        {
            details = problemLines.Count + " unique problem line(s) in the log:\n" + string.Join("\n", problemLines.Take(maxReportedLines));

            if(problemLines.Count > maxReportedLines)
            {
                details += "\n...and " + (problemLines.Count - maxReportedLines) + " more.";
            }
        }

        Debug.LogError(logPrefix + "The game " + status + ". " + details + "\nLog: " + runLogPath);
    }

    private static List<string> readProblemLines(string logPath)
    {
        List<string> problemLines = new List<string>();
        HashSet<string> seenLines = new HashSet<string>();

        try
        {
            using(FileStream stream = new FileStream(logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            using(StreamReader reader = new StreamReader(stream))
            {
                string line;

                while((line = reader.ReadLine()) != null)
                {
                    string trimmedLine = line.Trim();

                    if(problemMarkers.Any(marker => trimmedLine.Contains(marker)) && seenLines.Add(trimmedLine))
                    {
                        problemLines.Add(trimmedLine);
                    }
                }
            }
        }
        catch(IOException e)
        {
            Debug.LogWarning(logPrefix + "Could not read the player log: " + e.Message);
        }

        return problemLines;
    }
}
#endif
