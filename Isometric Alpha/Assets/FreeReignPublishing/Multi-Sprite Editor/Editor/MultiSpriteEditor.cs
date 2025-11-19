using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Threading.Tasks;
using UnityEditor.AnimatedValues;
using UnityEngine.Serialization;

namespace FreeReignPublishing.MultiSpriteEditor
{

    // Contains all Reflection Type's to save.
    [Serializable]
    public class ReflectionCache
    {
        public Dictionary<string, Type> Types = new Dictionary<string, Type>();
        public Dictionary<string, MethodInfo> Methods = new Dictionary<string, MethodInfo>();
        public Dictionary<string, FieldInfo> Fields = new Dictionary<string, FieldInfo>();
        public Dictionary<string, PropertyInfo> Properties = new Dictionary<string, PropertyInfo>();
        public Dictionary<string, object> Objects = new Dictionary<string, object>();
    }

    // Contains all the setting's to save.
    [Serializable]
    public class SettingsSaveObject
    {
        public CompletedPopup.VolumeLevel volume;
        public MultiSpriteEditor.SupportedDockPositions autoDockPosition;
        public MultiSpriteEditor.TextureSelectionOptions textureSelectionOption;
        public bool closeOnComplete;
        public bool hideMseBanner;
    }
    
    // Used to display messages in GUI.
    public class Message
    {
        public readonly string Text;
        public readonly MessageType Type;

        public Message(string text, MessageType type)
        {
            this.Text = text;
            this.Type = type;
        }
    }

    public class MultiSpriteEditor : EditorWindow
    {

        // Menu to display.
        private enum Menu
        {
            SPRITE_EDITOR, SPRITE_EDITOR_EXECUTE,

            CUSTOM_OUTLINE, CUSTOM_OUTLINE_EXECUTE,

            CUSTOM_PHYSICS_SHAPE, CUSTOM_PHYSICS_SHAPE_EXECUTE,

            SETTINGS, UNSUPPORTED
        }

        public enum SupportedDockPositions { Left, Right, None }

        // Sprite Editor Instances
        private static ScriptableObject _ogSpriteEditorInstance; // "og" meaning first Instance
        private ScriptableObject _reloadedSpriteEditorInstance; // "reloaded" meaning a copy of "og" on assembly reloaded

        private static EditorWindow _spriteEditorWindow;

        private Texture2D[] _selectedTexture2Ds;
        
        private ReflectionCache _moduleReflectionCache;
        private ReflectionCache _spriteEditorReflectionCache;

        private static SupportedDockPositions _autoDockPosition;
        private Menu _currentlyDisplayedMenu;
        private AnimBool _dropdownAnim;

        private int _objectSelectionIndex;
        private int _texture2DsSlicedOrOutlined;
        private int _spritesOutlined;
        private int _rectsCount;

        private float _outlineTesselationValue;

        private bool _isPaused;
        private bool _isExecuting;
        private bool _settingsChanged;
        private bool _closeOnComplete;
        private bool _hideMseBanner;
        
        public enum TextureSelectionOptions { Default, Ping }

        private TextureSelectionOptions _textureSelectionOption = TextureSelectionOptions.Ping;

        // Can Adjust "DelayBeforeAction"/"DelayAfterAction" to notice outline being applied more. Warning might break.

        private const int DelayBeforeAction = 50; // Meaning before Slicing/Outlining
        private const int DelayAfterAction = 350; // Meaning after Slicing/Outlining

        private const int GUI_MenuYPos = 25; // Every GUI Menu's Y position
        private const int GUI_MenuWidth = 168; // Every GUI Menu's width
        private const float GUI_MenuWidthHalf = GUI_MenuWidth * 0.5f;

        private readonly Vector2 SpriteEditorSize = new Vector2(1282, 732);

        private const string SavePath = "/FreeReignPublishing/Multi-Sprite Editor/MSE_Settings_Save.txt";
        private const string StampOfApproval = "\u00A9 2023 Free Reign Publishing Inc.";

        private const string LeftArrowUnicode = "\u2190";
        private const string RightArrowUnicode = "\u2192";

        // Used to display an Error Message in a Menu
        private readonly Message _cantOutlineMessage = new Message("Can't Outline Un-Sliced Sprites.", MessageType.Error);

        // Used to change selected Texture
        private int ObjectSelectionIndex
        {
            get => _objectSelectionIndex;
            set
            {
                _objectSelectionIndex = Mathf.Clamp(value, 0, _selectedTexture2Ds.Length - 1);
                Selection.activeObject = _selectedTexture2Ds[_objectSelectionIndex];
                
                if (_textureSelectionOption == TextureSelectionOptions.Ping)
                    EditorGUIUtility.PingObject(_selectedTexture2Ds[_objectSelectionIndex]);
            }
        }

#if UNITY_2019
        private bool IsDocked
        {
            get
            {
                BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
                MethodInfo method = GetType().GetProperty("docked", flags).GetGetMethod(true);
                return (bool)method.Invoke(this, null);
            }
        }
#endif

        // DONT CHANGE FROM ZERO: 0 = (Sprite Editor Module) which is always default
        private int _lastModuleIndex = 0;

        // MAIN

        [MenuItem("Window/2D/Multi-Sprite Editor #s")]
        public static void Open()
        {
            _ogSpriteEditorInstance = GetWindow(typeof(UnityEditor.U2D.Sprites.SpriteEditorModuleBase).Assembly.GetType("UnityEditor.U2D.Sprites.SpriteEditorWindow"));
            _spriteEditorWindow = (EditorWindow)_ogSpriteEditorInstance;

            if (_spriteEditorWindow != null)
                _spriteEditorWindow.Show();

            if (_autoDockPosition == SupportedDockPositions.None)
            {
                var tool = GetWindow<MultiSpriteEditor>("MSE Controls");
                tool.Focus();
            }
            else
            {
                GetWindow<MultiSpriteEditor>("MSE Controls", typeof(UnityEditor.U2D.Sprites.SpriteEditorModuleBase).Assembly.GetType("UnityEditor.U2D.Sprites.SpriteEditorWindow"));
                _spriteEditorWindow.Focus();
            }
        }

