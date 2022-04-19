using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class StartRunner : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera runnerCamera, idleCamera, runnerToIdleCamera;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //runnerCamera.Priority = 10;
            //idleCamera.Priority = 0;
            StartCoroutine(moving(other.gameObject));
            other.GetComponent<PlayerControl>().idleControlActive = false;
        }
    }
    IEnumerator moving(GameObject player)
    {

        float counter = 0;
        while (Mathf.Abs(player.transform.parent.transform.position.x) > 0 || Mathf.Abs(player.transform.localPosition.x) > 0)
        {
            counter += Time.deltaTime;
            player.transform.parent.transform.Translate(player.transform.parent.transform.forward * Time.deltaTime * player.GetComponent<PlayerControl>().acceleration);
            player.transform.parent.transform.position = Vector3.MoveTowards(player.transform.parent.transform.position, new Vector3(0, player.transform.parent.transform.position.y, player.transform.parent.transform.position.z), Time.deltaTime);
            player.transform.localPosition = Vector3.MoveTowards(player.transform.localPosition, new Vector3(0, player.transform.localPosition.y, player.transform.localPosition.z), Time.deltaTime);
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, Quaternion.Euler(player.transform.eulerAngles.x, 0, player.transform.eulerAngles.z), 100 * Time.deltaTime);

            yield return null;
        }
        GameManager.Instance.Notify_RunnerStartObservers();

        //runnerToIdleCamera.Priority = 10;
        //idleCamera.Priority = 0;
        //runnerCamera.Priority = 0;
        //yield return new WaitForSeconds(2f);
        //player.GetComponent<PlayerControl>().Lawyer.SetActive(false);
        //player.GetComponent<PlayerControl>().Hand.SetActive(true);
        //runnerToIdleCamera.Priority = 0;
        //idleCamera.Priority = 0;
        //runnerCamera.Priority = 10;
        //yield return new WaitForSeconds(1f);

        //GameManager.Instance.Notify_RunnerStartObservers();

    }
}
