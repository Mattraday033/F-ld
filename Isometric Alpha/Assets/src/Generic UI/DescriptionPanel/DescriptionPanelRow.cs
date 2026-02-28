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
    public LayoutElement descriptionTextLayoutElement;

    public GameObject plusButton;

    public void setBlockType(DescriptionPanelBuildingBlockType type)
    {
        this.type = type;

        if (plusButton != null &&
            type == DescriptionPanelBuildingBlockType.PrimaryStat &&
            !CombatStateManager.inCombat)
        {
            // yield return new WaitForEndOfFrame();

            if (OverallUIManager.currentScreenManager != null &&
                OverallUIManager.lastScreenType == ScreenType.Character && 
                CharacterScreen.levelUpCapable())
            {
                plusButton.SetActive(true);
            }
        } else
        {
            if(descriptionTextLayoutElement != null)
            {
                descriptionTextLayoutElement.preferredWidth = 55;
            }
        }

        // StartCoroutine(setPlusButtonVisibility());
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

        DescriptionPanel.setTextAutoSize(descriptionText, OverallUIManager.showFormula);
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
        iconSymbolText.text = symbol;

        if(iconHover != null)
        {
            iconHover.hoverMessageKey = symbol;
        }

        if(symbol.Equals(Dexterity.symbolChar))
        {
            iconSymbolText.margin = new Vector4(iconSymbolText.fontSize/10f + 1f, iconSymbolText.margin.y, iconSymbolText.margin.z, iconSymbolText.margin.w);
        }

        setIconHoverText(HoverMessageList.getMessage(symbol));
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
        descriptionText.color = ColorList.grey25;
    }

    public void setText(string text)
    {
        descriptionText.text = text;
        descriptionText.color = ColorList.grey25;
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
                CharacterScreen.levelUpCapable())
            {
                plusButton.SetActive(true);
            }
        }
    }

    public void centerText()
    {
        if(descriptionText == null)
        {
            return;
        }

        descriptionText.horizontalAlignment = HorizontalAlignmentOptions.Center;
        descriptionText.margin = Vector4.zero;
    }

    public void setDescriptionTextColor(Color? color)
    {
        if(descriptionText != null && color != null)
        {
            descriptionText.color = (Color) color;
        }
    }

}
