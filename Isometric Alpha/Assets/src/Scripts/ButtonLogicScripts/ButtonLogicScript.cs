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

    public static void evaluateAllScriptsInLocation(int movementIndex)
    {
        if(movementIndex != Constants.indexZero)
        {
            return;
        }

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

    private PlayerInteractionScript script;

    public OpenGateButtonLogicScript(int scriptIndex, int requiredButtons, string gateKey, PlayerInteractionScript script = null)
    {
        this.scriptIndex = scriptIndex;
        this.requiredButtons = requiredButtons;
        this.gateKey = gateKey;

        this.script = script;
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

        if(script != null)
        {
            script.runScript();
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
        foreach (int gateIndex in gatesPerButton[lastPressedButtonIndex])
        {
            string fullKey = AreaManager.locationName + gateKey + gateIndex;
            bool activated = TrapAndButtonStateManager.contains(fullKey);

            TrapAndButtonStateManager.setKey(fullKey, !activated);
        }

        lastPressedButtonIndex = -1;
    }

    public override bool scriptConditionsMet()
    {
        OnButtonDataRequest.Invoke(this);

        return lastPressedButtonIndex >= 0;
    }

}

// public enum WallType {None = 0, RoundRubble = 1, SingleStalagmite = 2,  TripleStalagmite = 3, BushRock = 4}

public class ButtonOrderLogicScript : ButtonLogicScript
{

    private int numberOfButtonsPressed = 0;
    private int currentButtonIndex = -1;
    private List<MonsterSpawnDetails> monstersToSpawn;

    private List<ObstacleSpawnDetails> obstacleSpawnDetails;
    private List<Obstacle> obstacles;

    private Color tint = Color.white;

    private int[] indexOrder;
    private string secretDoorKey;

    public ButtonOrderLogicScript(int[] indexOrder, Vector3Int[] obstacleCoords, string[] spriteNames, string secretDoorKey, Color tint)
    {
        this.indexOrder = indexOrder;
        this.secretDoorKey = secretDoorKey;

        createSpawnDetails(obstacleCoords, spriteNames);

        this.tint = tint;
    }

    private void createSpawnDetails(Vector3Int[] obstacleCoords, string[] spriteNames)
    {
        obstacleSpawnDetails = new List<ObstacleSpawnDetails>();

        for(int index = 0; index < obstacleCoords.Length && index < spriteNames.Length; index++)
        {
            obstacleSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.obstacle, obstacleCoords[index], spriteNames[index], getColorBySpriteName(spriteNames[index])));
        }
    }

    private static Color getColorBySpriteName(string spriteName)
    {
        switch(spriteName)
        {
            case PrefabNames.tripleStalagmite:
                return new Color32(135,255,125,255);
            case PrefabNames.singleStalagmite:
                return new Color32(255,150,175,255);
            case PrefabNames.mediumBushStalagmite:
                return new Color32(235,135,255,255);
            case PrefabNames.lowStalagmite:
                return new Color32(255,245,100,255);
            default:
                return Color.white;
        }
    }

    public override void startingAction()
    {
        monstersToSpawn = MonsterSpawnDetailsList.getMonsterSpawnDetails();
        obstacles = new List<Obstacle>();

        foreach(ObstacleSpawnDetails spawnDetails in obstacleSpawnDetails)
        {
            GameObject gameObject = SpawnInfoManager.spawnInteractable(spawnDetails);
            Obstacle obstacle = gameObject.GetComponent<Obstacle>();

            if(obstacle.spriteRenderer.color.Equals(Color.white))
            {
                obstacle.spriteRenderer.color = tint;
            }

            obstacle.setToDown();

            SpawnInfoManager.addGameObject(gameObject);

            obstacles.Add(obstacle);
        }

        if(SpawnInfoManager.lastSaveBlueprint == null)
        {
            for(int index = 0; index < monstersToSpawn.Count; index++)
            {
                MonsterDefeatKeysList.setDefeatKey(MonsterDefeatKeysList.generateMonsterDefeatKey(index), true);
            }
        } else if(PuzzleFlags.currentPuzzleIndex > 0)
        {
            for(int index = 0; index < PuzzleFlags.currentPuzzleIndex; index++)
            {
                obstacles[index].setToUp();
            }
        }
    }

    public override void getFloorButtonStatus(FloorButton floorButton)
    {
        if(floorButton.isPressed())
        {
            numberOfButtonsPressed++;
            currentButtonIndex = floorButton.index;
        }
    }

    public override bool validButtonForScript(FloorButton floorButton)
    {
        return true;
    }

    public override void runScript()
    {
        SecretDoorFlags.addSecretDoorFlag(secretDoorKey);
    }

    private void resetAllObstacles()
    {
        foreach(Obstacle obstacle in obstacles)
        {
            obstacle.setToDown();
        }
    }

    public override bool scriptConditionsMet()
    {
        OnButtonDataRequest.Invoke(this);

        if(currentButtonIndex == -1 || SecretDoorFlags.secretDoorHasBeenDiscovered(secretDoorKey))
        {
            return false;
        }

        bool conditionsMet = false;

        if(numberOfButtonsPressed >= Constants.sizeTwo || currentButtonIndex != indexOrder[PuzzleFlags.currentPuzzleIndex])
        {
            PuzzleFlags.currentPuzzleIndex = 0;
            resetAllObstacles();
            spawnMonster();
        } else if(PuzzleFlags.currentPuzzleIndex >= obstacles.Count)
        {
            conditionsMet = true;
        } else if(PuzzleFlags.currentPuzzleIndex < indexOrder.Length && currentButtonIndex == indexOrder[PuzzleFlags.currentPuzzleIndex])
        {
            obstacles[PuzzleFlags.currentPuzzleIndex].setToUp();
            PuzzleFlags.currentPuzzleIndex++;
        } 

        currentButtonIndex = -1;
        numberOfButtonsPressed = 0;

        return conditionsMet;
    }

    private void spawnMonster()
    {
        for(int index = 0; index < monstersToSpawn.Count; index++)
        {
            if(!MonsterDefeatKeysList.monsterIsDefeated(MonsterDefeatKeysList.generateMonsterDefeatKey(index)))
            {
                continue;
            }

            MonsterDefeatKeysList.setDefeatKey(MonsterDefeatKeysList.generateMonsterDefeatKey(index), false);

            Transform monster = SpawnInfoManager.spawnMonster(monstersToSpawn[index], index);

            MovementManager.replaceMovementTracker(monster.GetComponent<EnemyMovement>());
        }
    }

}