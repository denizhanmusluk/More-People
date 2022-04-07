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
    private void Start()
    {
        anim = GetComponent<Animator>();
        playerParent = transform.parent.GetComponent<PlayerParent>();

        followPoint = transform.GetChild(0).transform;
    }
    public void hitPeople(GameObject human)
    {
        if (human.GetComponent<PlayerBehaviour>() == null)
        {
            human.AddComponent<PlayerBehaviour>();
        }
        if (human.GetComponent<Employee>() == null)
        {
            human.AddComponent<Employee>();
            human.GetComponent<Employee>().Player = playerParent.transform.GetChild(0).transform;
        }
        if (human.GetComponent<PeopleHit>() != null)
        {
            Destroy(human.GetComponent<PeopleHit>());
        }
        StartCoroutine(moveToFollowPoint(human));
        transform.parent.GetComponent<PlayerParent>().throughlyScale();
    }
    public void followTarget(GameObject human)
    {
        StartCoroutine(following(human, latestFollowPoint));
    }
    IEnumerator moveToFollowPoint(GameObject human)
    {
        human.transform.parent = transform.parent;
        int stackCount = playerParent.humans.Count;
        Transform latestHuaman = playerParent.humans[stackCount-1];
        latestFollowPoint = latestHuaman.GetComponent<PlayerBehaviour>().followPoint;
        playerParent.humans.Add(human.transform);
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
  
        while (runnerActive)
        {
            //human.transform.position = Vector3.MoveTowards(human.transform.position, followPoint.position, (Mathf.Abs(playerParent.horizontalFollowSpeed) + 10 ) * 0.8f * Time.deltaTime);
            human.transform.position = Vector3.MoveTowards(human.transform.position, new Vector3( followPoint.position.x, human.transform.position.y, human.transform.position.z), followSpeed * (Vector3.Distance(human.transform.position, followPoint.position)) / FollowDistance * Time.deltaTime);

            yield return null;

        }
    }
}