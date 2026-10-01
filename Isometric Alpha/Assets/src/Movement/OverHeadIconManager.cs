using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

//lets a name source hide its overhead name tag and outline, such as a chest that has already been opened
public interface INameTagSuppressor
{
    public bool suppressNameTag { get; }
}

public class OverHeadIconManager : MonoBehaviour, IRevealable
{
    private const float twentyFivePercentMultiplier = 1.25f;

    public Canvas canvas;
    public Transform iconParent;

    [SerializeField]
    private SpriteLayerRendererList _RendererList;
    public SpriteLayerRendererList rendererList
    {
        get
        {
            return _RendererList;
        }
        set
        {
            _RendererList = value;
        }
    }

    public bool ignoreHover;
    public bool noNameTag = false;

    public INameSource nameSource;

    //only used by the objects that have no icon parent to hang an overhead name tag from
    public DescriptionPanel nameTag;

    private bool ignoreSecretDoors;

    private Dictionary<OverHeadIconType, GameObject> icons = new Dictionary<OverHeadIconType, GameObject>();

    private void Awake()
    {
        createListeners();
    }

    private void OnDestroy()
    {
        destroyListeners();
    }

    private void OnEnable()
    {
        PlayerStateManager.OnStateChangeToInDialogue.AddListener(disableCanvas);
        PlayerStateManager.OnStateChangeFromInDialogue.AddListener(enableCanvas);

        if(rendererList != null)
        {
            rendererList.registerBehaviour(SpriteLayer.Body, this, () => onSpriteHeightChange());
        }
    }

    private void OnDisable()
    {
        PlayerStateManager.OnStateChangeToInDialogue.RemoveListener(disableCanvas);
        PlayerStateManager.OnStateChangeFromInDialogue.RemoveListener(enableCanvas);

        if(rendererList != null)
        {
            rendererList.unregisterComponent(SpriteLayer.Body, this);
        }
    }

    private void onSpriteHeightChange()
    {
        if(rendererList != null && iconParent != null)
        {
            iconParent.position = SpriteUtil.getTopOfBounds(rendererList[SpriteLayer.Body], twentyFivePercentMultiplier);
        }
    }

    //for managers spawned at runtime, where OnEnable has already run by the time the renderer list is known
    public void setRendererList(SpriteLayerRendererList list)
    {
        if(rendererList != null)
        {
            rendererList.unregisterComponent(SpriteLayer.Body, this);
        }

        rendererList = list;

        if(rendererList == null)
        {
            return;
        }

        rendererList.registerBehaviour(SpriteLayer.Body, this, () => onSpriteHeightChange());

        onSpriteHeightChange();
    }

    private void disableCanvas()
    {
        if(canvas != null)
        {
            canvas.enabled = false;
        }
    }

    private void enableCanvas()
    {
        if(canvas != null)
        {
            canvas.enabled = true;
        }
    }

    #region IRevealable

    public void createListeners()
    {
        RevealManager.OnReveal.AddListener(onReveal);

        PlayerStateManager.OnStateChangeFromWalking.AddListener(displayNameTagBasedOnStateChange);
        PlayerStateManager.OnStateChangeToWalking.AddListener(displayNameTagBasedOnStateChange);

        PlayerStateManager.OnStateChangeFromWalking.AddListener(this.revealBasedOnStateChange);
        PlayerStateManager.OnStateChangeToWalking.AddListener(this.revealBasedOnStateChange);

        if(!ignoreSecretDoors)
        {
            SecretDoorFlags.OnSecretDoorDiscovery.AddListener(checkSpawnParams);
        }
    }

    public void destroyListeners()
    {
        RevealManager.OnReveal.RemoveListener(onReveal);
        SecretDoorFlags.OnSecretDoorDiscovery.RemoveListener(checkSpawnParams);

        PlayerStateManager.OnStateChangeFromWalking.RemoveListener(displayNameTagBasedOnStateChange);
        PlayerStateManager.OnStateChangeToWalking.RemoveListener(displayNameTagBasedOnStateChange);

        PlayerStateManager.OnStateChangeFromWalking.RemoveListener(this.revealBasedOnStateChange);
        PlayerStateManager.OnStateChangeToWalking.RemoveListener(this.revealBasedOnStateChange);
    }

    public void onReveal(bool toggleReveal)
    {
        if(rendererList == null)
        {
            return;
        }

        if(toggleReveal && !nameTagSuppressed())
        {
            rendererList.createOutline(getRevealColor());

            if(!this.hasGenericName())
            {
                spawnNameTag();
            }

        } else
        {
            rendererList.removeOutline();

            destroyNameTag();
        }
    }

    public virtual Color getRevealColor()
    {
        return ColorList.canBeInteractedWith;
    }

    private bool nameTagSuppressed()
    {
        INameTagSuppressor suppressor = nameSource as INameTagSuppressor;

        return suppressor != null && suppressor.suppressNameTag;
    }

