using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIdirection : MonoBehaviour
{
    public List<GameObject> targetList;
    Vector3 direction;
    Vector3 distance;
    bool followActive = false;
    Transform player;
    int selectionTarget;
    public void selectTarget(int _selectionTarget, Transform _player)
    {
        GetComponent<Image>().enabled = true;
        player = _player;
        selectionTarget = _selectionTarget;
        followActive = true;
    }

    private void Update()
    {
        if (followActive)
        {
            arrowUIPos();
        }
    }
    public void arrowUIPos()
    {
        direction = (targetList[selectionTarget].transform.position - player.transform.position).normalized;
        distance = targetList[selectionTarget].transform.position - player.transform.position;
        float distZ = Mathf.Clamp(distance.z, -20, 20);
        float distX = Mathf.Clamp(distance.x, -2, 2);
        distX = Mathf.Abs(distX);
        distZ = Mathf.Abs(distZ);
        int magnX;
        int magnZ;
        if (distX > 0)
        {
            magnX = 50;
        }
        else
        {
            magnX = -50;
        }

        if (distZ > 0)
        {
            magnZ = 100;
        }
        else
        {
            magnZ = -100;
        }
        GetComponent<RectTransform>().anchoredPosition = new Vector3(direction.x * 250 * distX + magnX, direction.z * 60 * distZ + magnZ , 0);


        float angle;
        if (direction == Vector3.zero)
        {
            angle = 0;
        }
        else
        {
            angle = Mathf.Atan(direction.x / direction.z);
        }
        angle = angle * 180 / 3.14f;
        if (direction.z < 0)
        {
            angle += 180;
        }

        GetComponent<RectTransform>().rotation = Quaternion.Euler(0,0, -angle);
    }
}
