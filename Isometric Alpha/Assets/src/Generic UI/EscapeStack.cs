using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEscapable
{
	public void handleEscapePress();
}

public static class EscapeStack
{
	private static ArrayList escapableObjects;

	static EscapeStack()
	{
		if(escapableObjects == null || escapableObjects is null)
		{
			escapableObjects = new ArrayList();
		}
	}

	public static void addEscapableObject(IEscapable newEscapableObject)
	{
		escapableObjects.Add(newEscapableObject);
		//Debug.LogError("escapableObject added to stack. Count: " + escapableObjects.Count);
	}
	
	public static void handleEscapePress()
	{
        if (escapableObjects.Count > 0)
		{
			IEscapable escapableObject = (IEscapable)escapableObjects[escapableObjects.Count - 1];

			escapableObject.handleEscapePress();
		}
	}
	
	public static void removeAllNullObjectsFromStack()
	{
		if (escapableObjects.Count > 0)
		{
			for (int index = escapableObjects.Count - 1; index >= 0; index--)
			{
				if (escapableObjects[index] == null)
				{
					escapableObjects.RemoveAt(index);
				}
			}
		}
    }

	public static void removeTopObjectFromStack()
	{
		if (escapableObjects.Count > 0)
		{
			IEscapable escapable = (IEscapable)escapableObjects[escapableObjects.Count - 1];

			for (int index = escapableObjects.Count - 1; index >= 0; index--)
			{
				if (escapableObjects[index] == escapable)
				{
					escapableObjects.RemoveAt(index);
					//Debug.LogError("escapableObject removed from stack. Count: " + escapableObjects.Count);
				}
			}
		}
	}

	public static void escapeAll()
	{
		// Debug.LogError("Escape all");
		for (int index = escapableObjects.Count - 1; index >= 0; index--)
		{
			handleEscapePress();
		}

		escapableObjects = new ArrayList();
		//Debug.LogError("all escapableObject have been removed from stack. Count: " + escapableObjects.Count);
    }

	public static int getEscapableObjectsCount() 
	{
		return escapableObjects.Count;
	}
}
