using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//One copy of each main screen and both maps, built while the loading screen is up and kept for the whole loaded game.
//They sit switched off under a holder that scene changes don't unload, and are moved under the live UI parent to be shown.
public static class PrebuiltScreenManager
{
    private const string holderName = "Prebuilt Screens";

    //one a frame, in this order. A screen left out of this list would be instantiated each time it is opened, as they all used to be
    private static readonly ScreenType[] buildOrder =
    {
        ScreenType.Settings,
        ScreenType.SaveAndLoad,
        ScreenType.Journal,
        ScreenType.Character,
        ScreenType.Inventory,
        ScreenType.Party
    };

    //a row of the map at a time, so no one frame of the loading screen has all sixty-four tiles to make
    private const int mapTilesPerFrame = MapFormatList.mapDimensions;

    private static Transform holder;
    private static Dictionary<ScreenType, ScreenManager> screens = new Dictionary<ScreenType, ScreenManager>();

    private static MapPopUpWindow mapWindow;
    private static WorldMapPopUpWindow worldMapWindow;

    //the loading screen holds its prompt until this is true, so it is set on every way out of build
    public static bool buildFinished { get; private set; } = true;

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        holder = null;
        screens = new Dictionary<ScreenType, ScreenManager>();
        mapWindow = null;
        worldMapWindow = null;
        buildFinished = true;

        //the scene changes that start a load discard the screens already. This is for a load that reaches the reset some other way
        LoadSaveFile.OnLoadResetData.AddListener(discardAll);

