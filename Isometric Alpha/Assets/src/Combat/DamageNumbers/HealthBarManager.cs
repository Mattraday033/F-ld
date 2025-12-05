using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class HealthBarManager : MonoBehaviour
{
	public Slider previewSlider;
	public Image previewImage;
	
	public Slider emptySlider;
    public Image emptyImage;
	
    public Stats linkedStats;

    public void setLinkedStats(Stats statsToLink)
    {
        linkedStats = statsToLink;
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
	
}
