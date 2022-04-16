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
    private void Start()
    {
        outline.fillAmount = 0;

        if (PlayerPrefs.GetInt("universityLevel") != 0)
        {
            Globals.universityLevel = PlayerPrefs.GetInt("universityLevel");
        }
        if (PlayerPrefs.GetInt("currentTeacherCount") != 0)
        {
            Globals.currentTeacherCount = PlayerPrefs.GetInt("currentTeacherCount");
        }

        build.buildInit(Globals.universityLevel);
        Debug.Log("universityLevel" + Globals.universityLevel);

        employeCountText.text = Globals.currentTeacherCount.ToString() + "/" + EmplCountforUpgrade[Globals.universityLevel].ToString();
        outline.fillAmount = (float)Globals.currentTeacherCount / (float)EmplCountforUpgrade[Globals.universityLevel];

        if (Globals.universityLevel == build.levels.Count - 1)
        {
            isFullCapacity = true;
        }
    }
    public void employeeDrop()
    {
        Globals.currentTeacherCount++;
        PlayerPrefs.SetInt("currentTeacherCount", Globals.currentTeacherCount);
        outline.fillAmount = (float)Globals.currentTeacherCount / (float)EmplCountforUpgrade[Globals.universityLevel];

        employeCountText.text = Globals.currentTeacherCount.ToString() + "/" + EmplCountforUpgrade[Globals.universityLevel].ToString();

        if (Globals.currentTeacherCount == EmplCountforUpgrade[Globals.universityLevel])
        {

            Destroy(build.loadedBuild);
            hospitalLevelUp();
        }
    }

    void hospitalLevelUp()
    {
        Globals.universityLevel++;
        PlayerPrefs.SetInt("universityLevel", Globals.universityLevel);

        Globals.currentTeacherCount = 0;
        PlayerPrefs.SetInt("currentTeacherCount", Globals.currentTeacherCount);

        outline.fillAmount = 0;


        build.buildInit(Globals.universityLevel);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerParent playerparent = other.transform.parent.GetComponent<PlayerParent>();
            for (int i = 1; i < playerparent.humans.Count; i++)
            {
                if (jobId == playerparent.humans[i].GetComponent<Employee>().jobId)
                {
                    if (!isFullCapacity)
                    {
                        if (i + 1 > (EmplCountforUpgrade[Globals.universityLevel] - Globals.currentTeacherCount) && (Globals.universityLevel == build.levels.Count - 2))
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
