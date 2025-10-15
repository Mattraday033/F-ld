using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SlotIconHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IHoverIconSource, IDescribable
{
    //[SerializeField]
    protected string hoverMessageKey;
    private string hoverText;

    //[SerializeField]
    private bool bonusDamageIcon = false;
    //[SerializeField]
    private bool damageIcon = false;

    public HoverIconDescriptionPanel descriptionPanel;

    public Image outlineImage;
    public Image backgroundImage;
    public Image iconImage;

    public virtual void Awake()
    {
        // spawnHoverMessagePanel();

        if (bonusDamageIcon)
        {
            setHoverMessage(HoverMessageList.getMessage(HoverMessageList.bonusDamageKey));
        }
        else if (damageIcon)
        {
            setHoverMessage(HoverMessageList.getMessage(HoverMessageList.damageKey));
        }
        else if (hoverMessageKey != null && hoverMessageKey.Length > 0)
        {
            setHoverMessage(HoverMessageList.getMessage(hoverMessageKey));
        }
    }

    public void setHoverMessage(string message)
    {
        if (message == null || message.Length <= 0)
        {
            enabled = false;
            return;
        }
        else
        {
            enabled = true;
        }

        hoverText = message;
    }

    public virtual void spawnHoverMessagePanel()
    {
        descriptionPanel = Instantiate(Resources.Load<GameObject>(PrefabNames.hoverIconDescriptionPanel), transform).GetComponent<HoverIconDescriptionPanel>();
    }

    public void showHoverMessagePanel()
    {
        if (descriptionPanel != null)
        {
            descriptionPanel.gameObject.SetActive(true);
        }
    }

    public void hideHoverMessagePanel()
    {
        if (descriptionPanel != null)
        {
            descriptionPanel.gameObject.SetActive(false);
        }
    }

    public void spawnHoverIcon()
    {
        MouseHoverManager.spawnHoverIcon(this, transform);
    }

    public void destroyHoverIcon()
    {
        MouseHoverManager.destroyHoverIcon();
    }

    public virtual GameObject getDescriptionPanelType()
    {
        return Resources.Load<GameObject>(PrefabNames.hoverIconDescriptionPanel);
    }

    public virtual IDescribable getObjectBeingDescribed()
    {
        return this;
    }

    void OnMouseEnter()
    {
        OnPointerEnter(null);
    }

    void OnMouseExit()
    {
        OnPointerExit(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverText != null && hoverText.Length > 0)
        {
            MouseHoverManager.startCoroutine(this, MouseHoverManager.waitToHandleDescriptionPanel(this, MouseHoverManager.shouldSpawnHoverIcon));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseHoverManager.OnHoverPanelCreation.Invoke();
        MouseHoverManager.startCoroutine(this, MouseHoverManager.waitToHandleDescriptionPanel(this, MouseHoverManager.shouldDestroyHoverIcon));
    }

    //IDescribable methods
    public string getName()
    {
        return hoverMessageKey;
    }
    public bool ineligible()
    {
        return false;
    }

    public GameObject getRowType(RowType rowType)
    {
        return null;
    }

	public GameObject getDescriptionPanelFull()
    {
        return getDescriptionPanelFull(PanelType.Standard);
    }

	public GameObject getDescriptionPanelFull(PanelType type)
    {
        return Resources.Load<GameObject>(PrefabNames.hoverIconDescriptionPanelInterior);
    }

	public GameObject getDecisionPanel()
    {
        return null;
    }

	public bool withinFilter(string[] filterParameters)
    {
        return false;
    }

    public void describeSelfFull(DescriptionPanel panel)
    {
        panel.setObjectBeingDescribed(this);

        DescriptionPanel.setText(panel.useDescriptionText, hoverText);
    }

	public void describeSelfRow(DescriptionPanel panel)
    {

    }

	public void setUpDecisionPanel(IDecisionPanel descisionPanel)
    {

    }

	public ArrayList getRelatedDescribables()
    {
        return new ArrayList();
    }

	public bool buildableWithBlocks()
    {
        return false;
    }
	public bool buildableWithBlocksRows()
    {
        return false;
    }
}

public static class HoverMessageList
{
    private const string zoneOfInfluenceKey = "ZOI";
    private const string zoneOfInfluenceMessage = "Zone of Influence. A Trait applied to the Zone's owner and all allies directly infront, behind, or beside this creature. Is not applied diagonally.";
    
    private const string actionWheelKey = "Action Wheel";
    private const string actionWheelMessage = "The Action Wheel contains all of the Actions a character can bring into battle. With the exception of Passive Abilities, if an Action is not on the Action Wheel, the character is gaining no benefits from it.";
    private const string passiveSlotsKey = "Passive Slots";

    private const string mainHandWeaponSlotKey = "Main Hand Weapon Slot";
    private const string mainHandWeaponSlotMessage = "Main Hand Weapon Slot. You can equip Main Hand Weapons from the Character Screen.";
    private const string mainHandWeaponMessage = "Main Hand Weapon. Unlike Offhand weapons, equipping it gives you a new Attack Action on your Action Wheel in Combat.";
    private const string twoHandedWeaponMessage = "This weapon requires two hands to wield. Two handed weapons have larger ranges and deal more damage than one handed ones, but don't benefit from the damage of your offhand. Using a two handed weapon forfeits the armor from your shield for the rest of the turn.";
    private const string oneHandedWeaponMessage = "You only need one hand to wield this weapon. One handed weapons have shorter ranges and deal less damage than two handed ones, but add the damage of your offhand weapon to their damage.";

    private const string offhandSlotMessage = "Offhand Slot.";
    private const string offhandSubMessage = "Offhand weapons give you extra Damage and Crit Chance when you attack with a One Handed Weapon. Shields give extra Armor as long as you haven't attacked with a Two Handed Weapon this turn.";
    private const string headSlotMessage = "Head Slot";
    private const string bodySlotMessage = "Body Slot";
    private const string handsSlotMessage = "Hand Slot";
    private const string feetSlotMessage = "Feet Slot";
    private const string trinketSlotMessage = "Trinket Slot";

    private const string keySubtypeMessage = "Key. Keys can be used to open locks on chests and doors. It can't be sold.";
    private const string questSubtypeMessage = "Quest Item. It can't be sold.";
    private const string treasureSubtypeMessage = "Treasure. It's only purpose is to be sold.";
    private const string bookSubtypeMessage = "Book. Using it will let your character read it.";
    private const string healingSubtypeMessage = "Healing Item. This item can be used to heal your character or your allies, in or out of combat.";
    private const string usableSubtypeMessage = "Usable Item.";
    private const string usableSubMessage = " Some Usable Items heal, apply Traits in combat, or provide you with information. Most Usable Items are destroyed when used.";

    private const string weaponSubtypeMessage = "Weapon. Main hand weapons provide a new Attack Action on your Action Wheel in Combat. Offhand weapons provide extra damage and crit chance when you attack with a one handed weapon.";
    private const string armorSubtypeMessage = "Armor. Wearing it provides Armor Points, which blocks .5% of damage per point. Some pieces of Armor also provide additional benefits.";

    private const string armorScoreKey = "Armor Score";
    private const string armorScoreMessage = "You gain armor from the items you have equipped, your Dexterity Stat, and some Abilities. For every point of Armor you have, you block .5% of incoming damage.";


    private const string actionTypeIconMessage = "This Action's Type. A complete list of Action Types can be found in the Journal's Glossary.";
    private const string traitTypeIconMessage = "This Trait's Type. A complete list of Trait Types can be found in the Journal's Glossary.";
    public const string damageKey = "Damage";
    private const string damageIconMessage = "The amount of damage this Action deals. Hold 'Alt' to see the Action's Damage Formula. A Damage Formula calculates an Action's damage based on your stats. For example: an Action with a Damage Formula of '3S + 5' deals 3 times your Strength, plus 5.";
    private const string critIconMessage = "The Critical Hit chance of this Action. Hold 'Alt' to see this Action's Crit Formula. A Crit Formula calculates an Action's Critical Hit chance based on your stats. For example: an Action with a Crit Formula of '3D + 5' has a Critical Hit chance of 3 times your Dexterity, plus 5.";
    private const string rangeIconMessage = "The Range of this Action. An Action's Range determines how many spaces it affects, and in what shape.";
    private const string cooldownIconMessage = "The Action's Cooldown. Actions with a Cooldown period are unavailable for a number of rounds after use.";
    private const string slotsIconMessage = "The maximum amount of Action Wheel Slots this Action can take up. Each Slot has it's own Cooldown period: assigning an Action to multiple Slots lets you use it more often.";
    private const string durationIconMessage = "This Action has an effect that lasts multiple rounds, such as applying a Trait to it's target.";

    private const string amountIconMessage = "Quantity";
    private const string worthIconMessage = "Gold Pieces";
    private const string goldIconMessage = "Gold Pieces";


    public const string bonusDamageKey = "Bonus Damage";
    private const string bonusDamageMessage = "Bonus Damage. Bonus Damage is added to the damage of all of your Abilities. Your Bonus Damage is equal to the highest Base Damage of all of your equipped Weapons. For example, a weapon with a Damage Formula of '3S + 5' provides 5 Bonus Damage. Hold 'Alt' when viewing a Weapon's stats to reveal formulas.";

    private const string weaponSlotKey = "Weapon Slots";
    private const string weaponSlotMessage = "Weapon Slots. Weapon Slots are used to assign your equipped weapons to your Action Wheel.";

    private const string stanceWeaponMessage = "Stance Weapon. Attacks made with Stance Weapons, such as fists and staffs, give the attacker additional stacks of their current Stance.";

    private const string levelMessage = "Level. Leveling up a character costs 1000 Experience and their Maximum Health, heals them to full, and boosts one of their Primary Stats. The highest level a character can reach is 20.";
    private const string healthMessage = "Health. A Party Member reduced to 0 health is knocked unconcious, and needs special abilities or items to be awakened in combat. Normal healing items can awaken a Party Member out of combat. If your character loses all of their health, however, they will die.";
    private const string affinityMessage = "Affinity. Affinity is gained whenever you defeat a creature in combat; the more creatures in combat you defeat, the more Affinity you will gain per fight. A higher Charisma also contributes to the amount of Affinity you gain. Spend Affinity to upgrade your companions.";
    private const string experienceMessage = "Experience. Your progress towards your next level up. Gain Experience from completing quests and defeating some boss monsters. For every 1000 Experience you gain, you can level up.";


    private const string bonusHealthMessage = "Bonus Health. Extra Health added to your Total Health. Determined by your Strength.";
    private const string criticalHitDamageMessage = "Critical Damage Multiplier. How much extra damage is dealt whenever critical hit is scored. Determined by a character's Strength.";
    private const string physicalResistMessage = "Physical Resistance. Your chance to ignore a Wound Trait applied to you in combat. Determined by a character's Strength.";

    private const string extraArmorMessage = "Bonus Armor. An extra amount of Armor in addition to the Armor gained from equipment. Determined by a character's Dexterity.";
    private const string surpriseRoundDamageMultiplierMessage = "Surprise Damage Multiplier. This is the percentage of extra damage dealt when in a surprise round. Determined by a character's Dexterity.";
    private const string armorPenetrationMessage = "Armor Penetration. The percentage of an enemy's armor your Actions will ignore. Determined by a character's Dexterity.";

    private const string mentalResistMessage = "Mental Resistance. Your chance to ignore a Mental Trait applied to you in combat. Determined by a character's Wisdom.";
    private const string passiveSlotsMessage = "Passive Slots. Passive Slots are Action Slots that can only be occupied by Equipped Passives, Stances, and Weapons, saving you space on your Action Wheel for Actions you wish to activate. Actions equippable to Passive Slots can still be equipped to the Action Wheel if desired. Determined by a character's Wisdom.";
    private const string bonusWeaponSlotsMessage = "Bonus Weapon Slots. You are able to carry more than the usual amount of weapons on your Action Wheel. Determined by a character's Wisdom.";

    private const string synergyMessage = "Synergy. Party Members get to add their Synergy to the damage they deal, and subtract it from the damage they take, per Zone of Influence they are inside. Determined by a character's Charisma.";
    private const string bonusExuberancesMessage = "Bonus Exuberances. Bonus Exuberances are added at the start of Combat, letting you use Abilities with Exuberance costs faster and more often. Determined by a character's Charisma.";
    private const string zoiMessage = "Zone of Influence. A Zone of Influence is a bonus applied to all allies adjacent to this character in Combat. Each character's Zone of Influence is different, but the potency of that bonus is determined by a character's Charisma.";

    private const string statPointKey = "Stat Points";
    private const string statPointMessage = "Stat Points. This shows how many times you can increase your Primary Stats. The four Primary Stats are Strength, Dexterity, Wisdom, and Charisma.";

    private const string strengthMessage = "Strength. This Primary Stat bolsters a character's Maximum Health, Critical Hit Damage, and Physical Resistance. Strength also governs the Intimidate skill.";
    private const string dexterityMessage = "Dexterity. This Primary Stat bolsters a character's Armor, Surprise Round Damage Modifier, and Armor Penetration. Dexterity also governs the Cunning skill.";
    private const string wisdomMessage = "Wisdom. This Primary Stat bolsters a character's Mental Resistance. Wisdom also provides bonus Passive Slots, increases the number of Weapons you can have equipped, and governs the Observation skill.";
    private const string charismaMessage = "Charisma. This Primary Stat increases your Synergy, gives access to Exuberances, and boosts a character's Zone of Influence. Charisma also governs the Leadership skill.";

    private const string usableItemInventoryTabMessage = "Usable Items Tab.";
    private const string usableItemOOCSubMessage = " Usable Items that can be activated out of combat can be found here.";
    private const string offHandTabMessage = "Off Hand Tab.";
    private const string armorTabKey = "Armor Tab";
    private const string armorTabMessage = "Armor Tab. Equipping Armor is the main way to boost your Armor Score and reduce incoming damage. Some Armor provides additional benefits.";
    private const string essentialTabKey = "Essential Tab";
    private const string essentialTabMessage = "Essential Tab. Essential Items such as Quest Items and Keys cannot be sold to a Merchant.";
    private const string junkTabKey = "Junk Tab";
    private const string junkTabMessage = "Junk Tab.";
    private const string junkSubMessage = " All Items marked as Junk can be sold simultaneously to a Merchant. Treasure Items are always marked as Junk.";

    public const string junkSlotKey = "Junk Slot";
    private const string junkSlotMessage = "Drag Items here to mark them as Junk. Junk Items can be sold more easily.";
    public const string toInvSlotKey = "To Inv Slot";
    private const string toInvSlotMessage = "Drag items here to remove them from Junk. Treasure Items cannot be removed from Junk.";
    public const string buySlotKey = "Buy Slot";
    private const string buySlotMessage = "Drag Items here to buy them.";
    public const string sellSlotKey = "Sell Slot";
    private const string sellSlotMessage = "Drag Items here to sell them.";

    public const string skillsLabelKey = "Skills";
    private const string skillsMessage = "Skills are Abilities that are usable outside of combat. Unlock Skills by upgrading your Primary Stats. The higher your Primary Stats, the more proficient in your skills. Only the Main Character's Primary Stat's contribute to your Skills.";
    public const string partyStatsLabelKey = "Party Stats";
    private const string partyStatsMessage = "Party Stats are Stats that reflect your Party's combined knowledge. Each Party Stat's progression is based on one or more of your Party's total Primary Stats.";
    public const string exuberancesLabelKey = "Exuberances";
    private const string exuberancesMessage = "Exuberances are resources that you can spend to activate powerful abilities during combat. To unlock Exuberances, at least one of your Party Members must have two or more Charisma.";


    private const string intimidateMessage = "Intimidate. This ability can be used to stop enemies from ambushing you, but prevents you from being ambushed in turn. Your Intimidate charges are determined by your character's Strength.";
    private const string cunningMessage = "Cunning. Cunning is used out of combat to fool enemies, turning them around and stunning them for a few steps, allowing you to ambush them or sneak around them. Some objects in the overworld can activated with Cunning as well. Your Cunning charges are determined by your character's Dexterity.";
    private const string observationMessage = "Observation. This skill allows you to find secret doors and hidden secrets. Determined by your character's Wisdom.";
    private const string leadershipMessage = "Leadership. This skill allows you to command your followers out of combat, telling them to stand on buttons or in doorways to block enemy movement. The number of Party Members you can command with Leadership is determined by your character's Charisma.";

    private const string redKnifeMessage = "This shows the amount of the Red Knife exuberance your Party will gain at the start of Combat.";
    private const string blueShieldMessage = "This shows the amount of the Blue Shield exuberance your Party will gain at the start of Combat.";
    private const string yellowThornMessage = "This shows the amount of the Yellow Thorn exuberance your Party will gain at the start of Combat.";
    private const string greenLeafMessage = "This shows the amount of the Grean Leaf exuberance your Party will gain at the start of Combat.";
    

    private const string regenMessage = "Regeneration. How much health each of your party members will heal after every combat. Determined by your Party's total Strength and Wisdom.";
    private const string surpriseRoundAmountMessage = "Surprise Duration. The number of rounds of extra Actions you will receive whenever you surprise an enemy. Determined by your Party's total Dexterity.";
    private const string retreatChanceMessage = "Retreat Chance. Your chance to successfully retreat from combat. Determined by your Party's total Dexterity and Wisdom.";
    private const string volleyAccuracyMessage = "Volley Accuracy Bonus. Extra accuracy applied whenever your party performs a Volley action. Determined by your Party's total Wisdom and Charisma.";
    private const string goldMultiplierMessage = "Gold Multiplier. Extra Gold received from combat. Can be gained by equipping certain Items and Abilities.";
    private const string partySlotsMessage = "Party Slots. The number of Party Members you can bring with you into combat. Determined by the highest level of any of your Party Members, as well as your Party's total Wisdom and Charisma.";
    private const string partyActionsMessage = "Party Actions. The number of Actions your Party can perform each round in combat. Determined by the highest level of any of your Party Members, as well as your Party's total Dexterity and Charisma.";
    public const string discountKey = "Discount";
    private const string discountMessage = "Discount. The total difference in an item's price, based on your Party's total Charisma. Sometimes merchants will give extra discounts or penalties based on what you've done for them or to them. A negative discount means the merchant is making their goods more expensive.";

    public const string retreatButtonKey = "Retreat Button";
    private const string retreatButtonMessage = "Click here to Retreat. The percentage is your success chance. If you fail, the enemy will take their entire turn before you get to act again. Even if you succeed, the enemy will be fully restored when you return. Combat entered through dialogue cannot be retreated from. Be careful who you pick a fight with!";

    private const string actionOrderKey = "Action Order";
    private const string actionOrderMessage = "Most Actions can only be performed between rounds, and will be added to the Action Order. Unless otherwise stated, the Action Order will alternate between Actions performed by your allies, and by your enemies. When you resolve the turn, all Actions in the Action Order will occur in the order they are displayed, starting at the top. To learn who is performing an Action in the Action Order, who it is targeting, and what it will do, hover your mouse over the Action's row.";

    private const string questJournalTabKey = "Quest Tab";
    private const string glossaryJournalTabKey = "Glossary Tab";

    public static string getMessage(string iconName)
    {
        switch (iconName)
        {
            case EquippableItem.headSlotIconName:
                return headSlotMessage;
            case EquippableItem.bodySlotIconName:
                return bodySlotMessage;
            case EquippableItem.handsSlotIconName:
                return handsSlotMessage;
            case EquippableItem.feetSlotIconName:
                return feetSlotMessage;
            case EquippableItem.trinketSlotIconName:
                return trinketSlotMessage;

            case EquippableItem.offHandSlotIconName:
                return offhandSlotMessage;

            case EquippableItem.mainHandSlotIconName:
                return mainHandWeaponMessage;
            case mainHandWeaponSlotKey:
                return mainHandWeaponSlotMessage;
            case EquippableItem.twoHandedSlotIconName:
                return twoHandedWeaponMessage;
            case EquippableItem.oneHandedSlotIconName:
                return oneHandedWeaponMessage;

            case Key.typeIconName:
                return keySubtypeMessage;
            case QuestItem.typeIconName:
                return questSubtypeMessage;

            case UsableItem.typeIconName:
                return usableSubtypeMessage + usableSubMessage;
            case BookItem.typeIconName:
                return bookSubtypeMessage;
            case HealingItem.typeIconName:
                return healingSubtypeMessage;

            case TreasureItem.typeIconName:
                return treasureSubtypeMessage;

            case Weapon.typeIconName:
                return weaponSubtypeMessage;
            case Armor.typeIconName:
                return armorSubtypeMessage;

            case IconList.actionTypeIconName:
                return actionTypeIconMessage;
            case IconList.traitTypeIconName:
                return traitTypeIconMessage;
            case IconList.critIconName:
                return critIconMessage;
            case IconList.rangeIconName:
                return rangeIconMessage;
            case IconList.cooldownIconName:
                return cooldownIconMessage;
            case IconList.slotsIconName:
                return slotsIconMessage;
            case IconList.durationIconName:
                return durationIconMessage;

            case IconList.amountIconName:
                return amountIconMessage;
            case IconList.worthIconName:
                if (!CombatStateManager.inCombat && PlayerOOCStateManager.currentActivity == OOCActivity.inUI && OverallUIManager.lastScreenType == ScreenType.Character)
                {
                    return goldIconMessage;
                }
                else
                {
                    return worthIconMessage;
                }

            case weaponSlotKey:
                return weaponSlotMessage;
            case armorScoreKey:
                return armorScoreMessage;

            case damageKey:
                return damageIconMessage;
            case bonusDamageKey:
                return bonusDamageMessage;

            case IconList.stanceWeaponIconName:
                return stanceWeaponMessage;

            case IconList.levelIconName:
                return levelMessage;
            case IconList.healthIconName:
                return healthMessage;
            case IconList.affinityIconName:
                return affinityMessage;
            case IconList.experienceIconName:
                return experienceMessage;

            case Strength.symbolChar:
                return strengthMessage;
            case Dexterity.symbolChar:
                return dexterityMessage;
            case Wisdom.symbolChar:
                return wisdomMessage;
            case Charisma.symbolChar:
                return charismaMessage;

            case IconList.intimidateIconName:
                return intimidateMessage;
            case IconList.bonusHealthIconName:
                return bonusHealthMessage;
            case IconList.criticalHitDamageIconName:
                return criticalHitDamageMessage;
            case IconList.physicalResistIconName:
                return physicalResistMessage;
            case IconList.regenIconName:
                return regenMessage;
            case IconList.cunningIconName:
                return cunningMessage;
            case IconList.extraArmorIconName:
                return extraArmorMessage;
            case IconList.surpriseRoundDamageMultiplierIconName:
                return surpriseRoundDamageMultiplierMessage;
            case IconList.surpriseRoundAmountIconName:
                return surpriseRoundAmountMessage;
            case IconList.observationIconName:
                return observationMessage;
            case IconList.armorPenetrationIconName:
                return armorPenetrationMessage;
            case IconList.mentalResistIconName:
                return mentalResistMessage;
            case IconList.retreatChanceIconName:
                return retreatChanceMessage;
            case passiveSlotsKey:
            case IconList.passiveSlotsIconName:
                return passiveSlotsMessage;
            case IconList.bonusWeaponSlotsIconName:
                return bonusWeaponSlotsMessage;
            case IconList.allExuberancesIconName:
                return bonusExuberancesMessage;
            case IconList.synergyIconName:
                return synergyMessage;
            case IconList.leadershipIconName:
                return leadershipMessage;
            case IconList.partySlotsIconName:
                return partySlotsMessage;
            case IconList.partyActionsIconName:
                return partyActionsMessage;
            case IconList.volleyIconName:
                return volleyAccuracyMessage;
            case IconList.goldMultiplierIconName:
                return goldMultiplierMessage;

            case IconList.redKnifeIconName:
                return redKnifeMessage;
            case IconList.blueShieldIconName:
                return blueShieldMessage;
            case IconList.yellowThornIconName:
                return yellowThornMessage;
            case IconList.greenLeafIconName:
                return greenLeafMessage;

            case zoneOfInfluenceKey:
                return zoneOfInfluenceMessage;

            case actionWheelKey:
                return actionWheelMessage;

            case usableItemInventoryTabMessage:
                return usableItemInventoryTabMessage + usableSubMessage + usableItemOOCSubMessage;
            case offHandTabMessage:
                return offHandTabMessage + offhandSubMessage;
            case armorTabKey:
                return armorTabMessage;
            case essentialTabKey:
                return essentialTabMessage;
            case junkTabKey:
                return junkTabMessage + junkSubMessage;

            case junkSlotKey:
                return junkSlotMessage + junkSubMessage;
            case toInvSlotKey:
                return toInvSlotMessage;
            case buySlotKey:
                return buySlotMessage;
            case sellSlotKey:
                return sellSlotMessage;

            case statPointKey:
                return statPointMessage;
            case discountKey:
                return discountMessage;

            case skillsLabelKey:
                return skillsMessage;
            case partyStatsLabelKey:
                return partyStatsMessage;
            case exuberancesLabelKey:
                return exuberancesMessage;

            case questJournalTabKey:
                return questJournalTabKey;
            case glossaryJournalTabKey:
                return glossaryJournalTabKey;

            case retreatButtonKey:
                return retreatButtonMessage; 
            
            case actionOrderKey:
                return actionOrderMessage;

            default:
                return "";
        }
    }
}
