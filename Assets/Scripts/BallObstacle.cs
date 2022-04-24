using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallObstacle : MonoBehaviour
{
    public enum States { X, Zreverse, Z }
    public States RotateAxis;
    Vector3 axis;
    void Start()
    {
        switch (RotateAxis)
        {
            case States.X:
                {
                    axis = new Vector3(1, 0, 0);
                }
                break;
            case States.Zreverse:
                {
                    axis = new Vector3(0, 0, -1);
                }
                break;
            case States.Z:
                {
                    axis = new Vector3(0, 0, 1);
                }
                break;
        }
        StartCoroutine(rotBal(axis));
    }

 IEnumerator rotBal(Vector3 vect)
    {
        float seconds;
        seconds = Random.Range(0f, 0.8f);
        yield return new WaitForSeconds(seconds);
        while (true)
        {
            float counter = 0;
            float angle = 0;
            while (counter < Mathf.PI)
            {
                counter += 2 * Time.deltaTime;
                angle = Mathf.Cos(counter);
                angle *= 70;

                transform.localRotation = Quaternion.Euler(angle * vect.x, angle * vect.y, angle * vect.z);

                yield return null;
            }

            while (counter > 0)
            {
                counter -= 10 * Time.deltaTime;
                angle = Mathf.Cos(counter);
                angle *= 70;

                transform.localRotation = Quaternion.Euler(angle * vect.x, angle * vect.y, angle * vect.z);
                yield return null;
            }
        }
    }
}
