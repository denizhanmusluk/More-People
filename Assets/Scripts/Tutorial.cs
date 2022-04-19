using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class Tutorial : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera camTutorial;
    [SerializeField] GameObject arrow;
    Sequence sequence;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            arrow.SetActive(true);
            tweenScale();
            StartCoroutine(camSet());
            GetComponent<Collider>().enabled = false;
        }
    }
    IEnumerator camSet()
    {
        yield return new WaitForSeconds(2f);
        camTutorial.Priority = 20;
        yield return new WaitForSeconds(2.5f);
        camTutorial.Priority = 0;
        yield return new WaitForSeconds(6f);
        sequence.Kill(this);
        arrow.SetActive(false);

    }

    void tweenScale()
    {
        sequence = DOTween.Sequence();
        sequence.Append(arrow.transform.DOScale(Vector3.one * 0.2f, 0.3f).SetLoops(-1, LoopType.Yoyo));

        sequence.AppendInterval(0f);
        sequence.SetLoops(-1, LoopType.Yoyo);
        sequence.SetRelative(true);
    }
}