using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Properties;

public interface ITutorialSequenceTarget
{
    public bool isUI();

    public string getTutorialHash();
    public GameObject getGameObject();
    public Transform getTransform();
    public RectTransform getRectTransform();
    public void assignToTutorialSequence(TutorialSequenceStep tutorialSequenceStep);

    public Vector2 getDimensions();

    public void createListeners();
    public void destroyListeners();



    public void highlight(bool skip);
    public void unhighlight(bool skip);
}

[Serializable]
public struct TutorialSequenceAdditionalScript
{
    public KeyCode keyCode;
    public TutorialSequenceStepScript additionalScript;

    public void runScript()
    {
        additionalScript.runScript();
    }

    public TutorialSequenceAdditionalScript(KeyCode keyCode, TutorialSequenceStepScript additionalScript)
    {
        this.keyCode = keyCode;
        this.additionalScript = additionalScript;
    }
}

[Serializable]
public struct TutorialSequenceStep : IDescribable 
{
    private const string pressToContinueMessagePrefix = "Press '";
    private const string pressToContinueMessageSuffix = "' to continue...";

    private const string buttonOnlyContinueMessage = "Click the button to continue...";
    private const string dragWeaponToContinueMessage = "Equip a Weapon to continue...";
    private const string dragActionToContinueMessage = "Equip an Action to continue...";

    public static bool hashFound = false;

    public string tutorialMessageKey;
    public bool createPopUpScreenBlocker;
    public bool addShiftToKeyCodeMessage;
    public bool skipHighlight;
    public bool skipUnhighlight;
    public bool dragWeaponContinueMessage;
    public bool dragActionContinueMessage;

    public bool blockInternalRaycastsOnCutOutMask;

    public string tutorialTargetHash;

    public TutorialSequenceStepScript scriptAtStart;
    public TutorialSequenceStepScript scriptAtEnd;

    //scripts that can be ran that affect the game that don't progress the tutorial
    //[SerializeField]
    public TutorialSequenceAdditionalScript[] additionalScripts;
    public static TutorialSequenceStepWindow currentTutorialMessageWindow;

    public ArrowDirection arrowDirection;

    public KeyCode[] nextStepKeyCode;

    public TutorialSequenceStep(string tutorialMessageKey, string tutorialTargetHash, ArrowDirection arrowDirection, KeyCode[] nextStepKeyCode)
    {
        this.tutorialMessageKey = tutorialMessageKey;
        this.tutorialTargetHash = tutorialTargetHash;
        this.arrowDirection = arrowDirection;
        this.nextStepKeyCode = nextStepKeyCode;

        this.scriptAtStart = null;
        this.scriptAtEnd = null;
        this.additionalScripts = new TutorialSequenceAdditionalScript[0];
        this.skipHighlight = false;
        this.skipUnhighlight = false;
        this.createPopUpScreenBlocker = false;
        this.addShiftToKeyCodeMessage = false;
        this.blockInternalRaycastsOnCutOutMask = false;
        this.dragWeaponContinueMessage = false;
        this.dragActionContinueMessage = false;
    }
    public TutorialSequenceStep(string tutorialMessageKey, string tutorialTargetHash, ArrowDirection arrowDirection, KeyCode[] nextStepKeyCode, bool skipHighlight, bool skipUnhighlight)
    {
        this.tutorialMessageKey = tutorialMessageKey;
        this.tutorialTargetHash = tutorialTargetHash;
        this.arrowDirection = arrowDirection;
        this.nextStepKeyCode = nextStepKeyCode;
        this.skipHighlight = skipHighlight;
        this.skipUnhighlight = skipUnhighlight;

        this.scriptAtStart = null;
        this.scriptAtEnd = null;
        this.additionalScripts = new TutorialSequenceAdditionalScript[0];
        this.createPopUpScreenBlocker = false;
        this.addShiftToKeyCodeMessage = false;
        this.blockInternalRaycastsOnCutOutMask = false;
        this.dragWeaponContinueMessage = false;
        this.dragActionContinueMessage = false;
    }
    public TutorialSequenceStep(string tutorialMessageKey, string tutorialTargetHash, ArrowDirection arrowDirection, KeyCode[] nextStepKeyCode, bool createPopUpScreenBlocker)
    {
        this.tutorialMessageKey = tutorialMessageKey;
        this.tutorialTargetHash = tutorialTargetHash;
        this.arrowDirection = arrowDirection;
        this.nextStepKeyCode = nextStepKeyCode;
        this.createPopUpScreenBlocker = createPopUpScreenBlocker;

        this.scriptAtStart = null;
        this.scriptAtEnd = null;
        this.additionalScripts = new TutorialSequenceAdditionalScript[0];
        this.skipHighlight = false;
        this.skipUnhighlight = false;
        this.addShiftToKeyCodeMessage = false;
        this.blockInternalRaycastsOnCutOutMask = false;
        this.dragWeaponContinueMessage = false;
        this.dragActionContinueMessage = false;
    }

