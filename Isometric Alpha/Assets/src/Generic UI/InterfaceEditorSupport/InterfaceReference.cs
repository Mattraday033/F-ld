using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InterfaceReference<TInterface>
    where TInterface : class
{
    [SerializeField] Object m_Target;
    public TInterface Target => m_Target as TInterface;
}
