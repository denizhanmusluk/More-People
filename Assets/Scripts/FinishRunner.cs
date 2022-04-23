using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishRunner : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerControl playerControl;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerControl = other.GetComponent<PlayerControl>();
            PlayerParent playerParent = other.transform.parent.GetComponent<PlayerParent>();
            playerParent.agent.enabled = true;
            playerControl.currentBehaviour = PlayerControl.States.runnerToIdle;
            playerControl.idleCamera.Priority = 0;
            playerControl.runnerCamera.Priority = 0;
            playerControl.runnerToIdleCamera.Priority = 10;
            playerParent.UItargetSelect();

            StartCoroutine(CameraChange());
            for (int i = 0; i < playerParent.humans.Count; i++)
            {
                playerParent.humans[i].GetComponent<PlayerBehaviour>().runnerActive = false;
            }
            for (int i = 1; i < playerParent.humans.Count; i++)
            {
                playerParent.humans[i].GetComponent<Employee>().currentBehaviour = Employee.States.wait;
                playerParent.humans[i].GetComponent<Employee>().agent.enabled = true;
                playerParent.humans[i].transform.parent = transform.root;
            }

            GameManager.Instance.Notify_GameFinishObservers();
        }
    }
    IEnumerator CameraChange()
    {
        yield return new WaitForSeconds(1f);
        playerControl.idleCamera.Priority = 10;
        playerControl.runnerCamera.Priority = 0;
        playerControl.runnerToIdleCamera.Priority = 0;
        playerControl.Lawyer.SetActive(true);
        playerControl.Hand.SetActive(false);
        playerControl.anim.SetBool("walk", true);

        yield return new WaitForSeconds(1f);

        playerControl.anim.SetTrigger("flip");

    }
}