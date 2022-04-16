using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xGate : MonoBehaviour
{
    GameObject cloneHuman;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IDamageble>() != null && other.tag != "Player")
        {
            cloneHuman = Instantiate(other.gameObject, transform.position, Quaternion.identity, other.transform);
            cloneHuman.GetComponent<Collider>().enabled = false;
            cloneHuman.GetComponent<Animator>().SetBool("walk", true);
            other.transform.parent.GetComponent<PlayerParent>().humans.Add(cloneHuman.transform);
            other.GetComponent<Collider>().enabled = false;
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
        hmn.GetComponent<Collider>().enabled = true;
        cloneHuman.GetComponent<Collider>().enabled = true;
    }
}
