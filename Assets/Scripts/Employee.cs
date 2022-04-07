using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee : MonoBehaviour
{
    public int jobId = 0;
    public enum States { wait, moving , runner }
    public States currentBehaviour;
    public Transform Player;
    Animator anim;
    bool isTrigger = false;
    float followDistance;
    bool idleSceneActive = false;
    private void Start()
    {
        followDistance = Random.Range(2f, 4f);
        anim = GetComponent<Animator>();
        currentBehaviour = States.runner;
    }
    private void Update()
    {
        switch (currentBehaviour)
        {
            case States.runner:
                {

                }
                break;
            case States.wait:
                {
                    idleSceneActive = true;
                    waiting();
                }
                break;
            case States.moving:
                {
                    ApplySteer();
                    move();
                }
                break;
        }

 
    }
    public void waiting()
    {
        anim.SetBool("walk", false);

        if (Vector3.Distance(transform.position, Player.position) > followDistance)
        {
            currentBehaviour = States.moving;
            transform.GetComponent<CapsuleCollider>().radius = 0.5f;
        }
    }
    public void move()
    {
        anim.SetBool("walk", true);

        transform.position = Vector3.MoveTowards(transform.position, Player.position,  2 * (Vector3.Distance(transform.position, Player.position)) / 1 * Time.deltaTime);

        if (Vector3.Distance(transform.position, Player.position) < followDistance)
        {
            currentBehaviour = States.wait;
            transform.GetComponent<CapsuleCollider>().radius = 0.25f;
        }
    }
    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(Player.position);
        relativeVector /= relativeVector.magnitude;
        float newSteerY = (relativeVector.x / relativeVector.magnitude) * 100;

        transform.Rotate(0, newSteerY * Time.deltaTime * 100f, 0);

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Employee>() != null && idleSceneActive)
        {
            anim.SetBool("walk", true);

            isTrigger = true;
            Vector3 direction = (transform.position - other.transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(direction.x, 0, direction.z), 0.5f * Time.deltaTime);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation,  Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + (transform.eulerAngles.y - other.transform.eulerAngles.y), transform.eulerAngles.z), 100 * Time.deltaTime);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Employee>() != null && idleSceneActive)
        {
            isTrigger = false;
            anim.SetBool("walk", false);

        }
    }


}
