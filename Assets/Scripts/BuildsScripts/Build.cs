using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;


public class Build : MonoBehaviour,IBuild
{
    [SerializeField] public List<GameObject> levels;
    public GameObject loadedBuild;
    float firstScale;
    public int thisBuildingLevel;
    [SerializeField] public List<GameObject> customerList;
    public bool troubleActive = false;
    public int buildNo;

    bool colorIndicate = false;

    public TextMeshProUGUI Text1, Text2;
    Sequence sequence ,sequence2;
    private void Start()
    {
        TroubleManager.Instance.Add_TroubleObserver(this);

    }
    private void Awake()
    {
 
        sequence = DOTween.Sequence();
        sequence2 = DOTween.Sequence();

    }
    public void buildInit(int level)
    {
        thisBuildingLevel = level;
    

        loadedBuild = Instantiate(levels[level], transform.position, transform.rotation);
        loadedBuild.transform.parent = transform;
        for(int i = 0; i < FindObjectOfType<SpawnManager>().transform.childCount; i++)
        {
            //SpawnManager.Instance.transform.GetChild(i).GetComponent<NPCSpawner>().target.Add(transform.GetChild(0).transform);
            FindObjectOfType<SpawnManager>().transform.GetChild(i).GetComponent<NPCSpawner>().target.Add(transform.GetChild(0).transform);
            //SpawnManager.Instance.transform.GetChild(i).GetComponent<NPCSpawner>().target.Add(transform.GetChild(0).transform);
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
            colorIndicate = true;
            troubleActive = true;
            //trouble this build
            Debug.Log("trouble " + transform.name);
            for(int i = 0; i < customerList.Count; i++)
            {
                Debug.Log("trouble human" + customerList[i].transform.name);
                //customerList[i].GetComponent<NPC>().currentSelection = NPC.States.trouble;
                if (buildNo == 1)
                {
                    customerList[i].GetComponent<NPC>().currentSelection = NPC.States.troubleHospital;
                    customerList[i].GetComponent<NPC>()._randomEmoji();

                }
                if (buildNo == 2)
                {
                    customerList[i].GetComponent<NPC>().currentSelection = NPC.States.troublePolice;
                    customerList[i].GetComponent<NPC>()._randomDead();
                }
                if (buildNo == 3)
                {
                    customerList[i].GetComponent<NPC>().currentSelection = NPC.States.troubleFarm;

                }
                if (buildNo == 4)
                {

                }
            }
            textColorSet();

        }
        else
        {
            sequence.Kill();
            sequence2.Kill();
            Text1.color = Color.white;
            Text2.color = Color.white;
        }
        //TroubleManager.Instance.Remove_TroubleObserver(this);

    }
    void textColorSet()
    {


        sequence.Append(Text1.DOColor(Color.red, 1).SetLoops(-1, LoopType.Yoyo));
        sequence2.Append(Text2.DOColor(Color.red, 1).SetLoops(-1, LoopType.Yoyo));

        sequence.AppendInterval(0f);
        sequence.SetLoops(1, LoopType.Yoyo);
        sequence.SetRelative(true);

        sequence2.AppendInterval(0f);
        sequence2.SetLoops(1, LoopType.Yoyo);
        sequence2.SetRelative(true);



        //Text1.DOColor(Color.red, 1).SetLoops(-1, LoopType.Yoyo);
        //Text2.DOColor(Color.red, 1).SetLoops(-1, LoopType.Yoyo);
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
