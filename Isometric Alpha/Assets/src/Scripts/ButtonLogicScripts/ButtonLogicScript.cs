using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ButtonLogicScript
{

    public static UnityEvent<ButtonLogicScript> OnButtonDataRequest;

    protected int scriptIndex;

    public string getKey()
    {
        return AreaManager.locationName + scriptIndex;
    }

    public abstract void runScript();

    public abstract bool scriptConditionsMet();

    public abstract void getFloorButtonStatus(FloorButton floorButton);

    public static bool validButtonForScript(ButtonLogicScript script, FloorButton floorButton)
    {
        return floorButton.getKey().Equals(script.getKey()) && floorButton.isPressed();
    }

    public static void evaluateScript(ButtonLogicScript script)
    {
        if (script.scriptConditionsMet())
        {
            script.runScript();
        }
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeUnityEvent()
    {
        OnButtonDataRequest = new UnityEvent<ButtonLogicScript>();

        MovementManager.OnMoveFinished.AddListener(evaluateAllScriptsInLocation);        
    }

    public static void evaluateAllScriptsInLocation()
    {
        List<ButtonLogicScript> scriptList = ButtonScriptList.getButtonScripts(AreaManager.locationName);

        foreach(ButtonLogicScript script in scriptList)
        {
            evaluateScript(script);
        }
    }

}

public class OpenGateButtonLogicScript : ButtonLogicScript
{

    private int requiredButtons;
    private int pressedCount = 0;
    private string gateKey;

    public OpenGateButtonLogicScript(int scriptIndex, int requiredButtons, string gateKey)
    {
        this.scriptIndex = scriptIndex;
        this.requiredButtons = requiredButtons;
        this.gateKey = gateKey;        
    }

    public override void getFloorButtonStatus(FloorButton floorButton)
    {

        if(validButtonForScript(this, floorButton))
        {
            pressedCount++;
        }
    }

    public override void runScript()
    {
        GateAndChestManager.addKey(gateKey);
    }

    public override bool scriptConditionsMet()
    {
        OnButtonDataRequest.Invoke(this);

        if(pressedCount >= requiredButtons)
        {
            pressedCount = 0;
            return true;
        } else
        {
            pressedCount = 0;
            return false;            
        }        
    }

}