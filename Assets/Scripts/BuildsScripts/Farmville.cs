using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Farmville : MonoBehaviour, IEmployeeDropping
{
    [SerializeField] int jobId = 3;
    [SerializeField] public Image outline;
    [SerializeField] public TextMeshProUGUI employeCountText;
    [SerializeField] public int[] EmplCountforUpgrade;
    public Build build;
    public bool isFullCapacity = false;
    private void Start()
    {
        outline.fillAmount = 0;

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

        employeCountText.text = Globals.currentFarmerCount.ToString() + "/" + EmplCountforUpgrade[Globals.farmvilleLevel].ToString();
        outline.fillAmount = (float)Globals.currentFarmerCount / (float)EmplCountforUpgrade[Globals.farmvilleLevel];

        if (Globals.farmvilleLevel == build.levels.Count - 1)
        {
            isFullCapacity = true;
        }
    }
    public void employeeDrop()
    {
        Globals.currentFarmerCount++;
        PlayerPrefs.SetInt("currentFarmerCount", Globals.currentFarmerCount);
        outline.fillAmount = (float)Globals.currentFarmerCount / (float)EmplCountforUpgrade[Globals.farmvilleLevel];

        employeCountText.text = Globals.currentFarmerCount.ToString() + "/" + EmplCountforUpgrade[Globals.farmvilleLevel].ToString();

        if (Globals.currentFarmerCount == EmplCountforUpgrade[Globals.farmvilleLevel])
        {

            Destroy(build.loadedBuild);
            hospitalLevelUp();
        }
    }

    void hospitalLevelUp()
    {
        Globals.farmvilleLevel++;
        PlayerPrefs.SetInt("farmvillelevel", Globals.farmvilleLevel);

        Globals.currentFarmerCount = 0;
        PlayerPrefs.SetInt("currentFarmerCount", Globals.currentFarmerCount);

        outline.fillAmount = 0;


        build.buildInit(Globals.farmvilleLevel);

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
                        if (i + 1 > (EmplCountforUpgrade[Globals.farmvilleLevel] - Globals.currentFarmerCount) && (Globals.farmvilleLevel == build.levels.Count - 2))
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
