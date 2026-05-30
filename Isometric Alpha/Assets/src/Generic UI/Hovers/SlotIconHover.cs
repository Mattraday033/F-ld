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

    public bool inWorldSpace = false;

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

    public void setHoverMessage(string hoverMessageKey, string message)
    {
        this.hoverMessageKey = hoverMessageKey;

        setHoverMessage(message);
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
        float scale = 1f;

        if(inWorldSpace)
        {
            scale = getInWorldSpaceScale();
        }

        MouseHoverManager.spawnHoverIcon(this, transform, scale);
    }

    protected virtual float getInWorldSpaceScale()
    {
        return .36f;
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
        if(ignoreHover || (eventData != null && eventData.used))
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
        if(ignoreHover || (eventData != null && eventData.used))
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

    public virtual void describeSelfFull(DescriptionPanel panel)
    {
        panel.setObjectBeingDescribed(this);

        DescriptionPanel.setText(panel.nameText, getHoverMessageKeyForDisplay());
        DescriptionPanel.setText(panel.useDescriptionText, hoverText);
    }

    protected virtual string getHoverMessageKeyForDisplay()
    {
        if(bonusDamageIcon)
        {
            return "Bonus Damage";
        }

        string[] keySections = hoverMessageKey.Split("-");

        if(keySections.Length > Constants.sizeTwo)
        {
            hoverMessageKey = keySections[1] + "-" + keySections[2];
        } 

        switch(hoverMessageKey)
        {
            case HoverMessageList.characterScreenKey:
                return HoverMessageList.characterScreenKey + " ["+KeyBindingList.characterScreenKey.ToString()+"]" ;
            case HoverMessageList.inventoryScreenKey:
                return HoverMessageList.inventoryScreenKey + " ["+KeyBindingList.inventoryScreenKey.ToString()+"]" ;
            case HoverMessageList.partyScreenKey:
                return HoverMessageList.partyScreenKey + " ["+KeyBindingList.partyScreenKey.ToString()+"]" ;
            case HoverMessageList.journalScreenKey:
                return HoverMessageList.journalScreenKey + " ["+KeyBindingList.journalScreenKey.ToString()+"]" ;
            case HoverMessageList.saveAndLoadScreenKey:
                return HoverMessageList.saveAndLoadScreenKey + " ["+KeyBindingList.loadScreenKey.ToString()+"]" ;
            case HoverMessageList.settingsScreenKey:
                return HoverMessageList.settingsScreenKey + " ["+KeyBindingList.settingsScreenKey.ToString()+"]" ;
            case HoverMessageList.retreatButtonKey:
                return "Retreat";
            case IconList.surpriseIconName:
                return "Surprise Status";
            case HoverMessageList.zoneOfInfluenceKey:
                return HoverMessageList.zoiKey;
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

	public virtual List<IDescribable> getRelatedDescribables()
    {
        List<IDescribable> relatedDescribables = new List<IDescribable>();

        switch(hoverText)
        {
            case HoverMessageList.zoiMessage:
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
    public const string zoneOfInfluenceMessage = "A Trait applied to the Zone's owner and all allies directly adjacent to this creature. Zones of Influence are not applied diagonally.";
    
    private const string actionWheelKey = "Action Wheel";
    private const string actionWheelMessage = "The Action Wheel contains all of the Actions a character can bring into battle. With the exception of Passive Abilities, if an Action is not on the Action Wheel, the character is gaining no benefits from it.";
    public const string passiveSlotsKey = "Passive Slots";

    private const string mainHandWeaponTabKey = "Main Hand Tab";
    private const string mainHandWeaponSlotMessage = "You can equip Main-Hand Weapons from the Character and Inventory Screens.";
    private const string mainHandWeaponTabMessage = "Here you can find all the Main-Hand Weapons you have in your Inventory. " + mainHandWeaponMessage;
    private const string mainHandWeaponMessage = "Equipping a Main-Hand Weapon gives you a new Attack Action on your Action Wheel in Combat.";
    private const string twoHandedWeaponMessage = "This Weapon requires two hands to wield. Two-Handed Weapons have larger ranges and deal more damage than one handed ones, but don't benefit from the damage of your Off Hand. Using a Two-Handed Weapon forfeits the benefits from your Shield for the rest of the turn.";
    private const string oneHandedWeaponMessage = "You only need one hand to wield this Weapon. One-Handed Weapons have shorter ranges and deal less damage than Two-Handed ones, but add the damage of your Off-Hand Weapon to their damage.";

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

    private const string allItemsTabKey = "All Items Tab";
    private const string allItemsTabMessage = "Items of every type.";

    private const string weaponSubtypeMessage = "Main-Hand Weapons provide a new Attack Action on your Action Wheel in Combat. Off-Hand weapons provide extra damage and crit chance when you attack with a One-Handed Weapon.";
    private const string armorSubtypeMessage = "Wearing Armor provides Armor Score, which blocks a percentage of incoming damage. Some pieces of Armor also provide additional benefits.";

    private const string armorScoreKey = "Armor Score";
    private const string armorScoreMessage = "Armor Score reduces incoming Damage by a percentage. A Character's Armor Score cannot reduce incoming Damage below 1. A Character gains Armor Score from the Items they have equipped, their Dexterity Stat, and some Traits/Abilities.";


    public const string actionTypePrefix = "Action Type: ";
    public const string traitTypePrefix = "Trait Type: ";
    public const string damageKey = "Damage";
    private const string damageIconMessage = "The amount of damage this Action deals. Hold 'Alt' to see an Action's Damage Formula. A Damage Formula calculates an Action's damage based on your stats. For example: an Action with a Damage Formula of '3S + 5' deals 3 times your Strength, plus 5.";
    private const string critIconMessage = "The Critical Hit chance of this Action. Hold 'Alt' to see an Action's Crit Formula. A Crit Formula calculates an Action's Critical Hit chance based on your stats. For example: an Action with a Crit Formula of '3D + 5' has a Critical Hit chance of 3 times your Dexterity, plus 5.";
    private const string rangeIconMessage = "The Range of this Action. An Action's Range determines how many spaces it affects, and in what shape. Hold 'Alt' or check the Glossary to see a Range's size and shape.";
    private const string cooldownIconMessage = "The Action's Cooldown. Actions with a Cooldown period are unavailable for a number of rounds after use.";
    private const string slotsIconMessage = "The maximum amount of Action Wheel Slots this Action can take up. Each Slot has it's own Cooldown period: assigning an Action to multiple Slots lets you use it more often.";
    private const string durationIconMessage = "This Action has an effect that lasts multiple rounds, such as applying a Trait to it's target.";

    private const string amountIconMessage = "Quantity";
    private const string worthIconMessage = "An Item's worth in Gold Pieces. A Shopkeeper's Discount affect's the cost of the Items they sell and how much they will pay for Items you sell to them.";
    private const string goldIconMessage = "Your Party's total Gold Pieces";

    private const string goldRewardKey = "Reward";
    private const string goldRewardMessage = "The amount of Gold your Party earned in Combat.";

    private const string invulnerableIconMessage = "Invulnerability provides a flat reduction to incoming Damage per hit. Invulnerability can only reduce incoming Damage down to 1. This reduction is applied before the Damage reduction from Armor Score.";
    private const string vulnerableIconMessage = "Extra Damage that is applied when Damage is taken. Applied before the Damage reduction from Armor Score.";
    private const string healingBoostIconMessage = "Extra Health recovery that is applied when a creature receives Healing.";

    public const string bonusDamageKey = "Bonus Damage";
    private const string bonusDamageMessage = "A Character's Bonus Damage is added to the damage of all of their Abilities. Each Character's Bonus Damage is equal to the highest Base Damage of all of their equipped Weapons. For example, a Weapon with a Damage Formula of '3S + 5' provides 5 Bonus Damage. Hold 'Alt' when viewing a Weapon's Stats to reveal formulas.";

    public const string weaponSlotKey = "Weapon Slots";
    private const string weaponSlotMessage = "The number of Main-Hand Weapons a Character can have equipped to their Action Wheel. The higher a Character's Wisdom, the more Weapon Slots that Character has.";

    private const string stanceWeaponMessage = "Attacks made with Stance Weapons, such as fists and staffs, give the attacker additional stacks of their current Stance.";
    private const string stanceMessage = "Stances are a type of Equipped Passive Ability that provide a beneficial Trait with a stackable bonus. Only one Stance can be equipped at a time.\n\n" + stanceWeaponMessage;
    private const string stanceCostKey = "Stance" + CostIcon.costSuffix;
    private const string stanceCostMessage = "This Action costs Stance Stacks to use. " + stanceMessage;
    public const string traitCostKey = "Trait" + CostIcon.costSuffix;
    private const string traitCostMessage = "This Action costs Trait Stacks to use. Certain Traits can be gained multiple times as 'Stacks'. Each Stackable Trait gains Stacks in a different way. See the Trait's description for details.";

    private const string levelMessage = "Leveling up a character costs 1000 Experience. Gaining a Level will increase Maximum Health, return all missing health, and boost one Primary Stat. The highest Level a character can reach is 20.";
    private const string healthMessage = "A Party Member reduced to 0 health is knocked unconscious, and needs special abilities or items to be awakened in combat. Normal healing items can awaken a Party Member out of combat. If your character loses all of their health, however, they will die and you will lose the game.";
    private const string experienceMessage = "A Character's progress towards their next Level up. Gain Experience from completing quests and defeating some boss monsters. For every 1000 Experience gained, a Character can Level up once.";
    private const string experienceRewardKey = "Combat Experience";
    private const string experienceRewardMessage = "The amount of Experience each Party Member gained from this Combat, whether they participated in it or not. You will only earn Combat Experience if a fight is particularly challenging, or if it was related to a Quest.";

    private const string mandatoryTargetMessage = "This creature must be targeted by all Actions that affect it's side of the field. This creature's allies ignore this restriction. If more than one Mandatory Target share the same side of the field, only one Mandatory Target must be targeted for an Action to be allowed.";
    private const string stunnedTargetMessage = "This creature cannot take Actions while this Trait is applied.";

    private const string bonusHealthMessage = "Extra Health added to your Total Health. Determined by your Strength.";
    private const string criticalHitDamageMessage = "How much extra damage is dealt whenever a critical hit is scored. Determined by a character's Strength.";
    private const string woundResistMessage = "Your chance to ignore a Wound Trait applied to you in Combat. Determined by a character's Strength.";

    private const string extraArmorMessage = "Extra Armor, in addition to that gained from your equipment. Determined by a character's Dexterity.\n\n" + armorScoreMessage;
    private const string surpriseRoundDamageMultiplierMessage = "This is the percentage of extra damage dealt when in a surprise round. Determined by a character's Dexterity.";
    private const string armorPenetrationMessage = "The amount of an Enemy's Armor Score your Actions will ignore. Determined by a Character's Dexterity.";
    private const string armorShredMessage = "A negative modifier to a Creature's Armor Score. Cannot reduce a Creature's Armor Score below 0%.";

    private const string mentalResistMessage = "Your chance to ignore a Mental Trait applied to you in combat. Determined by a character's Wisdom.";
    private const string passiveSlotsMessage = "Passive Slots are Action Slots that can only be occupied by Equipped Passives, Stances, and Weapons, saving you space on your Action Wheel for Actions you wish to activate. Actions equippable to Passive Slots can still be equipped to the Action Wheel if desired. Determined by a character's Wisdom.";

    private const string synergyMessage = "Party Members get to add their Synergy to the damage they deal, and subtract it from the damage they take, per Zone of Influence they are inside. Determined by a character's Charisma.";
    private const string bonusExuberancesMessage = "The number of Exuberances your Party has at the start of Combat. Having more Starting Exuberances allows you to use Abilities with Exuberance costs faster and more often. Determined by a character's Charisma.";
    public const string zoiMessage = "A Zone of Influence Trait is a Trait applied to all Allies adjacent to this Character in Combat. Each Character's Zone of Influence Trait is different, but the potency of that Trait is determined by a Character's Charisma.";

    public const string zoiKey = "Zone of Influence";

    private const string characterAbilityKey = "Character Abilities";
    private const string characterAbilityMessage = "Each Party Member gets a number of unique Abilities they unlock at certain Levels.";

    private const string statPointKey = "Stat Points";
    private const string statPointMessage = "This shows how many times you can increase your Primary Stats. The four Primary Stats are Strength, Dexterity, Wisdom, and Charisma.";

    private const string compassKey = "Compass";
    private const string hostilityKey = "Hostility";
    private const string hostilityMessage = "If an Area's Hostility is green, that means that you cannot be attacked by random monsters. Areas with red Hostility may contain random Monsters. Yellow Hosility means you have committed a crime. When the bars of an area fill up with yellow, they will turn red and guards will be sent after you.";
    private const string footingKey = "Footing";
    private const string footingMessage = "Some enemies will chase you when you get too close. These enemies only move half as fast as you. When the Left Foot is visibile, enemies chasing you will move the next time you take a step.";

    private const string strengthMessage = "This Primary Stat bolsters a character's Maximum Health, Critical Hit Damage, and Wound Resistance. Strength also governs the Intimidate Skill.";
    private const string dexterityMessage = "This Primary Stat bolsters a character's Armor, Surprise Round Damage Modifier, and Armor Penetration. Dexterity also governs the Cunning Skill.";
    private const string wisdomMessage = "This Primary Stat bolsters a character's Mental Resistance. Wisdom also provides bonus Passive Slots, increases the number of Weapons you can have equipped, and governs the Observation Skill.";
    private const string charismaMessage = "This Primary Stat increases your Synergy, gives access to Exuberances, and boosts a character's Zone of Influence. Charisma also governs the Leadership Skill.";

    private const string usableItemInventoryTabMessage = "Usable Items Tab.";
    private const string usableItemOOCSubMessage = " Usable Items that can be activated out of combat can be found here." + howToUseItemMessage;
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

    private const string startingRedKnifeMessage = "This shows the amount of the Red Knife Exuberance your Party will gain at the start of Combat. Exuberances are used to power certain Abilities, usually tied to the Charisma Stat.\n\n" + AbilityList.redKnifeAcquisitionMethodExplanation;
    private const string redKnifeMessage = "This shows the amount of the Red Knife Exuberance your Party currently has. Exuberances are used to power certain Abilities, usually tied to the Charisma Stat.\n\n" + AbilityList.redKnifeAcquisitionMethodExplanation;
    private const string startingBlueShieldMessage = "This shows the amount of the Blue Shield Exuberance your Party will gain at the start of Combat. Exuberances are used to power certain Abilities, usually tied to the Charisma Stat.\n\n" + AbilityList.blueShieldAcquisitionMethodExplanation;
    private const string blueShieldMessage = "This shows the amount of the Blue Shield Exuberance your Party currently has. Exuberances are used to power certain Abilities, usually tied to the Charisma Stat.\n\n" + AbilityList.blueShieldAcquisitionMethodExplanation;
    private const string startingYellowThornMessage = "This shows the amount of the Yellow Thorn Exuberance your Party will gain at the start of Combat. Exuberances are used to power certain Abilities, usually tied to the Charisma Stat.\n\n" + AbilityList.yellowThornAcquisitionMethodExplanation;
    private const string yellowThornMessage = "This shows the amount of the Yellow Thorn Exuberance your Party currently has. Exuberances are used to power certain Abilities, usually tied to the Charisma Stat.\n\n" + AbilityList.yellowThornAcquisitionMethodExplanation;
    private const string startingGreenLeafMessage = "This shows the amount of the Grean Leaf Exuberance your Party will gain at the start of Combat. Exuberances are used to power certain Abilities, usually tied to the Charisma Stat.\n\n" + AbilityList.greenLeafAcquisitionMethodExplanation;
    private const string greenLeafMessage = "This shows the amount of the Grean Leaf Exuberance your Party currently has. Exuberances are used to power certain Abilities, usually tied to the Charisma Stat.\n\n" + AbilityList.greenLeafAcquisitionMethodExplanation;
    

    private const string exuberanceCostMessage = "This Action costs Exuberances to use. Exuberances are resources you receive for completing certain feats in combat. There are four types of Exuberances: Red Knife, Blue Shield, Yellow Thorn, and Green Leaf.\n\n";
    private const string redKnifeCostMessage = exuberanceCostMessage + AbilityList.redKnifeAcquisitionMethodExplanation;
    private const string blueShieldCostMessage = exuberanceCostMessage + AbilityList.blueShieldAcquisitionMethodExplanation;
    private const string yellowThornCostMessage = exuberanceCostMessage + AbilityList.yellowThornAcquisitionMethodExplanation;
    private const string greenLeafCostMessage = exuberanceCostMessage + AbilityList.greenLeafAcquisitionMethodExplanation;
    
    #region Action Type Messages
    public const string abilityActionTypeMessage = "An Ability is a Combat Action that is gained by meeting certain Level and/or stat requirements. Abilities often have cooldown periods after use, and may be able to take up multiple Slots on the Action Wheel.\n\n<b>Abilities are the only Action Type to benefit from Bonus Damage.</b>";
    public const string attackActionTypeMessage = "Attacks are Combat Actions that are gained by equipping a Weapon. Attacks have no cooldown. Each Party Member can only have a certain number of Attacks equipped to their Action Wheel, determined by that Character's Wisdom.\n\nItems held in the Off Hand Slot add their Damage to Attacks provided by One Handed Weapons, but not Two Handed Weapons.";
    public const string itemActionTypeMessage = "Item Actions are Actions provided by equipping Usable Items to the Action Wheel. Most Item Actions consume the equipped Item on use. Some Item Actions do not require a Party Action to use, allowing them to be used instantly.";
    public const string passiveActionTypeMessage = "Passive Abilities are Abilities that are always on, and do not require being equipped to the Action Wheel to provide their benefits.";
    public const string equippedPassiveActionTypeMessage = "Equipped Passives are Abilities that provide a Trait to a Character at the beginning of Combat. All Equipped Passives require that a Character equips them to the Action Wheel before that Character will begin to gain their benefits.";

    #endregion

    #region Trait Type Messages
    public const string boostTraitTypeMessage = "Boost Traits provide offensive effects, like increasing speed or damage dealt.";
    public const string chargeTraitTypeMessage = "Charge Traits allow a creature to cast more powerful Abilities.";
    public const string equippedPassiveTraitTypeMessage = "An Equipped Passive Trait is a Trait provided by an Equipped Passive Ability. A Equipped Passive Trait is permanent so long as the Ability that provided the Trait is equipped to your Action Wheel.";
    public const string foeTypeTraitTypeMessage = "There are three types of Creatures: Masters, Minions, and Summons. Master Creatures are the leaders of a pack of Enemies, and must be defeated to win in combat. Minion and Summon Creatures are typically less powerful than, and take orders, from Master Creatures. Minions and Summons do not need to be defeated to win.";
    public const string influenceTraitTypeMessage = "Influence Traits are Traits provided by a Party Member's Zone of Influence. A Character's Zone of Influence is each tile directly adjacent to that Character. Moving out of a Character's Zone of Influence will remove that Character's Influence Trait.";
    public const string interactionTraitTypeMessage = "These Traits explain how this creature interacts with certain types of Actions, granting immunities to certain effects or enabling others.";
    public const string mentalTraitTypeMessage = "Mental Traits are harmful effects applied to a creature. You and your allies have a chance to resist Mental Traits with your Mental Resistance.";
    public const string onDeathTraitTypeMessage = "On Death Traits are Traits that cause an Action to occur when the Trait holder is killed.";
    public const string protectionTraitTypeMessage = "These traits provide defensive effects, like reducing incoming damage or preventing targeting.";
    public const string sizeTraitTypeMessage = "A Size trait means that a Creature takes up multiple squares. Attacking more than one square the same Creature occupies will hurt that Creature multiple times.";
    public const string targetPriorityTraitTypeMessage = "Target Priority Traits determine who and where a creature is allowed to attack, such as prioritizing closer targets, or attacking randomly.";
    public const string woundTraitTypeMessage = "Wound Traits are harmful effects applied to a Creature. Party Members have a chance to resist Wound Traits with their Wound Resistance.";
    #endregion

    private const string regenMessage = "How much health each of your party members will heal after every combat. Determined by your Party's total Strength and Wisdom.";
    private const string surpriseRoundAmountMessage = "The number of rounds of extra Actions you will receive whenever you surprise an enemy. Determined by your Party's total Dexterity.";
    private const string retreatChanceMessage = "Your chance to successfully retreat from combat. Determined by your Party's total Dexterity and Wisdom.";
    private const string volleyAccuracyMessage = "Extra accuracy applied whenever your party performs a Volley action. Determined by your Party's total Wisdom and Charisma.";
    private const string goldMultiplierMessage = "Extra Gold received from combat. Can be gained by equipping certain Items and Abilities.";
    private const string partySlotsMessage = "The number of Party Members your Party can bring into combat. Determined by the highest Level of any of your Party Members, as well as your Party's total Wisdom and Charisma.";
    private const string partyActionsMessage = "The number of Actions your Party can perform each round in combat. Determined by the highest Level of any of your Party Members, as well as your Party's total Dexterity and Charisma.";
    public const string discountKey = "Discount";
    private const string discountMessage = "The total difference in an item's price, based on your Party's total Charisma. Sometimes merchants will give extra discounts or penalties based on what you've done for them or to them. A negative discount means the merchant is making their goods more expensive.";

    public const string retreatButtonKey = "Retreat Button";
    private const string retreatButtonMessage = "Click here to Retreat. The percentage shown on the button is the chance Retreating will succeed. If the Party fails to Retreat, the enemy will take their entire turn before the Party gets to act again. Even if Retreating succeeds, the enemy will be fully restored when the Party returns. Combat entered through dialogue cannot be retreated from.\n\n<i>Be careful who you pick a fight with!</i>";

    private const string actionOrderKey = "Action Order";
    private const string actionOrderMessage = "Most Actions can only be performed between rounds, and will be added to the Action Order. Unless otherwise stated, the Action Order will alternate between Actions performed by your allies, and by your enemies. When you resolve the turn, all Actions in the Action Order will occur in the order they are displayed, starting at the top. To learn who is performing an Action in the Action Order, who it is targeting, and what it will do, hover your mouse over the Action's row.";

    private const string questJournalTabKey = "Quest Tab";
    private const string glossaryJournalTabKey = "Glossary Tab";

    public const string masterMessage = "Defeating this Creature moves the Party closer to Victory.";
    public const string minionMessage = "When all Master Creatures are defeated, this Creature will flee. It does not need to be killed to achieve Victory.";

    public const string characterScreenKey = "Character Screen";
    private const string characterScreenMessage = "Here you can check your Character's Stats, change equipped Abilities, and spend Exp to Level Up.";
    public const string inventoryScreenKey = "Inventory Screen";
    private const string inventoryScreenMessage = "Here you can see what Items you have picked up, or change your Character's Equipment";
    public const string partyScreenKey = "Party Screen";
    private const string partyScreenMessage = "Here you can see your Party's Stats and Skills, swap out Party Members, and change your Party's starting Formation.";
    public const string journalScreenKey = "Journal Screen";
    private const string journalScreenMessage = "Here you can see what quests you have, and look up Terms in the Glossary.";
    public const string saveAndLoadScreenKey = "Save/Load Screen";
    public const string settingsScreenKey = "Settings Screen";
    private const string settingsScreenMessage = "Here you can change the Game's Settings, or Quit the Game.";

    private const string restPointMessage = "This location has a Rest Point. Rest Points will restore the Health of all Party Members, as well as any expended Skill charges.";
    private const string shopIconMessage = "This location has a Shop. Shops sell useful items or equipment and buy unwanted loot.";

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

            case allItemsTabKey:
                    return allItemsTabMessage;
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

            #region Action Types
            case actionTypePrefix + AbilityList.abilityActionTypeName:
                return abilityActionTypeMessage;
            case actionTypePrefix + AbilityList.attackActionTypeName:
                return attackActionTypeMessage;
            case actionTypePrefix + AbilityList.itemActionTypeName:
                return itemActionTypeMessage;
            case actionTypePrefix + AbilityList.passiveActionTypeName:
                return passiveActionTypeMessage;
            case actionTypePrefix + AbilityList.equippedPassiveActionTypeName:
                return equippedPassiveActionTypeMessage;
            #endregion

            #region Trait Types

            case traitTypePrefix + TraitList.boostName:
                return boostTraitTypeMessage;
            case traitTypePrefix + TraitList.chargeName:
                return chargeTraitTypeMessage;
            case traitTypePrefix + AbilityList.equippedPassiveActionTypeName:
                return equippedPassiveTraitTypeMessage;
            case traitTypePrefix + TraitList.foeTypeName:
                return foeTypeTraitTypeMessage;
            case traitTypePrefix + TraitList.influenceName:
                return influenceTraitTypeMessage;
            case traitTypePrefix + TraitList.mentalName:
                return mentalResistMessage;
            case traitTypePrefix + TraitList.onDeathName:
                return onDeathTraitTypeMessage;
            case traitTypePrefix + TraitList.protectionName:
                return protectionTraitTypeMessage;
            case traitTypePrefix + TraitList.sizeName:
                return sizeTraitTypeMessage;
            case traitTypePrefix + TraitList.targetPriorityName:
                return targetPriorityTraitTypeMessage;
            case traitTypePrefix + TraitList.woundName:
                return woundTraitTypeMessage;
            #endregion

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
            case IconList.healingBoostIconName:
                return healingBoostIconMessage;

            case IconList.stanceIconName:
                return stanceMessage;
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

            case IconList.mandatoryTargetIcon:
                return mandatoryTargetMessage;
            case IconList.stunnedIcon:
                return stunnedTargetMessage;
            case IconList.minionIcon:
                return minionMessage;
            case IconList.masterIcon:
                return masterMessage;

            case IconList.strengthIconName:
            case Strength.symbolChar:
                return strengthMessage;
            case IconList.dexterityIconName:
            case Dexterity.symbolChar:
                return dexterityMessage;
            case IconList.wisdomIconName:
            case Wisdom.symbolChar:
                return wisdomMessage;
            case IconList.charismaIconName:
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
            case IconList.woundResistIconName:
                return woundResistMessage;
            case IconList.regenIconName:
                return regenMessage;
            case IconList.cunningIconName:
                return cunningMessage;
            case IconList.bonusArmorIconName:
                return extraArmorMessage;
            case IconList.surpriseRoundDamageMultiplierIconName:
                return surpriseRoundDamageMultiplierMessage;
            case IconList.surpriseRoundAmountIconName:
                return surpriseRoundAmountMessage;
            case IconList.observationIconName:
                return observationMessage;
            case IconList.armorPenetrationIconName:
                return armorPenetrationMessage;
            case IconList.armorShredIconName:
                return armorShredMessage;
            case IconList.mentalResistIconName:
                return mentalResistMessage;
            case IconList.retreatChanceIconName:
                return retreatChanceMessage;
            case IconList.passiveSlotsIconName:
                return passiveSlotsMessage;
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
                if(CombatStateManager.inCombat)
                {
                    return redKnifeMessage;
                } else
                {
                    return startingRedKnifeMessage;
                }
            case IconList.blueShieldIconName:
                if(CombatStateManager.inCombat)
                {
                    return blueShieldMessage;
                } else
                {
                    return startingBlueShieldMessage;
                }
            case IconList.yellowThornIconName:
                if(CombatStateManager.inCombat)
                {
                    return yellowThornMessage;
                } else
                {
                    return startingYellowThornMessage;
                }
            case IconList.greenLeafIconName:
                if(CombatStateManager.inCombat)
                {
                    return greenLeafMessage;
                } else
                {
                    return startingGreenLeafMessage;
                }

            case IconList.redKnifeIconName + CostIcon.costSuffix:
                return redKnifeCostMessage;
            case IconList.blueShieldIconName + CostIcon.costSuffix:
                return blueShieldCostMessage;
            case IconList.yellowThornIconName + CostIcon.costSuffix:
                return yellowThornCostMessage;
            case IconList.greenLeafIconName + CostIcon.costSuffix:
                return greenLeafCostMessage;

            case stanceCostKey:
                return stanceCostMessage;

            case traitCostKey:
                return traitCostMessage;

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
                return "Here you can save your game. Remember, you can also Quick Save with <nobr>' " + KeyBindingList.quicksaveKey.ToString() + " '</nobr>, and the game will take an Autosave whenever you enter an area. You can have up to three Autosaves at a time.";
            case settingsScreenKey:
                return settingsScreenMessage;

            case zoneOfInfluenceKey:
            case zoiKey:
                return zoiMessage;

            case IconList.restPointIcon:
                return restPointMessage;
            case IconList.shopIcon:
                return shopIconMessage;

            default:

                if(iconName != null && iconName.Contains(zoneOfInfluenceKey))
                {
                    return zoiMessage;
                }

                return "";
        }
    }
}
