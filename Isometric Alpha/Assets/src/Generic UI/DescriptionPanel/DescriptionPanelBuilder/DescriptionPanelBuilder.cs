using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum DescriptionPanelBuildingBlockType
{
    Name = 1, Icon = 2, Text = 3, DamageText = 4, BonusDamageText = 5, Range = 6,
    DescriptionText = 7, PrimaryStat = 8, SecondaryStat = 9, Skills = 10, Exuberances = 11
};

public enum DescriptionPanelBuilderType {Standard = 0, CombatStats = 1, Stats = 2, UpgradeStatsDifference = 3, PlayerSideStats = 4, CombatActionsAndTraits = 5};


public interface IDescribableInBlocks
{
    public string getName();
    public List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks();

	public static List<IDescribableInBlocks> getRelatedBlocks(IDescribable describable)
	{
		List<IDescribableInBlocks> relatedBlocks = new List<IDescribableInBlocks>();

		ArrayList relatedDescribables = describable.getRelatedDescribables();

		foreach (IDescribable relatedDescribable in relatedDescribables)
		{
			relatedBlocks.Add(relatedDescribable as IDescribableInBlocks);
		}

		return relatedBlocks;
	}
}

public struct DescriptionPanelBuildingBlock
{
    public DescriptionPanelBuildingBlockType type;
    public string iconName;
    public string symbolCharacter;
    public string text;
    public string formula;

