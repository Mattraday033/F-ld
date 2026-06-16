using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum DescriptionPanelBuildingBlockType
{
    Name = 1, Icon = 2, Text = 3, DamageText = 4, BonusDamageText = 5, Range = 6,
    DescriptionText = 7, PrimaryStat = 8, SecondaryStat = 9, Skills = 10, Exuberances = 11, Item = 12
};

public enum DescriptionPanelBuilderType {Standard = 0, CombatStats = 1, Stats = 2, UpgradeStatsDifference = 3, PlayerSideStats = 4, CombatActionsAndTraits = 5};


public interface IDescribableInBlocks : INameSource
{
    public List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks();

    public bool requiresInspectNode();

	public static List<IDescribableInBlocks> getRelatedDescribableInBlocks(IDescribableInBlocks blockOrigin)
	{
		List<IDescribableInBlocks> relatedBlocks = new List<IDescribableInBlocks>();

        if(blockOrigin as IDescribable != null)
        {
            IDescribable describable = blockOrigin as IDescribable;

            List<IDescribable> relatedDescribables = describable.getRelatedDescribables();

            foreach(IDescribable relatedDescribable in relatedDescribables)
            {
                if(relatedDescribable as IDescribableInBlocks != null)
                {
                    relatedBlocks.Add(relatedDescribable as IDescribableInBlocks);
                }
            }
        }

		return relatedBlocks;
	}

	public static List<IDescribableInBlocks> getRelatedBlocks(IDescribable describable)
	{
		List<IDescribableInBlocks> relatedBlocks = new List<IDescribableInBlocks>();

		List<IDescribable> relatedDescribables = describable.getRelatedDescribables();

		foreach (IDescribable relatedDescribable in relatedDescribables)
		{
			relatedBlocks.Add(relatedDescribable as IDescribableInBlocks);
		}

		return relatedBlocks;
	}
}

public class DescriptionPanelBuildingBlock
{
    public DescriptionPanelBuildingBlockType type;
    public string iconName;
    public string symbolCharacter;
    public string text;
    public string formula;
    public Item item;

