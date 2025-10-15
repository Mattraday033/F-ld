using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SkillDescription : IDescribable, IDescribableInBlocks
{
    private string name;
    private string useDescription;
    private string iconName;

    private StatType requiredStatType;
    private int requiredStatLevel;

    public SkillDescription(string name, string useDescription, string iconName, StatType requiredStatType, int requiredStatLevel)
    {
        this.name = name;
        this.useDescription = useDescription;
        this.iconName = iconName;


        this.requiredStatType = requiredStatType;
        this.requiredStatLevel = requiredStatLevel;
    }

    public string getUseDescription()
    {
        return useDescription;
    }
    public string getIconName()
    {
        return iconName;
    }
    public string getType()
    {
        return "Skill";
    }
    public Sprite getIconSprite()
    {
        return Helpers.loadSpriteFromResources(getIconName());
    }
    public string getRange()
    {
        switch (requiredStatType)
        {
            case StatType.Str:
                return 3 + " Tiles";

            case StatType.Dex:
                return ((CunningManager.cunningRange - 1) / 2) + " Tiles";

            case StatType.Wis:
                return ((ObservationManager.observeRange - 1) / 2) + " Tiles";
                
            case StatType.Cha:
                return "1 Tile";

            default:
                throw new IOException("Unknown StatType: " + requiredStatType.ToString());
        }
    }
    public int getRequiredStatLevel()
    {
        return requiredStatLevel;
    }

    // IDescribable Methods
    public string getName()
    {
        return name;
    }

    public bool ineligible()
    {
        return false;
    }

    public virtual GameObject getRowType(RowType rowType)
    {
        string rowTypeName = "";

        switch (rowType)
        {
            case RowType.LevelUp:
                rowTypeName = PrefabNames.skillLevelUpDescriptionPanels;
                break;
            default:
                throw new IOException("Incompatible RowType: " + rowType);
        }

        return Resources.Load<GameObject>(rowTypeName);
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
        panel.setObjectBeingDescribed(this);

        DescriptionPanel.setText(panel.nameText, getName());
        DescriptionPanel.setText(panel.useDescriptionText, getUseDescription());
        DescriptionPanel.setText(panel.typeText, getType());
        DescriptionPanel.setText(panel.rangeText, getRange());

        DescriptionPanel.setImage(panel.iconPanel, getIconSprite());
    }

    public void describeSelfRow(DescriptionPanel panel)
    {

    }

    public void setUpDecisionPanel(IDecisionPanel descisionPanel)
    {

    }

    public ArrayList getRelatedDescribables()
    {
        return new ArrayList();
    }

    public bool buildableWithBlocks()
    {
        return true;
    }
    
    public bool buildableWithBlocksRows()
    {
        return true;
    }

    //IDescribableInBlocks methods
    public List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Name, getName()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getActionTypeBlock(getType()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getRangeBlock(getRange()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(getUseDescription()));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getIconName()));

        return buildingBlocks;
        
    }
}