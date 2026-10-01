using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

//drawn with a GameObject on every cell it covers rather than one large sprite; CombatantSpawnDetails gives each cell its own Combatant
public class MultiAnimationEnemyStats : LargeEnemyStats
{

    #region Constructors

    public MultiAnimationEnemyStats(string key, int armor, int tHP, Trait[] traits, CombatAction combatAction = null) :
    base(key, armor, tHP, traits: traits, combatAction: combatAction)
    {

    }

    #endregion

}
