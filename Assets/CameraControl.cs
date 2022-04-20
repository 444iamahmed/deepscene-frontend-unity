using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float zoomSpeed = 3f;
    [SerializeField] float rotateSpeed = 100f;

    [SerializeField] Transform target;
    Transform dummy;
    Quaternion targetRotation;
    CameraInput cameraInput;
    Vector3 deltaPosition;

    // Start is called before the first frame update
    void Start()
    {
        cameraInput = GetComponent<CameraInput>();
        //target = GetComponentInChildren<Transform>();
        targetRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        deltaPosition = (Vector3.ProjectOnPlane(transform.forward, Vector3.up) * cameraInput.vertical + Vector3.ProjectOnPlane(transform.right, Vector3.up) * cameraInput.horizontal) * moveSpeed * Time.deltaTime;

        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed);

        transform.position += deltaPosition;
        //target.position += deltaPosition;

        if (cameraInput.scrollWheelChange != 0)
        {                                            //If the scrollwheel has changed
            //float R = cameraInput.scrollWheelChange * 15;                                   //The radius from current camera
            //float PosX = Camera.main.transform.eulerAngles.x + 90;              //Get up and down
            //float PosY = -1 * (Camera.main.transform.eulerAngles.y - 90);       //Get left to right
            //PosX = PosX / 180 * Mathf.PI;                                       //Convert from degrees to radians
            //PosY = PosY / 180 * Mathf.PI;                                       //^
            //float X = R * Mathf.Sin(PosX) * Mathf.Cos(PosY);                    //Calculate new coords
            //float Z = R * Mathf.Sin(PosX) * Mathf.Sin(PosY);                    //^
            //float Y = R * Mathf.Cos(PosX);                                      //^
            //float CamX = Camera.main.transform.position.x;                      //Get current camera postition for the offset
            //float CamY = Camera.main.transform.position.y;                      //^
            //float CamZ = Camera.main.transform.position.z;                      //^
            transform.position += transform.forward * cameraInput.scrollWheelChange * zoomSpeed * Time.deltaTime;//Move the main camera
        }

        if (cameraInput.keyE)
            transform.RotateAround(target.position, Vector3.up, -rotateSpeed * Time.deltaTime);
        else if (cameraInput.keyQ)
            transform.RotateAround(target.position, Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
