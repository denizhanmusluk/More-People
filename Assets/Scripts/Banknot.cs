using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banknot : MonoBehaviour
{
    public int banknotValue;
    float motionSpeed = 50;
    GameObject target;
    public MoneyCollect mnyCollect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerControl>() != null)
        {
            mnyCollect.moneyNum--;
            transform.GetComponent<Collider>().enabled = false;
            //other.gameObject.GetComponent<Player>().MoneyUpdate(30);
            GameManager.Instance.moneyUp(banknotValue);
            target = other.GetComponent<PlayerControl>().moneyTarget;
            Destroy(GetComponent<Rigidbody>());
            //Score.Instance.scoreUp();
            StartCoroutine(targetMotion());


        }
    }
    IEnumerator targetMotion()
    {
        while (Vector3.Distance(transform.position, target.transform.position) > 0.3f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, (10 / Vector3.Distance(transform.position, target.transform.position)) * motionSpeed * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, target.transform.localScale, motionSpeed * 0.1f * Time.deltaTime);
            yield return null;
        }
        GameObject money = gameObject;
        money.transform.parent = null;
        Destroy(money);
    }
}