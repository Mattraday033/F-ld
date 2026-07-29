using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCNameTag : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    public RectTransform textTransform;
    public HorizontalOrVerticalLayoutGroup layoutGroup;

    public void labelNPC(string name)
    {
        nameText.text = name;
    }

    public void orientTransformMiddle()
    {
        layoutGroup.enabled = false;

        textTransform.anchoredPosition = new Vector2(.5f, .5f);
        textTransform.anchorMin = new Vector2(.5f, .5f);
        textTransform.anchorMax = new Vector2(.5f, .5f);
        textTransform.pivot = new Vector2(.5f, .5f);

        updatePosition();
    }

    public void orientTransformSide()
    {
        layoutGroup.enabled = true;

        // textTransform.anchoredPosition = new Vector2(1f, .5f);
        // textTransform.anchorMin = new Vector2(1f, .5f);
        // textTransform.anchorMax = new Vector2(1f, .5f);
        // textTransform.pivot = new Vector2(1f, .5f);

        updatePosition();
    }

    private void updatePosition()
    {
        textTransform.localPosition = Vector3.zero;
        Helpers.updateGameObjectPosition(textTransform);
    }
}
