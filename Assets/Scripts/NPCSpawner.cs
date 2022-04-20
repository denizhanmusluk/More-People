using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour,IStartGameObserver
{
    //public static NPCSpawner Instance;

    public GameObject[] npcPrefab;
    public float spawnTime = 4f;
    [SerializeField] public List<Transform> target;
    public bool spawnActive = true;
    GameObject player;
    private void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //}
        GameManager.Instance.Add_StartObserver(this);
    }
   public void StartGame()
    {
 
        StartCoroutine(SpawnCustomer());
        GameManager.Instance.Remove_StartObserver(this);

    }
    private void OnEnable()
    {

    }
    IEnumerator SpawnCustomer()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("spawn start");
        player = GameObject.Find("Player");
        while (spawnActive)
        {
            //Debug.Log("spawn active");

            if (Vector3.Distance(player.transform.position,transform.position) < 15f)
            {
                spawnActive = false;
                StartCoroutine(playerDistanceCheck());
                break;
            }
            yield return new WaitForSeconds(20 * spawnTime / (2 + Globals.universityLevel + Globals.farmvilleLevel + Globals.policeStationLevel + Globals.hospitalLevel));

            if (Vector3.Distance(player.transform.position, transform.position) < 15f)
            {
                spawnActive = false;
                StartCoroutine(playerDistanceCheck());
                break;
            }
            GameObject _npc = Instantiate(npcPrefab[Random.Range(0, npcPrefab.Length)], transform.position, transform.rotation, this.transform);
            int selectTarget = Random.Range(0, target.Count);
            _npc.GetComponent<NPC>().destination = target[selectTarget];
            target[selectTarget].parent.GetComponent<Build>().customerList.Add(_npc);
            if (target[selectTarget].parent.GetComponent<Build>().troubleActive)
            {
                if (target[selectTarget].parent.GetComponent<Build>().buildNo == 1)
                {
                    _npc.GetComponent<NPC>().currentSelection = NPC.States.troubleHospital;
                    _npc.GetComponent<NPC>()._randomEmoji();

                }
                if (target[selectTarget].parent.GetComponent<Build>().buildNo == 2)
                {
                    _npc.GetComponent<NPC>().currentSelection = NPC.States.troublePolice;
                    _npc.GetComponent<NPC>()._randomDead();
                }
                if (target[selectTarget].parent.GetComponent<Build>().buildNo == 3)
                {
                    //_npc.GetComponent<NPC>().troubleFarm();
                    _npc.GetComponent<NPC>().currentSelection = NPC.States.troubleFarm;

                }
                if (target[selectTarget].parent.GetComponent<Build>().buildNo == 4)
                {

                }

            }
            else
            {
                _npc.GetComponent<NPC>().currentSelection = NPC.States.moveTarget;
            }

        }
    }
    IEnumerator playerDistanceCheck()
    {
        while (!spawnActive)
        {
            if (Vector3.Distance(player.transform.position, transform.position) > 16f)
            {
                spawnActive = true;

                StartCoroutine(SpawnCustomer());
            }
            yield return null;
        }
    }
}