    public DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType type, string text = null, string iconName = null, string formula = null, char symbolChar = ' ')
    {
        this.type = type;

        this.text = text;
        this.iconName = iconName;

        if(symbolChar == ' ')
        {
            this.symbolCharacter = null;
        } else
        {
            this.symbolCharacter = symbolChar + "";
        }

        this.formula = formula;
        this.item = null;
    }

    public DescriptionPanelBuildingBlock(Item item)
    {
        this.type = DescriptionPanelBuildingBlockType.Item;
        this.iconName = null;

        this.text = null;
        this.formula = null;
        this.symbolCharacter = null;
        this.item = item;
    }
    public virtual Sprite getIcon()
    {
        return Helpers.loadSpriteFromResources(iconName);
    }

    #region 
    public static DescriptionPanelBuildingBlock getNameBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Name, text: text);
    }
    public static DescriptionPanelBuildingBlock getActionTypeBlock(string text, string iconName)
    {
        return new DescriptionPanelTypeBuildingBlock(text, iconName);
    }

    public static DescriptionPanelBuildingBlock getDamageBlock(string text, string formula)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.DamageText, text: text,formula: formula);
    }

    public static DescriptionPanelBuildingBlock getBonusDamageBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.BonusDamageText, text: text, formula: text);
    }

    public static DescriptionPanelBuildingBlock getBonusDamageBlock(string text, string formula)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.BonusDamageText, text: text, formula: formula);
    }

    public static DescriptionPanelBuildingBlock getCritBlock(string text, string formula)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.critIconName, formula);
    }

    public static DescriptionPanelBuildingBlock getInvulnerableBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.invulnerableIconName);
    }

    public static DescriptionPanelBuildingBlock getInvulnerableBlock(string text, string formula)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.invulnerableIconName, formula);
    }

    public static DescriptionPanelBuildingBlock getVulnerableBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.vulnerableIconName);
    }

    public static DescriptionPanelBuildingBlock getVulnerableBlock(string text, string formula)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.vulnerableIconName, formula);
    }

    public static DescriptionPanelBuildingBlock getHealingBoostBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.healingBoostIconName);
    }

    public static DescriptionPanelBuildingBlock getHealingBoostBlock(string text, string formula)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.healingBoostIconName, formula);
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

    public static DescriptionPanelBuildingBlock getTraitTypeBlock(string text, string iconName)
    {
        return new DescriptionPanelTypeBuildingBlock(text, iconName);
    }

    //Item Blocks
    public static DescriptionPanelBuildingBlock getArmorBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.armorScoreIconName);
    }

    public static DescriptionPanelBuildingBlock getArmorBlock(string text, string formula)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.armorScoreIconName, formula);
    }

    public static DescriptionPanelBuildingBlock getArmorShredBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.armorShredIconName);
    }

    public static DescriptionPanelBuildingBlock getArmorShredBlock(string text, string formula)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.armorShredIconName, formula);
    }

    public static DescriptionPanelBuildingBlock getAmountBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.amountIconName);
    }

    public static DescriptionPanelBuildingBlock getWorthBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.worthIconName);
    }

    public static DescriptionPanelBuildingBlock getPartyGoldBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, text, IconList.partyGoldIconName);
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

    public static DescriptionPanelBuildingBlock getHealthBlock(int currentHealth)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, currentHealth.ToString(), IconList.healthIconName);
    }

    public static DescriptionPanelBuildingBlock getHealthBlock(string currentHealth, string totalHealth)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text,  currentHealth + "/" + totalHealth, IconList.healthIconName);
    }

    public static DescriptionPanelBuildingBlock getHealthBlock(int currentHealth, int totalHealth)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, currentHealth + "/" + totalHealth, IconList.healthIconName);
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
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.PrimaryStat, text, symbolChar: symbolChar[0]);
    }

    public static DescriptionPanelBuildingBlock getBlockWithFormula(DescriptionPanelBuildingBlock block, string formula)
    {
        block.formula = formula;

        return block;
    }


    #region Strength Stats

    public static DescriptionPanelBuildingBlock getStrengthBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.PrimaryStat, text, iconName: IconList.strengthIconName);
    }

    public static DescriptionPanelBuildingBlock getBonusHealthBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.bonusHealthIconName, symbolChar: Strength.symbolChar[0]);
    }

    public static DescriptionPanelBuildingBlock getCriticalHitDamageBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.criticalHitDamageIconName, symbolChar: Strength.symbolChar[0]);
    }

    public static DescriptionPanelBuildingBlock getWoundResistBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.woundResistIconName, symbolChar: Strength.symbolChar[0]);
    }

    #endregion

    #region Dexterity Stats

    public static DescriptionPanelBuildingBlock getDexterityBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.PrimaryStat, text, iconName: IconList.dexterityIconName);
    }

    public static DescriptionPanelBuildingBlock getExtraArmorBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.bonusArmorIconName, symbolChar: Dexterity.symbolChar[0]);
    }

    public static DescriptionPanelBuildingBlock getSurpriseRoundDamageMultiplierBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.surpriseRoundDamageMultiplierIconName, symbolChar: Dexterity.symbolChar[0]);
    }

    public static DescriptionPanelBuildingBlock getArmorPenetrationBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.armorPenetrationIconName, symbolChar: Dexterity.symbolChar[0]);
    }

    #endregion

    #region Wisdom Stats

    public static DescriptionPanelBuildingBlock getWisdomBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.PrimaryStat, text, iconName: IconList.wisdomIconName);
    }

    public static DescriptionPanelBuildingBlock getMentalResistBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.mentalResistIconName, symbolChar: Wisdom.symbolChar[0]);
    }

    public static DescriptionPanelBuildingBlock getPassiveSlotsBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.passiveSlotsIconName, symbolChar: Wisdom.symbolChar[0]);
    }

    public static DescriptionPanelBuildingBlock getBonusWeaponSlotsBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.weaponSlotsIconName, symbolChar: Wisdom.symbolChar[0]);
    }
    #endregion

    #region Charisma Stats

    public static DescriptionPanelBuildingBlock getCharismaBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.PrimaryStat, text, iconName: IconList.charismaIconName);
    }

    public static DescriptionPanelBuildingBlock getSynergyBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.synergyIconName, symbolChar: Charisma.symbolChar[0]);
    }

    public static DescriptionPanelBuildingBlock getBonusExuberancesBlock(string text)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.allExuberancesIconName, symbolChar: Charisma.symbolChar[0]);
    }

    public static DescriptionPanelBuildingBlock getZOIBlock(string text, string zoiIconName)
    {
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, zoiIconName, symbolChar: Charisma.symbolChar[0]);
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
        return new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text, IconList.retreatChanceIconName, symbolChar: Dexterity.symbolChar[0]);
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

