using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum CunningObjectSpriteCategory { Crank = 0 }

public abstract class CunningObject : MonoBehaviour, ISkillTarget, IRevealable, INameSource
{
    public const string tagText = "Device";
    public const string nameOnTag = "Cunning Target";

    private const bool trackChangeInStateManager = true;

    public int index;
    public bool activated = false;
    [SerializeField]
    private SpriteLayerRendererList _RendererList;
    public SpriteLayerRendererList rendererList
    {
        get
        {
            return _RendererList;
        }
    }
    public Facing startFacing;
    public Facing endFacing;
    public CunningObjectSpriteCategory category;


    public void intimidate() { }

    public void build(Facing startFacing, Facing endFacing, CunningObjectSpriteCategory category)
    {
        this.startFacing = startFacing;
        this.endFacing = endFacing;
        this.category = category;

        setToCurrentSprite();
    }

    public abstract void setStatus(string key, bool status);
    
    public string getName()
    {
        return nameOnTag;
    }

    protected Facing getCurrentFacing()
    {
        if (!activated)
        {
            return startFacing;
        }
        else
        {
            return endFacing;
        }
    }

    public void setToCurrentSprite()
    {
        rendererList[SpriteLayer.Body].sprite = CunningObjectSpriteList.getCurrentSprite(getCurrentFacing(), category);
    }

    public int getChargeCost(SkillType skillType)
    {
        return Constants.objectSkillChargeCost;
    }


    public virtual bool validTarget(SkillType skillType)
    {
        switch(skillType)
        {
            case SkillType.Cunning:
                return true;
            default:
                return false;
        }
    }

    public void cunning()
    {
        cunning(trackChangeInStateManager);
    }

    public abstract void cunning(bool skipKeyHandling);

    public string getKey()
    {
        return generateKey(AreaManager.locationName, index);
    }

    public static string generateKey(string locationName, int index)
    {
        return locationName + "_CO_" + index;
    }

    public void trackKey()
    {
        TrapAndButtonStateManager.setKey(getKey(), activated);
    }

    private void OnEnable()
    {
        createListeners();
    }

    private void OnDisable()
    {
        destroyListeners();
    }

    //IRevealable interface methods

    public virtual void createListeners()
    {
        RevealManager.OnReveal.AddListener(onReveal);
        TrapAndButtonStateManager.OnSetTraps.AddListener(setStatus);
    }

    public virtual void destroyListeners()
    {
        RevealManager.OnReveal.RemoveListener(onReveal);
        TrapAndButtonStateManager.OnSetTraps.RemoveListener(setStatus);
    }

    public void onReveal(bool toggleReveal)
    {
        if(toggleReveal)
        {
            rendererList.createOutline(getRevealColor());
        } else
        {
            rendererList.removeOutline();
        }
    }

    public Color getRevealColor()
    {
        return ColorList.canBeCunninged;
    }

    public void createHoverTag()
    {
        MouseHoverManager.createHoverTag(tagText);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayerObject.toggleButtonPrompt(false);

        if (!RevealManager.currentlyRevealed)
        {
            rendererList.createOutline(getRevealColor());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerObject.restoreButtonPrompt();

        if (!RevealManager.currentlyRevealed)
        {
            rendererList.removeOutline();
        }
    }

    public abstract Vector3 getTargetPosition();
}

public static class CunningObjectSpriteList
{
    private static Dictionary<KeyValuePair<Facing, CunningObjectSpriteCategory>, string> cunningObjectSprites;

    public static Sprite getCurrentSprite(Facing facing, CunningObjectSpriteCategory category)
    {
        return Helpers.loadSpriteFromResources(cunningObjectSprites[new KeyValuePair<Facing, CunningObjectSpriteCategory>(facing, category)]);
    }

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateCunningObjectSprites()
    {
        cunningObjectSprites = new Dictionary<KeyValuePair<Facing, CunningObjectSpriteCategory>, string>();

        cunningObjectSprites.Add(new KeyValuePair<Facing, CunningObjectSpriteCategory>(Facing.Random, CunningObjectSpriteCategory.Crank), PrefabNames.crankSW);
        cunningObjectSprites.Add(new KeyValuePair<Facing, CunningObjectSpriteCategory>(Facing.NorthWest, CunningObjectSpriteCategory.Crank), PrefabNames.crankSW);
        cunningObjectSprites.Add(new KeyValuePair<Facing, CunningObjectSpriteCategory>(Facing.SouthWest, CunningObjectSpriteCategory.Crank), PrefabNames.crankSW);

        cunningObjectSprites.Add(new KeyValuePair<Facing, CunningObjectSpriteCategory>(Facing.NorthEast, CunningObjectSpriteCategory.Crank), PrefabNames.crankSE);
        cunningObjectSprites.Add(new KeyValuePair<Facing, CunningObjectSpriteCategory>(Facing.SouthEast, CunningObjectSpriteCategory.Crank), PrefabNames.crankSE);
    }
}
