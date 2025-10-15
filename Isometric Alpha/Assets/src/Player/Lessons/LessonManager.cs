using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LessonManager
{
	
	public static void addLesson(Lesson lesson)
	{
		addLesson(lesson.getKey());
	}
	
	public static void addLesson(string lessonKey)
	{
		State.lessonsLearned = Helpers.appendArray<string>(State.lessonsLearned, lessonKey);
	}
	
	public static ArrayList getLessons(string[] lessonKeys)
	{
		ArrayList lessons = new ArrayList();
		
		foreach(string key in lessonKeys)
		{
			lessons.Add(LessonList.getLesson(key));
		}
		
		return lessons;
	}
	
	public static ArrayList getAllLessons()
	{
		ArrayList allLessons = new ArrayList();
		
		foreach(string lessonKey in State.lessonsLearned)
		{
			allLessons.Add(LessonList.getLesson(lessonKey));
		}
		
		return allLessons;
	}
	//State.statBoosts.Aggregate(0, (a,b) =>  a += b.getExtraHealth());
	public static string[] getAllLessonKeys()
	{
		Lesson[] allLessons = Array.ConvertAll(getAllLessons().ToArray(), item => (Lesson) item);
		string[] allLessonKeys = new string[0];
		
		return allLessons.Aggregate(allLessonKeys, (current,item) => Helpers.appendArray<string>(current,item.getKey()));
	}
	
	public static SecondaryStatBoost[] getAllStatBoosts()
	{
		ArrayList allStatBoosts = new ArrayList();
		string[] allLessonKeys = getAllLessonKeys();
		
		foreach(string lessonKey in allLessonKeys)
		{
			SecondaryStatBoost statBoost = StatBoostList.getStatBoost(lessonKey);
			
			if(statBoost != null)
			{
				allStatBoosts.Add(statBoost);
			}
		}
		
		return Array.ConvertAll(allStatBoosts.ToArray(), item => (SecondaryStatBoost) item);
	}
	
	public static Ability[] getAllAbilities()
	{
		ArrayList allCombatActions = new ArrayList();
		string[] allLessonKeys = getAllLessonKeys();
		
		foreach(string lessonKey in allLessonKeys)
		{
			Ability currentAbility = null;
			
			AbilityList.lessonAbilityDictionary.TryGetValue(lessonKey, out currentAbility);
			
			if(currentAbility != null)
			{
				allCombatActions.Add(currentAbility);	
			}
		}
		
		return Array.ConvertAll(allCombatActions.ToArray(), item => (Ability) item);
	}
	
	public static void resetLessons()
	{
		State.lessonsLearned = new string[0];
	}
}
