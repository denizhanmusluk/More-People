using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishRunner : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerControl playerControl = other.GetComponent<PlayerControl>();
            PlayerParent playerParent = other.transform.parent.GetComponent<PlayerParent>();
            playerControl.currentBehaviour = PlayerControl.States.runnerToIdle;
            playerControl.idleCamera.Priority = 10;
            playerControl.runnerCamera.Priority = 0;
            for (int i = 0; i < playerParent.humans.Count; i++)
            {
                playerParent.humans[i].GetComponent<PlayerBehaviour>().runnerActive = false;
            }
            for (int i = 1; i < playerParent.humans.Count; i++)
            {
                playerParent.humans[i].GetComponent<Employee>().currentBehaviour = Employee.States.wait;
                playerParent.humans[i].transform.parent = transform.root;
            }

            GameManager.Instance.Notify_GameFinishObservers();
        }
    }
    IEnumerator setSpeed()
    {
        while (true)
        {

            yield return null;
        }
    }
}