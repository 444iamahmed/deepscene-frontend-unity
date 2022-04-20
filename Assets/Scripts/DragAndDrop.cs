//
//  UINavigation.cs
//
//  Created by Rigoberto Sáenz Imbacuán (https://linkedin.com/in/rsaenzi/)
//  Copyright © 2021. All rights reserved.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragAndDrop : MonoBehaviour {

    // The plane the object is currently being dragged on
    Plane dragPlane;
    RaycastHit hit;
    [SerializeField] Transform dragPlaneTarget;
    [SerializeField] static float hoverHeight = 5;
    [SerializeField] static float hoverTime = 0.1f;
    [SerializeField] static float dragSpeed = 5f;
    // The difference between where the mouse is on the drag plane and 
    // where the origin of the object is on the drag plane
    Vector3 offset;
    Vector3 lastMousePos;
    Vector3 hoverOffset;
    Vector3 hoverPosition;
    Vector3 velocity = Vector3.zero;

    float planeY;

    Camera myMainCamera;

    Transform targetTransform;

    bool mouseDown = false;
    bool inAir = false;
    bool goingDown = false;
    bool heightMode = false;
    Outline outline;
    void Start() {
        myMainCamera = Camera.main;

        if (transform.parent != null)
            targetTransform = transform.parent.transform;
        else
            targetTransform = transform;

        dragPlaneTarget = GameObject.FindGameObjectWithTag("Plane").transform;

        hoverOffset = new Vector3(0, hoverHeight, 0);
        hoverPosition = targetTransform.position;

        outline = targetTransform.GetComponent<Outline>();
        outline.enabled = false;

        //StartCoroutine(GoingUp());
    }

    void Update()
    {
        if(mouseDown)
            targetTransform.position = Vector3.SmoothDamp(targetTransform.position, hoverPosition, ref velocity, hoverTime);

        if(Input.GetKeyDown(KeyCode.LeftShift))
            heightMode = !heightMode;

    }

    private void OnMouseEnter()
    {
        outline.enabled = true;
        
    }

    void OnMouseDown() 
    {
        Ray camRay = myMainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!goingDown && !heightMode)
        {
            Debug.Log(targetTransform.position.y);
            mouseDown = true;
            //if (!goingDown)
            //{

            dragPlane = new Plane(dragPlaneTarget.transform.up, targetTransform.position);
            planeY = targetTransform.position.y;

            float planeDist;
            dragPlane.Raycast(camRay, out planeDist);
            offset = targetTransform.position - camRay.GetPoint(planeDist);
            //}
        }
        if(heightMode)
        {
            mouseDown = true;

            
            offset = targetTransform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

            lastMousePos = Input.mousePosition;


        }
    }

    void OnMouseDrag() {

        Ray camRay = myMainCamera.ScreenPointToRay(Input.mousePosition);

        if (!goingDown && mouseDown && !heightMode)
        //{ //dragDone = false;
        //if (inAir)
        {

            float planeDist;
            
            dragPlane.Raycast(camRay, out planeDist);
            //targetTransform.position = camRay.GetPoint(planeDist) + offset + hoverOffset;
            hoverPosition = camRay.GetPoint(planeDist) + offset + hoverOffset;
            //dragDone = true;
        }
        //}
        if(heightMode && mouseDown && !goingDown)
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            Vector3 pos = targetTransform.position;
            pos.y += delta.y * dragSpeed;
            //pos.y = Mathf.Clamp(pos.y, minY, maxY);
            hoverPosition = pos;
            lastMousePos = Input.mousePosition;
            //hoverPosition = new Vector3(targetTransform.position.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, targetTransform.position.z) + offset;
        }

    }

    private void OnMouseUp()
    {
        if (!goingDown && !heightMode)
        {
            mouseDown = false;
            StartCoroutine(Timer());
        }
    }
    IEnumerator Timer()
    {
        goingDown = true;
        Vector3 restPos = new Vector3(targetTransform.position.x, planeY, targetTransform.position.z);
        Vector3 _velocity = Vector3.zero;
        while(targetTransform.position != restPos)
        {
            targetTransform.position = Vector3.SmoothDamp(targetTransform.position, restPos, ref _velocity, hoverTime);
            yield return new WaitForFixedUpdate();
        }
        targetTransform.position = restPos;
        goingDown = false;
        
        //yield return new WaitUntil(()=>targetTransform.position.y == planeY);
    }

    
    private void OnMouseExit()
    {
        outline.enabled = false;
        
    }


}