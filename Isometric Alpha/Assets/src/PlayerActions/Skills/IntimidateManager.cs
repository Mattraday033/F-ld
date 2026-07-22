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
    private static void initializeInimidateManager()
    {
        intimidatesRemaining = -1;
        targetsFound = 0;

        PlayerOOCStateManager.OnStateChangeToSkill.AddListener(noLongerHasTarget);
        PlayerOOCStateManager.OnStateChangeFromSkill.AddListener(noLongerHasTarget);
        OnSkillTargetFound.AddListener(incrementIntimidateTargets);

        LoadSaveFile.OnLoadReadBlueprint.AddListener(readSaveBlueprint);
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
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.intimidating);
        IntimidateManager.getInstance().createSkillArea();
        PlayerObject.setButtonPromptVisibility();
    }

    public static void leaveIntimidateMode()
    {
        IntimidateManager.getInstance().destroySkillArea();
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
        PlayerObject.setButtonPromptVisibility();
    }

    public override void createSkillArea()
    {
        Vector3Int playerCoords = getPlayerCoords();

        int range = getRange();
        skillGrid = new SkillIndicator[range, range];

        for (int row = 0; row < range; row++)
        {
            for (int col = 0; col < range; col++)
            {
                if (coordsWithinRange(row, col))
                {
                    skillGrid[row, col] = instantiateTile(playerCoords, row, col).GetComponent<SkillIndicator>();
                    skillGrid[row, col].updateColliderPosition();
                }
                else
                {
                    continue;
                }

                skillGrid[row, col].setColor(getTileColor(skillGrid[row, col]));
            }
        }

        cullSkillArea();
    }

    public override bool canUseSkill()
    {
        return targetsFound > 0 && getIntimidatesRemaining() > 0;
    }

    public override string getTilePrefabName()
    {
        return PrefabNames.intimidateTileName;
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
