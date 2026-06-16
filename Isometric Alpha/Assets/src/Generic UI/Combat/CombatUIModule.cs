using UnityEngine;
using UnityEngine.Events;

public class CombatUIModule : MonoBehaviour
{

    public readonly static UnityEvent OnHideCombatUI = new UnityEvent();

    public void hideUI()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        OnHideCombatUI.AddListener(hideUI);
    }

    protected virtual void OnDestroy()
    {
        OnHideCombatUI.RemoveListener(hideUI);
    }
}
