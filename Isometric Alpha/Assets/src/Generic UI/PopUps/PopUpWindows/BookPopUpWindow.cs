using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookPopUpWindow : PopUpWindow
{
    public DescriptionPanel descriptionPanel;

    private OOCActivity previousActivity;
    private bool giveCopyOfBook;
    private GameObject bookGameObject;
    public ScrollableUIElement contentsGrid;
    public Slider slider;
    public GameObject contentsParent;
    public CanvasGroup canvasGroup;

    private BookItem book;

    private static BookPopUpWindow instance;

    public static BookPopUpWindow getInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            throw new IOException("Duplicate instances of BookPopUpWindow exist");
        }

        instance = this;

        StartCoroutine(waitOneFrameThenShow());
    }

    private IEnumerator waitOneFrameThenShow()
    {
        yield return null;
        TutorialSequenceStepTargetUIObject.createCutOutMask(transform);
        canvasGroup.alpha = Constants.sizeOne;
    }

    public void setPreviousActivity(OOCActivity previousActivity)
    {
        this.previousActivity = previousActivity;
    }

    public void setBook(BookItem book)
    {
        this.book = book;
    }
    public void setGiveCopyOfBook(bool giveCopyOfBook)
    {
        this.giveCopyOfBook = giveCopyOfBook;
    }

    public void setBookGameObject(GameObject bookGameObject)
    {
        this.bookGameObject = bookGameObject;
    }

    public void populate()
    {
        if(book.startAtTop())
        {
            contentsGrid.setScrollBarToBottomOnPopulate = false;
            slider.value = 1;
        }

        book.describeSelfFull(descriptionPanel);
        slider.value = 1;
    }

    public override void handleEscapePress()
    {
        base.handleEscapePress();

        pickUpBookOnUIClose();

        PlayerOOCStateManager.setCurrentActivity(previousActivity);
    }

    public void pickUpBookOnUIClose()
    {
        if (giveCopyOfBook)
        {
            Inventory.addItem(book);

            if (bookGameObject != null && !(bookGameObject is null))
            {
                bookGameObject.SetActive(false);
            }
        }
    }

    public static void disableDefaultContentsRow()
    {
        if(instance != null && instance.contentsParent != null)
        {
            instance.contentsParent.SetActive(false);
        }
    }
}
