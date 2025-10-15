using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangeDescriptionPanelRow : DescriptionPanelRow
{

    public FormationDisplayUI formationDisplayUI;
    public GridGlossaryEntry glossaryEntry;

    private void OnEnable()
    {
        DescriptionPanelBuilder.OnFormulaSwap.AddListener(displayFormationUI);
    }

    private void OnDisable()
    {
        DescriptionPanelBuilder.OnFormulaSwap.RemoveListener(displayFormationUI);
    }

    private void displayFormationUI()
    {
        if (OverallUIManager.showFormula)
        {
            if (glossaryEntry == null)
            {
                setGlossaryEntry();
            }

            if (glossaryEntry != null)
            {
                glossaryEntry.applyToFormationDisplayUI(formationDisplayUI);
                formationDisplayUI.gameObject.SetActive(true);
            }
        }
        else
        {
            formationDisplayUI.gameObject.SetActive(false);
        }
    }


    private void setGlossaryEntry()
    {
        ArrayList allRangeEntries = Range.getAllRangesGlossaryEntries();

        foreach (GridGlossaryEntry entry in allRangeEntries)
        {
            if (entry.title.Equals(descriptionText.text))
            {
                glossaryEntry = entry;
                return;
            }
        }
    }
}
