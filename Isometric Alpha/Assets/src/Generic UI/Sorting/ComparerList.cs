using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface ISortable : IDescribable
{
    public int getQuantity();

    public int getWorth();

    public string getType();

    public string getSubtype();

    public int getLevel();

    public int getNumber();
}

public enum SortBy { Name = 1, Quantity = 2, Worth = 3, Type = 4, Level = 5, Number = 6, Eligibility = 7}

public static class ComparerList
{ 

    public static IComparer<ISortable> getComparer(SortBy sortBy)
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


public class NameComparer : IComparer<ISortable>
{
    public int Compare(ISortable x, ISortable y)
    {
        return x.getName().CompareTo(y.getName());
    }
}

public class QuantityComparer : IComparer<ISortable>
{
    public int Compare(ISortable x, ISortable y)
    {
        int comparisonInt = x.getQuantity() - y.getQuantity();

        if (comparisonInt != 0)
        {
            return comparisonInt;
        }
        else
        {
            return x.getName().CompareTo(y.getName());
        }
    }
}

public class WorthComparer : IComparer<ISortable>
{
    public int Compare(ISortable x, ISortable y)
    {
        int comparisonInt = x.getWorth() - y.getWorth();

        if (comparisonInt != 0)
        {
            return comparisonInt;
        }
        else
        {
            return x.getName().CompareTo(y.getName());
        }
    }
}

public class TypeComparer : IComparer<ISortable>
{
    public int Compare(ISortable x, ISortable y)
    {
        int comparisonInt = x.getType().CompareTo(y.getType());

        if (comparisonInt != 0)
        {
            return comparisonInt;
        }
        else
        {
            comparisonInt = x.getSubtype().CompareTo(y.getSubtype());

            if (comparisonInt != 0)
            {
                return comparisonInt;
            } else
            {
                return x.getName().CompareTo(y.getName());
            }
        }
    }
}

public class LevelComparer : IComparer<ISortable>
{
    public int Compare(ISortable x, ISortable y)
    {
        int comparisonInt = x.getLevel() - y.getLevel();

        if (comparisonInt != 0)
        {
            return comparisonInt;
        }
        else
        {
            return x.getName().CompareTo(y.getName());
        }
    }
}

public class NumberComparer : IComparer<ISortable> //NumberComparer is set to compare in descending order, so the largest/latest numbers are displayed first
{
    public int Compare(ISortable x, ISortable y)
    {
        int comparisonInt =  y.getNumber() - x.getNumber(); //y and x are flipped

        if (comparisonInt != 0)
        {
            return comparisonInt;
        }
        else
        {
            return x.getName().CompareTo(y.getName());
        }
    }
}

public class EligibilityComparer : IComparer<IDescribable> //EligibilityComparer is set to compare so that eligible items are sorted first
{
    public int Compare(IDescribable x, IDescribable y)
    {
        if (!x.ineligible() && y.ineligible())
        {
            return 1;
        } else if(x.ineligible() && !y.ineligible())
        {
            return -1;
        }
        else
        {
            return x.getName().CompareTo(y.getName());
        }
    }
}

