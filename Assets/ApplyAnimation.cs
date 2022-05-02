using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyAnimation : MonoBehaviour
{

    public bool apply = false;
    SceneGraph graph;
    public Vector3 target;

    Vector3 velocity = Vector3.zero;
    float smoothTime = 20f;
    // Start is called before the first frame update
    void Start()
    {
        graph = FindObjectOfType<SceneGraph>();
    }

    // Update is called once per frame
    void Update()
    {
        if(apply && target != null && Vector3.Distance(transform.position, target) > graph.distanceCloseRange)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
        }
    }

}
