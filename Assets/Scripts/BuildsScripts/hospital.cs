using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class hospital : MonoBehaviour,IEmployeeDropping
{
    [SerializeField] public int jobId = 1;
    [SerializeField] public Image outline, icon;
    [SerializeField] public TextMeshProUGUI employeCountText;
    [SerializeField] public int[] EmplCountforUpgrade;
    public Build build;
    public bool isFullCapacity = false;
    PlayerParent playerparent;
    private void Start()
    {
        build.Text1 = FindObjectOfType<GameManager>().doctorText.transform.parent.GetChild(0).GetComponent<TextMeshProUGUI>();
        build.hiringImage = FindObjectOfType<GameManager>().doctorText.transform.parent.GetChild(2).gameObject;
        //build.Text2 = FindObjectOfType<GameManager>().doctorText.transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>();
        build.buildNo = jobId;
        StartCoroutine(startDelay());

        if (PlayerPrefs.GetInt("hospitalLevel") != 0)
        {
            Globals.hospitalLevel = PlayerPrefs.GetInt("hospitalLevel");
        }
        if (PlayerPrefs.GetInt("currentDoctorCount") != 0)
        {
            Globals.currentDoctorCount = PlayerPrefs.GetInt("currentDoctorCount");
        }

        build.buildInit(Globals.hospitalLevel);
        Debug.Log("hospitalLevel" + Globals.hospitalLevel);



        if (Globals.hospitalLevel == build.levels.Count)
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
        employeCountText.text = Globals.currentDoctorCount.ToString() + "/" + EmplCountforUpgrade[Globals.hospitalLevel].ToString();
        outline.fillAmount = (float)Globals.currentDoctorCount / (float)EmplCountforUpgrade[Globals.hospitalLevel];

            GameManager.Instance.doctorText.transform.parent.gameObject.SetActive(true);
            GameManager.Instance.doctorText.text = employeCountText.text;
   
    }
    public void employeeDrop()
    {
        Globals.currentDoctorCount++;
        PlayerPrefs.SetInt("currentDoctorCount", Globals.currentDoctorCount);
        if (outline != null && employeCountText != null)
        {
            outline.fillAmount = (float)Globals.currentDoctorCount / (float)EmplCountforUpgrade[Globals.hospitalLevel];

            employeCountText.text = Globals.currentDoctorCount.ToString() + "/" + EmplCountforUpgrade[Globals.hospitalLevel].ToString();
            StartCoroutine(iconScaleSet());

        }
        if (Globals.currentDoctorCount == EmplCountforUpgrade[Globals.hospitalLevel])
        {
            if (EmplCountforUpgrade.Length - 1 > Globals.hospitalLevel)
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
        Globals.hospitalLevel++;
        PlayerPrefs.SetInt("hospitalLevel", Globals.hospitalLevel);

        Globals.currentDoctorCount = 0;
        PlayerPrefs.SetInt("currentDoctorCount", Globals.currentDoctorCount);



        build.buildInit(Globals.hospitalLevel);
        StartCoroutine(startDelay());

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerparent = other.transform.parent.GetComponent<PlayerParent>();
            for(int i = 1; i< playerparent.humans.Count; i++)
            {
                if(jobId == playerparent.humans[i].GetComponent<Employee>().jobId)
                {
                    if (!isFullCapacity)
                    {
                        if ( i + 1>  (EmplCountforUpgrade[Globals.hospitalLevel] - Globals.currentDoctorCount) && (Globals.hospitalLevel == build.levels.Count - 1))
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
