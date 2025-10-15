using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TutorialMessageList
{
    private const string hostileTargetTutorialMessagePrefix = "Hostile Target Tutorial Message ";
    private const string intimidateTutorialMessagePrefix = "Intimidate Tutorial Message ";
    private const string interactableObjectTutorialMessagePrefix = "Interactable Object Tutorial Message ";

    private const string cunningTutorialMessagePrefix = "Cunning Tutorial Message ";
    private const string observationTutorialMessagePrefix = "Observation Tutorial Message ";
    private const string leadershipTutorialMessagePrefix = "Leadership Tutorial Message ";

    public const string equippableItemTutorialMessagePrefix = "Equippable Item Tutorial Message ";
    public const string formationPopUpTutorialMessagePrefix = "Formation PopUp Tutorial Message ";
    public const string hiddenObjectTutorialMessagePrefix = "Hidden Object Tutorial Message ";
    public const string addingAbilitiesTutorialMessagePrefix = "Adding Abilities Tutorial Message ";
    public const string playerLevelUpTutorialMessagePrefix = "Player Level Up Tutorial Message ";
    public const string combatTutorialMessagePrefix = "Combat Tutorial Message ";
    public const string selectingAllyKey = "Combat Tutorial Message 11";
    public const string selectingAbilityFromWheelKey = "Combat Tutorial Message 14";
    public const string selectingTargetKey = "Combat Tutorial Message 15";
    public const string combatTutorialRepositionMessagePrefix = "Combat Tutorial Reposition Message ";
    public const string repositionStepKey = "Combat Tutorial Reposition Message 1";

    public const string combatTraitTutorialMessagePrefix = "Combat Trait Tutorial Message ";

    public const string movableObjectTutorialMessagePrefix = "Movable Object Tutorial Message ";

    public const string questCounterTutorialMessagePrefix = "Quest Counter Tutorial Message ";

    public const string partyMemberUpgradeTutorialMessagePrefix = "Party Member Upgrade Tutorial Message ";

    private static Dictionary<string, string> tutorialDictionary = new Dictionary<string, string>();

    static TutorialMessageList()
    {
        tutorialDictionary.Add(hostileTargetTutorialMessagePrefix + 1, "There is a creature in your path. Press 'Shift' to highlight it.");
        tutorialDictionary.Add(hostileTargetTutorialMessagePrefix + 2, "The creature is highlighted in red. This means it is a hostile creature. Press 'Shift' again to remove the highlight.");
        tutorialDictionary.Add(hostileTargetTutorialMessagePrefix + 3, "The hostility tracker is red, so you can be attacked in this location. When you are safe, it will be green.");
        tutorialDictionary.Add(hostileTargetTutorialMessagePrefix + 4, "This arrow shows which way it is facing. If you move next to a creature while facing it's sides or back, you will surprise it and gain a free round of attacks.");
        tutorialDictionary.Add(hostileTargetTutorialMessagePrefix + 5, "Be careful how you approach creatures. If they move next to you while facing your sides or back, they will surprise you instead.");
        tutorialDictionary.Add(hostileTargetTutorialMessagePrefix + 6, "Press 'D' to move next to the creature and start combat.");

        tutorialDictionary.Add(intimidateTutorialMessagePrefix + 1, "This creature is hiding around a blind corner. The Intimidate skill can keep it from surprising you. Press '1' to activate it.");
        tutorialDictionary.Add(intimidateTutorialMessagePrefix + 2, "The orange tiles show Intimidate's range. A red tile shows a target. All targets in range will be affected by Intimidate. Intimidated creatures cannot surprise you or be surprised.");
        tutorialDictionary.Add(intimidateTutorialMessagePrefix + 3, "You have limited uses of Intimidate. These are replenished when you enter a new area. Press 'E' to use Intimidate.");
        tutorialDictionary.Add(intimidateTutorialMessagePrefix + 4, "The arrow is pink. This means the creature is intimidated. Press 'A' to move next to the creature and start combat.");

        tutorialDictionary.Add(interactableObjectTutorialMessagePrefix + 1, "Rubble blocks your path. If you highlight interactables, the rubble will be shown in green. This means you can interact with it like you would an NPC. Press 'E' to interact with it.");
        tutorialDictionary.Add(interactableObjectTutorialMessagePrefix + 2, "Barrels block your path. If you highlight interactables, the barrels will be shown in green. This means you can interact with them like you would an NPC. Press 'E' to interact with it.");


        tutorialDictionary.Add(cunningTutorialMessagePrefix + 1, "This creature is hiding around a blind corner. This is a good opportunity to use the Cunning skill. Press '2' to begin to activate the Cunning Skill.");
        tutorialDictionary.Add(cunningTutorialMessagePrefix + 2, "The yellow tiles show the Cunning skill's range. A red tile shows a target. You are currently targeting the green tile. The 'WASD' keys to change the tile you are targeting. Press 'S' to target the creature.");
        tutorialDictionary.Add(cunningTutorialMessagePrefix + 3, "You are now targeting the creature. Press 'E' to use Cunning on the creature.");
        tutorialDictionary.Add(cunningTutorialMessagePrefix + 4, "The creature's arrow has turned red. This means that it has been distracted. It also has been turned around, and can't move. Press 'A' to start combat.");

        tutorialDictionary.Add(cunningTutorialMessagePrefix + 5, "Some obstacles can be activated with Cunning. Cunning targets have a yellow border when you highlight interactables. Press '2' to begin to activate the Cunning Skill again.");
        tutorialDictionary.Add(cunningTutorialMessagePrefix + 6, "You have limited uses of Cunning, shown here. Your charges are replenished when you enter a new area, or by using certain items. Press 'W' to target the object.");
        tutorialDictionary.Add(cunningTutorialMessagePrefix + 7, "Press 'E' to activate the object.");

        tutorialDictionary.Add(observationTutorialMessagePrefix + 1, "Your path is blocked by a hidden door. Hidden doors are not highlighted by pressing 'Shift'. Instead, use the Observation skill to reveal them. Press 'W' to face the door.");
        tutorialDictionary.Add(observationTutorialMessagePrefix + 2, "Now press '3' to activate the Observation Skill.");
        tutorialDictionary.Add(observationTutorialMessagePrefix + 3, "The pink tiles show where you are observing. The hidden doors are now also shaded pink. This means that you can now interact with them.");
        tutorialDictionary.Add(observationTutorialMessagePrefix + 4, "When the Observation Symbol is green, that means you are observing. When you are not observing, it will turn red. Press '3' to exit the Observation Skill.");
        tutorialDictionary.Add(observationTutorialMessagePrefix + 5, "Now press 'W' to walk up to the secret door.");
        tutorialDictionary.Add(observationTutorialMessagePrefix + 6, "Press 'E' to interact.");


        tutorialDictionary.Add(leadershipTutorialMessagePrefix + 1, "Your path is blocked by a gate. Some gates can be opened by pressing buttons. Press 'A' to move on to the button.");
        tutorialDictionary.Add(leadershipTutorialMessagePrefix + 2, "The gate was not opened. This means there is another button that needs to be pressed. Buttons can also be held down by objects found in the terrain, or your followers.");
        tutorialDictionary.Add(leadershipTutorialMessagePrefix + 3, "Press '4' to place a follower.");
        tutorialDictionary.Add(leadershipTutorialMessagePrefix + 4, "You follower has been placed. Press 'W' to move off of the button and reveal your follower.");
        tutorialDictionary.Add(leadershipTutorialMessagePrefix + 5, "Your follower is now holding down the first button. You can't move through placed followers, but neither can enemies. Remove followers by pressing 'Z' while facing them.");
        tutorialDictionary.Add(leadershipTutorialMessagePrefix + 6, "Press 'W' to move to the second button and open the gate.");

        tutorialDictionary.Add(equippableItemTutorialMessagePrefix + 1, "You have been given some equipment. Click the Inventory button, or press 'I'.");
        tutorialDictionary.Add(equippableItemTutorialMessagePrefix + 2, "These are your equipped Items.");
        tutorialDictionary.Add(equippableItemTutorialMessagePrefix + 3, "These are your available Weapons.");
        tutorialDictionary.Add(equippableItemTutorialMessagePrefix + 4, "To equip a Weapon, hold click on the Weapon you wish to select, then drag that Weapon into the highlighted slot.");
    //    tutorialDictionary.Add(equippableItemTutorialMessagePrefix + 5, "This shows the weapon's damage.");
    //    tutorialDictionary.Add(equippableItemTutorialMessagePrefix + 6, "This shows the weapon's chance to critically hit, and deal extra damage.");
    //    tutorialDictionary.Add(equippableItemTutorialMessagePrefix + 7, "This shows the weapon's range. The range of a weapon determines the size and shape of a weapon's attack area.");
    //    tutorialDictionary.Add(equippableItemTutorialMessagePrefix + 8, "Click the 'Equip' button.");
    //    tutorialDictionary.Add(equippableItemTutorialMessagePrefix + 9, "This is the Action Wheel. Equipped weapons need to be placed on the Action Wheel so you can use them in combat.");
    //    tutorialDictionary.Add(equippableItemTutorialMessagePrefix + 10, "The amount of slots that can be used on weapons is limited by your strength, and shown here.");
    //    tutorialDictionary.Add(equippableItemTutorialMessagePrefix + 11, "Use the 'A' and 'D' keys to choose a slot, then press 'E' to equip your weapon.");

        tutorialDictionary.Add(formationPopUpTutorialMessagePrefix + 1, "A companion has joined your party. This screen will allow you to select which party members join you in battle.");
        tutorialDictionary.Add(formationPopUpTutorialMessagePrefix + 2, "This shows your companion limit. Your companion limit is determined by your level and your charisma.");
        tutorialDictionary.Add(formationPopUpTutorialMessagePrefix + 3, "This section shows your available companions. Click a companion to begin to add them to your formation.");
        tutorialDictionary.Add(formationPopUpTutorialMessagePrefix + 4, "Now select the square you would like to add them to.");
        tutorialDictionary.Add(formationPopUpTutorialMessagePrefix + 5, "The companion has been added to your formation. To remove a companion, click it's square. When finished, click 'Accept' or press 'E'.");

        tutorialDictionary.Add(hiddenObjectTutorialMessagePrefix + 1, "Bálint has asked for you to gather leaves in this area. Sometimes, quest objectives and other important objects are hidden behind terrain. Press 'F' to remove the tops of buildings and scenery to get a better look behind them.");

        // tutorialDictionary.Add(addingAbilitiesTutorialMessagePrefix + 1, "You have gained new abilities. To use them in combat, you will need to add them to your Action Wheel. Press 'C' or click the Character screen button.");
        // tutorialDictionary.Add(addingAbilitiesTutorialMessagePrefix + 2, "This is the Character Screen. It shows your character's stats, experience, level, and available Abilities.");
        tutorialDictionary.Add(addingAbilitiesTutorialMessagePrefix + 3, "You have unlocked new Abilities. All available Abilities are listed here. To learn more about an Ability, hover over it.");
        tutorialDictionary.Add(addingAbilitiesTutorialMessagePrefix + 4, "To add an Action to the Action Wheel, click the Ability, or drag it into an open slot on the Action Wheel.");

        tutorialDictionary.Add(playerLevelUpTutorialMessagePrefix + 1, "You have gained enough Experience to Level Up. Press 'C' or click the Character screen button.");
        tutorialDictionary.Add(playerLevelUpTutorialMessagePrefix + 2, "These are your Character's Stats. Hover over them to learn more about them.");
        tutorialDictionary.Add(playerLevelUpTutorialMessagePrefix + 3, "Hover over one of the Plus Buttons to learn what your Character will gain from increasing that Stat. Press a Plus Button to Level Up.");
        

        tutorialDictionary.Add(combatTutorialMessagePrefix + 1, "This is your character. As the leader of your party, if you fall in combat you will lose the game.");
        tutorialDictionary.Add(combatTutorialMessagePrefix + 2, "All creatures on this side of the battlefield are your allies.");
        tutorialDictionary.Add(combatTutorialMessagePrefix + 3, "All creatures on this side of the battlefield are enemies.");

        tutorialDictionary.Add(combatTutorialMessagePrefix + 4 + " EnemyGetsSurpriseRound", "The Surprise Round Icon is red, meaning you have been surprised. When you are surprised, you forfeit your first turn. Press 'Space' to let the enemy take their turn.");
        tutorialDictionary.Add(combatTutorialMessagePrefix + 4 + " PlayerGetsSurpriseRound", "This is the Surprise Round Icon. Green means you surprised the enemy and they won't get to attack on their first turn.");
        tutorialDictionary.Add(combatTutorialMessagePrefix + 4 + " NoSurpriseRound", "This is the Surprise Round Icon. It is grey, meaning no one is surprised, and no one will get any free attacks.");

        tutorialDictionary.Add(combatTutorialMessagePrefix + 5, "It is your turn. Move the white selector square with the 'WASD' keys. When it is under an ally, press 'E'.");
        tutorialDictionary.Add(combatTutorialMessagePrefix + 6, "This is this character's Action Wheel. Use the 'A' and 'D' keys to cycle through your choices. Press 'E' to select an Action.");
        tutorialDictionary.Add(combatTutorialMessagePrefix + 7, "Use the 'WASD' keys to target a creature. Press 'E' to queue your action.");
        tutorialDictionary.Add(combatTutorialMessagePrefix + 8, "Most Actions can only be performed between rounds. When you resolve the turn, all Actions in the Action Order will occur in order, starting at the top.");
        tutorialDictionary.Add(combatTutorialMessagePrefix + 9, "You can perform one Action per round. Your companions, however, draw from a shared pool of Action slots. These icons show how many actions your party has left.");

        tutorialDictionary.Add(combatTutorialMessagePrefix + 10, "When you are finished choosing your Actions, click the 'Resolve Turn' button, or press 'Space'. Press 'E' to end this tutorial.");


        //tutorialDictionary.Add(combatTutorialMessagePrefix + 15, "Now use the 'WASD' keys to move the targeting selector beneath the creature you want to target. Press 'E' to target the creature. If you can't target a specific creature, it may be because the Action you have chosen has a special targeting restriction, such as only being able to target allies. To back out of targeting, press 'Escape'.");
        tutorialDictionary.Add(combatTutorialRepositionMessagePrefix + 1, "Notice the indicator has turned yellow. This means the Action you've chosen allows you to choose a secondary target. Move the selector with 'WASD' and choose the second location with 'E'.");

        tutorialDictionary.Add(movableObjectTutorialMessagePrefix + 1, "This crate is movable. Movable objects highlight in blue.");
        tutorialDictionary.Add(movableObjectTutorialMessagePrefix + 2, "If your character walks into it they will push it, so long as nothing is behind it.");
        tutorialDictionary.Add(movableObjectTutorialMessagePrefix + 3, "If a movable object is stuck, face it and press 'Z' to put it back where you found it.");

        tutorialDictionary.Add(combatTraitTutorialMessagePrefix + 1, "This creature has a trait that makes it take less damage. Press 'Shift' + 'W' to quickly select it.");
        tutorialDictionary.Add(combatTraitTutorialMessagePrefix + 2, "When your selector is under a single creature, that creatures stats will be displayed here.");
        tutorialDictionary.Add(combatTraitTutorialMessagePrefix + 3, "These icons show the enemy's traits. Traits are special boosts or penalties that have been applied to a creature. Hover over them to learn more about them.");

        tutorialDictionary.Add(questCounterTutorialMessagePrefix + 1, "This is the Quest Counter. It appears when you enter an area with a Quest Objective.");
        tutorialDictionary.Add(questCounterTutorialMessagePrefix + 2, "You can view the Quests with Objectives in the current area on your Map.");
        tutorialDictionary.Add(questCounterTutorialMessagePrefix + 3, "This is your Map. It will only show you places you've been before. Nearby places will be silhouetted in black.");
        tutorialDictionary.Add(questCounterTutorialMessagePrefix + 4, "The Quest Symbol next to an area name means a quest wants you to go there. If you cannot find the Quest Objective it is indicating, try looking inside buildings in that area.");

        tutorialDictionary.Add(partyMemberUpgradeTutorialMessagePrefix + 1, "You earned enough affinity to upgrade a companion.");
        tutorialDictionary.Add(partyMemberUpgradeTutorialMessagePrefix + 2, "You can tell you have enough affinity because the party screen button has a counter next to it.");
        tutorialDictionary.Add(partyMemberUpgradeTutorialMessagePrefix + 3, "You earn affinity for each monster you defeat in combat. The more monsters you beat, the more affinity you get.");
        tutorialDictionary.Add(partyMemberUpgradeTutorialMessagePrefix + 4, "Having more companions, and a higher charisma, also grants you more affinity per monster.");
    }

    public static string getTutorialMessage(string key)
    {
        return tutorialDictionary[key];
    }


}

        /*
            tutorialDictionary.Add(combatTraitTutorialMessagePrefix + 1, "You interact with allies and enemies on the battlefield by moving this indicator, or 'selector', with the 'WASD' keys. You can jump to the next available target in a specific direction by holding 'Shift' while moving. Press 'Shift' + 'W' to move the selector to the closest enemy.");
            tutorialDictionary.Add(combatTraitTutorialMessagePrefix + 2, "When the selector is under a creature, information about them will be displayed in the bottom right corner of the screen.");
            tutorialDictionary.Add(combatTraitTutorialMessagePrefix + 3, "These icons show the enemy's traits. Traits are special modifiers or rules that have been applied to a creature. Hover your mouse over a Trait's icon to get a description of what it does. You can learn a lot about a creature by reading their Traits.");

            tutorialDictionary.Add(combatTraitTutorialMessagePrefix + 4, "This Trait marks a creature as a Master creature. All Master creatures must be defeated to prevail in Combat.");
            tutorialDictionary.Add(combatTraitTutorialMessagePrefix + 5 + " SnapToPlayer", "This Trait marks a creature as a Master creature. All Master creatures must be defeated to prevail in Combat. Press 'Shift' + 'S' to move the selector back to your side of the field.");
            tutorialDictionary.Add(combatTraitTutorialMessagePrefix + 6 + " MinionTraitIcon", "This Trait marks a creature as a Minion creature. Minion creatures do not need to be defeated to win, and will flee when the last Master creature is defeated. Press 'Shift' + 'S' to move the selector back to your side of the field.");
        */

        /* Old Combat tutorial
            tutorialDictionary.Add(combatTutorialMessagePrefix + 1, "You have entered combat. Here, your party's mettle will be tested against your enemies. You can press 'Shift' + 'Tab' at any time to skip this tutorial.");
            tutorialDictionary.Add(combatTutorialMessagePrefix + 2, "This is your character. As the leader of your party, if you fall in combat you will lose the game.");
            tutorialDictionary.Add(combatTutorialMessagePrefix + 3, "This section of the battlefield is yours. All creatures on this side of the field are your allies. Which companions join you in combat is determined by your formation. You can edit your formation on the Party Screen when you aren't in combat.");
            tutorialDictionary.Add(combatTutorialMessagePrefix + 4, "This section of the battlefield is the enemy's. Any creatures you see on this side of the battlefield want to see you fail.");

            tutorialDictionary.Add(combatTutorialMessagePrefix + 5 + " EnemyGetsSurpriseRound", "This is the Surprise Round Icon. It is red, which indicates that you have been surprised. When you are surprised, the enemy gets a free round of attacks before you are able to retaliate. Green indicates you have surprised the enemy, instead. Grey indicates no one is surprised. Press 'Space' to let the enemy take their turn.");
            tutorialDictionary.Add(combatTutorialMessagePrefix + 5 + " PlayerGetsSurpriseRound", "This is the Surprise Round Icon. It is green, which indicates that you surprised the enemy. This means the enemy won't get to attack on their first turn. Red indicates you have been surprised. Grey indicates no one is surprised.");
            tutorialDictionary.Add(combatTutorialMessagePrefix + 5 + " NoSurpriseRound", "This is the Surprise Round Icon. It is grey, which indicates that no one is surprised. If it were green, the enemy would not get to attack on their first turn. Red indicates you have been surprised, instead.");

            tutorialDictionary.Add(combatTutorialMessagePrefix + 6, "You interact with allies and enemies on the battlefield by moving this indicator, or 'selector', with the 'WASD' keys. You can jump to the next available target in a specific direction by holding 'Shift' while moving. Press 'Shift' + 'W' to move the selector to the closest enemy.");
            tutorialDictionary.Add(combatTutorialMessagePrefix + 7, "When the selector is under a creature, information about them will be displayed in the bottom right corner of the screen.");
            tutorialDictionary.Add(combatTutorialMessagePrefix + 8, "These icons show the enemy's traits. Traits are special modifiers or rules that have been applied to a creature. Hover your mouse over a Trait's icon to get a description of what it does. You can learn a lot about a creature by reading their Traits.");

            tutorialDictionary.Add(combatTutorialMessagePrefix + 9, "This Trait marks a creature as a Master creature. All Master creatures must be defeated to prevail in Combat.");
            tutorialDictionary.Add(combatTutorialMessagePrefix + 9 + " SnapToPlayer", "This Trait marks a creature as a Master creature. All Master creatures must be defeated to prevail in Combat. Press 'Shift' + 'S' to move the selector back to your side of the field.");

            tutorialDictionary.Add(combatTutorialMessagePrefix + 10 + " MinionTraitIcon", "This Trait marks a creature as a Minion creature. Minion creatures do not need to be defeated to win, and will flee when the last Master creature is defeated. Press 'Shift' + 'S' to move the selector back to your side of the field.");

            tutorialDictionary.Add(combatTutorialMessagePrefix + 11, "It is your turn. You can choose an ally to direct by moving the indicator with the 'WASD' keys. When it is under the ally you want to choose, press 'E' to select them.");
            tutorialDictionary.Add(combatTutorialMessagePrefix + 12, "This is the selected character's Action Wheel. It shows all of the Actions you can currently use with this character.");
            tutorialDictionary.Add(combatTutorialMessagePrefix + 13, "Activated Passives are activated automatically at the start of combat, so you won't be able to select them. Others have special requirements that have to be met before they can be activated, such as being in a surprise round, or having a specific item in your inventory. If you can't activate an Action at this time, it's icon will be greyed out. Consult the Action's description in the top right of your screen to learn it's requirements.");
            tutorialDictionary.Add(combatTutorialMessagePrefix + 14, "Use the 'A' and 'D' keys to cycle through your choices. Press 'E' to select the Action you want to use.");
            tutorialDictionary.Add(combatTutorialMessagePrefix + 15, "Now use the 'WASD' keys to move the targeting selector beneath the creature you want to target. Press 'E' to target the creature. If you can't target a specific creature, it may be because the Action you have chosen has a special targeting restriction, such as only being able to target allies. To back out of targeting, press 'Escape'.");
            tutorialDictionary.Add(combatTutorialMessagePrefix + 16, "You have chosen the target of your Action. Most Actions can only be performed between rounds, and will be added to the Action Order. When you resolve the turn, all Actions in the Action Order will occur in the order they are displayed, starting at the top. To learn who is performing an Action in the Action Order, who it is targeting, and what it will do, you can hover your mouse over the Action's icon.");
            tutorialDictionary.Add(combatTutorialMessagePrefix + 17, "Each party member can only perform, at most, one Action per round. Your character can always perform an Action, as long as they aren't incapacitated by an enemy's attack. Your companions, however, draw from a shared pool of Action slots. To see how many actions your party has left, look at these icons here.");
            tutorialDictionary.Add(combatTutorialMessagePrefix + 18, "If the large icon is green, your character has already chosen an Action. Red means you can still choose an Action. The smaller icons show the remaining Actions your companions can take. You gain more Companion Actions as you level up. Having a high Charisma also increases the number of Actions your party can take.");
            tutorialDictionary.Add(combatTutorialMessagePrefix + 19, "Should you ever feel outmatched, you can press this button to retreat. But be warned: the percentage shown is your chance of successfully extracting your party from combat. If you fail to retreat, the enemy will take their entire turn before you get to act again. And even if you succeed, the enemy will be fully restored when you return.");
            tutorialDictionary.Add(combatTutorialMessagePrefix + 20, "A high Wisdom will give you a better chance of retreating. However, combat entered through dialogue cannot be retreated from. Be careful who you pick a fight with!");

            tutorialDictionary.Add(combatTutorialMessagePrefix + 21, "When you are finished choosing all of your Actions, click the 'Resolve Turn' button, or press 'Space'. Press 'E' to end this tutorial."); 


          //tutorialDictionary.Add(combatTutorialMessagePrefix + 15, "Now use the 'WASD' keys to move the targeting selector beneath the creature you want to target. Press 'E' to target the creature. If you can't target a specific creature, it may be because the Action you have chosen has a special targeting restriction, such as only being able to target allies. To back out of targeting, press 'Escape'.");
            tutorialDictionary.Add(combatTutorialRepositionMessagePrefix + 1, "The Action you've chosen allows you to choose a secondary target. Certain Actions let you attack twice, hitting multiple locations. Others allow you to move an enemy to another location, which can be useful for isolating enemies or grouping them together to hit them all at once. You can tell the Action needs you to choose a secondary location when it's targeting selector turns yellow.");

            tutorialDictionary.Add(movableObjectTutorialMessagePrefix + 1, "This is crate is movable. You can tell an object is movable if it has a blue border when you highlight interactables. To move it, walk into it. Your character will push it in the direction you are walking, so long as there is nothing behind it.");
            tutorialDictionary.Add(movableObjectTutorialMessagePrefix + 2, "If a movable object is stuck, face it and press 'Z' to snap it back to where you found it. This won't work if there is something standing on top of it's starting location.");
       */