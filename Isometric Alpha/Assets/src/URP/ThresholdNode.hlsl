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

#endif


