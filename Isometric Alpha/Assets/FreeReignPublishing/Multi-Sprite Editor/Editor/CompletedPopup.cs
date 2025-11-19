using System;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace FreeReignPublishing.MultiSpriteEditor
{
    public class CompletedPopup : EditorWindow
    {
        public enum VolumeLevel { Off, Low, Medium, High }

        public static VolumeLevel DingVolumeLevel = VolumeLevel.Medium;
    
        private static string _messageToDisplay;
        private static readonly Vector2 PopupSize = new Vector2(205, 95);
    
        private static AudioClip DingClipVolumeLow => Resources.Load<AudioClip>("Ding_Volume_Low");
        private static AudioClip DingClipVolumeMedium => Resources.Load<AudioClip>("Ding_Volume_Medium");
        private static AudioClip DingClipVolumeHigh => Resources.Load<AudioClip>("Ding_Volume_High");

        private void OnEnable() => AssemblyReloadEvents.beforeAssemblyReload += this.Close;
    
        private void OnDisable() => AssemblyReloadEvents.beforeAssemblyReload -= this.Close;

        /// <summary>
        /// Shows a "thing" Completed Popup. Before closing itself
        /// </summary>
        /// <param name="position"></param>
        /// <param name="message"></param>
        /// <param name="timeTillClose"></param>
        public static async void Show(Vector2 position, string message, int timeTillClose)
        {
            var popupInstance = ScriptableObject.CreateInstance<CompletedPopup>();
            
            var tempRect = popupInstance.position;
        
            tempRect.position = position;
            tempRect.size = PopupSize;
        
            popupInstance.position = tempRect;
        
            _messageToDisplay = message;

            popupInstance.ShowPopup();
            Ding();
        
            await Task.Delay(timeTillClose);
        
            popupInstance.Close();
        }

        public void OnGUI()
        {
            var checkmarkStyle = new GUIStyle();
            checkmarkStyle.normal.background = Resources.Load<Texture2D>("GreenCheckmark");
            checkmarkStyle.fixedWidth = 65;
            checkmarkStyle.fixedHeight = 65;
        
            var messageStyle = new GUIStyle(GUI.skin.label);
            messageStyle.fontSize = 19;
            messageStyle.fontStyle = EditorStyles.boldLabel.fontStyle;
        
            using (new GUILayout.HorizontalScope())
            {
                GUI.Box(new Rect(15, 17, 65, 65), string.Empty, checkmarkStyle);
                GUI.Label(new Rect(95, 0, 200, 100), _messageToDisplay, messageStyle);
            }
        }

        /// <summary>
        /// Plays a custom Ding, At the current "VolumeLevel".
        /// </summary>
        public static void Ding()
        {
            switch (DingVolumeLevel)
            {
                case VolumeLevel.Off:
                    // Play no sound, so return.
                    return;
                case VolumeLevel.Low:
                    MSEEditorUtilities.PlayClip(DingClipVolumeLow);
                    break;
                case VolumeLevel.Medium:
                    MSEEditorUtilities.PlayClip(DingClipVolumeMedium);
                    break;
                case VolumeLevel.High:
                    MSEEditorUtilities.PlayClip(DingClipVolumeHigh);
                    break;
            }
        }
    }
}