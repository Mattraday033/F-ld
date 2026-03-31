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
    private string invulnerabilityTotal = "";
    private string damageFormula;
    private string critFormula;
    private string armorFormula;
    private string invulnerabilityFormula;
    private bool hasListener = false;
    private bool hasFormula = false;

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

        StatBoostSource formulaSource = describable as StatBoostSource;

        if (formulaSource != null)
        {
            damageFormula = formulaSource.getDamageFormula().Replace(" ", "");
            critFormula = formulaSource.getCritFormula().Replace(" ", "");
            armorFormula = formulaSource.getArmorFormula().Replace(" ", "");
            invulnerabilityFormula = formulaSource.getInvulnerableFormula().Replace(" ", "");

            DescriptionPanelBuilder.OnFormulaSwap.AddListener(swapStatText);
            hasListener = true;
            hasFormula = true;
        }

        setColumnVisibility();
    }

    private void setColumnVisibility()
    {
        Item item = getObjectBeingDescribed() as Item;

        if (item == null || 
            damageText == null || 
            critRatingText == null || 
            armorRatingText == null|| 
            invulnerabilityText == null)
        {
            return;
        }

        string subtype = item.getSubtype();

        if(item as OffHandWeapon != null || item as Shield != null)
        {
            subtype = Weapon.subtype;
        }

        switch (subtype)
        {
            case Armor.subtype:
                damageText.gameObject.SetActive(false);
                critRatingText.gameObject.SetActive(false);
                armorRatingText.gameObject.SetActive(true);
                invulnerabilityText.gameObject.SetActive(true);
                return;
            case Weapon.subtype:
                damageText.gameObject.SetActive(true);
                critRatingText.gameObject.SetActive(true);
                armorRatingText.gameObject.SetActive(false);
                invulnerabilityText.gameObject.SetActive(false);
                return;
            default:
                damageText.gameObject.SetActive(false);
                critRatingText.gameObject.SetActive(false);
                armorRatingText.gameObject.SetActive(false);
                invulnerabilityText.gameObject.SetActive(false);
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
            if ((damageTotal == null || damageTotal.Length <= 0) && damageText != null)
            {
                damageTotal = damageText.text;
            }

            if ((critTotal == null || critTotal.Length <= 0) && critRatingText != null)
            {
                critTotal = critRatingText.text;
            }

            if ((armorTotal == null || armorTotal.Length <= 0)&& armorRatingText != null)
            {
                armorTotal = armorRatingText.text;
            }

            if ((invulnerabilityTotal == null || invulnerabilityTotal.Length <= 0)&& invulnerabilityText != null)
            {
                invulnerabilityTotal = invulnerabilityText.text;
            }


            setText(damageText, damageFormula);
            setText(critRatingText, critFormula);
            setText(armorRatingText, armorFormula);
            setText(invulnerabilityText, invulnerabilityFormula);

            // adjustTextFontSize(damageText, fontSizeModifier);
            // adjustTextFontSize(critRatingText, fontSizeModifier);
            // adjustTextFontSize(armorRatingText, fontSizeModifier);

        }
        else
        {
            setText(damageText, damageTotal);
            setText(critRatingText, critTotal);
            setText(armorRatingText, armorTotal);
            setText(invulnerabilityText, invulnerabilityTotal);

            // setTextFontSize(damageText, oldFontSize);
            // setTextFontSize(critRatingText, oldFontSize);
            // setTextFontSize(armorRatingText, oldFontSize);
        }

        setTextAutoSize(damageText, OverallUIManager.showFormula);
        setTextAutoSize(critRatingText, OverallUIManager.showFormula);
        setTextAutoSize(armorRatingText, OverallUIManager.showFormula);
        setTextAutoSize(invulnerabilityText, OverallUIManager.showFormula);
    }

}