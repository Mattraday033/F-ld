using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

public enum RowType {
						Standard = 1,
						Equipment = 2,
						StatRequirements = 3,
						AbilityEditor = 4,
						CompanionAbilities = 5,
						FormationEditor = 6,
						JournalCategory = 7,
						CombatActionOrder = 8,
						LevelUp = 9,
						Shop = 10,
						Map = 11,
						MapWithoutHover = 12,
						PartyScreen = 13
					}

public class ScrollableUIElement : MonoBehaviour
{
	public const string panelNamePrefix = "Panel_";

    public readonly static UnityEvent HaltAllScrolling = new UnityEvent();
    public readonly static UnityEvent ContinueAllScrolling = new UnityEvent();

    public readonly static UnityEvent PanelsPopulated = new UnityEvent();

	public GameObject grid;
	public GameObject scrollContainer;
	public GameObject scrollableArea;
	public ContentSizeFitter scrollableAreaContentSizeFitter;

	public bool setScrollBarToBottomOnPopulate = false;

	public GameObject scrollBar;
	public GameObject slidingArea;
	public ScrollRect scrollableComponent;
    public Slider slider;

	public bool clickFirstPanel;
	public bool clickLastPanel;
	public bool useBlockDescriptionsInRows;
	private const bool clickThisPanel = true;
	public bool keepScrollBarVisable = false;
	public bool normalizeAfterPopulate = false;

	public bool describeAsFullPanel;
	public RowType rowType;
	public string specificRowType;

	public ColumnHeader columnHeader;
	public bool sortPanels = true;
	public SortBy defaultSortBy = SortBy.Name;

	public List<GridRow> listOfRows = new List<GridRow>();

	//set on displays that are kept alive and refilled, like the combat hover panel's traits. A refill re-describes the rows it
	//already has and hides the ones it doesn't need, instead of destroying and instantiating them. Off everywhere else
	public bool reuseRows = false;

	//rows a refill hid rather than destroyed, kept for the next refill
	private List<GridRow> spareRows = new List<GridRow>();

	//the prefab each reusable row was made from, so a refill only hands a row to a describable that would get the same one
	private Dictionary<GridRow, GameObject> rowPrefabs = new Dictionary<GridRow, GameObject>();

	//setToIneligible can't be undone, so these rows are destroyed instead of reused
	private HashSet<GridRow> ineligibleRows = new HashSet<GridRow>();

	public GameObject[] objectsDisabledWithSlider;

	private void Awake()
	{
        // if (performDisableScrollBarCheck)
        // {
        //     disableScrollCheck();
        // } else
         if (scrollableComponent != null && scrollableComponent.verticalScrollbar != null)
		{
			scrollableComponent.verticalScrollbar.size = 0.1f;
		}

        HaltAllScrolling.AddListener(haltScrolling);
        ContinueAllScrolling.AddListener(continueScrolling);
	}

    private void OnDestroy()
    {
        HaltAllScrolling.RemoveListener(haltScrolling);
        ContinueAllScrolling.RemoveListener(continueScrolling);
    }

    private void haltScrolling()
    {
        if(scrollableComponent == null)
        {
            return;
        }

        scrollableComponent.enabled = false;
    }

    private void continueScrolling()
    {
        if(scrollableComponent != null)
        {
            scrollableComponent.enabled = true;

            if(scrollableComponent.verticalScrollbar != null)
            {
			    scrollableComponent.verticalScrollbar.size = 0.1f;
            }
        }
    }

	public void setRowType(RowType newRowType)
	{
		this.rowType = newRowType;
	}

	public RowType getRowType()
	{
		return rowType;
	}

	public void appendPanels(IEnumerable<IDescribable> listOfDescribables)
	{
		populatePanels(new List<IDescribable>(listOfDescribables), false);
	}

	public virtual void appendPanels(List<IDescribable> listOfDescribables)
	{
		populatePanels(listOfDescribables, false);
	}

	public void populatePanels(IEnumerable<IDescribable> listOfDescribables)
	{
		populatePanels(new List<IDescribable>(listOfDescribables), true);
	}

	public virtual void populatePanels(List<IDescribable> listOfDescribables)
	{
		populatePanels(listOfDescribables, true);
	}

