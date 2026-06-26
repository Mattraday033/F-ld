using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate bool WinConditionCheck();
public delegate void CombatEndBehaviour();

public class WinCondition : IDescribable
{

    protected string winConName= "";
    private string winConDescription = "";
    private string iconName = "";

    private WinConditionCheck winLogic;
    private CombatEndBehaviour winBehaviour;
    private CombatEndBehaviour lossBehaviour;

    public WinCondition( string winConName, 
                         string iconName, 
                         string winConDescription = "", 
                         WinConditionCheck winLogic = null, 
                         CombatEndBehaviour winBehaviour = null, 
                         CombatEndBehaviour lossBehaviour = null)
    {
        this.winConName = winConName;
        this.winConDescription = winConDescription;
        this.iconName = iconName;
        this.winLogic = winLogic ?? WinLoseConditionList.defeatAllEnemiesLogic;
        this.winBehaviour = winBehaviour ?? WinLoseConditionList.showCombatResults;
        this.lossBehaviour = lossBehaviour ?? WinLoseConditionList.gameOver;
    }

    public virtual bool showWinConTutorial()
    {
        return true;
    }

    public virtual string getWinConDescription()
    {
        return winConDescription;
    }

    public Sprite getSprite()
    {
        return Helpers.loadSpriteFromResources(iconName);
    }

    public virtual bool playerHasWon()
    {
        return winLogic();
    }

    public void performWinBehaviour()
    {
        CombatUIModule.OnHideCombatUI.Invoke();
        winBehaviour();
    }

    public void performLossBehaviour()
    {
        CombatUIModule.OnHideCombatUI.Invoke();
        lossBehaviour();
    }

    #region IDescribable Methods
    
    public virtual string getName()
    {
        return "Win Con: " + winConName;
    }

	public bool ineligible()
    {
        return false;
    }

	public GameObject getRowType(RowType rowType)
    {
        return null;
    }

	public GameObject getDescriptionPanelFull()
    {
        return null;
    }

	public GameObject getDescriptionPanelFull(PanelType type)
    {
        return null;
    }

	public GameObject getDecisionPanel()
    {
        return null;
    }

	public bool withinFilter(string[] filterParameters)
    {
        return false;
    }

	public void describeSelfFull(DescriptionPanel panel)
    {
    }

	public void describeSelfRow(DescriptionPanel panel)
    {
    }

	public void setUpDecisionPanel(IDecisionPanel descisionPanel)
    {
    }

	public List<IDescribable> getRelatedDescribables()
    {
        return new List<IDescribable>();
    }

	public bool buildableWithBlocks()
    {
        return false;
    }
	public bool buildableWithBlocksRows()
    {
        return false;
    }
    #endregion

}

public class DefaultWinCondition : WinCondition
{
    public DefaultWinCondition( string winConName, 
                         string iconName, 
                         string winConDescription = "") :
    base(winConName, iconName, winConDescription)
    {
        
    }

    public override bool showWinConTutorial()
    {
        return false;
    }
}

public class WavesWinCondition : WinCondition
{

    public static int wavesDefeated;

    private const string wavesWinConDescriptionPart1 = "This enemy is too numerous and cannot be defeated. Instead, defeat ";
    private const string wavesWinConDescriptionPart2 = " waves to receive a bonus reward at the end of combat. If the Party is defeated before then, the Party will be moved to another location instead of dying.\n\nWaves defeated: ";

    private int wavesRequiredToWin = 0;

    public WavesWinCondition(int wavesRequiredToWin, 
                             string iconName, 
                             CombatEndBehaviour winBehaviour = null, 
                             CombatEndBehaviour lossBehaviour = null) : 
    base(wavesRequiredToWin.ToString(), iconName, winBehaviour: winBehaviour, lossBehaviour: lossBehaviour)
    {
        this.wavesRequiredToWin = wavesRequiredToWin;
    }
    
    public override string getName()
    {
        return "Win Con: Survive " + winConName + " Waves";
    }
    
    public override string getWinConDescription()
    {
        return wavesWinConDescriptionPart1 + winConName + wavesWinConDescriptionPart2 + wavesDefeated + "/" + winConName + "\n";
    }

    public override bool playerHasWon()
    {
        return wavesDefeated >= wavesRequiredToWin;
    }

