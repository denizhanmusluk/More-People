using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroubleManager : MonoBehaviour
{
    public static TroubleManager Instance;
    private List<IBuild> troubleObservers;
    private List<ITroubleFix> troubleFixObservers;
    private List<IisTrouble> isTroubleObservers;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        troubleObservers = new List<IBuild>();
        troubleFixObservers = new List<ITroubleFix>();
        isTroubleObservers = new List<IisTrouble>();
    }

    #region Trouble Observer
    public void Add_TroubleObserver(IBuild observer)
    {
        troubleObservers.Add(observer);
    }

    public void Remove_TroubleObserver(IBuild observer)
    {
        troubleObservers.Remove(observer);
    }

    public void Notify_GameTroubleObservers()
    {
        foreach (IBuild observer in troubleObservers.ToArray())
        {
            if (troubleObservers.Contains(observer))
                observer.troubleCheck();
        }
    }
    #endregion

    #region Trouble Observer
    public void Add_TroubleFixObserver(ITroubleFix observer)
    {
        troubleFixObservers.Add(observer);
    }

    public void Remove_TroubleFixObserver(ITroubleFix observer)
    {
        troubleFixObservers.Remove(observer);
    }

    public void Notify_GameTroubleFixObservers()
    {
        foreach (ITroubleFix observer in troubleFixObservers.ToArray())
        {
            if (troubleFixObservers.Contains(observer))
                observer.torubleFix();
        }
    }
    #endregion


    #region Trouble Observer
    public void Add_isTroubleObserver(IisTrouble observer)
    {
        isTroubleObservers.Add(observer);
    }

    public void Remove_isTroubleObserver(IisTrouble observer)
    {
        isTroubleObservers.Remove(observer);
    }

    public void Notify_isTroubleObservers()
    {
        foreach (IisTrouble observer in troubleFixObservers.ToArray())
        {
            if (isTroubleObservers.Contains(observer))
                observer.isTrouble();
        }
    }
    #endregion
}
