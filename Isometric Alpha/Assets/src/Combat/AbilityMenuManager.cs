using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;

public interface IHandlesAbilityWheelSelectionInput
{
	public int getCurrentlySelectedAbilityIndex();
	public CombatAction getCurrentlySelectedAbility();
	public void moveSelectedButtonClockwise();
	public void moveSelectedButtonCounterClockwise();
	public void chooseAbility(int abilityIndex);
}

public interface IActionArrayStorage 
{
	public int getMaximumNumberOfCombatActions();
	public CombatActionArray getStoredCombatActionArray();
	public CombatAction getAbilityAt(int abilityIndex);
	public void setAbilityAtIndex(CombatAction combatAction, int abilityIndex);
	public void removeAbility(int abilityIndex);
	public bool canAddActionToArray(CombatAction combatAction);
}

[Serializable]
public class AbilityMenuManager : MonoBehaviour, IHandlesAbilityWheelSelectionInput, IActionArrayStorage, ICounter
{
    public const int NorthCircleIndex = 0;
    public const int NorthEastCircleIndex = 1;
    public const int EastCircleIndex = 2;
    public const int SouthEastCircleIndex = 3;
    public const int SouthCircleIndex = 4;
    public const int SouthWestCircleIndex = 5;
    public const int WestCircleIndex = 6;
    public const int NorthWestCircleIndex = 7;

    public const int playerMaxCombatActionCount = 8;
    public const int partyMemberMaxCombatActionCount = 5;

    public static UnityEvent OnAbilityWheelUpdate = new UnityEvent();

    public DescriptionPanel parentDescriptionPanel;
    public Stats actionArraySource;

    private int currentlySelectedAbilityIndex = 0;

    //[SerializeField]
    public bool displayOnly;
    public GameObject abilityButtonCanvas;
    public AbilityMenuButton[] abilityButtons;

    private bool noButtonsSelectable = false;

    public GameObject itemQuantityPanel;
    public TextMeshProUGUI itemQuantityText;

    public DescriptionPanelSlot descriptionPanelSlot;

    private static AbilityMenuManager instance;

    public static AbilityMenuManager getInstance()
    {
        return instance;
    }

    public virtual void Awake()
    {

        foreach (AbilityMenuButton button in abilityButtons)
        {
            button.setAbilityMenuManager(this);
        }

        if (displayOnly)
        {
            updateAbilityButtonImages();
            OnAbilityWheelUpdate.AddListener(updateAbilityButtonImages);
        }

        if (CombatStateManager.inCombat)
        {
            descriptionPanelSlot = CurrentActionHoverPanelManager.getInstance();
        }
    }

    private void OnDisable()
    {
        if (displayOnly)
        {
            OnAbilityWheelUpdate.RemoveListener(updateAbilityButtonImages);
        }
    }

