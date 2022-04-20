using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerParent : MonoBehaviour
{
    [SerializeField] public List<Transform> humans;
    public float horizontalFollowSpeed;
    public NavMeshAgent agent;
    public UIdirection direction;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
    }
    public void throughlyScale()
    {
        StartCoroutine(scaleCalling());
    }
    IEnumerator scaleCalling()
    {
        int humanCount = humans.Count;
        for(int i = 0; i < humanCount - 1; i++)
        {
            StartCoroutine(throughlyScaling(humans[humanCount - 1 - i].transform));
            yield return new WaitForSeconds(0.05f);
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
   public void UItargetSelect()
    {
        if (humans.Count > 1)
        {
            direction.selectTarget(humans[humans.Count - 1].GetComponent<Employee>().jobId, transform);
        }
        else
        {
            direction.selectTarget(0, transform);
        }
    }
}
