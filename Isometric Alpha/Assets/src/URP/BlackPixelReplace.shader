Shader "Custom/BlackPixelReplace"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        // Pixels whose brightest channel is <= this are treated as "black".
        _Threshold ("Black Threshold", Range(0,1)) = 0.1
        // Pixels whose alpha is <= this are treated as (mostly) transparent and left untouched.
        _AlphaThreshold ("Transparent Threshold", Range(0,1)) = 0.1
        // Color black pixels are replaced with. Default = RGBA(225,225,225,255).
        _ReplaceColor ("Black Replacement Color", Color) = (0.882353,0.882353,0.882353,1)
        // Color every other (non-transparent, non-black) pixel is replaced with.
        // Default = ColorList.surpriseIconYellow = RGBA(255,230,30,255).
        _OtherColor ("Other Replacement Color", Color) = (1,0.9019608,0.1176471,1)

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

            fixed4 _Color;
            fixed4 _RendererColor;
            float  _Threshold;
            float  _AlphaThreshold;
            fixed4 _ReplaceColor;
            fixed4 _OtherColor;

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

            sampler2D _MainTex;

            fixed4 frag (v2f IN) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, IN.texcoord);

                // Transparent / mostly-transparent pixels are left completely untouched.
                if (tex.a > _AlphaThreshold)
                {
                    // Detect "black" using the texture's brightest channel,
                    // independent of any tint applied via vertex/material color.
                    if (max(tex.r, max(tex.g, tex.b)) <= _Threshold)
                    {
                        // Black pixel -> black replacement color (keep original alpha).
                        tex.rgb = _ReplaceColor.rgb;
                        tex.a  *= _ReplaceColor.a;
                    }
                    else
                    {
                        // Any other visible color -> the second replacement color.
                        tex.rgb = _OtherColor.rgb;
                        tex.a  *= _OtherColor.a;
                    }
                }

                fixed4 c = tex * IN.color;
                c.rgb *= c.a; // premultiplied alpha to match Blend One OneMinusSrcAlpha
                return c;
            }
            ENDCG
        }
    }

    Fallback "Sprites/Default"
}
