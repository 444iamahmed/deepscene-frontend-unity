using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class SceneGraph : MonoBehaviour
{


    TMP_InputField inputField;


    //public List<Tuple> graph { get; set; }
    public Dictionary<GameObject, List<Tuple>> graph;
    public Dictionary<GameObject, List<GameObject>> undirectedGraph;

    //float directionConeSize = 90f;

    public Vector3 secondaryDiagonalStart;
    public Vector3 secondaryDiagonalEnd;
    public Vector3 primaryDiagonalStart;
    public Vector3 primaryDiagonalEnd;


    public float distanceCloseRange = 30f;
    public float distanceFurtherRange = 100f;
    public float distanceFarRange = 200f;
    public float verticalThreshold = 70f;
    public Transform reference;

    Dictionary<string, GameObject> objects = new Dictionary<string, GameObject>();



    Manager manager;



    HashSet<GameObject> visited;
    void Start()
    {
        //graph = new List<Tuple>();

        reference = GameObject.FindGameObjectWithTag("Reference").transform;
        graph = new Dictionary<GameObject, List<Tuple>>();
        undirectedGraph = new Dictionary<GameObject, List<GameObject>>();
        manager = GetComponent<Manager>();
    }
    public void AddTuple(Tuple tuple)
    {
        //bool subFlag = false;
        //bool objFlag = false;

        GameObject subGameObject; 
        GameObject objGameObject;


        if (!objects.ContainsKey(tuple.sub))
        {
            tuple.subGameObject = manager.AddInstanceMapping(Instantiate(Resources.Load<GameObject>("Prefabs/" + tuple.sub)));
            subGameObject = manager.GetGameObjectFromInstanceID(tuple.subGameObject);
            subGameObject.transform.Rotate(0f, UnityEngine.Random.Range(0f, 360f), 0f);
            objects.Add(tuple.sub, subGameObject);
            //subFlag = true;
        }
        else
        {
            tuple.subGameObject = objects[tuple.sub].GetInstanceID();
            subGameObject = manager.GetGameObjectFromInstanceID(tuple.subGameObject);

        }
        if (!objects.ContainsKey(tuple.obj))
        {
            tuple.objGameObject = manager.AddInstanceMapping(Instantiate(Resources.Load<GameObject>("Prefabs/" + tuple.obj)));
            objGameObject = manager.GetGameObjectFromInstanceID(tuple.objGameObject);
            objGameObject.transform.Rotate(0f, UnityEngine.Random.Range(0f, 360f), 0f);
            objects.Add(tuple.obj, objGameObject);
            //objFlag = true;
        }
        else
        {
            tuple.objGameObject = objects[tuple.obj].GetInstanceID();
            objGameObject = manager.GetGameObjectFromInstanceID(tuple.objGameObject);

        }
        if (!graph.ContainsKey(subGameObject))
            graph.Add(subGameObject, new List<Tuple>());

        if(!undirectedGraph.ContainsKey(subGameObject))
            undirectedGraph.Add(subGameObject, new List<GameObject>());

        

        if (!graph.ContainsKey(objGameObject))
            graph.Add(objGameObject, new List<Tuple>());


        if(!undirectedGraph.ContainsKey(objGameObject))
            undirectedGraph.Add(objGameObject, new List<GameObject>());

        bool exists = false;
            foreach(var list in graph.Values)
            {
                foreach(Tuple t in list)
                if (t.objGameObject == tuple.objGameObject && t.subGameObject == tuple.subGameObject)
                {
                    exists = true;
                }
            }
        if (!exists)
        {
            graph[subGameObject].Add(tuple);
            undirectedGraph[subGameObject].Add(objGameObject);
            undirectedGraph[objGameObject].Add(subGameObject);
        }//graph.Add(tuple);


        Debug.Log(JsonUtility.ToJson(JsonUtility.FromJson<Tuple>(JsonUtility.ToJson(tuple))));
        
    }

    public void Evaluate()
    {
        ComputeDiagnoals();


        foreach (KeyValuePair<GameObject, List<Tuple>> kvp in graph)
        {
            foreach (Tuple tuple in kvp.Value)
            {
                Vector3 subToObj = manager.GetGameObjectFromInstanceID(tuple.objGameObject).transform.position - manager.GetGameObjectFromInstanceID(tuple.subGameObject).transform.position;
                EvaluateDirection(tuple, subToObj);
                EvaluateDistance(tuple, subToObj);
            }
        }
        //Debug.Log(JsonHelper.makeJsonFromArray<Tuple>(graph.ToArray()));
        manager.Save();
    }

   public void TrainInstance()
   {
        manager.Reeval();
   }
    void EvaluateDistance(Tuple tuple, Vector3 subToObj)
    {
        if (subToObj.magnitude < distanceCloseRange)
            tuple.distanceTruth = Distance.CLOSE;
        else if (subToObj.magnitude >= distanceCloseRange && subToObj.magnitude < distanceFurtherRange)
            tuple.distanceTruth = Distance.FURTHER;
        else
            tuple.distanceTruth = Distance.FAR;

        //Debug.Log(tuple.distanceTruth);
    }

    void EvaluateDirection(Tuple tuple, Vector3 subToObj)
    {
        float x = (primaryDiagonalStart.z * primaryDiagonalEnd.x - primaryDiagonalStart.x * primaryDiagonalEnd.z) * (primaryDiagonalStart.z * subToObj.x - primaryDiagonalStart.x * subToObj.z);
        float y = (secondaryDiagonalStart.z * secondaryDiagonalEnd.x - secondaryDiagonalStart.x * secondaryDiagonalEnd.z) * (secondaryDiagonalStart.z * subToObj.x - secondaryDiagonalStart.x * subToObj.z);


        Debug.Log(x);
        Debug.Log(y);
        if (manager.GetGameObjectFromInstanceID(tuple.subGameObject).transform.position.y > manager.GetGameObjectFromInstanceID(tuple.objGameObject).transform.position.y && subToObj.magnitude > verticalThreshold)
            tuple.directionTruth = Direction.UP;
        else if (manager.GetGameObjectFromInstanceID(tuple.subGameObject).transform.position.y < manager.GetGameObjectFromInstanceID(tuple.objGameObject).transform.position.y && subToObj.magnitude > verticalThreshold)
            tuple.directionTruth = Direction.DOWN;
        else if (x < 0 && y >= 0)
            tuple.directionTruth = Direction.RIGHT;
        else if (x >= 0 && y >= 0)
            tuple.directionTruth = Direction.FRONT;
        else if (x >= 0 && y < 0)
            tuple.directionTruth = Direction.LEFT;
        else
            tuple.directionTruth = Direction.BEHIND;

        Debug.Log(tuple.directionTruth);
        //(primaryDiagonal.z * secondaryDiagonal.x - primaryDiagonal.x * secondaryDiagonal.z) * (primaryDiagonal.z * direction.x - primaryDiagonal.x * direction.z) < 0;

    }

    public void ComputeDiagnoals()
    {
        primaryDiagonalStart = Quaternion.AngleAxis(45, Vector3.up) * reference.right;
        primaryDiagonalEnd = Quaternion.AngleAxis(-45, Vector3.up) * reference.right;

        secondaryDiagonalStart = Quaternion.AngleAxis(-45, Vector3.up) * -reference.right;
        secondaryDiagonalEnd = Quaternion.AngleAxis(45, Vector3.up) * -reference.right;
    }

    public void SetPredictions(Tuple tuple)
    {
        foreach (KeyValuePair<GameObject, List<Tuple>> kvp in graph)
        {
            foreach (Tuple t in kvp.Value)
            {
                if (manager.GetGameObjectFromInstanceID(t.subGameObject) == manager.GetGameObjectFromInstanceID(tuple.subGameObject) && manager.GetGameObjectFromInstanceID(t.objGameObject) == manager.GetGameObjectFromInstanceID(tuple.objGameObject))
                {
                    t.directionPrediction = tuple.directionPrediction;
                    t.distancePrediction = tuple.distancePrediction;
                }
            }
        }
    }

    public Tuple[] GetTuples()
    {
        List<Tuple> retTuples = new List<Tuple>();
        foreach(List<Tuple> tuples in graph.Values)
        {
            retTuples.AddRange(tuples);
        }

        return retTuples.ToArray();
    }
    public void ApplyPredictions()
    {
        
        var components = FindComponents();
        foreach (var component in components)
        {
            //Debug.Log("new component");
            foreach(var node in component.Keys)
            {
                //Debug.Log(node.name);
                if (component[node].Count <= 0)
                {
                    SetRandomPosition(node);

                    SetRelativePositions(node, component);
                    

                }   
            }
            
        }
    }

    void SetRelativePositions(GameObject child, Dictionary<GameObject, List<Tuple>> component)
    {
        Debug.Log("Calling");
        foreach (var possibleParent in component.Keys)
        {
            //Debug.Log(child.name);
            foreach (var t in component[possibleParent])
                if (t.objGameObject == child.GetInstanceID())
                {
                    //DummyFill(t);
                    SetRelativePosition(t);
                    SetRelativePositions(manager.GetGameObjectFromInstanceID(t.subGameObject), component);
                }
        }
    }

    void DummyFill(Tuple t)
    {
        t.directionPrediction = t.directionTruth;
        t.distancePrediction = t.distanceTruth;
        t.animate = false;
    }
    void SetRelativePosition(Tuple tuple)
    {
        Debug.Log(tuple.directionPrediction);
        Debug.Log(tuple.distancePrediction);
        if (tuple.animate)
        {
            Debug.Log("ANIMATING");
            ApplyAnimation animator = manager.GetGameObjectFromInstanceID(tuple.subGameObject).GetComponent<ApplyAnimation>();
            animator.target = manager.GetGameObjectFromInstanceID(tuple.objGameObject).transform.position;
            
            animator.apply = true;
        }
        Ray ray = new Ray(manager.GetGameObjectFromInstanceID(tuple.objGameObject).transform.position, Vector3.zero);
        if (tuple.directionPrediction == Direction.LEFT)
        {
            ray.direction = Quaternion.AngleAxis(Random.Range(-45f, 45f), Vector3.up) * -reference.right;
        }
        else if (tuple.directionPrediction == Direction.RIGHT)
        {
            ray.direction = Quaternion.AngleAxis(Random.Range(-45f, 45f), Vector3.up) * reference.right;
        }
        else if (tuple.directionPrediction == Direction.FRONT)
        {
            ray.direction = Quaternion.AngleAxis(Random.Range(-45f, 45f), Vector3.up) * -reference.forward;
        }
        else if (tuple.directionPrediction == Direction.BEHIND)
        {
            ray.direction = Quaternion.AngleAxis(Random.Range(-45f, 45f), Vector3.up) * reference.forward;
        }
        else if (tuple.directionPrediction == Direction.UP)
        {
            ray.direction = Quaternion.AngleAxis(Random.Range(-45f, 45f), Vector3.forward) * reference.up;
        }
        else
            ray.direction = Quaternion.AngleAxis(-Random.Range(-45f, 45f), Vector3.forward) * -reference.up;

        //Gizmos.DrawLine(ray.origin, ray.GetPoint(200f));


        if (tuple.distancePrediction == Distance.CLOSE)
            manager.GetGameObjectFromInstanceID(tuple.subGameObject).transform.position = ray.GetPoint(Random.Range(5f, distanceCloseRange));
        else if (tuple.distancePrediction == Distance.FURTHER)
            manager.GetGameObjectFromInstanceID(tuple.subGameObject).transform.position = ray.GetPoint(Random.Range(distanceCloseRange, distanceFurtherRange));
        else
            manager.GetGameObjectFromInstanceID(tuple.subGameObject).transform.position = ray.GetPoint(Random.Range(distanceFurtherRange, distanceFarRange));
    }
    void SetRandomPosition(GameObject gameObject)
    {
        gameObject.transform.position = Vector3.zero;
        //RaycastHit hit;
        //Physics.Raycast(Camera.main.ViewportPointToRay(Random.insideUnitCircle), out hit);
        //    gameObject.transform.position = hit.point;
    }
    List<Dictionary<GameObject, List<Tuple>>> FindComponents()
    {
        visited = new HashSet<GameObject>();

        List<Dictionary<GameObject, List<Tuple>>> components = new List<Dictionary<GameObject, List<Tuple>>>();

        foreach (GameObject node in undirectedGraph.Keys)
        {
            if (!visited.Contains(node))
            {
                components.Add(new Dictionary<GameObject, List<Tuple>>());
                FindComponentsUtil(node, components.Last());
            }
        }

        foreach (Dictionary<GameObject, List<Tuple>> component in components)
        {
            Debug.Log("new component");
            foreach (GameObject node in component.Keys)
                Debug.Log(node.name);
        }
        return components; 
    }
    void FindComponentsUtil(GameObject node, Dictionary<GameObject, List<Tuple>> component)
    {
        if(visited == null)
            visited = new HashSet<GameObject> ();

        visited.Add(node);

        if (undirectedGraph.ContainsKey(node))
        {
            component.Add(node, graph[node]);
            foreach (GameObject neighbour in undirectedGraph[node])
            {
                if (!visited.Contains(neighbour)) 
                    FindComponentsUtil(neighbour, component);
            }
        }

    }
    private void Update()
    {
        
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