using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//public static class JsonHelper
//{
//    public static T[] FromJson<T>(string json)
//    {
//        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
//        return wrapper.Items;
//    }

//    public static string ToJson<T>(T[] array)
//    {
//        Wrapper<T> wrapper = new Wrapper<T>();
//        wrapper.Items = array;
//        return JsonUtility.ToJson(wrapper);
//    }

//    public static string ToJson<T>(T[] array, bool prettyPrint)
//    {
//        Wrapper<T> wrapper = new Wrapper<T>();
//        wrapper.Items = array;
//        return JsonUtility.ToJson(wrapper, prettyPrint);
//    }

//    [Serializable]
//    private class Wrapper<T>
//    {
//        public T[] Items;
//    }
//}

public class JsonHelper
{
    public static T[] getJsonArray<T>(string json)
    {
        string newJson = json;
        if (!json.Contains("array"))
            newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    public static string makeJsonFromArray<T>(T[] data)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.array = data;
        return JsonUtility.ToJson(wrapper);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}