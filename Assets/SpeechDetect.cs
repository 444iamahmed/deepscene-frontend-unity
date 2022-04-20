using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWebGLSpeechDetection;


public class SpeechDetect : MonoBehaviour
{

    private ISpeechDetectionPlugin _mSpeechDetectionPlugin = null;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(_Start());
    }

    IEnumerator _Start()
    {
        _mSpeechDetectionPlugin = WebGLSpeechDetectionPlugin.GetInstance();
        // check the reference to the plugin
        if (null == _mSpeechDetectionPlugin)
        {
            Debug.LogError("WebGL Speech Detection Plugin is not set!");
            yield break;
        }
        // wait for plugin to become available
        while (!_mSpeechDetectionPlugin.IsAvailable())
        {
            yield return null;
        }

        _mSpeechDetectionPlugin.AddListenerOnDetectionResult(HandleDetectionResult);


        // get the singleton instance
        // check the reference to the plugin
        if (null != _mSpeechDetectionPlugin)
        {
            // launch the proxy
            _mSpeechDetectionPlugin.ManagementLaunchProxy();
        }

        int port = 5000;
        _mSpeechDetectionPlugin.ManagementSetProxyPort(port);


        _mSpeechDetectionPlugin.ManagementOpenBrowserTab();

        Debug.Log("S");


    }
    bool HandleDetectionResult(DetectionResult detectionResult)
    {
        return false; //not handled
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
