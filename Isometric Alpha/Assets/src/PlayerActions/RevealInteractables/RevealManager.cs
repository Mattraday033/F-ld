using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public interface IRevealable : INameSource, IPointerEnterHandler,
	IPointerExitHandler
{
    public SpriteOutline getSpriteOutline();
    
	public void createListeners();

	public void destroyListeners();

	public void onReveal(bool toggleReveal);

	public Color getRevealColor();

	public void createHoverTag();
}

public static class IRevealableExtensions
{
    public static void revealBasedOnStateChange(this IRevealable revealable)
    {
        if(PlayerOOCStateManager.currentActivity == OOCActivity.walking && 
            RevealManager.currentlyRevealed && 
            INonRevealableNameSource.nameSourceIsRevealable(revealable))
        {
            revealable.getSpriteOutline().createOutline(revealable.getRevealColor());
        } else if(PlayerOOCStateManager.currentActivity != OOCActivity.walking)
        {
            revealable.onReveal(false);
        }
    }
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