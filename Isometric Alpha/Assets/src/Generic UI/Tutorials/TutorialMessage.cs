using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMessage : IDescribable
{
    private string name;
    private string message;
    private string imageKey;

    public TutorialMessage(string name, string message)
    {
        this.name = name;
        this.message = message;
        this.imageKey = "";
    }

    public TutorialMessage(string name, string message, string imageKey)
    {
        this.name = name;
        this.message = message;
        this.imageKey = imageKey;
    }

    //IDescribable Methods
    public string getName()
    {
        return name;
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
        return getDescriptionPanelFull(PanelType.Standard);
    }

    public GameObject getDescriptionPanelFull(PanelType type)
    {
        if (imageKey != null && !imageKey.Equals(""))
        {
            return Resources.Load<GameObject>(PrefabNames.tutorialMessageWithImage);
        }
        else
        {
            return Resources.Load<GameObject>(PrefabNames.tutorialMessageWithoutImage);
        }
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

        DescriptionPanel.setText(panel.useDescriptionText, message);
        DescriptionPanel.setImage(panel.iconPanel, Helpers.loadSpriteFromResources(imageKey));
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
        return false;
    }
    
    public bool buildableWithBlocksRows()
    {
        return false;
    }
}
