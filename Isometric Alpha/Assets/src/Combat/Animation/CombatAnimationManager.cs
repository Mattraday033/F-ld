using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimationManager : MonoBehaviour
{
	
	public Transform damageNumberCanvas;
	
	public static Dictionary<int, Projectile> currentProjectiles = new Dictionary<int, Projectile>();
	
	private static CombatAnimationManager instance;
	
	public CombatStateManager combatStateManager;
	public CombatResultsUI combatResultsUI;
	public GameObject combatResultsUIScreen;
		
	private static int framesBetweenActorAndTarget = 100;
	private static float adjustment = -3f;
	public static int frameWaitMax = 1;

	private const float defaultMaxTime = .5f;

	private static int currentKey = 0;
	
	public float[] drawFrameAtThisX;
	
	private double a;
	private double b;
	private double c;
	
	private void Awake()
	{
		if(instance != null)
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
		foreach(KeyValuePair<int,Projectile> kvp in currentProjectiles)
		{
			Destroy(kvp.Value.gameObject);
		}
		
		currentProjectiles = new Dictionary<int,Projectile>();
	}
	
	public int getCurrentKey()
	{
		currentKey++;
		return currentKey;
	}
	
	public static void animationFinishedCombatActions()
	{
		if(!getInstance().hasOngoingAnimations())
		{
			DeadCombatantManager.handleDeadCombatants();
			
			CombatStateManager.getInstance().checkForWinOrLossStates(); 
			
			CombatUI.populateCombatActionPanels();
		}
	}
	
	public void loadProjectile(GridCoords actorCoords, GridCoords targetCoords, bool crit, int damageNumber, bool healsTarget)
	{
		loadProjectile(actorCoords, targetCoords, crit, damageNumber, 0, healsTarget, false);
	}
	public void loadProjectile(GridCoords actorCoords, GridCoords targetCoords, bool crit, int damageNumber, int framesToWait, bool healsTarget, bool targetCanBeDead)
	{
		int key = getCurrentKey();

		Projectile currentProjectile = Instantiate(Resources.Load<GameObject>("ProjectileTestPrefab")).GetComponent<Projectile>();

		currentProjectile.key = key;

		currentProjectile.damage = damageNumber;
		currentProjectile.crit = crit;
		currentProjectile.healsTarget = healsTarget;

		currentProjectile.targetCoords = targetCoords;
		currentProjectile.affectsDeadTargets = targetCanBeDead;

		currentProjectile.maxTime = defaultMaxTime;

		currentProjectiles.Add(key, currentProjectile);

		if (CombatGrid.positionsAreOnSameSide(actorCoords, targetCoords))
		{
			Vector3 endPosition = CombatGrid.getPositionAt(targetCoords);
			AppearAtDestination trajectory = new AppearAtDestination(endPosition.y);

			currentProjectile.trajectory = trajectory;
			currentProjectile.pointsBetweenActorAndTarget = new float[1] { endPosition.x };

			currentProjectile.moveTo(targetCoords.toVector3());
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
			}
			catch (IndexOutOfRangeException e)
			{
				Debug.LogError("Caught IndexOutOfRangeException: actorCoords = " + actorCoords.ToString());
				Destroy(currentProjectile.gameObject);
			}
		}
	}
	
	
	public float[] findEquidistantPointsBetweenTwoPoints(int amountOfPoints, float leftMostPoint, float rightMostPoint)
	{
		float[] outputArray = new float[amountOfPoints+1];
		float distanceBetweenTwoPoints = (rightMostPoint - leftMostPoint);
		
		outputArray[0] = leftMostPoint;
		
		for(int outputArrayIndex = 1; outputArrayIndex < outputArray.Length; outputArrayIndex++)
		{
			outputArray[outputArrayIndex] = leftMostPoint + ((distanceBetweenTwoPoints/amountOfPoints)*outputArrayIndex);
		}
		
		return outputArray;
	}
	
	/*
	private IEnumerator moveProjectile(int key, float[] pointsBetweenActorAndTarget, Parabola trajectory, bool crit, int damageNumber, int framesToWaitBeforeFiring, bool healsTarget, float frameSpeed)
    {

		animator.SetBool("inFlight", false);
		
		int explosionFrameCount = 0;
		bool damageNumberCreated = false;
		while(!animator.GetBool("finished") && explosionFrameCount <= 300)
		{	
			explosionFrameCount++;
			
			if(explosionFrameCount > 12 && !damageNumberCreated)
			{
				if(damageNumber >= 0)
				{
					DamageNumberPopup.create(damageNumber, projectile.transform.position, damageNumberCanvas, crit, healsTarget);
				}
				
				damageNumberCreated = true;
			}
			
			yield return null;
		}
		
		
		currentProjectiles.Remove(key);
		
		animationFinishedCombatActions();
		
    }
	*/

    public bool hasOngoingAnimations()
	{
		if(currentProjectiles.Count > 0)
		{
			return true;
		} else
		{
			return false;
		}
	}
	
	public static int getCurrentFrameIndex(float elapsedTime, float maxTime, int frames)
	{
		if(elapsedTime > maxTime)
		{
			return frames;
		}

		return (int) ((elapsedTime/maxTime) * (float) frames);
	}

	public static Vector3 calcZenithCoords(Vector3 startCoords, Vector3 endCoords)
	{
		Vector3 slopePoint1 = new Vector3(4.5f,-1.35f,0f);
		Vector3 slopePoint2 = new Vector3(-0.5f,-3.5f,0f);
		
		float x1 = slopePoint1.x;
		float x2 = slopePoint2.x;
		
		float y1 = slopePoint1.y;
		float y2 = slopePoint2.y;
		
		float slope = ((y2 - y1) / (x2 - x1));
		
		float middleX = ((startCoords.x + endCoords.x)/2f);
		
		float zenithY = (slope * middleX) + adjustment;
		
		
		return new Vector3(middleX,zenithY,0f);
	}
	
	/*
	public static Vector3 calcZenithCoords(Vector3 startCoords, Vector3 endCoords)
	{
		
		
		float x1 = startCoords.x;
		float x2 = endCoords.x;
		float y1 = startCoords.y;
		float y2 = endCoords.y;
		
		float slope = ((y2 - y1) / (x2 - x1));
		
		float middleX = ((x1 + x2)/2f);
		
		float zenithY = (slope * middleX) + adjustment;
		
		
		return new Vector3(middleX,zenithY,0f);
	}
	*/
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
		double b = (x3*x3 * (y1 - y2) + x2*x2 * (y3 - y1) + x1*x1 * (y2 - y3)) / denom;
		double c = (x2 * x3 * (x2 - x3) * y1 + x3 * x1 * (x3 - x1) * y2 + x1 * x2 * (x1 - x2) * y3) / denom;
		
		trajectory = new Parabola(a,b,c);
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
/*
 * 
 * private IEnumerator moveProjectile(int key, float[] pointsBetweenActorAndTarget, Parabola trajectory, bool crit, int damageNumber, int framesToWaitBeforeFiring, bool healsTarget)
    {
		for(int frameWaitCount = 0; frameWaitCount < framesToWaitBeforeFiring; frameWaitCount++)
		{
			yield return null;
		}
		
		GameObject projectile = currentProjectiles[key];
		
		Animator animator = projectile.GetComponent<Animator>();
		
		for(int frameCount = 0; frameCount < pointsBetweenActorAndTarget.Length; frameCount++)
		{
			
			float x = pointsBetweenActorAndTarget[frameCount];
			float y = (float) trajectory.findY((double) x);
			
			if(!float.IsNaN(x) && !float.IsNaN(y))
			{
				projectile.transform.position = new Vector3(x,y,0f);
			} else
			{
				projectile.SetActive(false);
				break;
			}
			
			Helpers.updateColliderPosition(projectile);
			
			for(int frameWaitCount = 0; frameWaitCount < frameWaitMax; frameWaitCount++)
			{
				yield return null;
			}
		}
		
		animator.SetBool("inFlight", false);
		
		int explosionFrameCount = 0;
		bool damageNumberCreated = false;
		while(!animator.GetBool("finished") && explosionFrameCount <= 300)
		{	
			explosionFrameCount++;
			
			if(explosionFrameCount > 12 && !damageNumberCreated)
			{
				if(damageNumber >= 0)
				{
					DamageNumberPopup.create(damageNumber, projectile.transform.position, damageNumberCanvas, crit, healsTarget);
				}
				
				damageNumberCreated = true;
			}
			
			yield return null;
		}
		
		
		GameObject.Destroy(projectile);
		
		currentProjectiles.Remove(key);
		
		animationFinishedCombatActions();
    }
*/