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

            if(eventData != null)
            {
                eventData.Use();
            }
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
            case HoverMessageList.restPointCharacterKey:
                return IconList.restPointIcon;
            case HoverMessageList.intimidatedShopkeeperKey:
                return HoverMessageList.intimidatedKey;
            case IconList.intimidateIconName:
            case IconList.cunningIconName:
            case IconList.observationIconName:
            case IconList.leadershipIconName:

                switch(PlayerOOCStateManager.currentActivity)
                {
                    case OOCActivity.walking:
                    case OOCActivity.inChestUI:
                    case OOCActivity.cunning:
                    case OOCActivity.intimidating:
                    case OOCActivity.observing:
                    case OOCActivity.inFade:
                        return hoverMessageKey  + " ["+KeyBindingList.skillKey.ToString()+"]" ;
                    default:
                        return hoverMessageKey;

                }
            case HoverMessageList.localMapKey:
                return HoverMessageList.localMapKey + " ["+KeyBindingList.mapKey.ToString()+"]" ;
            case HoverMessageList.worldMapKey:
                return HoverMessageList.worldMapKey + " ["+KeyBindingList.worldMapKey.ToString()+"]" ;
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
    
    public const string actionWheelKey = "Action Wheel";
    public const string actionWheelMessage = "The Action Wheel contains all of the Actions a character can bring into battle. With the exception of Passive Abilities, if an Action is not on the Action Wheel, the character is gaining no benefits from it.";
    public const string passiveSlotsKey = "Passive Slots";

    public const string mainHandWeaponTabKey = "Main Hand Tab";
    public const string mainHandWeaponSlotMessage = "You can equip Main-Hand Weapons from the Character and Inventory Screens.";
    public const string mainHandWeaponTabMessage = "Here you can find all the Main-Hand Weapons you have in your Inventory. " + mainHandWeaponMessage;
    public const string mainHandWeaponMessage = "Equipping a Main-Hand Weapon gives you a new Attack Action on your Action Wheel in Combat.";
    public const string twoHandedWeaponMessage = "This Weapon requires two hands to wield. Two-Handed Weapons have larger ranges and deal more damage than one handed ones, but don't benefit from the damage of your Off Hand. Using a Two-Handed Weapon forfeits the benefits from your Shield for the rest of the turn.";
    public const string oneHandedWeaponMessage = "You only need one hand to wield this Weapon. One-Handed Weapons have shorter ranges and deal less damage than Two-Handed ones, but add the damage of your Off-Hand Weapon to their damage.";

    public const string offhandSlotMessage = "Off Hand Slot";
    public const string offhandSubMessage = "Off-Hand Weapons give you extra Damage and Crit Chance when you attack with a One-Handed Weapon. Shields give extra Armor as long as you haven't attacked with a Two-Handed Weapon this turn.";
    public const string headSlotMessage = "Head Slot";
    public const string bodySlotMessage = "Body Slot";
    public const string handsSlotMessage = "Hand Slot";
    public const string feetSlotMessage = "Feet Slot";
    public const string trinketSlotMessage = "Trinket Slot";

    public const string keySubtypeMessage = "Keys can be used to open locks on chests and doors. Keys cannot be sold.";
    public const string questSubtypeMessage = "Quest Items are needed to complete specific Quest objectives. Quest Items cannot be sold.";
    public const string treasureSubtypeMessage = "The only purpose of a Treasure Item is to be sold. Treasure Items cannot be removed from your Junk pocket.";
    public const string bookSubtypeMessage = "Using a book will let you read its contents.";
    public const string healingSubtypeMessage = "This item can be used to heal you or your allies, in or out of Combat.";
    public const string usableSubtypeMessage = "Usable Item";
    public const string usableSubMessage = "Some Usable Items heal, apply Traits in Combat, or provide you with information. Most Usable Items are destroyed when used.";

    public const string allItemsTabKey = "All Items Tab";
    public const string allItemsTabMessage = "Items of every type.";

    public const string weaponSubtypeMessage = "Main-Hand Weapons provide a new Attack Action on your Action Wheel in Combat. Off-Hand Weapons provide extra damage and crit chance when you attack with a One-Handed Weapon.";
    public const string armorSubtypeMessage = "Wearing Armor provides Armor Score, which blocks a percentage of incoming damage. Some pieces of Armor also provide additional benefits.";

    public const string armorScoreKey = "Armor Score";
    public const string armorScoreMessage = "Armor Score reduces incoming Damage by a percentage. A Character's Armor Score cannot reduce incoming Damage below 1. A Character gains Armor Score from the Items they have equipped, their Dexterity Stat, and some Traits/Abilities.";


    public const string actionTypePrefix = "Action Type: ";
    public const string traitTypePrefix = "Trait Type: ";
    public const string damageKey = "Damage";
    public const string damageIconMessage = "The amount of damage this Action deals. Hold 'Alt' to see an Action's Damage Formula. A Damage Formula calculates an Action's damage based on your stats. For example: an Action with a Damage Formula of '3S + 5' deals 3 times your Strength, plus 5.";
    public const string critIconMessage = "The Critical Hit chance of this Action. Hold 'Alt' to see an Action's Crit Formula. A Crit Formula calculates an Action's Critical Hit chance based on your stats. For example: an Action with a Crit Formula of '3D + 5' has a Critical Hit chance of 3 times your Dexterity, plus 5.";
    public const string rangeIconMessage = "The Range of this Action. An Action's Range determines how many spaces it affects, and in what shape. Hold 'Alt' or check the Glossary to see a Range's size and shape.";
    public const string cooldownIconMessage = "The Action's Cooldown. Actions with a Cooldown period are unavailable for a number of rounds after use.";
    public const string slotsIconMessage = "The maximum amount of Action Wheel Slots this Action can take up. Each Slot has it's own Cooldown period: assigning an Action to multiple Slots lets you use it more often.";
    public const string durationIconMessage = "This Action has an effect that lasts multiple rounds, such as applying a Trait to it's target.";

    public const string amountIconMessage = "Quantity";
    public const string worthIconMessage = "An Item's worth in Gold Pieces. A Shopkeeper's Discount affects the cost of the Items they sell and how much they will pay for Items you sell to them.";
    public const string goldIconMessage = "Your Party's total Gold Pieces";

    public const string goldRewardKey = "Reward";
    public const string goldRewardMessage = "The amount of Gold your Party earned in Combat.";

    public const string invulnerableIconMessage = "Invulnerability provides a flat reduction to incoming Damage per hit. Invulnerability can only reduce incoming Damage down to 1. This reduction is applied before the Damage reduction from Armor Score.";
    public const string vulnerableIconMessage = "Extra Damage that is applied when Damage is taken. Applied before the Damage reduction from Armor Score.";
    public const string healingBoostIconMessage = "Extra Health recovery that is applied when a creature receives Healing.\n\nThis stat only affects incoming Healing, and not Healing done by this Creature that targets other Creatures.";

    public const string bonusDamageKey = "Bonus Damage";
    public const string bonusDamageMessage = "A Character's Bonus Damage is added to the damage of all of their Abilities. Each Character's Bonus Damage is equal to the highest Base Damage of all of their equipped Weapons. For example, a Weapon with a Damage Formula of '3S + 5' provides 5 Bonus Damage. Hold 'Alt' when viewing a Weapon's Stats to reveal formulas.";

    public const string weaponSlotKey = "Weapon Slots";
    public const string weaponSlotMessage = "The number of Main-Hand Weapons a Character can have equipped to their Action Wheel. The higher a Character's Wisdom, the more Weapon Slots that Character has.";

    public const string stanceWeaponMessage = "Attacks made with Stance Weapons, such as fists and staffs, give the attacker additional stacks of their current Stance.";
    public const string stanceMessage = "Stances are a type of Equipped Passive Ability that provide a beneficial Trait with a stackable bonus. Only one Stance can be equipped at a time.\n\n" + stanceWeaponMessage;
    public const string stanceCostKey = "Stance" + CostIcon.costSuffix;
    public const string stanceCostMessage = "This Action costs Stance Stacks to use. " + stanceMessage;
    public const string traitCostKey = "Trait" + CostIcon.costSuffix;
    public const string traitCostMessage = "This Action costs Trait Stacks to use. Certain Traits can be gained multiple times as 'Stacks'. Each Stackable Trait gains Stacks in a different way. See the Trait's description for details.";

    public const string levelMessage = "Leveling up a character costs 1000 Experience. Gaining a Level will increase Maximum Health, return all missing health, and boost one Primary Stat. The highest Level a character can reach is 20.";
    public const string healthMessage = "A Party Member reduced to 0 health is knocked unconscious, and needs special Abilities or Items to be awakened in Combat. Normal healing Items can awaken a Party Member out of Combat. If your Character loses all of their health, however, they will die and you will lose the game.";
    public const string experienceMessage = "A Character's progress towards their next Level up. Gain Experience from completing quests and defeating some boss monsters. For every 1000 Experience gained, a Character can Level up once.";
    public const string experienceRewardKey = "Combat Experience";
    public const string experienceRewardMessage = "The amount of Experience each Party Member gained from this Combat, whether they participated in it or not. You will only earn Combat Experience if a fight is particularly challenging, or if it was related to a Quest.";

    public const string mandatoryTargetMessage = "This creature must be targeted by all Actions that affect it's side of the field. This creature's allies ignore this restriction. If more than one Mandatory Target share the same side of the field, only one Mandatory Target must be targeted for an Action to be allowed.";
    public const string stunnedTargetMessage = "This creature cannot take Actions while this Trait is applied.";

    public const string bonusHealthMessage = "Extra Health added to your Total Health. Determined by your Strength.";
    public const string criticalHitDamageMessage = "How much extra damage is dealt whenever a critical hit is scored. Determined by a character's Strength.";
    public const string woundResistMessage = "Your chance to ignore a Wound Trait applied to you in Combat. Determined by a character's Strength.";

    public const string extraArmorMessage = "Extra Armor, in addition to that gained from your equipment. Determined by a character's Dexterity.\n\n" + armorScoreMessage;
    public const string surpriseRoundDamageMultiplierMessage = "This is the percentage of extra damage dealt when in a surprise round. Determined by a character's Dexterity.";
    public const string armorPenetrationMessage = "The amount of an Enemy's Armor Score your Actions will ignore. Determined by a Character's Dexterity.";
    public const string armorShredMessage = "A negative modifier to a Creature's Armor Score. Cannot reduce a Creature's Armor Score below 0%.";

    public const string mentalResistMessage = "Your chance to ignore a Mental Trait applied to you in Combat. Determined by a character's Wisdom.";
    public const string passiveSlotsMessage = "Passive Slots are Action Slots that can only be occupied by Equipped Passives, Stances, and Weapons, saving you space on your Action Wheel for Actions you wish to activate. Actions equippable to Passive Slots can still be equipped to the Action Wheel if desired. Determined by a character's Wisdom.";

    public const string synergyMessage = "Party Members get to add their Synergy to the damage they deal, and subtract it from the damage they take, per Zone of Influence they are inside. Determined by a character's Charisma.";
    public const string bonusExuberancesMessage = "The number of Exuberances your Party has at the start of Combat. Having more Starting Exuberances allows you to use Abilities with Exuberance costs faster and more often. Determined by a character's Charisma.";
    public const string zoiMessage = "A Zone of Influence Trait is a Trait applied to all Allies adjacent to this Character in Combat. Each Character's Zone of Influence Trait is different, but the potency of that Trait is determined by a Character's Charisma.";

    public const string zoiKey = "Zone of Influence";

    public const string characterAbilityKey = "Character Abilities";
    public const string characterAbilityMessage = "Each Party Member gets a number of unique Abilities they unlock at certain Levels.";

    public const string statPointKey = "Stat Points";
    public const string statPointMessage = "This shows how many times you can increase your Primary Stats. The four Primary Stats are Strength, Dexterity, Wisdom, and Charisma.";

    public const string compassKey = "Compass";
    public const string zoneHostilityKey = "Zone Hostility";
    public const string zoneHostilityMessage = "This shows the current Zone's Hostility. Zones are groupings of Locations, shown on the Local Map.\n\nIf this symbol shows a flower, the Zone is not Hostile. If it shows a skull, the Zone is Hostile, and the Party can expect Enemies to attack them in most Locations within this Zone.";
    public const string locationHostilityKey = "Location Hostility";
    public const string locationHostilityMessage = "This shows the current Location's Hostility. Locations are spaces within a Zone, such as a specific room, building, or street. To see the Party's current Location, check the Local Map.\n\nIf this symbol shows a flower, the Location is not Hostile and the Party won't be attacked by wandering monsters while exploring it. If it shows a skull, the Location is Hostile, and the Party can be attacked here.\n\nIf the Party is in a non-Hostile Zone, and they enter a Hostile Location within that Zone, the rest of the Zone will <b>not</b> turn Hostile.";
    public const string zoneAlertnessKey = "Zone Alertness";
    public const string zoneAlertnessMessage = "The number of Exclamation Marks shows the current Zone's Alertness Level. The Alertness Level of a Zone can be raised by attacking NPC's, using Skills on certain NPC's and objects, and certain Dialogue Choices. Should the Party reach level 5 Alertness, the entire Zone will turn Hostile, and guards will be sent to after them. Turning a Zone Hostile can affect the outcome of Quests, and may be irreversible.\n\n<i>Be careful who you attack!</i>";

    public const string footingKey = "Footing";
    public const string footingMessage = "Some enemies will chase you when you get too close. These enemies only move half as fast as you. When the Left Foot is visibile, enemies chasing you will move the next time you take a step.";

    public const string strengthMessage = "This Primary Stat bolsters a character's Maximum Health, Critical Hit Damage, and Wound Resistance. Strength also governs the Intimidate Skill.";
    public const string dexterityMessage = "This Primary Stat bolsters a character's Armor, Surprise Round Damage Modifier, and Armor Penetration. Dexterity also governs the Cunning Skill.";
    public const string wisdomMessage = "This Primary Stat bolsters a character's Mental Resistance. Wisdom also provides bonus Passive Slots, increases the number of Weapons you can have equipped, and governs the Observation Skill.";
    public const string charismaMessage = "This Primary Stat increases your Synergy, gives access to Exuberances, and boosts a character's Zone of Influence. Charisma also governs the Leadership Skill.";

    public const string tabSuffix = " Tab";
    public const string strengthTabKey = IconList.strengthIconName + tabSuffix;
    public const string dexterityTabKey = IconList.dexterityIconName + tabSuffix;
    public const string wisdomTabKey = IconList.wisdomIconName + tabSuffix;
    public const string charismaTabKey = IconList.charismaIconName + tabSuffix;
    public const string strengthTabMessage = "Here you can see all Abilities a Character can learn by raising the Strength Stat when gaining a Level. Strength Abilities tend to be larger, flashier, and deal their damage on impact, rather than over time.";
    public const string dexterityTabMessage = "Here you can see all Abilities a Character can learn by raising the Dexterity Stat when gaining a Level. Dexterity Abilities tend to be more selective with who they target, and provide debilitating effects along with their damage.";
    public const string wisdomTabMessage = "Here you can see all Abilities a Character can learn by raising the Wisdom Stat when gaining a Level. Wisdom Abilities tend to be more tactically oriented, and can bolster your Party Members or hinder your enemies in equal measure.\n\nMost Wisdom Abilities require Stance Stacks to activate.";
    public const string charismaTabMessage = "Here you can see all Abilities a Character can learn by raising the Charisma Stat when gaining a Level. Charisma Abilities tend to be more supportive in nature, healing Party Members or encouraging them to fight harder.\n\nMost Charisma Abilities require one or more Exuberances to activate.";


    public const string usableItemInventoryTabMessage = "Usable Items Tab.";
    public const string usableItemOOCSubMessage = " Usable Items that can be activated out of Combat can be found here." + howToUseItemMessage;
    public const string howToUseItemMessage = " <B>To Use a Usable Item, drag the Item onto the Party Member you want to use it on while on the Inventory Screen.</B>";
    public const string offHandTabMessage = "Off Hand Tab.";
    public const string armorTabKey = "Armor Tab";
    public const string armorTabMessage = "Equipping Armor is the main way to boost your Armor Score and reduce incoming damage. Some Armor provides additional benefits.";
    public const string essentialTabKey = "Essential Tab";
    public const string essentialTabMessage = "Essential Items such as Quest Items and Keys cannot be sold to a Merchant.";
    public const string junkTabKey = "Junk Tab";
    public const string junkTabMessage = "Here you can see all the Items you have marked as Junk.";
    public const string junkSubMessage = " All Items marked as Junk can be sold simultaneously to a Merchant. Treasure Items are always marked as Junk.";

    public const string junkSlotKey = "Junk Slot";
    public const string junkSlotMessage = "Drag Items here to mark them as Junk.";
    public const string toInvSlotKey = "To Inv Slot";
    public const string toInvSlotMessage = "Drag items here to remove them from Junk. Treasure Items cannot be removed from Junk.";
    public const string buySlotKey = "Buy Slot";
    public const string buySlotMessage = "Drag Items here to buy them.";
    public const string sellSlotKey = "Sell Slot";
    public const string sellSlotMessage = "Drag Items here to sell them.";

    public const string skillsLabelKey = "Skills";
    public const string skillsMessage = "Skills are Abilities that are usable outside of Combat. Unlock Skills by upgrading your Primary Stats. Proficiency in each Skill is based on the the highest Primary Stat of all Party Members.";
    public const string partyStatsLabelKey = "Party Stats";
    public const string partyStatsMessage = "Party Stats are Stats that reflect your Party's combined knowledge. Each Party Stat's progression is based on one or more of your Party's total Primary Stats.";
    public const string exuberancesLabelKey = "Exuberances";
    public const string exuberancesMessage = "Exuberances are resources that you can spend to activate powerful abilities during Combat. To unlock Exuberances, at least one of your Party Members must have two or more Charisma.";


    public const string intimidateMessage = "This Skill can be used to stop enemies from ambushing you, but prevents you from ambushing them in turn. Your Intimidate charges are determined by the highest Strength of all Party Members.\n\n"  + Constants.skillChargeDescription;
    public const string cunningMessage = "This Skill is used out of Combat to fool enemies, turning them around and stunning them for a few steps, allowing you to ambush them or sneak around them. Some objects in the overworld can be activated with Cunning as well. Your Cunning charges are determined by the highest Dexterity of all Party Members.\n\n"  + Constants.skillChargeDescription;
    public const string observationMessage = "This Skill allows you to find hidden doors and lost secrets. Face your Character towards a suspicious object or wall and use Observation to check if the target can be interacted with. If the target contains a secret, it will change color. Determined by the highest Wisdom of all Party Members.\n\nObservation has no limit on its use.";
    public const string leadershipMessage = "This Skill allows you to command your followers out of Combat, telling them to stand on buttons or in doorways to block enemy movement. The number of Party Members you can command with Leadership is determined by the highest Charisma of all Party Members.\n\nLeadership does not use Skill Charges, but is instead limited by the number of Companions you have, regardless of whether they are in your Party, and the highest Charisma of all Party Members currently in the Party.";

    public const string intimidatedKey = "Intimidated";
    public const string intimidatedShopkeeperKey = "Intimidated Shopkeeper";
    public const string intimidatedShopkeeperMessage = "This Shopkeeper is afraid of your Party, and will give your Party a discount on goods.";
    public const string intimidatedMessage = "This creature is weary of your presence. It cannot surprise anyone or be surprised.";
    public const string distractedKey = "Distracted";
    public const string distractedMessage = "This creature is distracted. It cannot move for a number of steps.";
    public const string evadedKey = "Evaded";
    public const string evadedMessage = "Your Party has recently retreated from a fight with this creature. It cannot move while your Party makes its escape.";

    public const string noActionRequiredMessage = "This Action does not count against the Party's total Party Actions limit. When activated, it will occur instantly.";

    public const string startingRedKnifeMessage = "This shows the amount of the Red Knife Exuberance your Party will gain at the start of Combat. Exuberances are used to power certain Abilities, usually tied to the Charisma Stat.\n\n" + AbilityList.redKnifeAcquisitionMethodExplanation;
    public const string redKnifeMessage = "This shows the amount of the Red Knife Exuberance your Party currently has. Exuberances are used to power certain Abilities, usually tied to the Charisma Stat.\n\n" + AbilityList.redKnifeAcquisitionMethodExplanation;
    public const string startingBlueShieldMessage = "This shows the amount of the Blue Shield Exuberance your Party will gain at the start of Combat. Exuberances are used to power certain Abilities, usually tied to the Charisma Stat.\n\n" + AbilityList.blueShieldAcquisitionMethodExplanation;
    public const string blueShieldMessage = "This shows the amount of the Blue Shield Exuberance your Party currently has. Exuberances are used to power certain Abilities, usually tied to the Charisma Stat.\n\n" + AbilityList.blueShieldAcquisitionMethodExplanation;
    public const string startingYellowThornMessage = "This shows the amount of the Yellow Thorn Exuberance your Party will gain at the start of Combat. Exuberances are used to power certain Abilities, usually tied to the Charisma Stat.\n\n" + AbilityList.yellowThornAcquisitionMethodExplanation;
    public const string yellowThornMessage = "This shows the amount of the Yellow Thorn Exuberance your Party currently has. Exuberances are used to power certain Abilities, usually tied to the Charisma Stat.\n\n" + AbilityList.yellowThornAcquisitionMethodExplanation;
    public const string startingGreenLeafMessage = "This shows the amount of the Grean Leaf Exuberance your Party will gain at the start of Combat. Exuberances are used to power certain Abilities, usually tied to the Charisma Stat.\n\n" + AbilityList.greenLeafAcquisitionMethodExplanation;
    public const string greenLeafMessage = "This shows the amount of the Grean Leaf Exuberance your Party currently has. Exuberances are used to power certain Abilities, usually tied to the Charisma Stat.\n\n" + AbilityList.greenLeafAcquisitionMethodExplanation;
    

    public const string exuberanceCostMessage = "This Action costs Exuberances to use. Exuberances are resources you receive for completing certain feats in Combat. There are four types of Exuberances: Red Knife, Blue Shield, Yellow Thorn, and Green Leaf.\n\n";
    public const string redKnifeCostMessage = exuberanceCostMessage + AbilityList.redKnifeAcquisitionMethodExplanation;
    public const string blueShieldCostMessage = exuberanceCostMessage + AbilityList.blueShieldAcquisitionMethodExplanation;
    public const string yellowThornCostMessage = exuberanceCostMessage + AbilityList.yellowThornAcquisitionMethodExplanation;
    public const string greenLeafCostMessage = exuberanceCostMessage + AbilityList.greenLeafAcquisitionMethodExplanation;
    
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
    public const string foeTypeTraitTypeMessage = "There are three types of Creatures: Masters, Minions, and Summons. Master Creatures are the leaders of a pack of Enemies, and must be defeated to win in Combat. Minion and Summon Creatures are typically less powerful than, and take orders, from Master Creatures. Minions and Summons do not need to be defeated to win.";
    public const string influenceTraitTypeMessage = "Influence Traits are Traits provided by a Party Member's Zone of Influence. A Character's Zone of Influence is each tile directly adjacent to that Character. Moving out of a Character's Zone of Influence will remove that Character's Influence Trait.";
    public const string interactionTraitTypeMessage = "These Traits explain how this creature interacts with certain types of Actions, granting immunities to certain effects or enabling others.";
    public const string mentalTraitTypeMessage = "Mental Traits are harmful effects applied to a creature. You and your allies have a chance to resist Mental Traits with your Mental Resistance.";
    public const string onDeathTraitTypeMessage = "On Death Traits are Traits that cause an Action to occur when the Trait holder is killed.";
    public const string protectionTraitTypeMessage = "These traits provide defensive effects, like reducing incoming damage or preventing targeting.";
    public const string sizeTraitTypeMessage = "A Size trait means that a Creature takes up multiple squares. Attacking more than one square the same Creature occupies will hurt that Creature multiple times.";
    public const string targetPriorityTraitTypeMessage = "Target Priority Traits determine who and where a creature is allowed to attack, such as prioritizing closer targets, or attacking randomly.";
    public const string woundTraitTypeMessage = "Wound Traits are harmful effects applied to a Creature. Party Members have a chance to resist Wound Traits with their Wound Resistance.";
    #endregion

    public const string regenMessage = "How much health each of your party members will heal after every Combat. Determined by your Party's total Strength and Wisdom.";
    public const string surpriseRoundAmountMessage = "The number of rounds of extra Actions you will receive whenever you surprise an enemy. Determined by your Party's total Dexterity.";
    public const string retreatChanceMessage = "Your chance to successfully retreat from Combat. Determined by your Party's total Dexterity and Wisdom.";
    public const string volleyAccuracyMessage = "Extra accuracy applied whenever your Party performs a Volley action. Determined by your Party's total Wisdom and Charisma.";
    public const string goldMultiplierMessage = "Extra Gold received from Combat. Can be gained by equipping certain Items and Abilities.";
    public const string partySlotsMessage = "The number of Companions your Party can bring into Combat. Determined by the highest Level of any of your Party Members, as well as your Party's total Wisdom and Charisma.";
    public const string partyActionsMessage = "The number of Actions your Party can perform each round in Combat. Determined by the highest Level of any of your Party Members, as well as your Party's total Dexterity and Charisma.";
    public const string discountKey = "Discount";
    public const string discountMessage = "A modifier to an Item's price when purchasing from shops, based on your Party's total Charisma. Sometimes merchants will give extra discounts or penalties based on what you've done for them or to them. A negative discount means the merchant is making their goods more expensive.";

    public const string retreatButtonKey = "Retreat Button";
    public const string retreatButtonMessage = "Click here to Retreat. The percentage shown on the button is the chance Retreating will succeed. If the Party fails to Retreat, the enemy will take their entire turn before the Party gets to act again. Even if Retreating succeeds, the enemy will be fully restored when the Party returns. Combat entered through dialogue cannot be retreated from.\n\n<i>Be careful who you pick a fight with!</i>";

    public const string actionOrderKey = "Action Order";
    public const string actionOrderMessage = "Most Actions can only be performed between rounds, and will be added to the Action Order. Unless otherwise stated, the Action Order will alternate between Actions performed by your allies, and by your enemies. When you resolve the turn, all Actions in the Action Order will occur in the order they are displayed, starting at the top. To learn who is performing an Action in the Action Order, who it is targeting, and what it will do, hover your mouse over the Action's row.";

    public const string questJournalTabKey = "Quest Tab";
    public const string glossaryJournalTabKey = "Glossary Tab";

    public const string masterMessage = "Defeating this Creature moves the Party closer to Victory.";
    public const string minionMessage = "When all Master Creatures are defeated, this Creature will flee. It does not need to be killed to achieve Victory.";

    public const string characterScreenKey = "Character Screen";
    public const string characterScreenMessage = "Here you can check your Character's Stats, change equipped Abilities, and spend Exp to Level Up.";
    public const string inventoryScreenKey = "Inventory Screen";
    public const string inventoryScreenMessage = "Here you can see what Items you have picked up, or change your Character's Equipment";
    public const string partyScreenKey = "Party Screen";
    public const string partyScreenMessage = "Here you can see your Party's Stats and Skills, swap out Party Members, and change your Party's starting Formation.";
    public const string journalScreenKey = "Journal Screen";
    public const string journalScreenMessage = "Here you can see what quests you have, and look up Terms in the Glossary.";
    public const string saveAndLoadScreenKey = "Save/Load Screen";
    public const string settingsScreenKey = "Settings Screen";
    public const string settingsScreenMessage = "Here you can change the Game's Settings, or Quit the Game.";

    public const string localMapKey = "Local Map";
    public const string localMapMessage = "Here you can see where you are in the current Zone, as well as what Quests you have for it.";
    public const string worldMapKey = "World Map";
    public const string worldMapMessage = "Here you can see where you are in the world of " + NPCNameList.fold;

    public const string restPointMessage = "This location has a Rest Point. Using a Rest Point will restore the Health of all Party Members, as well as any expended Skill charges.";
    public const string shopIconMessage = "This location has a Shop. Shops sell useful items or equipment and buy unwanted loot.";

    public const string shopkeeperIconKey = "Shopkeeper";
    public const string shopkeeperIconMessage = "This Character is a Shopkeeper. Shopkeepers sell useful items or equipment and buy unwanted loot.";
    
    public const string restPointCharacterKey = "Rest Point Character";
    public const string restPointCharacterMessage = "This Character serves as a Rest Point. Using a Rest Point will restore the Health of all Party Members, as well as any expended Skill charges.";

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

            case intimidatedShopkeeperKey:
                return intimidatedShopkeeperMessage;
            case intimidatedKey:
                return intimidatedMessage;
            case distractedKey:
                return distractedMessage;
            case evadedKey:
                return evadedMessage;

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

            case strengthTabKey:
                return strengthTabMessage;
            case dexterityTabKey:
                return dexterityTabMessage;
            case wisdomTabKey:
                return wisdomTabMessage;
            case charismaTabKey:
                return charismaTabMessage;

            case compassKey:
                return compassKey;
            case zoneHostilityKey:
                return zoneHostilityMessage;
            case locationHostilityKey:
                return locationHostilityMessage;
            case zoneAlertnessKey:
                return zoneAlertnessMessage;
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
            case IconList.noActionRequiredIconName:
                return noActionRequiredMessage;

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

            case localMapKey:
                return localMapMessage;
            case worldMapKey:
                return worldMapMessage;

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
            case restPointCharacterKey:
                return restPointCharacterMessage;
            case IconList.shopIcon:
                return shopIconMessage;
            case shopkeeperIconKey:
                return shopkeeperIconMessage;

            default:

                if(iconName != null && iconName.Contains(zoneOfInfluenceKey))
                {
                    return zoiMessage;
                }

                return "";
        }
    }
}
