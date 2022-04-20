using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 500f;
    [SerializeField] float lineDistance = 10000f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
            transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.C))
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);

        //Debug.Log("SS");
        Debug.DrawLine(transform.position, transform.forward * lineDistance, Color.blue);
        Debug.DrawLine(transform.position, transform.right * lineDistance, Color.red);
        Debug.DrawLine(transform.position, -transform.right * lineDistance, Color.red);
    }
}
