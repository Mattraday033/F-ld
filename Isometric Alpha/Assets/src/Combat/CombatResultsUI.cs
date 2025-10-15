using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class CombatResultsUI : PopUpWindow
{
	private const bool defeatedEnemy = true;

    public TextMeshProUGUI goldText;
	public TextMeshProUGUI xpText;
	public TextMeshProUGUI affinityText;
	public TextMeshProUGUI itemsText;

	private static CombatResultsUI instance;

	public static CombatResultsUI getInstance()
	{
		return instance;
	}

    private void Awake()
    {
		if(instance != null)
		{
			throw new IOException("Duplicate instances of CombatResultsUI exist");
		}

		instance = this;
    }

	void Update()
	{
		KeyPressManager.updateKeyBools();

		if ((KeyBindingList.continueUIKeyIsPressed() || KeyBindingList.eitherBackoutKeyIsPressed())
			&& !KeyPressManager.handlingPrimaryKeyPress)
		{
			acceptButtonPress();
			KeyPressManager.handlingPrimaryKeyPress = true;
		}
    }

    public void displayDrops(EnemyPackInfo packInfo)
	{
		displayDrops(DropTableList.getDropTable(packInfo.dropTableName),
					 packInfo.numberOfDrops,
					 packInfo.guaranteedDrops,
					 packInfo.xpDrop);
	}

    private void displayDrops(DropTable dropTable, int numberOfDrops, ItemListID[] guaranteedDrops, int xpDropped)
    {
        ArrayList itemDrops = CombatResultsManager.determineItemDrops(dropTable, numberOfDrops, guaranteedDrops);
        int goldDropped = CombatResultsManager.determineGoldDrops(dropTable, numberOfDrops);

        if (xpDropped < 0)
        {
            xpDropped = 0;
        }

        xpText.text += xpDropped + " XP";

        goldText.text += goldDropped + " " + Purse.moneySymbol;

        string itemDropsText = "";

        if (itemDrops.Count <= 0)
        {
            itemDropsText = "None";
        }
        else
        {
            foreach (Item item in itemDrops)
            {
                itemDropsText += item.getKey() + ": x" + item.getQuantity() + "\n";
            }
        }

        itemsText.text = getRegenerationResultsText();
        itemsText.text += itemDropsText;
    }

    private string getRegenerationResultsText()
    {
        string regenText = "";

        foreach (AllyStats ally in State.formation)
        {
            if (ally == null)
            {
                continue;
            }

            int regenAmount = Strength.getCurrentRegenerationAmount(ally);

            if (regenAmount > 0)
            {
                regenText = ally.getName() + " has healed for " + regenAmount + " HP.\n";
            }
        }

        if (regenText.Length > 0)
        {
            regenText += "\n";
        }

        return regenText;
    }
    
	public override void acceptButtonPress()
    {
        EscapeStack.handleEscapePress();

        CombatStateManager.returnToOverworld(defeatedEnemy);
    }

}
