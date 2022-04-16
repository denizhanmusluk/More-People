using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] public Transform destination;
    public Animator anim;
    public enum States { moveTarget, trouble,dead}
    public States currentSelection;
    int deadSelecting;
    [SerializeField] GameObject[] deadSprite;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = transform.GetComponent<Animator>();
        anim.SetBool("walk", true);
        agent.SetDestination(destination.position);
        //currentSelection = States.moveTarget;

    }
    void Update()
    {
        switch (currentSelection)
        {
            case States.moveTarget:
                {
                    moveTarget();
                }
                break;
            case States.trouble:
                {
                    troubleMove();
                }
                break;
            case States.dead:
                {
                    agent.SetDestination(transform.position);
                }
                break;
        }
    }
    public void moveTarget()
    {
        agent.SetDestination(destination.position);

        if (Vector3.Distance(transform.position, destination.position) < 1)
        {
            anim.SetBool("walk", false);
            destination.transform.parent.GetComponent<Build>().customerList.Remove(this.gameObject);
            Destroy(this.gameObject, 0.3f);
        }
        else
        {
            anim.SetBool("walk", true);
        }
    }
    public void troubleMove()
    {
        agent.SetDestination(destination.position);

        if (Vector3.Distance(transform.position, destination.position) < 1)
        {
            anim.SetBool("walk", false);
            destination.transform.parent.GetComponent<Build>().customerList.Remove(this.gameObject);
        }
        else
        {
            anim.SetBool("walk", true);
        }
    }
    public void _randomDead()
    {
        StartCoroutine(randomDead());
    }
    IEnumerator randomDead()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            deadSelecting = Random.Range(0, 3);
            if(deadSelecting == 0)
            {
                GetComponent<Ragdoll>().RagdollActivate(true);
                currentSelection = States.dead;
    
                break;
                //Destroy(this.gameObject ,5);
            }
        }
        yield return new WaitForSeconds(6f);
        int selecting = Random.Range(0, deadSprite.Length);
        Instantiate(deadSprite[selecting], transform.position, Quaternion.Euler(90, 0, 0));
        Destroy(this.gameObject);
    }
}
