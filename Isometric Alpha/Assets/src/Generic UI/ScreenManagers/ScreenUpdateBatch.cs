using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Showing a prebuilt screen sets off several events that each tell the same pieces to refresh: the interior update, the default
//side tab being chosen, the party strip choosing its member. While those go out, a piece inside the screen puts its refresh
//off rather than doing it, and each one is then done once, after the last event, with everything those events decided in place
public static class ScreenUpdateBatch
{
    //the screen whose refreshes are being put off, and null whenever none are
    private static ScreenManager collectingFor;

    private static readonly List<UnityAction> refreshesPutOff = new List<UnityAction>();

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        collectingFor = null;
        refreshesPutOff.Clear();
    }

    //true when the refresh has been put off, which leaves the caller nothing to do for now.
    //Only for a refresh that takes no argument and reads what it needs when it runs, since it runs later and once
    public static bool putOff(ScreenManager owner, UnityAction refresh)
    {
        //a piece with no screen of its own, like the party strip, always refreshes on the spot
        if (collectingFor == null || owner != collectingFor)
        {
            return false;
        }

        if (!refreshesPutOff.Contains(refresh))
        {
            refreshesPutOff.Add(refresh);
        }

        return true;
    }

    //the refreshes are done in the order they were first asked for, which is the order the pieces would have refreshed in anyway
    public static void run(ScreenManager screen, Action events)
    {
        //already inside a batch, so these events join it
        if (collectingFor != null)
        {
            events();

            return;
        }

        List<UnityAction> refreshes = new List<UnityAction>();

        try
        {
            collectingFor = screen;

            events();
        }
        finally
        {
            //closed before any refresh runs, so that the events a refresh sets off itself are acted on straight away as they always were
            collectingFor = null;

            refreshes.AddRange(refreshesPutOff);
            refreshesPutOff.Clear();
        }

        foreach (UnityAction refresh in refreshes)
        {
            try
            {
                refresh();
            }
            catch (Exception exception)
            {
                //one piece failing to refresh shouldn't leave the rest of the screen stale
                Debug.LogException(exception);
            }
        }
    }
}