public class DescriptionPanelTypeBuildingBlock : DescriptionPanelBuildingBlock
{

    public DescriptionPanelTypeBuildingBlock(string text, string iconName) :
    base(DescriptionPanelBuildingBlockType.Text, text: text, iconName: iconName)
    {
        
    }

    public override Sprite getIcon()
    {
        return Helpers.loadSpriteFromResources(iconName);
    }
}


public class DescriptionPanelBuilder : MonoBehaviour
{
    private const int descriptionTextTopPaddingAmount = 15;
    private const int maxRowCount = 3;

    public readonly static UnityEvent OnFormulaSwap = new UnityEvent();

    public IDescribableInBlocks blockOrigin;
    public RectTransform[] rectTransforms;
    public Transform rowParent;
    public Transform iconParent;

    public ContentSizeFitter fitter;

    public DescriptionPanelBlockFormatter formatter;
    public IBuilderFilter filter;

    public GridLayoutGroup statGridLayout;

    private List<DescriptionPanelRow> rows = new List<DescriptionPanelRow>();

    public DescriptionPanelBuilder nextBuilder;

    public bool inspectNodesAllowed = true;

    // public ScrollableUIElement scrollableUIElement;

    public void buildDescriptionPanel(IDescribableInBlocks blockOrigin)
    {
        buildDescriptionPanel(blockOrigin, null);
    }

    public virtual void buildDescriptionPanel(IDescribableInBlocks blockOrigin, BlockFormat format)
    {
        this.blockOrigin = blockOrigin;

        if (format != null && formatter != null)
        {
            formatter.setFormat(format);
        } else if(formatter != null && formatter.formatOverride != BlockFormatType.None)
        {
            formatter.setFormat(BlockFormat.getBlockFormat(formatter.formatOverride));
        } else if(CombatStateManager.inCombat && format == null && formatter != null)
        {
            formatter.setFormat(BlockFormat.getBlockFormat(BlockFormatType.CombatHover));
        }

        addToAdditionalBuilders(blockOrigin, format);

        destroyRows();

        List<DescriptionPanelBuildingBlock> buildingBlocks = blockOrigin.getDescriptionBuildingBlocks();

        foreach (DescriptionPanelBuildingBlock block in buildingBlocks)
        {
            if (filter != null && !filter.blockPassesFilter(block))
            {
                continue;
            }

            rows.Add(buildRow(block));
        }

        if(statGridLayout != null && rowParent.childCount >= maxRowCount)
        {
            statGridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            statGridLayout.constraintCount = maxRowCount;
        }

        if(blockOrigin.requiresInspectNode() && inspectNodesAllowed)
        {
            activateInspectNode();
            setFitterToPreferredSize();

        } else if(CombatStateManager.inCombat)
        {
            setFitterToPreferredSize();
        }

        if(!inspectNodesAllowed)
        {
            deactivateInspectNode();
        }

        rebuildLayouts();

        StartCoroutine(waitAndUpdateGameObjectPosition());
    }

