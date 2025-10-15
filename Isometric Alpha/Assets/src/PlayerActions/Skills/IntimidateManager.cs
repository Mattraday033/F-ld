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

    public static int intimidatesRemaining = -1;

    public static int getIntimidatesRemaining()
    {

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
        return PlayerMovement.getInstance().intimidateManager;
    }

    public static void enterIntimidateMode()
    {
        SkillManager.destroyAllSkillGrids();
        IntimidateManager.getInstance().createSkillArea();
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.intimidating);
    }

    public static void leaveIntimidateMode()
    {
        IntimidateManager.getInstance().destroySkillArea();
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
    }

    public override void createSkillArea()
    {
        Vector3Int playerCoords = getPlayerCoords();

        int range = getRange();
        skillGrid = new GameObject[range, range];

        for (int row = 0; row < range; row++)
        {
            for (int col = 0; col < range; col++)
            {
                if (coordsWithinRange(row, col))
                {
                    skillGrid[row, col] = instantiateTile(playerCoords, row, col);
                    Helpers.updateColliderPosition(skillGrid[row, col]);
                }
                else
                {
                    continue;
                }

                skillGrid[row, col].GetComponent<SpriteRenderer>().color = getTileColor(skillGrid[row, col]);
            }
        }

        cullSkillArea();
    }

    public override string getTilePrefabName()
    {
        return PrefabNames.intimidateTileName;
    }

    public override Color getTileBaseColor()
    {
        return new Color(1f, 0.6470588f, 0f, 1f);
    }

    public override Color getTileTargetColor()
    {
        return Color.red;
    }

    private bool coordsWithinRange(int row, int col)
    {
        return (Math.Abs(row - getCurrentPlayerSkillGridCoords().row) + Math.Abs(col - getCurrentPlayerSkillGridCoords().col)) <= getCurrentPlayerSkillGridCoords().row;
    }

    public override int getRange()
    {
        return intimidateRange;
    }

    public override bool executeSkill()
    {
        ArrayList listOfTargets = new ArrayList();

        foreach (GameObject tile in skillGrid)
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
            }

            destroySkillArea();
            decrementIntimidatesRemaining();

            return true;
        }
        else
        {
            return false;
        }
    }
    
    public static void destroyIntimdiateSkillArea()
    {
        if (getInstance() != null)
        {
            getInstance().destroySkillArea();
        }
    }
}
