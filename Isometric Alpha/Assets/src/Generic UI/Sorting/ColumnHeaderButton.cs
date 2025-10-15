using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;

public class ColumnHeaderButton : MonoBehaviour
{
    public ColumnHeader columnHeader;
    public SortBy sortBy;
    public int columnHeaderIndex;

    public void setComparisonMethod()
    {
        setComparisonMethod(true);
    }

    public void setComparisonMethod(bool populateGrid)
    {
        if(populateGrid)
        {
            columnHeader.setComparisonMethodAndPopulateGrid(ComparerList.getComparer(sortBy));
        } else
        {
            columnHeader.setComparisonMethod(ComparerList.getComparer(sortBy));
        }

        columnHeader.setCurrentHeaderButton(columnHeaderIndex);
    }
}
