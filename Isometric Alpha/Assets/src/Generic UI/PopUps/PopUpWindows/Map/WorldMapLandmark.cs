using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class WorldMapLandmark : MonoBehaviour, INameSource
{

    private const int hoverSortPriority = 10;
    private const float yPosScaleAdjustment = 0.25f;
    private readonly static Vector3 largeScale = new Vector3(1.5f, 1.5f, 1f);

    private int previousSortPriority = 1;

    public RectTransform rectTransform;
    public SpriteRenderer spriteRenderer;
    public PolygonCollider2D polygonCollider2D;

    public string zoneKey;
    private string landmarkName = "Lovashi Camp";

    public NameTagGenerator nameTagGenerator;
    public MapPopUpButton mapPopUpButton;

    public GameObject playerIndicator;
    public SpriteRenderer playerIndicatorSprite;

    public void setLandmark(LandmarkSpawnDetails spawnDetails)
    {
        this.zoneKey = spawnDetails.zoneKey;
        this.landmarkName = spawnDetails.landmarkName;

        spriteRenderer.sprite = spawnDetails.getSprite();
        spriteRenderer.sortingOrder = spawnDetails.getSortPriority();
        spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

        polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
    }

    public string getName()
    {
        return landmarkName;
    }

    public void revealIndicator()
    {
        PartyManager.getPlayerStats().setHeadSprite(playerIndicatorSprite);
        playerIndicator.SetActive(true);
    }

    private void setLandmarkToLarge()
    {
        rectTransform.localScale = largeScale;
        // rectTransform.position = new Vector3(rectTransform.position.x, rectTransform.position.y + yPosScaleAdjustment, rectTransform.position.z);

        previousSortPriority = spriteRenderer.sortingOrder;
        spriteRenderer.sortingOrder = hoverSortPriority;
    }

    private void setLandmarkToNormal()
    {
        rectTransform.localScale = Vector3.one;
        // rectTransform.position = new Vector3(rectTransform.position.x, rectTransform.position.y - yPosScaleAdjustment, rectTransform.position.z);
        spriteRenderer.sortingOrder = previousSortPriority;
    }

    private void OnMouseEnter()
    {
        setLandmarkToLarge();
        nameTagGenerator.spawnNameTag();
        spriteRenderer.color = ColorList.grey245;
    }

    private void OnMouseExit()
    {
        setLandmarkToNormal();
        nameTagGenerator.destroyNameTag();
        spriteRenderer.color = Color.white;
    }

    private void OnMouseDown()
    {
        spriteRenderer.color = ColorList.grey215;
    }

    private void OnMouseUp()
    {
        spriteRenderer.color = ColorList.grey245;

        WorldMapPopUpWindow.getInstance().popupProgenitor.destroyPopUp();

        mapPopUpButton.spawnPopUp(zoneKey);
    }

}
