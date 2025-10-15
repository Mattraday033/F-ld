using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBookInfo : MonoBehaviour
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
        setUpBookManager(receivesBook, OOCActivity.inUI);
    }

    public void setUpBookManager(bool receivesBook, OOCActivity previousActivity)
    {
        getBook().use(PartyManager.getPlayerStats(), receivesBook, previousActivity, gameObject);
    }
    

}
