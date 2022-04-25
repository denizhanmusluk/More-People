﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class viewTroubledBuild : MonoBehaviour, IisTrouble, IRunner, IFinish
{
    [SerializeField] CinemachineVirtualCamera[] buildCams;
    bool troubleActive = false;
    bool coroutineActive = true;
    void Start()
    {
        TroubleManager.Instance.Add_isTroubleObserver(this);
        GameManager.Instance.Add_RunnerStartObserver(this);
        GameManager.Instance.Add_FinishObserver(this);
    }
    public void finishRunner()
    {
        troubleActive = true;
    }
    public void runnerStar()
    {
        Globals.troubleBuildNo = 0;
        GameManager.Instance.Remove_FinishObserver(this);
        GameManager.Instance.Remove_RunnerStartObserver(this);
        TroubleManager.Instance.Remove_isTroubleObserver(this);

        troubleActive = false;

    }
    public void isTrouble()
    {
        if (Globals.troubleBuildNo > 0)
        {
            StartCoroutine(troubledBuildCheck());
        }
    }
    IEnumerator troubledBuildCheck()
    {
        while (coroutineActive)
        {
            if (troubleActive)
            {
                coroutineActive = false;
                troubleActive = false;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        buildCams[Globals.troubleBuildNo-1].Priority = 30;
        yield return new WaitForSeconds(2f);
        buildCams[Globals.troubleBuildNo-1].Priority = 0;
        coroutineActive = true;
        troubleActive = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (Globals.troubleBuildNo > 0)
            {
                StartCoroutine(troubledBuildReview());
            }
        }
    }
    IEnumerator troubledBuildReview()
    {
        while (coroutineActive)
        {
            if (troubleActive)
            {
                coroutineActive = false;
                troubleActive = false;
            }
            yield return null;
        }
        buildCams[Globals.troubleBuildNo-1].Priority = 30;
        yield return new WaitForSeconds(1.5f);
        buildCams[Globals.troubleBuildNo-1].Priority = 0;
        coroutineActive = true;
        troubleActive = true;
    }

}
