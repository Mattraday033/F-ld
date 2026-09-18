using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gate : MonoBehaviour, IRevealable, INameSource
{
    protected bool playSFX = false;

    private string gateKey;
    public string hoverName;    
    public string hiddenTerrainFlag;    

    [SerializeField]
    private SpriteLayerRendererList _RendererList;
    public SpriteLayerRendererList rendererList
    {
        get
        {
            return _RendererList;
        }
    }

    protected virtual void Awake()
    {
        _RendererList = GetComponent<SpriteLayerRendererList>();
        playSFX = !GateAndChestManager.hasBeenOpened(getGateKey());
    }

    public void setKey(string gateKey)
    {
        this.gateKey = gateKey;

        checkGateStatus();
    }

    public virtual void checkGateStatus()
    {
        if (GateAndChestManager.hasBeenOpened(getGateKey()))
        {
            if(PlayerOOCStateManager.currentActivity == OOCActivity.inFade)
            {
                playSFX = false;
                
            } else if(playSFX)
            {
                playSFX = false;
                playOpeningAudioClip();
            }

            gameObject.SetActive(false);
            SecretDoorFlags.addSecretDoorFlag(hiddenTerrainFlag);
        }
    }

    protected void playOpeningAudioClip()
    {
        if(GateAndChestManager.preventSFX)
        {
            return;
        }

        AudioManager.playAudioClipAsSingleton(getSFXType(getName()));
    }

    private static SFXType getSFXType(string gateKey)
    {
        switch(DialogueList.scrubNameOfEndNumbers(gateKey))
        {
            case NPCNameList.unstablePillar:
            case NPCNameList.awkwardRubble:
            case NPCNameList.liftableRubble:
                return SFXType.RockIntro;
            default:
                return SFXType.GateOpen;
        }
    }

    public string getName()
    {
        return gateKey;
    }

    public string getGateKey()
    {
        return AreaManager.locationName+gateKey;
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
        GateAndChestManager.OnGateKeyAdd.AddListener(checkGateStatus);
	}

	public virtual void destroyListeners()
	{
		RevealManager.OnReveal.RemoveListener(onReveal);
        GateAndChestManager.OnGateKeyAdd.RemoveListener(checkGateStatus);
	}

	public void onReveal(bool toggleReveal)
	{
        if(toggleReveal && !rendererList[SpriteLayer.Body].color.Equals(Color.clear))
        {
            rendererList.createOutline(getRevealColor());
        } else
        {
            rendererList.removeOutline();
        }
	}

	public Color getRevealColor()
	{
		return ColorList.canBeInteractedWith;
	}

	public void createHoverTag()
	{
		MouseHoverManager.getMouseHoverBase();
		MouseHoverManager.createHoverTag(hoverName);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
        switch(PlayerOOCStateManager.currentActivity)
        {
            case OOCActivity.walking:
            case OOCActivity.inChestUI:
            case OOCActivity.cunning:
            case OOCActivity.intimidating:
            case OOCActivity.observing:
                break;
            default:
                return;
        }

        PlayerObject.toggleButtonPrompt(false);

		if (!RevealManager.currentlyRevealed)
		{
            rendererList.createOutline(getRevealColor());
			createHoverTag();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
        PlayerObject.restoreButtonPrompt();

        switch(PlayerOOCStateManager.currentActivity)
        {
            case OOCActivity.walking:
            case OOCActivity.inChestUI:
            case OOCActivity.cunning:
            case OOCActivity.intimidating:
            case OOCActivity.observing:
                break;
            default:
                return;
        }

		if (!RevealManager.currentlyRevealed)
		{
			rendererList.removeOutline();
		}

		MouseHoverManager.destroyMouseHoverBase();
	}
	
}
