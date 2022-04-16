using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyBuild : MonoBehaviour
{
    [SerializeField] int cost;
    [SerializeField] public Image outline;
    [SerializeField] public TextMeshProUGUI costText;
    bool pressed = false;
    public bool isWork = false;
    bool sellActive = true;
    [SerializeField] GameObject buildPrefab;
    [SerializeField] Vector3 buildPositionOffset;
    [SerializeField] string buyName;
    void Start()
    {
        if (PlayerPrefs.GetInt(buyName) == 1)
        {
            //Instantiate(buildPrefab, buildPositionOffset, transform.rotation, transform.parent);
            StartCoroutine(buildScaling());
            GetComponent<Collider>().enabled = false;

        }
        costText.text = cost.ToString();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pressed = true;
        }  
        if (Input.GetMouseButtonUp(0))
        {
            pressed = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerPrefs.SetInt(buyName, 0);

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Globals.moneyAmount >= cost)
            {
                if (sellActive)
                {
                    buy();
                }
                //GameManager.Instance.MoneyUpdate(-cost);
            }
        }
    }
    public void buy()
    {
        if (pressed == false && isWork == false)
        {
            if (Globals.moneyAmount >= cost)
            {
                isWork = true;


                LeanTween.value(0, 1, 2).setOnUpdate((float val) =>
                {
                    outline.fillAmount = val;
                    costText.text = ((1 - val) * (float)cost).ToString("N0");
                }).setOnComplete(() =>
                {
                    outline.fillAmount = 0;
                    isWork = false;
                    sellActive = false;
                    //MoneyUpdate(buyObj.GetComponent<buy>().buyAmount * -1);
                    GameManager.Instance.MoneyUpdate(-cost);
                    StartCoroutine(buildScaling());
                    GetComponent<Collider>().enabled = false;
                });

            }
        }
    }
    IEnumerator buildScaling()
    {
        int buildChildCount = transform.childCount;
        for (int i = 0; i < buildChildCount; i++)
        {
            StartCoroutine(throughlyScaling(transform.GetChild(i).transform));
            yield return new WaitForSeconds(0.05f);
        }
        instantiateBuild();
    }
    // Update is called once per frame
    IEnumerator throughlyScaling(Transform bld)
    {
        float counter = 1f;
        float lastSize = 0f;
        float sizeDelta;

        while (counter < 1f)
        {
            counter -= 15 * Time.deltaTime;

            bld.localScale = new Vector3(counter, counter, counter);
            yield return null;
        }
        bld.localScale = new Vector3(lastSize, lastSize, lastSize);
    }
    void instantiateBuild()
    {
        Instantiate(buildPrefab, buildPositionOffset, transform.rotation, transform.parent);
        PlayerPrefs.SetInt(buyName, 1);
    }
}
