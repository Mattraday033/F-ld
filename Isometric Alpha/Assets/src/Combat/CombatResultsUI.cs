using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

public class CombatResultsUI : PopUpWindow
{
    public readonly static UnityEvent OnCombatResultsUICreation = new UnityEvent();

	private const bool defeatedEnemy = true;

    public TextMeshProUGUI goldText;
	public TextMeshProUGUI xpText;

	private static CombatResultsUI instance;

	public static CombatResultsUI getInstance()
	{
		return instance;
	}

    private void Awake()
    {
		if(instance != null)
		{
			throw new IOException("Duplicate instances of CombatResultsUI exist");
		}

		instance = this;
        OnCombatResultsUICreation.Invoke();

        AudioManager.playNoMusic();
        AudioManager.playAudioClipAsSingleton(AudioClipList.winMusic);
    }

	void Update()
	{
		KeyPressManager.updateKeyBools();

        if(InspectNode.inspecting)
        {
            return;
        }

		if ((Input.GetKey(KeyBindingList.combatSelectKey.getCurrentKeyCode()) || 
                Input.GetKey(KeyBindingList.acceptKey.getCurrentKeyCode()) || 
                Input.GetKey(KeyBindingList.acceptInputKey.getCurrentKeyCode())
                || KeyBindingList.settingsScreenOrBackKeyPressed())
			&& !KeyPressManager.handlingPrimaryKeyPress)
		{
			acceptButtonPress();
			KeyPressManager.handlingPrimaryKeyPress = true;
		}
    }

    public void displayDrops(EnemyPackInfo packInfo)
	{
		displayDrops(DropTableList.getDropTable(packInfo.dropTableName),
					 packInfo.guaranteedDrops,
					 packInfo.getXPDrops());
	}

    private void displayDrops(DropTable dropTable, ItemListID[] guaranteedDrops, int xpDropped)
    {
        List<Item> itemDrops = CombatResultsManager.determineItemDrops(dropTable, guaranteedDrops);
        int goldDropped = CombatResultsManager.determineGoldDrops(dropTable);

        if (xpDropped < 0)
        {
            xpDropped = 0;
        }

        xpText.text = xpDropped + "";

        goldText.text = goldDropped + Purse.moneySymbol;

        CombatResults combatResults = new CombatResults(itemDrops);

        descriptionPanelSlot.setPrimaryDescribable(combatResults);
    }
    
    public void applyRegenerationToParty()
    {
        foreach(AllyStats ally in State.formation)
        {
            if(ally != null && Strength.getCurrentRegenerationAmount(ally) > 0)
            {
                Strength.applyRegeneration(ally);
            }
        }
    }

	public override void acceptButtonPress()
    {
        EscapeStack.handleEscapePress();

        applyRegenerationToParty();

        CombatStateManager.returnToOverworld(defeatedEnemy);
        
        AudioManager.setMusicSourceVolume(0f);
        
        MusicFade.OnMusicMidFade.Invoke();
    }

}

public class CombatResults : IDescribable, IDescribableInBlocks
{
    public List<Item> loot;

    public CombatResults(List<Item> loot)
    {
        this.loot = loot;
    }

    public List<DescriptionPanelBuildingBlock> getRegenerationDescription()
    {
        List<DescriptionPanelBuildingBlock> blocks = new List<DescriptionPanelBuildingBlock>();

        foreach(AllyStats ally in State.formation)
        {
            if(ally != null && Strength.getCurrentRegenerationAmount(ally) > 0)
            {
                int regen = Strength.getCurrentRegenerationAmount(ally);

                blocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(ally.getName().Replace(PartyManager.playerMarker, "") + " heals " + regen + " health."));
            }
        }

        return blocks;
    }

    #region IDescribable

    public string getName()
    {
        return "Combat Results";
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
        return true;
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
        return true;
    }

	public bool buildableWithBlocksRows()
    {
        return true;
    }

#endregion

#region IDescribableInBlocks

    public List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        List<DescriptionPanelBuildingBlock> blocks = new List<DescriptionPanelBuildingBlock>();

        blocks.Add(DescriptionPanelBuildingBlock.getNameBlock(getName()));

        blocks.AddRange(getRegenerationDescription());

        foreach(Item item in loot)
        {
            blocks.Add(new DescriptionPanelBuildingBlock(item));
        }

        if(blocks.Count <= 1)
        {
            blocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock("None"));
        }

        return blocks;
    }

    public bool requiresInspectNode() 
    { 
        return false; 
    }

#endregion

}
