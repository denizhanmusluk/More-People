using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class environmentParticles : MonoBehaviour,ITroubleFix, IisTrouble
{
    public int buildNo;
    private void Start()
    {
        TroubleManager.Instance.Add_TroubleFixObserver(this);
        TroubleManager.Instance.Add_isTroubleObserver(this);
    }
    public  void torubleFix()
    {
        if (buildNo == 2)
        {
            foreach (var chld in GetComponentsInChildren<GameObject>())
            {
                chld.SetActive(false);
            }
            TroubleManager.Instance.Remove_TroubleFixObserver(this);
        }
    }
    public void isTrouble()
    {
        if (buildNo == 2)
        {
            foreach (var chld in GetComponentsInChildren<GameObject>())
            {
                chld.SetActive(true);
            }
            TroubleManager.Instance.Remove_isTroubleObserver(this);
        }
    }
}