    public TutorialSequenceStep(string tutorialMessageKey, string tutorialTargetHash, ArrowDirection arrowDirection, KeyCode[] nextStepKeyCode, bool skipHighlight, bool skipUnhighlight, bool createPopUpScreenBlocker)
    {
        this.tutorialMessageKey = tutorialMessageKey;
        this.tutorialTargetHash = tutorialTargetHash;
        this.arrowDirection = arrowDirection;
        this.nextStepKeyCode = nextStepKeyCode;
        this.skipHighlight = skipHighlight;
        this.skipUnhighlight = skipUnhighlight;
        this.createPopUpScreenBlocker = createPopUpScreenBlocker;

        this.scriptAtStart = null;
        this.scriptAtEnd = null;
        this.additionalScripts = new TutorialSequenceAdditionalScript[0];
        this.addShiftToKeyCodeMessage = false;
        this.blockInternalRaycastsOnCutOutMask = false;
        this.dragWeaponContinueMessage = false;
        this.dragActionContinueMessage = false;
    }
    public TutorialSequenceStep(string tutorialMessageKey, string tutorialTargetHash, TutorialSequenceAdditionalScript[] additionalScripts, ArrowDirection arrowDirection, KeyCode[] nextStepKeyCode)
    {
        this.tutorialMessageKey = tutorialMessageKey;
        this.tutorialTargetHash = tutorialTargetHash;
        this.additionalScripts = additionalScripts;
        this.arrowDirection = arrowDirection;
        this.nextStepKeyCode = nextStepKeyCode;

        this.scriptAtStart = null;
        this.scriptAtEnd = null;
        this.skipHighlight = false;
        this.skipUnhighlight = false;
        this.createPopUpScreenBlocker = false;
        this.addShiftToKeyCodeMessage = false;
        this.blockInternalRaycastsOnCutOutMask = false;
        this.dragWeaponContinueMessage = false;
        this.dragActionContinueMessage = false;
    }

    public TutorialSequenceStep(string tutorialMessageKey, string tutorialTargetHash, TutorialSequenceStepScript scriptAtStart, TutorialSequenceStepScript scriptAtEnd, ArrowDirection arrowDirection, KeyCode[] nextStepKeyCode)
    {
        this.tutorialMessageKey = tutorialMessageKey;
        this.tutorialTargetHash = tutorialTargetHash;
        this.scriptAtStart = scriptAtStart;
        this.scriptAtEnd = scriptAtEnd;
        this.arrowDirection = arrowDirection;
        this.nextStepKeyCode = nextStepKeyCode;

        this.additionalScripts = new TutorialSequenceAdditionalScript[0];
        this.skipHighlight = false;
        this.skipUnhighlight = false;
        this.createPopUpScreenBlocker = false;
        this.addShiftToKeyCodeMessage = false;
        this.blockInternalRaycastsOnCutOutMask = false;
        this.dragWeaponContinueMessage = false;
        this.dragActionContinueMessage = false;
    }

    public TutorialSequenceStep(string tutorialMessageKey, string tutorialTargetHash, TutorialSequenceStepScript scriptAtStart, TutorialSequenceStepScript scriptAtEnd, ArrowDirection arrowDirection, KeyCode[] nextStepKeyCode, bool createPopUpScreenBlocker)
    {
        this.tutorialMessageKey = tutorialMessageKey;
        this.tutorialTargetHash = tutorialTargetHash;
        this.scriptAtStart = scriptAtStart;
        this.scriptAtEnd = scriptAtEnd;
        this.arrowDirection = arrowDirection;
        this.nextStepKeyCode = nextStepKeyCode;
        this.createPopUpScreenBlocker = createPopUpScreenBlocker;

        this.additionalScripts = new TutorialSequenceAdditionalScript[0];
        this.skipHighlight = false;
        this.skipUnhighlight = false;
        this.addShiftToKeyCodeMessage = false;
        this.blockInternalRaycastsOnCutOutMask = false;
        this.dragWeaponContinueMessage = false;
        this.dragActionContinueMessage = false;
    }
    public TutorialSequenceStep(string tutorialMessageKey, string tutorialTargetHash, TutorialSequenceStepScript scriptAtStart, TutorialSequenceStepScript scriptAtEnd, ArrowDirection arrowDirection, KeyCode[] nextStepKeyCode, bool skipHighlight, bool skipUnhighlight, bool createPopUpScreenBlocker)
    {
        this.tutorialMessageKey = tutorialMessageKey;
        this.tutorialTargetHash = tutorialTargetHash;
        this.scriptAtStart = scriptAtStart;
        this.scriptAtEnd = scriptAtEnd;
        this.arrowDirection = arrowDirection;
        this.nextStepKeyCode = nextStepKeyCode;
        this.skipHighlight = skipHighlight;
        this.skipUnhighlight = skipUnhighlight;
        this.createPopUpScreenBlocker = createPopUpScreenBlocker;

        this.additionalScripts = new TutorialSequenceAdditionalScript[0];
        this.addShiftToKeyCodeMessage = false;
        this.blockInternalRaycastsOnCutOutMask = false;
        this.dragWeaponContinueMessage = false;
        this.dragActionContinueMessage = false;
    }

