using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This project has its own Key item class in the global namespace, which would win over the Input System's
//Key enum, so the enum is aliased rather than imported.
using InputKey = UnityEngine.InputSystem.Key;

public static class EnumUtil
{
    public static string ToFriendlyString(this Enum template)
    {
        string name = template.ToString();
        string newName = "";

        int index = 0;
        foreach(char c in name)
        {
            if((Char.IsUpper(c) || Char.IsDigit(c)) && 
                index != 0)
            {
                newName += " " + c;
            } else
            {
                newName += c;
            }

            index++;
        }

        return newName;
    }

    private const string keyboardPathPrefix = "<Keyboard>/";
    private const string digitKeyPrefix = "Digit";

    //Legacy KeyCodes and the new Input System's Keys do not line up by name (KeyCode.Alpha1 is Key.Digit1,
    //KeyCode.Return is Key.Enter, KeyCode.Keypad1 is Key.Numpad1), so the pairs are spelled out here.
    //
    //LeftApple and RightApple are deliberately absent - they share their underlying values with LeftCommand
    //and RightCommand, so listing both would throw when this dictionary is built. KeyCodes with no keyboard
    //equivalent at all (mouse buttons, joystick buttons, Break, SysReq, Help, Clear) are absent as well.
    private static readonly Dictionary<KeyCode, InputKey> keyCodeToKey = new Dictionary<KeyCode, InputKey>()
    {
        { KeyCode.A, InputKey.A }, { KeyCode.B, InputKey.B }, { KeyCode.C, InputKey.C }, { KeyCode.D, InputKey.D },
        { KeyCode.E, InputKey.E }, { KeyCode.F, InputKey.F }, { KeyCode.G, InputKey.G }, { KeyCode.H, InputKey.H },
        { KeyCode.I, InputKey.I }, { KeyCode.J, InputKey.J }, { KeyCode.K, InputKey.K }, { KeyCode.L, InputKey.L },
        { KeyCode.M, InputKey.M }, { KeyCode.N, InputKey.N }, { KeyCode.O, InputKey.O }, { KeyCode.P, InputKey.P },
        { KeyCode.Q, InputKey.Q }, { KeyCode.R, InputKey.R }, { KeyCode.S, InputKey.S }, { KeyCode.T, InputKey.T },
        { KeyCode.U, InputKey.U }, { KeyCode.V, InputKey.V }, { KeyCode.W, InputKey.W }, { KeyCode.X, InputKey.X },
        { KeyCode.Y, InputKey.Y }, { KeyCode.Z, InputKey.Z },

        { KeyCode.Alpha0, InputKey.Digit0 }, { KeyCode.Alpha1, InputKey.Digit1 }, { KeyCode.Alpha2, InputKey.Digit2 },
        { KeyCode.Alpha3, InputKey.Digit3 }, { KeyCode.Alpha4, InputKey.Digit4 }, { KeyCode.Alpha5, InputKey.Digit5 },
        { KeyCode.Alpha6, InputKey.Digit6 }, { KeyCode.Alpha7, InputKey.Digit7 }, { KeyCode.Alpha8, InputKey.Digit8 },
        { KeyCode.Alpha9, InputKey.Digit9 },

        { KeyCode.Keypad0, InputKey.Numpad0 }, { KeyCode.Keypad1, InputKey.Numpad1 }, { KeyCode.Keypad2, InputKey.Numpad2 },
        { KeyCode.Keypad3, InputKey.Numpad3 }, { KeyCode.Keypad4, InputKey.Numpad4 }, { KeyCode.Keypad5, InputKey.Numpad5 },
        { KeyCode.Keypad6, InputKey.Numpad6 }, { KeyCode.Keypad7, InputKey.Numpad7 }, { KeyCode.Keypad8, InputKey.Numpad8 },
        { KeyCode.Keypad9, InputKey.Numpad9 },
        { KeyCode.KeypadPeriod, InputKey.NumpadPeriod },
        { KeyCode.KeypadDivide, InputKey.NumpadDivide },
        { KeyCode.KeypadMultiply, InputKey.NumpadMultiply },
        { KeyCode.KeypadMinus, InputKey.NumpadMinus },
        { KeyCode.KeypadPlus, InputKey.NumpadPlus },
        { KeyCode.KeypadEnter, InputKey.NumpadEnter },
        { KeyCode.KeypadEquals, InputKey.NumpadEquals },

        { KeyCode.F1, InputKey.F1 }, { KeyCode.F2, InputKey.F2 }, { KeyCode.F3, InputKey.F3 }, { KeyCode.F4, InputKey.F4 },
        { KeyCode.F5, InputKey.F5 }, { KeyCode.F6, InputKey.F6 }, { KeyCode.F7, InputKey.F7 }, { KeyCode.F8, InputKey.F8 },
        { KeyCode.F9, InputKey.F9 }, { KeyCode.F10, InputKey.F10 }, { KeyCode.F11, InputKey.F11 }, { KeyCode.F12, InputKey.F12 },
        { KeyCode.F13, InputKey.F13 }, { KeyCode.F14, InputKey.F14 }, { KeyCode.F15, InputKey.F15 },

        { KeyCode.UpArrow, InputKey.UpArrow },
        { KeyCode.DownArrow, InputKey.DownArrow },
        { KeyCode.LeftArrow, InputKey.LeftArrow },
        { KeyCode.RightArrow, InputKey.RightArrow },

        { KeyCode.Escape, InputKey.Escape },
        { KeyCode.Space, InputKey.Space },
        { KeyCode.Tab, InputKey.Tab },
        { KeyCode.Return, InputKey.Enter },
        { KeyCode.Backspace, InputKey.Backspace },
        { KeyCode.Insert, InputKey.Insert },
        { KeyCode.Delete, InputKey.Delete },
        { KeyCode.Home, InputKey.Home },
        { KeyCode.End, InputKey.End },
        { KeyCode.PageUp, InputKey.PageUp },
        { KeyCode.PageDown, InputKey.PageDown },

        { KeyCode.LeftShift, InputKey.LeftShift },
        { KeyCode.RightShift, InputKey.RightShift },
        { KeyCode.LeftAlt, InputKey.LeftAlt },
        { KeyCode.RightAlt, InputKey.RightAlt },
        { KeyCode.AltGr, InputKey.RightAlt },
        { KeyCode.LeftControl, InputKey.LeftCtrl },
        { KeyCode.RightControl, InputKey.RightCtrl },
        { KeyCode.LeftCommand, InputKey.LeftMeta },
        { KeyCode.RightCommand, InputKey.RightMeta },
        { KeyCode.LeftWindows, InputKey.LeftMeta },
        { KeyCode.RightWindows, InputKey.RightMeta },
        { KeyCode.Menu, InputKey.ContextMenu },

        { KeyCode.CapsLock, InputKey.CapsLock },
        { KeyCode.Numlock, InputKey.NumLock },
        { KeyCode.ScrollLock, InputKey.ScrollLock },
        { KeyCode.Print, InputKey.PrintScreen },
        { KeyCode.Pause, InputKey.Pause },

        { KeyCode.BackQuote, InputKey.Backquote },
        { KeyCode.Quote, InputKey.Quote },
        { KeyCode.Semicolon, InputKey.Semicolon },
        { KeyCode.Comma, InputKey.Comma },
        { KeyCode.Period, InputKey.Period },
        { KeyCode.Slash, InputKey.Slash },
        { KeyCode.Backslash, InputKey.Backslash },
        { KeyCode.LeftBracket, InputKey.LeftBracket },
        { KeyCode.RightBracket, InputKey.RightBracket },
        { KeyCode.Minus, InputKey.Minus },
        { KeyCode.Equals, InputKey.Equals },
    };

