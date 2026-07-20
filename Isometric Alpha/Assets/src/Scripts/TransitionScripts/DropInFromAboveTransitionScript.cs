using System.Collections;
using UnityEngine;
using Cinemachine;

public class DropInFromAboveTransitionScript : PlayerInteractionScript
{
    private const float verticalMoveSpeed = 18f;
    private const float pauseAtTopSeconds = .25f;
    private const float pauseAfterLandingSeconds = .75f;
    private const float offscreenBuffer = 2f;          

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

        mainCM.m_Follow = null;
        mainCM.transform.position = new Vector3(restPosition.x, restPosition.y, mainCM.transform.position.z);

        float spriteHeight = player_spriteHeight();
        float offscreenY = restPosition.y + mainCamera.orthographicSize + spriteHeight + offscreenBuffer;
        Vector3 offscreenPosition = new Vector3(restPosition.x, offscreenY, restPosition.z);

        playerTransform.position = offscreenPosition;

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

        yield return new WaitForSeconds(pauseAfterLandingSeconds);

        //Re-couple the camera to the player and hand control back.
        mainCM.m_Follow = playerTransform;
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
        PlayerObject.setSpriteSortingLayer(SortingLayerManager.firstSortingLayerInfo);
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