    public TutorialSequenceStep(string tutorialMessageKey, string tutorialTargetHash, TutorialSequenceStepScript scriptAtStart, TutorialSequenceStepScript scriptAtEnd, TutorialSequenceAdditionalScript[] additionalScripts, ArrowDirection arrowDirection, KeyCode[] nextStepKeyCode, bool createPopUpScreenBlocker)
    {
        this.tutorialMessageKey = tutorialMessageKey;
        this.tutorialTargetHash = tutorialTargetHash;
        this.scriptAtStart = scriptAtStart;
        this.scriptAtEnd = scriptAtEnd;
        this.additionalScripts = additionalScripts;
        this.arrowDirection = arrowDirection;
        this.nextStepKeyCode = nextStepKeyCode;
        this.createPopUpScreenBlocker = createPopUpScreenBlocker;

        this.skipHighlight = false;
        this.skipUnhighlight = false;
        this.addShiftToKeyCodeMessage = false;
        this.blockInternalRaycastsOnCutOutMask = false;
        this.dragWeaponContinueMessage = false;
        this.dragActionContinueMessage = false;
    }

    public TutorialSequenceStep(string tutorialMessageKey, string tutorialTargetHash, TutorialSequenceStepScript scriptAtStart, TutorialSequenceStepScript scriptAtEnd, TutorialSequenceAdditionalScript[] additionalScripts, ArrowDirection arrowDirection, KeyCode[] nextStepKeyCode)
    {
        this.tutorialMessageKey = tutorialMessageKey;
        this.tutorialTargetHash = tutorialTargetHash;
        this.scriptAtStart = scriptAtStart;
        this.scriptAtEnd = scriptAtEnd;
        this.additionalScripts = additionalScripts;
        this.arrowDirection = arrowDirection;
        this.nextStepKeyCode = nextStepKeyCode;

        this.skipHighlight = false;
        this.skipUnhighlight = false;
        this.createPopUpScreenBlocker = false;
        this.addShiftToKeyCodeMessage = false;
        this.blockInternalRaycastsOnCutOutMask = false;
        this.dragWeaponContinueMessage = false;
        this.dragActionContinueMessage = false;
    }

    public TutorialSequenceStep(string tutorialMessageKey, string tutorialTargetHash, TutorialSequenceStepScript scriptAtStart, TutorialSequenceStepScript scriptAtEnd, TutorialSequenceAdditionalScript[] additionalScripts, ArrowDirection arrowDirection, KeyCode[] nextStepKeyCode, bool skipHighlight, bool skipUnhighlight)
    {
        this.tutorialMessageKey = tutorialMessageKey;
        this.tutorialTargetHash = tutorialTargetHash;
        this.scriptAtStart = scriptAtStart;
        this.scriptAtEnd = scriptAtEnd;
        this.additionalScripts = additionalScripts;
        this.arrowDirection = arrowDirection;
        this.nextStepKeyCode = nextStepKeyCode;
        this.skipHighlight = skipHighlight;
        this.skipUnhighlight = skipUnhighlight;

        this.createPopUpScreenBlocker = false;
        this.addShiftToKeyCodeMessage = false;
        this.blockInternalRaycastsOnCutOutMask = false;
        this.dragWeaponContinueMessage = false;
        this.dragActionContinueMessage = false;
    }
    public TutorialSequenceStep(string tutorialMessageKey, string tutorialTargetHash, TutorialSequenceStepScript scriptAtStart, TutorialSequenceStepScript scriptAtEnd, TutorialSequenceAdditionalScript[] additionalScripts, ArrowDirection arrowDirection, KeyCode[] nextStepKeyCode, bool skipHighlight, bool skipUnhighlight, bool createPopUpScreenBlocker)
    {
        this.tutorialMessageKey = tutorialMessageKey;
        this.tutorialTargetHash = tutorialTargetHash;
        this.scriptAtStart = scriptAtStart;
        this.scriptAtEnd = scriptAtEnd;
        this.additionalScripts = additionalScripts;
        this.arrowDirection = arrowDirection;
        this.nextStepKeyCode = nextStepKeyCode;
        this.skipHighlight = skipHighlight;
        this.skipUnhighlight = skipUnhighlight;
        this.createPopUpScreenBlocker = createPopUpScreenBlocker;

        this.addShiftToKeyCodeMessage = false;
        this.blockInternalRaycastsOnCutOutMask = false;
        this.dragWeaponContinueMessage = false;
        this.dragActionContinueMessage = false;
    }

