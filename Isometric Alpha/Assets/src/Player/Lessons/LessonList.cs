using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*
	public const string Key = "";
	public const string LessonDescription = "";
	public const string MechanicalDescription = "";
	public static Lesson Lesson = new Lesson(Key,LessonDescription,MechanicalDescription);
*/

public static class LessonList
{	
	
	
	
	

	public const string clayFrontalAssaultKey = "The Frontal Assault";
	public const string clayFrontalAssaultLessonDescription = "You remember Clay's words to you when you took the job: \"you might be able to surprise him and lay him flat before he can hit you.\" Sounds like a waste of time to you. It might be because of the hit Thatch scored upside your head, but you prefer a frontal assault over a sneaky one.";
	public const string clayFrontalAssaultMechanicalDescription = "You now smash through an additional 5% of your opponent's armor.";
	public static Lesson clayFrontalAssaultLesson = new Lesson(clayFrontalAssaultKey,clayFrontalAssaultLessonDescription,clayFrontalAssaultMechanicalDescription);

	public const string clayRemorseKey = "A Weary Heart";
	public const string clayRemorseLessonDescription = "Most jobs don't stick with you, but this wasn't like most jobs. The big man bawled his eyes out when you told him you were here to kill him. He begged for his life without shame, and you killed him anyways. Your exterior was like stone, but a crack found it's way to your heart all the same.";
	public const string clayRemorseMechanicalDescription = "Your regrets move you to avoid combat in the future. Your Armor increases by 5 and your chance to successfully retreat is increased by 20%.";
	public static Lesson clayRemorseLesson = new Lesson(clayRemorseKey,clayRemorseLessonDescription,clayRemorseMechanicalDescription);
	
	public const string clayStealthKey = "Stay Unseen";
	public const string clayStealthLessonDescription = "Your target was a beast of a man, with fists to match. But even diamonds can crack if you hit them just right. Going forward, perhaps you'll give the stealthy approach a try.";
	public const string clayStealthMechanicalDescription = "This stealth thing seems to have it's advantages. You deal 10% more damage during surprise rounds.";
	public static Lesson clayStealthLesson = new Lesson(clayStealthKey,clayStealthLessonDescription,clayStealthMechanicalDescription);
	
	public const string clayKeptNecklaceKey = "Keepsake Kept";
	public const string clayKeptNecklaceLessonDescription = "Thatch's necklace looks expertly made; a trinket forged for him by his old master. It paid for his life, but it would have been a shame to give away to Clay. Some quick witted words convinced Clay to forget about it, maybe this will work on other's and their valuables as well.";
	public const string clayKeptNecklaceMechanicalDescription = "You will pick your words carefully from now on when making deals. You gain a 5% discount on items bought from shops.";
	public static Lesson clayKeptNecklaceLesson = new Lesson(clayKeptNecklaceKey,clayKeptNecklaceLessonDescription,clayKeptNecklaceMechanicalDescription);
	
	public const string clayPacifistKey = "Slipping Upward";
	public const string clayPacifistLessonDescription = "You took the job to kill Thatch, but somewhere along the way you found you couldn't follow through on it. Or maybe you never truly meant to from the start. Either way, you feel confident that you made the right choice and you think Thatch would agree.";
	public const string clayPacifistMechanicalDescription = "Sticking to your convictions has made you more resolute. Your chance to Resist Mental afflictions raises by 4%.";
	public static Lesson clayPacifistLesson = new Lesson(clayPacifistKey,clayPacifistLessonDescription,clayPacifistMechanicalDescription);
	
	public const string clayHeroKey = "A Hero, Actually";
	public const string clayHeroLessonDescription = "If anyone else had taken the job, Thatch may very well be dead. But by taking the job and then convincing Clay that you had killed Thatch, you prolonged Thatch's life. So what if you get a little compensation for it, shouldn't good deeds be rewarded?";
	public const string clayHeroMechanicalDescription = "You find it easy to convince others (and yourself) of your heroism. Allies in your zone gain +5% critical damage.";
	public static Lesson clayHeroLesson = new Lesson(clayHeroKey,clayHeroLessonDescription,clayHeroMechanicalDescription);
	
	public const string Key = "";
	public const string LessonDescription = "";
	public const string MechanicalDescription = "";
	public static Lesson Lesson = new Lesson(Key,LessonDescription,MechanicalDescription);

	public static Lesson getLesson(string key)
	{
		switch(key)
		{
			case clayFrontalAssaultKey:
				return clayFrontalAssaultLesson;
			case clayRemorseKey:
				return clayRemorseLesson;
			case clayStealthKey:
				return clayStealthLesson;
				
			case clayKeptNecklaceKey:
				return clayKeptNecklaceLesson;
			case clayPacifistKey:
				return clayPacifistLesson;
			case clayHeroKey:
				return clayHeroLesson;
			default:
				throw new IOException("Unrecognized Lesson Key: " + key);
		}
	}
}
