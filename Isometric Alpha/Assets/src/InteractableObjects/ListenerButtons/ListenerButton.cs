using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WallType {None = 0, RoundRubble = 1, SingleStalagmite = 2,  TripleStalagmite = 3, BushRock = 4}


public class ListenerButton : MonoBehaviour, IFloorButton 
{
    public WallType wallType;
    public Collider2D collider;
    public SpriteRenderer sprite;

    public void setActive(bool active)
    {
        if (active)
        {
            sprite.color = Color.green;
        }
        else
        {
            sprite.color = Color.white;
        }
    }
    
    private void OnEnable()
    {
        if (ListenerButtonManager.isFinished())
        {
            setActive(true);
            return;
        }

        declareButton();
        ListenerButtonManager.FinishedWithListenerButtons.AddListener(setActive);
    }

    private void OnDisable()
    {
        ListenerButtonManager.FinishedWithListenerButtons.RemoveListener(setActive);
    }

    public virtual void evaluate()
    {
        if(ListenerButtonManager.isFinished() || wallType == ListenerButtonManager.previousWallType)
        {
            return;
        }

        if (Helpers.hasCollision(collider) && Helpers.getCollision(collider).gameObject.tag.Equals(LayerAndTagManager.playerTag))
        {
            if (ListenerButtonManager.correctWallType(wallType))
            {
                if (ListenerButtonManager.codeIndex == ListenerButtonManager.getInstance().codeGameObjects.Length - 1)
                {
                    ListenerButtonManager.markAsFinished();
                    ListenerButtonManager.activateBridge();
                    ListenerButtonManager.FinishedWithListenerButtons.Invoke(true);
                    return;
                }

                ListenerButtonManager.activateNextCodeObject();
                ListenerButtonManager.incrementCodeIndex();
                ListenerButtonManager.setSaveFlag();
            }
            else
            {
                resetCode();
                ListenerButtonManager.getInstance().spawnMonster();
            }

            ListenerButtonManager.setPreviousWallType(wallType);
            ListenerButtonManager.FinishedWithListenerButtons.Invoke(false);
            sprite.color = Color.green;
        }
        else
        {
            sprite.color = Color.white;
        }
    }

    public bool currentlyActive()
    {
        return sprite.color != Color.white;
    }

    public void declareButton()
    {
        MovementManager.getInstance().addFloorButton(this);
    }

    public void resetCode()
    {
        ListenerButtonManager.resetCodeGameObjects();
    }

}
