using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class University : MonoBehaviour, IEmployeeDropping
{
    [SerializeField] int jobId = 4;
    [SerializeField] public Image outline;
    [SerializeField] public TextMeshProUGUI employeCountText;
    [SerializeField] public int[] EmplCountforUpgrade;
    public Build build;
    public bool isFullCapacity = false;
    PlayerParent playerparent;
    private void Start()
    {
        build.Text1 = FindObjectOfType<GameManager>().teacheText.transform.parent.GetChild(0).GetComponent<TextMeshProUGUI>();
        build.hiringImage = FindObjectOfType<GameManager>().teacheText.transform.parent.GetChild(2).gameObject;

        //build.Text2 = FindObjectOfType<GameManager>().teacheText.transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>();
        build.buildNo = jobId;

        StartCoroutine(startDelay());

        if (PlayerPrefs.GetInt("universityLevel") != 0)
        {
            Globals.universityLevel = PlayerPrefs.GetInt("universityLevel");
        }
        if (PlayerPrefs.GetInt("currentTeacherCount") != 0)
        {
            Globals.currentTeacherCount = PlayerPrefs.GetInt("currentTeacherCount");
        }

        build.buildInit(Globals.universityLevel);


        if (Globals.universityLevel == build.levels.Count)
        {
            isFullCapacity = true;
        }
    }
    IEnumerator startDelay()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (var img in GetComponentsInChildren<Image>())
        {
            outline = img;
        }
        foreach (var txt in GetComponentsInChildren<TextMeshProUGUI>())
        {
            employeCountText = txt;
        }
        employeCountText.text = Globals.currentTeacherCount.ToString() + "/" + EmplCountforUpgrade[Globals.universityLevel].ToString();
        outline.fillAmount = (float)Globals.currentTeacherCount / (float)EmplCountforUpgrade[Globals.universityLevel];

            GameManager.Instance.teacheText.transform.parent.gameObject.SetActive(true);
            GameManager.Instance.teacheText.text = employeCountText.text;

    
    }
    public void employeeDrop()
    {
        Globals.currentTeacherCount++;
        PlayerPrefs.SetInt("currentTeacherCount", Globals.currentTeacherCount);
        if (outline != null && employeCountText != null)
        {
            outline.fillAmount = (float)Globals.currentTeacherCount / (float)EmplCountforUpgrade[Globals.universityLevel];
            employeCountText.text = Globals.currentTeacherCount.ToString() + "/" + EmplCountforUpgrade[Globals.universityLevel].ToString();
        }
        if (Globals.currentTeacherCount == EmplCountforUpgrade[Globals.universityLevel])
        {
            if (EmplCountforUpgrade.Length >= Globals.universityLevel)
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
        Globals.universityLevel++;
        PlayerPrefs.SetInt("universityLevel", Globals.universityLevel);

        Globals.currentTeacherCount = 0;
        PlayerPrefs.SetInt("currentTeacherCount", Globals.currentTeacherCount);



        build.buildInit(Globals.universityLevel);
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
                        if (i + 1 > (EmplCountforUpgrade[Globals.universityLevel] - Globals.currentTeacherCount) && (Globals.universityLevel == build.levels.Count - 1))
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
}
