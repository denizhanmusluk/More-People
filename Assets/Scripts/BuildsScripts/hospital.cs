using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class hospital : MonoBehaviour,IEmployeeDropping
{
    [SerializeField] int jobId = 1;
    [SerializeField] public Image outline;
    [SerializeField] public TextMeshProUGUI employeCountText;
    [SerializeField] public int[] EmplCountforUpgrade;
    public Build build;
    public bool isFullCapacity = false;
    private void Start()
    {
        outline.fillAmount = 0;

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

        employeCountText.text = Globals.currentDoctorCount.ToString() + "/" + EmplCountforUpgrade[Globals.hospitalLevel].ToString();
        outline.fillAmount = (float)Globals.currentDoctorCount / (float)EmplCountforUpgrade[Globals.hospitalLevel];

        if (Globals.hospitalLevel == build.levels.Count - 1)
        {
            isFullCapacity = true;
        }
    }
    public void employeeDrop()
    {
        Globals.currentDoctorCount++;
        PlayerPrefs.SetInt("currentDoctorCount", Globals.currentDoctorCount);
        outline.fillAmount = (float)Globals.currentDoctorCount / (float)EmplCountforUpgrade[Globals.hospitalLevel];

        employeCountText.text = Globals.currentDoctorCount.ToString() + "/" + EmplCountforUpgrade[Globals.hospitalLevel].ToString();

        if (Globals.currentDoctorCount == EmplCountforUpgrade[Globals.hospitalLevel])
        {
          
                Destroy(build.loadedBuild);
                hospitalLevelUp();
        }
    }
 
    void hospitalLevelUp()
    {
        Globals.hospitalLevel++;
        PlayerPrefs.SetInt("hospitalLevel", Globals.hospitalLevel);

        Globals.currentDoctorCount = 0;
        PlayerPrefs.SetInt("currentDoctorCount", Globals.currentDoctorCount);

        outline.fillAmount = 0;


        build.buildInit(Globals.hospitalLevel);

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerParent playerparent = other.transform.parent.GetComponent<PlayerParent>();
            for(int i = 1; i< playerparent.humans.Count; i++)
            {
                if(jobId == playerparent.humans[i].GetComponent<Employee>().jobId)
                {
                    if (!isFullCapacity)
                    {
                        if ( i + 1>  (EmplCountforUpgrade[Globals.hospitalLevel] - Globals.currentDoctorCount) && (Globals.hospitalLevel == build.levels.Count - 2))
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
