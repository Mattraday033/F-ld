using System.Collections;
using UnityEngine;
using Cinemachine;

public class DropInFromAboveTransitionScript : PlayerInteractionScript
{
    private const float verticalMoveSpeed = 18f;
    private const float pauseAtTopSeconds = .25f;
    private const float waitBeforeStandUp = .175f;
    private const float offscreenBuffer = 2f;
    private const float standUpTimeoutBuffer = .5f;

    public override void runScript(GameObject target = null)
    {
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.inAnimation);

        PlayerObject player = PlayerObject.getInstance();

        if (player == null)
        {
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
            return;
        }

        player.StartCoroutine(runDropInAnimation());
    }

    private IEnumerator runDropInAnimation()
    {
        PlayerObject.setSpriteSortingLayer(SortingLayerManager.sixthSortingLayerInfo);

        Transform playerTransform = PlayerObject.getInstanceTransform();
        FadeToBlackManager fadeManager = FadeToBlackManager.getInstance();

        if (playerTransform == null ||
            fadeManager == null ||
            fadeManager.mainCM == null ||
            fadeManager.mainCamera == null)
        {
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
            yield break;
        }

        CinemachineVirtualCamera mainCM = fadeManager.mainCM;
        Camera mainCamera = fadeManager.mainCamera;

        Vector3 restPosition = playerTransform.position;

        mainCM.Follow = null;
        mainCM.transform.position = new Vector3(restPosition.x, restPosition.y, mainCM.transform.position.z);

        float spriteHeight = player_spriteHeight();
        float offscreenY = restPosition.y + mainCamera.orthographicSize + spriteHeight + offscreenBuffer;
        Vector3 offscreenPosition = new Vector3(restPosition.x, offscreenY, restPosition.z);

        playerTransform.position = offscreenPosition;

        AnimationManager animationManager = PlayerObject.getAnimationManager();

        startFallingAnimation(animationManager);

        while (FadeToBlackManager.isBlack() || FadeToBlackManager.isMidScreenFade())
        {
            yield return null;
        }

        yield return new WaitForSeconds(pauseAtTopSeconds);

        //Drop straight back down until the sprite is resting on the transition spot.
        while (playerTransform.position.y > restPosition.y)
        {
            playerTransform.position = Vector3.MoveTowards(playerTransform.position, restPosition, verticalMoveSpeed * Time.deltaTime);
            yield return null;
        }

        playerTransform.position = restPosition;

        PlayerMovement.updateStartEndPosition();

        // yield return new WaitForSeconds(waitBeforeStandUp);

        //The player has stopped falling, so get back up before anything else can happen.
        yield return playStandUpAnimation(animationManager);

        setIdleForCurrentArea(animationManager);

        //Re-couple the camera to the player and hand control back.
        mainCM.Follow = playerTransform;
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
        PlayerObject.setSpriteSortingLayer(SortingLayerManager.firstSortingLayerInfo);
    }

    //The falling idle is a heart beat driven idle, so it holds until the idle is changed again.
    private void startFallingAnimation(AnimationManager animationManager)
    {
        if (animationManager == null ||
            !IdleDictionary.idleDictContainsSprites(animationManager.animationName, CharacterAnimationType.Vertical_Falling))
        {
            return;
        }

        animationManager.haltAllAnimations();
        animationManager.setCurrentIdle(CharacterAnimationType.Vertical_Falling);
    }

    //Standing up sets the idle back to the normal out of combat idle when the clip ends.
    private IEnumerator playStandUpAnimation(AnimationManager animationManager)
    {
        if (animationManager == null)
        {
            yield break;
        }

        animationManager.playStandUpAnimation();

        //Animancer needs a frame before the new state reports itself as playing.
        yield return null;

        float maxWait = animationManager.getAnimationLength(CharacterAnimationType.StandUp) + standUpTimeoutBuffer;
        float timeWaited = 0f;

        while (animationManager.animancer.IsPlaying() && timeWaited < maxWait)
        {
            yield return null;

            timeWaited += Time.deltaTime;
        }
    }

    //Standing up always ends in the out of combat idle, so correct it for the area the player landed in.
    private void setIdleForCurrentArea(AnimationManager animationManager)
    {
        if (animationManager == null)
        {
            return;
        }

        if (AreaList.currentAreaIsHostile())
        {
            animationManager.setCurrentIdle(CharacterAnimationType.Idle_Front);
        } else
        {
            animationManager.setCurrentIdle(CharacterAnimationType.OOC_Idle_Front);
        }
    }

    private float player_spriteHeight()
    {
        PlayerObject player = PlayerObject.getInstance();

        if (player == null || player.playerSpriteRenderer == null)
        {
            return 0f;
        }

        return player.playerSpriteRenderer.bounds.size.y;
    }
}
