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
    public Image[] hostilityBars;

    private static OOCUIManager instance;

    public Image topOOCUIBackground;
    public Image bottomOOCUIBackground;
    public GameObject oocUIParent;

    public TextMeshProUGUI intimidateCountText;
    public TextMeshProUGUI cunningCountText;
    public TextMeshProUGUI leadershipCountText;

    public GameObject intimidateParent;
    public Image intimidateCountImage;

    public GameObject cunningParent;
    public Image cunningCountImage;

    private static bool manuallySetObservationPanelColor = false;
    public GameObject observationParent;
    public Image observationToggleImage;

    public GameObject leadershipParent;
    public Image leadershipCountImage;

    public Image leftFootImage;
    public Image rightFootImage;

    public Button[] allOOCUIButtons;

    public QuestCounter questCounter;
    public PartyMemberUpgradeCounter partyMemberUpgradeCounter;
    public CharacterLevelCounter characterLevelCounter;

    void Start()
    {
        State.oocUIManager = this;


        updateOOCUI();
        updateFooting();
        updateCounters();
    }

    public static void updateCounters()
    {

        if(Flags.isInNewGameMode())
        {
            return;
        }

        updateQuestCounter();

        updatePartyMemberUpgradeCounter();
        updateCharacterLevelUpCounter();
    }

    public static void updateCharacterLevelUpCounter()
    {
        if (instance != null && instance.characterLevelCounter != null)
        {
            instance.characterLevelCounter.setCounter();
        }
    }

    public static void updatePartyMemberUpgradeCounter()
    {
        if (instance != null && instance.partyMemberUpgradeCounter != null)
        {
            instance.partyMemberUpgradeCounter.setCounter();
        }
    }

    public static void updateQuestCounter()
    {
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
        getInstance().updateUI();
    }

    public void updateUI()
    {
        if (Flags.isInNewGameMode() || CombatStateManager.inCombat)
        {
            disableOOCUI();
            return;
        }

        updateIntimidateText();

        updateCunningText();

        updateObservationText();

        updateLeadershipText();
    }

    private void updateIntimidateText()
    {
        if (PartyStats.getMaxIntimidateCount() != 0)
        {
            intimidateParent.SetActive(true);
            intimidateCountText.text = IntimidateManager.getIntimidatesRemaining() + "/" + PartyStats.getMaxIntimidateCount() + "  I";

            if (IntimidateManager.getIntimidatesRemaining() == 0)
            {
                intimidateCountImage.color = Color.gray;
            }
            else
            {
                intimidateCountImage.color = Color.white;
            }
        }
        else
        {
            intimidateParent.SetActive(false);
        }
    }


    private void updateCunningText()
    {
        if (PartyStats.getMaxCunningCount() != 0)
        {
            cunningParent.SetActive(true);
            cunningCountText.text = CunningManager.getCunningsRemaining() + "/" + PartyStats.getMaxCunningCount() + " C";

            if (CunningManager.getCunningsRemaining() == 0)
            {
                cunningCountImage.color = Color.gray;
            }
            else
            {
                cunningCountImage.color = Color.white;
            }
        }
        else
        {
            cunningParent.SetActive(false);
        }
    }

    private void updateObservationText()
    {
        if (manuallySetObservationPanelColor)
        {
            return;
        }

        if (PartyManager.getPlayerStats() != null && PartyManager.getPlayerStats().getWisdom() < SkillManager.skillUnlockLevel)
        {
            observationParent.SetActive(false);
            return;
        }

        observationParent.SetActive(true);

        if (PlayerOOCStateManager.currentActivity == OOCActivity.observing)
        {
            observationToggleImage.color = Color.green;
        }
        else if (PlayerOOCStateManager.currentActivity != OOCActivity.observing)
        {
            observationToggleImage.color = Color.red;
        }
    }

    public static void manuallySetObservationPanelColorOn()
    {
        manuallySetObservationPanelColor = true;

        getInstance().observationToggleImage.color = Color.green;
    }

    public static void manuallySetObservationPanelColorOff()
    {
        manuallySetObservationPanelColor = false;
        updateOOCUI();
    }

    private void updateLeadershipText()
    {
        if (PartyStats.getMaxPlacablePartyMembers() != 0)
        {
            leadershipParent.SetActive(true);
            leadershipCountText.text = PartyMemberPlacer.getPlacedPartyMemberCount() + "/" + PartyStats.getMaxPlacablePartyMembers() + " L";
        }
        else
        {
            leadershipParent.SetActive(false);
        }
    }

    public void disableOOCUI()
    {
        if(oocUIParent == null || oocUIParent is null)
        {
            return;
        }

        State.oocUIManager = this;
        oocUIParent.SetActive(false);
        topOOCUIBackground.color = Color.clear;
        bottomOOCUIBackground.color = Color.clear;
    }

    public void enableOOCUI()
    {

        State.oocUIManager = this;
        oocUIParent.SetActive(true);
        topOOCUIBackground.color = Color.black;
        bottomOOCUIBackground.color = Color.black;
    }

    public void testAddXP()
    {
        PartyManager.addXP(AllyStats.xpNeededToLevelUp);
    }

    private void setupHostilityBars()
    {
        int lowestGreenIndex = AreaList.getCurrentAreaHostility();

        if (lowestGreenIndex >= Area.hostilityThreshold)
        {
            setAllHostilityBarsToRed();
            return;
        }
        else
        {
            for (int barIndex = 0; barIndex < hostilityBars.Length; barIndex++)
            {
                if (barIndex < lowestGreenIndex)
                {
                    hostilityBars[barIndex].color = Color.yellow;
                }
                else
                {
                    hostilityBars[barIndex].color = Color.green;
                }
            }
        }
    }

    private void setAllHostilityBarsToRed()
    {
        foreach (Image bar in hostilityBars)
        {
            bar.color = Color.red;
        }
    }

    public void updateFooting()
    {
        if (State.onLeftFoot)
        {
            leftFootImage.color = Color.green;
            rightFootImage.color = Color.red;
        }
        else
        {
            rightFootImage.color = Color.green;
            leftFootImage.color = Color.red;
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

        if (!SceneManager.GetActiveScene().name.Equals(SceneNameList.startMenu))
        {
            setupHostilityBars();
        }
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
        return getListOfQuestsForDisplay().Count;
    }

    public ArrayList getListOfQuestsForDisplay()
    {
        IMapObject location = MapObjectList.getMapObject(getListKey());

        return location.getAllQuestsInLocation();
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
        updateCounters();
    }
    public List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        listOfEvents.Add(CombatActionArray.OnCombatActionArrayChange);
        listOfEvents.Add(Stats.OnStatsChange);
        listOfEvents.Add(UpgradePartyMemberDecisionPanel.OnPartyMemberUpgraded);

        return listOfEvents;
    }
}