	public bool Equals(TutorialSequenceStep otherStep)
	{
        if(!tutorialMessageKey.Equals(otherStep.tutorialMessageKey))
        {
            return false;
        }

        if(!tutorialTargetHash.Equals(otherStep.tutorialTargetHash))
        {
            return false;
        }

        return true;
	}

    public bool nextStepKeyCodePressed()
    {
        foreach (KeyCode keyCode in nextStepKeyCode)
        {
            if (Input.GetKey(keyCode))
            {
                return true;
            }
        }

        return false;
    }

    public void activateStartingScript(ITutorialSequenceTarget tutorialTarget)
    {
        TutorialSequence.fromButton = false;

        if (scriptAtStart != null)
        {
            scriptAtStart.runScript(tutorialTarget.getGameObject());
        }
    }

    public void activateEndingScript()
    {
        if (TutorialSequence.fromButton)
        {
            TutorialSequence.fromButton = false;
            return;
        }

        if (scriptAtEnd != null && TutorialSequenceStepWindow.getInstance() != null)
        {
            scriptAtEnd.runScript(TutorialSequenceStepWindow.getInstance().tutorialSequenceTarget.getGameObject());
        }
        else if (TutorialSequenceStepTargetButton.currentButton != null)
        {
            TutorialSequenceStepTargetButton.currentButton.onClick.Invoke();
        }
    }

    public bool additionalScriptButtonPressed()
    {
        foreach (TutorialSequenceAdditionalScript additionalScript in additionalScripts)
        {
            if (Input.GetKey(additionalScript.keyCode))
            {
                return true;
            }
        }

        return false;
    }

    public void runAdditionalScripts()
    {
        foreach (TutorialSequenceAdditionalScript additionalScript in additionalScripts)
        {
            if (Input.GetKey(additionalScript.keyCode))
            {
                additionalScript.runScript();
                return;
            }
        }
    }

    private void handlePopUpScreenBlocker()
    {
        if (createPopUpScreenBlocker)
        {
            PopUpBlocker.spawnPopUpScreenBlocker();
        }
        else if (EscapeStack.getEscapableObjectsCount() <= 0)
        {
            PopUpBlocker.destroyPopUpScreenBlocker();
        }
    }

    private PanelType getPanelType(bool ultraWide)
    {
        if (ultraWide)
        {
            return PanelType.TutorialUITargetUltraWide;
        }

        return PanelType.TutorialUITarget;
    }

    public void createMessageWindowAndRunScript(string tutorialHash)
    {
        createMessageWindowAndRunScript(TutorialSequenceStepTargetObject.getLatestTutorialTarget(tutorialHash), false, false);
    }

    public void createMessageWindowAndRunScript(ITutorialSequenceTarget tutorialTarget)
    {
        createMessageWindowAndRunScript(tutorialTarget, false, false);
    }

    public void createMessageWindowAndRunScript(string tutorialHash, bool useUltraWideTutorialWindow, bool disableArrow)
    {
        createMessageWindowAndRunScript(TutorialSequenceStepTargetObject.getLatestTutorialTarget(tutorialHash), useUltraWideTutorialWindow, disableArrow);
    }

    public void createMessageWindowAndRunScript(ITutorialSequenceTarget tutorialTarget, bool useUltraWideTutorialWindow, bool disableArrow) //, ArrowDirection directionToObject 
    {
        RectTransform tutorialTargetTransform = tutorialTarget.getRectTransform();

        handlePopUpScreenBlocker();

        activateStartingScript(tutorialTarget);

        tutorialTarget.highlight(skipHighlight);

        // Debug.LogError("Tutorial window parent = " + tutorialTarget.getTransform().name);

        currentTutorialMessageWindow = GameObject.Instantiate(getDescriptionPanelFull(getPanelType(useUltraWideTutorialWindow)), tutorialTarget.getTransform()).GetComponent<TutorialSequenceStepWindow>();

        if (!tutorialTarget.isUI())
        {
            if (CombatStateManager.inCombat)
            {
                currentTutorialMessageWindow.transform.localScale = new Vector3(0.007f, 0.007f, 1f);
            }
            else
            {
                currentTutorialMessageWindow.transform.localScale = new Vector3(0.009f, 0.01f, 1f);
            }

            Canvas.ForceUpdateCanvases();
            OOCUIManager.disableAllOOCUIButtons();
        }
        else
        {
            OOCUIManager.enableAllOOCUIButtons();
        }

        currentTutorialMessageWindow.setTutorialSequenceStepAndTarget(this, tutorialTarget, disableArrow);

        describeSelfFull(currentTutorialMessageWindow);

        //Helpers.updateGameObjectPosition(currentTutorialMessageWindow.gameObject.transform);

        currentTutorialMessageWindow.setWorldPositionForDescriptionPanel(tutorialTargetTransform, arrowDirection);
    }

