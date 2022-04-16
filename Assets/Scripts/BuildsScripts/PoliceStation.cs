using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PoliceStation : MonoBehaviour, IEmployeeDropping
{
    [SerializeField] int jobId = 2;
    [SerializeField] public Image outline;
    [SerializeField] public TextMeshProUGUI employeCountText;
    [SerializeField] public int[] EmplCountforUpgrade;
    public Build build;
    public bool isFullCapacity = false;
    private void Start()
    {
        outline.fillAmount = 0;

        if (PlayerPrefs.GetInt("policeStationLevel") != 0)
        {
            Globals.policeStationLevel = PlayerPrefs.GetInt("policeStationLevel");
        }
        if (PlayerPrefs.GetInt("currentPoliceCount") != 0)
        {
            Globals.currentPoliceCount = PlayerPrefs.GetInt("currentPoliceCount");
        }

        build.buildInit(Globals.policeStationLevel);

        employeCountText.text = Globals.currentPoliceCount.ToString() + "/" + EmplCountforUpgrade[Globals.policeStationLevel].ToString();
        outline.fillAmount = (float)Globals.currentPoliceCount / (float)EmplCountforUpgrade[Globals.policeStationLevel];

        if (Globals.policeStationLevel == build.levels.Count - 1)
        {
            isFullCapacity = true;
        }
    }
    public void employeeDrop()
    {
        Globals.currentPoliceCount++;
        PlayerPrefs.SetInt("currentPoliceCount", Globals.currentPoliceCount);
        outline.fillAmount = (float)Globals.currentPoliceCount / (float)EmplCountforUpgrade[Globals.policeStationLevel];

        employeCountText.text = Globals.currentPoliceCount.ToString() + "/" + EmplCountforUpgrade[Globals.policeStationLevel].ToString();

        if (Globals.currentPoliceCount == EmplCountforUpgrade[Globals.policeStationLevel])
        {
            //levelUp
            Destroy(build.loadedBuild);
            hospitalLevelUp();
        }


    }

    void hospitalLevelUp()
    {
        Globals.policeStationLevel++;
        PlayerPrefs.SetInt("policeStationLevel", Globals.policeStationLevel);

        Globals.currentPoliceCount = 0;
        PlayerPrefs.SetInt("currentPoliceCount", Globals.currentPoliceCount);

        outline.fillAmount = 0;


        build.buildInit(Globals.policeStationLevel);

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
                        if (i + 1 > (EmplCountforUpgrade[Globals.policeStationLevel] - Globals.currentPoliceCount) && (Globals.policeStationLevel == build.levels.Count - 2))
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
