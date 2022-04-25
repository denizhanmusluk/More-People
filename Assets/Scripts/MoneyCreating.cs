﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCreating : MonoBehaviour
{
    [SerializeField] GameObject moneyPrefab;

    //public int moneyNum;
    //public List<GameObject> moneyList;
    public float spawnTime = 0.67f;
    public int maxLimit = 12;
    public int banknotValue = 1;
    [SerializeField] Transform[] moneyInstantiatePoint;
    //[SerializeField] Transform firstInstPoint, firstInstPoint2;
    [SerializeField] MoneyCollect moneyCollecting;
    [SerializeField] public MeshRenderer buildMesh;
    float spawnTurboTime = 0.04f;
   public float firstSpawnTime;
    private void Awake()
    {
        firstSpawnTime = spawnTime;

    }
    void Start()
    {

        Debug.Log("firstSpawnTime " + firstSpawnTime);

        StartCoroutine(SpawnMoney());
    }

    IEnumerator SpawnMoney()
    {
        while (!transform.parent.GetComponent<Build>().troubleActive)
        {
            yield return new WaitForSeconds(spawnTime);
            //firstSpawnTime = spawnTime;

            if (moneyCollecting.moneyNum < maxLimit)
            {
                GameObject material = Instantiate(moneyPrefab, transform.position, Quaternion.Euler(-90,90,0), this.transform);
                material.GetComponent<Banknot>().banknotValue = banknotValue;
                float clothRow = moneyCollecting.moneyNum / moneyInstantiatePoint.Length;
                //worker.GetComponent<Animator>().SetTrigger("work");
                material.GetComponent<Banknot>().mnyCollect = moneyCollecting;

                   //while (Vector3.Distance(material.transform.position, firstInstPoint2.position) > 0.1f)
                   //{
                   //    material.transform.position = Vector3.MoveTowards(material.transform.position, firstInstPoint2.position, 1.5f * Time.deltaTime);
                   //    material.transform.rotation = Quaternion.RotateTowards(material.transform.rotation, firstInstPoint2.rotation, 75 * Time.deltaTime);
                   //    yield return null;
                   //}
                   Vector3 targetPos = moneyInstantiatePoint[moneyCollecting.moneyNum % moneyInstantiatePoint.Length].position + new Vector3(0, (clothRow / 3), 0);
                //while (Vector3.Distance(material.transform.position, targetPos) > 0.1f)
                //{
                //    material.transform.position = Vector3.MoveTowards(material.transform.position, targetPos, 15 * Time.deltaTime);
                //    material.transform.localRotation = Quaternion.RotateTowards(material.transform.localRotation, Quaternion.Euler(90, 0, 90), 600 * Time.deltaTime);
                //    yield return null;
                //}
                material.transform.position = targetPos;
                //moneyList.Add(material);
                moneyCollecting.moneyList.Add(material);
                moneyCollecting.moneyNum++;

            }
        }
        yield return null;
        StartCoroutine(SpawnMoney());

    }
    public void spawnSpeed()
    {
        StartCoroutine(spawnSpeedSet());
    }
    IEnumerator spawnSpeedSet()
    {
        Debug.Log("spawnTurboTime " + spawnTurboTime);
        Debug.Log("firstSpawnTime " + firstSpawnTime);
        spawnTime = spawnTurboTime;
        yield return new WaitForSeconds(5f);
        spawnTime = firstSpawnTime;
        Debug.Log("spawn active 2");

    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.GetComponent<IStack>() != null)
    //    {
    //        other.GetComponent<IStack>().materialStack(this.gameObject);

    //    }
    //    if (other.GetComponent<AIbehaviour>() != null)
    //    {
    //        //other.GetComponent<AIbehaviour>().anim.SetBool("walk", false);


    //        if (materialList.Count == 0 || other.GetComponent<AIControl>().maxStack == other.GetComponent<AIControl>().totalStackNumber)
    //        {
    //            other.GetComponent<AIbehaviour>().dropMaterial();
    //            //other.GetComponent<AIbehaviour>().anim.SetBool("walk", true);

    //        }
    //    }
    //}
}