    public bool isTutorialTarget(string hash)
    {
        if (tutorialTargetHash.Equals(""))
        {
            Debug.LogError("Unassigned Tutorial Hash");
        }

        if (hashFound)
        {
            return false;
        }

        if (tutorialTargetHash.Equals(hash))
        {
            hashFound = true;
        }

        return tutorialTargetHash.Equals(hash);
    }

    //IDescribable Methods
    public string getName()
    {
        return tutorialMessageKey;
    }

    public bool ineligible()
    {
        return false;
    }

    public GameObject getRowType(RowType rowType)
    {
        return null;
    }

    public GameObject getDescriptionPanelFull()
    {
        return getDescriptionPanelFull(PanelType.Standard);
    }

    public GameObject getDescriptionPanelFull(PanelType panelType)

    {
        if (panelType == PanelType.TutorialUITargetUltraWide)
        {
            return DescriptionPanel.getDescriptionPanel(PrefabNames.tutorialSequencePopUpDescriptionPanelUltraWide);
        }

        return DescriptionPanel.getDescriptionPanel(PrefabNames.tutorialSequencePopUpDescriptionPanel);
    }

    public GameObject getDecisionPanel()
    {
        return null;
    }

    public bool withinFilter(string[] filterParameters)
    {
        return true;
    }

    public void describeSelfFull(DescriptionPanel panel)
    {
        panel.setObjectBeingDescribed(this);

        DescriptionPanel.setText(panel.useDescriptionText, TutorialMessageList.getTutorialMessage(tutorialMessageKey));

        if (dragWeaponContinueMessage)
        {
            DescriptionPanel.setText(panel.inputText, dragWeaponToContinueMessage);
        } else if (dragActionContinueMessage)
        {
            DescriptionPanel.setText(panel.inputText, dragActionToContinueMessage);
        } else if (nextStepKeyCode.Length == 0)
        {
            DescriptionPanel.setText(panel.inputText, buttonOnlyContinueMessage);
        }
        else if (addShiftToKeyCodeMessage)
        {
            DescriptionPanel.setText(panel.inputText, pressToContinueMessagePrefix + "Shift' + '" + getNextStepKeyCodesAsString() + pressToContinueMessageSuffix);
        }
        else
        {
            DescriptionPanel.setText(panel.inputText, pressToContinueMessagePrefix + getNextStepKeyCodesAsString() + pressToContinueMessageSuffix);
        }
    }

    private string getNextStepKeyCodesAsString()
    {
        if (nextStepKeyCode.Length == 0)
        {
            return "";
        }

        string keyCodes = nextStepKeyCode[0].ToString();

        return keyCodes.Replace("Left", "").Replace("Right", "").Replace("Alpha", "");
    }

    public void describeSelfRow(DescriptionPanel panel)
    {

    }

    public void setUpDecisionPanel(IDecisionPanel descisionPanel)
    {

    }

    public ArrayList getRelatedDescribables()
    {
        return new ArrayList();
    }

	public bool buildableWithBlocks()
    {
        return false;
    }

    public bool buildableWithBlocksRows()
    {
        return false;
    }

