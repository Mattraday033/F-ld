using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBoostTrait : Trait
{
    private int bonusDamageDealt;
    private int bonusCritChance;

    public DamageBoostTrait(string traitName, string traitType, string traitDescription, string traitIconName, Color traitIconBackgroundColor, int bonusDamageDealt) :
    base(traitName, traitType, traitDescription, traitIconName, traitIconBackgroundColor)
    {
        this.bonusDamageDealt = bonusDamageDealt;
        this.bonusCritChance = 0;
    }
    public DamageBoostTrait(string traitName, string traitType, string traitDescription, string traitIconName, Color traitIconBackgroundColor, int bonusDamageDealt, int bonusCritChance) :
    base(traitName, traitType, traitDescription, traitIconName, traitIconBackgroundColor)
    {
        this.bonusDamageDealt = bonusDamageDealt;
        this.bonusCritChance = bonusCritChance;
    }

    public DamageBoostTrait(string traitName, string traitType, string traitDescription, string traitIconName, int roundsLeft, Color traitIconBackgroundColor, int bonusDamageDealt) :
    base(traitName, traitType, traitDescription, traitIconName, roundsLeft, traitIconBackgroundColor)
    {
        this.bonusDamageDealt = bonusDamageDealt;
        this.bonusCritChance = 0;
    }
    public DamageBoostTrait(string traitName, string traitType, string traitDescription, string traitIconName, int roundsLeft, Color traitIconBackgroundColor, int bonusDamageDealt, int bonusCritChance) :
    base(traitName, traitType, traitDescription, traitIconName, roundsLeft, traitIconBackgroundColor)
    {
        this.bonusDamageDealt = bonusDamageDealt;
        this.bonusCritChance = bonusCritChance;
    }

    public override int getBonusDamageDealt()
    {
        return bonusDamageDealt;
    }
    public override int getBonusCritChance()
    {
        return bonusCritChance;
    }
}
