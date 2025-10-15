using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface ISortable
{
    public string getName();

    public int getQuantity();

    public int getWorth();

    public string getType();

    public string getSubtype();

    public int getLevel();

    public int getNumber();

    public bool ineligible();
}

public enum SortBy { Name = 1, Quantity = 2, Worth = 3, Type = 4, Level = 5, Number = 6, Eligibility = 7}

public static class ComparerList
{ 

    public static IComparer getComparer(SortBy sortBy)
    {
        switch (sortBy)
        {
            case SortBy.Name:
                return new NameComparer();
            case SortBy.Quantity:
                return new QuantityComparer();
            case SortBy.Worth:
                return new WorthComparer();
            case SortBy.Type:
                return new TypeComparer();
            case SortBy.Level:
                return new LevelComparer();
            case SortBy.Number:
                return new NumberComparer();
            case SortBy.Eligibility:
                return new EligibilityComparer();
            default:
                throw new IOException("Unimplemented SortBy: " + sortBy.ToString());
        }
    }

}


public class NameComparer : IComparer
{
    public int Compare(object x, object y)
    {
        IDescribable xDescribable = (IDescribable) x;
        IDescribable yDescribable = (IDescribable) y;

        return xDescribable.getName().CompareTo(yDescribable.getName());
    }
}

public class QuantityComparer : IComparer
{
    public int Compare(object x, object y)
    {
        ISortable xSortable = (ISortable) x;
        ISortable ySortable = (ISortable) y;

        int comparisonInt = xSortable.getQuantity() - ySortable.getQuantity();

        if (comparisonInt != 0)
        {
            return comparisonInt;
        }
        else
        {
            return xSortable.getName().CompareTo(ySortable.getName());
        }
    }
}

public class WorthComparer : IComparer
{
    public int Compare(object x, object y)
    {
        ISortable xSortable = (ISortable) x;
        ISortable ySortable = (ISortable) y;

        int comparisonInt = xSortable.getWorth() - ySortable.getWorth();

        if (comparisonInt != 0)
        {
            return comparisonInt;
        }
        else
        {
            return xSortable.getName().CompareTo(ySortable.getName());
        }
    }
}

public class TypeComparer : IComparer
{
    public int Compare(object x, object y)
    {
        ISortable xSortable = (ISortable) x;
        ISortable ySortable = (ISortable) y;

        int comparisonInt = xSortable.getType().CompareTo(ySortable.getType());

        if (comparisonInt != 0)
        {
            return comparisonInt;
        }
        else
        {
            comparisonInt = xSortable.getSubtype().CompareTo(ySortable.getSubtype());

            if (comparisonInt != 0)
            {
                return comparisonInt;
            } else
            {
                return xSortable.getName().CompareTo(ySortable.getName());
            }
        }
    }
}

public class LevelComparer : IComparer
{
    public int Compare(object x, object y)
    {
        ISortable xSortable = (ISortable) x;
        ISortable ySortable = (ISortable) y;

        int comparisonInt = xSortable.getLevel() - ySortable.getLevel();

        if (comparisonInt != 0)
        {
            return comparisonInt;
        }
        else
        {
            return xSortable.getName().CompareTo(ySortable.getName());
        }
    }
}

public class NumberComparer : IComparer //NumberComparer is set to compare in descending order, so the largest/latest numbers are displayed first
{
    public int Compare(object x, object y)
    {
        ISortable xSortable = (ISortable) x;
        ISortable ySortable = (ISortable) y;

        int comparisonInt =  ySortable.getNumber() - xSortable.getNumber(); //y and x are flipped

        if (comparisonInt != 0)
        {
            return comparisonInt;
        }
        else
        {
            return xSortable.getName().CompareTo(ySortable.getName());
        }
    }
}

public class EligibilityComparer : IComparer //EligibilityComparer is set to compare so that eligible items are sorted first
{
    public int Compare(object x, object y)
    {
        ISortable xSortable = (ISortable) x;
        ISortable ySortable = (ISortable) y;

        if (!xSortable.ineligible() && ySortable.ineligible())
        {
            return 1;
        } else if(xSortable.ineligible() && !ySortable.ineligible())
        {
            return -1;
        }
        else
        {
            return xSortable.getName().CompareTo(ySortable.getName());
        }
    }
}

