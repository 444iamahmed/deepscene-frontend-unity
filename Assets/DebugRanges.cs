using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRanges : MonoBehaviour
{

    SceneGraph sceneGraph;
    float duration = 1000f;

    [SerializeField] float angle1;
    [SerializeField] float angle2;
    [SerializeField] float angle3;
    [SerializeField] float angle4;
    // Start is called before the first frame update
    void Start()
    {
        sceneGraph = FindObjectOfType<SceneGraph>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, new Ray(transform.position, sceneGraph.primaryDiagonalStart).GetPoint(duration), Color.red);
        Debug.DrawLine(transform.position, new Ray(transform.position, sceneGraph.primaryDiagonalEnd).GetPoint(duration), Color.green);
        Debug.DrawLine(transform.position, new Ray(transform.position, sceneGraph.secondaryDiagonalStart).GetPoint(duration), Color.blue);
        Debug.DrawLine(transform.position, new Ray(transform.position, sceneGraph.secondaryDiagonalEnd).GetPoint(duration), Color.yellow);

        //Debug.DrawLine(transform.position, new Ray(transform.position, Quaternion.AngleAxis(angle1, Vector3.up) * sceneGraph.reference.right).GetPoint(duration), Color.red);
        //Debug.DrawLine(transform.position, new Ray(transform.position, Quaternion.AngleAxis(angle2, Vector3.up) * -sceneGraph.reference.right).GetPoint(duration), Color.white);
        //Debug.DrawLine(transform.position, new Ray(transform.position, Quaternion.AngleAxis(angle3, Vector3.up) * sceneGraph.reference.forward).GetPoint(duration), Color.blue);
        //Debug.DrawLine(transform.position, new Ray(transform.position, Quaternion.AngleAxis(angle4, Vector3.up) * -sceneGraph.reference.forward).GetPoint(duration), Color.yellow);

        //if(Input.GetMouseButtonDown(0))
        //{
        //    sceneGraph.Evaluate();
        //}
    }


}