    public DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType type, string text)
    {
        this.type = type;

        if (type == DescriptionPanelBuildingBlockType.Icon)
        {
            this.iconName = text;
            this.text = null;
        }
        else
        {
            this.text = text;
            this.iconName = null;
        }

        this.symbolCharacter = null;
        this.formula = null;
    }

    public DescriptionPanelBuildingBlock(string formula, DescriptionPanelBuildingBlockType type, string text)
    {
        this.type = type;

        if (type == DescriptionPanelBuildingBlockType.Icon)
        {
            this.iconName = text;
            this.text = null;
        }
        else
        {
            this.text = text;
            this.iconName = null;
        }

        this.symbolCharacter = null;
        this.formula = formula;
    }

    public DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType type, string text, string iconName)
    {
        this.type = type;
        this.iconName = iconName;
        this.text = text;

        this.symbolCharacter = null;
        this.formula = null;
    }

    public DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType type, string text, char symbolChar)
    {
        this.type = type;
        this.symbolCharacter = symbolChar.ToString();
        this.text = text;

        this.iconName = null;
        this.formula = null;
    }

    public DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType type, string text, string iconName, char symbolChar)
    {
        this.type = type;
        this.symbolCharacter = symbolChar.ToString();
        this.text = text;
        this.iconName = iconName;

        this.formula = null;
    }


    public DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType type, string text, string iconName, string formula)
    {
        this.type = type;
        this.iconName = iconName;

        this.text = text;
        this.formula = formula;
        this.symbolCharacter = null;
    }

    public Sprite getIcon()
    {
        return Helpers.loadSpriteFromResources(iconName);
    }

    #region 
    public static DescriptionPanelBuildingBlock getNameBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Name, text);
    }
    public static DescriptionPanelBuildingBlock getActionTypeBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.actionTypeIconName);
    }

    public static DescriptionPanelBuildingBlock getDamageBlock(string text, string formula)
    {
        return new DescriptionPanelBuildingBlock(formula, DescriptionPanelBuildingBlockType.DamageText, text);
    }

    public static DescriptionPanelBuildingBlock getBonusDamageBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(text, DescriptionPanelBuildingBlockType.BonusDamageText, text);
    }

    public static DescriptionPanelBuildingBlock getBonusDamageBlock(string text, string formula)
    {
        return new DescriptionPanelBuildingBlock(text, DescriptionPanelBuildingBlockType.BonusDamageText, formula);
    }


    public static DescriptionPanelBuildingBlock getCritBlock(string text, string formula)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.critIconName, formula);
    }

    public static DescriptionPanelBuildingBlock getRangeBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Range, text, IconList.rangeIconName);
    }

    public static DescriptionPanelBuildingBlock getRangeBlock(int rangeIndex)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Range, Range.getRangeTitle(rangeIndex), IconList.rangeIconName);
    }

    public static DescriptionPanelBuildingBlock getCooldownBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.cooldownIconName);
    }

    public static DescriptionPanelBuildingBlock getSlotsBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.slotsIconName);
    }

    public static DescriptionPanelBuildingBlock getDurationBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.durationIconName);
    }

    //Trait Blocks

    public static DescriptionPanelBuildingBlock getTraitTypeBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.traitTypeIconName);
    }

    //Item Blocks
    public static DescriptionPanelBuildingBlock getArmorBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.armorScoreIconName);
    }

    public static DescriptionPanelBuildingBlock getAmountBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.amountIconName);
    }

    public static DescriptionPanelBuildingBlock getWorthBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.worthIconName);
    }

    public static DescriptionPanelBuildingBlock getDescriptionBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.DescriptionText, text);
    }

    //Stats Blocks

    public static DescriptionPanelBuildingBlock getHealthBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.healthIconName);
    }

    public static DescriptionPanelBuildingBlock getLevelBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.levelIconName);
    }

    public static DescriptionPanelBuildingBlock getAffinityTotalBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.affinityIconName);
    }

    public static DescriptionPanelBuildingBlock getExperienceBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.experienceIconName);
    }


    public static DescriptionPanelBuildingBlock getCharBlock(string text, string symbolChar)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.PrimaryStat, text, symbolChar[0]);
    }

    public static DescriptionPanelBuildingBlock getBlockWithFormula(DescriptionPanelBuildingBlock block, string formula)
    {
        block.formula = formula;

        return block;
    }


    #region Strength Stats

    public static DescriptionPanelBuildingBlock getStrengthBlock(string text)
    {
        return getCharBlock(text, Strength.symbolChar);
    }

    public static DescriptionPanelBuildingBlock getBonusHealthBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.bonusHealthIconName, Strength.symbolChar[0]);
    }

    public static DescriptionPanelBuildingBlock getCriticalHitDamageBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.criticalHitDamageIconName, Strength.symbolChar[0]);
    }

    public static DescriptionPanelBuildingBlock getPhysicalResistBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.physicalResistIconName, Strength.symbolChar[0]);
    }

    #endregion

    #region Dexterity Stats

    public static DescriptionPanelBuildingBlock getDexterityBlock(string text)
    {
        return getCharBlock(text, Dexterity.symbolChar);
    }

    public static DescriptionPanelBuildingBlock getExtraArmorBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.extraArmorIconName, Dexterity.symbolChar[0]);
    }

    public static DescriptionPanelBuildingBlock getSurpriseRoundDamageMultiplierBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.surpriseRoundDamageMultiplierIconName, Dexterity.symbolChar[0]);
    }

    public static DescriptionPanelBuildingBlock getArmorPenetrationBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.armorPenetrationIconName, Dexterity.symbolChar[0]);
    }

    #endregion

    #region Wisdom Stats

    public static DescriptionPanelBuildingBlock getWisdomBlock(string text)
    {
        return getCharBlock(text, Wisdom.symbolChar);
    }

    public static DescriptionPanelBuildingBlock getMentalResistBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.mentalResistIconName, Wisdom.symbolChar[0]);
    }

    public static DescriptionPanelBuildingBlock getPassiveSlotsBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.passiveSlotsIconName, Wisdom.symbolChar[0]);
    }

    public static DescriptionPanelBuildingBlock getBonusWeaponSlotsBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.bonusWeaponSlotsIconName, Wisdom.symbolChar[0]);
    }
    #endregion

    #region Charisma Stats

    public static DescriptionPanelBuildingBlock getCharismaBlock(string text)
    {
        return getCharBlock(text, Charisma.symbolChar);
    }

    public static DescriptionPanelBuildingBlock getSynergyBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.synergyIconName, Charisma.symbolChar[0]);
    }

    public static DescriptionPanelBuildingBlock getBonusExuberancesBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.allExuberancesIconName, Charisma.symbolChar[0]);
    }

    public static DescriptionPanelBuildingBlock getZOIBlock(string text, string zoiIconName)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, zoiIconName, Charisma.symbolChar[0]);
    }

    #endregion

    #region Party Stats

    public static DescriptionPanelBuildingBlock getRegenBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.regenIconName);
    }

    public static DescriptionPanelBuildingBlock getPartyActionsBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.partyActionsIconName);
    }

    public static DescriptionPanelBuildingBlock getPartySlotsBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.partySlotsIconName);
    }

    public static DescriptionPanelBuildingBlock getSurpriseRoundAmountBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.surpriseRoundAmountIconName);
    }

    public static DescriptionPanelBuildingBlock getRetreatChanceBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.retreatChanceIconName, Dexterity.symbolChar[0]);
    }

    public static DescriptionPanelBuildingBlock getDiscountBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.discountIconName);
    }

    public static DescriptionPanelBuildingBlock getVolleyBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.volleyIconName);
    }

    public static DescriptionPanelBuildingBlock getGoldMultiplierBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.goldMultiplierIconName);
    }

    #endregion

    #region Skills

    public static DescriptionPanelBuildingBlock getIntimidateBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Skills, text, IconList.intimidateIconName);
    }

    public static DescriptionPanelBuildingBlock getCunningBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Skills, text, IconList.cunningIconName);
    }

    public static DescriptionPanelBuildingBlock getObservationBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Skills, text, IconList.observationIconName);
    }

    public static DescriptionPanelBuildingBlock getLeadershipBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Skills, text, IconList.leadershipIconName);
    }

    #endregion

    #region Exuberances

    public static DescriptionPanelBuildingBlock getRedKnifeBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Exuberances, text, IconList.redKnifeIconName);
    }

    public static DescriptionPanelBuildingBlock getBlueShieldBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Exuberances, text, IconList.blueShieldIconName);
    }

    public static DescriptionPanelBuildingBlock getYellowThornBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Exuberances, text, IconList.yellowThornIconName);
    }
    
    public static DescriptionPanelBuildingBlock getGreenLeafBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Exuberances, text, IconList.greenLeafIconName);
    }

    #endregion


    #endregion

}



