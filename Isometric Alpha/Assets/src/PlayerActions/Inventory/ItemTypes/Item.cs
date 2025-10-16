using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using Newtonsoft.Json;

[System.Serializable]
public class Item : ICloneable, IJSONConvertable, IDescribable, ISortable, IDescribableInBlocks
{
	private ItemListID listID;

	private string key;
	private string type;
	private string subtype;
	private string loreDescription;

	private int worth;
	private int quantity;

	public Item(ItemListID listID, string key, string loreDescription, string type, string subtype, int worth)
	{
		this.listID = listID;

		this.key = key;
		this.type = type;
		this.subtype = subtype;
		this.loreDescription = loreDescription;

		this.worth = worth;
		this.quantity = 1;
	}

	[JsonConstructor]
	public Item(ItemListID listID, string key, string loreDescription, string type, string subtype, int worth, int quantity)
	{
		this.listID = listID;

		this.key = key;
		this.type = type;
		this.subtype = subtype;
		this.loreDescription = loreDescription;

		this.worth = worth;
		this.quantity = quantity;

	}

	public override bool Equals(object obj)
	{
		Item item = obj as Item;

		if (item != null &&
			item.getSlotID() == getSlotID() &&
			String.Equals(item.getKey(), getKey(), StringComparison.Ordinal) &&
			String.Equals(item.getType(), getType(), StringComparison.Ordinal) &&
			String.Equals(item.getSubtype(), getSubtype(), StringComparison.Ordinal))
		{
			return true;
		}

		return false;
	}

	public string getName()
	{
        return getKey();
	}

	public ItemListID getItemListID()
	{
		return listID;
	}

	public int addQuantity(int addition)
	{
		quantity += addition;
		return quantity;
	}

	public int removeQuantity(int subtraction)
	{
		if ((quantity - subtraction) < 0)
		{
			throw new IOException("Quantity of " + key + "cannot go below 0. Quantity = " + quantity + ". subtraction = " + subtraction + ".");
		}

		quantity -= subtraction;
		return quantity;
	}

	public void setQuantity(int newQuantity)
	{
		quantity = newQuantity;
	}

	public virtual int getSlotID()
	{
		return -1;
	}

	public virtual bool canBeJunk()
	{
		return true;
	}

	public virtual bool mustBeJunk()
	{
		return false;
	}

	public string getKey()
	{
		return key;
	}

	public string getType()
	{
		return type;
	}

	public string getSubtype()
	{
		return subtype;
	}

	public string getLoreDescription()
	{
		return loreDescription;
	}

	//only use for quest item save/load editting lore descriptions
	public void setLoreDescription(string newLore)
	{
		loreDescription = newLore;
	}

	public int getWorth()
	{
        return getWorth(ShopPopUpWindow.getCurrentMode());
	}

	public int getWorth(ShopMode shopMode)
	{
		switch (shopMode)
		{
			case ShopMode.Buy:
				return (int)((worth) * PartyStats.getDiscountMultiplier() * ShopPopUpWindow.getCurrentShopkeeper().getDiscount());
			default:
				return worth;
		}
	}

    public string getWorthForDisplay()
    {
        return getWorth() + Purse.moneySymbol;
    }

	public bool isJunk()
	{
		return Inventory.junkContainsItem(getKey());
	}

	public virtual int getID()
	{
		return -1;
	}

	public virtual bool isEquippable()
	{
		return false;
	}

	public virtual bool isUnequippable()
	{
		return false;
	}

	public virtual bool usableOutOfCombat()
	{
		return false;
	}

	public virtual bool usableInCombat()
	{
		return false;
	}

	public virtual bool useRequiresAnAction()
	{
		return true;
	}

	public virtual bool fitsUseCriteria(Stats stats)
	{
		return false;
	}

	public virtual bool infiniteUses()
	{
		return false;
	}

	public virtual bool usableOnParty()
	{
		return false;
	}

	public virtual bool removeFromInventoryWhenCreatingCombatAction()
	{
		return false;
	}

	public virtual string getTypeIconName()
	{
		throw new IOException("Base Version of getTypeIconName() was called erroneously");
	}

	public virtual Color getTypeIconBackgroundColor()
	{
		return Color.black;
	}

	public virtual string getSlotIconName()
	{
		throw new IOException("Base Version of getSlotIconName() was called erroneously");
	}

	public virtual Color getSlotIconBackgroundColor()
	{
		return Color.black;
	}

	public virtual bool isEquipped(AllyStats target)
	{
		foreach (CombatAction action in target.getActionArray().getActions())
        {
            if (action != null && action.getSourceItem() != null && action.getSourceItem().getKey().Equals(getKey()))
            {
                return true;
            }
        }
        
		return false;
	}

	//used when calculating worth via some formula, such as in the case of finding worth of armor via it's armorRating
	public void setWorth(int worth)
	{
		this.worth = worth;
	}

	public int getQuantity()
	{
		return quantity;
	}

	public string getQuantityForDisplay()
	{
        return "x" + getQuantity();
	}

	public virtual string getDamageFormula()
	{
		return "0";
	}

	public virtual int getDamageFormulaTotal()
	{
		return 0;
	}

