using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class HealthBarManager : MonoBehaviour
{
    public GameObject masterSymbol;
    public GameObject minionSymbol;
    public GameObject mandatoryTargetSymbol;
    public GameObject stunnedSymbol;

	public Image backgroundImage; //starts green

	public Slider previewSlider;
	public Image previewImage;
	
	public Slider emptySlider;
    public Image emptyImage;
	
    public Stats linkedStats;

    private bool linkedStatsIsAlive => linkedStats != null && linkedStats.isAlive();
    private bool hovered;

    private void Awake()
    {
        if(!CombatStateManager.inCombat)
        {
            return;
        }

        Trait.OnTraitApplication.AddListener(updateHealthBarColor);
        Trait.OnTraitRemoval.AddListener(updateHealthBarColor);
        // LargeEnemyStats.OnLargeEnemySpawn.AddListener(cleanUpHiddenHealthBars);
        DescriptionPanelBuilder.OnFormulaSwap.AddListener(updateCreatureTypeSymbols);
        DescriptionPanelBuilder.OnFormulaSwap.AddListener(show);
        CombatUIModule.OnHideCombatUI.AddListener(hide);
        GameplaySettingsList.OnHealthBarVisibilitySettingChange.AddListener(show);
        SelectorManager.SelectorMoved.AddListener(determineVisibility);

        if(CombatStateManager.whoseTurn == WhoseTurn.Won ||
                    CombatStateManager.whoseTurn == WhoseTurn.Lost)
        {
            hide();
        }
    }

    private void OnDestroy()
    {
        if(!CombatStateManager.inCombat)
        {
            return;
        }

        Trait.OnTraitApplication.RemoveListener(updateHealthBarColor);
        Trait.OnTraitRemoval.RemoveListener(updateHealthBarColor);
        // LargeEnemyStats.OnLargeEnemySpawn.RemoveListener(cleanUpHiddenHealthBars);
        DescriptionPanelBuilder.OnFormulaSwap.RemoveListener(updateCreatureTypeSymbols);
        DescriptionPanelBuilder.OnFormulaSwap.RemoveListener(show);
        SelectorManager.SelectorMoved.RemoveListener(determineVisibility);
        CombatUIModule.OnHideCombatUI.RemoveListener(hide);
        GameplaySettingsList.OnHealthBarVisibilitySettingChange.RemoveListener(show);
    }

    // Called by CombatantHover when the mouse enters/leaves this bar's owner.
    public void setHovered(bool isHovered)
    {
        hovered = isHovered;
        show();
    }

    private bool healthBarsAlwaysVisibleSettingOn()
    {
        Debug.LogError("set = " + GameplaySettingsList.healthBarsAlwaysVisible.settingOptions[Constants.onSettingIndex].set);
        return GameplaySettingsList.healthBarsAlwaysVisible.settingOptions[Constants.onSettingIndex].set;
    }

    private void updateCreatureTypeSymbols()
    {
        // OverallUIManager.showFormula &&
        if(linkedStats != null)
        {
            masterSymbol.SetActive(linkedStats.isMaster());
            minionSymbol.SetActive(linkedStats.isMinion());
        }
        // else
        // {
        //     masterSymbol.SetActive(false);
        //     minionSymbol.SetActive(false);
        // }
    }

    public void updateHealthBarColor(Trait trait)
    {
        if(linkedStats.inPreviewMode)
        {
            return;
        }

        stunnedSymbol.SetActive(linkedStats.isStunned());
        mandatoryTargetSymbol.SetActive(linkedStats.isMandatoryTarget());
        
        if(linkedStats.isDebuffed() && linkedStats.isBuffed())
        {
            backgroundImage.color = ColorList.buffedDebuffed;
            return;
        }

        if(linkedStats.isBuffed())
        {
            backgroundImage.color = ColorList.buffedBlue;
            return;
        }

        if(linkedStats.isDebuffed())
        {
            backgroundImage.color = ColorList.debuffedPurple;
            return;
        }

        backgroundImage.color = ColorList.healthyGreen;
    }

    public void setLinkedStats(Stats statsToLink)
    {
        if(statsToLink != null && statsToLink.isRepositionClone())
        {
            return;
        }

        linkedStats = statsToLink;

        updateHealthBarColor(null);
        show();
    }

    private void determineVisibility(List<Selector> declaredSelectors)
    {
        if(linkedStats == null || linkedStats.isDead() ||
            CombatStateManager.whoseTurn == WhoseTurn.Won ||
                    CombatStateManager.whoseTurn == WhoseTurn.Lost || 
                    CombatStateManager.whoseTurn == WhoseTurn.Resolving)
        {
            hide();
            return;
        }

        if(healthBarsAlwaysVisibleSettingOn())
        {
            show();
            return;
        }

        foreach(Selector selector in declaredSelectors)
        {
            if(selector.containsTarget(linkedStats) && 
                (
                healthBarsAlwaysVisibleSettingOn() || 
                hovered || 
                (SelectorManager.currentSelector != null && SelectorManager.currentSelector.containsTarget(linkedStats))
                ))
            {
                show();
                return;
            }
        }

        hide();
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }

    public void show()
    {
        if(linkedStatsIsAlive)
        {
            updateCreatureTypeSymbols();
            updateHealthBarColor(null);
            gameObject.SetActive(true);
        }
    }

    public void setTotalHealth(int totalHealth)
    {
        emptySlider.maxValue = totalHealth;
        previewSlider.maxValue = totalHealth;

        GameObjectUtil.updateGameObjectPosition(previewImage.gameObject);
        GameObjectUtil.updateGameObjectPosition(emptyImage.gameObject);
    }

	public int getTotalHealth()
	{
		if(emptySlider.maxValue != previewSlider.maxValue)
		{
			Debug.LogError("emptySlider.maxValue != previewSlider.maxValue");
		}
		
		return (int) emptySlider.maxValue;
	}

	public void setMissingHealth(int missingHealth)
	{
		emptySlider.value = missingHealth;
		
		if(emptySlider.value > 0 && emptyImage.gameObject != null)
		{
			GameObjectUtil.updateGameObjectPosition(emptyImage.gameObject);
		} else if(emptyImage.gameObject != null)
		{
			emptyImage.gameObject.SetActive(false);
		}
	}

	public int getMissingHealth()
	{
		return (int) emptySlider.value;
	}

	public void resetPreviewHealth()
	{
		previewSlider.value = emptySlider.value;
		previewImage.color = Color.yellow;

		if (previewSlider.value > 0 && previewImage.gameObject != null)
		{
			GameObjectUtil.updateGameObjectPosition(previewImage.gameObject);
		}
		else if (previewImage.gameObject != null)
		{
			previewImage.gameObject.SetActive(false);
		}
	}
	
	public void addPreviewHealth(int incomingDamage)
	{
		if(previewSlider.value < emptySlider.value)
		{
			previewSlider.value = emptySlider.value;
		}
		
		if(previewSlider.maxValue <= previewSlider.value + incomingDamage)
		{
			previewSlider.value = previewSlider.maxValue;
			previewImage.color = Color.red;
		} else
		{	
			previewSlider.value += incomingDamage;
			previewImage.color = Color.yellow;
		}
		
		if(previewSlider.value > 0 && previewImage.gameObject != null)
		{
			GameObjectUtil.updateGameObjectPosition(previewImage.gameObject);
		} else if(previewImage.gameObject != null)
		{
			previewImage.gameObject.SetActive(false);
		}
	}
	
    // private void cleanUpHiddenHealthBars()
    // {
    //     if(!gameObject.activeInHierarchy && (linkedStats == null || linkedStats.isLarge()))
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    public void setPosition(Vector3 worldPosition)
    {
        if(worldPosition.Equals(Vector3.zero))
        {
            return;
        }

        transform.position = worldPosition;

        GameObjectUtil.updateGameObjectPosition(gameObject);
    }

    public static HealthBarManager createHealthBar(Stats stats, Transform parent)
    {
        GameObject healthBar = Instantiate(Resources.Load<GameObject>(PrefabNames.healthBar), parent);
        RectTransform rect = healthBar.GetComponent<RectTransform>();

        HealthBarManager healthBarManager = healthBar.GetComponent<HealthBarManager>();
        healthBarManager.linkedStats = stats;
        // rect.position = Helpers.getAveragePosition(stats.getAllWorldPositions());
        rect.localScale = new Vector3(.14f, .14f);
        rect.anchoredPosition = Vector3.zero; // e.g. (0, -0.05, 0)
        rect.localPosition = new(0f,0.225f,0f);

        if(CombatStateManager.inCombat && !GameplaySettingsList.healthBarsAlwaysVisible.settingOptions[Constants.onSettingIndex].set)
        {
            healthBarManager.hide();
        }

        return healthBarManager;
    }

}
