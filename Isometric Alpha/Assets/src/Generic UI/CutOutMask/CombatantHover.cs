using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantHover : CombatMouseHover
{

    public Stats linkedStats;

    public void OnMouseEnter()
    {
        if (CombatStateManager.whoseTurn == WhoseTurn.Player && getTargetStats() != null && !useHoverTiles())
        {
            onReveal(Constants.reveal);

            createHoverTag();

            if (CombatStateManager.currentActivity == CurrentActivity.ChoosingLocation && !DamagePreviewManager.hasPreviewAtCoords(getTargetStats().position))
            {
                createHoverDamagePreview();
            }

        }
    }

    public void OnMouseExit()
    {
        if (CombatStateManager.whoseTurn == WhoseTurn.Player && getTargetStats() != null)
        {
            if (getTargetGameObject() != null && !useHoverTiles())
            {
                getSpriteOutline().removeOutline();
            }

            SelectorManager.displayHoverUIForCurrentSelectorTarget();

            if (CombatStateManager.currentActivity == CurrentActivity.ChoosingLocation)
            {
                DamagePreviewManager.removeAllHoverPreviews();
                DamagePreviewManager.setUpDamagePreviews();
            }
        }
    }

    public void OnMouseOver()
    {
        if(AbilityMenuButton.hoveringOverAbilityMenuButton)
        {
            getSpriteOutline().removeOutline();
        } else
        {
            onReveal(Constants.reveal);
        }
    }

    protected override Stats getTargetStats()
    {
        return linkedStats;
    }

    protected override GridCoords getTargetCoords()
    {
        return linkedStats.position;
    }

    public SpriteOutline getSpriteOutline()
    {
        return getTargetStats().outline;
    }

    public void onReveal(bool toggleReveal)
    {
        if(toggleReveal)
        {
            getSpriteOutline().createOutline(getRevealColor());
        } else
        {
            getSpriteOutline().removeOutline();
        }
    }

    public Color getRevealColor()
    {
        if (CombatGrid.positionIsOnAlliedSide(getTargetStats().position))
        {
            return ColorList.canBeInteractedWith;
        }
        else
        {
            return ColorList.attacksOnSight;
        }
    }

}
