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

    //the sorting order the landmark was given. Hovering raises it, and this is what it goes back to
    private int baseSortPriority = 1;

    public RectTransform rectTransform;
    public SpriteRenderer spriteRenderer;
    public PolygonCollider2D polygonCollider2D;

    public string zoneKey;
    private string landmarkName = "Lovashi Camp";

    public OverHeadIconManager overHeadIconManager;
    public MapPopUpButton mapPopUpButton;

    public GameObject playerIndicator;
    public SpriteRenderer playerIndicatorSprite;

    public void setLandmark(LandmarkSpawnDetails spawnDetails)
    {
        this.zoneKey = spawnDetails.zoneKey;
        this.landmarkName = spawnDetails.landmarkName;

        //landmarks aren't spawned through the spawn behaviours that hand other icon managers their name source
        overHeadIconManager.nameSource = this;

        baseSortPriority = spawnDetails.getSortPriority();

        spriteRenderer.sprite = spawnDetails.getSprite();
        spriteRenderer.sortingOrder = baseSortPriority;
        spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

        polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
    }

    public string displayName { get { return landmarkName; } }
    public string uniqueName { get { return zoneKey; } }

    public void revealIndicator()
    {
        PartyManager.getPlayerStats().setHeadSprite(playerIndicatorSprite);
        playerIndicator.SetActive(true);
    }

    public void hideIndicator()
    {
        playerIndicator.SetActive(false);
    }

    //for a landmark on the prebuilt world map, which can be hidden while the pointer is on it and then never gets OnMouseExit
    public void clearHover()
    {
        hovered = false;

        rectTransform.localScale = Vector3.one;
        spriteRenderer.sortingOrder = baseSortPriority;
        spriteRenderer.color = Color.white;

        overHeadIconManager.destroyNameTag();
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

    //OnMouseUp fires on release wherever the pointer is, so these tell a click on the landmark apart from a drag of the map
    private bool hovered = false;
    private Vector3 pressPosition;

    private void OnMouseEnter()
    {
        hovered = true;

        setLandmarkToLarge();
        overHeadIconManager.spawnNameTag();
        spriteRenderer.color = ColorList.grey245;
    }

    private void OnMouseExit()
    {
        hovered = false;

        setLandmarkToNormal();
        overHeadIconManager.destroyNameTag();
        spriteRenderer.color = Color.white;
    }

    private void OnMouseDown()
    {
        pressPosition = InputReader.mousePosition;
        spriteRenderer.color = ColorList.grey215;
    }

    private void OnMouseUp()
    {
        spriteRenderer.color = hovered ? ColorList.grey245 : Color.white;

        if(pressWasDrag())
        {
            return;
        }

        WorldMapPopUpWindow.getInstance().popupProgenitor.destroyPopUp();

        mapPopUpButton.spawnPopUp(zoneKey);
    }

    //the world map's scroll rect starts dragging once the pointer passes the event system's drag threshold, so the same distance is used here
    private bool pressWasDrag()
    {
        int dragThreshold = EventSystem.current != null ? EventSystem.current.pixelDragThreshold : defaultDragThreshold;

        return (InputReader.mousePosition - pressPosition).sqrMagnitude >= dragThreshold * dragThreshold;
    }

    //the event system's own default, for when there is none
    private const int defaultDragThreshold = 10;

}
