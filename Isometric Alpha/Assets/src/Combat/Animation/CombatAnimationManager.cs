using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationTracker
{
    public GameObject getGameObject();
    public void removeAnimation();

}

public class CombatAnimationManager : MonoBehaviour
{

    public Transform damageNumberCanvas;

    private static Dictionary<int, IAnimationTracker> currentAnimations;

    private static CombatAnimationManager instance;

    private const int framesBetweenActorAndTarget = 100;
    private const float adjustment = -3f;

    private const float defaultMaxTime = .5f;

    private int currentKey = 0;


    [RuntimeInitializeOnLoadMethod]
    private static void initializeCombatAnimationManager()
    {
        currentAnimations = new Dictionary<int, IAnimationTracker>();
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

    public static CombatAnimationManager getInstance()
    {
        return instance;
    }

    public static void flushAnimations()
    {
        foreach (KeyValuePair<int, IAnimationTracker> kvp in currentAnimations)
        {
            Destroy(kvp.Value.getGameObject());
        }

        currentAnimations = new Dictionary<int, IAnimationTracker>();
    }

    public static int getCurrentKey()
    {
        return getInstance().currentKey++;
    }

    public static void trackAnimation(int key, IAnimationTracker tracker)
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

    public static void removeAnimation(int key)
    {
        if(currentAnimations.ContainsKey(key))
        {
            currentAnimations.Remove(key);
        }
    }

    public static void checkAllAnimationsFinished()
    {
        if (!getInstance().hasOngoingAnimations())
        {
            DeadCombatantManager.handleDeadCombatants();

            CombatStateManager.getInstance().checkForWinOrLossStates();

            CombatUI.populateCombatActionPanels();
        }
    }

    public static void loadInstantEffect(string abilityName, GridCoords targetCoords, bool crit, int damageNumber, bool healsTarget, bool targetCanBeDead)
    {
        Stats target = CombatGrid.getCombatantAtCoords(targetCoords);

        if(target == null || (target.isDead() && !targetCanBeDead))
        {
            return;
        }

        EffectAnimationManager currentEffect = EffectAnimationManager.instantiatePrefab();

        currentEffect.damage = damageNumber;
        currentEffect.crit = crit;
        currentEffect.healsTarget = healsTarget;

        currentEffect.targetCoords = targetCoords;

        currentEffect.transform.position = CombatGrid.getPositionAt(targetCoords);

        currentAnimations.Add(currentEffect.key, currentEffect);

        currentEffect.setAnimations(abilityName);
    }

    public static Projectile loadProjectile(GridCoords actorCoords, GridCoords targetCoords, bool crit, int damageNumber, bool healsTarget, bool targetCanBeDead)
    {
        return loadProjectile(actorCoords, targetCoords, crit, damageNumber, healsTarget, targetCanBeDead, null);
    }

    public static Projectile loadProjectile(GridCoords actorCoords, GridCoords targetCoords, bool crit, int damageNumber, bool healsTarget, bool targetCanBeDead, ScriptOnLanding script)
    {
        int key = getCurrentKey();

        Projectile currentProjectile = Projectile.instantiatePrefab();

        currentProjectile.key = key;

        currentProjectile.scriptOnLanding = script;

        currentProjectile.damage = damageNumber;
        currentProjectile.crit = crit;
        currentProjectile.healsTarget = healsTarget;

        currentProjectile.setTargetCoords(targetCoords);
        currentProjectile.affectsDeadTargets = targetCanBeDead;

        currentProjectile.maxTime = defaultMaxTime;

        currentAnimations.Add(key, currentProjectile);

        if (CombatGrid.positionsAreOnSameSide(actorCoords, targetCoords))
        {
            Vector3 endPosition = CombatGrid.getPositionAt(targetCoords);
            AppearAtDestination trajectory = new AppearAtDestination(endPosition.y);

            currentProjectile.trajectory = trajectory;
            currentProjectile.pointsBetweenActorAndTarget = new float[1] { endPosition.x };

            currentProjectile.moveTo(targetCoords.toVector3());
            return currentProjectile;
        }
        else
        {
            Parabola trajectory;
            Vector3 startCoords = actorCoords.toVector3();
            Vector3 endCoords = targetCoords.toVector3();
            Vector3 zenithCoords = calcZenithCoords(startCoords, endCoords);

            calcTrajectory(startCoords,
                            endCoords,
                            zenithCoords,
                            out trajectory);



            currentProjectile.trajectory = trajectory;
            currentProjectile.pointsBetweenActorAndTarget = findEquidistantPointsBetweenTwoPoints(framesBetweenActorAndTarget,
                                                                                                actorCoords.toVector3().x,
                                                                                                targetCoords.toVector3().x);

            try
            {
                currentProjectile.moveTo(actorCoords.toVector3());
                return currentProjectile;
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogError("Caught IndexOutOfRangeException: actorCoords = " + actorCoords.ToString());
                Destroy(currentProjectile.gameObject);
                return null;
            }
        }
    }

    public static void loadInvisibleProjectile(GridCoords actorCoords, GridCoords targetCoords, bool crit, int damageNumber, bool healsTarget, bool targetCanBeDead)
    {
        Projectile projectile = loadProjectile(actorCoords, targetCoords, crit, damageNumber, healsTarget, targetCanBeDead);

        projectile.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public static float[] findEquidistantPointsBetweenTwoPoints(int amountOfPoints, float leftMostPoint, float rightMostPoint)
    {
        float[] outputArray = new float[amountOfPoints + 1];
        float distanceBetweenTwoPoints = (rightMostPoint - leftMostPoint);

        outputArray[0] = leftMostPoint;

        for (int outputArrayIndex = 1; outputArrayIndex < outputArray.Length; outputArrayIndex++)
        {
            outputArray[outputArrayIndex] = leftMostPoint + ((distanceBetweenTwoPoints / amountOfPoints) * outputArrayIndex);
        }

        return outputArray;
    }

    public static bool trackerBeingTracked(IAnimationTracker tracker)
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

    public static int getCurrentFrameIndex(float elapsedTime, float maxTime, int frames)
    {
        if (elapsedTime > maxTime)
        {
            return frames;
        }

        return (int)((elapsedTime / maxTime) * (float)frames);
    }

    public static Vector3 calcZenithCoords(Vector3 startCoords, Vector3 endCoords)
    {
        Vector3 slopePoint1 = new Vector3(4.5f, -1.35f, 0f);
        Vector3 slopePoint2 = new Vector3(-0.5f, -3.5f, 0f);

        float x1 = slopePoint1.x;
        float x2 = slopePoint2.x;

        float y1 = slopePoint1.y;
        float y2 = slopePoint2.y;

        float slope = ((y2 - y1) / (x2 - x1));

        float middleX = ((startCoords.x + endCoords.x) / 2f);

        float zenithY = (slope * middleX) + adjustment;


        return new Vector3(middleX, zenithY, 0f);
    }

    public static void calcTrajectory(Vector3 startCoords, Vector3 endCoords, Vector3 zenithCoords, out Parabola trajectory)
    {
        float x1 = startCoords.x;
        float x2 = endCoords.x;
        float x3 = zenithCoords.x;
        float y1 = startCoords.y;
        float y2 = endCoords.y;
        float y3 = zenithCoords.y;

        double denom = (x1 - x2) * (x1 - x3) * (x2 - x3);
        double a = (x3 * (y2 - y1) + x2 * (y1 - y3) + x1 * (y3 - y2)) / denom;
        double b = (x3 * x3 * (y1 - y2) + x2 * x2 * (y3 - y1) + x1 * x1 * (y2 - y3)) / denom;
        double c = (x2 * x3 * (x2 - x3) * y1 + x3 * x1 * (x3 - x1) * y2 + x1 * x2 * (x1 - x2) * y3) / denom;

        trajectory = new Parabola(a, b, c);
    }


}

public abstract class Route
{
	public abstract double findY(double x);
}

public class Parabola : Route
{
	public double a;
	public double b;
	public double c;
	
	public Parabola(double a, double b, double c)
	{
		this.a = a;
		this.b = b;
		this.c = c;
	}
	
	public override double findY(double x)
	{
		return (a * x*x) + (b * x) + c;
	}
}

public class AppearAtDestination : Route
{
    public double y;

    public AppearAtDestination(double y)
    {
		this.y = y;
    }

    public override double findY(double x)
    {
		return y;
    }
}