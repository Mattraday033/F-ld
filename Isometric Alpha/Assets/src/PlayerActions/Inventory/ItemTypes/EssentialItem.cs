using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class EssentialItem : Item, IJSONConvertable
{
    private bool canBeRemoved = false;
    public const string type = "Essential";


    public EssentialItem(ItemListID listId, string key, string loreDescription, string subtype) : base(listId, key, loreDescription, type, subtype, 1)
    {

    }

    public EssentialItem(ItemListID listId, string key, string loreDescription, string subtype, int quantity) : base(listId, key, loreDescription, type, subtype, 1, quantity)
    {

    }

    public EssentialItem(ItemListID listId, string key, string loreDescription, string subtype, int worth, int quantity) : base(listId, key, loreDescription, type, subtype, worth, quantity)
    {

    }


    public void setToRemovable()
    {
        canBeRemoved = true;
    }

    public override bool canBeJunk()
    {
        return false;
    }

    public bool getCanBeRemoved()
    {
        return canBeRemoved;
    }
	public override void describeSelfFull(DescriptionPanel panel)
	{
		base.describeSelfFull(panel);

		DescriptionPanel.setImageColor(panel.iconBackgroundPanel, getTypeIconBackgroundColor());
		DescriptionPanel.setImage(panel.iconPanel, Helpers.loadSpriteFromResources(getTypeIconName()));
	}

}
