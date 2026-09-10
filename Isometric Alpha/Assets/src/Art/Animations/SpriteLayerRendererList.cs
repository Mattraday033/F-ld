using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLayerRendererList : MonoBehaviour
{
    #region SpriteRenderers
    [SerializeField]
    private SpriteRenderer shieldBackRenderer;
    [SerializeField]
    private SpriteRenderer weaponRenderer;
    [SerializeField]
    private SpriteRenderer bodyRenderer;
    [SerializeField]
    private SpriteRenderer cloakRenderer;
    [SerializeField]
    private SpriteRenderer faceRenderer;
    [SerializeField]
    private SpriteRenderer hairRenderer;
    [SerializeField]
    private SpriteRenderer shieldFrontRenderer;
    #endregion

    private bool instantiated = false;
    private Dictionary<SpriteLayer, SpriteRenderer> spriteLayers;

    public void Awake()
    {
        if(instantiated)
        {
            return;
        }

        spriteLayers = new Dictionary<SpriteLayer, SpriteRenderer>()
        {
            [SpriteLayer.Shield_Back] = shieldBackRenderer,  
            [SpriteLayer.Weapon] = weaponRenderer,  
            [SpriteLayer.Body] = bodyRenderer,  
            [SpriteLayer.Cloak] = cloakRenderer,  
            [SpriteLayer.Face] = faceRenderer,  
            [SpriteLayer.Hair] = hairRenderer,  
            [SpriteLayer.Shield_Front] = shieldFrontRenderer  
        };

        instantiated = true;
    }

    public SpriteRenderer this[SpriteLayer layer]
    {
        get { return spriteLayers[layer]; }
    }

}
