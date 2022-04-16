using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Build : MonoBehaviour,IBuild
{
    [SerializeField] public List<GameObject> levels;
    public GameObject loadedBuild;
    float firstScale;
    public int thisBuildingLevel;
    [SerializeField] public List<GameObject> customerList;
    public bool troubleActive = false;

    private void Start()
    {
        TroubleManager.Instance.Add_TroubleObserver(this);
    }
    public void buildInit(int level)
    {
        thisBuildingLevel = level;
    

        loadedBuild = Instantiate(levels[level], transform.position, transform.rotation);
        loadedBuild.transform.parent = transform;
        for(int i = 0; i< SpawnManager.Instance.transform.childCount; i++)
        {
            SpawnManager.Instance.transform.GetChild(i).GetComponent<NPCSpawner>().target.Add(transform.GetChild(0).transform);
        }
        //SpawnManager.Instance.target.Add(transform.GetChild(0).transform);

        StartCoroutine(buildScaling());
    }
    public void troubleCheck()
    {
        //Debug.Log("Globals.maxBuildLevel" + Globals.maxBuildLevel);
        //Debug.Log("thisBuildingLevel" + thisBuildingLevel);
        if(Globals.maxBuildLevel - 1 > thisBuildingLevel)
        {
            troubleActive = true;
            //trouble this build
            Debug.Log("trouble " + transform.name);
            for(int i = 0; i < customerList.Count; i++)
            {
                Debug.Log("trouble human" + customerList[i].transform.name);
                customerList[i].GetComponent<NPC>().currentSelection = NPC.States.trouble;
            }
        }
        //TroubleManager.Instance.Remove_TroubleObserver(this);

    }
    IEnumerator buildScaling()
    {
        int buildChildCount = loadedBuild.transform.childCount;
        for (int i = 0; i < buildChildCount; i++)
        {
            StartCoroutine(throughlyScaling(loadedBuild.transform.GetChild(i).transform));
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(thisBuildingLevel);
        Debug.Log("Globals.maxBuildLevel" + Globals.maxBuildLevel);
        Debug.Log("thisBuildingLevel" + thisBuildingLevel);
        if (Globals.maxBuildLevel < thisBuildingLevel)
        {
            Globals.maxBuildLevel = thisBuildingLevel;
        }
        TroubleManager.Instance.Notify_GameTroubleObservers();

    }
    // Update is called once per frame
    IEnumerator throughlyScaling(Transform bld)
    {
        float counter = 0f;
        float firstSize = 1f;
        float sizeDelta;

        while(counter < 1f)
        {
            counter += 15 * Time.deltaTime;

            bld.localScale = new Vector3(counter, counter, counter);
            yield return null;
        }
        bld.localScale = new Vector3(firstSize, firstSize, firstSize);
        counter = 0f;
        while (counter < Mathf.PI)
        {
            counter += 15 * Time.deltaTime;
            sizeDelta = 1f - Mathf.Abs(Mathf.Cos(counter));
            sizeDelta /= 3f;
            bld.localScale = new Vector3(firstSize + sizeDelta, firstSize + sizeDelta, firstSize + sizeDelta);

            yield return null;
        }
        bld.localScale = new Vector3(firstSize, firstSize, firstSize);
    
    }
}
