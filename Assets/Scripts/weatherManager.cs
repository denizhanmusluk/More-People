using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weatherManager : MonoBehaviour, ITroubleFix,IisTrouble,IRunner,IFinish
{
    bool weatherActive = false;
    int troubleLevel = 0;
    [SerializeField] public GameObject directionLight;
    [SerializeField] float firstLight = 1.1f, rainLight = 0.6f;
    bool rainCheckActive = true;
    [SerializeField] ParticleSystem rainParticle;
    ParticleSystem.EmissionModule rainEmission;
    float rainMaxEmission = 300f;
    void Start()
    {
        rainEmission = rainParticle.emission;
        directionLight = GameObject.Find("DirectionalLight");
        TroubleManager.Instance.Add_TroubleFixObserver(this);
        TroubleManager.Instance.Add_isTroubleObserver(this);
        GameManager.Instance.Add_RunnerStartObserver(this);
        GameManager.Instance.Add_FinishObserver(this);
    }
    public void finishRunner()
    {
        weatherActive = true;
    }
    public void runnerStar()
    {
        GameManager.Instance.Remove_FinishObserver(this);
        GameManager.Instance.Remove_RunnerStartObserver(this);
        weatherActive = false;
    }
    public void torubleFix()
    {
        troubleLevel--;

    }
    public void isTrouble()
    {
        troubleLevel++;
        StartCoroutine(rainCheck());
    }
    IEnumerator rainCheck()
    {
        while (rainCheckActive)
        {
            if (weatherActive)
            {
                rainCheckActive = false;
                StartCoroutine(rainlySun());
                rainParticle.gameObject.SetActive(true);
                rainEmission.rateOverTime = 0f;
            }
            yield return null;
        }
    }
    IEnumerator rainlySun()
    {
        yield return new WaitForSeconds(2f);
        float cntr = firstLight;
        while (cntr > rainLight)
        {
            cntr -= 0.05f * Time.deltaTime;
            directionLight.GetComponent<Light>().intensity = cntr;


            yield return null;
        }
        directionLight.GetComponent<Light>().intensity = rainLight;
        StartCoroutine(rainStart());

        while (troubleLevel > 0)
        {
            int flashCount = Random.Range(1, 4);
            for (int i = 0; i < flashCount; i++)
            {
                float counter = 0;
                float lightDelta;
                directionLight.GetComponent<Light>().intensity = rainLight;

                while (counter < Mathf.PI)
                {
                    counter += 10 * Time.deltaTime;
                    lightDelta = 1-Mathf.Abs(Mathf.Cos(counter));
                    //lightDelta *= firstLight - rainLight;
                    directionLight.GetComponent<Light>().intensity = rainLight + lightDelta;
                    yield return null;
                }
                yield return new WaitForSeconds(Random.Range(0.3f, 0.6f));
            }
            yield return new WaitForSeconds(Random.Range(3f, 6f));
        }
        StartCoroutine(rainStop());

    }
    IEnumerator rainStart()
    {
        float counter = 0f;
        while(counter < rainMaxEmission)
        {
            counter += 50 * Time.deltaTime;
            rainEmission.rateOverTime = counter;

            yield return null;
        }
        rainEmission.rateOverTime = rainMaxEmission;
    }
    IEnumerator rainStop()
    {
        float counter = rainMaxEmission;
        while (counter > 0)
        {
            counter -= 1500 * Time.deltaTime;
            rainEmission.rateOverTime = counter;

            yield return null;
        }
        rainEmission.rateOverTime = 0;
        rainParticle.gameObject.SetActive(false);

    }
}