    void Update() //here for Key Input
    {
        if (displayOnly)
        {
            return;
        }

        KeyPressManager.updateKeyBools();

        if (KeyPressManager.handlingPrimaryKeyPress)
        {
            return;
        }

        if (CombatStateManager.inCombat)
        {
            if (CombatStateManager.currentActivity == CurrentActivity.Retreating)
            {
                return;
            }
            else if (CombatStateManager.currentActivity == CurrentActivity.ChoosingAbility)
            {
                if (Input.anyKeyDown)
                {
                    if (KeyBindingList.eitherBackoutKeyIsPressed())
                    {
                        abilityButtons[currentlySelectedAbilityIndex].disableCombatActionSelectorPreview();

                        return;
                    }

                    if (noButtonsSelectable)
                    {
                        return;
                    }

                    if (Input.GetKey(KeyCode.Alpha1))
                    {
                        chooseAbility(NorthCircleIndex);
                        KeyPressManager.handlingPrimaryKeyPress = true;
                        return;
                    }

                    if (Input.GetKey(KeyCode.Alpha2))
                    {
                        chooseAbility(NorthEastCircleIndex);
                        KeyPressManager.handlingPrimaryKeyPress = true;
                        return;
                    }

                    if (Input.GetKey(KeyCode.Alpha3))
                    {
                        chooseAbility(EastCircleIndex);
                        KeyPressManager.handlingPrimaryKeyPress = true;
                        return;
                    }

                    if (Input.GetKey(KeyCode.Alpha4))
                    {
                        chooseAbility(SouthEastCircleIndex);
                        KeyPressManager.handlingPrimaryKeyPress = true;
                        return;
                    }

                    if (Input.GetKey(KeyCode.Alpha5))
                    {
                        chooseAbility(SouthCircleIndex);
                        KeyPressManager.handlingPrimaryKeyPress = true;
                        return;
                    }

                    if (Input.GetKey(KeyCode.Alpha6))
                    {
                        chooseAbility(SouthWestCircleIndex);
                        KeyPressManager.handlingPrimaryKeyPress = true;
                        return;
                    }

                    if (Input.GetKey(KeyCode.Alpha7))
                    {
                        chooseAbility(WestCircleIndex);
                        KeyPressManager.handlingPrimaryKeyPress = true;
                        return;
                    }

                    if (Input.GetKey(KeyCode.Alpha8))
                    {
                        chooseAbility(NorthWestCircleIndex);
                        KeyPressManager.handlingPrimaryKeyPress = true;
                        return;
                    }

                    if (Input.GetKey(KeyBindingList.moveCounterClockwiseKey))
                    {
                        moveSelectedButtonCounterClockwise();
                        KeyPressManager.handlingPrimaryKeyPress = true;
                        return;
                    }

                    if (Input.GetKey(KeyBindingList.moveClockwiseKey))
                    {
                        moveSelectedButtonClockwise();
                        KeyPressManager.handlingPrimaryKeyPress = true;
                        return;
                    }

                    if (KeyBindingList.continueUIKeyIsPressed())
                    {
                        selectAction();

                        KeyPressManager.handlingPrimaryKeyPress = true;

                        return;
                    }
                }
            }
            else if (CombatStateManager.currentActivity != CurrentActivity.Tutorial)
            {
                disableAbilityButtonCanvas();
            }
        }
    }

