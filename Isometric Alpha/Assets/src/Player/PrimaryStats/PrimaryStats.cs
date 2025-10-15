using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrimaryStats
{


    public static int getStatAtLevelFastProgression(int level)
    {
        switch (level)
        {
            case 2:
            case 3:
                return 3;
            case 4:
            case 5:
                return 4;
            default:
                return 2;
        }
    }

    public static int getStatAtLevelSlowProgression(int level)
    {
        switch (level)
        {	
            case 1:
            case 2:
                return 1;
            case 3:
            case 4:
                return 2;
            case 5:
                return 3;
            default:
                return 1;
        }
    }

}
