using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IDialogueParticipant
{

    public string obstacleName;

    public void setObstacleName(string obstacleName)
    {
        this.obstacleName = obstacleName;
    }

    public string getName()
    {
        return obstacleName;
    }

    public Dialogue getDialogue()
    {
        return null;
    }

    public virtual void setToDown()
    {
        gameObject.SetActive(false);
    }
    
    public virtual void setToUp()
    {
        gameObject.SetActive(true);
    }

}
