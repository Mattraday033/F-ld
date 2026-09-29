using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum CunningObjectSpriteCategory { Crank = 0 }

public class CunningObject : MonoBehaviour, ISkillTarget, IRevealable, INameSource
{
    public const string tagText = "Device";
    public const string nameOnTag = "Cunning Target";

    public int index;
    public bool evenActivation = false;
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
    public CunningObjectSpriteCategory category;
    public CunningAction cunningAction;
    private QuestStepActivationScript _Script;
    public QuestStepActivationScript script
    {
        set
        {
            _Script = value;
        }
    }


    public void intimidate() { }

    public void setStatus(string key, bool status)
    {

        if (!getKey().Equals(key))
        {
            return;
        }

        evenActivation = status;

        setToCurrentSprite();
    }

    public string displayName { get { return nameOnTag; } }
    public string uniqueName { get { return getKey(); } }

    protected Facing getCurrentFacing()
    {
        return getFacing(evenActivation);
    }

    public static Facing getFacing(bool evenActivation)
    {
        if (!evenActivation)
        {
            return Facing.NorthEast;
        }
        else
        {
            return Facing.SouthWest;
        }
    }

    public void setToCurrentSprite()
    {
        CunningObjectSpriteList.getCurrentSprite(getCurrentFacing(), category).applyAppearance(rendererList);
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
        if(_Script != null && !TrapAndButtonStateManager.contains(getKey()))
        {
            _Script.runScript(gameObject);
        }

        if(cunningAction != null)
        {
            cunningAction(index, evenActivation);
        }

        evenActivation = !evenActivation;
        TrapAndButtonStateManager.setKey(getKey(), evenActivation);
    }

    public string getKey()
    {
        return generateKey(AreaManager.locationName, index);
    }

    public static string generateKey(string locationName, int index)
    {
        return locationName + "_CO_" + index;
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

    public Vector3 getTargetPosition()
    {
        return transform.position;
    }
}

public static class CunningObjectSpriteList
{
    private static Dictionary<KeyValuePair<Facing, CunningObjectSpriteCategory>, string> cunningObjectSprites;

    public static SpriteDescription getCurrentSprite(Facing facing, CunningObjectSpriteCategory category)
    {
        return new SpriteDescription(spriteName: cunningObjectSprites[new KeyValuePair<Facing, CunningObjectSpriteCategory>(facing, category)]);
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

//spawn details are built once at load, so the sprite is chosen from the saved activation state when it is applied rather than when it is built
public class CunningObjectAppearance : IAppearance
{
    private int index;
    private CunningObjectSpriteCategory category;

    public bool large { get { return false; } }
    public bool withScale { get { return true; } }

    public CunningObjectAppearance(int index, CunningObjectSpriteCategory category)
    {
        this.index = index;
        this.category = category;
    }

    public void applyAppearance(SpriteLayerRendererList rendererList, CharacterAnimationType type = CharacterAnimationType.OOC_Idle_Front, bool updateColors = false)
    {
        bool evenActivation = TrapAndButtonStateManager.contains(CunningObject.generateKey(AreaManager.locationName, index));

        CunningObjectSpriteList.getCurrentSprite(CunningObject.getFacing(evenActivation), category).applyAppearance(rendererList, type, updateColors);
    }
}

public delegate void CunningAction(int index, bool evenActivation);

public static class CunningActionList
{
    public readonly static CunningAction deactivateBlockers = (index, evenActivation) =>
    {
        EventList.SetActiveByIndexChannel.Invoke(ActivationCategory.Cunning, index, evenActivation);
    };
}