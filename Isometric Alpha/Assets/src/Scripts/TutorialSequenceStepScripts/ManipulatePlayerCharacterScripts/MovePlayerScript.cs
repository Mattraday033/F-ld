using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerScript : TutorialSequenceStepScript
{

    protected IEnumerator movePlayer(TutorialSequenceStepScript facingScript, Vector3Int directionMod)
    {
        Vector3Int startingCell = PlayerMovement.getInstance().getCell();

        do
        {
            yield return null;
        }while(PlayerMovement.getInstance().isMoving());

        //the key that ended the step is often a movement key still held down, which walks the player on its own
        if(!PlayerMovement.getInstance().getCell().Equals(startingCell))
        {
            yield break;
        }

        if(facingScript != null)
        {
            facingScript.runScript();
        }

        PlayerMovement.getInstance().directionMod = directionMod;
        AreaManager.getMovementManager().moveAllSprites();
    }

}

public class MovePlayerNorthWestScript  : MovePlayerScript
{
    public override void runScript(GameObject target = null)
    {
        PlayerMovement.getInstance().StartCoroutine(movePlayer(new FaceNorthWestScript(), MovementManager.distance1TileNorthWestGrid));
    }
}

public class MovePlayerNorthEastScript  : MovePlayerScript
{
    public override void runScript(GameObject target = null)
    {
        PlayerMovement.getInstance().StartCoroutine(movePlayer(new FaceNorthEastScript(), MovementManager.distance1TileNorthEastGrid));
    }
}

public class MovePlayerSouthWestScript  : MovePlayerScript
{
    public override void runScript(GameObject target = null)
    {
        PlayerMovement.getInstance().StartCoroutine(movePlayer(new FaceSouthWestScript(), MovementManager.distance1TileSouthWestGrid));
    }
}

public class MovePlayerSouthEastScript  : MovePlayerScript
{
    public override void runScript(GameObject target = null)
    {
        PlayerMovement.getInstance().StartCoroutine(movePlayer(new FaceSouthEastScript(), MovementManager.distance1TileSouthEastGrid));
    }
}