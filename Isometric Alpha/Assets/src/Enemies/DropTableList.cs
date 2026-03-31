using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class DropTableList
{
	public const string slaveMineDTKey = "slaveMineDT";
	public const string lovashiGuardsDTKey = "lovashiGuardsDT";

	public static DropTable getDropTable(string name)
	{
        switch(name)
        {
            case lovashiGuardsDTKey:
                return ItemList.lovashiGuardsDT;
            default:
                return ItemList.slaveMineDT;
        }

    }

}