public class DescriptionPanelBuilder : MonoBehaviour
{
    private const int descriptionTextTopPaddingAmount = 15;

    public static UnityEvent OnFormulaSwap = new UnityEvent();

    public IDescribableInBlocks blockOrigin;
    public RectTransform[] rectTransforms;
    public Transform rowParent;
    public Transform iconParent;

    public DescriptionPanelBlockFormatter formatter;
    public IBuilderFilter filter;

    private List<DescriptionPanelRow> rows = new List<DescriptionPanelRow>();

    public DescriptionPanelBuilder nextBuilder;

    // public ScrollableUIElement scrollableUIElement;

    public void buildDescriptionPanel(IDescribableInBlocks blockOrigin)
    {
        buildDescriptionPanel(blockOrigin, null);
    }

    public void buildDescriptionPanel(IDescribableInBlocks blockOrigin, BlockFormat format)
    {
        this.blockOrigin = blockOrigin;

        if (format != null && formatter != null)
        {
            formatter.setFormat(format);
        }

        addToAdditionalBuilders(blockOrigin, format);

        List<DescriptionPanelBuildingBlock> buildingBlocks = blockOrigin.getDescriptionBuildingBlocks();

        foreach (DescriptionPanelBuildingBlock block in buildingBlocks)
        {
            if (filter != null && !filter.blockPassesFilter(block))
            {
                continue;
            }

            rows.Add(buildRow(block));
        }

        if (iconParent != null)
        {
            iconParent.SetAsLastSibling();
        }

        rebuildLayouts();

        StartCoroutine(waitAndUpdateGameObjectPosition());
    }

    private IEnumerator waitAndUpdateGameObjectPosition()
    {
        if (rowParent == null)
        {
            yield break;
        }

        yield return new WaitForEndOfFrame();
        Helpers.updateGameObjectPosition(rowParent.gameObject);
    }