    public static bool allStepConditionalsMet(TutorialSequenceStep tutorialSequenceStep)
    {
        CombatAction loadedCombatAction;
        Selector currentSelector;
        AbilityMenuManager currentAbilityManager;

        switch (tutorialSequenceStep.tutorialMessageKey)
        {
            case TutorialMessageList.selectingAbilityFromWheelKey:

                AbilityMenuManager abilityMenuManager = AbilityMenuManager.getInstance();

                return abilityMenuManager.getCurrentlySelectedAbilityMenuButton().casterCanPayActionCost();
            case TutorialMessageList.selectingAllyKey:

                Collider2D collider = Helpers.getCollision(SelectorManager.currentSelector.getCollider());

                if (collider == null)
                {
                    return false;
                }


                if (Helpers.getCollision(SelectorManager.currentSelector.getCollider()).gameObject.tag.Equals(LayerAndTagManager.playerTag) &&
                    Input.GetKey(KeyBindingList.combatAcceptChoiceKey) && !SelectorManager.getInstance().isMoving)
                {
                    return true;
                }

                if (Helpers.getCollision(SelectorManager.currentSelector.getCollider()).gameObject.tag.Equals(LayerAndTagManager.npcTag) &&
                    Input.GetKey(KeyBindingList.combatAcceptChoiceKey) && !SelectorManager.getInstance().isMoving)
                {
                    return true;
                }

                return false;
            case TutorialMessageList.selectingTargetKey:

                currentSelector = SelectorManager.currentSelector;

                if (CombatStateManager.findingEmptySpaceForReposition())
                {
                    loadedCombatAction = RepositionManager.currentSingleTargetRepositionCombatAction;
                }
                else
                {
                    loadedCombatAction = AbilityMenuManager.getInstance().getCurrentlySelectedLoadedCombatAction();
                }

                if (loadedCombatAction.movesTarget() && currentSelector.targetsImmobileTarget())
                {
                    return false;
                }

                if (loadedCombatAction.targetsOnlyEmptySpace())
                {
                    if (!currentSelector.hasAtLeastOneTarget(SelectorManager.allyAndEnemyTagCriteria))
                    {
                        return true;
                    }
                }
                else
                {
                    if (loadedCombatAction.targetMustBeDead())
                    {
                        if (currentSelector.hasAtLeastOneTarget(SelectorManager.enemyTagCriteria))
                        {
                            if (CombatGrid.enemyHasMandatoryTarget() && !currentSelector.hasAtLeastOneMandatoryTarget() && !loadedCombatAction.isSelfTargeting())
                            {
                                return false;
                            }

                            return true;
                        }
                    }
                    else
                    {
                        if (currentSelector.hasAtLeastOneLivingTarget(SelectorManager.enemyTagCriteria))
                        {
                            if (CombatGrid.enemyHasMandatoryTarget() && !currentSelector.hasAtLeastOneMandatoryTarget() && !loadedCombatAction.isSelfTargeting())
                            {
                                return false;
                            }

                            return true;
                        }
                    }
                }

                return false;
            case TutorialMessageList.repositionStepKey:

                currentSelector = SelectorManager.currentSelector;
                loadedCombatAction = AbilityMenuManager.getInstance().getCurrentlySelectedLoadedCombatAction();

                if (loadedCombatAction.tertiaryCoordsRequiresEmptySpace() && CombatGrid.getCombatantAtCoords(currentSelector.getCoords()) != null)
                {
                    return false;
                }

                if (loadedCombatAction.tertiaryCoordsRequiresEmptySpace())
                {
                    if (!currentSelector.hasAtLeastOneTarget(SelectorManager.allyAndEnemyTagCriteria))
                    {
                        return true;
                    }
                }
                else
                {
                    if (currentSelector.hasAtLeastOneTarget(SelectorManager.allyAndEnemyTagCriteria))
                    {
                        return true;
                    }
                }

                return false;
        }

        return true;
    }
}

[Serializable]
public class TutorialSequence 
{
    private const bool bypassTutorialSequenceCheck = true;

    public UnityEvent endOfSequenceEvent;

    public static bool fromButton;
    public static TutorialSequence currentTutorialSequence;
    public static UnityEvent OnEnableButtons = new UnityEvent();
    public static UnityEvent<TutorialSequenceStep> TutorialSequenceTargetFinder = new UnityEvent<TutorialSequenceStep>();
    public static UnityEvent DestroyAllTutorialMessageWindows = new UnityEvent();

    public static List<TutorialSequence> tutorialSequenceQueue = new List<TutorialSequence>();

    public OOCActivity activityToReturnTo = OOCActivity.walking;
    public CurrentActivity combatActivityToReturnTo = CurrentActivity.ChoosingActor;

    public SkipTutorialScript skipScript;

    public bool skipCurrentActivityChange;

    private bool started;
    private int currentStepIndex = 0;
    private int previousStep = -1;
    public string tutorialSeenFlagName;

    public static bool endingSequence = false;

    //[SerializeField]
    public TutorialSequenceStep[] tutorialSequenceSteps;

    public TutorialSequence(OOCActivity activityToReturnTo, bool skipCurrentActivityChange, string tutorialSeenFlag, TutorialSequenceStep[] tutorialSequenceSteps)
    {
        this.tutorialSeenFlagName = tutorialSeenFlag;
        this.skipCurrentActivityChange = skipCurrentActivityChange;
        this.activityToReturnTo = activityToReturnTo;
        this.tutorialSequenceSteps = tutorialSequenceSteps;
        previousStep = -1;
    }

    public TutorialSequence(CurrentActivity combatActivityToReturnTo, bool skipCurrentActivityChange, string tutorialSeenFlag, List<TutorialSequenceStep> tutorialSequenceSteps)
    {
        this.tutorialSeenFlagName = tutorialSeenFlag;
        this.skipCurrentActivityChange = skipCurrentActivityChange;
        this.combatActivityToReturnTo = combatActivityToReturnTo;
        this.tutorialSequenceSteps = tutorialSequenceSteps.ToArray();
        previousStep = -1;
    }

    public void setSkipScript(SkipTutorialScript skipScript)
    {
        this.skipScript = skipScript;
    }

    public void skipTutorial()
    {
        if(skipScript != null)
        {
            skipScript.runScript();
        }
    }

    public bool isSkippable()
    {
        return skipScript != null;
    }

