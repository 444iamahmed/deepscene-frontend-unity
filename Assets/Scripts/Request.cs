using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MyBehaviour : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        //UnityWebRequest www = UnityWebRequest.Get("172.16.53.212:9001/");
        //yield return www.SendWebRequest();

        //if (www.result != UnityWebRequest.Result.Success)
        //{
        //    Debug.Log(www.error);
        //}
        //else
        //{
        //    // Show results as text
        //    Debug.Log(www.downloadHandler.text);

        //    // Or retrieve results as binary data
        //    byte[] results = www.downloadHandler.data;
        //}

        yield return null;
        Debug.Log("ASD");
    }
}