using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingNPCMovement : MonoBehaviour
{

    private Coroutine currentMoveCoroutine;

    public void moveBetweenCells(Vector2Int startCell, Vector2Int endCell, float timeToMove, bool disableAtEnd = false, string effectAtEndName = null)
    {
        if (currentMoveCoroutine != null)
        {
            StopCoroutine(currentMoveCoroutine);
        }

        currentMoveCoroutine = StartCoroutine(moveBetweenCellsCoroutine(startCell, endCell, timeToMove, disableAtEnd, effectAtEndName));
    }

    private IEnumerator moveBetweenCellsCoroutine(Vector2Int startCell, Vector2Int endCell, float timeToMove, bool disableAtEnd = false, string effectAtEndName = null)
    {
        Grid grid = AreaManager.getMasterGrid();

        Vector3 startingPosition = grid.GetCellCenterWorld(new Vector3Int(startCell.x, startCell.y, 0));
        Vector3 endingPosition = grid.GetCellCenterWorld(new Vector3Int(endCell.x, endCell.y, 0));

        transform.position = startingPosition;

        float elapsedTime = 0;

        while (elapsedTime <= timeToMove && timeToMove > 0)
        {
            transform.position = Vector3.Lerp(startingPosition, endingPosition, elapsedTime / timeToMove);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endingPosition;

        currentMoveCoroutine = null;

        playEffectAtEnd(effectAtEndName, endingPosition);

        gameObject.SetActive(!disableAtEnd);
    }

    private void playEffectAtEnd(string effectAtEndName, Vector3 endingPosition)
    {
        if (effectAtEndName == null || !Enum.TryParse(effectAtEndName, ignoreCase: true, out EffectAnimationType effectType))
        {
            return;
        }

        EffectAnimationManager effect = EffectAnimationManager.instantiatePrefab();

        effect.transform.position = endingPosition;

        effect.setAnimations(effectType);
    }

}