        [MenuItem("Window/2D/Multi-Sprite Editor #s", true)]
        public static bool CanBeOpened()
        {
            return Selection.GetFiltered<Texture2D>(SelectionMode.Assets).Length > 1 && _spriteEditorWindow == null;
        }

        // CALLBACKS

        private async void OnEnable()
        {
            LoadSettings();
            SetupDropdownAnim();
            _selectedTexture2Ds = Selection.GetFiltered<Texture2D>(SelectionMode.Assets);

            if (_ogSpriteEditorInstance != null) // first time opened
            {
                var tempRect = _spriteEditorWindow.position;
                tempRect.size = SpriteEditorSize;
                _spriteEditorWindow.position = tempRect;

                var updatedWndPosition = _spriteEditorWindow.CenterOnMainWindow().position;

                AutoDock(updatedWndPosition);
            }
            else // Assembly Reloaded
                _spriteEditorWindow = _reloadedSpriteEditorInstance as EditorWindow;

            await Task.Delay(500);

            this.minSize = new Vector2(199, 289);
            this.maxSize = new Vector2(199, 289);

            if (_selectedTexture2Ds.Length <= 1)
            {
                _spriteEditorWindow.Close();
                this.Close();
            }

            _spriteEditorReflectionCache = SpriteEditorReflectionCache();
            _moduleReflectionCache = SliceModuleReflectionCache();

            var mseBackground = new GUIStyle();
            mseBackground.normal.background = Resources.Load<Texture2D>("MSE - Menu Background");
            mseBackground.fixedWidth = 171;
            mseBackground.fixedHeight = 21;

            var mseBackground2 = new GUIStyle();
            mseBackground2.normal.background = Resources.Load<Texture2D>("MSE - Background Attachment");
            mseBackground2.fixedWidth = 171;

            SwitchMenu(Menu.SPRITE_EDITOR);
        }

        private void OnDisable()
        {
            if (_ogSpriteEditorInstance != null)
                _reloadedSpriteEditorInstance = _ogSpriteEditorInstance;

            _spriteEditorReflectionCache = null;
            _moduleReflectionCache = null;
        }

        private void OnGUI()
        {
            EditorGUI.BeginDisabledGroup(!_dropdownAnim.target); // To prevent interacting with a menu

            DisplayMenu(_currentlyDisplayedMenu);

            EditorGUI.EndDisabledGroup(); // To prevent interacting with a menu

            //Menu Display - Cosmetic

            var enumStyle = new GUIStyle(EditorStyles.popup) { alignment = TextAnchor.MiddleCenter, fontStyle = EditorStyles.boldLabel.fontStyle };
            GUI.Box(new Rect(new Vector2((position.width * 0.5f) - GUI_MenuWidthHalf, 3), new Vector2(GUI_MenuWidth, 10)), UpdateMenuDisplayText(_currentlyDisplayedMenu), enumStyle);

            //Copyright

            GUI.Box(new Rect(0, position.height - 20, position.width, 20), string.Empty);
            var labelStyle = GUI.skin.label;
            GUI.Label(new Rect(position.width / 2 - 98, position.height - 20, position.width, 20), StampOfApproval, labelStyle);
        }

        private void OnInspectorUpdate()
        {
            if (_spriteEditorWindow == null)
                this.Close();

            if (_spriteEditorReflectionCache == null)
                return;

            var newModuleIndex = _lastModuleIndex;

            if (_spriteEditorReflectionCache.Fields.TryGetValue("m_CurrentModuleIndex", out FieldInfo field))
                newModuleIndex = (int)field.GetValue(_spriteEditorWindow);

            if (_lastModuleIndex != newModuleIndex)
            {
                if (_isExecuting)
                {
                    _spriteEditorReflectionCache.Types["SpriteEditorWindow"].GetMethod("SetupModule", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_spriteEditorWindow, new object[] { _lastModuleIndex });
                    return;
                }

                switch (newModuleIndex)
                {
                    case 0:
                        _moduleReflectionCache = SliceModuleReflectionCache();
                        SwitchMenu(Menu.SPRITE_EDITOR);
                        break;

                    case 1:
                        _moduleReflectionCache = OutlineModuleReflectionCache();
                        SwitchMenu(Menu.CUSTOM_OUTLINE);
                        break;

                    case 2:
                        _moduleReflectionCache = OutlineModuleReflectionCache();
                        SwitchMenu(Menu.CUSTOM_PHYSICS_SHAPE);
                        break;

                    default:
                        _moduleReflectionCache = null;
                        SwitchMenu(Menu.UNSUPPORTED);
                        break;
                }

                Repaint();
                _lastModuleIndex = newModuleIndex;
            }
        }

        // REFLECTION CACHE

        private ReflectionCache SpriteEditorReflectionCache()
        {
            var cache = new ReflectionCache();

            var spriteEditorWindowType = typeof(UnityEditor.U2D.Sprites.SpriteEditorModuleBase).Assembly.GetType("UnityEditor.U2D.Sprites.SpriteEditorWindow");

            cache.Types.Add("SpriteEditorWindow", spriteEditorWindowType);
            cache.Fields.Add("m_RectsCache", spriteEditorWindowType.GetField("m_RectsCache", BindingFlags.Instance | BindingFlags.NonPublic));
            cache.Fields.Add("m_CurrentModule", spriteEditorWindowType.GetField("m_CurrentModule", BindingFlags.Instance | BindingFlags.NonPublic));
            cache.Fields.Add("m_CurrentModuleIndex", spriteEditorWindowType.GetField("m_CurrentModuleIndex", BindingFlags.Instance | BindingFlags.NonPublic));

            return cache;
        }

