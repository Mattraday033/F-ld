using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IAnimationTracker
{
    public int key;

    public ScriptOnLanding scriptOnLanding;
    public Animator animator;

    public Route trajectory;
    public float[] pointsBetweenActorAndTarget;

    public float elapsedTime = 0f;
    public float maxTime;

    public bool affectsDeadTargets = false;
    public GridCoords targetCoords;
    private Stats targetSnapshot;

    public int damage;
    public bool crit;
    public bool healsTarget;

    private bool skipSpawningDamageNumbers = false;

    // Update is called once per frame
    void Update()
    {

        if (animator.GetBool("inFlight"))
        {
            int currentIndex = getCurrentFrameIndex(elapsedTime, maxTime, pointsBetweenActorAndTarget.Length - 1);

            travelAlongTrajectory(currentIndex);
        }
        else
        {
            performLandingAnimation();
        }

        elapsedTime += Time.deltaTime;
    }

    private void travelAlongTrajectory(int pointIndex)
    {
        float x = pointsBetweenActorAndTarget[pointIndex];
        float y = (float)trajectory.findY((double)x);

        if (!float.IsNaN(x) && !float.IsNaN(y))
        {
            moveTo(new Vector3(x, y, 0f));
        }
        else
        {
            removeAnimation();
            return;
        }

        if (pointIndex >= pointsBetweenActorAndTarget.Length - 1)
        {
            transitionToLandingAnimation();
        }
    }

    private void performLandingAnimation()
    {

        if (animator.GetBool("spawnDamageNumber") && !skipSpawningDamageNumbers)
        {
            if (shouldSpawnDamageNumbers())
            {
                if((scriptOnLanding == null || !scriptOnLanding.ran) && targetSnapshot != null)
                {
                    Stats target = CombatGrid.getCombatantAtCoords(targetCoords);

                    if(target != null)
                    {
                        target.playAnimationOnDamage();
                    }
                }

                DamageNumberPopup.create(damage, transform.position, CombatAnimationManager.getInstance().damageNumberCanvas, crit, healsTarget);
            }

            skipSpawningDamageNumbers = true;
        }

        if(scriptOnLanding != null && !scriptOnLanding.ran)
        {
            StartCoroutine(runLandingScript(scriptOnLanding));
        }

        if (animator.GetBool("finished"))
        {
            removeAnimation();
        }
    }

    private const float timeToWaitB4Script = .175f;
    private IEnumerator runLandingScript(ScriptOnLanding script)
    {
        script.ran = true;
        float timeElapsed = 0f;

        while (timeElapsed < timeToWaitB4Script)
        {
            yield return null;
            timeElapsed += Time.deltaTime;
        }

        script.runScript();
    }

    private bool shouldSpawnDamageNumbers()
    {
        if (damage >= 0 && targetSnapshot != null && (!targetSnapshot.isDead() || (healsTarget && affectsDeadTargets)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void transitionToLandingAnimation()
    {
        animator.SetBool("inFlight", false);
        elapsedTime = 0f;
    }

    public void setTargetCoords(GridCoords targetCoords)
    {
        this.targetCoords = targetCoords.clone();
        this.targetSnapshot = CombatGrid.getCombatantAtCoords(this.targetCoords).clone();
    }

    public void moveTo(Vector3 newPosition)
    {
        transform.position = newPosition;

        Helpers.updateColliderPosition(gameObject);
    }

    private static int getCurrentFrameIndex(float elapsedTime, float maxTime, int numberOfPointsAlongTrajectory)
    {
        if (elapsedTime > maxTime)
        {
            return numberOfPointsAlongTrajectory;
        }

        return (int)((elapsedTime / maxTime) * (float)numberOfPointsAlongTrajectory);
    }

    public static Projectile instantiatePrefab()
    {
        return Instantiate(Resources.Load<GameObject>(PrefabNames.projectile)).GetComponent<Projectile>();
    }

    public void removeAnimation()
    {
        CombatAnimationManager.removeAnimation(key);

        DestroyImmediate(gameObject);

        CombatAnimationManager.checkAllAnimationsFinished();
    }

    public GameObject getGameObject()
    {
        return gameObject;
    }
}

public abstract class ScriptOnLanding
{
    public bool ran = false;

    public ScriptOnLanding()
    {

    }

    public abstract void runScript();
}

public class KnockBackOnLanding : ScriptOnLanding
{
    private GridCoords targetCoords;
    private Stats targetSnapshot;
    private GridCoords moveToCoords;
    private GridCoords collisionCoords;
    private Stats collisionTargetSnapshot;

    public KnockBackOnLanding(GridCoords targetCoords, GridCoords moveToCoords, GridCoords collisionCoords) : base()
    {
        this.targetCoords = targetCoords;

        if (CombatGrid.getCombatantAtCoords(targetCoords) != null)
        {
            this.targetSnapshot = CombatGrid.getCombatantAtCoords(targetCoords).clone();
        }



        this.moveToCoords = moveToCoords;

        if (CombatGrid.getCombatantAtCoords(collisionCoords) != null)
        {
            this.collisionTargetSnapshot = CombatGrid.getCombatantAtCoords(collisionCoords).clone();
        }

        this.collisionCoords = collisionCoords;
    }

    public override void runScript()
    {
        Stats combatantToBeMoved = CombatGrid.getCombatantAtCoords(targetCoords);
        Stats combatantCollision = CombatGrid.getCombatantAtCoords(collisionCoords);

        if(combatantToBeMoved != null)
        {
            combatantToBeMoved.moveTo(moveToCoords);

            if(!targetSnapshot.isDead())
            {
                Debug.LogError("combatantToBeMoved.isDead() = " + combatantToBeMoved.isDead());
                combatantToBeMoved.playAnimationOnDamage();
                combatantToBeMoved.updateHealthBar();
            }

            if(combatantCollision != null && !collisionTargetSnapshot.isDead())
            {
                combatantCollision.updateHealthBar();
                combatantCollision.playAnimationOnDamage();
            }
        }
    }
}