    public void startSequence()
    {

        setStateToInTutorialSequence();

        started = true;
        previousStep = -1;
        currentStepIndex = 0;

        if (endOfSequenceEvent != null)
        {
            endOfSequenceEvent.AddListener(endSequence);
        }

        spawnTutorialPopUp();
    }

    public bool hasBeenSeen()
    {
        return started || Flags.getFlag(tutorialSeenFlagName);
    }

    public void moveToNextStep()
    {
        moveToNextStep(false);
    }

    public void moveToNextStep(bool fromButton)
    {
        if (TutorialSequence.fromButton || previousStep == currentStepIndex ||
            !TutorialSequenceStep.allStepConditionalsMet(getCurrentTutorialSequenceStep()))
        {
            return;
        }

        TutorialSequenceStep.hashFound = false;
        TutorialSequence.fromButton = fromButton;

        // Debug.LogError("1");

        if (tutorialSequenceSteps.Length > currentStepIndex + 1)
        {
            // Debug.LogError("2");
            previousStep = currentStepIndex;

            TutorialSequence sequenceSnapShot = currentTutorialSequence;

            destroyTutorialPopUp();

            if(sequenceSnapShot != currentTutorialSequence)
            {
                return; //Ending Script changed the tutorial sequence
            }

            currentStepIndex++;

            if (CombatStateManager.inCombat && CombatStateManager.whoseTurn == WhoseTurn.Resolving)
            {
                return;
            }

            spawnTutorialPopUp();
        }
        else
        {
            endSequence();
        }
    }

    public void endSequence()
    {

        if (endingSequence)
        {
            return;
        }
        else
        {
            endingSequence = true;

            if (endOfSequenceEvent != null)
            {
                endOfSequenceEvent.RemoveListener(endSequence);
            }

            DialogueManager.stopTutorials();

            destroyTutorialPopUp(!currentTutorialSequence.inFinalStep());

            if (EscapeStack.getEscapableObjectsCount() <= 0)
            {
                PopUpBlocker.destroyPopUpScreenBlocker();
            }

            OOCUIManager.enableAllOOCUIButtons();

            Flags.setFlag(tutorialSeenFlagName, true);

            currentTutorialSequence = null;
            TutorialSequenceStep.hashFound = false;

            RevealManager.OnReveal.Invoke();

            if (tutorialSequenceQueue.Count > 0)
            {
                TutorialSequence nextSequence = tutorialSequenceQueue[0];
                tutorialSequenceQueue.RemoveAt(0);
                NotificationManager.skipNextNotificationSpawn();
                startTutorialSequence(nextSequence);
            }
            else
            {
                returnToActivity();
            }

            endingSequence = false;
            OnEnableButtons.Invoke();
        }
    }

    public static void endCurrentTutorialSequence()
    {
        if (!currentlyInTutorialSequence())
        {
            return;
        }

        currentTutorialSequence.endSequence();
    }

    private void returnToActivity()
    {
        if (CombatStateManager.inCombat)
        {
            CombatStateManager.setCurrentActivity(combatActivityToReturnTo);
        }
        else
        {
            PlayerOOCStateManager.setCurrentActivity(activityToReturnTo, bypassTutorialSequenceCheck);
        }
    }

    public void spawnTutorialPopUp()
    {
        TutorialSequenceStep currentStep = getCurrentTutorialSequenceStep();

        // Debug.LogError("invoking TutorialSequenceTargetFinder");
        TutorialSequenceTargetFinder.Invoke(currentStep);

        if (inFinalStep())
        {
            TutorialSequenceStepTargetRow.ChangeButtonInteractivity.Invoke(true);
        }
    }

    public static void spawnCurrentTutorialPopUp()
    {
        currentTutorialSequence.spawnTutorialPopUp();
    }

    public bool inFinalStep()
    {
        return currentStepIndex >= currentTutorialSequence.tutorialSequenceSteps.Length - 1;
    }

    private void destroyTutorialPopUp()
    {
        destroyTutorialPopUp(false);
    }

    private void destroyTutorialPopUp(bool skipEndingScript)
    {
        TutorialSequenceStep tutorialSequenceStep = getCurrentTutorialSequenceStep();

        if (!skipEndingScript)
        {
            tutorialSequenceStep.activateEndingScript();

            // Helpers.debugNullCheck("tutorialSequenceStep",tutorialSequenceStep);
            // Helpers.debugNullCheck("currentTutorialSequence",currentTutorialSequence);

            if (currentTutorialSequence == null || !tutorialSequenceStep.Equals(currentTutorialSequence.getCurrentTutorialSequenceStep()))
            {
                return; //Ending Script changed the tutorial sequence
            }
        }

        DestroyAllTutorialMessageWindows.Invoke();
    }

    private bool nextStepKeyCodeIsPressed()
    {
        TutorialSequenceStep currentStep = getCurrentTutorialSequenceStep();

        return currentStep.nextStepKeyCodePressed();
    }

