﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PoliceStation : MonoBehaviour, IEmployeeDropping
{
    [SerializeField] int jobId = 2;
    [SerializeField] public Image outline, icon;
    [SerializeField] public TextMeshProUGUI employeCountText;
    [SerializeField] public int[] EmplCountforUpgrade;
    public Build build;
    public bool isFullCapacity = false;
    PlayerParent playerparent;
    //public TextMeshProUGUI policeText1, policeText2;
    private void Start()
    {
        build.Text1 = FindObjectOfType<GameManager>().policeText.transform.parent.GetChild(0).GetComponent<TextMeshProUGUI>();
        build.hiringImage = FindObjectOfType<GameManager>().policeText.transform.parent.GetChild(2).gameObject;

        //build.Text2 = FindObjectOfType<GameManager>().policeText.transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>();
        build.buildNo = jobId;

        StartCoroutine(startDelay());


        if (PlayerPrefs.GetInt("policeStationLevel") != 0)
        {
            Globals.policeStationLevel = PlayerPrefs.GetInt("policeStationLevel");
        }
        if (PlayerPrefs.GetInt("currentPoliceCount") != 0)
        {
            Globals.currentPoliceCount = PlayerPrefs.GetInt("currentPoliceCount");
        }

        build.buildInit(Globals.policeStationLevel);



        if (Globals.policeStationLevel == build.levels.Count)
        {
            isFullCapacity = true;
        }
    }
    IEnumerator startDelay()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (var img in GetComponentsInChildren<Image>())
        {
            if (img.transform.name == "OutLine")
            {
                outline = img;
            }
            if (img.transform.name == "icon")
            {
                icon = img;
            }
        }
        foreach (var txt in GetComponentsInChildren<TextMeshProUGUI>())
        {
            employeCountText = txt;
        }
        employeCountText.text = Globals.currentPoliceCount.ToString() + "/" + EmplCountforUpgrade[Globals.policeStationLevel].ToString();
        outline.fillAmount = (float)Globals.currentPoliceCount / (float)EmplCountforUpgrade[Globals.policeStationLevel];

        GameManager.Instance.policeText.transform.parent.gameObject.SetActive(true);
        GameManager.Instance.policeText.text = employeCountText.text;

     
    }
    public void employeeDrop()
    {
        Globals.currentPoliceCount++;
        PlayerPrefs.SetInt("currentPoliceCount", Globals.currentPoliceCount);
        if (outline != null && employeCountText != null)
        {
            outline.fillAmount = (float)Globals.currentPoliceCount / (float)EmplCountforUpgrade[Globals.policeStationLevel];
            employeCountText.text = Globals.currentPoliceCount.ToString() + "/" + EmplCountforUpgrade[Globals.policeStationLevel].ToString();
            StartCoroutine(iconScaleSet());
        }
        if (Globals.currentPoliceCount == EmplCountforUpgrade[Globals.policeStationLevel])
        {
            //levelUp
            if (EmplCountforUpgrade.Length - 1 > Globals.policeStationLevel)
            {
                Destroy(build.loadedBuild);
                hospitalLevelUp();
            }
        }

        StartCoroutine(targetSelectDelay());
    }
    IEnumerator targetSelectDelay()
    {
        yield return new WaitForSeconds(0.1f);
        playerparent.UItargetSelect();

    }
    void hospitalLevelUp()
    {

        Globals.policeStationLevel++;
        PlayerPrefs.SetInt("policeStationLevel", Globals.policeStationLevel);

        Globals.currentPoliceCount = 0;
        PlayerPrefs.SetInt("currentPoliceCount", Globals.currentPoliceCount);



        build.buildInit(Globals.policeStationLevel);
        StartCoroutine(startDelay());

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerparent = other.transform.parent.GetComponent<PlayerParent>();
            for (int i = 1; i < playerparent.humans.Count; i++)
            {
                if (jobId == playerparent.humans[i].GetComponent<Employee>().jobId)
                {
                    if (!isFullCapacity)
                    {
                        if (i + 1 > (EmplCountforUpgrade[Globals.policeStationLevel] - Globals.currentPoliceCount) && (Globals.policeStationLevel == build.levels.Count - 1))
                        {
                            isFullCapacity = true;

                        }

                        Transform employe = playerparent.humans[i];
                        employe.GetComponent<Employee>().employeDropping(transform);
                        //playerparent.humans.Remove(employe);
                        //employeeDrop();
                    }
                }
            }
        }
    }

    IEnumerator iconScaleSet()
    {
        float counter1 = 0f;
        float scaleValue1 = 0f;

        while (counter1 < Mathf.PI)
        {
            counter1 += 40 * Time.deltaTime;
            scaleValue1 = 1 - Mathf.Abs(Mathf.Cos(counter1));
            icon.transform.localScale = Vector3.one + new Vector3(scaleValue1 / 5f, scaleValue1 / 5f, scaleValue1 / 5f);
            yield return null;
        }
        icon.transform.localScale = Vector3.one;
    }
}
