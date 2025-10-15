using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class BookPopUpWindow : PopUpWindow
{
    public DescriptionPanel descriptionPanel;

    private OOCActivity previousActivity;
    private bool giveCopyOfBook;
    private GameObject bookGameObject;

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
        book.describeSelfFull(descriptionPanel);
    }

    public override void handleEscapePress()
    {
        base.handleEscapePress();

        pickUpBookOnUIClose();

        PlayerOOCStateManager.returnToPreviousActivity();
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
}
