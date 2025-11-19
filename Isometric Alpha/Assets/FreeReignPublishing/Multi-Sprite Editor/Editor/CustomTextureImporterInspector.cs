using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
#if UNITY_2019
using UnityEditor.Experimental.AssetImporters;
#else
using UnityEditor.AssetImporters;
#endif

namespace FreeReignPublishing.MultiSpriteEditor
{
    [CustomEditor(typeof(TextureImporter), true)]
    [CanEditMultipleObjects]
    public class CustomTextureImporterInspector : Editor
    { 
        private static AssetImporterEditor _nativeTextureImporterEditor; 
        private Delegate _spriteGUIDelegate;
        
        private void OnEnable()
        {
            //Only want to create and modify inspector when its non-existent
            if (_nativeTextureImporterEditor != null) return;
            
            //"TextureImporterInspector" is the class that renders the Texture Importer by default
            var textureImporterInspector_Type = Type.GetType("UnityEditor.TextureImporterInspector, UnityEditor");
            
            //Here we create a new copy of the original TextureImporterInspector
            _nativeTextureImporterEditor = (AssetImporterEditor)AssetImporterEditor.CreateEditor(targets, textureImporterInspector_Type);

            //Needed as in some cases its still null
            if (_nativeTextureImporterEditor == null)
                return;
        
            //Errors if not set
            textureImporterInspector_Type.GetMethod("InternalSetAssetImporterTargetEditor", BindingFlags.NonPublic | BindingFlags.Instance)
                                         .Invoke(_nativeTextureImporterEditor, new object[] { this });

            //Both needed for swapping out old GUIMethod for new one
            var guiMethod_Type = textureImporterInspector_Type.GetNestedType("GUIMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            var guiElementMethods_Obj = textureImporterInspector_Type.GetField("m_GUIElementMethods", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_nativeTextureImporterEditor);

            //Needed to grab the right Remove Method
            var genericArguments = guiElementMethods_Obj.GetType().GetGenericArguments();
            var keyType = genericArguments[0]; // Maybe implement some error handling.

            //Remove/Add Methods from Dictionary's 
            var dicRemoveMethod = guiElementMethods_Obj.GetType().GetMethod("Remove", new [] { keyType });
            var dicAddMethod = guiElementMethods_Obj.GetType().GetMethod("Add");

            //create instance of "this" class to get our new SpriteGUI method
            var myClass = ScriptableObject.CreateInstance(this.GetType());
            var mySpriteGUI = myClass.GetType().GetMethod("MySpriteGUI", BindingFlags.NonPublic | BindingFlags.Instance);
            
            //The new GUIMethod to replace the old one in "TextureImporterInspector"
            _spriteGUIDelegate = Delegate.CreateDelegate(guiMethod_Type, myClass, mySpriteGUI);

            //Remove's the SpriteGUI method
            dicRemoveMethod.Invoke(guiElementMethods_Obj, new object[] { 64 });
            
            //Add's our SpriteGUI method
            dicAddMethod.Invoke(guiElementMethods_Obj, new object[] { 64, _spriteGUIDelegate });
        }

#if UNITY_2023_1_OR_NEWER
        private void MySpriteGUI(object guiElements)
        {
            //Display the default sprite mode GUI
            _nativeTextureImporterEditor.GetType().GetMethod("SpriteGUI", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_nativeTextureImporterEditor, new object[] { null });

            //Display our custom "MSE" button

            using (new EditorGUI.DisabledScope(!MultiSpriteEditor.CanBeOpened()))
            {
                if (GUILayout.Button("Open Multi-Sprite Editor", GUILayout.ExpandWidth(true), GUILayout.Height(19)))
                {
                    MultiSpriteEditor.Open();
                }
            }

            EditorGUI.indentLevel = 0;
        }
#elif UNITY_2019_1_OR_NEWER
        private void MySpriteGUI(object guiElements)
        {
            //Display the default sprite mode GUI
            _nativeTextureImporterEditor.GetType().GetMethod("SpriteGUI", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_nativeTextureImporterEditor, new object[] { null });
        
            //Display our custom "MSE" button
            
            GUILayout.Space(-21);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Space(-85);
        
            using (new EditorGUI.DisabledScope(!MultiSpriteEditor.CanBeOpened()))
            {
                if (GUILayout.Button("Multi-Sprite Editor", GUILayout.Width(114), GUILayout.Height(19)))
                {
                    MultiSpriteEditor.Open();
                }
            }
        
            GUILayout.Space(85);
            GUILayout.EndHorizontal();
            GUILayout.Space(21);
            EditorGUI.indentLevel = 0;
        }
#endif

        private void OnDisable()
        {
            _spriteGUIDelegate = null;
        }

        private void OnDestroy()
        {
            _spriteGUIDelegate = null;

            if (_nativeTextureImporterEditor == null) return;

            try
            {
                DestroyImmediate(_nativeTextureImporterEditor);
                _nativeTextureImporterEditor = null;
            }
            catch { }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (_nativeTextureImporterEditor != null) 
                _nativeTextureImporterEditor.OnInspectorGUI();
            serializedObject.ApplyModifiedProperties();
        }
    }
}