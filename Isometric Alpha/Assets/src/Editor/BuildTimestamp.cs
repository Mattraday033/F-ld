using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

/// <summary>
/// Writes a Build-Timestamp text file holding the date and time of the build.
///
/// The file is written into Assets/StreamingAssets before the build so it is packaged with the
/// player, and written again into the built player's StreamingAssets folder afterwards so the
/// shipped file always matches the build that contains it.
/// </summary>
public class BuildTimestamp : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    private const string streamingAssetsFolderName = "StreamingAssets";

    private const string windowsAndLinuxDataFolderSuffix = "_Data";
    private const string macDataFolder = "/Contents/Resources/Data";
    private const string macAppExtension = ".app";

    private static string buildDateAndTime = null;

    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        buildDateAndTime = DateTime.Now.ToString(Constants.buildDateAndTimeFormat);

        writeBuildTimestampFile(Application.streamingAssetsPath, buildDateAndTime);

        AssetDatabase.Refresh();
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        if(String.IsNullOrEmpty(buildDateAndTime))
        {
            buildDateAndTime = DateTime.Now.ToString(Constants.buildDateAndTimeFormat);
        }

        string streamingAssetsFolderInBuild = getStreamingAssetsFolderInBuild(report);

        if(String.IsNullOrEmpty(streamingAssetsFolderInBuild))
        {
            return;
        }

        writeBuildTimestampFile(streamingAssetsFolderInBuild, buildDateAndTime);
    }

    private static void writeBuildTimestampFile(string streamingAssetsFolder, string dateAndTime)
    {
        if(!Directory.Exists(streamingAssetsFolder))
        {
            Directory.CreateDirectory(streamingAssetsFolder);
        }

        File.WriteAllText(Path.Combine(streamingAssetsFolder, PrefabNames.buildTimestampFileName), dateAndTime);
    }

    /// <summary>
    /// Returns the StreamingAssets folder inside the built player, or null for platforms whose
    /// output cannot be written to directly (Android, iOS, consoles).
    /// </summary>
    private static string getStreamingAssetsFolderInBuild(BuildReport report)
    {
        string outputPath = report.summary.outputPath;

        if(String.IsNullOrEmpty(outputPath))
        {
            return null;
        }

        switch(report.summary.platform)
        {
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
            case BuildTarget.StandaloneLinux64:
                string outputFolder = Path.GetDirectoryName(outputPath);
                string dataFolderName = Path.GetFileNameWithoutExtension(outputPath) + windowsAndLinuxDataFolderSuffix;

                return Path.Combine(outputFolder, dataFolderName, streamingAssetsFolderName);

            case BuildTarget.StandaloneOSX:
                string appFolder = outputPath.EndsWith(macAppExtension, StringComparison.OrdinalIgnoreCase) ? outputPath : outputPath + macAppExtension;

                return Path.Combine(appFolder + macDataFolder, streamingAssetsFolderName);

            default:
                return null;
        }
    }

}
