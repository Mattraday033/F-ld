using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCNameTag : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    public void labelNPC(string name)
    {
        nameText.text = name;
    }
}
