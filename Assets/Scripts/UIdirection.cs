using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIdirection : MonoBehaviour, IisTrouble, ITroubleFix, IRunner
{
    public List<GameObject> targetList;
    Vector3 direction;
    Vector3 distance;
    bool followActive = false;
    Transform player;
    int selectionTarget;
    public Sequence sequence;
    bool troubleActive = false;
    private void Start()
    {
        sequence = DOTween.Sequence();
        sequence.Kill();
        TroubleManager.Instance.Add_TroubleFixObserver(this);
        TroubleManager.Instance.Add_isTroubleObserver(this);
        GameManager.Instance.Add_RunnerStartObserver(this);
    }
    public void selectTarget(int _selectionTarget, Transform _player)
    {
        GetComponent<Image>().enabled = true;
        player = _player;
        selectionTarget = _selectionTarget;
        followActive = true;
    }
   public void arrowScaleSet()
    {
        sequence.Kill();
        StartCoroutine(arrowLoopScale());

    }
    IEnumerator arrowLoopScale()
    {
        yield return new WaitForSeconds(0.5f);
        sequence = DOTween.Sequence();

        sequence.Append(transform.GetComponent<RectTransform>().DOScale(Vector3.one * 0.4f, 0.3f).SetLoops(-1, LoopType.Yoyo));

        sequence.AppendInterval(0f);
        sequence.SetLoops(-1, LoopType.Yoyo);
        sequence.SetRelative(true);
    }
    public void seqKill()
    {
        sequence.Kill();
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
        GetComponent<RectTransform>().anchoredPosition = new Vector3(direction.x  * distX * Screen.width / 5 + magnX, direction.z* distZ * Screen.height / 48 + magnZ , 0);


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
    public void runnerStar()
    {
        troubleActive = false;
        followActive = false;
        //GetComponent<Image>().enabled = false;

        //TroubleManager.Instance.Remove_isTroubleObserver(this);
        TroubleManager.Instance.Remove_TroubleFixObserver(this);

        GameManager.Instance.Remove_RunnerStartObserver(this);
    }
    public void isTrouble()
    {
        troubleActive = true;
        this.StartCoroutine(loopColorScaleSet());
        this.StartCoroutine(scaleSet());
        TroubleManager.Instance.Remove_isTroubleObserver(this);

    }
    public void torubleFix()
    {
        troubleActive = false;
    }
    IEnumerator loopColorScaleSet()
    {
        float counter1 = 0f;
        float scaleValue1 = 0f;
        while (troubleActive)
        {
            counter1 += 5 * Time.deltaTime;
            scaleValue1 = Mathf.Abs(Mathf.Cos(counter1));
            GetComponent<Image>().color = new Color(1, 1 - scaleValue1, 1 - scaleValue1);
            yield return null;
        }
        GetComponent<Image>().color = new Color(1, 1, 1);
    }
    IEnumerator scaleSet()
    {
        float counter1 = 0f;
        float scaleValue1 = 0f;
        while (troubleActive)
        {
            counter1 += 20 * Time.deltaTime;
            scaleValue1 = Mathf.Abs(Mathf.Cos(counter1));
            transform.localScale = Vector3.one + new Vector3(scaleValue1 / 5f, scaleValue1 / 5f, scaleValue1 / 5f);

            yield return null;
        }
        transform.localScale = Vector3.one;
    }
}
