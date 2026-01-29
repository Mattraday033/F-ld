using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetToSkillScript : TutorialSequenceStepScript
{
    private SkillType skillType;

    public SetToSkillScript(SkillType skillType)
    {
        this.skillType = skillType;
    }

    public override void runScript(GameObject target)
    {
        SkillButtonManager.setToSkill(skillType);
    }

}
