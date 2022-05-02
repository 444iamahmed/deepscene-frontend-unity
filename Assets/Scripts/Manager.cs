using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Text;
using TMPro;
using Newtonsoft.Json;

public class Manager : MonoBehaviour
{

    

    TMP_InputField inputField;

    public string server = "http://localhost:9001";

    [SerializeField] string tagSpeechCall;
    [SerializeField] string tuplesCall;
    [SerializeField] string saveTuplesCall;
    [SerializeField] string reevalCall;
    [SerializeField] string predictCall;

    SceneGraph graph;

    Dictionary<int, GameObject> instanceMappings = new Dictionary<int, GameObject> ();
    // Start is called before the first frame update
    void Start()
    {
        inputField = GameObject.FindGameObjectWithTag("SceneDescription").GetComponent<TMP_InputField>();
        StartCoroutine(GetTags());
        graph = FindObjectOfType<SceneGraph>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.anyKeyDown)
        //{
        //    Debug.Log("CALLED");
        //    StartCoroutine(GetTuples());
        //}
    }

    public void Generate()
    {
        //StartCoroutine(GetTuples());
        StartCoroutine(Predict());

    }

    IEnumerator Predict()
    {
        string request = server + "/" + predictCall + "/";

        Debug.Log(request);
        UnityWebRequest www = new UnityWebRequest(request, "POST");
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes("{ \"text\": \"" + inputField.text + "\" }"));
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            var x = www.downloadHandler.text;
            x = x.Trim('\"');
            x = x.Replace("\\\"", "\"");


            var y = JsonConvert.DeserializeObject<List<Tuple>>(x);

            Debug.Log(y[0].sub);
            Debug.Log(y[0].obj);
            Debug.Log(y[0].relation);
            Debug.Log(y[0].relation);
            Debug.Log(y[0].directionPrediction);

            // Or retrieve results as binary data
            foreach (Tuple tuple in y)
            {
                graph.AddTuple(tuple);
            }
            graph.ApplyPredictions();
        }

    }
    IEnumerator GetTags()
    {

        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();


        //formData.Add(new MultipartFormDataSection("text", inputText));
        string request = server + "/" + tagSpeechCall + "/";

        Debug.Log(request);
        UnityWebRequest www = new UnityWebRequest(request, "POST");
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes("{ \"text\": \"" + inputField.text + "\" }"));
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
       
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);


            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
        
    }

    IEnumerator GetTuples()
    {
        string inputText = inputField.text;
        
        UnityWebRequest www = UnityWebRequest.Get(server + "/" + tuplesCall + "/" + inputText);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            
            // Show results as text
            Debug.Log(www.downloadHandler.text.ToString());

            var x = www.downloadHandler.text;
            x = x.Trim('\"');
            x = x.Replace("\\\"", "\"");


            var y = JsonConvert.DeserializeObject<List<Tuple>>(x);
            //List<GameObject> objects = new List<GameObject>();   

            foreach (Tuple tuple in y)
            {
                graph.AddTuple(tuple);
            }
            //foreach (Tuple tuple in tuples)
            //{
            //    Debug.Log(tuple.sub);
            //    GameObject temp = Instantiate(Resources.Load<GameObject>("Prefabs/" + tuple.sub));
            //    temp.AddComponent<MoveTowards>();
            //    temp.GetComponent<MoveTowards>().destination = Instantiate(Resources.Load<GameObject>("Prefabs/" + tuple.obj));
            //    objects.Add(temp);
                
            //}


            
            
            //foreach (char s in www.downloadHandler.text)
            //{
            //    Debug.Log(s);
            //}
            // Or retrieve results as binary data
            //byte[] results = www.downloadHandler.data;
            //Debug.Log(results);



        }
    }

    public void Save()
    {
        StartCoroutine(SaveTuplesUtility());
    }
    IEnumerator SaveTuplesUtility()
    {
        string request = server + "/" + saveTuplesCall + "/";

        Debug.Log(request);
        string data = JsonConvert.SerializeObject(new List<Tuple>(graph.GetTuples()));
        Debug.Log(JsonConvert.DeserializeObject<List<Tuple>>(data));
        //string data = JsonUtility.ToJson(graph.graph[0]);
        //Debug.Log(data.("\""));
        //string data = "asd\"asd";

        Debug.Log(data);
        string x = data.Replace("\"", "%22");
        //data.Replace("\"", "%22");
        Debug.Log(x);

        

        UnityWebRequest www = new UnityWebRequest(request, "POST");
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes("{ \"text\": \"" + x + "\" }"));
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            var json = www.downloadHandler.text;
            json = json.Trim('\"');
            json = json.Replace("\\\"", "\"");


            var tupleList = JsonConvert.DeserializeObject<List<Tuple>>(json);
            List<GameObject> objects = new List<GameObject>();

            foreach (Tuple tuple in tupleList)
            {
                Debug.Log(tuple.subGameObject);
                Debug.Log(tuple.objGameObject);
                Debug.Log(tuple.sub);
                Debug.Log(tuple.obj);
                Debug.Log(tuple.directionPrediction);
                Debug.Log(tuple.distancePrediction);
                graph.SetPredictions(tuple);
            }
            graph.ApplyPredictions();
        }

        
    }

    public int AddInstanceMapping(GameObject gameObject)
    {
        instanceMappings.Add(gameObject.GetInstanceID(), gameObject);
        return gameObject.GetInstanceID();
    }

    public GameObject GetGameObjectFromInstanceID(int instanceID)
    {
        return instanceMappings[instanceID];
    }
    public void Reeval()
    {

    }
    IEnumerator ReevalUtility()
    {
        string request = server + "/" + reevalCall + "/";

        Debug.Log(request);
        string data = JsonConvert.SerializeObject(new List<Tuple>(graph.GetTuples()));
        Debug.Log(JsonConvert.DeserializeObject<List<Tuple>>(data));
        //string data = JsonUtility.ToJson(graph.graph[0]);
        //Debug.Log(data.("\""));
        //string data = "asd\"asd";

        Debug.Log(data);
        string x = data.Replace("\"", "%22");
        //data.Replace("\"", "%22");
        Debug.Log(x);



        UnityWebRequest www = new UnityWebRequest(request, "POST");
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes("{ \"text\": \"" + x + "\" }"));
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            string text = www.downloadHandler.text.Replace("\\\"", "\"").Trim('\"');
            // Show results as text
            Debug.Log(text);
            
        }

    }

    public void ResetScene()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
}