    public void createHoverTag()
    {
        //Empty on purpose (may add for things like portcullis controls in mine lvl 2)
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(nameTagSuppressed())
        {
            return;
        }

        if (!ignoreHover && (eventData == null || !eventData.used))
        {
            if (eventData != null)
            {
                eventData.Use();
            }

            if(!RevealManager.currentlyRevealed && rendererList != null)
            {
                rendererList.createOutline(getRevealColor());
            }

            PlayerObject.toggleButtonPrompt(false);
            spawnNameTag();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerObject.restoreButtonPrompt();

        if (!ignoreHover)
        {
            if(!RevealManager.currentlyRevealed && rendererList != null)
            {
                rendererList.removeOutline();
            }

            if(this.hasGenericName() || !RevealManager.currentlyRevealed)
            {
                destroyNameTag();
            }
        }
    }

    public string displayName
    {
        get
        {
            if(nameSource == null)
            {
                return Constants.emptyString;
            }

            return nameSource.displayName;
        }
    }

    public string uniqueName
    {
        get
        {
            if(nameSource == null)
            {
                return Constants.emptyString;
            }

            return nameSource.uniqueName;
        }
    }

    #endregion

    #region SpawnParams

    public virtual void checkSpawnParams(string secretDoorFlag)
    {
        if(nameSource == null)
        {
            return;
        }

        if(!SpawnParamsList.getSpawnParams(AreaManager.locationName, nameSource.uniqueName).canSpawn(nameSource.uniqueName))
        {
            gameObject.SetActive(false);
        } else
        {
            gameObject.SetActive(true);
        }
    }

    public void setToIgnoreSecretDoors()
    {
        ignoreSecretDoors = true;
        SecretDoorFlags.OnSecretDoorDiscovery.RemoveListener(checkSpawnParams);
    }

    #endregion

    #region NameTag

    public void spawnNameTag()
    {
        if(!gameObject.activeInHierarchy)
        {
            destroyNameTag();
            return;
        }

        if(noNameTag)
        {
            return;
        }

        if(iconParent != null)
        {
            createOverHeadIcon(OverHeadIconType.NameTag, nameOfNPC: displayName);
            return;
        }

        spawnStandaloneNameTag();
    }

    //fallback for objects that have no icon parent to hang an overhead name tag from
    private void spawnStandaloneNameTag()
    {
        if(nameTag != null)
        {
            return;
        }

        if(gameObject.GetComponent<RectTransform>() == null)
        {
            gameObject.AddComponent<RectTransform>();
        }

        nameTag = Instantiate(Resources.Load<GameObject>(PrefabNames.npcNameTag), transform).GetComponent<DescriptionPanel>();

        nameTag.nameText.text = displayName;

        if(PlayerStateManager.currentActivity == CurrentActivity.InWorldMap)
        {
            Canvas nameTagCanvas = nameTag.GetComponent<Canvas>();

            if(nameTagCanvas != null)
            {
                nameTagCanvas.overrideSorting = true;
                nameTagCanvas.sortingLayerName = LayerAndTagManager.mapSortingLayerName;
                nameTagCanvas.sortingOrder = Constants.indexTwelve;
            }
        }
    }

    public void destroyNameTag()
    {
        if (nameTag != null)
        {
            Destroy(nameTag.gameObject);
            nameTag = null;
        }

        destroyIcon(OverHeadIconType.NameTag);
    }

    public void displayNameTagBasedOnStateChange()
    {
        if(PlayerStateManager.currentActivity == CurrentActivity.Walking &&
            RevealManager.currentlyRevealed &&
            !this.hasGenericName() &&
            !nameTagSuppressed())
        {
            spawnNameTag();
        }
    }

    #endregion

    #region Icons

    public void createOverHeadIcon(OverHeadIconType iconType, IOverHeadIconSource source = null, string nameOfNPC = null)
    {
        if(icons.ContainsKey(iconType))
        {
            return;
        }

        switch(iconType)
        {
            case OverHeadIconType.NameTag:

                if(hoveringOverIcon())
                {
                    return;
                }

                NPCNameTag overHeadNameTag = Instantiate(Resources.Load<GameObject>(PrefabNames.overHeadNameTag), iconParent).GetComponent<NPCNameTag>();

                overHeadNameTag.labelNPC(nameOfNPC);
                overHeadNameTag.transform.SetAsFirstSibling();

                if(iconParent.childCount > 1)
                {
                    overHeadNameTag.orientTransformSide();
                } else
                {
                    overHeadNameTag.orientTransformMiddle();
                }

                icons[iconType] = overHeadNameTag.gameObject;
                return;
            default:
                OverHeadIcon icon = Instantiate(Resources.Load<GameObject>(PrefabNames.overHeadIcon), iconParent).GetComponent<OverHeadIcon>();
                icon.source = source;
                icon.canvas = canvas;

                icon.setDisplay(iconType);
                icon.iconManager = this;

                icons[iconType] = icon.gameObject;

                if(icons.ContainsKey(OverHeadIconType.NameTag))
                {
                    icons[OverHeadIconType.NameTag].GetComponent<NPCNameTag>().orientTransformSide();
                }
                return;
        }
    }

    public bool hasIcon(OverHeadIconType type)
    {
        return icons.ContainsKey(type);
    }

    public bool hoveringOverIcon()
    {
        foreach(GameObject gameObject in icons.Values)
        {
            OverHeadIcon icon = gameObject.GetComponent<OverHeadIcon>();

            if(icon != null && icon.hovered)
            {
                return true;
            }
        }

        return false;
    }

    public void removeAllDestroyedIcons()
    {
        List<OverHeadIconType> iconKeys = new List<OverHeadIconType>(icons.Keys);

        foreach(OverHeadIconType key in iconKeys)
        {
            if(icons[key] == null)
            {
                icons.Remove(key);
            }
        }
    }

    public void destroyIcon(OverHeadIconType iconType)
    {
        if(icons.ContainsKey(iconType))
        {
            DestroyImmediate(icons[iconType]);
            removeAllDestroyedIcons();
        }
    }

    #endregion
}
