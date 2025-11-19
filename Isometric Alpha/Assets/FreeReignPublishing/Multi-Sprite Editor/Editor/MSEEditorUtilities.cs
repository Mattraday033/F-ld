using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace FreeReignPublishing.MultiSpriteEditor
{
    public static class MSEEditorUtilities
    {
        /// <summary>
        /// centers window inside of Unity Editor
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>

#if UNITY_2020_1_OR_NEWER
        public static Rect CenterOnMainWindow(this EditorWindow window)
        {
            var main = EditorGUIUtility.GetMainWindowPosition();
            var pos = window.position;
            float w = (main.width - pos.width) * 0.5f;
            float h = (main.height - pos.height) * 0.5f;
            pos.x = main.x + w;
            pos.y = main.y + h;
            window.position = pos;
            return pos;
        }
#else
        public static Rect CenterOnMainWindow(this EditorWindow window)
        {
            var main = new Rect(0f, 0f, Screen.currentResolution.width, Screen.currentResolution.height);
            var pos = window.position;
            float w = (main.width - pos.width)*0.5f;
            float h = (main.height - pos.height)*0.5f;
            pos.x = main.x + w;
            pos.y = main.y + h;
            window.position = pos;
            return pos; 
        }
#endif

        /// <summary>
        /// Adds a space between each Capital Letter
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string AddSpacesToSentence(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
        
        /// <summary>
        /// Delays Func until "predicate" is equal to "exitIfValue" 
        /// </summary>
        /// <param name="predicate"></param>
        /// Value to check
        /// <param name="exitIfValue"></param>
        /// Exit condition
        /// <param name="sleep"></param>
        /// Delay until next conditional check
        public static async Task WaitUntil(System.Func<bool> predicate, bool exitIfValue,  int sleep = 500)
        {
            while (exitIfValue ? !predicate() : predicate())
            {
                await Task.Delay(sleep);
            }
        }
        
        public static void SaveToJson(string path, object obj)
        {
            var json = JsonUtility.ToJson(obj);
            
            try
            {
                File.WriteAllText(Application.dataPath + path, json);
            }
            catch
            {
                Debug.LogError("MSE: Please insure save path exists: " + path);
            }
        }
        
        public static T LoadFromJson<T>(string path)
        {
            if (!File.Exists(Application.dataPath + path)) return default(T);
          
            var json = File.ReadAllText(Application.dataPath + path);
            return JsonUtility.FromJson<T>(json);
        }

        /// <summary>
        /// Plays a sound in editor. Using reflection
        /// </summary>
        /// <param name="clip"></param>

#if UNITY_2020_1_OR_NEWER
        public static void PlayClip(AudioClip clip)
        {
            var unityEditorAssembly = typeof(AudioImporter).Assembly;
     
            var audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            var method = audioUtilClass.GetMethod(
                "PlayPreviewClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
                null
            );
        
            method.Invoke(
                null,
                new object[] { clip, 0, false }
            );
        }
#else
        public static void PlayClip(AudioClip clip)
        {
            var unityEditorAssembly = typeof(AudioImporter).Assembly;
     
            var audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            var method = audioUtilClass.GetMethod(
                "PlayClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
                null
            );
        
            method.Invoke(
                null,
                new object[] { clip, 0, false }
            ); 
        }
#endif
    }
}