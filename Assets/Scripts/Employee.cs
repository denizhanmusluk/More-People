using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Employee : MonoBehaviour
{
    public int jobId = 0;
    public enum States { wait, moving , runner ,moveToBuild}
    public States currentBehaviour;
    public Transform Player;
    Animator anim;
    bool isTrigger = false;
    float followDistance;
    bool idleSceneActive = false;
    public NavMeshAgent agent;
   public Transform buildTarget;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        followDistance = Random.Range(2f, 4f);
        anim = GetComponent<Animator>();
        currentBehaviour = States.runner;
        agent.enabled = false;
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
                    //ApplySteer();
                    move();
                }
                break;
            case States.moveToBuild:
                {
                    moveToTargetBuild();
                }
                break;
        }

 
    }
    public void employeDropping(Transform build)
    {
        anim.SetBool("walk", true);
        agent.enabled = true;
        buildTarget = build.GetChild(0).transform;

        currentBehaviour = States.moveToBuild;
    }
    public void moveToTargetBuild()
    {
        if(Vector3.Distance(buildTarget.position, transform.position) > 1f)
        {
            agent.SetDestination(buildTarget.position);
        }
        else
        {
            buildTarget.transform.parent.GetComponent<IEmployeeDropping>().employeeDrop();
            Player.GetComponent<PlayerParent>().humans.Remove(this.gameObject.transform);
            Destroy(this.gameObject);
        }
    }
    IEnumerator moving()
    {
        while (true)
        {

            yield return null;
        }
    }
    public void waiting()
    {
        anim.SetBool("walk", false);
        agent.SetDestination(transform.position);


        if (Vector3.Distance(transform.position, Player.position) > followDistance)
        {
            currentBehaviour = States.moving;
            transform.GetComponent<CapsuleCollider>().radius = 0.5f;
        }
    }
    public void move()
    {
        anim.SetBool("walk", true);

        //transform.position = Vector3.MoveTowards(transform.position, Player.position,  2 * (Vector3.Distance(transform.position, Player.position)) / 1 * Time.deltaTime);
        agent.SetDestination(Player.position);

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
        float newSteerY = (relativeVector.x / relativeVector.magnitude) * 20;

        transform.Rotate(0, newSteerY * Time.deltaTime * 20f, 0);

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Employee>() != null && idleSceneActive)
        {
            anim.SetBool("walk", true);

            //isTrigger = true;
            //Vector3 direction = (transform.position - other.transform.position).normalized;
            //transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(direction.x, 0, direction.z), 0.5f * Time.deltaTime);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Employee>() != null && idleSceneActive)
        {
            //isTrigger = false;
            anim.SetBool("walk", false);

        }
    }
}
