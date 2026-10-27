using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationManagerExtensions
{
    public static void removeAnimation(this IAnimationManager tracker)
    {
        AnimationTracker.removeAnimation(tracker.key);

        // if(spriteSetByHeartBeat())
        // {
        //     enableExtras();
        // }

        if(tracker.singleFire)
        {
            tracker.gameObject.SetActive(false);
            GameObject.Destroy(tracker.gameObject);
        }

        AnimationTracker.checkAllAnimationsFinished();
    }
}

public class AnimationTracker : MonoBehaviour
{

    public Transform damageNumberCanvas;

    private static Dictionary<int, IAnimationManager> currentAnimations;

    private static AnimationTracker instance;

    private static int currentKey = 0;


    [RuntimeInitializeOnLoadMethod]
    private static void initializeAnimationTracker()
    {
        currentAnimations = new Dictionary<int, IAnimationManager>();
        instance = null;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Selector Manager in the scene.");
        }

        instance = this;
    }

    public static AnimationTracker getInstance()
    {
        return instance;
    }

    public static void flushAnimations()
    {
        foreach (KeyValuePair<int, IAnimationManager> kvp in currentAnimations)
        {
            Destroy(kvp.Value.gameObject);
        }

        currentAnimations = new Dictionary<int, IAnimationManager>();
    }

    public static int getCurrentKey()
    {
        return currentKey++;
    }

    public static void trackAnimation(int key, IAnimationManager tracker)
    {
        if(tracker == null)
        {
            return;
        }

        if(!currentAnimations.ContainsKey(key))
        {
            currentAnimations.Add(key, tracker);
        }
    }

    public static void trackAnimation(int key, IAnimationManager tracker, float animationDuration)
    {
        trackAnimation(key, tracker);

        if(instance != null)
        {
            instance.StartCoroutine(instance.ensureAnimationCleanup(key, animationDuration));
        }
    }

    private IEnumerator ensureAnimationCleanup(int key, float animationDuration)
    {
        yield return new WaitForSeconds(animationDuration + 0.1f);

        if(currentAnimations.ContainsKey(key))
        {
            removeAnimation(key);
            checkAllAnimationsFinished();
        }
    }

    public static void removeAnimation(int key)
    {
        if(currentAnimations.ContainsKey(key))
        {
            currentAnimations.Remove(key);
        }
    }

    public static void checkAllAnimationsFinished()
    {
        if(currentAnimations.Count > 0)
        {
            return;
        }

        //tracking works anywhere, but finishing every animation only ends a turn in combat
        if(CombatStateManager.inCombat)
        {
            DeadCombatantManager.handleDeadCombatants();

            CombatStateManager.getInstance().checkForWinOrLossStates();

            CombatUI.populateCombatActionPanels();
        }
    }

    public static void loadInstantEffect(EffectAnimationType animationType, GridCoords targetCoords, bool crit, int damageNumber, bool healsTarget, bool targetCanBeDead,  ScriptOnLanding landingScript, GridCoords actorCoords)
    {

        loadInstantEffect(animationType, targetCoords, crit, damageNumber, healsTarget, targetCanBeDead);

        if(landingScript != null)
        {
            ProjectileManager.loadInvisibleProjectile(actorCoords, targetCoords, false, -1, false, true, landingScript);
        }
    }

    public static void loadInstantEffect(EffectAnimationType animationType, GridCoords targetCoords, bool crit, int damageNumber, bool healsTarget, bool targetCanBeDead, bool ignoreMissingTargets = true)
    {
        if(CombatGrid.combatantIsRepositionClone(targetCoords))
        {
            return;
        }


        if(ignoreMissingTargets && 
            (!CombatGrid.combatantExistsAtCoords(targetCoords, out Stats target) || (target.isDead && !targetCanBeDead)))
        {
            return;
        }

        AnimationData animationData = AnimationDataList.getAnimationData(animationType);

        if(animationData == null)
        {
            return;
        }

        EffectAnimationManager currentEffect = EffectAnimationManager.createEffect(targetCoords, animationType, animationData,
                                                    damagePacket: new DamagePacket(damageNumber, crit, healsTarget));

        //the effect isn't tracked yet: its length would come from its AnimationData timings rather than a clip
        // AnimationClip clip = Resources.Load<AnimationClip>(PrefabNames.abilityEffectFolderPath + animationType);
        // float effectDuration = clip != null ? clip.length : 2f;
        // trackAnimation(currentEffect.key, currentEffect, effectDuration);
    }

    public static bool trackerBeingTracked(IAnimationManager tracker)
    {
        return currentAnimations.ContainsValue(tracker);
    }

    public bool hasOngoingAnimations()
    {
        if (currentAnimations.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
