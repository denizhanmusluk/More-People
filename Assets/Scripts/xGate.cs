using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xGate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IDamageble>() != null && other.tag != "Player")
        {
            transform.GetComponent<Collider>().enabled = false;
            StartCoroutine(setGateCollider());
            StartCoroutine(cloneHmn(other.transform.gameObject));
            Debug.Log("xGaTEx");
            //other.GetComponent<Collider>().enabled = false;

            //GameObject cloneHuman = Instantiate(other.gameObject, other.transform.position + new Vector3(0.7f, 0, 0), Quaternion.identity);
            //cloneHuman.GetComponent<Collider>().enabled = false;
            //cloneHuman.transform.parent = other.transform;

            //cloneHuman.GetComponent<Animator>().SetBool("walk", true);
            //other.transform.parent.GetComponent<PlayerParent>().humans.Add(cloneHuman.transform);
            //StartCoroutine(throughlyScaling(other.transform , cloneHuman));
        }
    }
    IEnumerator cloneHmn(GameObject human)
    {
        human.GetComponent<Collider>().enabled = false;
        yield return null;
        GameObject cloneHuman = Instantiate(human, human.transform.position + new Vector3(0.7f, 0, 0), Quaternion.identity,human.transform.parent);
        cloneHuman.GetComponent<Collider>().enabled = false;
        cloneHuman.GetComponent<Animator>().SetBool("walk", true);
        yield return null;
        cloneHuman.transform.parent = human.transform;
        StartCoroutine(throughlyScaling(human.transform));
        yield return new WaitForSeconds(1f);
        human.transform.parent.GetComponent<PlayerParent>().humans.Add(cloneHuman.transform);
        yield return new WaitForSeconds(1f);
        cloneHuman.GetComponent<Collider>().enabled = true;

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
        yield return new WaitForSeconds(1f);
        hmn.GetComponent<Collider>().enabled = true;
    }
    IEnumerator setGateCollider()
    {
        yield return new WaitForSeconds(Time.deltaTime*2);
        transform.GetComponent<Collider>().enabled = true;
    }
}
