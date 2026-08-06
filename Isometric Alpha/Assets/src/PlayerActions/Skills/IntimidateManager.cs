using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using UnityEngine;

public class IntimidateManager : CunningManager
{
    public const int intimidateRange = 11;
    public const int playerIntimidateCoords = (intimidateRange - 1) / 2;

    public static int targetsFound;

    public static int intimidatesRemaining;

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        intimidatesRemaining = -1;
        targetsFound = 0;

        PlayerOOCStateManager.OnStateChangeToSkill.AddListener(noLongerHasTarget);
        PlayerOOCStateManager.OnStateChangeFromSkill.AddListener(noLongerHasTarget);
        OnSkillTargetFound.AddListener(incrementIntimidateTargets);

        LoadSaveFile.OnLoadReadBlueprint.AddListener(readSaveBlueprint);
    }

    public override ContactFilter2D getCollisionFilter()
    {
        ContactFilter2D filterCollider = new ContactFilter2D();
        filterCollider.useTriggers = true;
        filterCollider.SetLayerMask(LayerAndTagManager.blocksIntimidateLayerMask);

        return filterCollider;
    }


    private static void readSaveBlueprint(SaveBlueprint blueprint)
    {
        setIntimidatesRemaining(blueprint.intimidatesRemaining);
    }

    private static void incrementIntimidateTargets()
    {
        targetsFound++;
    }

    public static void decrementIntimidateTargets()
    {
        targetsFound--;
    }

    private static void noLongerHasTarget()
    {
        targetsFound = 0;
    }

    public static int getIntimidatesRemaining()
    {
        if(PartyStats.inTutorialArea() && Flags.getFlag(FlagNameList.startedTaborIntimidateTutorial))
        {
            return 1;
        }

        if (intimidatesRemaining < 0)
        {
            resetIntimidatesRemaining();
        }

        return intimidatesRemaining;

    }

    public static void setIntimidatesRemaining(int newIntimidatesRemaining)
    {
        intimidatesRemaining = newIntimidatesRemaining;
    }

    public static void incrementIntimidatesRemaining()
    {
        if (intimidatesRemaining + 1 <= PartyStats.getMaxIntimidateCount())
        {
            intimidatesRemaining++;
            OOCUIManager.updateOOCUI();
        }
    }

    public static void decrementIntimidatesRemaining()
    {
        if (intimidatesRemaining > 0)
        {
            intimidatesRemaining--;
            OOCUIManager.updateOOCUI();
        }
    }

    public static void resetIntimidatesRemaining()
    {
        intimidatesRemaining = PartyStats.getMaxIntimidateCount();

        if (State.oocUIManager != null)
        {
            OOCUIManager.updateOOCUI();
        }

    }

    public static IntimidateManager getInstance()
    {
        return new IntimidateManager();
    }

    public static void enterIntimidateMode()
    {
        SkillManager.destroyAllSkillGrids();
        IntimidateManager.getInstance().createSkillArea();
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.intimidating);
        PlayerObject.setButtonPromptVisibility();
    }

    public static void leaveIntimidateMode()
    {
        IntimidateManager.getInstance().destroySkillArea();
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
        PlayerObject.setButtonPromptVisibility();
    }

    protected override bool allowHovers()
    {
        return false;
    }

    protected override void setSelectorOriginTile()
    {
        //empty on purpose
    }

    public override bool canUseSkill()
    {
        return targetsFound > 0 && base.canUseSkill();
    }

    public static bool hasEnoughCharges()
    {
        return intimidatesRemaining >= getInstance().getHighestChargeCost();
    }

    private int getHighestChargeCost()
    {
        int highestChargeCost = 0;

        // if(skillGrid == null)
        // {
        //     return highestChargeCost;
        // }

        foreach(SkillIndicator tile in skillGrid)
        {
            if(tile == null)
            {
                continue;
            }

            ISkillTarget target = getTargetFromTile(tile);

            if(target != null)
            {
                int targetChargeCost = target.getChargeCost(SkillType.Intimidate);
       
                if(highestChargeCost < targetChargeCost)
                {
                    highestChargeCost = targetChargeCost;
                }
            }
        }

        return highestChargeCost;
    }

    public override Color getTileBaseColor()
    {
        return ColorList.intimidateIndicatorOrange;
    }

    public override Color getTileTargetColor()
    {
        return ColorList.skillIndicatorTargetableColor;
    }

    public override int getRange()
    {
        return intimidateRange;
    }

    public override bool cullFromCollision(Collider2D[] collisions)
    {
        foreach (Collider2D collision in collisions)
        {
            if (collision != null && 
                (collision.gameObject.layer == LayerAndTagManager.colliderLayer || 
                 collision.gameObject.layer == LayerAndTagManager.observableLayer))
            {
                return true;
            }
        }

        return false;
    }

    public override bool executeSkill()
    {
        if(!canUseSkill())
        {
            return false;
        }

        List<ISkillTarget> listOfTargets = new List<ISkillTarget>();

        foreach (SkillIndicator tile in skillGrid)
        {
            if (tile == null || tile is null)
            {
                continue;
            }

            ISkillTarget skillTarget = getTargetFromTile(tile);

            if (skillTarget != null && !(skillTarget is null))
            {
                listOfTargets.Add(skillTarget);
            }
        }

        if (listOfTargets.Count > 0)
        {
            foreach (ISkillTarget target in listOfTargets)
            {
                target.intimidate();
                // AudioManager.playSmokebombSFX();
                createEffect(target.getTargetPosition());
            }

            destroySkillArea();
            decrementIntimidatesRemaining();

            OnSkillUse.Invoke();

            return true;
        }
        else
        {
            return false;
        }
    }
    
    public override string getEffectType()
    {
        return EffectAnimationType.Intimidate.ToString();
    }

    public static void destroyIntimdiateSkillArea()
    {
        if (getInstance() != null)
        {
            getInstance().destroySkillArea();
        }
    }
}
