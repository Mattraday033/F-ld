#ifndef ThresholdNode_INCLUDED
    #define ThresholdNode_INCLUDED

    void Threshold_float(float4 RGBA, float threshold, out float4 result, out float alpha)
    {
        float value = RGBA.w;
        result = float4(0.0,0.0,0.0,0.0);
        alpha = 0.0;

        if (value >= threshold)
        {
            result = RGBA;
            alpha = RGBA.w;
        }
    }

    void Combine_float(float4 main, float4 left, float4 right, float4 top, float4 bottom, 
                                    float4 topLeft, float4 topRight, float4 bottomLeft, float4 bottomRight,
                                    float4 color, out float4 result, out float alpha)
    {
        result = float4(0.0,0.0,0.0,0.0);
        alpha = 0.0; 

        if(main.w > .1)
        {
            result = main;
        } else 
        {
            float4 outlines[8] = {left, right, top, bottom, topLeft, topRight, bottomLeft, bottomRight};

            int highestAlphaIndex = 0;
            float highestAlpha = 0.0;

            for(int index = 0; index < outlines.Length; index++)
            {
                if(outlines[index].w > .1)
                {
                    result = color;
                    break;
                }
            }

            // result = outlines[highestAlphaIndex];
        }

        alpha = result.w;
    }

    void OverwriteSprite_float(float4 base, float4 insert, float threshold,
                                    out float4 result, out float alpha)
    {
        if(insert.w > threshold)
        {
            result = insert;
        } else
        {
            result = base;
        }

        alpha = result.w;
    }

    void ConsistentAlpha_float(float alpha1, float alpha2, float threshold, out float alpha)
    {
        if(alpha1 > threshold || alpha2 > threshold)
        {
            alpha = 1;
        } else
        {
            alpha = 0;
        }
    }

    // samples a texture through its own tiling and offset, and draws nothing where that lands outside the texture.
    // the layer textures are clamped, so without the cut off a small canvas would smear its edge pixels across the frame
    void SampleCanvas_float(UnityTexture2D tex, float2 uv, out float4 result, out float alpha)
    {
        float2 canvasUV = uv * tex.scaleTranslate.xy + tex.scaleTranslate.zw;
        float2 inside = step(0.0, canvasUV) * step(canvasUV, 1.0);

        result = SAMPLE_TEXTURE2D(tex.tex, tex.samplerstate, canvasUV) * (inside.x * inside.y);
        alpha = result.w;
    }

    // SampleCanvas's alpha, read at mip 0 so it can be used inside a loop that stops early
    float LayerAlphaLod(UnityTexture2D tex, float2 uv)
    {
        float2 canvasUV = uv * tex.scaleTranslate.xy + tex.scaleTranslate.zw;
        float2 inside = step(0.0, canvasUV) * step(canvasUV, 1.0);

        return SAMPLE_TEXTURE2D_LOD(tex.tex, tex.samplerstate, canvasUV, 0).w * (inside.x * inside.y);
    }

    float AllLayersAlpha(UnityTexture2D shieldBack, UnityTexture2D weapon, UnityTexture2D body, UnityTexture2D cloak,
                            UnityTexture2D face, UnityTexture2D hair, UnityTexture2D shieldFront, float2 uv)
    {
        float alpha = LayerAlphaLod(shieldBack, uv);
        alpha = max(alpha, LayerAlphaLod(weapon, uv));
        alpha = max(alpha, LayerAlphaLod(body, uv));
        alpha = max(alpha, LayerAlphaLod(cloak, uv));
        alpha = max(alpha, LayerAlphaLod(face, uv));
        alpha = max(alpha, LayerAlphaLod(hair, uv));
        alpha = max(alpha, LayerAlphaLod(shieldFront, uv));

        return alpha;
    }

    // The coloured outline. A pixel is lit when any layer is opaque somewhere on the square ring the outline's
    // width away. Checking the whole ring, rather than only its corners and midpoints, keeps the outline joined
    // up around the tip of a feature thinner than the outline is wide.
    // mainTex is the outline's own blank sprite, the size of the body's texture, so one of its texels is one
    // pixel in the space sizeX is measured in. The outline is as wide as it is tall, so sizeX alone gives the ring
    void RingOutline_float(float4 main, UnityTexture2D mainTex,
                            UnityTexture2D shieldBack, UnityTexture2D weapon, UnityTexture2D body, UnityTexture2D cloak,
                            UnityTexture2D face, UnityTexture2D hair, UnityTexture2D shieldFront,
                            float2 uv, float sizeX, float4 color, float threshold,
                            out float4 result, out float alpha)
    {
        result = main;

        if(main.w <= threshold)
        {
            result = float4(0.0, 0.0, 0.0, 0.0);

            float2 texel = mainTex.texelSize.xy;
            int radius = max(1, (int)round(sizeX * mainTex.texelSize.z));

            [loop]
            for(int k = -radius; k <= radius; k++)
            {
                // the top and bottom rows of the ring
                if(AllLayersAlpha(shieldBack, weapon, body, cloak, face, hair, shieldFront, uv + float2(k, radius) * texel) > threshold ||
                    AllLayersAlpha(shieldBack, weapon, body, cloak, face, hair, shieldFront, uv + float2(k, -radius) * texel) > threshold)
                {
                    result = color;
                    break;
                }

                // the sides, less the corners the rows already covered
                if(abs(k) < radius &&
                    (AllLayersAlpha(shieldBack, weapon, body, cloak, face, hair, shieldFront, uv + float2(radius, k) * texel) > threshold ||
                    AllLayersAlpha(shieldBack, weapon, body, cloak, face, hair, shieldFront, uv + float2(-radius, k) * texel) > threshold))
                {
                    result = color;
                    break;
                }
            }
        }

        alpha = result.w;
    }

#endif


