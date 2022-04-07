using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoystick : MonoBehaviour
{
    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;
    public bool pressed = false;
    private Rigidbody rb;
    private Animator Anim;
    public GameObject parent;
    public float speed;
    private void Start()
    {
        //Anim = GetComponent<Animator>();
        rb = parent.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Swipe();
    }
    public void Swipe()
    {

        if (Input.GetMouseButtonDown(0))
        {
            firstPressPos = (Vector2)Input.mousePosition;
            pressed = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            rb.velocity = new Vector3(0, 0, 0);
            secondPressPos = (Vector2)Input.mousePosition;
            currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            firstPressPos = (Vector2)Input.mousePosition;
            pressed = false;
        }

        if (pressed == true)
        {
            //Anim.SetBool("walk", true);
            secondPressPos = (Vector2)Input.mousePosition;
            currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            currentSwipe.Normalize();


            Vector3 direction = new Vector3(currentSwipe.x, 0f, currentSwipe.y);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion newRot = Quaternion.Euler(0, targetAngle, 0);


            transform.rotation = newRot;
            parent.transform.position = parent.transform.position + (direction * speed * Time.deltaTime);


        }
        else
        {
            //Anim.SetBool("walk", false);
        }
    }
}
