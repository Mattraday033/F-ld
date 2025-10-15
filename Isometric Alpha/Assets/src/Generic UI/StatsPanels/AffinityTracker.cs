using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AffinityTracker : MonoBehaviour, IStatsPanel
{
	public TextMeshProUGUI affinityText;

	private void Awake()
	{

		updateStatsPanel();
	}

	public void updateStatsPanel()
	{
		affinityText.text = "" + AffinityManager.getTotalAffinity();
	}
}
