using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class CameraInput : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float scrollWheelChange;

    public bool keyQ;
    public bool keyE;

    bool physicsDone;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      
        if (physicsDone)
            ResetInput();

        SetInput();
    }

    private void ResetInput()
    {
        horizontal = 0;
        vertical = 0;

        keyE = false;
        keyQ = false;
        physicsDone = false;
    }

    void SetInput()
    {
        horizontal = Mathf.Clamp(horizontal + Input.GetAxis("Horizontal"), -1f, 1f);
        vertical = Mathf.Clamp(vertical + Input.GetAxis("Vertical"), -1f, 1f);

        scrollWheelChange = Input.GetAxis("Mouse ScrollWheel");


        keyQ = keyQ || Input.GetKey(KeyCode.Q);
        keyE = keyE || Input.GetKey(KeyCode.E);
        //mouseHorizontalAxis = Mathf.Clamp(mouseHorizontalAxis + Input.GetAxis("Mouse X"), -1f, 1f);
        //mousevertical = Mathf.Clamp(mousevertical + Input.GetAxis("Mouse Y"), -1f, 1f);
    }
    private void FixedUpdate()
    {
        physicsDone = true;
    }
}
