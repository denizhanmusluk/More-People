using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class guilty : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform destination;
    [SerializeField] public ParticleSystem gunParticle;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(destination.position);

    }
}
