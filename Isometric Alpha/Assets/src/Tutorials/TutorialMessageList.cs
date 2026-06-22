using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TutorialMessageList
{
    private const string keyCodePlaceHolder = "##";

    private const string revealKeyCodePlaceHolder = "::";
    private const string jumpKeyCodePlaceHolder = "--";

    private const string clockwiseKeyCodePlaceHolder = "<<";
    private const string counterClockwiseKeyCodePlaceHolder = ">>";
    private const string resolveTurnKeyCodePlaceHolder = "??";

    private const string returnObjectKeyCodePlaceHolder = "[]";


    public const string hostileTargetTutorialMessagePrefix = "Hostile Target Tutorial Message ";
    public const string hostilityBarsTutorialMessagePrefix = "Hostility Bars Tutorial Message ";
    public const string intimidateTutorialMessagePrefix = "Intimidate Tutorial Message ";
    public const string interactableObjectTutorialMessagePrefix = "Interactable Object Tutorial Message ";

    public const string cunningTutorialMessagePrefix = "Cunning Tutorial Message ";
    public const string thirdCunningTutorialMessagePrefix = "Third Cunning Tutorial Message ";
    public const string observationTutorialMessagePrefix = "Observation Tutorial Message ";
    public const string secondObservationTutorialMessagePrefix = "Second Observation Tutorial Message ";
    public const string leadershipTutorialMessagePrefix = "Leadership Tutorial Message ";

    public const string equippableItemTutorialMessagePrefix = "Equippable Item Tutorial Message ";
    public const string formationTutorialMessagePrefix = "Formation Tutorial Message ";
    public const string hiddenObjectTutorialMessagePrefix = "Hidden Object Tutorial Message ";
    public const string addingAbilitiesTutorialMessagePrefix = "Adding Abilities Tutorial Message ";
    public const string playerLevelUpTutorialMessagePrefix = "Player Level Up Tutorial Message ";
    public const string companionSpecificAbiltiesTutorialMessagePrefix = "Companion Specific Abilities Tutorial Message ";
    public const string combatTutorialMessagePrefix = "Combat Tutorial Message ";
    public const string selectingAllyKey = "Combat Tutorial Message 11";
    public const string selectingAbilityFromWheelKey = "Combat Tutorial Message 14";
    public const string selectingTargetKey = "Combat Tutorial Message 15";
    public const string combatTutorialRepositionMessagePrefix = "Combat Tutorial Reposition Message ";
    public const string repositionStepKey = "Combat Tutorial Reposition Message 1";

    public const string combatTraitTutorialMessagePrefix = "Combat Trait Tutorial Message ";
    public const string mandatoryTargetTutorialMessagePrefix = "Mandatory Target Tutorial Message ";


    public const string exuberanceCostTutorialMessagePrefix = "Exuberance Cost Tutorial Message ";
    public const string traitCostTutorialMessagePrefix = "Trait Cost Tutorial Message ";

    public const string winConUITutorialMessagePrefix = "Win Con UI Tutorial Message ";


    public const string movableObjectTutorialMessagePrefix = "Movable Object Tutorial Message ";

    public const string questCounterTutorialMessagePrefix = "Quest Counter Tutorial Message ";

    public const string partyMemberUpgradeTutorialMessagePrefix = "Party Member Upgrade Tutorial Message ";

    public const string multiMemberObstacleTutorialMessagePrefix = "Multi Member Obstacle Tutorial Message ";

    private static Dictionary<string, string> tutorialDictionary;

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateTutorialMessageList()
    {
        tutorialDictionary = new Dictionary<string, string>();

        tutorialDictionary.Add(hostileTargetTutorialMessagePrefix + 1, "There is a creature in your path. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to highlight it.");
        tutorialDictionary.Add(hostileTargetTutorialMessagePrefix + 2, "The creature is highlighted in red. This means it is a hostile creature. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> again to remove the highlight.");
        tutorialDictionary.Add(hostileTargetTutorialMessagePrefix + 3, "The hostility tracker shows a red skull for this location, so you can be attacked here. When you are safe, it will show a green flower.");
        tutorialDictionary.Add(hostileTargetTutorialMessagePrefix + 4, "The enemy is facing away from you. If you move next to a creature while facing it's sides or back, you will surprise it and gain a free round of attacks.");
        tutorialDictionary.Add(hostileTargetTutorialMessagePrefix + 5, "Be careful how you approach creatures. Creatures can surprise you if they sneak up on you.");
        tutorialDictionary.Add(hostileTargetTutorialMessagePrefix + 6, "Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to move next to the creature and start combat.");

        tutorialDictionary.Add(hostilityBarsTutorialMessagePrefix + 1, "You have commited an action that has raised this Zone's Alertness Level. Attacking, using Skills on NPC's, and certain Dialogue Choices can result in raising a Zone's Alertness. A given action will often raise the current Zone's Alertness by fewer levels, or even by none at all, if you are indoors.");
        tutorialDictionary.Add(hostilityBarsTutorialMessagePrefix + 2, "Should you ever receive your fifth Alertness Level, the entire Zone will become Hostile. This will result in previously peaceful Locations being filled with guards looking to attack your Party Members. It can also affect the outcome of certain quests. Be careful who you attack!");

        tutorialDictionary.Add(intimidateTutorialMessagePrefix + 1, "This creature is hiding around a blind corner. If someone in your Party has put points in Strength, then the Intimidate skill can keep it from surprising you. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to activate it.");
        tutorialDictionary.Add(intimidateTutorialMessagePrefix + 2, "The orange tiles show Intimidate's range. A red tile shows a target. All targets in range will be affected by Intimidate. Intimidated creatures cannot surprise you or be surprised.");
        tutorialDictionary.Add(intimidateTutorialMessagePrefix + 3, "You have limited uses of Intimidate. These are replenished when you enter a new area. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to use Intimidate.");
        tutorialDictionary.Add(intimidateTutorialMessagePrefix + 4, "The symbol above the creature indicates it has been intimidated. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to start combat.");

        tutorialDictionary.Add(interactableObjectTutorialMessagePrefix + 1, "Rubble blocks your path. If you highlight interactables, the rubble will be shown in green. This means you can interact with it like you would an NPC. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to interact with it.");
        tutorialDictionary.Add(interactableObjectTutorialMessagePrefix + 2, "Barrels block your path. If you highlight interactables, the barrels will be shown in green. This means you can interact with them like you would an NPC. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to interact with it.");

        tutorialDictionary.Add(cunningTutorialMessagePrefix + 1, "This creature is hiding around a blind corner. If someone in your Party has put points in Dexterity, then you can use the Cunning skill to keep it from surprising you. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to begin to activate the Cunning Skill.");
        tutorialDictionary.Add(cunningTutorialMessagePrefix + 2, "The yellow tiles show the Cunning skill's range. A red tile shows a target. You are currently targeting the green tile. Use the Movement keys to change the tile you are targeting. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to target the creature.");
        tutorialDictionary.Add(cunningTutorialMessagePrefix + 3, "You are now targeting the creature. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to use Cunning on the creature.");
        tutorialDictionary.Add(cunningTutorialMessagePrefix + 4, "The symbol above the creature indicates it has been distracted. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to start combat.");

        tutorialDictionary.Add(cunningTutorialMessagePrefix + 5, "If someone in your Party has put points in Dexterity, then you can use the Cunning skill to get past certain obstacles. Cunning targets have a yellow border when you highlight interactables. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to begin to activate the Cunning Skill.");
        tutorialDictionary.Add(cunningTutorialMessagePrefix + 6, "You have limited uses of Cunning, shown here. Your charges are replenished when you enter a new area, or by using certain items. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to target the object.");
        tutorialDictionary.Add(cunningTutorialMessagePrefix + 7, "Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to activate the object.");

        tutorialDictionary.Add(observationTutorialMessagePrefix + 1, "Your path is blocked by a hidden door. Hidden doors are not highlighted by pressing <nobr>' " + revealKeyCodePlaceHolder + " '.</nobr> Instead, if someone in your Party has put points in Wisdom, then you can use the Observation skill to reveal them. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to face the door.");
        tutorialDictionary.Add(observationTutorialMessagePrefix + 2, "Now press <nobr>' " + keyCodePlaceHolder + " '</nobr> to activate the Observation Skill.");
        tutorialDictionary.Add(observationTutorialMessagePrefix + 3, "The pink tiles show where you are observing. The hidden doors are now also shaded pink. This means that you can now interact with them. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to exit the Observation Skill.");
        // tutorialDictionary.Add(observationTutorialMessagePrefix + 4, "When the Observation Symbol is outlined in yellow, that means you are observing. When you are not observing, it will darken.");
        tutorialDictionary.Add(observationTutorialMessagePrefix + 5, "Now press <nobr>' " + keyCodePlaceHolder + " '</nobr> to walk up to the secret door.");
        tutorialDictionary.Add(observationTutorialMessagePrefix + 6, "Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to interact.");

        tutorialDictionary.Add(leadershipTutorialMessagePrefix + 1, "Your path is blocked. Some obstacles can be removed by pressing buttons. If someone in your Party has put points in Charisma, then you can use the Leadership skill to tell Party Members to stand on buttons. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to move on to the button.");
        tutorialDictionary.Add(leadershipTutorialMessagePrefix + 3, "Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to place a follower.");
        tutorialDictionary.Add(leadershipTutorialMessagePrefix + 4, "You follower has been placed. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to move off of the button and reveal your follower.");
        tutorialDictionary.Add(leadershipTutorialMessagePrefix + 5, "Your follower is now holding down the first button. You can't move through placed followers, but neither can enemies. Remove followers by pressing <nobr>' " + returnObjectKeyCodePlaceHolder + " '</nobr> while facing them.");
        tutorialDictionary.Add(leadershipTutorialMessagePrefix + 6, "Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to move to the second button.");

        tutorialDictionary.Add(equippableItemTutorialMessagePrefix + 1, "You have been given some equipment. Click the Inventory button, or press <nobr>' " + keyCodePlaceHolder + " '</nobr>");
        tutorialDictionary.Add(equippableItemTutorialMessagePrefix + 2, "These are your equipped Items.");
        tutorialDictionary.Add(equippableItemTutorialMessagePrefix + 3, "These are your available Weapons.");
        tutorialDictionary.Add(equippableItemTutorialMessagePrefix + 4, "To equip a Weapon, hold click on the Weapon you wish to select, then drag that Weapon into the highlighted slot.");

        tutorialDictionary.Add(formationTutorialMessagePrefix + 1, "A Companion has joined your Party. Click the Party button or press <nobr>' " + keyCodePlaceHolder + " '</nobr>");
        tutorialDictionary.Add(formationTutorialMessagePrefix + 2, "This section shows which Companions are currently in your Party, and where they will appear in Combat. If a Party Member is not on this Grid, they are <b>NOT</b> currently in your Party.");
        tutorialDictionary.Add(formationTutorialMessagePrefix + 3, "This section shows your available Companions.");
        tutorialDictionary.Add(formationTutorialMessagePrefix + 4, "This shows your Party Size. You cannot have more Companions in your Party than your Party Size will allow. Your Party Size is determined by your Level and your Party's total combined Wisdom and Charisma.");
        tutorialDictionary.Add(formationTutorialMessagePrefix + 5, "To add a Companion, click and drag them onto the Formation Grid. To end this Tutorial, press <nobr>' " + keyCodePlaceHolder + " '</nobr>");

        tutorialDictionary.Add(hiddenObjectTutorialMessagePrefix + 1, "This is the Quest Counter. It appears when you enter an area with a Quest Objective.");
        tutorialDictionary.Add(hiddenObjectTutorialMessagePrefix + 2, "If you cannot find the Quest Objective it is indicating, try looking inside buildings in that area.");
        tutorialDictionary.Add(hiddenObjectTutorialMessagePrefix + 3, "The entrances to some buildings will be hidden behind terrain. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to hide terrain.");

        tutorialDictionary.Add(addingAbilitiesTutorialMessagePrefix + 3, "You have unlocked new Abilities. All available Abilities are listed here. To learn more about an Ability, hover over it.");
        tutorialDictionary.Add(addingAbilitiesTutorialMessagePrefix + 4, "To add an Action to the Action Wheel, click the Ability, or drag it into an open slot on the Action Wheel.");

        tutorialDictionary.Add(playerLevelUpTutorialMessagePrefix + 1, "You have gained enough Experience to Level Up. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> or click the Character screen button.");
        tutorialDictionary.Add(playerLevelUpTutorialMessagePrefix + 2, "These are your Character's Stats. Hover over them to learn more about them.");
        tutorialDictionary.Add(playerLevelUpTutorialMessagePrefix + 3, "Hover over one of the Plus Buttons to learn what your Character will gain from increasing that Stat. Press a Plus Button to Level Up.");
        
        tutorialDictionary.Add(companionSpecificAbiltiesTutorialMessagePrefix + 1, "This Party Member has just reached Level 3. At this Level, Party Members gain special Abilities available only to them. Click here to learn this Party Member's unique moves.");

        tutorialDictionary.Add(combatTutorialMessagePrefix + 1, "This is your character. As the leader of your party, if you fall in combat you will lose the game.");
        tutorialDictionary.Add(combatTutorialMessagePrefix + 2, "All creatures on this side of the battlefield are your allies.");
        tutorialDictionary.Add(combatTutorialMessagePrefix + 3, "All creatures on this side of the battlefield are enemies.");

        tutorialDictionary.Add(combatTutorialMessagePrefix + 4 + " EnemyGetsSurpriseRound", "The Surprise Round Icon is red, meaning you have been surprised. When you are surprised, you forfeit your first turn. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to let the enemy take their turn.");
        tutorialDictionary.Add(combatTutorialMessagePrefix + 4 + " PlayerGetsSurpriseRound", "This is the Surprise Round Icon. Green means you surprised the enemy and they won't get to attack on their first turn.");
        tutorialDictionary.Add(combatTutorialMessagePrefix + 4 + " NoSurpriseRound", "This is the Surprise Round Icon. It is grey, meaning no one is surprised, and no one will get any free attacks.");

        tutorialDictionary.Add(combatTutorialMessagePrefix + 5, "It is your turn. Move the white selector square with the Movement keys. When it is under an ally, press <nobr>' " + keyCodePlaceHolder + " '</nobr>");
        tutorialDictionary.Add(combatTutorialMessagePrefix + 6, "This is this character's Action Wheel. Use the ' " + counterClockwiseKeyCodePlaceHolder + " ' and ' " + clockwiseKeyCodePlaceHolder + " ' keys to cycle through your choices. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to select an Action.");
        tutorialDictionary.Add(combatTutorialMessagePrefix + 7, "Use the Movement keys to target a creature. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to queue your action.");
        tutorialDictionary.Add(combatTutorialMessagePrefix + 8, "Most Actions can only be performed between rounds. When you resolve the turn, all Actions in the Action Order will occur in order, starting at the top.");
        tutorialDictionary.Add(combatTutorialMessagePrefix + 9, "You and your companions can each perfom a single action per round. You can only perform as many total Actions as you have Action Slots, shown here.");
        tutorialDictionary.Add(combatTutorialMessagePrefix + 10, "When you are finished choosing your Actions, click the ' Resolve Turn ' button, or press ' " + resolveTurnKeyCodePlaceHolder + " '. Press <nobr>' " + keyCodePlaceHolder + " '</nobr> to end this tutorial.");

        tutorialDictionary.Add(exuberanceCostTutorialMessagePrefix + 1, "You have tried to activate an Ability that costs Exuberances, but you don't have the required amount.");
        tutorialDictionary.Add(exuberanceCostTutorialMessagePrefix + 2, "The number of Exuberances your party has is shown here. Hover over each Exuberance's Icon to learn how to earn more of that Exuberance.");
        tutorialDictionary.Add(exuberanceCostTutorialMessagePrefix + 3, "You can learn an Ability's Exuberance cost by reading it's description...");
        tutorialDictionary.Add(exuberanceCostTutorialMessagePrefix + 4, "... or by selecting it on the Action Wheel.");

        tutorialDictionary.Add(traitCostTutorialMessagePrefix + 1, "You have tried to activate an Ability that costs Stacks of a certain Trait, but you don't have the required amount.");
        tutorialDictionary.Add(traitCostTutorialMessagePrefix + 2, "The number of Trait Stacks each Character has is shown here. Hover over a Trait's Icon to learn how to earn more Stacks. If a Trait has no number on it's Icon, it cannot be Stacked.");
        tutorialDictionary.Add(traitCostTutorialMessagePrefix + 3, "If you do not see the correct Icon in the Trait Display, you have not equipped the correct Equippable Passive to your Action Wheel.");
        tutorialDictionary.Add(traitCostTutorialMessagePrefix + 4, "You can learn an Ability's Trait cost by reading it's description.");

        tutorialDictionary.Add(winConUITutorialMessagePrefix + 1, "Some Combat encounters have alternate 'Win Conditions'. This means to achieve Victory, you may have to accomplish something other than defeating all Master Creatures.");
        tutorialDictionary.Add(winConUITutorialMessagePrefix + 2, "To learn more about what this alternate Win Condition requires of you, hover over this icon.");

        tutorialDictionary.Add(combatTutorialRepositionMessagePrefix + 1, "Notice the indicator has turned yellow. This means the Action you've chosen allows you to choose a secondary target. Use the Movement keys to position the selector and choose the second location with <nobr>' " + keyCodePlaceHolder + " '</nobr>");

        tutorialDictionary.Add(movableObjectTutorialMessagePrefix + 1, "This crate is movable. Movable objects highlight in blue.");
        tutorialDictionary.Add(movableObjectTutorialMessagePrefix + 2, "If your character walks into it they will push it, so long as nothing is behind it.");
        tutorialDictionary.Add(movableObjectTutorialMessagePrefix + 3, "If a movable object is stuck, face it and press ' " + returnObjectKeyCodePlaceHolder + " ' to put it back where you found it.");

        tutorialDictionary.Add(combatTraitTutorialMessagePrefix + 1, "This creature has a trait that makes it take less damage. Press ' " + jumpKeyCodePlaceHolder + " ' + <nobr>' " + keyCodePlaceHolder + " '</nobr> to quickly select it.");
        tutorialDictionary.Add(combatTraitTutorialMessagePrefix + 2, "When your selector is under a single creature, that creature's stats will be displayed here.");
        tutorialDictionary.Add(combatTraitTutorialMessagePrefix + 3, "These icons show the enemy's traits. Traits are special boosts or penalties that have been applied to a creature. Hover over them to learn more about them.");

        tutorialDictionary.Add(mandatoryTargetTutorialMessagePrefix + 1, "This creature has a trait that makes it the Mandatory Target. Press ' " + jumpKeyCodePlaceHolder + " ' + <nobr>' " + keyCodePlaceHolder + " '</nobr> to quickly select it.");
        tutorialDictionary.Add(mandatoryTargetTutorialMessagePrefix + 2, "Being the Mandatory Target means every effect that targets something on it's side of the field must include it.");
        tutorialDictionary.Add(mandatoryTargetTutorialMessagePrefix + 3, "Actions that do not include at least one Mandatory Target will be prevented from being added to the Action Queue.");

        // tutorialDictionary.Add(questCounterTutorialMessagePrefix + 1, "This is the Quest Counter. It appears when you enter an area with a Quest Objective.");
        // tutorialDictionary.Add(questCounterTutorialMessagePrefix + 2, "You can view the Quests with Objectives in the current area on your Map.");
        // tutorialDictionary.Add(questCounterTutorialMessagePrefix + 3, "This is your Map. It will only show you places you've been before. Nearby locations will be silhouetted in black.");
        // tutorialDictionary.Add(questCounterTutorialMessagePrefix + 4, "The Quest Symbol next to an area name means a quest wants you to go there. If you cannot find the Quest Objective it is indicating, try looking inside buildings in that area.");

        tutorialDictionary.Add(questCounterTutorialMessagePrefix + 1, "You have been given multiple Quest Objectives at once.");
        tutorialDictionary.Add(questCounterTutorialMessagePrefix + 2, "You can view the Quests with Objectives in the current Zone on your Map. Click here, or press <nobr>' " + keyCodePlaceHolder + " '</nobr> to open your Map.");
        tutorialDictionary.Add(questCounterTutorialMessagePrefix + 3, "This is your Map. It will only show you places you've been before. Nearby locations will be silhouetted in black.");
        tutorialDictionary.Add(questCounterTutorialMessagePrefix + 4, "The Quest Symbol next to an area name means a quest wants you to go there.");
        tutorialDictionary.Add(questCounterTutorialMessagePrefix + 5, "You can see the names of your current Quests here. Hover over them to learn more about them.");

        tutorialDictionary.Add(multiMemberObstacleTutorialMessagePrefix + 1, "Some obstacles require two or more Party Members to remove. Directing your Party Members requires the 'Leadership' Skill.");
        tutorialDictionary.Add(multiMemberObstacleTutorialMessagePrefix + 2, "You can only use the 'Leadership' skill if you have a Party Member with two or more Charisma. If you have a qualified Party Member, the 'Leadership' skill will appear as an option on your Skills bar. Click the Arrow Buttons to cycle through your available Skills.");

        tutorialDictionary.Add(secondObservationTutorialMessagePrefix + 1, "Some doors are hidden from sight. Finding these doors requires the 'Observation' Skill.");
        tutorialDictionary.Add(secondObservationTutorialMessagePrefix + 2, "You can only use the 'Observation' skill if you have a Party Member with two or more Wisdom. If you have a qualified Party Member, the 'Wisdom' skill will appear as an option on your Skills bar. Click the Arrow Buttons to cycle through your available Skills.");

        tutorialDictionary.Add(thirdCunningTutorialMessagePrefix + 1, "Some obstacles can be activated with the 'Cunning' Skill. Cunning targets have a yellow border when you highlight interactables.");
        tutorialDictionary.Add(thirdCunningTutorialMessagePrefix + 2, "You can only use the 'Cunning' skill if you have a Party Member with two or more Dexterity. If you have a qualified Party Member, the 'Cunning' skill will appear as an option on your Skills bar. Click the Arrow Buttons to cycle through your available Skills.");

        tutorialDictionary.Add(partyMemberUpgradeTutorialMessagePrefix + 1, "You have earned enough affinity to upgrade a companion.");
        tutorialDictionary.Add(partyMemberUpgradeTutorialMessagePrefix + 2, "You can tell you have enough affinity because the party screen button has a counter next to it.");
        tutorialDictionary.Add(partyMemberUpgradeTutorialMessagePrefix + 3, "You earn affinity for each monster you defeat in combat. The more monsters you beat, the more affinity you get.");
        tutorialDictionary.Add(partyMemberUpgradeTutorialMessagePrefix + 4, "Having more companions, and a higher charisma, also grants you more affinity per monster.");
    }

    public static string getTutorialMessage(string key, KeyBind keyBind)
    {
        string message = tutorialDictionary[key];

        switch(key)
        {
            case leadershipTutorialMessagePrefix + "5":
            case movableObjectTutorialMessagePrefix + "3":
                message = message.Replace(returnObjectKeyCodePlaceHolder, KeyBindingList.removePlacedCompanionMovableObjectKey.ToString());
                break;
            case observationTutorialMessagePrefix + "1":
                message = message.Replace(revealKeyCodePlaceHolder, KeyBindingList.revealKey.ToString());
                break;
            case mandatoryTargetTutorialMessagePrefix + "1":
            case combatTraitTutorialMessagePrefix + "1":
                message = message.Replace(jumpKeyCodePlaceHolder, KeyBindingList.jumpMoveKey.ToString());
                break;
            case combatTutorialMessagePrefix + "10":
                message = message.Replace(resolveTurnKeyCodePlaceHolder, KeyBindingList.resolveTurnKey.ToString());
                break;
            case combatTutorialMessagePrefix + "6":
                message = message.Replace(counterClockwiseKeyCodePlaceHolder, KeyBindingList.moveCounterClockwiseKey.ToString()).Replace(clockwiseKeyCodePlaceHolder, KeyBindingList.moveClockwiseKey.ToString());
                break;
        }

        return message.Replace(keyCodePlaceHolder, keyBind.ToString());
    }


}