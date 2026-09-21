using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventList
{

    public static readonly UnityEvent<string, bool> SetActiveByNameChannel = new UnityEvent<string, bool>();


}