    public void selectAction()
    {
        if (!abilityButtons[currentlySelectedAbilityIndex].casterCanPayActionCost())
        {
            return;
        }

        abilityButtons[currentlySelectedAbilityIndex].disableCombatActionSelectorPreview();
        abilityButtons[currentlySelectedAbilityIndex].enableCombatActionSelector();

        CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingLocation);
    }

    public AbilityMenuButton getCurrentlySelectedAbilityMenuButton()
    {
        return abilityButtons[currentlySelectedAbilityIndex];
    }

    public void incrementCurrentlySelectedAbilityIndex()
    {
        if (currentlySelectedAbilityIndex == (abilityButtons.Length - 1))
        {
            setCurrentlySelectedAbilityIndex(0);
        }
        else
        {
            setCurrentlySelectedAbilityIndex(currentlySelectedAbilityIndex + 1);
        }
    }

    public void decrementCurrentlySelectedAbilityIndex()
    {
        if (currentlySelectedAbilityIndex == 0)
        {
            setCurrentlySelectedAbilityIndex(abilityButtons.Length - 1);
        }
        else
        {
            setCurrentlySelectedAbilityIndex(currentlySelectedAbilityIndex - 1);
        }
    }

    public void setCurrentlySelectedAbilityIndex(int newCurrentlySelectedAbilityIndex)
    {
        if (CombatStateManager.inCombat)
        {
            CurrentActionHoverPanelManager.removeCurrentPrimaryDescribable();
            getCurrentlySelectedAbilityMenuButton().disableCombatActionSelectorPreview();
        }

        int previousIndex = currentlySelectedAbilityIndex;
        this.currentlySelectedAbilityIndex = newCurrentlySelectedAbilityIndex;

        abilityButtons[previousIndex].updateCooldownCostText();
        getCurrentlySelectedAbilityMenuButton().updateCooldownCostText();

        if (CombatStateManager.inCombat && getCurrentlySelectedAbility() != null && !getCurrentlySelectedAbility().unactivatable() &&
            !noButtonsSelectable)
        {
            getCurrentlySelectedAbilityMenuButton().enableCombatActionSelectorPreview();
            CurrentActionHoverPanelManager.addPrimaryDescriptionPanel(getCurrentlySelectedAbility());
        }
    }

    public void enableAbilityButtonCanvas()
    {
        instance = this;
        updateAbilityButtonImages();

        if (abilityButtonCanvas != null)
        {
            abilityButtonCanvas.SetActive(true);
        }

        enabled = true;

        checkForAbilitiesOnCooldown();

        if (!CombatStateManager.isPlayerSurpriseRound())
        {
            greyOutAllAmbushAbilities();
        }

        checkForItemCombatActionsWithoutItems();

        selectFirstAvailableButton();

        if (CombatStateManager.inCombat && !noButtonsSelectable)
        {
            CurrentActionHoverPanelManager.removeCurrentPrimaryDescribable();

            getCurrentlySelectedAbilityMenuButton().enableCombatActionSelectorPreview();
            CurrentActionHoverPanelManager.addPrimaryDescriptionPanel(getCurrentlySelectedAbility());
        }
    }

    public void disableAbilityButtonCanvas()
    {

        // Debug.LogError("disabling Ability Button Canvas");

        if (abilityButtonCanvas != null)
        {
            abilityButtonCanvas.SetActive(false);
        }

        enabled = false;

        if (CombatStateManager.inCombat)
        {
            if (CombatStateManager.currentActivity != CurrentActivity.ChoosingAbility &&
                CombatStateManager.currentActivity != CurrentActivity.ChoosingLocation &&
                CombatStateManager.currentActivity != CurrentActivity.ChoosingTertiary)
            {
                CurrentActionHoverPanelManager.removeCurrentPrimaryDescribable();
            }

            getCurrentlySelectedAbilityMenuButton().disableCombatActionSelectorPreview();
        }
    }

    public void deselectAllAbilityButtons()
    {
        foreach (AbilityMenuButton abilityButton in abilityButtons)
        {
            abilityButton.deselectButton();
        }
    }

    public void updateItemQuantityPanel()
    {
        if (itemQuantityPanel == null)
        {
            return;
        }

        if (abilityButtons[currentlySelectedAbilityIndex].loadedCombatAction != null &&
            abilityButtons[currentlySelectedAbilityIndex].loadedCombatAction.needsItemQuantityPanel())
        {
            Item sourceItem = abilityButtons[currentlySelectedAbilityIndex].loadedCombatAction.getSourceItem();
            itemQuantityPanel.SetActive(true);
            itemQuantityText.text = "x" + Inventory.getItem(sourceItem.getKey()).getQuantity();
        }
        else
        {
            itemQuantityPanel.SetActive(false);
            itemQuantityText.text = "";
        }
    }

    public void disableAllAbilityButtons()
    {
        foreach (AbilityMenuButton abilityButton in abilityButtons)
        {
            abilityButton.disable();
        }
    }

    public void updateAbilityButtonImages()
    {
        disableAllAbilityButtons();

        populateAbilityMenuFromCombatActionArray(getActionArraySource().getActionArray().getActions());
    }

    public Stats getActionArraySource()
    {
        if (actionArraySource == null)
        {
            if (parentDescriptionPanel != null)
            {
                actionArraySource = Stats.convertIDescribableToStats(parentDescriptionPanel.getObjectBeingDescribed());
            }

            if (actionArraySource == null)
            {
                actionArraySource = OverallUIManager.getCurrentPartyMember();
                Debug.LogError("actionArraySource.getName() = " + actionArraySource.getName());
            }
        }

        return actionArraySource;
    }

    public void setToDisplayOnly()
    {
        displayOnly = true;
    }

    public CombatAction getCurrentlySelectedLoadedCombatAction()
    {
        return abilityButtons[currentlySelectedAbilityIndex].loadedCombatAction;
    }

    public void populateAbilityMenuFromCombatActionArray()
    {
        populateAbilityMenuFromCombatActionArray(getStoredCombatActionArray().getActions());
    }

    public void populateAbilityMenuFromCombatActionArray(CombatActionArray actions)
    {
        populateAbilityMenuFromCombatActionArray(actions.getActions());
    }

    public virtual void populateAbilityMenuFromCombatActionArray(CombatAction[] actions)
    {
        bool hasAtLeastOneActivatableAction = false;

        for (int actionIndex = 0; actionIndex < CombatActionArray.numberOfActivatablePlayerCombatActions; actionIndex++)
        {
            if (actions[actionIndex] != null && !actions[actionIndex].unactivatable())
            {
                hasAtLeastOneActivatableAction = true;
                break;
            }
        }

        if (!hasAtLeastOneActivatableAction)
        {
            actions[0] = new FistAttack();
        }

        int index = 0;
        foreach (CombatAction action in actions)
        {
            if (index >= abilityButtons.Length)
            {
                return;
            }

            if (action == null)
            {
                abilityButtons[index].disable();
                index++;
                continue;
            }

            action.onAddToAbilityMenu();

            abilityButtons[index].loadCombatAction(action);
            abilityButtons[index].enable();
            abilityButtons[index].enable(Helpers.loadSpriteFromResources(action.getIconName()), Color.clear, action);

            index++;
        }
    }

    public void selectFirstAvailableButton()
    {
        deselectAllAbilityButtons();
        noButtonsSelectable = false;

        int currentIndex = 0;
        foreach (AbilityMenuButton abilityButton in abilityButtons)
        {
            if (abilityButton.isSelectable())
            {
                chooseAbility(currentIndex);
                return;
            }
            else
            {
                currentIndex++;
            }
        }

        //no ability buttons are selectable
        noButtonsSelectable = true;
    }

    public void greyOutAllAmbushAbilities()
    {
        foreach (AbilityMenuButton button in abilityButtons)
        {
            if (button.loadedCombatAction != null && button.enabled && button.loadedCombatAction.getOnlyUsableDuringSurpriseRound())
            {
                button.greyOutAbility();
            }
        }
    }

    public void checkForAbilitiesOnCooldown()
    {
        foreach (AbilityMenuButton button in abilityButtons)
        {
            if (button.loadedCombatAction != null && button.loadedCombatAction.getCooldownRemaining() > 0)
            {
                button.greyOutAbility();
                button.updateCooldownCostText();
            }
            else if (button.loadedCombatAction != null && button.loadedCombatAction.getCooldownRemaining() <= 0)
            {
                button.resetGreyOutStatus();
                button.updateCooldownCostText();
            }
        }
    }

    public void checkForItemCombatActionsWithoutItems()
    {
        foreach (AbilityMenuButton button in abilityButtons)
        {
            if (button.loadedCombatAction != null && button.enabled && !button.loadedCombatAction.usableWithoutItemsInInventory())
            {
                if (button.loadedCombatAction.getSourceItem().getQuantity() == 0)
                {
                    button.greyOutAbility();
                }
            }
        }
    }

    private CombatAction getEquippedCombatActionInCurrentlySelectedSlot()
    {
        return getStoredCombatActionArray().getActionInSlot(getCurrentlySelectedAbilityIndex());
    }

    public bool canPlaceWeaponHere()
    {
        if (!getActionArraySource().hasAvailableWeaponSlots())
        {
            //if there are no available weapon slots BUT the hovered over ability IS a weapon, 
            //allow an accept button press. Otherwise, don't
            if (getEquippedCombatActionInCurrentlySelectedSlot() != null &&
                getEquippedCombatActionInCurrentlySelectedSlot().takesAWeaponSlot())
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        else
        {
            return true;
        }
    }

    //IHandlesAbilityWheelSelectionInput methods
    public int getCurrentlySelectedAbilityIndex()
    {
        return currentlySelectedAbilityIndex;
    }

    public CombatAction getCurrentlySelectedAbility()
    {
        return abilityButtons[currentlySelectedAbilityIndex].loadedCombatAction;
    }

    public void moveSelectedButtonClockwise()
    {
        int incrementCounter = 0;

        do
        {
            incrementCurrentlySelectedAbilityIndex();
            incrementCounter++;

            if (incrementCounter > abilityButtons.Length)
            {
                Debug.LogError("All AbilityMenuButton's are disabled");
                return;
            }

        } while (currentlySelectedSlotIsInvalid());

        deselectAllAbilityButtons();
        abilityButtons[currentlySelectedAbilityIndex].selectButton();

        if (!displayOnly)
        {
            updateItemQuantityPanel();
        }
    }

    public void moveSelectedButtonCounterClockwise()
    {
        int decrementCounter = 0;

        do
        {
            decrementCurrentlySelectedAbilityIndex();
            decrementCounter++;

            if (decrementCounter > abilityButtons.Length)
            {
                Debug.LogError("All AbilityMenuButton's are disabled");
                return;
            }

        } while (currentlySelectedSlotIsInvalid());

        deselectAllAbilityButtons();
        abilityButtons[currentlySelectedAbilityIndex].selectButton();

        if (!displayOnly)
        {
            updateItemQuantityPanel();
        }
    }

    private bool currentlySelectedSlotIsInvalid()
    {
        return (!displayOnly && (!abilityButtons[currentlySelectedAbilityIndex].enabled ||
                    abilityButtons[currentlySelectedAbilityIndex].loadedCombatAction == null ||
                    abilityButtons[currentlySelectedAbilityIndex].loadedCombatAction.unactivatable()))

                    || (displayOnly && !canPlaceWeaponHere());
    }

    public void chooseAbility(int abilityIndex)
    {
        if (!abilityButtons[abilityIndex].enabled ||
            abilityButtons[abilityIndex].loadedCombatAction == null ||
            abilityButtons[abilityIndex].loadedCombatAction.unactivatable())
        {
            return;
        }

        deselectAllAbilityButtons();
        setCurrentlySelectedAbilityIndex(abilityIndex);
        abilityButtons[currentlySelectedAbilityIndex].selectButton();
        updateItemQuantityPanel();
    }

    //IActionArrayStorage methods

    public CombatActionArray getStoredCombatActionArray()
    {
        return getActionArraySource().getActionArray();
    }

    public int getMaximumNumberOfCombatActions()
    {
        return abilityButtons.Length;
    }

    public void setAbilityAtIndex(CombatAction combatAction, int abilityIndex)
    {
        setAbilityAtIndex(combatAction, abilityIndex, true);
    }

    public void setAbilityAtIndex(CombatAction combatAction, int abilityIndex, bool selectButton)
    {
        if (combatAction == null)
        {
            abilityButtons[abilityIndex].disable();
            return;
        }

        abilityButtons[abilityIndex].enable(Helpers.loadSpriteFromResources(combatAction.getIconName()), Color.clear, combatAction);

        if (selectButton)
        {
            abilityButtons[abilityIndex].selectButton();
        }
    }

    public CombatAction getAbilityAt(int index)
    {
        return abilityButtons[index].loadedCombatAction;
    }

    public void removeAbility(int abilityIndex)
    {
        setAbilityAtIndex(null, abilityIndex);
    }

    public bool canAddActionToArray(CombatAction combatAction)
    {
        if (combatAction == null)
        {
            return true;
        }

        if (combatAction.takesAWeaponSlot() && !getActionArraySource().hasAvailableWeaponSlots())
        {
            return false;
        }

        return combatAction.hasAvailableSlots(this);
    }

    //ICounter
    private void OnEnable()
    {
        addListeners();
    }

    private void OnDestroy()
    {
        removeListeners();
    }

    public void addListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.AddListener(updateCounter);
        }
    }
    public void removeListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach(UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.RemoveListener(updateCounter);
        }
    }

    public void updateCounter()
    {
        populateAbilityMenuFromCombatActionArray(getStoredCombatActionArray().getActions());
    }

    public List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        listOfEvents.Add(CombatActionArray.OnCombatActionArrayChange);

        return listOfEvents;
    }
}
