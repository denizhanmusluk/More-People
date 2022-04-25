using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public TextMeshProUGUI Text1;
   //public Sequence sequence, sequence2, sequence3, sequence4;
    [SerializeField] public MeshRenderer buildMesh;
    [SerializeField] public Material firstMaterial;
    [SerializeField] public Material troubleMaterial;
    [SerializeField] GameObject attention;
    [SerializeField] GameObject envirnmonetParticles;
    [SerializeField] ParticleSystem fireWork;
    public GameObject hiringImage;
    public GameObject downArrow, upArrow;
    private void Start()
    {
        TroubleManager.Instance.Add_TroubleObserver(this);
        downArrow = GameManager.Instance.downArrow.gameObject;
        upArrow = GameManager.Instance.upArrow.gameObject;
    }

    private void Awake()
    {
 

        //hiringImage.SetActive(false);
        attention.SetActive(false);
        envirnmonetParticles.SetActive(false);

    }
    public void buildInit(int level)
    {

        //troubleActive = false;
        fireWork.Play();
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
        buildMesh= loadedBuild.GetComponent<MoneyCreating>().buildMesh;
        firstMaterial = buildMesh.material;
        loadedBuild.GetComponent<MoneyCreating>().spawnSpeed();
        StartCoroutine(buildScaling());
    }
    public void troubleCheck()
    {
        //Debug.Log("Globals.maxBuildLevel" + Globals.maxBuildLevel);
        //Debug.Log("thisBuildingLevel" + thisBuildingLevel);
        if(Globals.maxBuildLevel - 1 > thisBuildingLevel)
        {
            if (!troubleActive)
            {
                colorIndicate = true;
                troubleActive = true;
                //trouble this build

                for (int i = 0; i < customerList.Count; i++)
                {

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
                        //if (thisBuildingLevel >= 2)
                        //{
                        //    envirnmonetParticles.SetActive(true);
                        //}

                    }
                    if (buildNo == 3)
                    {
                        customerList[i].GetComponent<NPC>().currentSelection = NPC.States.troubleFarm;

                    }
                    if (buildNo == 4)
                    {
                        customerList[i].GetComponent<NPC>().currentSelection = NPC.States.troubleUniversity;
                    }
                }
                Globals.troubleBuildNo = buildNo;
                attention.SetActive(true);
                envirnmonetParticles.SetActive(true);

                //textColorSet();
                buildMesh.material = troubleMaterial;
                //BuildColorSet();

                StartCoroutine(loopColorScaleSet());
                //TroubleManager.Instance.Remove_TroubleObserver(this);
                TroubleManager.Instance.Notify_isTroubleObservers();
                Debug.Log("trouble active" + transform.name);
                upArrow.GetComponent<Image>().enabled = false;

            }
        }
        else
        {


      
            hiringImage.SetActive(false);

            if (troubleActive)
            {
                troubleActive = false;
                TroubleManager.Instance.Notify_GameTroubleFixObservers();
                Debug.Log("sorun cozuldu");
                attention.SetActive(false);
                envirnmonetParticles.SetActive(false);
                upArrow.GetComponent<Image>().enabled = true;

                //envirnmonetParticles.SetActive(false);

                // trouble fixed
            }
            //Text1.color = Color.white;
            //buildMesh.material.color = Color.white;
        }
        //TroubleManager.Instance.Remove_TroubleObserver(this);

    }
    IEnumerator loopColorScaleSet()
    {
        hiringImage.SetActive(true);
        upArrow.GetComponent<Image>().enabled = false;
        downArrow.SetActive(true);
        float counter1 = 0f;
        float counter2 = 0f;
        float counter3 = 0f;
        float counter4 = 0f;
        float scaleValue1 = 0f;
        float scaleValue2 = 0f;
        float scaleValue3 = 0f;
        float scaleValue4 = 0f;
        while (troubleActive)
        {
            counter1 += 5 * Time.deltaTime;
            counter2 += 5 * Time.deltaTime;
            counter3 += 5 * Time.deltaTime;
            counter4 += 5 * Time.deltaTime;
            scaleValue1 = Mathf.Abs(Mathf.Cos(counter1));
            scaleValue2 = Mathf.Abs(Mathf.Cos(counter2));
            scaleValue3 = Mathf.Abs(Mathf.Cos(counter3));
            scaleValue4 = Mathf.Abs(Mathf.Cos(counter4));

            buildMesh.material.color = new Color(1, 1- scaleValue1, 1- scaleValue1);
            attention.transform.localScale = new Vector3(1, 1.6f, 0.001f) + new Vector3(scaleValue2 / 5f, scaleValue2 / 5f, 0f);
            Text1.color = new Color(scaleValue3, 0, 0);
            hiringImage.transform.localScale = Vector3.one + new Vector3(scaleValue4 / 5f, scaleValue4 / 5f, scaleValue4 / 5f);
            downArrow.transform.localScale = Vector3.one + new Vector3(scaleValue4 / 5f, scaleValue4 / 5f, scaleValue4 / 5f);
            downArrow.GetComponent<Image>().color = new Color(1, 1 - scaleValue3, 1 - scaleValue3);




            yield return null;
        }
        buildMesh.material.color = new Color(1,1,1);
        attention.transform.localScale = new Vector3(1, 1.6f, 0.001f);
        Text1.color = new Color(0,0,0);
        hiringImage.transform.localScale = Vector3.one;
        downArrow.transform.localScale = Vector3.one;
        downArrow.GetComponent<Image>().color = new Color(1,1,1);
        downArrow.SetActive(false);

    }
    /*
    void BuildColorSet()
    {

        //buildMesh.material.color 
        sequence3.Append(buildMesh.material.DOColor(Color.red, 1).SetLoops(-1, LoopType.Yoyo));

        sequence3.AppendInterval(0f);
        sequence3.SetLoops(1, LoopType.Yoyo);
        sequence3.SetRelative(true);



        sequence2.Append(attention.transform.DOScale(Vector3.one * 1.1f, 0.3f).SetLoops(-1, LoopType.Yoyo));

        sequence2.AppendInterval(0f);
        sequence2.SetLoops(1, LoopType.Yoyo);
        sequence2.SetRelative(true);
        //Text1.DOColor(Color.red, 1).SetLoops(-1, LoopType.Yoyo);
        //Text2.DOColor(Color.red, 1).SetLoops(-1, LoopType.Yoyo);
    }
    void textColorSet()
    {


        sequence.Append(Text1.DOColor(Color.red, 1).SetLoops(-1, LoopType.Yoyo));
        //sequence2.Append(Text2.DOColor(Color.red, 1).SetLoops(-1, LoopType.Yoyo));

        sequence.AppendInterval(0f);
        sequence.SetLoops(1, LoopType.Yoyo);
        sequence.SetRelative(true);

        hiringImage.SetActive(true);

        sequence4.Append(hiringImage.transform.DOScale(Vector3.one * 1.1f, 0.3f).SetLoops(-1, LoopType.Yoyo));

        sequence4.AppendInterval(0f);
        sequence4.SetLoops(1, LoopType.Yoyo);
        sequence4.SetRelative(true);

        //sequence2.AppendInterval(0f);
        //sequence2.SetLoops(1, LoopType.Yoyo);
        //sequence2.SetRelative(true);



        //Text1.DOColor(Color.red, 1).SetLoops(-1, LoopType.Yoyo);
        //Text2.DOColor(Color.red, 1).SetLoops(-1, LoopType.Yoyo);
    }
    */
    IEnumerator buildScaling()
    {
        int buildChildCount = loadedBuild.transform.childCount;
        for (int i = 0; i < buildChildCount; i++)
        {
            StartCoroutine(throughlyScaling(loadedBuild.transform.GetChild(i).transform));
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(thisBuildingLevel * Random.Range(0, 1f));
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