    public TutorialSequenceStep getCurrentTutorialSequenceStep()
    {
        return tutorialSequenceSteps[currentStepIndex];
    }

    public static Transform getUIParent()
    {
        return PlayerMovement.getUIParentTransform();
    }

    public static bool startTutorialSequence(GameObject tutorialColliderGameObject)
    {
        if (tutorialColliderGameObject == null)
        {
            return false;
        }

        currentTutorialSequence = tutorialColliderGameObject.GetComponent<TutorialTriggerCollider>().getTutorialSequence();

        tutorialColliderGameObject.SetActive(false);

        if(shouldSkipTutorialSequence(currentTutorialSequence))
        {
            currentTutorialSequence = null;
            return false;
        }

        if (!currentTutorialSequence.hasBeenSeen())
        {
            currentTutorialSequence.startSequence();
            return true;
        }
        else
        {
            currentTutorialSequence = null;
            return false;
        }
    }

    public static bool startTutorialSequence(string tutorialSequenceKey)
    {
        if (tutorialSequenceKey == null)
        {
            return false;
        }

        currentTutorialSequence = TutorialSequenceList.getTutorialSequence(tutorialSequenceKey);



        if (currentTutorialSequence == null)
        {
            return false;
        }
        else if (currentTutorialSequence.hasBeenSeen())
        {
            currentTutorialSequence = null;
            return false;
        }
        else
        {
            currentTutorialSequence.startSequence();
            return true;
        }
    }

    public static bool startTutorialSequence(TutorialSequence tutorialSequence)
    {
        if (currentlyInTutorialSequence())
        {
            tutorialSequenceQueue.Add(tutorialSequence);
            return false;
        }

        currentTutorialSequence = tutorialSequence;

        if (currentTutorialSequence.hasBeenSeen())
        {
            currentTutorialSequence = null;
            return false;
        }
        else
        {
            currentTutorialSequence.startSequence();
            return true;
        }
    }

    public static bool currentlyInTutorialSequence()
    {
        return currentTutorialSequence != null;
    }

   public static bool canDestroyTutorialMessageWindows()
    {
        if(PlayerOOCStateManager.currentActivity != OOCActivity.inTutorialSequence 
            || !currentlyInTutorialSequence())
        {
            return true;
        }

        return !currentTutorialSequence.getCurrentTutorialSequenceStep().createPopUpScreenBlocker;
    }

    public static bool shouldAdvanceCurrentTutorialSequence()
    {
        return currentTutorialSequence.nextStepKeyCodeIsPressed();
    }

    public static void advanceCurrentTutorialSequence(bool fromButton)
    {
        currentTutorialSequence.moveToNextStep(fromButton);
    }

    public static void advanceCurrentTutorialSequence()
    {
        currentTutorialSequence.moveToNextStep();
    }

    public static bool additionalScriptButtonPressed()
    {
        return currentTutorialSequence.getCurrentTutorialSequenceStep().additionalScriptButtonPressed();
    }

    public static void runAdditionalScripts()
    {
        currentTutorialSequence.getCurrentTutorialSequenceStep().runAdditionalScripts();
    }

    private static void setStateToInTutorialSequence()
    {
        if (CombatStateManager.inCombat)
        {
            CombatStateManager.setCurrentActivity(CurrentActivity.Tutorial);
        }
        else
        {
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.inTutorialSequence);
        }
    }

    public static bool shouldSkipTutorialSequence(TutorialSequence tutorialSequence)
    {
        if(tutorialSequence == null)
        {
            return true;
        }

        string tutorialSeenFlagName = tutorialSequence.tutorialSeenFlagName;

        switch (tutorialSeenFlagName)
        {
            case TutorialSequenceList.firstHostitilityTutorialSeenFlag:
            case TutorialSequenceList.intimidateTutorialSeenFlag:
            case TutorialSequenceList.cunningTutorialSeenFlag:
            case TutorialSequenceList.secondCunningTutorialSeenFlag:
            case TutorialSequenceList.observationTutorialSeenFlag:
            case TutorialSequenceList.leadershipTutorialSeenFlag:
            case TutorialSequenceList.interactableObjectTutorialSeenFlag:
                return Flags.getFlag(TutorialSequenceList.skipThatchShackTutorialsFlag);
            case TutorialSequenceList.hiddenObjectsTutorialSeenFlag:

                if (Flags.getFlag(FlagNameList.gotLeavesForBalint) || State.terrainHidden)
                {
                    Flags.setFlag(TutorialSequenceList.hiddenObjectsTutorialSeenFlag, true);
                }

                if (!Flags.getFlag(FlagNameList.givenTaskByBalint))
                {
                    return true;
                }

                return Flags.getFlag(TutorialSequenceList.hiddenObjectsTutorialSeenFlag);
        }

        return false;
    }
}
