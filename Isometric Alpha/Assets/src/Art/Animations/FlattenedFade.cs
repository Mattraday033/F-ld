using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

//draws a character's layers at full opacity into one texture and shows that texture faded,
//so a see-through character looks like one image rather than layers showing through each other.
//The layers are moved far offscreen onto a layer no scene camera draws, and a capture camera of this character's own films them there
[DefaultExecutionOrder(1000)]
public class FlattenedFade : MonoBehaviour
{
    //each character gets its own capture slot, spaced wider than any creature frame, so no capture camera sees another character
    private static readonly Vector3 captureAreaOrigin = new Vector3(1000f, 1000f, 0f);
    private const float captureSlotSpacing = 20f;

    private const float cameraDistance = 10f;

    private static readonly HashSet<int> usedSlots = new();
    private static Material displayMaterial;

    //statics survive between play sessions with domain reload disabled
    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        usedSlots.Clear();
        displayMaterial = null;
    }

    private SpriteRenderer bodyRenderer;
    //-1 until a slot is taken, so a component that was never initialized frees nobody's slot
    private int slot = -1;

    private Transform rig;
    private Camera captureCamera;
    private RenderTexture captureTexture;

    private Texture2D displayTexture;
    private Sprite displaySprite;
    private SpriteRenderer displayRenderer;

    //the body frame the display sprite was last made for
    private Rect displayRect;
    private Vector2 displayPivot;
    private float displayPixelsPerUnit;

    //false when this can't run here, so the caller can fade the layers one by one instead
    public static bool apply(SpriteLayerRendererList rendererList, float alpha)
    {
        if(LayerAndTagManager.flattenedFadeLayer < 0)
        {
            Debug.LogWarning("[FlattenedFade] There is no FlattenedFade layer, so the layers are faded one by one.");
            return false;
        }

        if((SystemInfo.copyTextureSupport & CopyTextureSupport.RTToTexture) == 0)
        {
            return false;
        }

        if(rendererList == null || rendererList[SpriteLayer.Body] == null)
        {
            return false;
        }

        FlattenedFade flattenedFade = rendererList.GetComponent<FlattenedFade>();

        if(flattenedFade == null)
        {
            flattenedFade = rendererList.gameObject.AddComponent<FlattenedFade>();
            flattenedFade.initialize(rendererList);
        }

        flattenedFade.setAlpha(alpha);

        return true;
    }

    private void initialize(SpriteLayerRendererList rendererList)
    {
        bodyRenderer = rendererList[SpriteLayer.Body];

        slot = takeSlot();

        //the display stands where the body did, so it's made before the body is moved
        createDisplay(bodyRenderer.gameObject.layer);

        rig = new GameObject("Flattened Fade Rig").transform;
        rig.SetParent(transform, false);
        rig.gameObject.layer = LayerAndTagManager.flattenedFadeLayer;

        rendererList.moveCharacterLayers(rig, LayerAndTagManager.flattenedFadeLayer);

        createCaptureCamera();

        refresh();
    }

    private static int takeSlot()
    {
        int slot = 0;

        while(usedSlots.Contains(slot))
        {
            slot++;
        }

        usedSlots.Add(slot);

        return slot;
    }

    private void createDisplay(int layer)
    {
        if(displayMaterial == null)
        {
            displayMaterial = new Material(Shader.Find("Sprites/Default"));
        }

        GameObject display = new GameObject("Flattened Fade Display");
        display.layer = layer;
        display.transform.SetParent(transform, false);

        displayRenderer = display.AddComponent<SpriteRenderer>();
        displayRenderer.sharedMaterial = displayMaterial;
    }

    private void createCaptureCamera()
    {
        GameObject cameraObject = new GameObject("Flattened Fade Camera");
        cameraObject.layer = LayerAndTagManager.flattenedFadeLayer;
        cameraObject.transform.SetParent(rig, false);

        captureCamera = cameraObject.AddComponent<Camera>();

        //only ever rendered when asked, so it never joins the scene's cameras or takes mouse events
        captureCamera.enabled = false;
        captureCamera.orthographic = true;
        captureCamera.clearFlags = CameraClearFlags.SolidColor;
        captureCamera.backgroundColor = Color.clear;
        captureCamera.cullingMask = 1 << LayerAndTagManager.flattenedFadeLayer;
        captureCamera.nearClipPlane = 0.1f;
        captureCamera.farClipPlane = cameraDistance * 2f;
        //URP's HDR buffer has no alpha channel, which would leave the faded image with an opaque background
        captureCamera.allowHDR = false;
        captureCamera.allowMSAA = false;
        captureCamera.useOcclusionCulling = false;

        UniversalAdditionalCameraData cameraData = captureCamera.GetUniversalAdditionalCameraData();
        cameraData.renderPostProcessing = false;
        cameraData.antialiasing = AntialiasingMode.None;
        cameraData.renderShadows = false;
        cameraData.dithering = false;
        cameraData.requiresColorOption = CameraOverrideOption.Off;
        cameraData.requiresDepthOption = CameraOverrideOption.Off;
        cameraData.volumeLayerMask = 0;
    }

    public void setAlpha(float alpha)
    {
        Color color = displayRenderer.color;
        color.a = alpha;
        displayRenderer.color = color;
    }

    private void LateUpdate()
    {
        if(bodyRenderer == null)
        {
            return;
        }

        refresh();
    }

    //runs after the animations have set this frame's sprites, so the display never lags a frame behind them
    private void refresh()
    {
        Sprite bodySprite = bodyRenderer.sprite;

        if(bodySprite == null)
        {
            displayRenderer.enabled = false;
            return;
        }

        displayRenderer.enabled = true;

        rig.position = captureAreaOrigin + Vector3.right * slot * captureSlotSpacing;

        Rect rect = bodySprite.rect;
        float pixelsPerUnit = bodySprite.pixelsPerUnit;
        Vector2 pivot = bodySprite.pivot;

        //flipping mirrors the body around its pivot, so the frame's pivot is measured from the other side
        if(bodyRenderer.flipX)
        {
            pivot.x = rect.width - pivot.x;
        }

        updateTextures((int) rect.width, (int) rect.height);
        updateDisplaySprite(rect, pivot, pixelsPerUnit);
        frameCamera(rect, pivot, pixelsPerUnit);
        matchDisplayToBody();

        //a body hidden by the terrain is hidden on the display too, so there's nothing to film
        if(bodyRenderer.forceRenderingOff)
        {
            return;
        }

        render();
    }

    //the capture and display textures share a format, which CopyTexture needs
    private void updateTextures(int width, int height)
    {
        if(captureTexture != null && captureTexture.width == width && captureTexture.height == height)
        {
            return;
        }

        releaseTextures();

        GraphicsFormat format = QualitySettings.activeColorSpace == ColorSpace.Linear ?
                                    GraphicsFormat.R8G8B8A8_SRGB : GraphicsFormat.R8G8B8A8_UNorm;

        RenderTextureDescriptor descriptor = new RenderTextureDescriptor(width, height, format,
                                                                        SystemInfo.GetGraphicsFormat(DefaultFormat.DepthStencil));
        descriptor.msaaSamples = 1;
        descriptor.useMipMap = false;

        captureTexture = new RenderTexture(descriptor);
        captureTexture.name = "Flattened Fade Capture";
        captureTexture.filterMode = FilterMode.Point;
        captureTexture.wrapMode = TextureWrapMode.Clamp;
        captureTexture.Create();

        captureCamera.targetTexture = captureTexture;

        displayTexture = new Texture2D(width, height, format, TextureCreationFlags.None);
        displayTexture.name = "Flattened Fade Display";
        displayTexture.filterMode = FilterMode.Point;
        displayTexture.wrapMode = TextureWrapMode.Clamp;
        //only the GPU copy is ever written to, so the CPU copy is dropped
        displayTexture.Apply(false, true);

        //the old sprite was made over the old texture
        destroySprite();
    }

    private void updateDisplaySprite(Rect rect, Vector2 pivot, float pixelsPerUnit)
    {
        if(displaySprite != null && rect.size == displayRect.size && pivot == displayPivot && pixelsPerUnit == displayPixelsPerUnit)
        {
            return;
        }

        destroySprite();

        displayRect = rect;
        displayPivot = pivot;
        displayPixelsPerUnit = pixelsPerUnit;

        //FullRect, as a tight mesh would be cut from the texture's CPU copy, which is blank
        displaySprite = Sprite.Create(displayTexture,
                                        new Rect(0f, 0f, rect.width, rect.height),
                                        new Vector2(pivot.x/rect.width, pivot.y/rect.height),
                                        pixelsPerUnit, 0, SpriteMeshType.FullRect);
        displaySprite.name = "Flattened Fade";

        displayRenderer.sprite = displaySprite;
    }

    //points the camera at the body's frame, so one pixel of the texture is one pixel of the body
    private void frameCamera(Rect rect, Vector2 pivot, float pixelsPerUnit)
    {
        Transform body = bodyRenderer.transform;
        Vector3 scale = body.lossyScale;

        Vector3 frameCentre = body.TransformPoint(new Vector3((rect.width/2f - pivot.x)/pixelsPerUnit,
                                                                (rect.height/2f - pivot.y)/pixelsPerUnit,
                                                                0f));

        captureCamera.transform.SetPositionAndRotation(frameCentre - body.forward * cameraDistance, body.rotation);

        float frameWidth = rect.width/pixelsPerUnit * Mathf.Abs(scale.x);
        float frameHeight = rect.height/pixelsPerUnit * Mathf.Abs(scale.y);

        captureCamera.orthographicSize = frameHeight/2f;
        captureCamera.aspect = frameWidth/frameHeight;
    }

    //the rig sits in the character's own space, so the body's local position under it is where it stood on the character
    private void matchDisplayToBody()
    {
        Transform display = displayRenderer.transform;
        Transform body = bodyRenderer.transform;

        display.localPosition = body.localPosition;
        display.localRotation = body.localRotation;
        display.localScale = body.localScale;

        displayRenderer.sortingLayerID = bodyRenderer.sortingLayerID;
        displayRenderer.sortingOrder = bodyRenderer.sortingOrder;
        displayRenderer.spriteSortPoint = bodyRenderer.spriteSortPoint;
        displayRenderer.maskInteraction = bodyRenderer.maskInteraction;
        displayRenderer.forceRenderingOff = bodyRenderer.forceRenderingOff;
    }

    private void render()
    {
        UniversalRenderPipeline.SingleCameraRequest request = new UniversalRenderPipeline.SingleCameraRequest()
        {
            destination = captureTexture
        };

        if(RenderPipeline.SupportsRenderRequest(captureCamera, request))
        {
            RenderPipeline.SubmitRenderRequest(captureCamera, request);
        } else
        {
            captureCamera.Render();
        }

        Graphics.CopyTexture(captureTexture, displayTexture);
    }

    private void destroySprite()
    {
        if(displaySprite == null)
        {
            return;
        }

        if(displayRenderer != null)
        {
            displayRenderer.sprite = null;
        }

        Destroy(displaySprite);
        displaySprite = null;
    }

    private void releaseTextures()
    {
        if(captureTexture != null)
        {
            if(captureCamera != null)
            {
                captureCamera.targetTexture = null;
            }

            captureTexture.Release();
            Destroy(captureTexture);
            captureTexture = null;
        }

        if(displayTexture != null)
        {
            Destroy(displayTexture);
            displayTexture = null;
        }
    }

    //the rig, camera and display are children, so they go with the character
    private void OnDestroy()
    {
        usedSlots.Remove(slot);

        destroySprite();
        releaseTextures();
    }
}
