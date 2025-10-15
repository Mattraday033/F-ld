using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public enum DirectionMode { BuySendTo = 1, SellReceiveFrom = 2 };

public interface IAcceptsAmounts
{
	public void acceptAmount(int amount, DirectionMode directionMode);
}

public class AmountPanel : MonoBehaviour
{
	public const int greyRGB = 200;
	private const string buyButtonMessage = "Buy";
    private const string sellButtonMessage = "Sell";
    private const int incrementButtonIndex = 0;
    private const int decrementButtonIndex = 1;

    public int characterLimit = 2;

    private IAcceptsAmounts amountExecutor;
    private DirectionMode directionMode;

	public DescriptionPanel descriptionPanel;

	private int currentAmount = 1;

	public TMP_InputField amountInput;
	
	public Button acceptButton;
	public Button[] adjustmentButtons;
	public Image costWorthAmountImage;

    public TextMeshProUGUI costWorthAmountTMP;
    public TextMeshProUGUI acceptButtonText;

	public void setExecutor(IAcceptsAmounts executor)
	{
		amountExecutor = executor;
	}

	public void setDirectionMode(DirectionMode newMode)
	{
		directionMode = newMode;

		populate();
	}

	private void populate()
	{
		// if(directionMode == DirectionMode.BuySendTo)
		// {
		// 	acceptButtonText.text = buyButtonMessage;
        // } else
		// {
        //     acceptButtonText.text = sellButtonMessage;
        // }

		if(getMax() >= 1)
		{
			setAmount(1);
		} else
		{
			setAmount(0);
		}

		setCostWorth();
    }

	public int getMax()
	{
		if(directionMode.Equals(DirectionMode.BuySendTo))
		{
			int mostPlayerCanAfford = Purse.getCoinsInPurse() / descriptionPanel.getItemBeingDescribed().getWorth();
			int amountInStock = getItemBeingDescribed().getQuantity();

            if(mostPlayerCanAfford < amountInStock)
			{
				return mostPlayerCanAfford;
			} else
			{
				return amountInStock;
			}
		} else
		{
            return getItemBeingDescribed().getQuantity();
        }
	}

	private void setCostWorth()
	{
		costWorthAmountTMP.text = "" + Item.getTotalWorth(descriptionPanel.getItemBeingDescribed().getWorth(), currentAmount) + Purse.moneySymbol;
	}

	public void setAmountFromText()
    {
        try
        {
            int newAmount = int.Parse(amountInput.text);

			setAmount(newAmount);
        }
        catch (Exception e)
        {

        }

	}

	public void setAmount(int newAmount)
	{
		if(getMax() <= 0)
		{
			setPanelInteractability();
			currentAmount = 0;
			return;
		}

        if (newAmount <= getMax() && newAmount > 0)
        {
            currentAmount = newAmount;
        }
        else if (getMax() >= 1 && newAmount <= 0)
        {
            currentAmount = 1;
        } else if(getMax() <= 0 && newAmount <= 0)
		{
			currentAmount = 0;
		}
		else if (newAmount > getMax())
		{
			currentAmount = getMax();
		}

        amountInput.text = "" + currentAmount;

        setCostWorth();
		setPanelInteractability();
    }
	
	public void incrementAmount()
	{
		if(KeyBindingList.eitherShiftKeyIsPressed())
		{
			setAmountToMax();
        } else if(KeyBindingList.eitherControlKeyIsPressed())
		{
            setAmount(currentAmount + 10);
        } else
		{
            setAmount(currentAmount + 1);
        }
	}
	
	public void decrementAmount()
	{
        if (KeyBindingList.eitherShiftKeyIsPressed())
        {
			setAmountToOne();
        }
        else if (KeyBindingList.eitherControlKeyIsPressed())
        {
            setAmount(currentAmount - 10);
        }
        else
        {
            setAmount(currentAmount - 1);
        }
    }
	
	public void setAmountToMax()
	{
		setAmount(getMax());
	}
	
	public void setAmountToOne()
	{
		setAmount(1);
	}

    public void clipToCharacterLimit()
    {
        if (amountInput.text.Length > characterLimit)
        {
            amountInput.text = amountInput.text.Substring(0, characterLimit);
        }
    }

	public void setPanelInteractability()
    {
        if (getMax() <= 0)
        {
            foreach (Button button in adjustmentButtons)
            {
                button.interactable = false;
            }

            costWorthAmountTMP.text = "0g";
            amountInput.enabled = false;
            // costWorthAmountTMP.color = new Color(greyRGB, greyRGB, greyRGB);
            return;
        }
        else
        {
            amountInput.enabled = true;
        }

        if (getAmount() >= getMax())
        {
            adjustmentButtons[incrementButtonIndex].interactable = false;
        }
        else
        {
            adjustmentButtons[incrementButtonIndex].interactable = true;
        }

        if (getAmount() <= 1)
        {
            adjustmentButtons[decrementButtonIndex].interactable = false;
        }
        else
        {
            adjustmentButtons[decrementButtonIndex].interactable = true;
        }
    }
	
	public int getAmount()
	{
		return currentAmount;
	}
	
	private Item getItemBeingDescribed()
	{
		return (Item) descriptionPanel.getObjectBeingDescribed();
	}

	public void acceptButtonPress()
	{
		amountExecutor.acceptAmount(currentAmount, directionMode);
	}


}