    //Turns a KeyCode into the binding path the Input System expects, for example "<Keyboard>/leftShift".
    //Returns null for KeyCodes with no keyboard equivalent - an action bound to a path that resolves to
    //nothing never fires and never complains, so callers have to handle the miss themselves.
    public static string ToKeyboardPath(this KeyCode keyCode)
    {
        if (!keyCodeToKey.TryGetValue(keyCode, out InputKey key))
        {
            return null;
        }

        return keyboardPathPrefix + key.toControlName();
    }

    //The keyboard's controls are named after the Keys with a lower case first letter, apart from the number
    //row, where Key.Digit1 is named "1".
    private static string toControlName(this InputKey key)
    {
        string name = key.ToString();

        if (name.StartsWith(digitKeyPrefix))
        {
            return name.Substring(digitKeyPrefix.Length);
        }

        return Char.ToLowerInvariant(name[0]) + name.Substring(1);
    }

    public static Trait getCostTrait(this ActionCostType costType)
    {
        switch(costType)
        {
            case ActionCostType.Bloodlust:
                return TraitList.bloodlust.clone();
            case ActionCostType.Predation:
                return TraitList.predation.clone();
            default:
                return null;
        }
    }

    public static string getCostDescription(this ActionCostType costType, int amount)
    {
        switch(costType)
        {
            case ActionCostType.RedKnife:
            case ActionCostType.BlueShield:
            case ActionCostType.YellowThorn:
            case ActionCostType.GreenLeaf:
                return "Costs " + amount + " Stacks of the " + costType.ToFriendlyString() + " Exuberance.";
            case ActionCostType.Stance:
                return "Costs " + amount + " Stacks of any Stance.";
            default:
                return "Costs " + amount + " Stacks of the " + costType.ToFriendlyString() + " Trait.";
        }
    }

