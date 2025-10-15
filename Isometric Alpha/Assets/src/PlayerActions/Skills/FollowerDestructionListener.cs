using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerDestructionListener : MonoBehaviour
{

    private void OnEnable()
    {
        PartyMemberPlacer.DestroyAllFollowersEvent.AddListener(destroySelf);
    }

    private void OnDisable()
    {
        PartyMemberPlacer.DestroyAllFollowersEvent.RemoveListener(destroySelf);
    }

    private void destroySelf()
    {
        DestroyImmediate(gameObject);
    }

}
