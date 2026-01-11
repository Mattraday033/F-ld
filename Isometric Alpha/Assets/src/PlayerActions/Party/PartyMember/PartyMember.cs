using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PartyMember : IDescribable, IDescribableInBlocks
{
	public AllyStats stats;

	public bool placed;       //if they were placed on the overworld somewhere

	private bool _CanJoinParty;
	public bool canJoinParty
    {
        get
        {
            return _CanJoinParty;
        }
        set
        {
            _CanJoinParty = value;
            PartyManager.OnPartyChange.Invoke();
        }
    }

	public Vector3 placedPosition = Vector3.zero;

    public PartyMember(AllyStats stats)
    {
        this.stats = stats;
	}

	public string getName()
	{
		return stats.getName();
	}

    public bool isInParty()
    {
        return stats.isInParty(State.formation.getGrid());
    }

	public bool isInParty(Stats[][] positionGrid)
	{

		return stats.isInParty(positionGrid);
	}

	public CombatActionArray getUnlockedCombatActions()
	{
		// CombatAction[] unlockedCombatActions = new CombatAction[PartyMemberStats.partyMemberActionWheelLength];

		// unlockedCombatActions[0] = new Attack(PartyMemberEquipmentManager.getWeapon(stats.getName(), stats.getLevel()));

		// for (int currentLevel = 2; currentLevel <= stats.getLevel() && currentLevel <= PartyMemberStats.partyMemberActionWheelLength; currentLevel++)
		// {
		// 	unlockedCombatActions[currentLevel - 1] = AbilityList.getCompanionAbility(stats.getName(), currentLevel - 2);
		// }

		return new CombatActionArray(stats);
	}

	public static int getNextUpgradeCost(int currentLevel)
	{
		switch (currentLevel)
		{
			case 1:
				return 250;
			case 2:
				return 1250;
			case 3:
				return 4000;
			case 4:
				return 10000;
			default:
				return -1;
		}
	}

	public bool canBeUpgraded()
	{
		int affinity = AffinityManager.getTotalAffinity();

		if (affinity >= getNextUpgradeCost(stats.getLevel()))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

    //IDescribable Methods
    public bool ineligible()
    {
        // return !canJoinParty;
        return false;
	}

	public GameObject getRowType(RowType rowType)
	{
		string rowTypeName = "";

		switch (rowType)
		{
			case RowType.Standard:
			case RowType.StatRequirements:
			case RowType.CompanionAbilities:
			case RowType.AbilityEditor:
				rowTypeName = PrefabNames.partyMemberRow;
				break;
			case RowType.FormationEditor:
				rowTypeName = PrefabNames.formationEditorRow;
				break;
			case RowType.PartyScreen:
				rowTypeName = PrefabNames.partyMemberSpriteRow;
				break;
			default:
				throw new IOException("Incompatible RowType: " + rowType);
		}

		return Resources.Load<GameObject>(rowTypeName);
	}

	public GameObject getDescriptionPanelFull()
	{
		return getDescriptionPanelFull(PanelType.Standard);
	}

	public GameObject getDescriptionPanelFull(PanelType type)
	{
		string panelName = "";

		switch (type)
		{
			case PanelType.PartyScreenMain:
				panelName = PrefabNames.partyScreenMainDescPanel;
				break;
			default:
				panelName = PrefabNames.partyMemberDescriptionPanel;
				break;
		}
		return Resources.Load<GameObject>(panelName);
	}

	public GameObject getDecisionPanel()
	{
		return null;
	}

	public bool withinFilter(string[] filterParameters)
	{
		return true;
	}

	public void describeSelfFull(DescriptionPanel panel)
	{
		panel.setObjectBeingDescribed(this);

		// DescriptionPanel.setImageColor(panel.iconPanel, spriteColor);

		DescriptionPanel.setText(panel.nameText, getName().Replace(PartyManager.playerMarker, ""));
		
		DescriptionPanel.setImage(panel.iconPanel, stats.getSpriteIcon());

	}

    public void describeSelfRow(DescriptionPanel panel)
    {
        panel.setObjectBeingDescribed(this);

        DescriptionPanel.setText(panel.nameText, getName().Replace(PartyManager.playerMarker, ""));
        DescriptionPanel.setText(panel.levelText, stats.getLevel());
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
		return true;
	}

	public bool buildableWithBlocksRows()
	{
		return true;
	}

	public List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{
		return stats.getDescriptionBuildingBlocks();
	}

    public override bool Equals(object obj)
    {
        PartyMember otherPartyMember = obj as PartyMember;

        if(otherPartyMember == null)
        {
            return false;
        }

        if((otherPartyMember.stats == null && stats != null) || 
            (otherPartyMember.stats != null && stats == null))
        {
            return false;
        }

        return otherPartyMember.stats.getName().Equals(stats.getName());
    }

}

public class CompanionCombatActionDescriptionWrapper : IDescribable, IDescribableInBlocks
{
	private int requiredLevel;
	private PartyMember partyMember;
	private CombatAction companionCombatAction;

	public CompanionCombatActionDescriptionWrapper(PartyMember partyMember, int requiredLevel, CombatAction companionCombatAction)
	{
		this.partyMember = partyMember;
		this.requiredLevel = requiredLevel;
		this.companionCombatAction = companionCombatAction;
	}

	public bool ineligible()
	{
		if (partyMember.stats.getLevel() >= requiredLevel)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	public string getName()
	{
		return companionCombatAction.getName();
	}

	public GameObject getRowType(RowType rowType)
	{
		return companionCombatAction.getRowType(rowType);
	}

	public GameObject getDescriptionPanelFull()
	{
		return getDescriptionPanelFull(PanelType.Standard);
	}

	public GameObject getDescriptionPanelFull(PanelType type)
	{
		return Resources.Load<GameObject>(PrefabNames.companionCombatActionDescriptionPanels);
	}

	public GameObject getDecisionPanel()
	{
		return null;
	}

	public bool withinFilter(string[] filterParameters)
	{
		return companionCombatAction.withinFilter(filterParameters);
	}

	public void describeSelfFull(DescriptionPanel panel)
	{
		companionCombatAction.describeSelfFull(panel);

		DescriptionPanel.setText(panel.damageText, companionCombatAction.getDamageFormulaTotal());
		DescriptionPanel.setText(panel.critRatingText, companionCombatAction.getCritFormulaTotalForDisplay());

		panel.setObjectBeingDescribed(this);
	}

	public void describeSelfRow(DescriptionPanel panel)
	{
		companionCombatAction.describeSelfRow(panel);

		DescriptionPanel.setText(panel.levelText, requiredLevel);

		panel.setObjectBeingDescribed(this);
	}

	public void setUpDecisionPanel(IDecisionPanel decisionPanel)
	{
		companionCombatAction.setUpDecisionPanel(decisionPanel);
	}

	public List<IDescribable> getRelatedDescribables()
	{
		return new List<IDescribable>();
	}

	public bool buildableWithBlocks()
	{
		return companionCombatAction.buildableWithBlocks();
	}
	
	public bool buildableWithBlocksRows()
    {
        return companionCombatAction.buildableWithBlocksRows();
    }
	public List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{
		return companionCombatAction.getDescriptionBuildingBlocks();
	}
}