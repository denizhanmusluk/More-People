using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerControl : MonoBehaviour, IStartGameObserver
{

    private float m_previousY;
    private float dY;
    private float m_previousX;
    private float dX;
    private float dX_Sum;
    [Range(0.0f, 10.0f)]
    [SerializeField] float Controlsensivity;

    public GameObject Lawyer, Hand;

    public float acceleration = 15;
    public float backSpeed = 10;
    //public BullControl cn;
    [SerializeField] public float steeringSpeed = 180;

    public float Xmove, Steer, Speed;
    [SerializeField] public float bounding;
    [SerializeField] GameObject playerParents;
    //[SerializeField] public GameObject moneyTarget;
    //[SerializeField] public SoldierCollecting soldierCollect;
    [SerializeField] public CinemachineVirtualCamera runnerCamera, idleCamera, runnerToIdleCamera, idleCamera2;
    public enum States { idle, runner, idleControl , runnerToIdle}
    public States currentBehaviour;
    public int slotNum = 0;
    [SerializeField] GameObject moneyParticlePrefab;
    Vector3 playerFirstPos;
  public  RectTransform moneylabel;
   public PlayerParent playerParent;



    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;
    public bool pressed = false;
    public Animator anim;
    public float speed;
    GameObject parent;
    float spd;
    public bool idleControlActive = true;
    public bool runnerControlActive = false;

    [SerializeField] public GameObject moneyTarget;
    //[SerializeField] GameObject rainParticle;
    public weatherManager weather;
    private void Awake()
    {
        currentBehaviour = States.idle;

        GameManager.Instance.Add_StartObserver(this);
        //TroubleManager.Instance.Add_TroubleObserver(this);

    }
    private void Start()
    {
        moneyTarget = GameObject.Find("MoneyTarget");
        moneylabel = GameObject.Find("moneyLabel").GetComponent<RectTransform>();
        moneyTarget.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(moneylabel.transform.position.x, moneylabel.transform.position.y, Camera.main.WorldToScreenPoint(moneyTarget.transform.position).z));

        spd = acceleration;
        //runnerCamera.Priority = 0;
        //idleCamera.Priority = 10;
        //runnerToIdleCamera.Priority = 0;
        idleCamera2.Priority = 0;
        parent = transform.parent.gameObject;
        playerParent = transform.parent.GetComponent<PlayerParent>();

        playerFirstPos = transform.position;
        Lawyer.SetActive(true);
        Hand.SetActive(false);
    }
    public void setCam()
    {
        if(slotNum >= 3)
        {

            //cam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 1;
            //cam.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 3;
        }
    }
    public void StartGame()
    {
        GameManager.Instance.Remove_StartObserver(this);
        //moneylabel = GameObject.Find("moneyLabel").GetComponent<RectTransform>();
        //moneyTarget.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(moneylabel.transform.position.x, moneylabel.transform.position.y, Camera.main.WorldToScreenPoint(moneyTarget.transform.position).z));
        playerParent = transform.parent.GetComponent<PlayerParent>();

        currentBehaviour = States.runner;
        anim.SetBool("walk" ,true);
   
        StartCoroutine(startDelay());
    }
    IEnumerator startDelay()
    {
        Debug.Log("start game1");
        anim.SetTrigger("flip");
        yield return new WaitForSeconds(0.5f);

        runnerCamera.Priority = 0;
        idleCamera.Priority = 0;
        runnerToIdleCamera.Priority = 10;
        yield return new WaitForSeconds(0.65f);
        runnerCamera.Priority = 10;
        idleCamera.Priority = 0;
        runnerToIdleCamera.Priority = 0;


        Lawyer.SetActive(false);
        Hand.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        runnerControlActive = true;
    }
    void forward()
    {
    }
    //void backward()
    //{
    //    backSpeed -= 10 * Time.deltaTime;
    //    transform.parent.transform.Translate(-transform.parent.transform.forward * Time.deltaTime * backSpeed);
    //    if (backSpeed <= 0)
    //    {
    //        backSpeed = 0;
    //        currentBehaviour = States.runner;
    //        m_previousX = Input.mousePosition.x;
    //        dX = 0f;
    //        dX_Sum = 0f;
    //    }

    //}

    public void moneyCollcet()
    {

    }

    private void Update()
    {
        switch (currentBehaviour)
        {
            case States.idle:
                {
                }
                break;
            case States.runner:
                {
                    transform.parent.transform.Translate(transform.parent.transform.forward * Time.deltaTime * acceleration);

                    if (runnerControlActive)
                    {
                        runnerControl();
                    }
                }
                break;
            case States.idleControl:
                {
                    if (idleControlActive)
                    {
                        IdleControl();
                    }
                }
                break;
            case States.runnerToIdle:
                {
                    changeSpeed();
                }
                break;




        }
        playerParent.horizontalFollowSpeed = Xmove;
    }
    private void changeSpeed()
    {
        spd -= 10 * Time.deltaTime;
        if (spd > 0)
        {
            transform.parent.transform.Translate(transform.parent.transform.forward * Time.deltaTime * spd);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0, transform.localPosition.y, transform.localPosition.z), Time.deltaTime);
        }
        else
        {
            currentBehaviour = States.idleControl;
        }
    }
    private void runnerControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_previousX = Input.mousePosition.x;
            dX = 0f;
            dX_Sum = 0f;
        }
        if (Input.GetMouseButton(0))
        {
            dX = (Input.mousePosition.x - m_previousX) / 10f;
            dX_Sum += dX;
            m_previousX = Input.mousePosition.x;
        }
        if (Input.GetMouseButtonUp(0))
        {
            dX_Sum = 0f;
            dX = 0f;
        }
        Xmove = Controlsensivity * dX / (Time.deltaTime * 25);
        Move(Xmove, Steer, acceleration);


    }
    public void Move(float _swipe, float _steering, float _speed)
    {
        if (_swipe > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerFirstPos.x + bounding, transform.position.y, transform.position.z), Time.deltaTime * Mathf.Abs(_swipe));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, 45, transform.eulerAngles.z), steeringSpeed * Time.deltaTime);
        }
        if (_swipe < 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerFirstPos.x - bounding, transform.position.y, transform.position.z), Time.deltaTime * Mathf.Abs(_swipe));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, -45, transform.eulerAngles.z), steeringSpeed * Time.deltaTime);
        }
        if (_swipe == 0)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.z), 2 * steeringSpeed * Time.deltaTime);
        }
    }




   

    public void IdleControl()
    {

        if (Input.GetMouseButtonDown(0))
        {
            m_previousX = Input.mousePosition.x;
            dX = 0f;
            m_previousY = Input.mousePosition.y;
            dY = 0f;

            firstPressPos = (Vector2)Input.mousePosition;
            pressed = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            secondPressPos = (Vector2)Input.mousePosition;
            currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            firstPressPos = (Vector2)Input.mousePosition;
            pressed = false;
            dX = 0f;
            dY = 0f;

        }

        if (pressed == true)
        {
            dX = (Input.mousePosition.x - m_previousX);
            dY = (Input.mousePosition.y - m_previousY);

            anim.SetBool("walk", true);
            secondPressPos = (Vector2)Input.mousePosition;
            currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            currentSwipe.Normalize();


            Vector3 direction = new Vector3(currentSwipe.x, 0f, currentSwipe.y);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion newRot = Quaternion.Euler(0, targetAngle, 0);


            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, 300 * Time.deltaTime);
            parent.transform.position = parent.transform.position + (direction * speed * Time.deltaTime);
            if(Vector2.Distance(secondPressPos,firstPressPos) > 100f)
            {
                firstPressPos += new Vector2(dX, dY);
            }
            m_previousX = Input.mousePosition.x;
            m_previousY = Input.mousePosition.y;
        }
        else
        {
            anim.SetBool("walk", false);
        }
    }

    public void troubleCheck()
    {
        //rainParticle.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("change  1");

        if (other.transform.tag == "wall")
        {
            idleCamera2.Priority = 20;
            Debug.Log("change2");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "wall")
        {
            idleCamera2.Priority = 0;
        }
    }

    //IEnumerator targetMotion(GameObject money)
    //{
    //    while (Vector3.Distance(money.transform.position, moneyTarget.transform.position) > 0.3f)
    //    {
    //        money.transform.position = Vector3.MoveTowards(money.transform.position, moneyTarget.transform.position, (3 / Vector3.Distance(money.transform.position, moneyTarget.transform.position)) * acceleration * Time.deltaTime);
    //        money.transform.localScale = Vector3.Lerp(money.transform.localScale, moneyTarget.transform.localScale, acceleration * 0.3f * Time.deltaTime);
    //        yield return null;
    //    }
    //    //LevelScore.Instance.MoneyUpdate(money.transform.GetComponent<MoneyCollecting>().moneyValue);

    //    money.transform.parent = null;
    //    Destroy(money);
    //}
}
