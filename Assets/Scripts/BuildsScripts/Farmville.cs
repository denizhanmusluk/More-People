﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Farmville : MonoBehaviour, IEmployeeDropping
{
    [SerializeField] int jobId = 3;
    [SerializeField] public Image outline, icon;
    [SerializeField] public TextMeshProUGUI employeCountText;
    [SerializeField] public int[] EmplCountforUpgrade;
    public Build build;
    public bool isFullCapacity = false;
    PlayerParent playerparent;

    private void Start()
    {

        build.Text1 = FindObjectOfType<GameManager>().farmerText.transform.parent.GetChild(0).GetComponent<TextMeshProUGUI>();
        build.hiringImage = FindObjectOfType<GameManager>().farmerText.transform.parent.GetChild(2).gameObject;

        //build.Text2 = FindObjectOfType<GameManager>().farmerText.transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>();
        build.buildNo = jobId;

        StartCoroutine(startDelay());

        if (PlayerPrefs.GetInt("farmvillelevel") != 0)
        {
            Globals.farmvilleLevel = PlayerPrefs.GetInt("farmvillelevel");
        }
        if (PlayerPrefs.GetInt("currentFarmerCount") != 0)
        {
            Globals.currentFarmerCount = PlayerPrefs.GetInt("currentFarmerCount");
        }

        build.buildInit(Globals.farmvilleLevel);
        Debug.Log("farmvillelevel" + Globals.farmvilleLevel);


        if (Globals.farmvilleLevel == build.levels.Count)
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
        outline.fillAmount = 0;
        employeCountText.text = Globals.currentFarmerCount.ToString() + "/" + EmplCountforUpgrade[Globals.farmvilleLevel].ToString();
        outline.fillAmount = (float)Globals.currentFarmerCount / (float)EmplCountforUpgrade[Globals.farmvilleLevel];
     
            GameManager.Instance.farmerText.transform.parent.gameObject.SetActive(true);
            GameManager.Instance.farmerText.text = employeCountText.text;


    }
    public void employeeDrop()
    {
        Globals.currentFarmerCount++;
        PlayerPrefs.SetInt("currentFarmerCount", Globals.currentFarmerCount);
        if (outline != null && employeCountText != null)
        {
            outline.fillAmount = (float)Globals.currentFarmerCount / (float)EmplCountforUpgrade[Globals.farmvilleLevel];
            employeCountText.text = Globals.currentFarmerCount.ToString() + "/" + EmplCountforUpgrade[Globals.farmvilleLevel].ToString();
            StartCoroutine(iconScaleSet());
        }
        if (Globals.currentFarmerCount == EmplCountforUpgrade[Globals.farmvilleLevel])
        {
            if (EmplCountforUpgrade.Length >= Globals.farmvilleLevel)
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
        Globals.farmvilleLevel++;
        PlayerPrefs.SetInt("farmvillelevel", Globals.farmvilleLevel);

        Globals.currentFarmerCount = 0;
        PlayerPrefs.SetInt("currentFarmerCount", Globals.currentFarmerCount);



        build.buildInit(Globals.farmvilleLevel);
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
                        if (i + 1 > (EmplCountforUpgrade[Globals.farmvilleLevel] - Globals.currentFarmerCount) && (Globals.farmvilleLevel == build.levels.Count - 1))
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
