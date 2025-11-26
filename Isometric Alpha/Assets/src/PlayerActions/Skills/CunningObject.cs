using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum CunningObjectSpriteCategory { Statue = 0 }

public abstract class CunningObject : MonoBehaviour, ISkillTarget, IRevealable
{
    public const string tagText = "Device";

    private const bool trackChangeInStateManager = true;

    public int index;
    public bool activated = false;
    public SpriteRenderer spriteRenderer;
    public SpriteOutline outline;
    public Facing startFacing;
    public Facing endFacing;
    public CunningObjectSpriteCategory category;

    private void Awake()
    {
        outline = new SpriteOutline();
        outline.setSpriteRenderer(GetComponent<SpriteRenderer>());
    }

    public void intimidate() { }

    public void build(Facing startFacing, Facing endFacing, CunningObjectSpriteCategory category)
    {
        this.startFacing = startFacing;
        this.endFacing = endFacing;
        this.category = category;

        setToCurrentSprite();
    }

    public abstract void setStatus(string key, bool status);
    
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
        spriteRenderer.sprite = CunningObjectSpriteList.getCurrentSprite(getCurrentFacing(), category);

        switch (getCurrentFacing())
        {
            case Facing.NorthWest:
            case Facing.SouthWest:
                spriteRenderer.flipX = true;
                break;
            case Facing.NorthEast:
            case Facing.SouthEast:
                spriteRenderer.flipX = false;
                break;
        }
    }

    public int getChargeCost(SkillType skillType)
    {
        return Constants.sizeOne;
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

    public SpriteOutline getSpriteOutline()
    {
        return outline;
    }

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
            outline.createOutline(getRevealColor(), getOutlineSize());
        } else
        {
            outline.removeOutline();
        }
    }

    public Color getRevealColor()
    {
        return ColorList.canBeCunninged;
    }

	public OutlineMode getOutlineSize()
    {
        return OutlineMode.Bold;
    }

    public void createHoverTag()
    {
        MouseHoverManager.createHoverTag(tagText);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!RevealManager.currentlyRevealed)
        {
            outline.createOutline(getRevealColor(), getOutlineSize());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!RevealManager.currentlyRevealed)
        {
            outline.removeOutline();
        }
    }
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

        cunningObjectSprites.Add(new KeyValuePair<Facing, CunningObjectSpriteCategory>(Facing.NorthEast, CunningObjectSpriteCategory.Statue), PrefabNames.statueBack);
        cunningObjectSprites.Add(new KeyValuePair<Facing, CunningObjectSpriteCategory>(Facing.NorthWest, CunningObjectSpriteCategory.Statue), PrefabNames.statueBack);

        cunningObjectSprites.Add(new KeyValuePair<Facing, CunningObjectSpriteCategory>(Facing.SouthEast, CunningObjectSpriteCategory.Statue), PrefabNames.statueFront);
        cunningObjectSprites.Add(new KeyValuePair<Facing, CunningObjectSpriteCategory>(Facing.SouthWest, CunningObjectSpriteCategory.Statue), PrefabNames.statueFront);
    }
}
