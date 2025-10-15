using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BookPopUpButton : PopUpButton
{
    private BookItem book;
    private bool giveCopyOfBook;
    private OOCActivity previousActivity;
    private GameObject bookGameObject;

    public BookPopUpButton() :
    base(PopUpType.Book)
    {

    }
    public void setBook(BookItem book)
    {
        this.book = book;
    }

    public void setBook(ItemListID bookID)
    {
        this.book = (BookItem)ItemList.getItem(bookID);
    }

    public void spawnPopUp(BookItem book, bool giveCopyOfBook, OOCActivity previousActivity, GameObject bookGameObject)
    {
        setBook(book);

        this.giveCopyOfBook = giveCopyOfBook;

        this.previousActivity = previousActivity;

        this.bookGameObject = bookGameObject;

        spawnPopUp();
    }

    public void spawnPopUp(BookItem book)
    {
        setBook(book);

        this.giveCopyOfBook = false;

        spawnPopUp();
    }

    public override void spawnPopUp()
    {
        base.spawnPopUp();

        BookPopUpWindow bookPopUpWindow = (BookPopUpWindow)getPopUpWindow();

        bookPopUpWindow.setBook(book);
        bookPopUpWindow.setGiveCopyOfBook(giveCopyOfBook);
        bookPopUpWindow.setPreviousActivity(previousActivity);
        bookPopUpWindow.setBookGameObject(bookGameObject);

        bookPopUpWindow.populate();
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.inBookUI);
    }

    public override GameObject getCurrentPopUpGameObject()
    {
        if (BookPopUpWindow.getInstance() != null && !(BookPopUpWindow.getInstance() is null))
        {
            return BookPopUpWindow.getInstance().gameObject;
        }
        else
        {
            return null;
        }
    }

    public override bool shouldReturnToWalkingMode()
    {
        return false;
    }
}