    public static void incrementWavesDefeated()
    {
        wavesDefeated++;
    }

    private static void resetWavesDefeated()
    {
        wavesDefeated = 0;
    }

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        wavesDefeated = 0;

        CombatStateManager.OnCombatStart.AddListener(resetWavesDefeated);
        LoadSaveFile.OnLoadResetData.AddListener(resetWavesDefeated);
    }

}

public class EndOfCombatCutSceneScript
{
    private const float waitBeforeResultsScreen = 5f;
    private const float moveAlliesDownDuration = 1f;

    private static readonly string[] cutSceneSpriteNames = new string[]
    {
        "Javelineer", "Disciplinarian", "Spearman", "Axeman"
    };

    public void startCutScene()
    {
        // SecretDoorFlags.addSecretDoorFlag(SecretDoorKeyList.bodyPilePool);
        AreaManager.locationName = LocationNameList.bodyPile;
        CombatStateManager.instance.StartCoroutine(playCutScene());
    }

    private IEnumerator playCutScene()
    {
        if(CombatStateManager.whoseTurn == WhoseTurn.Won)
        {
            yield return new WaitForSeconds(.25f);
            EnvironmentalCombatActionList.addTakacsPuppetWaveSummon();

            CombatActionManager.getInstance().resolveACombatAction();
            CombatActionManager.getInstance().resolveACombatAction();
            CombatActionManager.getInstance().resolveACombatAction();
            CombatActionManager.getInstance().resolveACombatAction();

            yield return new WaitForSeconds(2.5f);
        } else
        {
            yield return new WaitForSeconds(.5f);
        }
        
        foreach (string spriteName in cutSceneSpriteNames)
        {
            AnimationManager.PlayAnimationByNPCName.Invoke(MonsterNameList.puppetedPrefix + spriteName, CharacterAnimationType.Attack_Normal_Front);
        }

        playBluntEffectOnAllAllies();

        yield return new WaitForSeconds(52f/60f);

        yield return CombatStateManager.instance.StartCoroutine(moveAllies());

        yield return new WaitForSeconds(3f);

		CombatUI.combatResultsPopUpButton.spawnPopUp();
    }

    private void playBluntEffectOnAllAllies()
    {
        List<Stats> allies = CombatGrid.getAllAllyCombatants();

        foreach (Stats ally in allies)
        {
            foreach (GridCoords allyCoords in ally.positions)
            {
                CombatAnimationManager.loadInstantEffect(EffectAnimationType.Blunt.ToString(), allyCoords, false, 0, false, true);
            }
        }
    }

    private IEnumerator moveAllies()
    {
        List<Transform> spriteTransforms = new List<Transform>();
        List<Vector3> startPositions = new List<Vector3>();
        List<Vector3> endPositions = new List<Vector3>();

        List<Stats> allies = CombatGrid.getAllAllyCombatants();

        foreach (Stats ally in allies)
        {
            if (ally.combatSprite == null || ally.positions.Count == 0)
            {
                continue;
            }

            GridCoords currentCoords = ally.positions[0];

            // World-space offset for moving one row south, computed from the grid so
            // the no-man's-land gap is accounted for. The CombatGrid dictionary is left untouched.
            Vector3 rowDownOffset = CombatGrid.getPositionAt(currentCoords.row + 15, currentCoords.col)
                                  - CombatGrid.getPositionAt(currentCoords.row, currentCoords.col);

            Transform spriteTransform = ally.combatSprite.transform;

            spriteTransforms.Add(spriteTransform);
            startPositions.Add(spriteTransform.position);
            endPositions.Add(spriteTransform.position + rowDownOffset);
        }

        float timeElapsed = 0f;

        while (timeElapsed < moveAlliesDownDuration)
        {
            float t = timeElapsed / moveAlliesDownDuration;

            for (int i = 0; i < spriteTransforms.Count; i++)
            {
                spriteTransforms[i].position = Vector3.Lerp(startPositions[i], endPositions[i], t);
            }

            yield return null;

            timeElapsed += Time.deltaTime;
        }

        for (int i = 0; i < spriteTransforms.Count; i++)
        {
            spriteTransforms[i].position = endPositions[i];
        }
    }
}
