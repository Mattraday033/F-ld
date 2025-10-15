using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BookItem : UsableItem
{
	public const string typeIconName = "BookItem";
	public const string subtype = "Book";
	
	public const int bookWorth = 5;
	public const string bookUseDescription = "Use this book to read it's contents.";
	public const string bookIconName = "Book";
	
	public int index;
	public string questName;
	public int questStep = -1;
	public string[] flagsFlippedWhenRead;

	public BookPopUpButton bookPopUpButton;

    public BookItem(ItemListID listID, string key, string loreDescription, int index): base(listID, key, loreDescription, bookUseDescription, subtype, bookIconName, bookWorth) 
	{
		this.index = index;

		this.bookPopUpButton = new BookPopUpButton();
    }

	public BookItem(ItemListID listID, string key, string loreDescription, int index, string[] flagsFlippedWhenRead): base(listID, key, loreDescription, bookUseDescription, subtype, bookIconName, bookWorth) 
	{
		this.index = index;
		this.flagsFlippedWhenRead = flagsFlippedWhenRead;

        this.bookPopUpButton = new BookPopUpButton();
    }

	public BookItem(ItemListID listID, string key, string loreDescription, int index, string questName, int questStep): base(listID, key, loreDescription, bookUseDescription, subtype, bookIconName, bookWorth) 
	{
		this.index = index;
		this.questName = questName;
		this.questStep = questStep;

        this.bookPopUpButton = new BookPopUpButton();
    }

	public BookItem(ItemListID listID, string key, string loreDescription, int index, string[] flagsFlippedWhenRead, string questName, int questStep): base(listID, key, loreDescription, bookUseDescription, subtype, bookIconName, bookWorth) 
	{
		this.index = index;
		this.flagsFlippedWhenRead = flagsFlippedWhenRead;
		this.questName = questName;
		this.questStep = questStep;

        this.bookPopUpButton = new BookPopUpButton();
    }
	
	public BookItem(ItemListID listID, string key, string loreDescription, int index, string[] flagsFlippedWhenRead, string questName, int questStep, int quantity): base(listID, key, loreDescription, bookUseDescription, subtype, bookIconName, bookWorth, quantity) 
	{
		this.index = index;
		this.flagsFlippedWhenRead = flagsFlippedWhenRead;
		this.questName = questName;
		this.questStep = questStep;

        this.bookPopUpButton = new BookPopUpButton();
    }

    public void use(Stats target, bool giveCopyOfBook, OOCActivity previousActivity, GameObject bookGameObject)
    {
        setAllReadFlags();

        setQuestStepOnRead();

        bookPopUpButton.spawnPopUp(this, giveCopyOfBook, previousActivity, bookGameObject);
    }

    public override void use(Stats target)
	{
        setAllReadFlags();

        setQuestStepOnRead();

		if(PlayerOOCStateManager.currentActivity != OOCActivity.inUI)
		{
			Debug.LogError("PlayerOOCStateManager.currentActivity not inUI as expected. PlayerOOCStateManager.currentActivity = " + PlayerOOCStateManager.currentActivity.ToString());
		}

        bookPopUpButton.spawnPopUp(this, false, OOCActivity.inUI, null);
    }
	
	public override bool infiniteUses()
	{
		return true;
	}
	
	public override bool usableOnParty()
	{
		return false;
	}
	
	public int getIndex()
	{
		return index;
	}
	
	public void setAllReadFlags()
	{
		if(flagsFlippedWhenRead != null && !(flagsFlippedWhenRead is null))
		{
			foreach(string flag in flagsFlippedWhenRead)
			{
				Flags.setFlag(flag, true);
			}
		}
	}
	
	public void setQuestStepOnRead()
	{
		if(questName != null && !(questName is null) && questStep >= 0)
		{
			QuestList.activateQuestStep(questName, questStep);
		}
	}
	
	public override bool usableOutOfCombat()
	{
		return true;
	}

	private string getAllReadFlags()
	{
		string output = "";
		
		foreach(string flag in flagsFlippedWhenRead)
		{
			if(output.Length <= 0)
			{
				output += flag;
			} else
			{
				output += "~" + flag;
			}
			
		}
		
		return output;
	}

	public override string getTypeIconName()
	{
		return typeIconName;
	}

	public override string getIconName()
	{
		return "";
	}


	//IDescribable methods
	public override void describeSelfFull(DescriptionPanel panel)
	{
		base.describeSelfFull(panel);

		DescriptionPanel.setText(panel.contentsText, BookList.getBookContents(getKey()));
	}
}
