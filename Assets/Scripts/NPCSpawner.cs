using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    //public static NPCSpawner Instance;

    public GameObject npcPrefab;
    public float spawnTime = 4f;
    [SerializeField] public List<Transform> target;
    private void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //}
    }
    void Start()
    {
        StartCoroutine(SpawnCustomer());
    }

    IEnumerator SpawnCustomer()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);

            GameObject _npc = Instantiate(npcPrefab, transform.position, transform.rotation, this.transform);
            int selectTarget = Random.Range(0, target.Count);
            _npc.GetComponent<NPC>().destination = target[selectTarget];
            target[selectTarget].parent.GetComponent<Build>().customerList.Add(_npc);
            if (target[selectTarget].parent.GetComponent<Build>().troubleActive)
            {
                _npc.GetComponent<NPC>().currentSelection = NPC.States.trouble;
                _npc.GetComponent<NPC>()._randomDead();
            }
            else
            {
                _npc.GetComponent<NPC>().currentSelection = NPC.States.moveTarget;
            }
        }
    }
}