	private void populatePanels(List<IDescribable> listOfDescribables, bool deleteOldPanels)
	{
		// Debug.LogError("populating Panels");

		if (sortPanels)
		{
			listOfDescribables = sortListOfPanels(listOfDescribables);
		}

		if (deleteOldPanels && reuseRows)
		{
			//the toggles below re-run OnDisable/OnEnable on every row, which a refill exists to avoid
			refillPanels(listOfDescribables);
		}
		else
		{
			if (deleteOldPanels)
			{
				deleteAllPanels();
			}

			int rowIndex = 0;
			foreach (IDescribable describable in listOfDescribables)
			{

				listOfRows.Add(populatePanel(describable, rowIndex));

				rowIndex++;
			}

			if (scrollContainer != null && !(scrollContainer is null) &&
				scrollableArea != null && !(scrollableArea is null))
			{
				GameObjectUtil.updateGameObjectPosition(scrollContainer);
				GameObjectUtil.updateGameObjectPosition(scrollableArea);
			}
		}

		// if (performDisableScrollBarCheck)
		// {
		// 	disableScrollCheck();
		// }

		disableScrollableComponentCheck();

		if (setScrollBarToBottomOnPopulate)
		{
			if (gameObject.activeInHierarchy)
			{
				StartCoroutine(buildThenScrollToBottom());
			}
		}

		if (clickFirstPanel)
		{
			clickFirstPanelInList();
		}
		else if (clickLastPanel)
		{
			clickLastPanelInList();
		}

		// if(normalizeAfterPopulate)
		// {
		// 	scrollableComponent.verticalScrollbar.value = scrollableComponent.verticalNormalizedPosition;
		// }

        if( deleteOldPanels && 
            gameObject.activeInHierarchy && 
            scrollableComponent != null && 
            scrollableComponent.verticalScrollbar != null && 
            updateScrollComp == null)
        {
            updateScrollComp = StartCoroutine(updateScrollComponent(scrollableComponent.verticalScrollbar));
        }

        PanelsPopulated.Invoke();
	}

    public bool populated()
    {
        return listOfRows.Count > 0;
    }

    private Coroutine updateScrollComp;

    private IEnumerator updateScrollComponent(Scrollbar scrollbar)
    {
        yield return new WaitForEndOfFrame();
        
        scrollableComponent.verticalNormalizedPosition = 1f;
        scrollableComponent.verticalScrollbar.size = 0.1f;
        scrollbar.Rebuild(CanvasUpdate.Prelayout);
        updateScrollComp = null;
    }

    private List<IDescribable> sortListOfPanels(List<IDescribable> listOfDescribables)
    {
        if(listOfDescribables.Count <= 0 || listOfDescribables[0] as ISortable == null)
        {
            return listOfDescribables;
        }

        List<ISortable> listOfSortables = listOfDescribables.Cast<ISortable>().ToList();

        listOfSortables.Sort(getComparisonMethod());

        return listOfSortables.Cast<IDescribable>().ToList();
    }

	public void clickFirstPanelInList()
	{
        disableGridRowAndClick(0);

		// foreach (GridRow gridRow in listOfRows)
		// {
		// 	if (gridRow.buttonTexts.Length > 0 && !gridRow.buttonTexts[0].text.Equals(""))
		// 	{
		// 		disableGridRowAndClick(gridRow.buttonTexts[0].text, true);
		// 		return;
		// 	}
		// }
	}

	public void clickLastPanelInList()
	{
		for (int index = listOfRows.Count - 1; index >= 0; index--)
		{
			GridRow gridRow = listOfRows[index];

			if (!gridRow.buttonTexts[0].text.Equals(""))
			{
				disableGridRowAndClick(gridRow.buttonTexts[0].text, true);
				return;
			}
		}
	}

	private IEnumerator buildThenScrollToBottom()
	{
		yield return null;
		yield return null;

        scrollableComponent.verticalNormalizedPosition = 0f;
        
        if(slider != null)
        {
            slider.value = 0f;
        }
	}

	public GridRow populatePanel(IDescribable describable, int rowIndex)
	{
		return populatePanel(describable, rowIndex, false);
	}

