Shader "Custom/SpriteOutlineTest1"
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

                // Empty pixel: it becomes outline if any of its four neighbours
                // (one texel away, up/down/left/right) belongs to the sprite.
                float2 offset = _MainTex_TexelSize.xy * _OutlineSize;
                float neighbourAlpha =
                    max(max(tex2D(_MainTex, IN.texcoord + float2( offset.x, 0)).a,
                            tex2D(_MainTex, IN.texcoord + float2(-offset.x, 0)).a),
                        max(tex2D(_MainTex, IN.texcoord + float2(0,  offset.y)).a,
                            tex2D(_MainTex, IN.texcoord + float2(0, -offset.y)).a));

                if (neighbourAlpha <= _AlphaThreshold)
                {
                    discard;
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
