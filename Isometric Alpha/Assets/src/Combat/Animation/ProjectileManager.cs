using System;
using UnityEngine;

//Collects the code that launches projectiles and works out their flight paths. Everything here was moved out of
//AnimationTracker.cs; each member notes where it came from.
//The Projectile MonoBehaviour, ScriptOnLanding and KnockBackOnLanding stay in Projectile.cs, and ProjectileAnimationFinisher
//stays in its own file, because Unity prefabs and animator controllers reference those scripts by file.
public static class ProjectileManager
{
    //moved from AnimationTracker
    private const int framesBetweenActorAndTarget = 100;
    //moved from AnimationTracker
    private const float adjustment = -3f;

    //moved from AnimationTracker
    private const float defaultMaxTime = .5f;

    //moved from AnimationTracker. The visible projectile isn't used by any CombatAction yet; loadInvisibleProjectile still uses it.
    public static Projectile loadProjectile(GridCoords actorCoords, GridCoords targetCoords, bool crit, int damageNumber, bool healsTarget, bool targetCanBeDead)
    {
        return loadProjectile(actorCoords, targetCoords, crit, damageNumber, healsTarget, targetCanBeDead, null);
    }

    //moved from AnimationTracker
    public static Projectile loadProjectile(GridCoords actorCoords, GridCoords targetCoords, bool crit, int damageNumber, bool healsTarget, bool targetCanBeDead, ScriptOnLanding script)
    {
        if(CombatGrid.combatantIsRepositionClone(targetCoords))
        {
            return null;
        }

        Projectile currentProjectile = Projectile.instantiatePrefab();

        currentProjectile.scriptOnLanding = script;

        currentProjectile.damage = damageNumber;
        currentProjectile.crit = crit;
        currentProjectile.healsTarget = healsTarget;

        currentProjectile.setTargetCoords(targetCoords);
        currentProjectile.affectsDeadTargets = targetCanBeDead;

        currentProjectile.maxTime = defaultMaxTime;

        AnimationTracker.trackAnimation(currentProjectile.key, currentProjectile, currentProjectile.maxTime * 4);

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
                GameObject.Destroy(currentProjectile.gameObject);
                return null;
            }
        }
    }

    //moved from AnimationTracker. In use: carries KnockBackAbility's landing script (see AnimationTracker.loadInstantEffect).
    public static void loadInvisibleProjectile(GridCoords actorCoords, GridCoords targetCoords, bool crit, int damageNumber, bool healsTarget, bool targetCanBeDead, ScriptOnLanding script = null)
    {
        Projectile projectile = loadProjectile(actorCoords, targetCoords, crit, damageNumber, healsTarget, targetCanBeDead, script);

        if(projectile == null)
        {
            return;
        }

        projectile.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    //moved from AnimationTracker
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

    //moved from AnimationTracker. Currently unused: Projectile.cs has its own private copy.
    public static int getCurrentFrameIndex(float elapsedTime, float maxTime, int frames)
    {
        if (elapsedTime > maxTime)
        {
            return frames;
        }

        return (int)((elapsedTime / maxTime) * (float)frames);
    }

    //moved from AnimationTracker
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

    //moved from AnimationTracker
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

//moved from AnimationTracker.cs
public abstract class Route
{
	public abstract double findY(double x);
}

//moved from AnimationTracker.cs
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

//moved from AnimationTracker.cs
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
