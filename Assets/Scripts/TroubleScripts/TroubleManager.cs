using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroubleManager : MonoBehaviour
{
    public static TroubleManager Instance;
    private List<IBuild> troubleObservers;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        troubleObservers = new List<IBuild>();
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
}
