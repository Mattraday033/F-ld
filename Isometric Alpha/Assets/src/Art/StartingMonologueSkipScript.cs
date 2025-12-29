using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingMonologueSkipScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(KeyBindingList.continueUIKeyIsPressed()) 
        {
            SceneChange.changeSceneToStartMenu();
        }
    }
}
