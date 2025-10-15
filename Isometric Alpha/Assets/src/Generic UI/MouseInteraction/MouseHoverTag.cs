using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseHoverTag : MonoBehaviour
{

    public TextMeshProUGUI tagContents;

    public void fillOutTag(string text)
    {
        tagContents.text = text;
    }

}