	public GridRow populatePanel(IDescribable describable, int rowIndex, bool clickRow)
	{
		GridRow gridRow;

		if (describable != null)
		{
			if (useBlockDescriptionsInRows && describable.buildableWithBlocksRows())
			{
				return populatePanelWithBlockDescriptions(describable as IDescribableInBlocks);
			}
			else
			{
				gridRow = Instantiate(describable.getRowType(rowType), scrollableArea.transform).GetComponent<GridRow>();
			}
		}
		else
		{
			gridRow = Instantiate(Resources.Load<GameObject>(specificRowType), scrollableArea.transform).GetComponent<GridRow>();
		}

		gridRow.setParentGrid(this);

		gridRow.gameObject.name = gridRow.gameObject.name + "_" + rowIndex;

		if (describable != null && !(describable is null))
		{
			if (describeAsFullPanel)
			{
				describable.describeSelfFull(gridRow.descriptionPanel);
			}
			else
			{
				describable.describeSelfRow(gridRow.descriptionPanel);
			}
		}

		gridRow.gameObject.SetActive(true);

		if (describable != null && !(describable is null) && describable.ineligible())
		{
			gridRow.setToIneligible();
		}

		if (clickRow)
		{
			gridRow.nameButton.onClick.Invoke();
		}

		return gridRow;
	}

	public GridRow populatePanelWithBlockDescriptions(IDescribableInBlocks describableInBlocks)
	{
		GameObject blockDescriptionRow = Instantiate(Resources.Load<GameObject>(PrefabNames.descriptionPanelBuilder), scrollableArea.transform);

		DescriptionPanelBuilder descriptionPanelBuilder = blockDescriptionRow.GetComponent<DescriptionPanelBuilder>();

		descriptionPanelBuilder.buildDescriptionPanel(describableInBlocks);

		return blockDescriptionRow.GetComponent<BlockGridRow>();
	}

	public string getDisabledRowName()
	{
		IDescribable describable = getDisabledRowDescribable();

		if (describable != null && !(describable is null))
		{
			return describable.uniqueName;
		}
		else
		{
			return null;
		}
	}

	public IDescribable getDisabledRowDescribable()
	{
		GridRow gridRow = getDisabledGridRow();

		if (gridRow == null || gridRow is null)
		{
			return null;
		}
		else
		{
			return gridRow.descriptionPanel.getObjectBeingDescribed();
		}
	}

	private RectTransform getDisabledGridRowRectTransform()
	{
		GridRow gridRow = getDisabledGridRow();

		if (gridRow == null || gridRow is null)
		{
			return null;
		}
		else
		{
			return gridRow.gameObject.GetComponent<RectTransform>();
		}
	}

	private GridRow getDisabledGridRow()
	{
		foreach (GridRow row in listOfRows)
		{
            if (row.nameButton == null)
            {
                return null;
            }
            
			if (!row.nameButton.interactable)
            {
                return row;
            }
		}

		return null;
	}

	private float getDisabledGridRowScrollBarValue()
	{
		float rowNumber = 1f;

		foreach (GridRow row in listOfRows)
		{
			if (!row.nameButton.interactable)
			{
				break;
			}
			else
			{
				rowNumber += 1f;
			}
		}

		if (rowNumber > (float)listOfRows.Count)
		{
			return -1f;
		}

		return (float)(rowNumber / (float)listOfRows.Count);
	}

	public bool disableGridRowAndClick(string name, bool enableRows = true)
	{
		if (enableRows)
		{
			enableAllGridRows();
		}

        foreach (GridRow row in listOfRows)
        {
            if (row.descriptionPanel.getObjectBeingDescribed() != null &&
                String.Equals(row.descriptionPanel.getObjectBeingDescribed().uniqueName, name, StringComparison.OrdinalIgnoreCase))
            {
                row.nameButton.onClick.Invoke();

                return true;
            }
        }

		return false;
	}

	public void disableGridRow(string name, bool enableRows = true)
	{
		if (enableRows)
		{
			enableAllGridRows();
		}

        foreach (GridRow row in listOfRows)
        {
            if (row.descriptionPanel.getObjectBeingDescribed() != null &&
                String.Equals(row.descriptionPanel.getObjectBeingDescribed().uniqueName, name, StringComparison.OrdinalIgnoreCase))
            {
                row.nameButton.interactable = false;

                return;
            }
        }
	}

	public void disableGridRowAndClick(int rowIndex)
	{
		enableAllGridRows();

		if (rowIndex >= listOfRows.Count)
		{
			return;
		}

		GridRow row = listOfRows[rowIndex];

		if (row.descriptionPanel.getObjectBeingDescribed() != null)
		{
            row.nameButton.onClick.Invoke();
		}
	}

