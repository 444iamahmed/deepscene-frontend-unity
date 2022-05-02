using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{

    SceneGraph graph;
    // Start is called before the first frame update
    void Start()
    {
        graph = FindObjectOfType<SceneGraph>();
        graph.ComputeDiagnoals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
