using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gate : MonoBehaviour, IRevealable, INameSource
{
    protected bool playSFX = false;

    private string gateKey;
    public string uniqueName { get { return gateKey; } }

    private string _NPCName = "";
    public string displayName { get { return _NPCName; } }

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
        _NPCName = NameSourceExtensions.toNPCName(gateKey);

        checkGateStatus();
    }

    public virtual void checkGateStatus()
    {
        if (GateAndChestManager.hasBeenOpened(getGateKey()))
        {
            if(PlayerStateManager.currentActivity == CurrentActivity.InFade)
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

        AudioManager.playAudioClipAsSingleton(getSFXType(uniqueName));
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

	//An extra space of a multi-cell gate is the blank Extra Space prefab, which has no SpriteLayerRendererList
	//and so nothing to outline. It still listens for OnGateKeyAdd, which is what turns its collider off when
	//the gate opens.
	public virtual void createListeners()
	{
        if(rendererList != null)
        {
            RevealManager.OnReveal.AddListener(onReveal);
        }

        GateAndChestManager.OnGateKeyAdd.AddListener(checkGateStatus);
	}

	public virtual void destroyListeners()
	{
        if(rendererList != null)
        {
		    RevealManager.OnReveal.RemoveListener(onReveal);
        }

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
        switch(PlayerStateManager.currentActivity)
        {
            case CurrentActivity.Walking:
            case CurrentActivity.InChestUI:
            case CurrentActivity.Cunning:
            case CurrentActivity.Intimidating:
            case CurrentActivity.Observing:
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

        switch(PlayerStateManager.currentActivity)
        {
            case CurrentActivity.Walking:
            case CurrentActivity.InChestUI:
            case CurrentActivity.Cunning:
            case CurrentActivity.Intimidating:
            case CurrentActivity.Observing:
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