	public void enableAllGridRows()
	{
		foreach (GridRow row in listOfRows)
		{
			if (row.descriptionPanel.getObjectBeingDescribed() == null)
			{
				continue;
			}

			row.enableButtons();
		}
	}

	public void debugShout()
	{
		// Debug.LogError("Moving");
	}

	public virtual void deleteAllPanels()
	{
		foreach (GridRow row in listOfRows)
		{
			row.onDestruction();
			Destroy(row.gameObject);
		}

		listOfRows = new List<GridRow>();

		foreach (GridRow row in spareRows)
		{
			if (row != null)
			{
				row.onDestruction();
				Destroy(row.gameObject);
			}
		}

		spareRows = new List<GridRow>();
		rowPrefabs.Clear();
		ineligibleRows.Clear();
	}

	//describes the list using the rows already here where it can, hiding leftovers and instantiating only when it runs out
	private void refillPanels(List<IDescribable> listOfDescribables)
	{
		//rows in sibling order, so taking them front to back keeps the displayed order matching the list
		List<GridRow> availableRows = new List<GridRow>(listOfRows);
		availableRows.AddRange(spareRows);
		availableRows.RemoveAll(row => row == null);
		availableRows.Sort((first, second) => first.transform.GetSiblingIndex().CompareTo(second.transform.GetSiblingIndex()));

		listOfRows = new List<GridRow>();
		spareRows = new List<GridRow>();

		int rowIndex = 0;
		foreach (IDescribable describable in listOfDescribables)
		{
			bool reusable = describable != null && !(describable is null) && !useBlockDescriptionsInRows && !describable.ineligible();

			GameObject rowPrefab = reusable ? describable.getRowType(rowType) : null;

			GridRow row = reusable ? takeReusableRow(availableRows, describable, rowPrefab) : null;

			if (row == null)
			{
				row = populatePanel(describable, rowIndex);

				if (reusable)
				{
					rowPrefabs[row] = rowPrefab;
				}
			}

			if (describable != null && !(describable is null) && describable.ineligible())
			{
				ineligibleRows.Add(row);
			}

			listOfRows.Add(row);

			rowIndex++;
		}

		//a row taken out of sibling order (one with a different prefab was skipped) is moved so the display still follows the list
		for (int index = 1; index < listOfRows.Count; index++)
		{
			if (listOfRows[index].transform.GetSiblingIndex() < listOfRows[index - 1].transform.GetSiblingIndex())
			{
				foreach (GridRow row in listOfRows)
				{
					row.transform.SetAsLastSibling();
				}

				break;
			}
		}

		foreach (GridRow leftoverRow in availableRows)
		{
			if (!rowPrefabs.ContainsKey(leftoverRow) || ineligibleRows.Contains(leftoverRow))
			{
				rowPrefabs.Remove(leftoverRow);
				ineligibleRows.Remove(leftoverRow);
				leftoverRow.onDestruction();
				Destroy(leftoverRow.gameObject);
				continue;
			}

			leftoverRow.gameObject.SetActive(false);
			spareRows.Add(leftoverRow);
		}
	}

	//the first available row made from the prefab this describable would get, re-described for it. Null when there's none
	private GridRow takeReusableRow(List<GridRow> availableRows, IDescribable describable, GameObject rowPrefab)
	{
		for (int index = 0; index < availableRows.Count; index++)
		{
			GridRow row = availableRows[index];

			if (ineligibleRows.Contains(row) || !rowPrefabs.TryGetValue(row, out GameObject builtFrom) || builtFrom != rowPrefab)
			{
				continue;
			}

			availableRows.RemoveAt(index);

			if (describeAsFullPanel)
			{
				describable.describeSelfFull(row.descriptionPanel);
			}
			else
			{
				describable.describeSelfRow(row.descriptionPanel);
			}

			row.gameObject.SetActive(true);

			return row;
		}

		return null;
	}

	public bool contains(string name)
	{
		foreach (GridRow row in listOfRows)
		{
			if (row.descriptionPanel.getObjectBeingDescribed() != null &&
				row.descriptionPanel.getObjectBeingDescribed().uniqueName.Equals(name))
			{
				return true;
			}
		}

		return false;
	}

