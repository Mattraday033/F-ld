using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class CharacterCreationPopUpButton : PopUpButton
{
    public CharacterCreationPopUpButton() :
    base(PopUpType.CharacterCreation)
    {

    }

    public override void spawnPopUp()
    {
        Instantiate(Resources.Load<GameObject>(getPopUpPrefabName(type)), PopUpBlocker.getPopUpParent());

        setPopUpWindow(getCurrentPopUpGameObject().GetComponent<CharacterCreationPopUpWindow>());

        getPopUpWindow().setProgenitor(this);

        EscapeStack.addEscapableObject(getPopUpWindow());
    }

    public override GameObject getCurrentPopUpGameObject()
    {
        if (CharacterCreationPopUpWindow.getInstanceCC() != null && !(CharacterCreationPopUpWindow.getInstanceCC() is null))
        {
            return CharacterCreationPopUpWindow.getInstanceCC().gameObject;
        }
        else
        {
            return null;
        }
    }
}
