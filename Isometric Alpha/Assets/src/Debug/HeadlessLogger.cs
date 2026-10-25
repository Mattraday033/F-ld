using System;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// Writes log lines to a file of your choosing, but only when the game runs headless (-batchmode).
/// In a normal run every call returns immediately and no file or folder is ever created.
///
/// Call CreateLogFile to start a new output file; every later Log/LogWarning/LogError call writes to
/// that file until CreateLogFile is called again.
///
/// Each line is appended and the file closed straight away, so nothing is lost if the process is
/// killed (as HeadlessBuildRunner does when its timer runs out).
/// </summary>
public static class HeadlessLogger
{
    private const string defaultFolderName = "HeadlessLogs";
    private const string defaultFileNamePrefix = "headless-";
    private const string defaultFileNameDateFormat = "yyyy-MM-dd_HH-mm-ss";
    private const string defaultExtension = ".log";
    private const string lineTimeFormat = "HH:mm:ss.fff";
    private const string unityLogPrefix = "[HeadlessLogger] ";

    private const string infoLevel = "INFO";
    private const string warningLevel = "WARNING";
    private const string errorLevel = "ERROR";

    private static readonly object fileLock = new object();

    private static bool? isHeadless = null;
    private static string currentLogFilePath = null;
    private static bool warnedAboutMissingFile = false;

    /// <summary>
    /// True when the game was started with -batchmode. Read once and cached, since it can't change
    /// while the game runs.
    /// </summary>
    public static bool IsHeadless
    {
        get
        {
            if(isHeadless == null)
            {
                isHeadless = Application.isBatchMode;
            }

            return isHeadless.Value;
        }
    }

    /// <summary>
    /// The file later log calls write to, or null if no file has been created (always null when
    /// not headless).
    /// </summary>
    public static string CurrentLogFilePath
    {
        get
        {
            return currentLogFilePath;
        }
    }

    /// <summary>
    /// The folder used for file names that aren't full paths: a HeadlessLogs folder next to the
    /// game's exe (next to the Assets folder when run from the editor).
    /// </summary>
    public static string DefaultFolder
    {
        get
        {
            return Path.Combine(Path.GetDirectoryName(Application.dataPath), defaultFolderName);
        }
    }

    /// <summary>
    /// Creates a new, empty log file and sends all later log calls to it. Does nothing unless the
    /// game is headless.
    /// </summary>
    /// <param name="fileName">
    /// A file name, which goes in DefaultFolder, or a full path. ".log" is added when there's no
    /// extension. Leave empty for a timestamped name like headless-2026-10-24_14-05-09.log.
    /// An existing file with the same name is replaced.
    /// </param>
    /// <returns>The new file's path, or null when not headless or the file couldn't be created.</returns>
    public static string CreateLogFile(string fileName = null)
    {
        if(!IsHeadless)
        {
            return null;
        }

        string path = resolvePath(fileName);

        lock(fileLock)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, String.Empty);
            }
            catch(Exception e) when(e is IOException || e is UnauthorizedAccessException || e is ArgumentException || e is NotSupportedException)
            {
                currentLogFilePath = null;
                Debug.LogWarning(unityLogPrefix + "Could not create log file " + path + ": " + e.Message);
                return null;
            }

            currentLogFilePath = path;
            warnedAboutMissingFile = false;
        }

        Debug.Log(unityLogPrefix + "Logging to " + path);

        return path;
    }

    public static void Log(string message)
    {
        write(infoLevel, message);
    }

    public static void LogWarning(string message)
    {
        write(warningLevel, message);
    }

    public static void LogError(string message)
    {
        write(errorLevel, message);
    }

    /// <summary>
    /// Raised after each line is written to the log file, with the full line (time and level prefix
    /// included, no trailing newline). Only raised when headless and a log file exists.
    /// </summary>
    public static event Action<string> LineWritten;

    /// <summary>
    /// Logs a structured event: "Area.Event key=value key2="value with spaces"".
    /// keyValuePairs alternates keys and values; values containing spaces or quotes are quoted and
    /// their quotes escaped. Does no formatting unless the game is headless.
    /// </summary>
    public static void LogEvent(string eventName, params object[] keyValuePairs)
    {
        if(!IsHeadless)
        {
            return;
        }

        write(infoLevel, formatEvent(eventName, keyValuePairs));
    }

    public static void LogWarningEvent(string eventName, params object[] keyValuePairs)
    {
        if(!IsHeadless)
        {
            return;
        }

        write(warningLevel, formatEvent(eventName, keyValuePairs));
    }

    public static void LogErrorEvent(string eventName, params object[] keyValuePairs)
    {
        if(!IsHeadless)
        {
            return;
        }

        write(errorLevel, formatEvent(eventName, keyValuePairs));
    }

    private static string formatEvent(string eventName, object[] keyValuePairs)
    {
        StringBuilder builder = new StringBuilder(eventName);

        if(keyValuePairs == null)
        {
            return builder.ToString();
        }

        for(int index = 0; index + 1 < keyValuePairs.Length; index += 2)
        {
            builder.Append(' ');
            builder.Append(keyValuePairs[index]);
            builder.Append('=');
            builder.Append(formatValue(keyValuePairs[index + 1]));
        }

        return builder.ToString();
    }

    private static string formatValue(object value)
    {
        string text = value == null ? "null" : value.ToString();

        text = text.Replace("\r", " ").Replace("\n", " ");

        if(text.Length == 0 || text.IndexOf(' ') >= 0 || text.IndexOf('"') >= 0 || text.IndexOf('=') >= 0)
        {
            return "\"" + text.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
        }

        return text;
    }

    private static string resolvePath(string fileName)
    {
        if(String.IsNullOrWhiteSpace(fileName))
        {
            fileName = defaultFileNamePrefix + DateTime.Now.ToString(defaultFileNameDateFormat);
        }

        if(!Path.HasExtension(fileName))
        {
            fileName += defaultExtension;
        }

        if(Path.IsPathRooted(fileName))
        {
            return fileName;
        }

        return Path.Combine(DefaultFolder, fileName);
    }

    private static void write(string level, string message)
    {
        if(!IsHeadless)
        {
            return;
        }

        string line = "[" + DateTime.Now.ToString(lineTimeFormat) + "] [" + level + "] " + message;
        bool written = false;

        lock(fileLock)
        {
            if(currentLogFilePath == null)
            {
                if(!warnedAboutMissingFile)
                {
                    warnedAboutMissingFile = true;
                    Debug.LogWarning(unityLogPrefix + "Log called before CreateLogFile, so the message was dropped: " + message);
                }

                return;
            }

            try
            {
                File.AppendAllText(currentLogFilePath, line + Environment.NewLine);
                written = true;
            }
            catch(Exception e) when(e is IOException || e is UnauthorizedAccessException)
            {
                Debug.LogWarning(unityLogPrefix + "Could not write to " + currentLogFilePath + ": " + e.Message);
            }
        }

        //raised outside the lock, so a listener that logs again can't deadlock
        if(written && LineWritten != null)
        {
            LineWritten(line);
        }
    }
}