	public void disableScrollCheck()
	{
		if (scrollContainer == null || scrollContainer is null ||
			scrollableArea == null || scrollableArea is null ||
			slidingArea == null || slidingArea is null ||
			!gameObject.activeInHierarchy)
		{
			return;
		}

		RectTransform containerRectTransform = scrollContainer.GetComponent<RectTransform>();
		RectTransform areaRectTransform = scrollableArea.GetComponent<RectTransform>();

		GameObjectUtil.updateGameObjectPosition(scrollContainer);
		GameObjectUtil.updateGameObjectPosition(scrollableArea);

		LayoutRebuilder.ForceRebuildLayoutImmediate(containerRectTransform);
		LayoutRebuilder.ForceRebuildLayoutImmediate(areaRectTransform);

		Canvas.ForceUpdateCanvases();

		StartCoroutine(slidingAreaCheck(containerRectTransform, areaRectTransform));
	}

	private IEnumerator slidingAreaCheck(RectTransform containerRectTransform, RectTransform areaRectTransform)
	{
		yield return new WaitForEndOfFrame();

		if ((scrollableComponent.horizontal && Math.Abs(containerRectTransform.rect.width) >= Math.Abs(areaRectTransform.rect.width)) ||
			(scrollableComponent.vertical && Math.Abs(containerRectTransform.rect.height) >= Math.Abs(areaRectTransform.rect.height)))
		{
			scrollableComponent.verticalNormalizedPosition = 0f;

			if (keepScrollBarVisable)
			{
				scrollBar.SetActive(true);
				slidingArea.SetActive(false);
				setActiveAllSliderObjects(true);
			}
			else
			{
				scrollBar.SetActive(false);
				setActiveAllSliderObjects(false);
			}
		}
		else
		{
			scrollBar.SetActive(true);
			slidingArea.SetActive(true);
			setActiveAllSliderObjects(true);

			LayoutRebuilder.ForceRebuildLayoutImmediate(scrollBar.GetComponent<RectTransform>());
			LayoutRebuilder.ForceRebuildLayoutImmediate(slidingArea.GetComponent<RectTransform>());

			Canvas.ForceUpdateCanvases();
		}
	}

	private void disableScrollableComponentCheck()
	{
		if (scrollContainer == null || scrollContainer is null ||
			scrollableArea == null || scrollableArea is null ||
			scrollBar == null || scrollBar is null)
		{
			return;
		}

		RectTransform containerRectTransform = scrollContainer.GetComponent<RectTransform>();
		RectTransform areaRectTransform = scrollableArea.GetComponent<RectTransform>();

		LayoutRebuilder.ForceRebuildLayoutImmediate(areaRectTransform);
		/*
		if(Math.Abs(containerRectTransform.rect.y) >= Math.Abs(areaRectTransform.rect.y))
		{
			scrollableComponent.enabled = false;
		} else
		{
			scrollableComponent.enabled = true;
		}*/
	}

	public void snapToDisabledRow()
	{
		RectTransform rowTransform = getDisabledGridRowRectTransform();

		if (rowTransform == null || rowTransform is null)
		{
			return;
		}

		// Debug.LogError("snapToDisabledRow()");

		Vector2 snapToPosition = ScrollRectExtensions.getSnapToPosition(scrollableComponent, rowTransform);

		scrollableComponent.content.localPosition = snapToPosition;
	}

	public IComparer<ISortable> getComparisonMethod()
	{
		if (columnHeader == null || columnHeader is null)
		{
			return ComparerList.getComparer(defaultSortBy);
		}
		else
		{
			return columnHeader.getComparisonMethod();
		}
	}
	
	private void setActiveAllSliderObjects(bool active)
	{
		if (objectsDisabledWithSlider == null)
		{
			return;
		}

		foreach (GameObject sliderObject in objectsDisabledWithSlider)
		{
			if (sliderObject == null || sliderObject is null)
			{
				continue;
			}

			sliderObject.SetActive(active);
		}
	}
}
public static class ScrollRectExtensions
{
    public static Vector2 getSnapToPosition(ScrollRect instance, RectTransform child)
    {
        Canvas.ForceUpdateCanvases();

        Vector2 viewportLocalPosition = instance.viewport.localPosition;
        Vector2 childLocalPosition = child.localPosition;

        Vector2 result = new Vector2( 0, 0 - (viewportLocalPosition.y + childLocalPosition.y));

        return result;
    }
}