using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfessionGate : MonoBehaviour
{
    public List<SkinnedMeshRenderer> employeeSkinPrefab;
    public int jobId;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IDamageble>() != null && other.tag != "Player")
        {
            other.transform.GetChild(1).transform.GetComponent<SkinnedMeshRenderer>().sharedMesh = employeeSkinPrefab[0].sharedMesh;
            other.GetComponent<Employee>().jobId = jobId;
            StartCoroutine(throughlyScaling(other.transform));
        }
    }

    IEnumerator throughlyScaling(Transform hmn)
    {
        float counter = 0f;
        float firstSize = 1f;
        float sizeDelta;
        while (counter < Mathf.PI)
        {
            counter += 15 * Time.deltaTime;
            sizeDelta = 1f - Mathf.Abs(Mathf.Cos(counter));
            sizeDelta /= 3f;
            hmn.localScale = new Vector3(firstSize + sizeDelta, firstSize + sizeDelta, firstSize + sizeDelta);

            yield return null;
        }
        hmn.localScale = new Vector3(firstSize, firstSize, firstSize);

    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.GetComponent<IDamageble>() != null && other.tag != "Player")
    //    {
    //        //other.gameObject.GetComponent<IDamageble>().hitPeople(gameObject);
    //        GameObject employee = Instantiate(employeePrefab[jobId], other.transform.position, other.transform.rotation);
    //        employee.GetComponent<PlayerBehaviour>().latestFollowPoint = other.GetComponent<PlayerBehaviour>().latestFollowPoint;
    //        employee.GetComponent<Animator>().SetTrigger("walk");
    //        employee.transform.parent = other.transform.parent;

    //        int empId = 0;
    //        for(int i = 0; i< other.transform.parent.GetComponent<PlayerParent>().humans.Count; i++)
    //        {
    //            if(other.transform.parent.GetComponent<PlayerParent>().humans[i].transform.position == other.transform.position)
    //            {
    //                empId = i;
    //                break;
    //            }
    //        }
    //        Debug.Log("id" + empId);
    //        other.transform.parent.GetComponent<PlayerParent>().humans[empId] = employee.transform;
    //        employee.GetComponent<PlayerBehaviour>().followTarget(employee);
    //        //other.transform.parent = null;
    //        //Destroy(other.gameObject);
    //    }
    //}
}
