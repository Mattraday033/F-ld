using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class DropTableList
{
	public static List<DropTable> allDropTables;

	public const string slaveMineDT1Name = "slaveMineDT1";

	public static DropTable slaveMineDT1;
	
	public static DropTable getDropTable(string name)
	{
		if(name == null || name is null)
		{
			throw new IOException("DropTable name was null.");
		}
		
		foreach(DropTable dropTable in allDropTables)
		{
			if(String.Equals(name, dropTable.name, StringComparison.OrdinalIgnoreCase))
			{
				return dropTable;
			}
		}

		return (DropTable) allDropTables[0];

    }

}