    public static string getPrimaryStatCharGenCombatDescription(this PrimaryStat stat)
    {
        switch(stat)
        {
            case PrimaryStat.Strength:
                return "Characters who train their Strength gain access to Abilities that affect large areas with big bursts of damage. Their Critical Hits deal more damage as well, and they have higher Health pools than other Characters.\n\nCertain Strength Abilities can push enemies around, or prevent them from attacking vulnerable allies.";
            case PrimaryStat.Dexterity:
                return "Training Dexterity unlocks Abilities that focus on debilitation, damage over time, and the element of surprise. Dexterity also increases a Character's own Armor Score, their Armor Penetration, and provides a damage boost during a surprise round.\n\nMost Actions have their Critical Hit chance determined by a Character's Dexterity.";
            case PrimaryStat.Wisdom:
                return "Raising Wisdom teaches Abilities that are more tactical. A Wise Character can reposition their opponents to better deal with them, or interrupt a foe's plans with a well placed strike.\n\nWisdom governs the number of Weapons that can be held at once, and the number of Passive Abilities that can be equipped.";
            default:
                return "In Combat, Charisma measures a Character's ability to lead and coordinate with their Party Members. Charisma has Abilities that bolster one's allies, and expose the weaknesses of one's enemies.\n\nCharisma also provides access to Exuberances, which are a resource that is used to activate certain powerful Abilities.";
        }
    }

    public static string getPrimaryStatCharGenDialogueDescription(this PrimaryStat stat)
    {
        switch(stat)
        {
            case PrimaryStat.Strength:
                return "Strength speech checks often involve coercing others into providing aid, or gaining another's confidence through displays of physical prowess.";
            case PrimaryStat.Dexterity:
                return "In Dialogue, Dexterity measures a Character's ability to outwit and outmaneuver. A Dexterity speech check might involve using double-talk to trick someone into giving up information, or catching their arm before they can draw a weapon in anger.";
            case PrimaryStat.Wisdom:
                return "Wisdom is the Primary Stat of perception, knowledge, and reason. Wisdom provides speech checks that allow a Character to make logical arguments, bestow sagely advice, or detect what has been obscured.";
            default:
                return "Charisma determines a Character's powers of communication and persuasion. A Charismatic Character could with one speech check gain someone's confidence, and with the next destroy their ego with mockery.";
        }
    }

    public static string getPrimaryStatCharGenMobilityDescription(this PrimaryStat stat)
    {
        switch(stat)
        {
            case PrimaryStat.Strength:
                return "Characters with high Strength can use their powerful muscles to lift boulders, break down gates, and push aside rubble. They may also use the Intimidate Skill to challenge enemies to open combat, and compel shopkeepers to lower their prices.";
            case PrimaryStat.Dexterity:
                return "Increasing Dexterity allows a Character to clamber over obstructions, squeeze into tight spaces, and operate mechanisms. Dexterous Characters also can use the Cunning Skill to activate traps and deceive enemies; either to slip past them, or assault them from behind.";
            case PrimaryStat.Wisdom:
                return "A high Wisdom unlocks the Observation Skill. This Skill can be used to find secret passages, uncover hidden objects, and even find ambushes before they are sprung.";
            default:
                return "Charisma allows a Character to use the Leadership skill to coordinate multiple Party Members at once. This allows Party Members to block the movement of enemies, or operate mechanisms as a team.";
        }
    }

