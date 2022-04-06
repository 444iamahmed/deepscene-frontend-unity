using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    public GameObject destination;

    private float moveSpeed = 3f;
    private float minDistance = 3f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (destination != null && (destination.transform.position - transform.position).magnitude > minDistance)
            transform.Translate((destination.transform.position - transform.position).normalized * moveSpeed * Time.deltaTime);
        else
            animator.Play(null);
    }
}
