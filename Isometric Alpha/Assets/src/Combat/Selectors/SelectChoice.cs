/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectChoice : MonoBehaviour
{
    public Vector3[] allPositionCoords;

    public Vector3 currentPositionCoords;
    public int currentPosition;

    public bool moveKeyPressedDown = false;
    public bool returnKeyPressedDown = false;
    public bool madeASelection = false;
    public bool initialReturnButtonPress = true;

    public SelectorMovement Selector;

   

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = 0;
        currentPositionCoords = allPositionCoords[currentPosition];
        transform.position = currentPositionCoords;
    }

    void Update() //here for Key Input
    {
        if (Input.GetKey(KeyCode.W) && currentPosition > 0 && !moveKeyPressedDown && State.playerState == PlayerState.SelectingCombatAction)
        {
            moveKeyPressedDown = true;
            currentPosition--;
            currentPositionCoords = allPositionCoords[currentPosition];
            transform.position = currentPositionCoords;

        } else if (Input.GetKey(KeyCode.S) && currentPosition < (allPositionCoords.Length-1) && !moveKeyPressedDown && State.playerState == PlayerState.SelectingCombatAction)
        {
            moveKeyPressedDown = true;
            currentPosition++;
            currentPositionCoords = allPositionCoords[currentPosition];
            transform.position = currentPositionCoords;
        } else if (Input.GetKey(KeyCode.Return) && !initialReturnButtonPress && State.playerState == PlayerState.SelectingCombatAction && currentPosition == 0)
        {
            returnKeyPressedDown = true;
            madeASelection = true;
            State.currentActivity = initialMenuChoices(currentPosition);
            State.playerState = PlayerState.SelectingTarget;
            Selector.setMadeASelection(true);
            Selector.storeCurrentCoords();

        } 
        


        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            moveKeyPressedDown = false;
        } 
        if (!Input.GetKey(KeyCode.Return))
        {
            initialReturnButtonPress = false;
            returnKeyPressedDown = false;
        }


    }

    public void setInitialReturnButtonPress(bool input)
    {
        initialReturnButtonPress = input;
    }

    public CurrentActivity initialMenuChoices(int currentPosition)
    {
        if (currentPosition == 0)
        {
            return CurrentActivity.Attack;
        } else if (currentPosition == 1)
        {
            return CurrentActivity.Defend;
        } else if (currentPosition == 2)
        {
            return CurrentActivity.Item;
        } else if (currentPosition == 3)
        {
            return CurrentActivity.Maneuver;
        } else
        {
            return CurrentActivity.Waiting;
        }
    }

}*/
