using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class OOCUIManager : MonoBehaviour, IQuestListSource, ICounter
{
    private static OOCUIManager instance;

    public HostilityBarManager hostilityBarManager;

    public GameObject oocUIParent;

    public SkillButtonManager skillButtonManager;
    public TextMeshProUGUI skillChargeCountText;
    public GameObject skillSwapArrowParent;
    public SlotIconHover skillIconHover;

    public Image leftFootImage;
    public Image rightFootImage;

    public Button[] allOOCUIButtons;

    public QuestCounter questCounter;
    public CharacterLevelCounter characterLevelCounter;

    [RuntimeInitializeOnLoadMethod]
    private static void initializeOOCUIManager()
    {
        instance = null;
        PlayerOOCStateManager.OnStateChangeToWalking.AddListener(updateOOCUI);
    }

    public static void updateCharacterLevelUpCounter()
    {
        if (instance != null && instance.characterLevelCounter != null)
        {
            instance.characterLevelCounter.setCounter();
        }
    }

    public static void updateQuestCounter()
    {
        if(instance == null || instance.questCounter == null)
        {
            return;
        }

        if (instance.getNumberOfQuests() <= 0)
        {
            instance.questCounter.gameObject.SetActive(false);
        }
        else
        {
            instance.questCounter.setQuestListSource(instance);
        }
    }

    public static void updateOOCUI()
    {
        if(instance == null)
        {
            return;
        }

        getInstance().updateUI();
    }

    public void updateUI()
    {
        if (Flags.isInNewGameMode() || CombatStateManager.inCombat || gameObject == null)
        {
            disableOOCUI();
            return;
        }

        updateSkillUI();

        updateFooting();
        hostilityBarManager.setUpHostilityBars();        
        
        updateQuestCounter();

        updateCharacterLevelUpCounter();
    }

    private void updateSkillUI()
    {
        if(skillSwapArrowParent == null)
        {
            return;
        }

        skillIconHover.iconImage.sprite = Helpers.loadSpriteFromResources(State.currentSkillType.ToString());

        switch(State.currentSkillType)
        {
            case SkillType.Cunning:
                skillChargeCountText.text = CunningManager.getCunningsRemaining() + "/" + PartyStats.getMaxCunningCount();
                break;
            case SkillType.Observation:
                skillChargeCountText.text = "";
                break;
            case SkillType.Leadership:
                skillChargeCountText.text = PartyMemberPlacer.getPlacedPartyMemberCount() + "/" + PartyStats.getMaxPlacablePartyMembers();
                break;
            default:
                skillChargeCountText.text = IntimidateManager.getIntimidatesRemaining() + "/" + PartyStats.getMaxIntimidateCount();
                break;
        }

        skillIconHover.hoverMessageKey = State.currentSkillType.ToString();
        skillIconHover.setHoverMessage(HoverMessageList.getMessage(State.currentSkillType.ToString()));

        skillButtonManager.setSkillButtonInteractability();
    }

    public void disableOOCUI()
    {
        if(oocUIParent == null || oocUIParent is null)
        {
            return;
        }

        State.oocUIManager = this;
        oocUIParent.SetActive(false);
    }

    public void enableOOCUI()
    {
        if(oocUIParent == null || oocUIParent is null || PlayerOOCStateManager.currentActivity == OOCActivity.Defeat)
        {
            return;
        }

        State.oocUIManager = this;
        oocUIParent.SetActive(true);
    }

    public void testAddXP()
    {
        PartyManager.addXP(AllyStats.xpNeededToLevelUp);
    }

    public void updateFooting()
    {


        if (State.onLeftFoot)
        {
            leftFootImage.color = Color.white;
            rightFootImage.color = Color.clear;
        }
        else
        {
            leftFootImage.color = Color.clear;
            rightFootImage.color = Color.white;
        }
    }

    public static OOCUIManager getInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            throw new IOException("There is already an instance of OOCUIManager");
        }

        instance = this;
        State.oocUIManager = this;
    }

    public static void disableAllOOCUIButtons()
    {
        if (getInstance() == null)
        {
            return;
        }

        foreach (Button button in getInstance().allOOCUIButtons)
        {
            button.enabled = false;
        }
    }

    public static void enableAllOOCUIButtons()
    {
        if (getInstance() == null)
        {
            return;
        }

        foreach (Button button in getInstance().allOOCUIButtons)
        {
            button.enabled = true;
        }
    }

    //IQuestListSource
    public string getListKey()
    {
        return AreaManager.locationName;
    }

    public bool highlightOnHover()
    {
        return false;
    }

    public int getNumberOfQuests()
    {
        return getListOfQuestStepsForDisplay().Count;
    }

    public List<QuestStep> getListOfQuestStepsForDisplay()
    {
        IMapObject location = MapObjectList.getMapObject(getListKey());

        return location.getAllQuestStepsInLocation();
    }

    //ICounter methods

    private void OnEnable()
    {
        updateCounter();
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
        updateUI();
    }

    public List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        listOfEvents.Add(CombatActionArray.OnCombatActionArrayChange);
        listOfEvents.Add(Stats.OnStatsChange);
        listOfEvents.Add(AllyStats.OnPartyMemberUpgraded);
        listOfEvents.Add(SkillManager.OnSkillUse);
        listOfEvents.Add(AreaManager.OnAreaSpawn);
        listOfEvents.Add(Formation.OnFormationChange);

        return listOfEvents;
    }
}
