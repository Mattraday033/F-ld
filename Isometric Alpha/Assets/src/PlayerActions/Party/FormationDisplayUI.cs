using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationDisplayUI : MonoBehaviour
{
	public PartyPositionGridRow[] formationUIGrid;
	
	public void setColorOfGridSquare(GridCoords coords, Color color)
	{
		formationUIGrid[coords.row].cells[coords.col].image.color = color;
	}
	
	public void setToReadOnly()
	{
		for(int rowIndex = 0; rowIndex < formationUIGrid.Length; rowIndex++)
		{
			for(int colIndex = 0; colIndex < formationUIGrid[rowIndex].cells.Length; colIndex++)
			{
				formationUIGrid[rowIndex].cells[colIndex].button.enabled = false;
			}
		}
	}

	public void setEmptySquaresToInteractable(Formation formation)
	{
		setEmptySquaresInterability(formation, true);
	}
	
	public void setEmptySquaresToUninteractable(Formation formation)
	{
		setEmptySquaresInterability(formation, false);
	}

	private void setEmptySquaresInterability(Formation formation, bool interactable)
	{
		for (int rowIndex = 0; rowIndex < formationUIGrid.Length; rowIndex++)
		{
			for (int colIndex = 0; colIndex < formationUIGrid[rowIndex].cells.Length; colIndex++)
			{
				if (formation.getGrid()[rowIndex][colIndex] == null)
				{
					formationUIGrid[rowIndex].cells[colIndex].button.interactable = interactable;
				}
			}
		}
	}
	
	public void populate(Formation formation)
	{
		for (int rowIndex = 0; rowIndex < formation.getGrid().Length; rowIndex++)
		{
			for (int colIndex = 0; colIndex < formation.getGrid()[rowIndex].Length; colIndex++)
			{
				PartyPositionGridSquare gridSquare = getGridSquareAtPosition(rowIndex, colIndex);

				if (formation.getGrid()[rowIndex][colIndex] != null)
				{
					gridSquare.populate(formation.getGrid()[rowIndex][colIndex]);
				}
				else
				{
					gridSquare.populate();
				}

				gridSquare.determineButtonEnabled();
			}
		}
	}
	
	public PartyPositionGridSquare getGridSquareAtPosition(int row, int col)
	{
		return formationUIGrid[row].cells[col];
	}
	
}