    public static SFXType convertEffectTypeToSFXType(this EffectAnimationType effect)
    {
        string effectName = effect.ToString();

        if(Enum.TryParse(effectName, ignoreCase: true, out SFXType sfxType))
        {
            return sfxType;
        } else
        {
            return SFXType.NoSFX;
        }
    }

    public static string getMaterialVARName(this ColorReplacementSlot slot)
    {
        return "_"+slot.ToString()+"_Replace";
    }

	public static Facing getOpposingFacing(this Facing facing)
	{
        switch(facing)
        {
            case Facing.NorthEast:
                return Facing.SouthWest;
            case Facing.NorthWest:
                return Facing.SouthEast;            
            case Facing.SouthWest:
                return Facing.NorthEast;
            case Facing.SouthEast:
                return Facing.NorthWest;
            default:
                return CharacterFacing.getRandomFacing();
        }
	}

    public static bool withScaleByChestType(this ChestType type)
    {
        switch(type)
        {
            case ChestType.Shelf:
                // return true;
                return false;
            default:
                // return false;
                return true;
        }
    }

    private static IEnumerable<T> getValues<T>() {
        return (T[]) Enum.GetValues(typeof(T));
    }

    public static CharacterAnimationType nextAnimationType(this CharacterAnimationType animationType)
    {
        switch(animationType)
        {
            case CharacterAnimationType.Stand_Up_Front:
                return CharacterAnimationType.OOC_Idle_Front;            
            case CharacterAnimationType.Stand_Up_Back:
                return CharacterAnimationType.OOC_Idle_Back;


            case CharacterAnimationType.Attack_Special_Front:
                return CharacterAnimationType.Attack_Normal_Front;

            case CharacterAnimationType.Attack_Special_Back:
                return CharacterAnimationType.Attack_Normal_Back;


            case CharacterAnimationType.Death_Front:
            case CharacterAnimationType.Wounded_Front:
            case CharacterAnimationType.Secondary_Idle_Front:
            case CharacterAnimationType.Spawn_Front:
            case CharacterAnimationType.Attack_Normal_Front:
                return CharacterAnimationType.Idle_Front;

            case CharacterAnimationType.Death_Back:
            case CharacterAnimationType.Wounded_Back:
            case CharacterAnimationType.Secondary_Idle_Back:
            case CharacterAnimationType.Spawn_Back:
            case CharacterAnimationType.Attack_Normal_Back:
                return CharacterAnimationType.Idle_Back;

            case CharacterAnimationType.Run_Front_Left:
            case CharacterAnimationType.Run_Front_Right:
                return CharacterAnimationType.Run_Front;

            case CharacterAnimationType.Run_Back_Left:
            case CharacterAnimationType.Run_Back_Right:
                return CharacterAnimationType.Run_Back;

            case CharacterAnimationType.Vertical_Falling:
            case CharacterAnimationType.Idle_Front:
            case CharacterAnimationType.Run_Front:
                return CharacterAnimationType.OOC_Idle_Front;

            case CharacterAnimationType.Run_Back:
            case CharacterAnimationType.Idle_Back:
                return CharacterAnimationType.OOC_Idle_Back;


            default:
                return CharacterAnimationType.None;
        }
    }

    public readonly static IEnumerable<SpriteLayer> SpriteLayers = getValues<SpriteLayer>();
    public readonly static IEnumerable<ColorReplacementSlot> ColorReplacementSlots = getValues<ColorReplacementSlot>();

    public readonly static IEnumerable<WeaponPose> WeaponPoses = getValues<WeaponPose>();

    public readonly static IEnumerable<BodyType> BodyTypes = getValues<BodyType>();
    public readonly static IEnumerable<WeaponAppearanceType> WeaponAppearanceTypes = getValues<WeaponAppearanceType>();
    public readonly static IEnumerable<FacialFeatureType> FacialFeatureTypes = getValues<FacialFeatureType>();
    public readonly static IEnumerable<HairType> HairTypes = getValues<HairType>();
    public readonly static IEnumerable<CloakType> CloakTypes = getValues<CloakType>();
}
