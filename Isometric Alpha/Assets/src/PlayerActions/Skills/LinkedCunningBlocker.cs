using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LinkedCunningBlocker : CunningBlocker
{

    protected readonly static UnityEvent<LinkedCunningBlocker> GetLinkedBlocker = new UnityEvent<LinkedCunningBlocker>();

    public int linkedIndex = -1;
    public bool midCunning = false;

    public LinkedCunningBlocker linkedBlocker;

    void Start()
    {
        GetLinkedBlocker.Invoke(this);
    }

    public override bool validTarget(SkillType skillType)
    {
        if(linkedBlocker.midCunning)
        {
            return base.validTarget(skillType);
        }

        midCunning = true;

        bool valid = base.validTarget(skillType) && linkedBlocker.validTarget(skillType);

        midCunning = false;

        return valid;
    }

    public override void cunning(bool trackChangeInStateManager)
    {
        midCunning = true;

        base.cunning(trackChangeInStateManager);

        if (!linkedBlocker.midCunning)
        {
            linkedBlocker.cunning(trackChangeInStateManager);
        }

        midCunning = false;
    }

    public void linkSelf(LinkedCunningBlocker linkedCunningBlocker)
    {
        if(linkedCunningBlocker.linkedIndex == index)
        {
            linkedCunningBlocker.linkedBlocker = this;
        }
    }

    public override void createListeners()
    {
        base.createListeners();

        GetLinkedBlocker.AddListener(linkSelf);
    }

    public override void destroyListeners()
    {
        base.destroyListeners();

        GetLinkedBlocker.RemoveListener(linkSelf);
    }

}