    public virtual DescriptionPanelRow buildRow(DescriptionPanelBuildingBlock block)
    {

        if (getParent(block) == null)
        {
            return null;
        }

        DescriptionPanelRow row = Instantiate(getDescriptionPanelRowGameObject(block.type), getParent(block)).GetComponent<DescriptionPanelRow>();

        if (block.type == DescriptionPanelBuildingBlockType.Icon)
        {
            iconParent.gameObject.SetActive(true);
        }

        if (block.iconName != null)
        {
            row.setIcon(block.getIcon());

            row.setIconHoverText(HoverMessageList.getMessage(block.iconName));
        }
        else if (block.symbolCharacter != null)
        {
            row.setIcon(block.symbolCharacter);

            row.setIconHoverText(HoverMessageList.getMessage(block.symbolCharacter));
        }

        if (hasFormatToFollow() && formatter.format.hasSizeParams())
        {
            row.setIconSize(formatter.format.iconSizeParams.x, formatter.format.iconSizeParams.y);
        }

        if (block.text != null)
        {
            if (hasFormatToFollow() && formatter.format.hasFontSizeParams())
            {
                row.setText(block.text, formatter.format.fontsize);
            }
            else
            {
                row.setText(block.text);
            }
        }

        if (hasFormatToFollow() && formatter.format.hasSpacingSizeParams())
        {
            row.setLayoutGroupSpacing(formatter.format.spaceBetweenIconAndText);
        }

        if (block.formula != null)
        {
            row.setStatTotalAndFormula(block.text, block.formula);
        }

        if (block.type == DescriptionPanelBuildingBlockType.DescriptionText)
        {
            row.GetComponent<HorizontalOrVerticalLayoutGroup>().padding.top = descriptionTextTopPaddingAmount;
        }

        if (hasFormatToFollow())
        {
            formatter.applyFormat(row);
        }

        if (PlayerOOCStateManager.currentActivity == OOCActivity.inTutorialSequence)
        {
            TutorialSequenceStepTargetUIObject tutorialObject = row.gameObject.AddComponent<TutorialSequenceStepTargetUIObject>();

            tutorialObject.tutorialHash = TutorialSequenceList.getDescriptionPanelRowTutorialHash(block);

            TutorialSequenceStepTargetObject.addToHashDictionary(tutorialObject);
        }

        row.setBlockType(block.type);

        return row;
    }

    public void destroyRows()
    {
        foreach (DescriptionPanelRow row in rows)
        {
            if (row == null)
            {
                continue;
            }

            DestroyImmediate(row.gameObject);
        }

        rows = new List<DescriptionPanelRow>();
    }

    private bool hasFormatToFollow()
    {
        return formatter != null && formatter.format != null;
    }

    public virtual Transform getParent(DescriptionPanelBuildingBlock block)
    {
        switch (block.type)
        {
            case DescriptionPanelBuildingBlockType.Icon:
                return iconParent;
            default:
                return rowParent;
        }
    }

    private void addToAdditionalBuilders(IDescribableInBlocks blockOrigin, BlockFormat format)
    {
        if (nextBuilder == null)
        {
            return;
        }

        nextBuilder.buildDescriptionPanel(blockOrigin, format);
    }

    public virtual GameObject getDescriptionPanelRowGameObject(DescriptionPanelBuildingBlockType type)
    {
        switch (type)
        {
            case DescriptionPanelBuildingBlockType.Name:
                return Resources.Load<GameObject>(PrefabNames.descriptionPanelBuildingBlockName);
            case DescriptionPanelBuildingBlockType.Icon:
                return Resources.Load<GameObject>(PrefabNames.descriptionPanelBuildingBlockIcon);
            case DescriptionPanelBuildingBlockType.PrimaryStat:
                return Resources.Load<GameObject>(PrefabNames.descriptionPanelBuildingBlockPrimaryStat);
            case DescriptionPanelBuildingBlockType.SecondaryStat:
            case DescriptionPanelBuildingBlockType.DescriptionText:
            case DescriptionPanelBuildingBlockType.Text:
                return Resources.Load<GameObject>(PrefabNames.descriptionPanelBuildingBlockText);
            case DescriptionPanelBuildingBlockType.Range:
                return Resources.Load<GameObject>(PrefabNames.descriptionPanelBuildingBlockRange);
            case DescriptionPanelBuildingBlockType.DamageText:
                return Resources.Load<GameObject>(PrefabNames.descriptionPanelBuildingBlockDamageText);
            case DescriptionPanelBuildingBlockType.BonusDamageText:
                return Resources.Load<GameObject>(PrefabNames.descriptionPanelBuildingBlockBonusDamageText);
            default:
                return Resources.Load<GameObject>(PrefabNames.descriptionPanelBuildingBlockName);
        }
    }

