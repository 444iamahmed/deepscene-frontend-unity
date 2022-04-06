using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Something : MonoBehaviour
{


    [SerializeField] string server;
    [SerializeField] string tuplesCall;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetText());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:9001/tag-speech/");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.data);

            // Or retrieve results as binary data
            //byte[] results = www.downloadHandler.data;

        }
    }
}
