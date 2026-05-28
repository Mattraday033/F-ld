using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
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

    private void Awake()
    {
        Trait.OnTraitApplication.AddListener(updateHealthBarColor);
        Trait.OnTraitRemoval.AddListener(updateHealthBarColor);
        LargeEnemyStats.OnLargeEnemySpawn.AddListener(cleanUpHiddenHealthBars);
        DescriptionPanelBuilder.OnFormulaSwap.AddListener(updateCreatureTypeSymbols);
    }

    private void OnDestroy()
    {
        Trait.OnTraitApplication.RemoveListener(updateHealthBarColor);
        Trait.OnTraitRemoval.RemoveListener(updateHealthBarColor);
        LargeEnemyStats.OnLargeEnemySpawn.RemoveListener(cleanUpHiddenHealthBars);
        DescriptionPanelBuilder.OnFormulaSwap.RemoveListener(updateCreatureTypeSymbols);
    }

    private void updateCreatureTypeSymbols()
    {
        if(OverallUIManager.showFormula && linkedStats != null)
        {
            masterSymbol.SetActive(linkedStats.isMaster());
            minionSymbol.SetActive(linkedStats.isMinion());
        }
        else
        {
            masterSymbol.SetActive(false);
            minionSymbol.SetActive(false);
        }
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

    public void hide()
    {
        gameObject.SetActive(false);
    }

    public void show()
    {
        if(linkedStats == null || linkedStats.isDead())
        {
            return;
        }
        
        updateHealthBarColor(null);
        gameObject.SetActive(true);
    }

    public void setTotalHealth(int totalHealth)
    {
        emptySlider.maxValue = totalHealth;
        previewSlider.maxValue = totalHealth;

        Helpers.updateGameObjectPosition(previewImage.gameObject);
        Helpers.updateGameObjectPosition(emptyImage.gameObject);
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
			Helpers.updateGameObjectPosition(emptyImage.gameObject);
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
			Helpers.updateGameObjectPosition(previewImage.gameObject);
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
			Helpers.updateGameObjectPosition(previewImage.gameObject);
		} else if(previewImage.gameObject != null)
		{
			previewImage.gameObject.SetActive(false);
		}
	}
	
    private void cleanUpHiddenHealthBars()
    {
        if(!gameObject.activeInHierarchy && (linkedStats == null || linkedStats.isLarge()))
        {
            DestroyImmediate(gameObject);
        }
    }

    public void setPosition(Vector3 worldPosition)
    {
        if(worldPosition.Equals(Vector3.zero))
        {
            return;
        }

        transform.position = worldPosition;

        Helpers.updateGameObjectPosition(gameObject);
    }

}