	public virtual string getDamageTotalForDisplay()
	{
		return "0 (0)";
	}

	public virtual string getCritFormula()
	{
		return "0";
	}

	public virtual int getCritFormulaTotal()
	{
		return 0;
	}

	public string getCritTotalForDisplay()
	{
		return getCritFormulaTotal() + "%";
	}

	public virtual string getCritFormulaTotalForDisplay()
	{
		return DamageCalculator.calculateFormula(getCritFormula(), null) + "%";
	}

	public virtual CombatAction getCombatAction()
	{
		return null;
	}

	public virtual bool appliesStanceStacks()
	{
		return false;
	}

	public object Clone()
	{
		return this.MemberwiseClone();
	}

	public virtual Item clone()
	{
		return (Item)Clone();
	}

	public string convertToJson()
	{
		return "{\"listIndex\":\"" + listID.listIndex + "\"," +
				"\"itemIndex\":\"" + listID.itemIndex + "\"," +
				"\"quantity\":\"" + getQuantity() + "\"" +
				"}";

	}

	public virtual GameObject getRowType(RowType rowType)
	{
		string rowTypeName = "";

		switch (rowType)
		{
			case RowType.Standard:
				rowTypeName = PrefabNames.inventoryRow;
				break;
            case RowType.StatRequirements:
				rowTypeName = PrefabNames.playerAbilityRow;
				break;
			case RowType.AbilityEditor:
				rowTypeName = PrefabNames.actionEditorRow;
				break;
			case RowType.Shop:
				rowTypeName = PrefabNames.shopRow;
				break;
			default:
				throw new IOException("Incompatible RowType: " + rowType);
		}

		return Resources.Load<GameObject>(rowTypeName);
	}

	public virtual GameObject getDecisionPanel()
	{
		return Resources.Load<GameObject>(PrefabNames.itemDecisionButtons);
	}

	public virtual GameObject getDescriptionPanelFull()
	{
		return getDescriptionPanelFull(PanelType.Standard);
	}

	public virtual GameObject getDescriptionPanelFull(PanelType panelType)
	{
		string panelTypeName = "";

		switch (panelType)
		{
			case PanelType.Standard:
				panelTypeName = PrefabNames.treasureEssentialDescPanelFull;
				break;
			default:
				throw new IOException("Unknown PanelType: " + panelType);
		}

		return DescriptionPanel.getDescriptionPanel(panelTypeName);
	}

    public virtual void describeSelfFull(DescriptionPanel panel)
    {
        panel.setObjectBeingDescribed(this);

        DescriptionPanel.setText(panel.nameText, getKey());
        DescriptionPanel.setText(panel.amountText, panel.amountPrefix + getQuantity());
        DescriptionPanel.setText(panel.worthText, getWorthForDisplay());
        DescriptionPanel.setText(panel.loreDescriptionText, getLoreDescription());

        DescriptionPanel.setImageColor(panel.typeIconBackgroundPanel, getTypeIconBackgroundColor());
        DescriptionPanel.setImage(panel.typeIconPanel, Helpers.loadSpriteFromResources(getTypeIconName()));
    }

	public virtual void describeSelfRow(DescriptionPanel panel)
	{
		describeSelfFull(panel);
	}

	public bool hasDescisionPanelType()
	{
		return true;
	}

	public void setUpDecisionPanel(IDecisionPanel descisionPanel)
	{
		descisionPanel.setObjectToBeDecidedOn(this);
	}

	public bool withinFilter(string[] filterParameters)
	{
		if (filterParameters == null || filterParameters.Length <= 0)
		{
			return true;
		}

		foreach (string parameter in filterParameters)
		{
			if (string.Equals(parameter, getSubtype(), StringComparison.OrdinalIgnoreCase) ||
				string.Equals(parameter, getType(), StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}

		return false;
	}

	public bool ineligible()
	{
		return false;
	}

	public ArrayList getRelatedDescribables()
	{
		return new ArrayList();
	}

	public bool buildableWithBlocks()
    {
        return true;
    }

	public bool buildableWithBlocksRows()
    {
        return false;
    }

	//IDescribableInBlocks methods
	public virtual List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{
		List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Name, getName()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getAmountBlock(getQuantityForDisplay()));
		buildingBlocks.Add(DescriptionPanelBuildingBlock.getWorthBlock(getWorthForDisplay()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(getLoreDescription()));

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getTypeIconName()));

		return buildingBlocks;
	}

    //Shopkeeping Methods
    
    public static int getTotalWorth(Item item)
	{
		return getTotalWorth(item.getWorth(ShopPopUpWindow.getCurrentMode()), item.getQuantity());
	}

    public static int getTotalWorth(Item item, ShopMode shopMode)
    {
        return getTotalWorth(item.getWorth(shopMode), item.getQuantity());
    }

	public static int getTotalWorth(int worth, int amount)
    {
        return amount * worth;
    }

	//ISortable
	public int getLevel()
	{
		throw new NotImplementedException("Cannot sort Items by Level");
	}

	public int getNumber()
	{
		throw new NotImplementedException("Cannot sort Items by Number. Did you mean Quantity?");
	}
}
