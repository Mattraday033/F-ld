Shader "Custom/SpriteOutlineTest2"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        // Colour of the outline drawn around the sprite. Default = opaque black.
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        // Outline thickness in source-texture pixels. 1 = a single pixel border.
        _OutlineSize ("Outline Size (pixels)", Range(0,8)) = 1
        // Pixels with alpha above this count as part of the sprite; the rest are
        // treated as empty space that the outline is allowed to fill.
        _AlphaThreshold ("Alpha Threshold", Range(0,1)) = 0.1
        // Size of one frame, in texture pixels, for sprite sheets imported in
        // Multiple mode. Every frame lives in the same texture, so the search for
        // the sprite's top row has to be told how big a frame is or it finds the
        // highest artwork in the whole sheet. (0,0) = the texture holds one sprite.
        _FrameSize ("Frame Size (pixels, 0 = whole texture)", Vector) = (0,0,0,0)

        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [HideInInspector] _AlphaTex ("External Alpha", 2D) = "white" {}
        [HideInInspector] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4    _MainTex_TexelSize;

            fixed4 _Color;
            fixed4 _RendererColor;
            fixed4 _OutlineColor;
            float  _OutlineSize;
            float  _AlphaThreshold;
            float4 _FrameSize;

            v2f vert (appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color * _RendererColor;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap(OUT.vertex);
                #endif
                return OUT;
            }

            // Index of the topmost non-empty row of the frame containing `uv`,
            // counted from the top of the texture (0 = the very top row), or the
            // bottom of that frame when it holds no artwork at all. Rows are walked
            // from the top of the frame down and the search stops at the first hit,
            // so the cost is driven by how much blank space sits above the artwork
            // rather than by the size of the sprite.
            int TopmostSpriteRow(float2 uv)
            {
                float texWidth  = _MainTex_TexelSize.z;
                float texHeight = _MainTex_TexelSize.w;

                // On a sprite sheet the search must be confined to the frame being
                // drawn: sweeping the whole texture would return the highest row of
                // whichever frame reaches furthest up, which is not this one.
                float frameWidth  = _FrameSize.x > 0 ? _FrameSize.x : texWidth;
                float frameHeight = _FrameSize.y > 0 ? _FrameSize.y : texHeight;

                int xStart = (int)(floor(uv.x * texWidth / frameWidth) * frameWidth);
                int yStart = (int)(floor((1.0 - uv.y) * texHeight / frameHeight) * frameHeight);
                int xEnd   = min((int)texWidth,  xStart + (int)frameWidth);
                int yEnd   = min((int)texHeight, yStart + (int)frameHeight);

                [loop]
                for (int y = yStart; y < yEnd; y++)
                {
                    float v = 1.0 - (y + 0.5) * _MainTex_TexelSize.y;

                    [loop]
                    for (int x = xStart; x < xEnd; x++)
                    {
                        float u = (x + 0.5) * _MainTex_TexelSize.x;
                        if (tex2Dlod(_MainTex, float4(u, v, 0, 0)).a > _AlphaThreshold)
                        {
                            return y;
                        }
                    }
                }

                return yEnd;
            }

            fixed4 frag (v2f IN) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, IN.texcoord);

                // Solid part of the sprite: draw it unchanged.
                if (tex.a > _AlphaThreshold)
                {
                    fixed4 c = tex * IN.color;
                    c.rgb *= c.a; // premultiplied alpha to match Blend One OneMinusSrcAlpha
                    return c;
                }

                // Empty pixel: it becomes outline if a neighbour one texel away
                // belongs to the sprite.
                float2 offset = _MainTex_TexelSize.xy * _OutlineSize;
                float aLeft  = tex2D(_MainTex, IN.texcoord + float2(-offset.x, 0)).a;
                float aRight = tex2D(_MainTex, IN.texcoord + float2( offset.x, 0)).a;
                float aAbove = tex2D(_MainTex, IN.texcoord + float2(0,  offset.y)).a;
                float aBelow = tex2D(_MainTex, IN.texcoord + float2(0, -offset.y)).a;

                if (max(max(aLeft, aRight), max(aAbove, aBelow)) <= _AlphaThreshold)
                {
                    discard;
                }

                // The outline's highest row is made up purely of pixels held there
                // by artwork underneath them: anything with sprite to its side or
                // above it necessarily sits lower down, so only the "sprite below
                // me and nothing else" case is worth the row search below.
                if (max(max(aLeft, aRight), aAbove) <= _AlphaThreshold)
                {
                    // That highest row sits one outline-width above the topmost row
                    // of the sprite itself. Blank this pixel when it lands on it,
                    // leaving every lower part of the outline (including the rest of
                    // the top edge, wherever the silhouette steps down) intact.
                    int outlineTexels     = max(1, (int)round(_OutlineSize));
                    int pixelRow          = (int)floor((1.0 - IN.texcoord.y) * _MainTex_TexelSize.w);
                    int highestOutlineRow = TopmostSpriteRow(IN.texcoord) - outlineTexels;

                    if (pixelRow == highestOutlineRow)
                    {
                        discard;
                    }
                }

                // Outline keeps its own colour (it is not tinted by the sprite's
                // colour) but still fades with the renderer's alpha.
                fixed4 outline = _OutlineColor;
                outline.a *= IN.color.a;
                outline.rgb *= outline.a; // premultiplied alpha
                return outline;
            }
            ENDCG
        }
    }

    Fallback "Sprites/Default"
}
