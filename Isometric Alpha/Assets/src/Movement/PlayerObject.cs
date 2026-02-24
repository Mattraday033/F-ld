using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;

public class PlayerObject : MonoBehaviour
{
    private static PlayerObject instance;

    public static PlayerObject getInstance()
    {
        return instance;
    }

    public static bool hasCustomPromptMessage;
    public Collider2D transitionCollider;
    public CapsuleCollider2D terrainCollider;
    public Transform UIParent;
    public GameObject pressButtonPrompt;
    public LayoutElement pressButtonPromptBackgroundLayout;
    public TextMeshProUGUI pressButtonPromptText;
    public AnimationManager animationManager;

    public MapPopUpButton mapPopUpButton;
    public WorldMapPopUpButton worldMapPopUpButton;

    public PlayerMovement playerMovement;

    [RuntimeInitializeOnLoadMethod]
    private static void initializePlayerMovement()
    {
        hasCustomPromptMessage = false;
        instance = null;
    }

    private void Awake()
    {
        instance = this;

        setAsCameraTarget();
        animationManager.setAnimations(PartyManager.getPlayer().getName());

        playerMovement.Awake();
        playerMovement.updateFacing();
        playerMovement.updateIdleDirection();
        
        TerrainVisibilityManager.initializeOnTransition();

        MovementManager.OnMoveFinished.AddListener(setButtonPromptVisibility);
        FadeToBlackManager.OnFadeBackInFinished.AddListener(setButtonPromptVisibility);
        PlayerOOCStateManager.OnStateChange.AddListener(setButtonPromptVisibility);

    }

    void Start()
    {
        StartCoroutine(setCameraSpeed());
    }

    private IEnumerator setCameraSpeed()
    {
        yield return null;

        if(PlayerOOCStateManager.currentActivity != OOCActivity.inDialogue)
        {
            DialogueManager.setCameraToDefaultSpeed();  
        }
    }

    private void OnDestroy()
    {
        MovementManager.OnMoveFinished.RemoveListener(setButtonPromptVisibility);
        FadeToBlackManager.OnFadeBackInFinished.RemoveListener(setButtonPromptVisibility);
        PlayerOOCStateManager.OnStateChange.RemoveListener(setButtonPromptVisibility);
    }

    private void setAsCameraTarget()
    {
        CinemachineVirtualCamera mainCM = GameObject.FindWithTag(LayerAndTagManager.mainVirtualCameraTag).GetComponent<CinemachineVirtualCamera>();
        mainCM.m_Follow = gameObject.transform;
    }

    public static Transform getInstanceTransform()
    {
        if (instance == null)
        {
            return null;
        }

        return instance.gameObject.transform;
    }

    public static Transform getUIParentTransform()
    {
        return instance.UIParent;
    }

    public static bool isBehindTerrain()
    {
        return Helpers.hasCollision(getInstance().terrainCollider, LayerAndTagManager.terrainLayerMask);
    }

