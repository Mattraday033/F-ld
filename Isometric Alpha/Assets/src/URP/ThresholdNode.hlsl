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

    void Combine_float(float4 main, float4 left, float4 right, float4 top, float4 bottom, out float4 result)
    {
        result = float4(0.0,0.0,0.0,0.0);

        if(main.w > 0)
        {
            result = main;
        } else if(right.w > 0)
        {
            result = right;
        } else if(left.w > 0)
        {
            result = left;
        } else if(top.w > 0)
        {
            result = top;
        } else if(bottom.w > 0)
        {
            result = bottom;
        }
    }

#endif


