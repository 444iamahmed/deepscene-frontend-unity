using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
        string x = JsonUtility.ToJson(gameObject);
        Debug.Log(x);
        Debug.Log(JsonUtility.FromJson<GameObject>(x));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
