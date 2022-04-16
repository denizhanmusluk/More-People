using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    protected Animator[] animator;
    protected Rigidbody Rigidbody;
    protected CapsuleCollider capsuleCollider;
    protected Collider[] childrenCollider;
    protected Rigidbody[] childrenRigidbody;

    void Awake()
    {
        animator = GetComponentsInChildren<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        childrenCollider = GetComponentsInChildren<Collider>();
        childrenRigidbody = GetComponentsInChildren<Rigidbody>();
    }

    private void Start()
    {
        RagdollActivate(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RagdollActivate(true);

        }
    }

    public void RagdollActivate(bool active)
    {
        foreach (var collider in childrenCollider)
            collider.enabled = active;
        foreach (var rigidb in childrenRigidbody)
        {
            rigidb.detectCollisions = active;
            rigidb.isKinematic = !active;
            //rigidb.drag = 10;
        }

        //rest
        foreach (var anims in animator)
        {
            anims.enabled = !active;
        }

        Rigidbody.detectCollisions = !active;
        Rigidbody.isKinematic = false;
        capsuleCollider.enabled = !active;
    }
}
