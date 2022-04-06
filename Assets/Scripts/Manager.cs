using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class Manager : MonoBehaviour
{

    public string inputText;

    public string server = "http://localhost:9001";

    [SerializeField] string tagSpeechCall;
    [SerializeField] string tuplesCall;
    [SerializeField] string saveTuplesCall;

    SceneGraph graph;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetTuples());
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

    IEnumerator GetTags()
    {

        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();


        //formData.Add(new MultipartFormDataSection("text", inputText));
        string request = server + "/" + tagSpeechCall + "/";

        Debug.Log(request);
        UnityWebRequest www = new UnityWebRequest(request, "POST");
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes("{ \"text\": \"" + inputText + "\" }"));
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
        inputText.Replace(" ", "%20");
        inputText.Replace(".", "%2E");
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

            //List<GameObject> objects = new List<GameObject>();

            foreach (Tuple tuple in JsonHelper.getJsonArray<Tuple>(www.downloadHandler.text))
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
        string data = JsonHelper.makeJsonFromArray<Tuple>(graph.graph.ToArray());
        //string data = JsonUtility.ToJson(graph.graph[0]);
        //Debug.Log(data.("\""));
        //string data = "asd\"asd";


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
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
}
