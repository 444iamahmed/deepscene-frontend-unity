using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Test : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        //string x = "{\"array\": [{ \"sub\": \"house\", \"obj\": \"tree\", \"relation\": \"is behind\"}]}";
        string x = "[{ \"sub\": \"house\", \"obj\": \"tree\", \"relation\": \"is behind\"}, { \"sub\": \"panda\", \"obj\": \"sada\", \"relation\": \"is asdas\"}]";

        var y = JsonConvert.DeserializeObject<List<Tuple>>(x);
        Debug.Log(x);
        Debug.Log(y[0].sub);
        Debug.Log(y[0].obj);
        Debug.Log(y[0].relation);

        Debug.Log(y[1].sub);
        Debug.Log(y[1].obj);
        Debug.Log(y[1].relation);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
