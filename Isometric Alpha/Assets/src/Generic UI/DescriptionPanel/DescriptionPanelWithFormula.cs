using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionPanelWithFormula : DescriptionPanel
{

    private string damageTotal = "";
    private string critTotal = "";
    private string armorTotal = "";
    private string damageFormula;
    private string critFormula;
    private string armorFormula;
    private bool hasListener = false;
    private bool hasFormula = false;

    private float oldFontSize = 28f;
    private const float fontSizeModifier = .75f;

    private void OnDestroy()
    {
        if (hasListener)
        {
            DescriptionPanelBuilder.OnFormulaSwap.RemoveListener(swapStatText);
        }
    }

    public override void setObjectBeingDescribed(IDescribable describable)
    {
        base.setObjectBeingDescribed(describable);

        IFormulaSource formulaSource = describable as IFormulaSource;

        if (formulaSource != null)
        {
            damageFormula = formulaSource.getDamageFormula().Replace(" ", "");
            critFormula = formulaSource.getCritFormula().Replace(" ", "");
            armorFormula = formulaSource.getArmorFormula().Replace(" ", "");

            DescriptionPanelBuilder.OnFormulaSwap.AddListener(swapStatText);
            hasListener = true;
            hasFormula = true;
        }

        setColumnVisibility();
    }

    private void setColumnVisibility()
    {
        Item item = getObjectBeingDescribed() as Item;

        if (item == null)
        {
            return;
        }

        switch (item.getSubtype())
        {
            case Armor.subtype:
                damageText.gameObject.SetActive(false);
                critRatingText.gameObject.SetActive(false);
                armorRatingText.gameObject.SetActive(true);
                return;
            case Weapon.subtype:
                damageText.gameObject.SetActive(true);
                critRatingText.gameObject.SetActive(true);
                armorRatingText.gameObject.SetActive(false);
                return;
            default:
                damageText.gameObject.SetActive(false);
                critRatingText.gameObject.SetActive(false);
                armorRatingText.gameObject.SetActive(false);
                return;
        }
    }

    private void swapStatText()
    {
        if (!hasFormula)
        {
            return;
        }

        if (OverallUIManager.showFormula)
        {
            if (damageTotal == null || damageTotal.Length <= 0)
            {
                damageTotal = damageText.text;
                oldFontSize = damageText.fontSize;
            }

            if (critTotal == null || critTotal.Length <= 0)
            {
                critTotal = critRatingText.text;
                oldFontSize = critRatingText.fontSize;
            }

            if (armorTotal == null || armorTotal.Length <= 0)
            {
                armorTotal = armorRatingText.text;
                oldFontSize = armorRatingText.fontSize;
            }

            DescriptionPanel.setText(damageText, damageFormula);
            DescriptionPanel.setText(critRatingText, critFormula);
            DescriptionPanel.setText(armorRatingText, armorFormula);

            DescriptionPanel.adjustTextFontSize(damageText, fontSizeModifier);
            DescriptionPanel.adjustTextFontSize(critRatingText, fontSizeModifier);
            DescriptionPanel.adjustTextFontSize(armorRatingText, fontSizeModifier);

        }
        else
        {
            DescriptionPanel.setText(damageText, damageTotal);
            DescriptionPanel.setText(critRatingText, critTotal);
            DescriptionPanel.setText(armorRatingText, armorTotal);

            DescriptionPanel.setTextFontSize(damageText, oldFontSize);
            DescriptionPanel.setTextFontSize(critRatingText, oldFontSize);
            DescriptionPanel.setTextFontSize(armorRatingText, oldFontSize);
        }
    }

}
