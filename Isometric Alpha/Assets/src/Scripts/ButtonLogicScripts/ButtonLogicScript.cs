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

    public virtual bool validButtonForScript( FloorButton floorButton)
    {
        return floorButton.getKey().Equals(getKey()) && floorButton.isPressed();
    }

    public virtual void startingAction()
    {
        //Empty on purpose
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
        if (validButtonForScript(floorButton))
        {
            pressedCount += floorButton.weight;
        }
    }

    public override void runScript()
    {
        if (scriptIndex <= 0)
        {
            GateAndChestManager.addKey(AreaManager.locationName + gateKey);
        }
        else
        {
            GateAndChestManager.addKey(AreaManager.locationName + gateKey + scriptIndex);
        }
    }

    public override bool scriptConditionsMet()
    {
        OnButtonDataRequest.Invoke(this);

        if (pressedCount >= requiredButtons)
        {
            pressedCount = 0;
            return true;
        }
        else
        {
            pressedCount = 0;
            return false;
        }
    }

}

public class OnOffButtonLogicScript : ButtonLogicScript
{
    private string gateKey;

    private int lastPressedButtonIndex = -1;

    private Dictionary<int, int[]> gatesPerButton;
    private Dictionary<int, bool> released;
    private int[] gatesOpenAtStart;

    public OnOffButtonLogicScript(string gateKey, Dictionary<int, int[]> gatesPerButton, int[] gatesOpenAtStart)
    {
        this.gateKey = gateKey;
        this.gatesPerButton = gatesPerButton;
        this.gatesOpenAtStart = gatesOpenAtStart;

        released = new Dictionary<int, bool>();

        foreach (KeyValuePair<int, int[]> kvp in gatesPerButton)
        {
            released.Add(kvp.Key, true);
        }
    }

    public override void startingAction()
    {
        foreach (int gateIndex in gatesOpenAtStart)
        {
            string fullKey = AreaManager.locationName + gateKey + gateIndex;
            TrapAndButtonStateManager.setKey(fullKey, true);           
        }
    }

    public override void getFloorButtonStatus(FloorButton floorButton)
    {
        Debug.LogError("getFloorButtonStatus");

        Debug.LogError("validButtonForScript(" + floorButton.index + ") = " + validButtonForScript(floorButton));

        if (validButtonForScript(floorButton))
        {
            lastPressedButtonIndex = floorButton.index;
        }

        released[floorButton.index] = !floorButton.isPressed();
    }

    public override bool validButtonForScript(FloorButton floorButton)
    {
        return floorButton != null && gatesPerButton.ContainsKey(floorButton.index) && floorButton.isPressed() && released[floorButton.index];
    }

    public override void runScript()
    {
        Debug.LogError("runScript");

        foreach (int gateIndex in gatesPerButton[lastPressedButtonIndex])
        {
            Debug.LogError("gateIndex = " + gateIndex);

            string fullKey = AreaManager.locationName + gateKey + gateIndex;
            bool activated = TrapAndButtonStateManager.contains(fullKey);

            TrapAndButtonStateManager.setKey(fullKey, !activated);
        }

        lastPressedButtonIndex = -1;
    }

    public override bool scriptConditionsMet()
    {
        OnButtonDataRequest.Invoke(this);

        Debug.LogError("lastPressedButtonIndex = " + lastPressedButtonIndex);

        return lastPressedButtonIndex >= 0;
    }

}