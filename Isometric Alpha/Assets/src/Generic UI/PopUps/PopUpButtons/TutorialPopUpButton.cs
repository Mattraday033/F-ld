using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TutorialPopUpButton : PopUpButton
{
    private CurrentActivity oldActivity;
    public bool spawnOnStart;
    public string tutorialKey;

    public const string tutorialPopUpButtonGameObjectKey = "TutorialPopUpButton";
    public const string combatTutorialKey = "Combat Tutorial";
    public const string itemTutorialKey = "Item Tutorial";
    public const string abilityWheelTutorialKey = "Ability Wheel Tutorial";
    public const string hostilityTutorialKey = "Hostility Tutorial";
    public const string questTutorialKey = "Quest Tutorial";
    public const string movableObjectsTutorialKey = "Movable Objects Tutorial"; 


    void Start()
    {
        // if(spawnOnStart || combatTutorialShouldSpawnAtStart())  
        // {
        //     StartCoroutine(spawnPopUpInTwoFramesFromNow());
        // }
    }

    private IEnumerator spawnPopUpInTwoFramesFromNow()
    {
        yield return null;

        spawnPopUp(tutorialKey);
    }

    private IEnumerator spawnWhenWalking()
    {
        while (PlayerOOCStateManager.currentActivity != OOCActivity.walking)
        {
            yield return null;
        }

        spawnPopUp(tutorialKey);
    }

    public TutorialPopUpButton() :
    base(PopUpType.Tutorial)
    {

    }

    public static TutorialPopUpButton instantiateButton(Transform parent)
    {
        return Instantiate(Resources.Load<GameObject>(TutorialPopUpButton.tutorialPopUpButtonGameObjectKey), parent).GetComponent<TutorialPopUpButton>();
    }

    public void spawnPopUp(string newTutorialKey)
    {
        tutorialKey = newTutorialKey;

        spawnPopUp();
    }

    public override void spawnPopUp()
    {
        if (tutorialKey == null || tutorialKey.Equals(""))
        {
            return;
        }

        if (TutorialPopUpWindow.getInstance() == null && TutorialPopUpWindow.getInstance() is null)
        {
            base.spawnPopUp();
        }
        else
        {
            setPopUpWindow(TutorialPopUpWindow.getInstance());
        }

        TutorialPopUpWindow tutorialPopUpWindow = (TutorialPopUpWindow) getPopUpWindow();
        
        tutorialPopUpWindow.addTutorialPages(tutorialKey);

        tutorialPopUpWindow.populate();

        if (CombatStateManager.inCombat)
        {
            oldActivity = CombatStateManager.currentActivity;
            CombatStateManager.setCurrentActivity(CurrentActivity.Tutorial);
            Flags.setFlag("seenCombatTutorial", true);
        }
        else
        {
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.inTutorialPopUp);
        }
    }

    public override void destroyPopUp()
    {
        base.destroyPopUp();

        if (CombatStateManager.inCombat)
        {
            CombatStateManager.setCurrentActivity(oldActivity);
        }
    }

    public override GameObject getCurrentPopUpGameObject()
    {
        if (TutorialPopUpWindow.getInstance() != null && !(TutorialPopUpWindow.getInstance() is null))
        {
            return TutorialPopUpWindow.getInstance().gameObject;
        }
        else
        {
            return null;
        }
    }
}