    public static GameObject getDescriptionPanelBuilder(Transform parent)
    {
        return getDescriptionPanelBuilder(DescriptionPanelBuilderType.Standard, parent);
    }

    public static GameObject getDescriptionPanelBuilder(DescriptionPanelBuilderType builderType, Transform parent)
    {
        switch (builderType)
        {
            case DescriptionPanelBuilderType.CombatStats:
                return Instantiate(Resources.Load<GameObject>(PrefabNames.combatStatsHoverDescriptionPanelBuilder), parent);
            case DescriptionPanelBuilderType.Stats:
                return Instantiate(Resources.Load<GameObject>(PrefabNames.statsDescriptionPanelBuilder), parent);
            case DescriptionPanelBuilderType.UpgradeStatsDifference:
                return Instantiate(Resources.Load<GameObject>(PrefabNames.statsUpgradeDescriptionPanelBuilder), parent);
            case DescriptionPanelBuilderType.PlayerSideStats:
                return Instantiate(Resources.Load<GameObject>(PrefabNames.playerSideStatsDescriptionPanelBuilder), parent);
            case DescriptionPanelBuilderType.CombatActionsAndTraits:
                return Instantiate(Resources.Load<GameObject>(PrefabNames.combatActionHoverDescriptionPanelBuilder), parent);
            default:
                return Instantiate(Resources.Load<GameObject>(PrefabNames.descriptionPanelBuilder), parent);
        }
    }

    public void rebuildLayouts()
    {
        Canvas.ForceUpdateCanvases();

        foreach (RectTransform rectTranform in rectTransforms)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTranform);
        }
    }

}

public static class IconList
{

    //Action Icons
    public const string actionTypeIconName = "ActionType";
    public const string traitTypeIconName = "TraitType";
    public const string armorTypeIconName = "ArmorType";
    public const string typeIconName = "Type";
    public const string critIconName = "Crit";
    public const string rangeIconName = "Range";
    public const string cooldownIconName = "Cooldown";
    public const string slotsIconName = "Slots";
    public const string durationIconName = "Duration";
    public const string stanceWeaponIconName = "Stance Weapon";

    //Item Icons
    public const string armorScoreIconName = "Armor Score";
    public const string amountIconName = "Amount";
    public const string worthIconName = "Worth";
    public const string junkIconName = "Junk";

    //Stats Icons
    public const string healthIconName = "Health";
    public const string levelIconName = "Level";
    public const string affinityIconName = "Affinity";
    public const string experienceIconName = "XP";

    public const string intimidateIconName = "Intimidate";
    public const string bonusHealthIconName = "Bonus Health";
    public const string criticalHitDamageIconName = "Crit Dam Mult";
    public const string physicalResistIconName = "PhysicalResist";
    public const string regenIconName = "Minor Regeneration";
    public const string cunningIconName = "Cunning";
    public const string extraArmorIconName = "Extra Armor";
    public const string surpriseRoundDamageMultiplierIconName = "Surprise Round Damage";
    public const string surpriseRoundAmountIconName = "Surprise Round Amount";
    public const string armorPenetrationIconName = "ArmorPenetration";
    public const string observationIconName = "Observation";
    public const string mentalResistIconName = "MentalResist";
    public const string retreatChanceIconName = "RetreatChance";
    public const string passiveSlotsIconName = "Bonus Slots";
    public const string bonusWeaponSlotsIconName = "Bonus Weapon Slots";
    public const string synergyIconName = "Synergy";
    public const string leadershipIconName = "Leadership";
    public const string affinityMultiplierIconName = "AffinityMultiplier";
    public const string partySlotsIconName = "Party Slots";
    public const string partyActionsIconName = "Party Actions";
    public const string goldMultiplierIconName = "GoldMultiplier";


    public const string allExuberancesIconName = "All Exuberances";
    public const string redKnifeIconName = "Red Knife";
    public const string blueShieldIconName = "Blue Shield";
    public const string yellowThornIconName = "Yellow Thorn";
    public const string greenLeafIconName = "Green Leaf";

    public const string discountIconName = "Discount";
    public const string volleyIconName = "Volley";
}
