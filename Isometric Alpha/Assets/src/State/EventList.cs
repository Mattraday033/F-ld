using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventList
{

    public static readonly UnityEvent DestroyAllSpawnedGameObjects = new UnityEvent();

    public static readonly UnityEvent<ActivationCategory, string, bool> SetActiveByNameChannel = new UnityEvent<ActivationCategory, string, bool>();

    public static readonly UnityEvent<ActivationCategory, int, bool> SetActiveByIndexChannel = new UnityEvent<ActivationCategory, int, bool>();

}