    private void setFitterToPreferredSize()
    {
        if(fitter != null)
        {
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        if(fitter != null && CombatStateManager.inCombat)
        {
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
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
        Transform blockParent = getParent(block);

        if (blockParent == null)
        {
            return null;
        }

        DescriptionPanelRow row = Instantiate(getDescriptionPanelRowGameObject(block.type), blockParent).GetComponent<DescriptionPanelRow>();

        blockParent.gameObject.SetActive(true);

        // switch(block.type)
        // {
        //     case DescriptionPanelBuildingBlockType.Icon:
        //         iconParent.gameObject.SetActive(true);
        //         break;
        // }

        if (block.iconName != null)
        {
            row.setIcon(block.getIcon());

            row.setIconHoverText(block.iconName, HoverMessageList.getMessage(block.iconName));
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
            if (hasFormatToFollow() && formatter.format.hasFontSizeParams() && block.type != DescriptionPanelBuildingBlockType.Name)
            {
                row.setText(block.text, formatter.format.fontSize);
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
        } else if(block.type == DescriptionPanelBuildingBlockType.Name)
        {
            row.transform.SetAsFirstSibling();

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

    public virtual void activateInspectNode()
    {
        //empty on purpose
    }

    public virtual void deactivateInspectNode()
    {
        //empty on purpose
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
            case DescriptionPanelBuildingBlockType.Item:
                return Resources.Load<GameObject>(PrefabNames.descriptionPanelBuildingBlockItem);
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
            if(rectTranform == null)
            {
                continue;
            }

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
    public const string stanceIconName = "Stance";

    //Item Icons
    public const string armorScoreIconName = "Armor Score";
    public const string amountIconName = "Amount";
    public const string worthIconName = "Worth";
    public const string partyGoldIconName = "Total Party Gold";
    public const string junkIconName = "Junk";

    //Stats Icons
    public const string strengthIconName = "Strength";
    public const string dexterityIconName = "Dexterity";
    public const string wisdomIconName = "Wisdom";
    public const string charismaIconName = "Charisma";

    public const string healthIconName = "Health";
    public const string levelIconName = "Level";
    public const string affinityIconName = "Affinity";
    public const string experienceIconName = "XP";

    public const string invulnerableIconName = "Invulnerability";
    public const string vulnerableIconName = "Vulnerability";

    public const string healingBoostIconName = "Healing Boost";

    public const string intimidateIconName = "Intimidate";
    public const string bonusHealthIconName = "Bonus Health";
    public const string criticalHitDamageIconName = "Crit Damage Multiplier";
    public const string woundResistIconName = "Wound Resist";
    public const string regenIconName = "Regeneration";
    public const string cunningIconName = "Cunning";
    public const string bonusArmorIconName = "Bonus Armor";
    public const string armorShredIconName = "Armor Shred";
    public const string surpriseRoundDamageMultiplierIconName = "Surprise Round Damage";
    public const string surpriseRoundAmountIconName = "Surprise Round Duration";
    public const string armorPenetrationIconName = "Armor Penetration";
    public const string observationIconName = "Observation";
    public const string mentalResistIconName = "Mental Resist";
    public const string retreatChanceIconName = "Retreat Chance";
    public const string passiveSlotsIconName = "Passive Slots";
    public const string weaponSlotsIconName = "Weapon Slots";
    public const string synergyIconName = "Synergy";
    public const string leadershipIconName = "Leadership";
    public const string partySlotsIconName = "Party Slots";
    public const string partyActionsIconName = "Party Actions";
    public const string goldMultiplierIconName = "Gold Multiplier";
    public const string ZOIIconName = "Zone of Influence";


    public const string allExuberancesIconName = "Starting Exuberances";
    public const string redKnifeIconName = "Red Knife";
    public const string blueShieldIconName = "Blue Shield";
    public const string yellowThornIconName = "Yellow Thorn";
    public const string greenLeafIconName = "Green Leaf";

    public const string discountIconName = "Discount";
    public const string volleyIconName = "Volley";

    public const string surpriseIconName = "SurpriseIcon";

    //Status Icons
    public const string mandatoryTargetIcon = "Mandatory Target";
    public const string stunnedIcon = "Stunned";

    public const string masterIcon = "Master";
    public const string minionIcon = "Minion";

    //Map Icons
    public const string restPointIcon = "Rest Point";
    public const string shopIcon = "Shop";

    //Hostility Icons
    public const string flowerIcon = "Flower";
    public const string hostileSkullIcon = "Skull";

    //Victory Conditions:
    public const string waves = "Waves";
}
