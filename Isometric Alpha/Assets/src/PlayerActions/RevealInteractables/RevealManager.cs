using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public interface IRevealable : IPointerEnterHandler,
	IPointerExitHandler
{
    public SpriteOutline getSpriteOutline();
    
	public void createListeners();

	public void destroyListeners();

	public void onReveal(bool toggleReveal);

	public Color getRevealColor();

	public void createHoverTag();
}

public static class RevealManager
{
	public static bool currentlyRevealed;

	public readonly static UnityEvent<bool> OnReveal = new UnityEvent<bool>();

    [RuntimeInitializeOnLoadMethod]
    private static void initializeRevealManager()
    {
        currentlyRevealed = false;
    }

	public static void toggleReveal()
	{
		currentlyRevealed = !currentlyRevealed;

		revealAllObjects();
	}

	public static void revealAllObjects()
	{
		OnReveal.Invoke(currentlyRevealed);
	}

    public static void resetReveals()   // For when non-standard highlight colors are used, 
    {                                   // and you want to reset to the correct outline colors
        OnReveal.Invoke(Constants.removeReveal);

        if(currentlyRevealed)
        {
           OnReveal.Invoke(Constants.reveal);
        }
    }
}