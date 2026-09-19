using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExtraSpawnBehaviour
{
    public void addBehaviour(GameObject gameObject);

    // static abstract string getString();
}

public delegate void OnSpawnBehaviour(List<object> args);

public abstract class SpawnableComponent : MonoBehaviour
{
    // public static abstract KeyValuePair<List<object>, OnSpawnBehaviour> getSpawnBehaviour(List<object> args);
}

// public class AnimationManagerSpawnBehaviour : IExtraSpawnBehaviour
// {
//     public AnimationManagerSpawnBehaviour()
//     {
//         this.index = index;
//         this.script = script;
//         this.secretDoorFlag = secretDoorFlag;
//         this.type = type;
//         // this.chestName = chestName;
//     }

//     public void addBehaviour(GameObject gameObject)
//     {
//         Container chest = gameObject.AddComponent<Container>();

//         chest.populate(index, type);

//         chest.script = script;

//         chest.setSecretDoorFlag(secretDoorFlag);
//     }
// }

public class ContainerSpawnBehaviour: IExtraSpawnBehaviour
{
    private int index;
    private QuestStepActivationScript script;
    private string secretDoorFlag;
    private ChestType type;
    // private string chestName;

    public ContainerSpawnBehaviour(int index,
                                    QuestStepActivationScript script = null,
                                    string secretDoorFlag = null,
                                    ChestType type = ChestType.Chest,
                                    string chestName = null)
    {
        this.index = index;
        this.script = script;
        this.secretDoorFlag = secretDoorFlag;
        this.type = type;
        // this.chestName = chestName;
    }

    public void addBehaviour(GameObject gameObject)
    {
        Container chest = gameObject.AddComponent<Container>();

        chest.populate(index, type);

        chest.script = script;

        chest.setSecretDoorFlag(secretDoorFlag);
    }
}