        SceneManager.sceneLoaded -= reattachMapWindow;
        SceneManager.sceneLoaded += reattachMapWindow;
    }

    //Coming back from combat, the overworld's UI is loaded again and the map window is still parked. A window that is hidden by
    //its canvas has to be switched on to be ready, which is the slow part, so it is done here inside the scene load
    //and not left for the first time the map is opened
    private static void reattachMapWindow(Scene scene, LoadSceneMode mode)
    {
        if (mapWindow == null || CombatStateManager.inCombat || !scene.name.Equals(SceneNameList.OOCUserInterface))
        {
            return;
        }

        mapWindow.attachHidden(PopUpScreenBlockerManager.getPopUpParent());
    }

    //one screen a frame, so the loading screen keeps moving while they are made
    public static IEnumerator build()
    {
        buildFinished = false;

        try
        {
            discardAll();

            Transform liveParent = PopUpScreenBlockerManager.getPopUpParent();

            //no pop-up parent means the overworld's UI scene isn't loaded, and there is nowhere real to build under.
            //Every screen is then instantiated when it is opened, as it always was
            if (liveParent == null)
            {
                yield break;
            }

            foreach (ScreenType screenType in buildOrder)
            {
                buildScreen(screenType, liveParent);

                yield return null;
            }

            buildMapWindow(liveParent);

            yield return null;

            while (!mapTilesFinished())
            {
                yield return null;
            }

            finishMapWindow(liveParent);

            buildWorldMapWindow(liveParent);

            yield return null;

            //GC is manual in builds, so nothing else would clear what the build threw away
            GC.Collect();
        }
        finally
        {
            buildFinished = true;
        }
    }

    //built under the live pop-up parent rather than the holder, so that anything read from the canvas in Awake is real,
    //then hidden before the frame is drawn
    private static void buildScreen(ScreenType screenType, Transform liveParent)
    {
        GameObject screenObject = null;

        try
        {
            screenObject = GameObject.Instantiate(Resources.Load<GameObject>(OverallUIManager.getScreenPrefabName(screenType)), liveParent);

            ScreenManager screen = screenObject.GetComponent<ScreenManager>();

            screen.markAsPrebuilt();

            //the same tidying as any other time it is hidden, since some screens fill a grid as they are made
            screen.hide(getHolder());

            screens[screenType] = screen;
        }
        catch (Exception exception)
        {
            //a screen that couldn't be built is left out, and is instantiated when it is opened instead
            Debug.LogException(exception);

            if (screenObject != null)
            {
                GameObject.Destroy(screenObject);
            }
        }

        //the screen's Awake named it the current screen, which everything else reads as "the UI is open"
        OverallUIManager.removeCurrentScreenType();
    }

    private static void buildMapWindow(Transform liveParent)
    {
        GameObject windowObject = null;

        try
        {
            windowObject = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.mapPopUpWindow), liveParent);

            mapWindow = windowObject.GetComponent<MapPopUpWindow>();

            mapWindow.markAsPrebuilt();
        }
        catch (Exception exception)
        {
            //a map that couldn't be built is left out, and is instantiated when it is opened instead
            Debug.LogException(exception);

            mapWindow = null;

            if (windowObject != null)
            {
                GameObject.Destroy(windowObject);
            }
        }
    }

    //makes the next row of tiles, and says whether there are none left to make
    private static bool mapTilesFinished()
    {
        if (mapWindow == null)
        {
            return true;
        }

        try
        {
            return mapWindow.createTiles(mapTilesPerFrame);
        }
        catch (Exception exception)
        {
            //the window is kept with the tiles it has. Any that are missing are made when the map is first opened
            Debug.LogException(exception);

            return true;
        }
    }

    //the window stays where it was built if it is hidden by its canvas, and goes to the holder if it is hidden by being switched off
    private static void finishMapWindow(Transform liveParent)
    {
        if (mapWindow == null)
        {
            return;
        }

        mapWindow.hide(getHolder());
        mapWindow.attachHidden(liveParent);
    }

    private static void buildWorldMapWindow(Transform liveParent)
    {
        GameObject windowObject = null;

        try
        {
            windowObject = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.worldMapPopUpWindow), liveParent);

            worldMapWindow = windowObject.GetComponent<WorldMapPopUpWindow>();

            worldMapWindow.markAsPrebuilt();
            worldMapWindow.hide(getHolder());
        }
        catch (Exception exception)
        {
            //a world map that couldn't be built is left out, and is instantiated when it is opened instead
            Debug.LogException(exception);

            worldMapWindow = null;

            if (windowObject != null)
            {
                GameObject.Destroy(windowObject);
            }
        }
    }

    //null when there is no prebuilt map, and the caller instantiates one as it always did
    public static MapPopUpWindow getMapWindow()
    {
        return mapWindow;
    }

    public static WorldMapPopUpWindow getWorldMapWindow()
    {
        return worldMapWindow;
    }

    public static void hideMapWindow()
    {
        mapWindow.hide(getHolder());
    }

    public static void hideWorldMapWindow()
    {
        worldMapWindow.hide(getHolder());
    }

    private static Transform getHolder()
    {
        if (holder == null)
        {
            GameObject holderObject = new GameObject(holderName, typeof(RectTransform));

            GameObject.DontDestroyOnLoad(holderObject);

            holder = holderObject.transform;
        }

        return holder;
    }

    //null when there is no prebuilt copy to show, and the caller instantiates the screen as it always did
    public static ScreenManager show(ScreenType screenType, Transform parent)
    {
        if (parent == null || !screens.TryGetValue(screenType, out ScreenManager screen) || screen == null)
        {
            return null;
        }

        screen.show(parent);

        return screen;
    }

    public static void hide(ScreenManager screen)
    {
        screen.hide(getHolder());
    }

    //called just before a scene is unloaded. A screen still sitting under that scene's UI would be destroyed with it
    public static void parkAll()
    {
        foreach (ScreenManager screen in screens.Values)
        {
            if (screen != null && screen.transform.parent != holder)
            {
                screen.hide(getHolder());
            }
        }

        if (mapWindow != null && mapWindow.transform.parent != holder)
        {
            mapWindow.park(getHolder());
        }

        if (worldMapWindow != null && worldMapWindow.transform.parent != holder)
        {
            worldMapWindow.hide(getHolder());
        }
    }

    //the screens belong to one loaded game. They go when it does, and the next load builds new ones
    public static void discardAll()
    {
        foreach (ScreenManager screen in screens.Values)
        {
            if (screen != null)
            {
                GameObject.Destroy(screen.gameObject);
            }
        }

        screens = new Dictionary<ScreenType, ScreenManager>();

        if (mapWindow != null)
        {
            GameObject.Destroy(mapWindow.gameObject);
        }

        if (worldMapWindow != null)
        {
            GameObject.Destroy(worldMapWindow.gameObject);
        }

        mapWindow = null;
        worldMapWindow = null;
    }
}
