using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionPanelRow : MonoBehaviour
{

    public DescriptionPanelBuildingBlockType type;

    private string statTotal;
    private string statFormula;
    private bool hasListener = false;
    private bool hasFormula = false;


    public GameObject iconObject;
    public Image iconImage;
    public Image iconOutline;
    public Image iconBackground;
    public LayoutElement iconLayoutElement;

    public HorizontalLayoutGroup layoutGroup;

    public TextMeshProUGUI iconSymbolText;
    public SlotIconHover iconHover;
    public TextMeshProUGUI descriptionText;

    public GameObject plusButton;

    public void setBlockType(DescriptionPanelBuildingBlockType type)
    {
        this.type = type;

        StartCoroutine(setPlusButtonVisibility());
    }

    private void OnDestroy()
    {
        if (hasListener)
        {
            DescriptionPanelBuilder.OnFormulaSwap.RemoveListener(swapStatText);
        }
    }

    public void setStatTotalAndFormula(string statTotal, string statFormula)
    {
        this.statTotal = statTotal;
        this.statFormula = statFormula;

        DescriptionPanelBuilder.OnFormulaSwap.AddListener(swapStatText);
        hasListener = true;
        hasFormula = true;
    }

    private void swapStatText()
    {
        if (!hasFormula)
        {
            return;
        }

        if (OverallUIManager.showFormula)
        {
            descriptionText.text = statFormula;
        }
        else
        {
            descriptionText.text = statTotal;
        }
    }

    public void setIcon(Sprite sprite)
    {
        if (iconObject == null || iconImage == null)
        {
            return;
        }

        iconObject.SetActive(true);
        iconImage.enabled = true;
        iconImage.sprite = sprite;

        if (iconSymbolText != null)
        {
            iconSymbolText.enabled = false;
        }
    }

    public void setIcon(string symbol)
    {
        if (iconObject == null || iconSymbolText == null)
        {
            return;
        }

        iconObject.SetActive(true);
        iconImage.gameObject.SetActive(false);
        iconImage.enabled = false;
        iconSymbolText.gameObject.SetActive(true);
        iconSymbolText.enabled = true;
        iconSymbolText.text = symbol.ToString();

        setIconHoverText(HoverMessageList.getMessage(symbol.ToString()));
    }

    public void setIconSize(int sizeX, int sizeY)
    {
        if (iconLayoutElement != null)
        {
            iconLayoutElement.preferredWidth = sizeX;
            iconLayoutElement.preferredHeight = sizeY;
        }
    }

    public void setIconOutlineColor(Color color)
    {
        DescriptionPanel.setImageColor(iconOutline, color);
    }

    public void setIconBackgroundColor(Color color)
    {
        DescriptionPanel.setImageColor(iconBackground, color);
    }

    public void setIconHoverText(string text)
    {
        if (iconHover == null)
        {
            return;
        }

        iconHover.setHoverMessage(text);
    }

    public void setText(string text, int fontSize)
    {
        descriptionText.text = text;
        descriptionText.fontSize = fontSize;
    }

    public void setText(string text)
    {
        descriptionText.text = text;
    }

    public void setLayoutGroupSpacing(int spacing)
    {
        if (layoutGroup != null)
        {
            layoutGroup.spacing = spacing;
        }
    }

    public void flipDirection()
    {
        layoutGroup.reverseArrangement = true;
        descriptionText.horizontalAlignment = HorizontalAlignmentOptions.Right;
    }

    private IEnumerator setPlusButtonVisibility()
    {
        if (plusButton != null &&
            type == DescriptionPanelBuildingBlockType.PrimaryStat &&
            !CombatStateManager.inCombat)
        {
            yield return new WaitForEndOfFrame();

            if (OverallUIManager.currentScreenManager != null &&
                OverallUIManager.currentScreenManager.levelUpCapable())
            {
                plusButton.SetActive(true);
            }
        }
    }

}
