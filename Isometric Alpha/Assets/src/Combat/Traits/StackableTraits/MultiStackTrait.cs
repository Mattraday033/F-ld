using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum MultiStackProcType { RedKnife = 0, BlueShield = 1, YellowThorn = 2, GreenLeaf = 3 }
public class MultiStackTrait : Trait
{
    public StackableTrait[] stackableTraits;

    public bool attachedToListeners = false;

    public MultiStackTrait(Trait baseTrait, StackableTrait[] stackableTraits) :
    base(baseTrait.getName(), baseTrait.getType(), baseTrait.getDescription(), baseTrait.getIconName(), baseTrait.getTraitIconBackgroundColor())
    {
        this.stackableTraits = stackableTraits;
    }
    public override bool hasActionCostType(ActionCostType typeToCheckFor)
    {
        foreach (StackableTrait stackableTrait in stackableTraits)
        {
            if (stackableTrait.hasActionCostType(typeToCheckFor))
            {
                return true;
            }
        }

        return false;
    }

    public override int getNumberOfStacks()
    {
        return 1;
    }

    public override int getNumberOfStacks(ActionCostType costType)
    {
        if (!hasActionCostType(costType))
        {
            return 0;
        }

        foreach (StackableTrait stackableTrait in stackableTraits)
        {
            if (stackableTrait.hasActionCostType(costType))
            {
                return stackableTrait.getNumberOfStacks();
            }
        }

        return 0;
    }

    public override void removeStacks(ActionCostType costType, int stacksToRemove)
    {
        for (int index = 0; index < stackableTraits.Length; index++)
        {
            if (stackableTraits[index].hasActionCostType(costType))
            {
                stackableTraits[index].removeStacks(costType, stacksToRemove);
                return;
            }
        }
    }

    private void reapplySpecificStackableTrait(Stats actor, MultiStackProcType procType)
    {
        if (CombatGrid.positionIsOnAlliedSide(actor.position))
        {
            stackableTraits[(int)procType].reapply();
        }
    }

    //IDescribable Methods

    public override GameObject getDescriptionPanelFull(PanelType panelType)
    {
        string panelTypeName = "";

        switch (panelType)
        {
            case PanelType.CombatHover:
                panelTypeName = PrefabNames.multiStackableTraitHoverDescriptionPanel;
                break;
            default:
                return base.getDescriptionPanelFull(panelType);
        }

        return DescriptionPanel.getDescriptionPanel(panelTypeName);
    }

    public override void describeSelfFull(DescriptionPanel panel)
    {
        base.describeSelfFull(panel);

        MultiStackableTraitDescriptionPanel multiPanel = (MultiStackableTraitDescriptionPanel)panel;

        for (int index = 0; index < multiPanel.iconPanels.Length && index < multiPanel.stackCounters.Length; index++)
        {
            DescriptionPanel.setImage(multiPanel.iconPanels[index], Helpers.loadSpriteFromResources(stackableTraits[index].getIconName()));
            DescriptionPanel.setText(multiPanel.stackCounters[index], stackableTraits[index].getNumberOfStacks());

            if (!stackableTraits[index].getTraitIconBackgroundColor().Equals(Color.black) &&
                    !stackableTraits[index].getTraitIconBackgroundColor().Equals(Color.clear))
            {
                DescriptionPanel.setTextColor(multiPanel.stackCounters[index], stackableTraits[index].getTraitIconBackgroundColor());
            }
        }
    }

    public override void describeSelfRow(DescriptionPanel panel)
    {
        base.describeSelfRow(panel);

        MultiStackableTraitDescriptionPanel multiPanel = (MultiStackableTraitDescriptionPanel)panel;

        for (int index = 0; index < multiPanel.iconPanels.Length && index < multiPanel.stackCounters.Length; index++)
        {
            DescriptionPanel.setImage(multiPanel.iconPanels[index], Helpers.loadSpriteFromResources(stackableTraits[index].getIconName()));
            DescriptionPanel.setText(multiPanel.stackCounters[index], stackableTraits[index].getNumberOfStacks());

            if (!stackableTraits[index].getTraitIconBackgroundColor().Equals(Color.black) &&
                    !stackableTraits[index].getTraitIconBackgroundColor().Equals(Color.clear))
            {
                multiPanel.stackCounters[index].color = stackableTraits[index].getTraitIconBackgroundColor();
            }
        }
    }

    public override GameObject getRowType(RowType rowType)
    {
        return Resources.Load<GameObject>(PrefabNames.multiStackableTraitSquareRowPanel);
    }

    public override ArrayList getRelatedDescribables()
    {
        ArrayList relatedDescribables = new ArrayList();

        relatedDescribables.AddRange(stackableTraits);

        return relatedDescribables;
    }

    public override Trait clone()
    {
        MultiStackTrait clonedTrait = (MultiStackTrait)Clone();

        clonedTrait.stackableTraits = new StackableTrait[stackableTraits.Length];

        for (int index = 0; index < stackableTraits.Length; index++)
        {
            clonedTrait.stackableTraits[index] = (StackableTrait)stackableTraits[index].clone();
        }

        return (Trait)clonedTrait;
    }
}
