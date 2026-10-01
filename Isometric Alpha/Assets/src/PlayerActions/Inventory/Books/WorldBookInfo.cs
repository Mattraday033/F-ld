using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBookInfo : MonoBehaviour, INameSource
{
    public const bool giveCopyOfBook = true;
    public const bool doNotGiveCopyOfBook = true;
    public int bookIndex;

    private BookItem getBook()
    {
        return (BookItem)ItemList.getItem(ItemList.bookListIndex, bookIndex, 1);
    }
    
    public void setUpBookManager(bool receivesBook)
    {
        setUpBookManager(receivesBook, CurrentActivity.InUI);
    }

    public void setUpBookManager(bool receivesBook, CurrentActivity previousActivity)
    {
        getBook().use(PartyManager.getPlayerStats(), receivesBook, previousActivity, gameObject);
    }
    
    public string displayName { get { return getBook().displayName; } }
    public string uniqueName { get { return getBook().uniqueName; } }

}
