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

#endif


