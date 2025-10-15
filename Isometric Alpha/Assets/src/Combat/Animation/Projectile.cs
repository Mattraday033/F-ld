using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int key;

    public Animator animator;

    public Route trajectory;
    public float[] pointsBetweenActorAndTarget;

    public float elapsedTime = 0f;
    public float maxTime;

    public bool affectsDeadTargets = false;
    public GridCoords targetCoords;

    public int damage;
    public bool crit;
    public bool healsTarget;

    private bool skipSpawningDamageNumbers = false;

    // Update is called once per frame
    void Update()
    {

        if(animator.GetBool("inFlight"))
        {
            int currentIndex = getCurrentFrameIndex(elapsedTime, maxTime, pointsBetweenActorAndTarget.Length-1);

            travelAlongTrajectory(currentIndex);
        } else
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
            destroyProjectile();
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
            if(shouldSpawnDamageNumbers())
            {
                DamageNumberPopup.create(damage, transform.position, CombatAnimationManager.getInstance().damageNumberCanvas, crit, healsTarget);
            }

            skipSpawningDamageNumbers = true;
        }

        if (animator.GetBool("finished"))
        {
            destroyProjectile();
        }
    }

    private bool shouldSpawnDamageNumbers()
    {
        Stats target = CombatGrid.getCombatantAtCoords(targetCoords);

        if (damage >= 0 && (target != null && (!target.isDead || (healsTarget && affectsDeadTargets))))
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

    private float getTravelMaxTime()
    {
        return maxTime;
    }

    private float getLandingMaxTime()
    {
        return maxTime/2f;
    }

    public void moveTo(Vector3 newPosition)
    {
        transform.position = newPosition;

        Helpers.updateColliderPosition(gameObject);
    }

    private void destroyProjectile()
    {
        CombatAnimationManager.currentProjectiles.Remove(key);
        
        Destroy(gameObject);

        CombatAnimationManager.animationFinishedCombatActions();
    }

    private static int getCurrentFrameIndex(float elapsedTime, float maxTime, int numberOfPointsAlongTrajectory)
    {
        if (elapsedTime > maxTime)
        {
            return numberOfPointsAlongTrajectory;
        }

        return (int)((elapsedTime / maxTime) * (float) numberOfPointsAlongTrajectory);
    }
}
