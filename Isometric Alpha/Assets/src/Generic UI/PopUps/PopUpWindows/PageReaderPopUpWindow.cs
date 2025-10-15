using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageReaderPopUpWindow : PopUpWindow
{
    public const string pageBridge = " Page ";
    private int pageNum = 1;

    private GameObject currentPageObject;

    public Button backButton;
    public Button nextButton;

    public Transform pageParent;

    public void populate()
    {
        setPageNum(getFirstPageNumber());   
        instantiatePage();
    }

    public int getCurrentPageNum()
    {
        return pageNum;
    }

    public void populateNextPage()
    {
        if (!hasMorePages())
        {
            return;
        }

        instantiatePage(getCurrentPageNum() + 1);
    }

    public void populatePreviousPage()
    {
        if (!hasPreviousPages())
        {
            return;
        }

        instantiatePage(getCurrentPageNum() - 1);
    }

    public void destroyCurrentPage()
    {
        if (currentPageObject != null && !(currentPageObject is null))
        {
            DestroyImmediate(currentPageObject);
        }
    }

    public void instantiatePage(int newPageNum)
    {
        setPageNum(newPageNum);

        instantiatePage();
    }

    public void setPageNum(int newPageNum)
    {
        pageNum = newPageNum;
        setButtonInteractability();
    }

    public void setButtonInteractability()
    {
        if (hasPreviousPages())
        {
            backButton.interactable = true;
        }
        else
        {
            backButton.interactable = false;
        }

        if (hasMorePages())
        {
            nextButton.interactable = true;
        }
        else
        {
            nextButton.interactable = false;
        }
    }

    public virtual int getFirstPageNumber()
    {
        return 0;
    }

    public virtual bool hasPreviousPages()
    {
        return getCurrentPageNum() > getFirstPageNumber();
    }

    public virtual bool hasMorePages()
    {
        GameObject testObject = getNextPageObject();

        if (testObject == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public virtual void setCurrentPageObject(GameObject newPageObject)
    {
        currentPageObject = newPageObject;
    }

    public virtual void instantiatePage()
    {
        destroyCurrentPage();

        setCurrentPageObject(Instantiate(getCurrentPageObject(), pageParent));
    }

    public virtual string getPageName(int pageNum)
    {
        return "";
    }
    
    public virtual GameObject getCurrentPageObject()
    {
        return Resources.Load<GameObject>(getPageName(getCurrentPageNum()));
    }

    public virtual GameObject getNextPageObject()
    {
        return Resources.Load<GameObject>(getPageName(getCurrentPageNum() + 1));
    }
}
