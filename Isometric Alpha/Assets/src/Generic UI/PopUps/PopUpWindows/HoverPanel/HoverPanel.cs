using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

//one panel is kept for the whole combat. Changing combatant refills it in place rather than rebuilding it
public class HoverPanel : PopUpWindow, IEscapable
{
	private static readonly ProfilerMarker populateMarker = new ProfilerMarker("HoverPanel.populate");

	public Transform descriptionPanelParent;
	public ScrollableUIElement traitDisplay;
	public Transform traitDescriptionPanelParent;

	private static HoverPanel instance;

	//forgotten whenever the panel hides, so showing anyone again always refills it with current data
	private Stats displayedCombatant;

	//allies and everyone else get different rows, so each layout keeps a builder of its own that is refilled in place
	private DescriptionPanelBuilder allyBuilder;
	private DescriptionPanelBuilder otherBuilder;

	//its own canvas, so a refill re-batches only the panel rather than the whole combat UI
	private Canvas panelCanvas;

	public static HoverPanel getInstance()
	{
		return instance;
	}

	public bool isShowing
	{
		get
		{
			return gameObject.activeSelf;
		}
	}

	[RuntimeInitializeOnLoadMethod]
	private static void resetInstance()
	{
		instance = null;
	}

	private void Awake()
	{
		if(instance != null && !(instance is null) && instance != this)
		{
			Debug.LogError("Duplicate instances of HoverPanel exist erroneously");
			Destroy(gameObject);
			return;
		}

		instance = this;

		panelCanvas = gameObject.AddComponent<Canvas>();

		//a nested canvas needs its own raycaster, or the trait icons stop receiving hovers
		gameObject.AddComponent<GraphicRaycaster>();

		traitDisplay.reuseRows = true;
	}

	private void OnDestroy()
	{
		if(instance == this)
		{
			instance = null;
		}
	}

	public void show()
	{
		gameObject.SetActive(true);
	}

	public void hide()
	{
		clearTraitHoverPanels();

		displayedCombatant = null;

		gameObject.SetActive(false);
	}

	//lets the panel be built while it's active but not drawn
	public void setRendered(bool rendered)
	{
		panelCanvas.enabled = rendered;
	}

	public Stats getDisplayedCombatant()
	{
		return displayedCombatant;
	}

	public void populate(Stats combatant)
	{
		if(combatant == null || ReferenceEquals(combatant, displayedCombatant))
		{
			return;
		}

		using(populateMarker.Auto())
		{
			//any trait hover panel open now describes the combatant being replaced
			clearTraitHoverPanels();

			descriptionPanelSlot.prebuiltBuilder = getBuilderFor(combatant);
			descriptionPanelSlot.setPrimaryDescribable(combatant);

			traitDisplay.populatePanels(combatant.traitContainer.toListForDisplay());

			displayedCombatant = combatant;
		}
		/*
			deactivate();
			combatant.describeStats(this);

			traitPanelManager.populateTraitPanels(combatant.getTraits());
			traitPanelManager.gameObject.SetActive(true);
		*/
	}

	//AllyStats is the only Stats that adds rows of its own, so this is the only split in layout
	private DescriptionPanelBuilder getBuilderFor(Stats combatant)
	{
		bool isAlly = combatant is AllyStats;

		DescriptionPanelBuilder builder = isAlly ? allyBuilder : otherBuilder;
		DescriptionPanelBuilder otherLayoutBuilder = isAlly ? otherBuilder : allyBuilder;

		if(builder == null)
		{
			builder = DescriptionPanelBuilder.getDescriptionPanelBuilder(descriptionPanelSlot.builderType, descriptionPanelSlot.descriptionPanelParent).GetComponent<DescriptionPanelBuilder>();
			builder.reuseRows = true;

			if(isAlly)
			{
				allyBuilder = builder;
			} else
			{
				otherBuilder = builder;
			}
		}

		if(otherLayoutBuilder != null)
		{
			otherLayoutBuilder.gameObject.SetActive(false);
		}

		builder.gameObject.SetActive(true);

		return builder;
	}

	//trait rows are reused rather than destroyed, so the hover panels they opened have to be closed here
	private void clearTraitHoverPanels()
	{
		foreach(GridRow row in traitDisplay.listOfRows)
		{
			TraitHoverMouseListener traitHover = row as TraitHoverMouseListener;

			if(traitHover != null)
			{
				traitHover.destroyAllDescriptionPanels();
			}
		}
	}

	public static Transform getTraitDescriptionPanelParent()
	{
		if(getInstance() == null)
		{
			return null;
		}

		return getInstance().traitDescriptionPanelParent;
	}
}
