using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEscapable
{
	public void handleEscapePress();
}

public static class EscapeStack
{
	private static List<IEscapable> escapableObjects;

    public readonly static UnityEvent<IEscapable> OnEscapableObjectRemovedFromStack = new UnityEvent<IEscapable>();

    [RuntimeInitializeOnLoadMethod]
    public static void instantiateEscapeStack()
    {
        escapableObjects = new List<IEscapable>();
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
			IEscapable escapableObject = escapableObjects[escapableObjects.Count - 1];

            if(escapableObject != null && !(escapableObject is null))
            {
                escapableObject.handleEscapePress();
            } else
            {
                removeAllNullObjectsFromStack();
            }

            if(Flags.isInNewGameMode())
            {
                removeTopObjectFromStack();
            }
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
			IEscapable escapable = escapableObjects[escapableObjects.Count - 1];

			for (int index = escapableObjects.Count - 1; index >= 0; index--)
			{
				if (escapableObjects[index] == escapable)
				{
					escapableObjects.RemoveAt(index);
				}
			}

            OnEscapableObjectRemovedFromStack.Invoke(escapable);
		}
	}

	public static void escapeAll()
	{
		// Debug.LogError("Escape all");

        if(!LoadSaveFile.midLoad)
        {
            for (int index = escapableObjects.Count - 1; index >= 0; index--)
            {
                handleEscapePress();
            }
        }

		escapableObjects = new List<IEscapable>();
		//Debug.LogError("all escapableObject have been removed from stack. Count: " + escapableObjects.Count);
    }

	public static IEscapable getTopEscapable() 
	{
		return escapableObjects[escapableObjects.Count - 1];
	}

	public static int getEscapableObjectsCount() 
	{
		return escapableObjects.Count;
	}
}
