using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//class SlotHoverIcon
public class SlotIconHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IHoverIconSource, IDescribable
{
    [SerializeField]
    public string hoverMessageKey;
    protected string hoverText;

    public bool ignoreHover = false;

    [SerializeField]
    private bool bonusDamageIcon = false;
    [SerializeField]
    private bool damageIcon = false;

    public HoverIconDescriptionPanel descriptionPanel;

    public Image outlineImage;
    public RectTransform backgroundTransform;
    public Image backgroundImage;
    public Image iconImage;
    public Image bubble;

    public virtual void Awake()
    {
        SlotIconImage slotIconImage = iconImage as SlotIconImage;

        if(slotIconImage != null)
        {
            slotIconImage.parentIcon = this;
        }

        if(ignoreHover)
        {
            return;
        }

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

    public virtual void spawnHoverIcon()
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

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if(ignoreHover || eventData == null || eventData.used)
        {
            return;
        }

        if (hoverText != null && hoverText.Length > 0)
        {
            MouseHoverManager.startCoroutine(this, MouseHoverManager.waitToHandleDescriptionPanel(this, MouseHoverManager.shouldSpawnHoverIcon));
            eventData.Use();
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if(ignoreHover || eventData == null || eventData.used)
        {
            return;
        }

        MouseHoverManager.startCoroutine(this, MouseHoverManager.waitToHandleDescriptionPanel(this, MouseHoverManager.shouldDestroyHoverIcon));
    }

    public void revealBubble()
    {
        if(bubble != null)
        {
            backgroundImage.color = ColorList.bubbleBackgroundColor;
            bubble.sprite = Helpers.loadSpriteFromResources(PrefabNames.UIBubble);
            bubble.gameObject.SetActive(true);
            backgroundTransform.offsetMax = new Vector2(-3,-3);
            backgroundTransform.offsetMin = new Vector2(3,3);
        }

        if(outlineImage != null)
        {
            outlineImage.color = ColorList.bubbleOutlineColor;
        }
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

        DescriptionPanel.setText(panel.nameText, getHoverMessageKeyForDisplay());
        DescriptionPanel.setText(panel.useDescriptionText, hoverText);
    }

    private string getHoverMessageKeyForDisplay()
    {
        if(bonusDamageIcon)
        {
            return "Bonus Damage";
        }

        switch(hoverMessageKey)
        {
            case IconList.surpriseIconName:
                return "Surprise Status";
            case HoverMessageList.passiveSlotsKey:
                return "Bonus Slots";
            case HoverMessageList.zoneOfInfluenceKey:
                return "Zone of Influence";
            case Strength.symbolChar:
                return PrimaryStat.Strength.ToString();
            case Dexterity.symbolChar:
                return PrimaryStat.Dexterity.ToString();
            case Wisdom.symbolChar:
                return PrimaryStat.Wisdom.ToString();
            case Charisma.symbolChar:
                return PrimaryStat.Charisma.ToString();
            case Constants.emptyString:
            case null:
                if(iconImage != null && 
                    iconImage.sprite != null && 
                    !iconImage.sprite.name.Equals(Constants.emptyString))
                {
                    hoverMessageKey = iconImage.sprite.name;
                    return getHoverMessageKeyForDisplay();
                } else
                {
                    return Constants.emptyString;
                }
            default:
                return hoverMessageKey.Replace(".","");
        }
    }

	public void describeSelfRow(DescriptionPanel panel)
    {

    }

	public void setUpDecisionPanel(IDecisionPanel descisionPanel)
    {

    }

	public List<IDescribable> getRelatedDescribables()
    {
        List<IDescribable> relatedDescribables = new List<IDescribable>();

        switch(hoverText)
        {
            case HoverMessageList.zoneOfInfluenceMessage:

                if(OverallUIManager.getCurrentPartyMember() != null)
                {
                    relatedDescribables.Add(OverallUIManager.getCurrentPartyMember().getZoneOfInfluenceTrait());
                }

                break;
        }

        return relatedDescribables;
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
    public const string zoneOfInfluenceKey = "ZOI-Icon";
    public const string zoneOfInfluenceMessage = "A Trait applied to the Zone's owner and all allies directly infront, behind, or beside this creature. Zones of Influence are not applied diagonally.";
    
    private const string actionWheelKey = "Action Wheel";
    private const string actionWheelMessage = "The Action Wheel contains all of the Actions a character can bring into battle. With the exception of Passive Abilities, if an Action is not on the Action Wheel, the character is gaining no benefits from it.";
    public const string passiveSlotsKey = "Passive Slots";

    private const string mainHandWeaponTabKey = "Main Hand Tab";
    private const string mainHandWeaponSlotMessage = "You can equip Main-Hand Weapons from the Character and Inventory Screens.";
    private const string mainHandWeaponTabMessage = "Here you can find all the Main-Hand Weapons you have in your Inventory. " + mainHandWeaponMessage;
    private const string mainHandWeaponMessage = "Equipping a Main-Hand Weapon gives you a new Attack Action on your Action Wheel in Combat.";
    private const string twoHandedWeaponMessage = "This weapon requires two hands to wield. Two-Handed Weapons have larger ranges and deal more damage than one handed ones, but don't benefit from the damage of your Off Hand. Using a Two-Handed Weapon forfeits the benefits from your Shield for the rest of the turn.";
    private const string oneHandedWeaponMessage = "You only need one hand to wield this weapon. One-Handed Weapons have shorter ranges and deal less damage than Two-Handed ones, but add the damage of your Off-Hand Weapon to their damage.";

    private const string offhandSlotMessage = "Off Hand Slot";
    private const string offhandSubMessage = "Off-Hand Weapons give you extra Damage and Crit Chance when you attack with a One-Handed Weapon. Shields give extra Armor as long as you haven't attacked with a Two-Handed Weapon this turn.";
    private const string headSlotMessage = "Head Slot";
    private const string bodySlotMessage = "Body Slot";
    private const string handsSlotMessage = "Hand Slot";
    private const string feetSlotMessage = "Feet Slot";
    private const string trinketSlotMessage = "Trinket Slot";

    private const string keySubtypeMessage = "Keys can be used to open locks on chests and doors. Keys cannot be sold.";
    private const string questSubtypeMessage = "Quest Items are needed to complete specific Quest objectives. Quest Items cannot be sold.";
    private const string treasureSubtypeMessage = "The only purpose of a Treasure Item is to be sold. Treasure Items cannot be removed from your Junk pocket.";
    private const string bookSubtypeMessage = "Using a book will let you read its contents.";
    private const string healingSubtypeMessage = "This item can be used to heal you or your allies, in or out of combat.";
    private const string usableSubtypeMessage = "Usable Item";
    private const string usableSubMessage = "Some Usable Items heal, apply Traits in combat, or provide you with information. Most Usable Items are destroyed when used.";

    private const string weaponSubtypeMessage = "Main-Hand Weapons provide a new Attack Action on your Action Wheel in Combat. Off-Hand weapons provide extra damage and crit chance when you attack with a One-Handed Weapon.";
    private const string armorSubtypeMessage = "Wearing Armor provides Armor Score, which blocks a percentage of incoming damage. Some pieces of Armor also provide additional benefits.";

    private const string armorScoreKey = "Armor Score";
    private const string armorScoreMessage = "You gain Armor Score from the Items you have equipped, your Dexterity Stat, and some Traits/Abilities. Armor reduces incoming damage by a percentage. Armor cannot reduce incoming damage below 1.";


    private const string actionTypeIconMessage = "This Action's Type. A complete list of Action Types can be found in the Journal's Glossary.";
    private const string traitTypeIconMessage = "This Trait's Type. A complete list of Trait Types can be found in the Journal's Glossary.";
    public const string damageKey = "Damage";
    private const string damageIconMessage = "The amount of damage this Action deals. Hold 'Alt' to see the Action's Damage Formula. A Damage Formula calculates an Action's damage based on your stats. For example: an Action with a Damage Formula of '3S + 5' deals 3 times your Strength, plus 5.";
    private const string critIconMessage = "The Critical Hit chance of this Action. Hold 'Alt' to see an Action's Crit Formula. A Crit Formula calculates an Action's Critical Hit chance based on your stats. For example: an Action with a Crit Formula of '3D + 5' has a Critical Hit chance of 3 times your Dexterity, plus 5.";
    private const string rangeIconMessage = "The Range of this Action. An Action's Range determines how many spaces it affects, and in what shape. Hold 'Alt' or check the Glossary to see a Range's shape.";
    private const string cooldownIconMessage = "The Action's Cooldown. Actions with a Cooldown period are unavailable for a number of rounds after use.";
    private const string slotsIconMessage = "The maximum amount of Action Wheel Slots this Action can take up. Each Slot has it's own Cooldown period: assigning an Action to multiple Slots lets you use it more often.";
    private const string durationIconMessage = "This Action has an effect that lasts multiple rounds, such as applying a Trait to it's target.";

    private const string amountIconMessage = "Quantity";
    private const string worthIconMessage = "An Item's worth in Gold Pieces. A shopkeeper may buy items for more than their worth based on their current Discount.";
    private const string goldIconMessage = "Your Party's total Gold Pieces";

    private const string goldRewardKey = "Reward";
    private const string goldRewardMessage = "The amount of Gold your Party earned in Combat.";

    private const string invulnerableIconMessage = "Invulnerability provides a flat reduction to incoming damage per hit. Can only reduce incoming damage down to 1. Applied before the damage reduction from Armor Score.";
    private const string vulnerableIconMessage = "Extra damage that is applied when damage is taken. Applied before the damage reduction from Armor Score.";

    public const string bonusDamageKey = "Bonus Damage";
    private const string bonusDamageMessage = "Bonus Damage is added to the damage of all of your Abilities. Your Bonus Damage is equal to the highest Base Damage of all of your equipped Weapons. For example, a weapon with a Damage Formula of '3S + 5' provides 5 Bonus Damage. Hold 'Alt' when viewing a Weapon's stats to reveal formulas.";

    private const string weaponSlotKey = "Weapon Slots";
    private const string weaponSlotMessage = "The number of Main-Hand Weapons you can have equipped to your Action Wheel. The higher a Character's Wisdom, the more Weapon Slots that Character has.";

    private const string stanceWeaponMessage = "Attacks made with Stance Weapons, such as fists and staffs, give the attacker additional stacks of their current Stance.";

    private const string levelMessage = "Leveling up a character costs 1000 Experience. Gaining a level will increase Maximum Health, return all missing health, and boost one Primary Stat. The highest level a character can reach is 20.";
    private const string healthMessage = "A Party Member reduced to 0 health is knocked unconscious, and needs special abilities or items to be awakened in combat. Normal healing items can awaken a Party Member out of combat. If your character loses all of their health, however, they will die and you will lose the game.";
    private const string experienceMessage = "Your progress towards your next level up. Gain Experience from completing quests and defeating some boss monsters. For every 1000 Experience you gain, you can level up.";
    private const string experienceRewardKey = "Combat Experience";
    private const string experienceRewardMessage = "The amount of Experience each Party Member gained from this Combat, whether they participated in it or not. You will only earn Combat Experience if a fight is particularly challenging, or if it was related to a Quest.";


    private const string bonusHealthMessage = "Bonus Health. Extra Health added to your Total Health. Determined by your Strength.";
    private const string criticalHitDamageMessage = "Critical Damage Multiplier. How much extra damage is dealt whenever critical hit is scored. Determined by a character's Strength.";
    private const string physicalResistMessage = "Physical Resistance. Your chance to ignore a Wound Trait applied to you in combat. Determined by a character's Strength.";

    private const string extraArmorMessage = "Bonus Armor. An extra amount of Armor in addition to the Armor gained from equipment. Determined by a character's Dexterity.";
    private const string surpriseRoundDamageMultiplierMessage = "Surprise Damage Multiplier. This is the percentage of extra damage dealt when in a surprise round. Determined by a character's Dexterity.";
    private const string armorPenetrationMessage = "Armor Penetration. The percentage of an enemy's armor your Actions will ignore. Determined by a character's Dexterity.";

    private const string mentalResistMessage = "Mental Resistance. Your chance to ignore a Mental Trait applied to you in combat. Determined by a character's Wisdom.";
    private const string passiveSlotsMessage = "Bonus Slots are Action Slots that can only be occupied by Equipped Passives, Stances, and Weapons, saving you space on your Action Wheel for Actions you wish to activate. Actions equippable to Bonus Slots can still be equipped to the Action Wheel if desired. Determined by a character's Wisdom.";
    private const string bonusWeaponSlotsMessage = "You are able to carry more than the usual amount of weapons on your Action Wheel. Determined by a character's Wisdom.";

    private const string synergyMessage = "Party Members get to add their Synergy to the damage they deal, and subtract it from the damage they take, per Zone of Influence they are inside. Determined by a character's Charisma.";
    private const string bonusExuberancesMessage = "The number of Exuberances your Party has at the start of Combat. Having more Starting Exuberances allows you to use Abilities with Exuberance costs faster and more often. Determined by a character's Charisma.";
    private const string zoiMessage = "A Zone of Influence is a bonus applied to all allies adjacent to this character in Combat. Each character's Zone of Influence is different, but the potency of that bonus is determined by a character's Charisma.";

    private const string characterAbilityKey = "Character Abilities";
    private const string characterAbilityMessage = "Each Party Member gets a number of unique Abilities they unlock at certain levels.";

    private const string statPointKey = "Stat Points";
    private const string statPointMessage = "This shows how many times you can increase your Primary Stats. The four Primary Stats are Strength, Dexterity, Wisdom, and Charisma.";

    private const string compassKey = "Compass";
    private const string hostilityKey = "Hostility";
    private const string hostilityMessage = "Areas with green Hostility means that you cannot be attacked by random monsters. Areas with red Hostility may contain random Monsters. Yellow Hosility means you have committed a crime. When the bars of an area fill up with yellow, they will turn red and guards will be sent after you.";
    private const string footingKey = "Footing";
    private const string footingMessage = "Some enemies will chase you when you get too close. These enemies only move half as fast as you. When the Left Foot is visibile, enemies chasing you will move the next time you take a step.";

    private const string strengthMessage = "This Primary Stat bolsters a character's Maximum Health, Critical Hit Damage, and Physical Resistance. Strength also governs the Intimidate skill.";
    private const string dexterityMessage = "This Primary Stat bolsters a character's Armor, Surprise Round Damage Modifier, and Armor Penetration. Dexterity also governs the Cunning skill.";
    private const string wisdomMessage = "This Primary Stat bolsters a character's Mental Resistance. Wisdom also provides bonus Passive Slots, increases the number of Weapons you can have equipped, and governs the Observation skill.";
    private const string charismaMessage = "This Primary Stat increases your Synergy, gives access to Exuberances, and boosts a character's Zone of Influence. Charisma also governs the Leadership skill.";

    private const string usableItemInventoryTabMessage = "Usable Items Tab.";
    private const string usableItemOOCSubMessage = "Usable Items that can be activated out of combat can be found here." + howToUseItemMessage;
    private const string howToUseItemMessage = " <B>To Use a Usable Item, drag the Item onto the Party Member you want to use it on while on the Inventory Screen.</B>";
    private const string offHandTabMessage = "Off Hand Tab.";
    private const string armorTabKey = "Armor Tab";
    private const string armorTabMessage = "Equipping Armor is the main way to boost your Armor Score and reduce incoming damage. Some Armor provides additional benefits.";
    private const string essentialTabKey = "Essential Tab";
    private const string essentialTabMessage = "Essential Items such as Quest Items and Keys cannot be sold to a Merchant.";
    private const string junkTabKey = "Junk Tab";
    private const string junkTabMessage = "Here you can see all the Items you have marked as Junk.";
    private const string junkSubMessage = " All Items marked as Junk can be sold simultaneously to a Merchant. Treasure Items are always marked as Junk.";

    public const string junkSlotKey = "Junk Slot";
    private const string junkSlotMessage = "Drag Items here to mark them as Junk.";
    public const string toInvSlotKey = "To Inv Slot";
    private const string toInvSlotMessage = "Drag items here to remove them from Junk. Treasure Items cannot be removed from Junk.";
    public const string buySlotKey = "Buy Slot";
    private const string buySlotMessage = "Drag Items here to buy them.";
    public const string sellSlotKey = "Sell Slot";
    private const string sellSlotMessage = "Drag Items here to sell them.";

    public const string skillsLabelKey = "Skills";
    private const string skillsMessage = "Skills are Abilities that are usable outside of combat. Unlock Skills by upgrading your Primary Stats. Proficiency in each Skill is based on the the highest Primary Stat of all Party Members.";
    public const string partyStatsLabelKey = "Party Stats";
    private const string partyStatsMessage = "Party Stats are Stats that reflect your Party's combined knowledge. Each Party Stat's progression is based on one or more of your Party's total Primary Stats.";
    public const string exuberancesLabelKey = "Exuberances";
    private const string exuberancesMessage = "Exuberances are resources that you can spend to activate powerful abilities during combat. To unlock Exuberances, at least one of your Party Members must have two or more Charisma.";


    private const string intimidateMessage = "This Skill can be used to stop enemies from ambushing you, but prevents you from ambushing them in turn. Your Intimidate charges are determined by the highest Strength of all Party Members.";
    private const string cunningMessage = "This Skill is used out of combat to fool enemies, turning them around and stunning them for a few steps, allowing you to ambush them or sneak around them. Some objects in the overworld can be activated with Cunning as well. Your Cunning charges are determined by the highest Dexterity of all Party Members.";
    private const string observationMessage = "This Skill allows you to find secret doors and hidden secrets. Determined by the highest Wisdom of all Party Members.";
    private const string leadershipMessage = "This Skill allows you to command your followers out of combat, telling them to stand on buttons or in doorways to block enemy movement. The number of Party Members you can command with Leadership is determined by the highest Charisma of all Party Members.";

    private const string redKnifeMessage = "This shows the amount of the Red Knife exuberance your Party will gain at the start of Combat.\n\n" + AbilityList.redKnifeAcquisitionMethodExplanation;
    private const string blueShieldMessage = "This shows the amount of the Blue Shield exuberance your Party will gain at the start of Combat.\n\n" + AbilityList.blueShieldAcquisitionMethodExplanation;
    private const string yellowThornMessage = "This shows the amount of the Yellow Thorn exuberance your Party will gain at the start of Combat.\n\n" + AbilityList.yellowThornAcquisitionMethodExplanation;
    private const string greenLeafMessage = "This shows the amount of the Grean Leaf exuberance your Party will gain at the start of Combat.\n\n" + AbilityList.greenLeafAcquisitionMethodExplanation;
    

    private const string regenMessage = "How much health each of your party members will heal after every combat. Determined by your Party's total Strength and Wisdom.";
    private const string surpriseRoundAmountMessage = "The number of rounds of extra Actions you will receive whenever you surprise an enemy. Determined by your Party's total Dexterity.";
    private const string retreatChanceMessage = "Your chance to successfully retreat from combat. Determined by your Party's total Dexterity and Wisdom.";
    private const string volleyAccuracyMessage = "Extra accuracy applied whenever your party performs a Volley action. Determined by your Party's total Wisdom and Charisma.";
    private const string goldMultiplierMessage = "Extra Gold received from combat. Can be gained by equipping certain Items and Abilities.";
    private const string partySlotsMessage = "The number of Party Members you can bring with you into combat. Determined by the highest level of any of your Party Members, as well as your Party's total Wisdom and Charisma.";
    private const string partyActionsMessage = "The number of Actions your Party can perform each round in combat. Determined by the highest level of any of your Party Members, as well as your Party's total Dexterity and Charisma.";
    public const string discountKey = "Discount";
    private const string discountMessage = "The total difference in an item's price, based on your Party's total Charisma. Sometimes merchants will give extra discounts or penalties based on what you've done for them or to them. A negative discount means the merchant is making their goods more expensive.";

    public const string retreatButtonKey = "Retreat Button";
    private const string retreatButtonMessage = "Click here to Retreat. The percentage is your success chance. If you fail, the enemy will take their entire turn before you get to act again. Even if you succeed, the enemy will be fully restored when you return. Combat entered through dialogue cannot be retreated from. Be careful who you pick a fight with!";

    private const string actionOrderKey = "Action Order";
    private const string actionOrderMessage = "Most Actions can only be performed between rounds, and will be added to the Action Order. Unless otherwise stated, the Action Order will alternate between Actions performed by your allies, and by your enemies. When you resolve the turn, all Actions in the Action Order will occur in the order they are displayed, starting at the top. To learn who is performing an Action in the Action Order, who it is targeting, and what it will do, hover your mouse over the Action's row.";

    private const string questJournalTabKey = "Quest Tab";
    private const string glossaryJournalTabKey = "Glossary Tab";

    private const string characterScreenKey = "Character Screen";
    private const string characterScreenMessage = "Here you can check your Character's Stats, change equipped Abilities, and spend Exp to Level Up.";
    private const string inventoryScreenKey = "Inventory Screen";
    private const string inventoryScreenMessage = "Here you can see what Items you have picked up, or change your Character's Equipment";
    private const string partyScreenKey = "Party Screen";
    private const string partyScreenMessage = "Here you can see your Party's Stats and Skills, swap out Party Members, and change your Party's starting Formation.";
    private const string journalScreenKey = "Journal Screen";
    private const string journalScreenMessage = "Here you can see what quests you have, and look up Terms in the Glossary.";
    private const string saveAndLoadScreenKey = "Save/Load Screen";
    private const string saveAndLoadScreenMessage = "Here you can save your game. Remember, you can also Quick Save with 'Q', and the game will take an Autosave whenever you enter an area. You can have up to three Autosaves at a time.";
    private const string settingsScreenKey = "Settings Screen";
    private const string settingsScreenMessage = "Here you can change the Game's Settings, or Quit the Game.";

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
                return mainHandWeaponMessage + " " + mainHandWeaponSlotMessage;
            case mainHandWeaponTabKey:
                return mainHandWeaponTabMessage;
            case EquippableItem.twoHandedSlotIconName:
                return twoHandedWeaponMessage;
            case EquippableItem.oneHandedSlotIconName:
                return oneHandedWeaponMessage;

            case Key.typeIconName:
                return keySubtypeMessage;
            case QuestItem.typeIconName:
                return questSubtypeMessage;

            case UsableItem.typeIconName:
                return usableSubMessage + howToUseItemMessage;
            case BookItem.typeIconName:
                return bookSubtypeMessage + howToUseItemMessage;
            case HealingItem.typeIconName:
                return healingSubtypeMessage + howToUseItemMessage;

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
            case goldRewardKey:
                return goldRewardMessage;
            case experienceRewardKey:
                return experienceRewardMessage;

            case weaponSlotKey:
                return weaponSlotMessage;
            case armorScoreKey:
                return armorScoreMessage;

            case damageKey:
                return damageIconMessage;
            case bonusDamageKey:
                return bonusDamageMessage;

            case IconList.invulnerableIconName:
                return invulnerableIconMessage;
            case IconList.vulnerableIconName:
                return vulnerableIconMessage;

            case IconList.stanceWeaponIconName:
                return stanceWeaponMessage;

            case IconList.levelIconName:
                return levelMessage;
            case IconList.healthIconName:
                return healthMessage;
            case IconList.affinityIconName:
            case IconList.experienceIconName:
                return experienceMessage;

            case characterAbilityKey:
                return characterAbilityMessage;

            case Strength.symbolChar:
                return strengthMessage;
            case Dexterity.symbolChar:
                return dexterityMessage;
            case Wisdom.symbolChar:
                return wisdomMessage;
            case Charisma.symbolChar:
                return charismaMessage;

            case compassKey:
                return compassKey;
            case hostilityKey:
                return hostilityMessage;
            case footingKey:
                return footingMessage;

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
                return usableSubMessage + usableItemOOCSubMessage;
            case offHandTabMessage:
                return offhandSubMessage;
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

            case characterScreenKey:
                return characterScreenMessage;
            case inventoryScreenKey:
                return inventoryScreenMessage;
            case partyScreenKey:
                return partyScreenMessage;
            case journalScreenKey:
                return journalScreenMessage;
            case saveAndLoadScreenKey:
                return saveAndLoadScreenMessage;
            case settingsScreenKey:
                return settingsScreenMessage;

            default:

                if(iconName != null && iconName.Contains(zoneOfInfluenceKey))
                {
                    return zoiMessage;
                }

                return "";
        }
    }
}
