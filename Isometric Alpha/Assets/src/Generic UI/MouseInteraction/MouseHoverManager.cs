using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IHoverIconSource
{
    public void spawnHoverIcon();
    public void destroyHoverIcon();

    public GameObject getDescriptionPanelType();
    public IDescribable getObjectBeingDescribed();
}

public static class MouseHoverManager
{
    public readonly static UnityEvent OnHoverPanelCreation = new UnityEvent();

    public const bool shouldSpawnHoverIcon = false;
    public const bool shouldDestroyHoverIcon = true;

    public static GameObject mouseHoverBase;

    public static MonoBehaviour coroutineParent;
    public static Coroutine hoverCoroutine;

    public static GameObject hoverDescriptionPanelObject;
    public static DescriptionPanelSlot hoverDescriptionPanelSlot;

    public static void startCoroutine(MonoBehaviour parent, IEnumerator coroutine)
    {
        stopCoroutine();

        if (parent != null && !(parent is null))
        {
            coroutineParent = parent;
            hoverCoroutine = coroutineParent.StartCoroutine(coroutine);
        }
    }

    public static void stopCoroutine()
    {
        if (coroutineParent != null && !(coroutineParent is null) && hoverCoroutine != null)
        {
            coroutineParent.StopCoroutine(hoverCoroutine);
        }
    }


    public static GameObject getMouseHoverBase()
    {
        return createBase(MouseHoverParentCanvas.getMouseHoverParent());
    }

    public static GameObject getDragAndDropBase()
    {
        return createBase(DragAndDropParentDeclarer.dragAndDropParent);
    }

    private static GameObject createBase(Transform parent)
    {
        if (mouseHoverBase != null)
        {
            return mouseHoverBase;
        }
        else
        {
            mouseHoverBase = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.mouseHoverBase), parent);

            mouseHoverBase.GetComponent<MouseHoverBase>().Update();

            mouseHoverBase.SetActive(true);

            return mouseHoverBase;
        }
    }

    public static Transform getHoverObjectParent()
    {
        if (mouseHoverBase != null)
        {
            return mouseHoverBase.transform;
        }
        else
        {
            return null;
        }
    }

    public static void createHoverTag(string text)
    {
        if (getHoverObjectParent() != null)
        {
            MouseHoverTag tag = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.mouseHoverTag), getHoverObjectParent()).GetComponent<MouseHoverTag>();
            tag.fillOutTag(text);
        }
    }

    public static void createHoverDescBlockPanel(IDescribableInBlocks blockSource)
    {
        if (getHoverObjectParent() != null)
        {
            DescriptionPanelBuilder blockBuilder = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.hoverDescriptionPanelBuilder), getHoverObjectParent()).GetComponent<DescriptionPanelBuilder>();

            blockBuilder.buildDescriptionPanel(blockSource);
        }
    }

    public static void destroyMouseHoverBase()
    {
        if (mouseHoverBase != null)
        {
            GameObject.DestroyImmediate(mouseHoverBase);
        }
    }

    public static IEnumerator waitToHandleDescriptionPanel(IHoverIconSource source, bool destroy)
    {
        float timeWaited = 0f;
        float timeToWait = DragAndDropManager.timeToWait;

        if (destroy)
        {
            timeToWait = 0f;
        }

        while (timeWaited < timeToWait)
        {
            if (Input.GetKey(KeyCode.Mouse0) && !destroy)
            {
                timeWaited = 0f;
            }

            yield return null;

            timeWaited += Time.deltaTime;
        }

        if (destroy)
        {
            source.destroyHoverIcon();
        }
        else
        {
            source.spawnHoverIcon();
        }
    }

    public static void spawnHoverIcon(IHoverIconSource source, Transform parent, bool setToWorldScale = false)
    {
        if (source.getObjectBeingDescribed() == null)
        {
            return;
        }

        OnHoverPanelCreation.Invoke();

        hoverDescriptionPanelObject = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.hoverIconCombatActionDescriptionPanel), parent);
        hoverDescriptionPanelObject.AddComponent<HoverPanelCreationListener>();

        hoverDescriptionPanelSlot = hoverDescriptionPanelObject.GetComponent<DescriptionPanelSlot>();

        hoverDescriptionPanelSlot.setPrimaryDescribable(source.getObjectBeingDescribed());
        hoverDescriptionPanelObject.SetActive(true);

        if(setToWorldScale)
        {
            hoverDescriptionPanelObject.transform.localScale = new Vector3(.007f, .007f);
        }
    }

    public static void spawnCustomHover(IHoverIconSource source, Transform parent, string prefabName)
    {
        if (source.getObjectBeingDescribed() == null)
        {
            return;
        }

        OnHoverPanelCreation.Invoke();

        hoverDescriptionPanelObject = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.hoverIconCombatActionDescriptionPanel), parent);
        hoverDescriptionPanelObject.AddComponent<HoverPanelCreationListener>();

        hoverDescriptionPanelSlot = hoverDescriptionPanelObject.GetComponent<DescriptionPanelSlot>();

        Transform customPrefabParent = hoverDescriptionPanelObject.GetComponent<DescriptionPanelSlot>().descriptionPanelParent;
        GameObject customObject = GameObject.Instantiate(Resources.Load<GameObject>(prefabName), customPrefabParent);

        DescriptionPanelBuilder builder = customObject.GetComponent<DescriptionPanelBuilder>();

        if(builder != null && hoverDescriptionPanelSlot != null)
        {
            hoverDescriptionPanelSlot.prebuiltBuilder = builder;

            if(CombatStateManager.inCombat)
            {
                hoverDescriptionPanelSlot.formatType = BlockFormatType.CombatHover;
            } else
            {
                hoverDescriptionPanelSlot.formatType = BlockFormatType.PlayerStats;
            }

            hoverDescriptionPanelSlot.setPrimaryDescribable(source.getObjectBeingDescribed());
        }

        customObject.SetActive(true);
    }

    public static void spawnQuestListHover(IQuestListSource questListSource, Transform parent)
    {
        OnHoverPanelCreation.Invoke();

        hoverDescriptionPanelObject = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.hoverIconCombatActionDescriptionPanel), parent);
        hoverDescriptionPanelObject.AddComponent<HoverPanelCreationListener>();

        HoverIconDescriptionPanel hoverIconDescriptionPanel = hoverDescriptionPanelObject.GetComponent<HoverIconDescriptionPanel>();

        GameObject questListGrid = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.questListDropDownGrid), hoverIconDescriptionPanel.descPanelWindow);

        ScrollableUIElement grid = questListGrid.GetComponentInChildren<ScrollableUIElement>();

        grid.populatePanels(questListSource.getListOfQuestStepsForDisplay());

        questListGrid.SetActive(true);
    }

    public static void destroyHoverIcon()
    {
        if(InspectNode.inspecting)
        {
            OnHoverPanelCreation.Invoke();
            return;
        }

        if (hoverDescriptionPanelObject != null)
        {
            GameObject.DestroyImmediate(hoverDescriptionPanelObject);
        }

        hoverDescriptionPanelSlot = null;
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeMouseHoverManager()
    {
        PlayerOOCStateManager.OnStateChange.AddListener(destroyHoverIcon);

        mouseHoverBase = null;
        coroutineParent = null;
        hoverCoroutine = null;
        hoverDescriptionPanelObject = null;
        hoverDescriptionPanelSlot = null;
    }

}