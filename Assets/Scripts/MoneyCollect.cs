using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCollect : MonoBehaviour
{
    public int moneyNum;
    public List<GameObject> moneyList;
    bool CollectActive = false;
   public GameObject moneyTarget;
    float motionSpeed = 50f;
    public RectTransform moneylabel;
    bool recollect = false;
    void Start()
    {
        //moneyTarget = GameObject.Find("MoneyTarget");
        //moneylabel = GameObject.Find("moneyLabel").GetComponent<RectTransform>();
        //moneyTarget.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(moneylabel.transform.position.x, moneylabel.transform.position.y, Camera.main.WorldToScreenPoint(moneyTarget.transform.position).z));

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            moneyTarget = GameObject.Find("MoneyTarget");
            moneylabel = GameObject.Find("moneyLabel").GetComponent<RectTransform>();
            moneyTarget.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(moneylabel.transform.position.x, moneylabel.transform.position.y, Camera.main.WorldToScreenPoint(moneyTarget.transform.position).z));

            CollectActive = true;
            StartCoroutine(moneyCollecting());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            CollectActive = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            CollectActive = true;
            if (recollect)
            {
                StartCoroutine(moneyCollecting());

            }
        }
    }

    IEnumerator moneyCollecting()
    {
       
            int moneyCount = moneyList.Count;

        recollect = false;

        if (CollectActive)
        {
            for (int i = 0; i < moneyCount; i++)
            {
                moneyList[i].GetComponent<Collider>().enabled = false;
            }

            for (int i = 0; i < moneyCount; i++)
            {
                GameObject money = moneyList[0];
                moneyList.Remove(money);
                moneyNum--;
                StartCoroutine(throughlyMoney(money.transform));
                yield return new WaitForSeconds(0.02f);
            }
        }
        recollect = true;
    }

    IEnumerator throughlyMoney(Transform money)
    {
   
        while (Vector3.Distance(money.transform.position, moneyTarget.transform.position) > 0.3f)
        {
            money.transform.position = Vector3.MoveTowards(money.transform.position, moneyTarget.transform.position, (10 / Vector3.Distance(money.transform.position, moneyTarget.transform.position)) * motionSpeed * Time.deltaTime);
            money.transform.localScale = Vector3.Lerp(money.transform.localScale, moneyTarget.transform.localScale, motionSpeed * 0.1f * Time.deltaTime);
            yield return null;

        }
        GameManager.Instance.moneyUp(money.GetComponent<Banknot>().banknotValue);
        //GameObject money = gameObject;
        money.transform.parent = null;
        Destroy(money.gameObject);
    }
}