    public static bool onTopOfTransitionOrTutorial()
    {
        if (Helpers.hasCollision(getInstance().transitionCollider) && !FadeToBlackManager.isMidFade())
        {
            if (Helpers.hasCollision(getInstance().transitionCollider, LayerAndTagManager.transitionLayerMask))
            {
                Transition transition = Helpers.getCollision(getInstance().transitionCollider, LayerAndTagManager.transitionLayerMask).transform.GetComponent<TransitionSpace>().getTransition();
                TransitionManager.changeLocation(transition);
                return true;
            }

            if (PlayerOOCStateManager.currentActivity != OOCActivity.inTutorialSequence && Helpers.hasCollision(getInstance().transitionCollider, LayerAndTagManager.tutorialLayerMask))
            {
                Collider2D tutorialCollider = Helpers.getCollision(getInstance().transitionCollider, LayerAndTagManager.tutorialLayerMask);

                if (TutorialSequence.startTutorialSequence(tutorialCollider.gameObject))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static void createCustomButtonPrompt(string promptMessage)
    {
        PlayerObject player = getInstance();

        if (player == null)
        {
            return;
        }

        if (promptMessage.Length > 0)
        {
            player.pressButtonPrompt.SetActive(true);
            player.pressButtonPromptText.text = promptMessage;

            rebuildPromptBackground();

            hasCustomPromptMessage = true;
        }
        else
        {
            player.pressButtonPrompt.SetActive(false);
        }
    }

    private static void rebuildPromptBackground()
    {
        PlayerObject player = getInstance();

        if (player == null)
        {
            return;
        }

        RectTransform textRect = player.pressButtonPromptText.GetComponent<RectTransform>();

        LayoutRebuilder.ForceRebuildLayoutImmediate(textRect);
        player.pressButtonPromptBackgroundLayout.preferredWidth = textRect.rect.width;
        player.pressButtonPromptBackgroundLayout.preferredHeight = textRect.rect.height;
    }

    public static void setButtonPromptVisibility()
    {
        setButtonPromptVisibility(MovementManager.playerSpriteIndex);
    }

    public static void setButtonPromptVisibility(int index)
    {
        PlayerObject player = getInstance();

        if (player == null || index != PlayerMovement.getInstance().getMovementIndex())
        {
            return;
        }

        if (PlayerOOCStateManager.currentActivity == OOCActivity.walking || 
            PlayerOOCStateManager.currentActivity == OOCActivity.cunning || 
            PlayerOOCStateManager.currentActivity == OOCActivity.intimidating)
        {
            string promptMessage = player.getPromptMessage();

            if (promptMessage.Length > 0)
            {

                player.pressButtonPrompt.SetActive(true);
                player.pressButtonPromptText.text = promptMessage;

                rebuildPromptBackground();

                hasCustomPromptMessage = false;
            }
            else if (hasCustomPromptMessage)
            {
                return;
            }
            else
            {
                player.pressButtonPrompt.SetActive(false);
            }
        }
        else
        {
            player.pressButtonPrompt.SetActive(false);
        }
    }

    private string getPromptMessage()
    {
        switch(PlayerOOCStateManager.currentActivity)
        {
            case OOCActivity.walking:
                if (PlayerMovement.canInteract())
                {
                    return "E: Interact";
                }

                Collider2D npcCollider = PositionQuery.npcAtPosition(PlayerMovement.getColliderWorldPosition());

                if (npcCollider != null && npcCollider.gameObject.tag.Equals(LayerAndTagManager.partyMemberTag))
                {
                    return "Z: Remove";
                }

                Collider2D moveableObjectCollider = PositionQuery.moveableObjectAtPosition(PlayerMovement.getColliderWorldPosition());

                if (moveableObjectCollider != null)
                {
                    EnemyMovement movableObject = moveableObjectCollider.gameObject.GetComponent<EnemyMovement>();

                    if (movableObject != null && movableObject.canBePutBackToStartingPosition())
                    {
                        return "Z: Return";
                    }
                }
                break;
            case OOCActivity.cunning:
                if (CunningManager.getInstance().canUseSkill())
                {
                    return "E: Cunning";
                } else if(CunningManager.getInstance().hasTooExpensiveTarget())
                {
                    return "Need More Charges";
                } else
                {
                    return "WASD: Move";
                }
            case OOCActivity.intimidating:
                if (IntimidateManager.getInstance().canUseSkill())
                {
                    return "E: Intimidate";
                }

                if(IntimidateManager.getIntimidatesRemaining() <= 0)
                {
                    return "Need More Charges";
                }

                break;
        }

        return "";
    }

    public static MapPopUpButton getMapPopUpButton()
    {
        return instance.mapPopUpButton;
    }

    public static WorldMapPopUpButton getWorldMapPopUpButton()
    {
        return instance.worldMapPopUpButton;
    }

    public static AnimationManager getAnimationManager()
    {
        return instance.animationManager;
    }

}