        private ReflectionCache SliceModuleReflectionCache()
        {
            var cache = new ReflectionCache();

            cache.Types.Add("SpriteEditorMenu", typeof(UnityEditor.U2D.Sprites.ISpriteEditor).Assembly.GetType("UnityEditor.U2D.Sprites.SpriteEditorMenu"));

            var moduleInstance = _spriteEditorReflectionCache.Fields["m_CurrentModule"].GetValue(_spriteEditorWindow);

            if (moduleInstance == null)
            {
                _spriteEditorReflectionCache.Types["SpriteEditorWindow"].GetMethod("SetupModule", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_spriteEditorWindow, new object[] { 0 });
                moduleInstance = _spriteEditorReflectionCache.Fields["m_CurrentModule"].GetValue(_spriteEditorWindow);
            }

            cache.Objects.Add("moduleInstance", moduleInstance);
            cache.Types.Add("moduleInstance", moduleInstance.GetType());


            cache.Objects.Add("texProvider", cache.Types["moduleInstance"].GetField("m_TextureDataProvider", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(cache.Objects["moduleInstance"]));

            cache.Methods.Add("DoSlicing", cache.Types["SpriteEditorMenu"].GetMethod("DoSlicing", BindingFlags.Instance | BindingFlags.NonPublic));

            return cache;
        }

        private ReflectionCache OutlineModuleReflectionCache()
        {
            var cache = new ReflectionCache();

            var moduleInstance = _spriteEditorReflectionCache.Fields["m_CurrentModule"].GetValue(_spriteEditorWindow);

            int moduleIndex = (int)_spriteEditorReflectionCache.Fields["m_CurrentModuleIndex"].GetValue(_spriteEditorWindow);

            cache.Objects.Add("moduleInstance", moduleInstance);

            cache.Types.Add("moduleInstance", moduleIndex == 1 ? moduleInstance.GetType() : moduleInstance.GetType().BaseType);

            cache.Fields.Add("m_Selected", cache.Types["moduleInstance"].GetField("m_Selected", BindingFlags.Instance | BindingFlags.NonPublic));

            cache.Properties.Add("selectedShapeOutline", cache.Types["moduleInstance"].GetProperty("selectedShapeOutline", BindingFlags.Instance | BindingFlags.NonPublic));

            cache.Methods.Add("SetupShapeEditorOutline", cache.Types["moduleInstance"].GetMethod("SetupShapeEditorOutline", BindingFlags.Instance | BindingFlags.NonPublic));
            cache.Methods.Add("SetDataModified", _spriteEditorReflectionCache.Types["SpriteEditorWindow"].GetMethod("SetDataModified", BindingFlags.Instance | BindingFlags.Public));
            cache.Properties.Add("shapeEditorDirty", cache.Types["moduleInstance"].GetProperty("shapeEditorDirty", BindingFlags.Instance | BindingFlags.NonPublic));

            var rects = _spriteEditorReflectionCache.Fields["m_RectsCache"].GetValue(_spriteEditorWindow) as IList<SpriteRect>;
            _rectsCount = rects.Count;

            return cache;
        }

        // SETUP

        private void SetupDropdownAnim()
        {
            _dropdownAnim = new AnimBool(false);
            _dropdownAnim.valueChanged.AddListener(Repaint);
            _dropdownAnim.speed = 1.2f;
        }

        // SAVE/LOAD

        private void SaveSettings()
        {
            var settings = new SettingsSaveObject
            {
                autoDockPosition = _autoDockPosition,
                volume = CompletedPopup.DingVolumeLevel,
                closeOnComplete = _closeOnComplete,
                hideMseBanner = _hideMseBanner,
                textureSelectionOption = _textureSelectionOption
            };

            MSEEditorUtilities.SaveToJson(SavePath, settings);

            _settingsChanged = false;
        }

        private void LoadSettings()
        {
            var settings = MSEEditorUtilities.LoadFromJson<SettingsSaveObject>(SavePath);

            if (settings == null)
                return;

            _autoDockPosition = settings.autoDockPosition;
            _closeOnComplete = settings.closeOnComplete;
            _hideMseBanner = settings.hideMseBanner;
            _textureSelectionOption = settings.textureSelectionOption;
            CompletedPopup.DingVolumeLevel = settings.volume;

            _settingsChanged = false;
        }

        // GUI

        private void DisplayMenu(Menu menu)
        {
            Vector2 dropdownPosition = new Vector2((position.width * 0.5f) - GUI_MenuWidthHalf, GUI_MenuYPos);

            var bg = new GUIStyle();
            bg.normal.background = Resources.Load<Texture2D>("MSE - Menu Background");

            var logo = new GUIStyle();
            logo.normal.background = Resources.Load<Texture2D>("MSE - Menu Backdrop");

            switch (menu)
            {
                case Menu.SPRITE_EDITOR:
                    GUILayout.BeginArea(new Rect(dropdownPosition, new Vector2(GUI_MenuWidth, _dropdownAnim.faded * 180)), string.Empty, EditorStyles.helpBox);
                    SliceMenuGUI();
                    GUILayout.EndArea();

                    if (!_hideMseBanner)
                    {
                        logo.normal.background = Resources.Load<Texture2D>("MSE - Menu Backdrop");
                        GUI.Box(new Rect(new Vector2(dropdownPosition.x, GUI_MenuYPos + _dropdownAnim.faded * 180 + 5),
                                new Vector2(logo.normal.background.width, logo.normal.background.height)),
                                string.Empty, logo);
                    }
                    break;

                case Menu.SPRITE_EDITOR_EXECUTE:
                    GUILayout.BeginArea(new Rect(dropdownPosition, new Vector2(GUI_MenuWidth, _dropdownAnim.faded * 96)), string.Empty, EditorStyles.helpBox);
                    SlicingMenuGUI();
                    GUILayout.EndArea();

                    if (!_hideMseBanner)
                    {
                        logo.normal.background = Resources.Load<Texture2D>("MSE - Menu Backdrop");
                        GUI.Box(new Rect(new Vector2(dropdownPosition.x, GUI_MenuYPos + _dropdownAnim.faded * 96 + 5),
                                new Vector2(logo.normal.background.width, logo.normal.background.height)),
                                string.Empty, logo);
                    }              
                    break;

                case Menu.CUSTOM_OUTLINE:
                case Menu.CUSTOM_PHYSICS_SHAPE:
                    GUILayout.BeginArea(new Rect(dropdownPosition, new Vector2(GUI_MenuWidth, _dropdownAnim.faded * 240)), string.Empty, EditorStyles.helpBox);
                    OutlineMenuGUI();
                    GUILayout.EndArea();

                    if (!_hideMseBanner)
                    {
                        logo.normal.background = Resources.Load<Texture2D>("MSE - Menu Backdrop");
                        GUI.Box(new Rect(new Vector2(dropdownPosition.x, GUI_MenuYPos + _dropdownAnim.faded * 240 + 5),
                                new Vector2(logo.normal.background.width, logo.normal.background.height)),
                                string.Empty, logo);
                    }                    
                    break;

                case Menu.CUSTOM_OUTLINE_EXECUTE:
                case Menu.CUSTOM_PHYSICS_SHAPE_EXECUTE:
                    GUILayout.BeginArea(new Rect(dropdownPosition, new Vector2(GUI_MenuWidth, _dropdownAnim.faded * 201)), string.Empty, EditorStyles.helpBox);
                    OutliningMenuGUI();
                    GUILayout.EndArea();

                    if (!_hideMseBanner)
                    {
                        logo.normal.background = Resources.Load<Texture2D>("MSE - Menu Backdrop");
                        GUI.Box(new Rect(new Vector2(dropdownPosition.x, GUI_MenuYPos + _dropdownAnim.faded * 203 + 5),
                                new Vector2(logo.normal.background.width, logo.normal.background.height)),
                                string.Empty, logo);
                    }             
                    break;

                case Menu.SETTINGS:
                    GUILayout.BeginArea(new Rect(dropdownPosition, new Vector2(GUI_MenuWidth, _dropdownAnim.faded * 241)), string.Empty, EditorStyles.helpBox);
                    SettingsGUI();
                    GUILayout.EndArea();

                    if (!_hideMseBanner)
                    {
                        logo.normal.background = Resources.Load<Texture2D>("MSE - Menu Backdrop");
                        GUI.Box(new Rect(new Vector2(dropdownPosition.x, GUI_MenuYPos + _dropdownAnim.faded * 241 + 5),
                                new Vector2(logo.normal.background.width, logo.normal.background.height)),
                                string.Empty, logo);
                    }                    
                    break;

                case Menu.UNSUPPORTED:
                    break;
            }
        }

        private void SliceMenuGUI() 
        {
            //SLICE - GROUP
            using (new GUILayout.AreaScope(new Rect(new Vector2(0, 0), new Vector2(GUI_MenuWidth, 120)), string.Empty, EditorStyles.helpBox))
            {
                if (GUI.Button(new Rect(GUI_MenuWidthHalf - 57.5f, 15, 115, 50), "Auto-Slice"))
                    SwitchMenu(Menu.SPRITE_EDITOR_EXECUTE);

                GUILayout.BeginHorizontal();

                if (GUI.Button(new Rect(GUI_MenuWidthHalf - 65 - 5, 75, 65, 30), LeftArrowUnicode))
                    ObjectSelectionIndex++;

                if (GUI.Button(new Rect(GUI_MenuWidthHalf + 5, 75, 65, 30), RightArrowUnicode))
                    ObjectSelectionIndex--;

                GUILayout.EndHorizontal();
            }

            if (GUI.Button(new Rect(GUI_MenuWidthHalf - 155 * 0.5f, 125, 155, 50), "Settings"))
                SwitchMenu(Menu.SETTINGS);
        }
        private void SlicingMenuGUI() 
        {
            //PROGRESS BAR - GROUP
            using (new GUILayout.AreaScope(new Rect(0, 0, GUI_MenuWidth, 60), string.Empty, EditorStyles.helpBox))
            {
                EditorGUI.ProgressBar(new Rect(10, 5, GUI_MenuWidth - 20, 50), _texture2DsSlicedOrOutlined == 0 ? 0 : (float)_texture2DsSlicedOrOutlined / (float)_selectedTexture2Ds.Length, $"Completed: {_texture2DsSlicedOrOutlined:0}/{_selectedTexture2Ds.Length}");
            }

            //PROGRESS CONTROLS - GROUP
            using (new GUILayout.AreaScope(new Rect(new Vector2(0, 60), new Vector2(GUI_MenuWidth, 36)), string.Empty, EditorStyles.helpBox))
            {
                var pauseButton = new GUIContent() { image = EditorGUIUtility.FindTexture(_isPaused ? "PlayButton On" : "PauseButton On") };

                GUILayout.BeginHorizontal();

                if (GUILayout.Button(pauseButton, GUILayout.Width(30), GUILayout.Height(30)))
                    _isPaused = !_isPaused;

                EditorGUI.BeginDisabledGroup(_isPaused);

                if (GUILayout.Button("Cancel", GUILayout.Width(126), GUILayout.Height(30)))
                    _isExecuting = false;

                EditorGUI.EndDisabledGroup();

                GUILayout.EndHorizontal();
            }
        }

        private void OutlineMenuGUI() 
        {
            EditorGUI.BeginDisabledGroup(_rectsCount <= 0);

            //TOLERANCE SLIDER - GROUP
            using (new GUILayout.AreaScope(new Rect(new Vector2(0, 0), new Vector2(GUI_MenuWidth, 60)), string.Empty, EditorStyles.helpBox))
            {
                EditorGUILayout.Space(5);

                GUILayout.BeginHorizontal();
                EditorGUILayout.Space(0);
                GUILayout.Label("Tolerance", EditorStyles.boldLabel);
                GUILayout.EndHorizontal();

                EditorGUILayout.Space(7);

                _outlineTesselationValue = EditorGUILayout.Slider(_outlineTesselationValue, 0, 1);
            }

            //OUTLINE - GROUP
            using (new GUILayout.AreaScope(new Rect(new Vector2(0, 60), new Vector2(GUI_MenuWidth, 120)), string.Empty, EditorStyles.helpBox))
            {
                if (GUI.Button(new Rect(GUI_MenuWidthHalf - 57.5f, 15, 115, 50), "Auto-Outline"))
                    SwitchMenu(_lastModuleIndex == 1 ? Menu.CUSTOM_OUTLINE_EXECUTE : Menu.CUSTOM_PHYSICS_SHAPE_EXECUTE);

                GUILayout.BeginHorizontal();

                if (GUI.Button(new Rect(GUI_MenuWidthHalf - 65 - 5, 75, 65, 30), LeftArrowUnicode))
                {
                    ObjectSelectionIndex++;
                    ObjectSelectionIndex = Mathf.Clamp(ObjectSelectionIndex, 0, _selectedTexture2Ds.Length - 1);
                    Selection.activeObject = _selectedTexture2Ds[ObjectSelectionIndex];
                }

                if (GUI.Button(new Rect(GUI_MenuWidthHalf + 5, 75, 65, 30), RightArrowUnicode))
                {
                    ObjectSelectionIndex--;
                    ObjectSelectionIndex = Mathf.Clamp(ObjectSelectionIndex, 0, _selectedTexture2Ds.Length - 1);
                    Selection.activeObject = _selectedTexture2Ds[ObjectSelectionIndex];
                }

                GUILayout.EndHorizontal();
            }

            EditorGUI.EndDisabledGroup();

            if (_rectsCount <= 0)
            {
                GUI.Box(new Rect(new Vector2(GUI_MenuWidthHalf - (GUI_MenuWidth - 10) * 0.5f, 90 - (40 * 0.5f)), new Vector2(GUI_MenuWidth - 10, 40)), string.Empty);
                GUI.Box(new Rect(new Vector2(GUI_MenuWidthHalf - (GUI_MenuWidth - 10) * 0.5f, 90 - (40 * 0.5f)), new Vector2(GUI_MenuWidth - 10, 40)), string.Empty);
                GUI.Box(new Rect(new Vector2(GUI_MenuWidthHalf - (GUI_MenuWidth - 10) * 0.5f, 90 - (40 * 0.5f)), new Vector2(GUI_MenuWidth - 10, 40)), string.Empty);
                EditorGUI.HelpBox(new Rect(new Vector2(GUI_MenuWidthHalf - (GUI_MenuWidth - 10) * 0.5f, 90 - (40 * 0.5f)), new Vector2(GUI_MenuWidth - 10, 40)), _cantOutlineMessage.Text, _cantOutlineMessage.Type);
            }

            if (GUI.Button(new Rect(GUI_MenuWidthHalf - (155 * 0.5f), 185, 155, 50), "Settings"))
                SwitchMenu(Menu.SETTINGS);
        }
        private void OutliningMenuGUI() 
        {
            //PROGRESS BARS - GROUP
            using (new GUILayout.AreaScope(new Rect(0, 0, GUI_MenuWidth, 105), string.Empty, EditorStyles.helpBox))
            {
                EditorGUI.ProgressBar(new Rect(10, 10, GUI_MenuWidth - 20, 50), _texture2DsSlicedOrOutlined == 0 ? 0 : (float)_texture2DsSlicedOrOutlined / (float)_selectedTexture2Ds.Length, $"Completed: {_texture2DsSlicedOrOutlined:0}/{_selectedTexture2Ds.Length}");
                EditorGUI.ProgressBar(new Rect(10, 70, GUI_MenuWidth - 20, 25), _spritesOutlined == 0 ? 0 : (float)_spritesOutlined / (float)_rectsCount, $"Outlined: {_spritesOutlined:0}/{_rectsCount}");
            }

            EditorGUI.BeginDisabledGroup(!_isPaused);

            //TOLERANCE SLIDER - GROUP
            using (new GUILayout.AreaScope(new Rect(new Vector2(0, 105), new Vector2(GUI_MenuWidth, 60)), string.Empty, EditorStyles.helpBox))
            {
                EditorGUILayout.Space(5);

                GUILayout.BeginHorizontal();
                EditorGUILayout.Space(0);
                GUILayout.Label("Tolerance", EditorStyles.boldLabel);
                GUILayout.EndHorizontal();

                EditorGUILayout.Space(7);

                _outlineTesselationValue = EditorGUILayout.Slider(_outlineTesselationValue, 0, 1);
            }

            EditorGUI.EndDisabledGroup();

            //PROGRESS CONTROLS - GROUP
            using (new GUILayout.AreaScope(new Rect(new Vector2(0, 165), new Vector2(GUI_MenuWidth, 36)), string.Empty, EditorStyles.helpBox))
            {
                using (new GUILayout.HorizontalScope())
                {
                    var pauseButton = new GUIContent
                    {
                        image = EditorGUIUtility.FindTexture(_isPaused ? "PlayButton On" : "PauseButton On")
                    };

                    if (GUILayout.Button(pauseButton, GUILayout.Width(30), GUILayout.Height(30)))
                        _isPaused = !_isPaused;

                    EditorGUI.BeginDisabledGroup(_isPaused);

                    if (GUILayout.Button("Cancel", GUILayout.Width(126), GUILayout.Height(30)))
                        _isExecuting = false;

                    EditorGUI.EndDisabledGroup();
                }
            }
        }

        private void SettingsGUI() 
        {
            using (new GUILayout.AreaScope(new Rect(Vector2.zero, new Vector2(GUI_MenuWidth, 148)), string.Empty, EditorStyles.helpBox))
            {
                //AUTO DOCK
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Auto Dock:");
                GUILayout.Space(5);
                
                EditorGUI.BeginChangeCheck();
                _autoDockPosition = (SupportedDockPositions)EditorGUILayout.EnumPopup(_autoDockPosition, GUILayout.Width(80));
                if (EditorGUI.EndChangeCheck())
                {
                    AutoDock(_spriteEditorWindow.position.position);
                    _settingsChanged = true;
                }

                GUILayout.EndHorizontal();

                GUILayout.Space(5);

                //DING VOLUME
                GUILayout.BeginHorizontal();
                GUILayout.Label("Ding Volume:");
                GUILayout.Space(-5);

                EditorGUI.BeginChangeCheck();
                CompletedPopup.DingVolumeLevel = (CompletedPopup.VolumeLevel)EditorGUILayout.EnumPopup(CompletedPopup.DingVolumeLevel, GUILayout.Width(80));
                if (EditorGUI.EndChangeCheck())
                {
                    CompletedPopup.Ding();
                    _settingsChanged = true;
                }

                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                
                //Texture Selection Options
                GUILayout.BeginVertical();
                GUILayout.Label("Texture Selection Options:");
                
                EditorGUI.BeginChangeCheck();
                _textureSelectionOption = (TextureSelectionOptions)EditorGUILayout.EnumPopup(_textureSelectionOption, GUILayout.Width(65));
                if (EditorGUI.EndChangeCheck())
                {
                    switch (_textureSelectionOption)
                    {
                        case TextureSelectionOptions.Default:
                            Selection.activeObject = Selection.activeObject;
                            break;
                        case TextureSelectionOptions.Ping:
                            EditorGUIUtility.PingObject(Selection.activeObject);
                            break;
                    }
                    _settingsChanged = true;
                }
                
                GUILayout.EndVertical();
                GUILayout.Space(5);

                //Hide MSE Banner
                GUILayout.BeginHorizontal();
                GUILayout.Label("Hide MSE Banner:");
                GUILayout.Space(5);

                EditorGUI.BeginChangeCheck();
                _hideMseBanner = EditorGUILayout.Toggle(_hideMseBanner);
                if (EditorGUI.EndChangeCheck())
                    _settingsChanged = true;

                GUILayout.EndHorizontal();

                //CLOSE ON FINISH
                GUILayout.BeginHorizontal();
                GUILayout.Label("Close On Finish:");
                GUILayout.Space(5);

                EditorGUI.BeginChangeCheck();
                _closeOnComplete = EditorGUILayout.Toggle(_closeOnComplete);
                if (EditorGUI.EndChangeCheck())
                    _settingsChanged = true;

                GUILayout.EndHorizontal();
            }

            GUILayout.Space(150);

            EditorGUI.BeginDisabledGroup(!_settingsChanged);
            if (GUILayout.Button("Save", GUILayout.Height(40)))
                SaveSettings();
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(_settingsChanged);
            if (GUILayout.Button("Back", GUILayout.Height(40)))
            {
                switch (_lastModuleIndex)
                {
                    case 0:
                        SwitchMenu(Menu.SPRITE_EDITOR);
                        break;

                    case 1:
                        SwitchMenu(Menu.CUSTOM_OUTLINE);
                        break;

                    case 2:
                        SwitchMenu(Menu.CUSTOM_PHYSICS_SHAPE);
                        break;
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        // EXECUTION LOOPS

        private async void LoopAndSlice()
        {
            // ----- START LOOP -----
            
            if (_moduleReflectionCache == null)
                return;

            var spriteEditorWindowType = _spriteEditorReflectionCache.Types["SpriteEditorWindow"];

            foreach (var texture in _selectedTexture2Ds)
            {
                // If Slicing paused or canceled then revert changes and exit loop
                // - Checking before slice
                if (!_isExecuting)
                {
                    _texture2DsSlicedOrOutlined = 0;
                    Repaint();
                    spriteEditorWindowType.GetMethod("DoRevert", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_spriteEditorWindow, null);
                    spriteEditorWindowType.GetMethod("SetupModule", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_spriteEditorWindow, new object[] { _lastModuleIndex });
                    SwitchMenu(Menu.SPRITE_EDITOR);
                    return;
                }

                // Pause Check
                await MSEEditorUtilities.WaitUntil(() => _isPaused, false);

                // Select Texture
                Selection.activeObject = texture;
                
                var path = AssetDatabase.GetAssetPath(texture);
                var ti = AssetImporter.GetAtPath(path) as TextureImporter;

                if (ti.spriteImportMode != SpriteImportMode.Multiple)
                {
                    ti.spriteImportMode = SpriteImportMode.Multiple;
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                }

                await Task.Delay(DelayBeforeAction);
                Slice();
                await Task.Delay(DelayAfterAction);

                // If Slicing paused or canceled then revert changes and exit loop
                // - Checking after slice and before saving the changes
                if (!_isExecuting)
                {
                    _texture2DsSlicedOrOutlined = 0;
                    Repaint();
                    spriteEditorWindowType.GetMethod("DoRevert", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_spriteEditorWindow, null);
                    spriteEditorWindowType.GetMethod("SetupModule", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_spriteEditorWindow, new object[] { _lastModuleIndex });
                    SwitchMenu(Menu.SPRITE_EDITOR);
                    return;
                }

                try // To check for potential errors
                {
                    spriteEditorWindowType.GetMethod("DoApply", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_spriteEditorWindow, null); 
                    spriteEditorWindowType.GetMethod("SetupModule", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_spriteEditorWindow, new object[] { _lastModuleIndex });
                }
                catch (Exception) // Check documentation for more details on errors
                {
                    Debug.LogError("MSE_ERROR: [SLICING] interrupted, Reopen tool to try again. If any additional errors appear please Reopen project.");
                    return;
                }
                
                // Slice Done/Saved so update progress bars
                _texture2DsSlicedOrOutlined++;
                Repaint();
            }

            // ----- LOOP COMPLETED -----
            
            await Task.Delay(500); 

            _texture2DsSlicedOrOutlined = 0;
            Repaint();
            CompletedPopup.Show(new Vector2(_spriteEditorWindow.position.x + _spriteEditorWindow.position.width * 0.5f - 205 * 0.5f,
                                                   _spriteEditorWindow.position.y + _spriteEditorWindow.position.height * 0.5f - 205 * 0.5f),
                                "Slicing\nComplete!", 1500);

            switch (_textureSelectionOption)
            {
                case TextureSelectionOptions.Default:
                    Selection.activeObject = _selectedTexture2Ds[_selectedTexture2Ds.Length - 1];
                    break;
                case TextureSelectionOptions.Ping:
                    Selection.objects = _selectedTexture2Ds;
                    break;
            }
            
            SwitchMenu(Menu.SPRITE_EDITOR);
        }

        private async void LoopAndOutline()
        {
            // ----- START LOOP -----
            
            if (_moduleReflectionCache == null)
                return;
            
            var spriteEditorWindowType = _spriteEditorReflectionCache.Types["SpriteEditorWindow"];

            foreach (var texture in _selectedTexture2Ds)
            {
                // If Outlining paused or canceled then revert changes and exit loop
                // - Checking before Outline
                if (!_isExecuting)
                {
                    _texture2DsSlicedOrOutlined = 0;
                    _spritesOutlined = 0;
                    Repaint();
                    spriteEditorWindowType.GetMethod("DoRevert", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_spriteEditorWindow, null);
                    spriteEditorWindowType.GetMethod("SetupModule", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_spriteEditorWindow, new object[] { _lastModuleIndex });
                    SwitchMenu(_lastModuleIndex == 1 ? Menu.CUSTOM_OUTLINE : Menu.CUSTOM_PHYSICS_SHAPE);
                    return;
                }

                // Pause Check
                await MSEEditorUtilities.WaitUntil(() => _isPaused, false);

                // Select Texture
                Selection.activeObject = texture;

                // Small delay to make sure new texture is fully selected
                await Task.Delay(50);

                // Need to an updated reference to the rectsCache
                var rectsCache = _spriteEditorReflectionCache.Fields["m_RectsCache"].GetValue(_spriteEditorWindow) as IList<SpriteRect>;
                _rectsCount = rectsCache.Count;

                if (_rectsCount <= 0)
                    return; 

                // Loop through all rects/sprites in each texture
                for (int j = 0; j < rectsCache.Count; j++)
                {
                    // Checking before outlining current rect/sprite
                    if (!_isExecuting)
                    {
                        _texture2DsSlicedOrOutlined = 0;
                        _spritesOutlined = 0;
                        Repaint();
                        spriteEditorWindowType.GetMethod("DoRevert", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_spriteEditorWindow, null);
                        spriteEditorWindowType.GetMethod("SetupModule", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_spriteEditorWindow, new object[] { _lastModuleIndex });
                        SwitchMenu(_lastModuleIndex == 1 ? Menu.CUSTOM_OUTLINE : Menu.CUSTOM_PHYSICS_SHAPE);
                        return;
                    }

                    // Pause Check
                    await MSEEditorUtilities.WaitUntil(() => _isPaused, false);

                    // Select rect/sprite
                    spriteEditorWindowType.GetProperty("selectedSpriteRect", BindingFlags.Instance | BindingFlags.Public).SetValue(_spriteEditorWindow, rectsCache[j]);
                    
                    // Outline
                    await Task.Delay(DelayBeforeAction);
                    Outline(rectsCache[j], j);
                    
                    // Update the progress bar
                    _spritesOutlined++;
                    Repaint();
                    
                    await Task.Delay(DelayAfterAction);
                }

                // If Outlining paused or canceled then revert changes and exit loop
                // - Checking after outline and before saving the changes
                if (!_isExecuting)
                {
                    _texture2DsSlicedOrOutlined = 0;
                    _spritesOutlined = 0;
                    Repaint();
                    spriteEditorWindowType.GetMethod("DoRevert", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_spriteEditorWindow, null);
                    spriteEditorWindowType.GetMethod("SetupModule", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_spriteEditorWindow, new object[] { _lastModuleIndex });
                    SwitchMenu(_lastModuleIndex == 1 ? Menu.CUSTOM_OUTLINE : Menu.CUSTOM_PHYSICS_SHAPE);
                    return;
                }

                await MSEEditorUtilities.WaitUntil(() => _isPaused, false);

                try // To check for potential errors
                {
                    spriteEditorWindowType.GetMethod("DoApply", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_spriteEditorWindow, null); 
                    spriteEditorWindowType.GetMethod("SetupModule", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_spriteEditorWindow, new object[] { _lastModuleIndex });
                }
                catch (Exception) // Check documentation for more details on errors
                {
                    Debug.LogError("MSE_ERROR: [OUTLINING] interrupted, Reopen tool to try again. If any additional errors appear please Reopen project.");
                    return;
                }
                
                // Outline Done/Saved so update progress bars
                _texture2DsSlicedOrOutlined++;
                _spritesOutlined = 0;
                Repaint();
            }
            
            // ----- LOOP COMPLETED -----

            await Task.Delay(500);

            _texture2DsSlicedOrOutlined = 0;
            Repaint();
            CompletedPopup.Show(new Vector2(_spriteEditorWindow.position.x + _spriteEditorWindow.position.width * 0.5f - 205 * 0.5f,
                                                   _spriteEditorWindow.position.y + _spriteEditorWindow.position.height * 0.5f - 205 * 0.5f),
                                "Outlining\nComplete!", 1500);
            
            switch (_textureSelectionOption)
            {
                case TextureSelectionOptions.Default:
                    Selection.activeObject = _selectedTexture2Ds[_selectedTexture2Ds.Length - 1];
                    break;
                case TextureSelectionOptions.Ping:
                    Selection.objects = _selectedTexture2Ds;
                    break;
            }
            
            SwitchMenu(_lastModuleIndex == 1 ? Menu.CUSTOM_OUTLINE : Menu.CUSTOM_PHYSICS_SHAPE);
        }

        //EXECUTES

        private void Slice()
        {
            if (_moduleReflectionCache == null)
                return;

            var spriteEditorMenuObj = CreateInstance(_moduleReflectionCache.Types["SpriteEditorMenu"]); // BEGIN

            var TDP = _moduleReflectionCache.Types["moduleInstance"].GetField("m_TextureDataProvider", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_moduleReflectionCache.Objects["moduleInstance"]);

            _moduleReflectionCache.Types["SpriteEditorMenu"].GetField("m_SpriteFrameModule", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic).SetValue(spriteEditorMenuObj, _moduleReflectionCache.Objects["moduleInstance"]);
            _moduleReflectionCache.Types["SpriteEditorMenu"].GetField("m_TextureDataProvider", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic).SetValue(spriteEditorMenuObj, TDP);

            var setting = _moduleReflectionCache.Types["SpriteEditorMenu"].GetField("s_Setting", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
            var settingType = typeof(UnityEditor.U2D.Sprites.ISpriteEditor).Assembly.GetType("UnityEditor.U2D.Sprites.SpriteEditorMenuSetting");

            if (setting.GetValue(spriteEditorMenuObj) == null)
                setting.SetValue(spriteEditorMenuObj, CreateInstance(settingType));

            //ACTION

            try
            {
                _moduleReflectionCache.Methods["DoSlicing"].Invoke(spriteEditorMenuObj, null);
            }
            catch (Exception)
            {
                Debug.LogError("MSE_ERROR: [SLICING] interrupted, Reopen tool to try again. If any additional errors appear please Reopen project.");
                return;
            }

            //ACTION

            DestroyImmediate(spriteEditorMenuObj); // END
        }

        private void Outline(SpriteRect spriteRect, int indexInCachedRects)
        {
            if (_moduleReflectionCache == null)
                return;

            try
            {
                _moduleReflectionCache.Fields["m_Selected"].SetValue(_moduleReflectionCache.Objects["moduleInstance"], spriteRect);

                var outline = _moduleReflectionCache.Types["moduleInstance"].GetField("m_Outline", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(_moduleReflectionCache.Objects["moduleInstance"]);
                var outlineType = outline.GetType();

                var list = outlineType.GetField("m_SpriteOutlineList", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(outline) as IList;
                var spriteOutline = list[indexInCachedRects];

                spriteOutline.GetType().GetField("m_TessellationDetail", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(spriteOutline, _outlineTesselationValue);

                var shapeOutlineList = _moduleReflectionCache.Properties["selectedShapeOutline"].GetValue(_moduleReflectionCache.Objects["moduleInstance"]) as IList;

                shapeOutlineList.Clear();

                _moduleReflectionCache.Methods["SetupShapeEditorOutline"].Invoke(_moduleReflectionCache.Objects["moduleInstance"], new[] { spriteRect });
                _moduleReflectionCache.Methods["SetDataModified"].Invoke(_spriteEditorWindow, null);
                _moduleReflectionCache.Properties["shapeEditorDirty"].SetValue(_moduleReflectionCache.Objects["moduleInstance"], true);
            }
            catch (Exception)
            {
                Debug.LogError("MSE_ERROR: [OUTLINING] interrupted, If any additional errors appear please reopen project.");
                return;
            }
        }

        // UTILITY

        private async void SwitchMenu(Menu menuType)
        {
            if (_currentlyDisplayedMenu != Menu.UNSUPPORTED)
            {
                _dropdownAnim.target = false; //Move Up
                await MSEEditorUtilities.WaitUntil(() => (_dropdownAnim.faded < 0.01f), true);
            }

            switch (menuType)
            {
                case Menu.SPRITE_EDITOR:
                    if (_closeOnComplete && _isExecuting)
                    {
                        _spriteEditorWindow.Close();
                        this.Close();
                    }

                    _isExecuting = false;
                    _isPaused = false;
                    break;

                case Menu.SPRITE_EDITOR_EXECUTE:
                    _isExecuting = true;
                    LoopAndSlice();
                    break;

                case Menu.CUSTOM_OUTLINE:
                case Menu.CUSTOM_PHYSICS_SHAPE:
                    if (_closeOnComplete && _isExecuting)
                    {
                        _spriteEditorWindow.Close();
                        this.Close();
                    }

                    _isExecuting = false;
                    _isPaused = false;
                    break;

                case Menu.CUSTOM_OUTLINE_EXECUTE:
                case Menu.CUSTOM_PHYSICS_SHAPE_EXECUTE:
                    _isExecuting = true;
                    LoopAndOutline();
                    break;
            }

            _currentlyDisplayedMenu = menuType;

            _dropdownAnim.target = true; // Move Down
            await MSEEditorUtilities.WaitUntil(() => (_dropdownAnim.faded > 0.8f), true);
        }

        private async void AutoDock(Vector2 windowPosition)
        {
            switch (_autoDockPosition)
            {
                case SupportedDockPositions.Left:
#if UNITY_2020_1_OR_NEWER
                    if (this.position.position.x < _spriteEditorWindow.position.position.x + _spriteEditorWindow.position.width / 2 && this.docked)
                        return;
#else
                    if (this.position.position.x < _spriteEditorWindow.position.position.x + _spriteEditorWindow.position.width / 2 && IsDocked)
                        return;
#endif
                    break;
                case SupportedDockPositions.Right:
#if UNITY_2020_1_OR_NEWER
                    if (this.position.position.x > _spriteEditorWindow.position.position.x + _spriteEditorWindow.position.width / 2 && this.docked)
                        return;
#else
                    if (this.position.position.x > _spriteEditorWindow.position.position.x + _spriteEditorWindow.position.width / 2 && IsDocked)
                        return;
#endif
                    break;
                case SupportedDockPositions.None:
                    return;
            }

            //Delay to not dock to fast otherwise MSE isn't ready
            await Task.Delay(5);
            this.DockTo(_spriteEditorWindow, windowPosition, _autoDockPosition == SupportedDockPositions.Left ? Docker.DockPosition.Left : Docker.DockPosition.Right);
        }

        private string UpdateMenuDisplayText(Menu menuType)
        {
            switch (menuType)
            {
                case Menu.SPRITE_EDITOR:
                    return _dropdownAnim.target == true ? "Sprite Editor" : "Switching...";

                case Menu.SPRITE_EDITOR_EXECUTE:
                    return _dropdownAnim.target == true ? "Slicing..." : "Switching...";

                case Menu.CUSTOM_OUTLINE:
                    return _dropdownAnim.target == true ? "Custom Outline" : "Switching...";

                case Menu.CUSTOM_OUTLINE_EXECUTE:
                case Menu.CUSTOM_PHYSICS_SHAPE_EXECUTE:
                    return _dropdownAnim.target == true ? "Outlining..." : "Switching...";

                case Menu.CUSTOM_PHYSICS_SHAPE:
                    return _dropdownAnim.target == true ? "Custom Physics Shape" : "Switching...";

                case Menu.SETTINGS:
                    return _dropdownAnim.target == true ? "Settings" : "Switching...";

                case Menu.UNSUPPORTED:
                    return "Unsupported";
            }

            return string.Empty;
        }
    }
}