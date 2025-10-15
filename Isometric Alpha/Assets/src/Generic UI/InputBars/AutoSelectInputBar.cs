using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoSelectInputBar : MonoBehaviour 
{
    public TMP_InputField inputField;
    public ScrollableUIElement grid;

    public void clickOnRow()
    {
        string rowName = inputField.text;

        if(!rowName.Equals(grid.getDisabledRowName()) && grid.disableGridRowAndClick(rowName))
        {
            grid.snapToDisabledRow();
        }
    }
}
