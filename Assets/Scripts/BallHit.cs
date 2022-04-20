using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHit : MonoBehaviour
{
    //public AudioClip impact;
    //AudioSource audioSource;
    private GameObject playerParent;
    //[SerializeField] ParticleSystem smoke1, smoke2;
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IDamageble>() != null && other.gameObject.GetComponent<PlayerControl>() == null)
        {
            Debug.Log("hit obs" + other.transform.name);

            GetComponent<Collider>().enabled = false;

            other.gameObject.GetComponent<IDamageble>().hitObstacle(gameObject);
            //playerParent = other.gameObject.transform.parent.gameObject;
            //if (Globals.rage)
            //{
            //    other.gameObject.GetComponent<IDamageble>().obstacleHitBreak(gameObject);
            //}
            //else
            //{
            //    Globals.bike--;
            //    BicycleCount.Instance.BikeCountSet();
            //    other.gameObject.GetComponent<IDamageble>().BallHit(gameObject);

            //    if (playerParent.transform.childCount == 0)
            //    {
            //        Debug.Log("game over");
            //    }
            //}
        }
    }
    IEnumerator obstacleDest()
    {
        yield return new WaitForSeconds(1f);
        transform.parent = null;
        Destroy(gameObject);
    }

}
