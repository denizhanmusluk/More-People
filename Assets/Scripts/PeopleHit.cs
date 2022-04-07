using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleHit : MonoBehaviour
{
    //[SerializeField] Material firstMat;
    //Color FirstColour;
    //[SerializeField] Material mat;
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        //FirstColour = firstMat.GetColor("_EmissionColor");

        /*
        StartCoroutine(collUpd());

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(true);
        */


        //transform.GetChild(1).GetComponent<Animator>().enabled = false;
    }
    private void Update()
    {
        //transform.Rotate(0, 50 * Time.deltaTime, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IDamageble>() != null)
        {
            anim.SetBool("walk",true);

            other.gameObject.GetComponent<IDamageble>().hitPeople(gameObject);
            Debug.Log("collision");

        }

    }
    //private void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.GetComponent<IDamageble>() != null)
    //    {
    //        other.gameObject.GetComponent<IDamageble>().hitPeople(gameObject);
    //        Debug.Log("collision");
    //        /*
    //        transform.GetChild(0).gameObject.SetActive(true);
    //        transform.GetChild(1).gameObject.SetActive(true);
    //        transform.GetChild(2).gameObject.SetActive(false);
    //        transform.GetChild(3).gameObject.SetActive(false);
    //        */
    //    }
    //    else
    //    {
    //        //gameObject.GetComponent<BoxCollider>().isTrigger = true;
    //        //gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    //        //StartCoroutine(colliderSet());
    //    }
    //}
    IEnumerator colliderSet()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<BoxCollider>().isTrigger = false;
    }

    IEnumerator collUpd()
    {
        while (true)
        {
            float counter = 1f;
            while (counter < 4f)
            {
                counter += 5 * Time.deltaTime;

                //mat.SetColor("_EmissionColor", FirstColour * counter);

                yield return null;
            }
            while (counter > 1)
            {
                counter -= 5 * Time.deltaTime;

                //mat.SetColor("_EmissionColor", FirstColour * counter);

                yield return null;
            }
        }
    }
}
