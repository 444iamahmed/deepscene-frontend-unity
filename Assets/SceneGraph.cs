 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using QuickGraph;


[Serializable]
public class SceneGraph : MonoBehaviour
{

    public List<Tuple> graph { get; set; }


    float directionConeSize = 90f;

    Vector3 secondaryDiagonalStart;
    Vector3 secondaryDiagonalEnd;
    Vector3 primaryDiagonalStart;
    Vector3 primaryDiagonalEnd;


    [SerializeField] float distanceCloseRange = 30f;
    [SerializeField] float distanceFurtherRange = 100f;


    Dictionary<string, GameObject> objects = new Dictionary<string, GameObject>();

    Manager manager;
    void Start()
    {
        graph = new List<Tuple>();
        manager = GetComponent<Manager>();
    }
    public void AddTuple(Tuple tuple)
    {
        
        if (!objects.ContainsKey(tuple.sub))
        {
            tuple.subGameObject = Instantiate(Resources.Load<GameObject>("Prefabs/" + tuple.sub));
            tuple.subGameObject.transform.Rotate(0f, UnityEngine.Random.Range(0f, 360f), 0f);
            objects.Add(tuple.sub, tuple.subGameObject);
        }
        else
            tuple.subGameObject = objects[tuple.sub];

        if (!objects.ContainsKey(tuple.obj))
        {
            tuple.objGameObject = Instantiate(Resources.Load<GameObject>("Prefabs/" + tuple.obj));
            tuple.objGameObject.transform.Rotate(0f, UnityEngine.Random.Range(0f, 360f), 0f);
            objects.Add(tuple.obj, tuple.subGameObject);
        }
        else
            tuple.objGameObject = objects[tuple.obj];

        graph.Add(tuple);

    }

    public void Evaluate()
    {
        ComputeDiagnoals();


        foreach (Tuple tuple in graph)
        {
            Vector3 subToObj =  tuple.objGameObject.transform.position - tuple.subGameObject.transform.position;
            EvaluateDirection(tuple, subToObj);
            EvaluateDistance(tuple, subToObj);

        }
        //Debug.Log(JsonHelper.makeJsonFromArray<Tuple>(graph.ToArray()));
        manager.Save();
    }

   
    void EvaluateDistance(Tuple tuple, Vector3 subToObj)
    {
        if (subToObj.magnitude < distanceCloseRange)
            tuple.distance = Distance.CLOSE;
        else if (subToObj.magnitude >= distanceCloseRange && subToObj.magnitude < distanceFurtherRange)
            tuple.distance = Distance.FURTHER;
        else
            tuple.distance = Distance.FAR;

        Debug.Log(tuple.distance);
    }

    void EvaluateDirection(Tuple tuple, Vector3 subToObj)
    {
        float x = (primaryDiagonalStart.z * primaryDiagonalEnd.x - primaryDiagonalStart.x * primaryDiagonalEnd.z) * (primaryDiagonalStart.z * subToObj.x - primaryDiagonalStart.x * subToObj.z);
        float y = (secondaryDiagonalStart.z * secondaryDiagonalEnd.x - secondaryDiagonalStart.x * secondaryDiagonalEnd.z) * (secondaryDiagonalStart.z * subToObj.x - secondaryDiagonalStart.x * subToObj.z);

        if (x < 0 && y >= 0)
            tuple.direction = Direction.BEHIND;
        else if (x >= 0 && y >= 0)
            tuple.direction = Direction.LEFT;
        else if (x >= 0 && y < 0)
            tuple.direction = Direction.FRONT;
        else
            tuple.direction = Direction.RIGHT;

        Debug.Log(tuple.direction);
        //(primaryDiagonal.z * secondaryDiagonal.x - primaryDiagonal.x * secondaryDiagonal.z) * (primaryDiagonal.z * direction.x - primaryDiagonal.x * direction.z) < 0;

    }

    void ComputeDiagnoals()
    {
        primaryDiagonalStart = Quaternion.AngleAxis(45, Vector3.up) * Camera.main.transform.right;
        primaryDiagonalEnd = Quaternion.AngleAxis(-45, Vector3.up) * Camera.main.transform.right;

        secondaryDiagonalStart = Quaternion.AngleAxis(-45, Vector3.up) * Camera.main.transform.right;
        secondaryDiagonalEnd = Quaternion.AngleAxis(45, Vector3.up) * Camera.main.transform.right;
    }
}

//expected format
//{
//    "objects": [
//      "sky", "man", "leg", "horse", "tail", "leg",
//      "short", "hill", "hill"
//    ],
//    "relationships": [
//      [0, "above", 1],
//      [1, "has", 2],
//      [1, "riding", 3],
//      [3, "has", 4],
//      [3, "has", 4],
//      [3, "has", 5]
//    ]
//  }