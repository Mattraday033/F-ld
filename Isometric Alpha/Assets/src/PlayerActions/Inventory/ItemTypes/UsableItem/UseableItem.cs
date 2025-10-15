using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//[System.Serializable]
public class UsableItem : Item, IJSONConvertable
{
    public const string typeIconName = "UsableItem";
    public const string type = "Use";

    private string useDescription;
    private string iconName;

    public UsableItem(ItemListID listID, string key, string loreDescription, string useDescription, string subtype, string iconName, int worth) : base(listID, key, loreDescription, type, subtype, worth)
    {
        this.useDescription = useDescription;
        this.iconName = iconName;
    }

    public UsableItem(ItemListID listID, string key, string loreDescription, string useDescription, string subtype, string iconName, int worth, int quantity) : base(listID, key, loreDescription, type, subtype, worth, quantity)
    {
        this.useDescription = useDescription;
        this.iconName = iconName;
    }

    public override string getDamageFormula()
    {
        if (getAmountToHeal() >= 1)
        {
            return "" + getAmountToHeal();
        }
        else
        {
            return "0";
        }
    }

    public override string getCritFormula()
    {
        return "0";
    }

    public virtual int getAmountToHeal()
    {
        return -1;
    }

    public override bool usableOnParty()
    {
        return true;
    }

    public virtual string getIconName()
    {
        return iconName;
    }

    public virtual int getRangeIndex()
    {
        return Range.singleTargetIndex;
    }

    public virtual string getRangeTitle()
    {
        return Range.getRangeTitle(getRangeIndex());
    }

    public virtual void use(Stats target)
    {
        throw new IOException("Base version of use() was called");
    }

    public string getUseDescription()
    {
        return useDescription + " " + getUseDescriptionAdditions();
    }

    private string getUseDescriptionAdditions()
    {
        string[] useDescriptionAdditions = new string[] { "", "", "" };

        if (usableInCombat() && usableOutOfCombat())
        {
            useDescriptionAdditions[0] = "Usable in/out of combat. ";
        }
        else if (usableInCombat())
        {
            useDescriptionAdditions[0] = "Only usable in combat. ";
        }
        else if (usableOutOfCombat())
        {
            useDescriptionAdditions[0] = "Only usable outside of combat. ";
        }
        else
        {
            useDescriptionAdditions[0] = "";
        }

        if (usableInCombat() && useRequiresAnAction())
        {
            useDescriptionAdditions[1] = "Requires an action. ";
        }
        else if (usableInCombat() && !useRequiresAnAction())
        {
            useDescriptionAdditions[1] = "Does not require an action. ";
        }
        else
        {
            useDescriptionAdditions[1] = "";
        }

        if (infiniteUses())
        {
            useDescriptionAdditions[2] = "Does not deteriorate after use. ";
        }
        else
        {
            useDescriptionAdditions[2] = "";
        }

        return useDescriptionAdditions[0] + useDescriptionAdditions[1] + useDescriptionAdditions[2];
    }

    public override CombatAction getCombatAction()
    {
        return new ItemCombatAction(this);
    }

    public override GameObject getDescriptionPanelFull(PanelType panelType)
    {
        string panelTypeName = "";

        switch (panelType)
        {
            case PanelType.Standard:

                if (usableInCombat())
                {
                    panelTypeName = PrefabNames.combatUsableUseItemDescPanelFull;
                }
                else
                {
                    panelTypeName = PrefabNames.useItemDescPanelFull;
                }

                break;
            default:
                throw new IOException("Unknown PanelType: " + panelType);
        }

        return DescriptionPanel.getDescriptionPanel(panelTypeName);
    }

    public override void describeSelfFull(DescriptionPanel panel)
    {
        base.describeSelfFull(panel);

        DescriptionPanel.setText(panel.useDescriptionText, getUseDescription());

        DescriptionPanel.setImageColor(panel.iconBackgroundPanel, Color.black);
        DescriptionPanel.setImage(panel.iconPanel, Helpers.loadSpriteFromResources(getIconName()));
    }

    public override string getTypeIconName()
    {
        return typeIconName;
    }

    //IBuildableWithBlocks methods
    public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Name, getName()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getAmountBlock(getQuantityForDisplay()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getWorthBlock(getWorthForDisplay()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(getUseDescription()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(getLoreDescription()));

        if (getIconName() != null && !(getIconName() is null) && getIconName().Length > 0)
        {
            buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getIconName()));
        }

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getTypeIconName()));

        return buildingBlocks;
    }

    public virtual bool fitsUseCriteria(Stats target)
    {
        return true;
    }
}
