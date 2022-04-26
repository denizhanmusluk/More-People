using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerBehaviour : MonoBehaviour, IDamageble
{
    public Transform followPoint;
    PlayerParent playerParent;
    float FollowDistance = 0.3f;
    Animator anim;
    public Transform latestFollowPoint;
    //public int jobId = 0;
    float followSpeed = 5f;
    public bool runnerActive = true;
   public GameObject humanfront;
    public GameObject humanBack;
    private void Start()
    {
        anim = GetComponent<Animator>();
        playerParent = transform.parent.GetComponent<PlayerParent>();

        followPoint = transform.GetChild(0).transform;
    }
    public void hitPeople(GameObject human)
    {
        if (transform.parent.GetComponent<PlayerParent>() != null)
        {
            if (human.GetComponent<PlayerBehaviour>() == null)
            {
                human.AddComponent<PlayerBehaviour>();
            }
            if (human.GetComponent<Employee>() == null)
            {
                human.AddComponent<Employee>();
                human.GetComponent<Employee>().Player = playerParent.transform;
            }
            if (human.GetComponent<PeopleHit>() != null)
            {
                Destroy(human.GetComponent<PeopleHit>());
            }
            //humanfront = human;
            //human.GetComponent<PlayerBehaviour>().humanBack = this.gameObject;
            StartCoroutine(moveToFollowPoint(human));

            transform.parent.GetComponent<PlayerParent>().throughlyScale();
        }
    }
    public void followTarget(GameObject human, Transform followPoint)
    {
        StartCoroutine(reMoveToFollowPoint(human, followPoint));
    }
    IEnumerator moveToFollowPoint(GameObject human)
    {
        human.transform.parent = transform.parent;
        int stackCount = playerParent.humans.Count;
        Transform latestHuaman = playerParent.humans[stackCount-1];
        latestFollowPoint = latestHuaman.GetComponent<PlayerBehaviour>().followPoint;
        playerParent.humans.Add(human.transform);

        human.GetComponent<PlayerBehaviour>().humanBack = latestHuaman.gameObject;

        latestHuaman.GetComponent<PlayerBehaviour>().humanfront = human;
        while (Vector3.Distance(human.transform.position, new Vector3(latestFollowPoint.position.x, human.transform.position.y, latestFollowPoint.position.z)) > 0.1f)
        {
            human.transform.position = Vector3.MoveTowards(human.transform.position, new Vector3(latestFollowPoint.position.x, human.transform.position.y , latestFollowPoint.position.z) , 10 * Time.deltaTime);
            yield return null;

        }
        human.transform.position = new Vector3(latestFollowPoint.position.x, human.transform.position.y, latestFollowPoint.position.z);
        StartCoroutine(following(human, latestFollowPoint));

    }

    IEnumerator following(GameObject human, Transform followPoint)
    {
        runnerActive = true;
        yield return null;
        while (runnerActive)
        {
            //human.transform.position = Vector3.MoveTowards(human.transform.position, followPoint.position, (Mathf.Abs(playerParent.horizontalFollowSpeed) + 10 ) * 0.8f * Time.deltaTime);
            human.transform.position = Vector3.MoveTowards(human.transform.position, new Vector3( followPoint.position.x, human.transform.position.y, human.transform.position.z), followSpeed * (Vector3.Distance(human.transform.position, followPoint.position)) / FollowDistance * Time.deltaTime);

            yield return null;

        }
    }
   public void hitObstacle(GameObject obs)
    {
        this.runnerActive = false;
        humanBack.GetComponent<PlayerBehaviour>().runnerActive = false;
        //humanBack.GetComponent<PlayerBehaviour>().runnerActive = true;
        transform.parent = playerParent.transform.parent;
        if (playerParent.humans[playerParent.humans.Count - 1].name != this.transform.name)
        {
            humanfront.GetComponent<PlayerBehaviour>().humanBack = humanBack;
            humanBack.GetComponent<PlayerBehaviour>().humanfront = humanfront;
            humanBack.GetComponent<PlayerBehaviour>().followTarget(humanBack.GetComponent<PlayerBehaviour>().humanfront, humanBack.GetComponent<PlayerBehaviour>().followPoint);

        }
        playerParent.humans.Remove(this.transform);
        GetComponent<Ragdoll>().RagdollActivateWithForce(true, transform.position.x - obs.transform.position.x);
        Destroy(this,0.1f);

    }
    IEnumerator reMoveToFollowPoint(GameObject human,Transform _followPoint)
    {
        while (Vector3.Distance(human.transform.position, new Vector3(_followPoint.position.x, human.transform.position.y, _followPoint.position.z)) > 0.5f)
        {
            human.transform.position = Vector3.MoveTowards(human.transform.position, new Vector3(_followPoint.position.x, human.transform.position.y, _followPoint.position.z), 60f * Time.deltaTime);
            yield return null;

        }
        human.transform.position = new Vector3(_followPoint.position.x, human.transform.position.y, _followPoint.position.z);
        StartCoroutine(following(human, followPoint));
    }
}