using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CunningNameTagGenerator : NameTagGenerator
{
	public override Color getRevealColor()
	{
		return ColorList.canBeCunninged;
	}

    public override void checkSpawnParams(string secretDoorFlag)
    {
        string nameToCheck = gameObject.name.Replace(OOCSpawnDetails.gameObjectNameSuffix, "");

        gameObject.SetActive(SpawnParamsList.getSpawnParams(AreaManager.locationName, nameToCheck).canSpawn(nameToCheck));
    }
}
