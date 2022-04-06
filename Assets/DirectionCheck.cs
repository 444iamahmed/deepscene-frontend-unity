using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionCheck : MonoBehaviour
{
    [SerializeField] Transform source;
    [SerializeField] Transform dest;

    Vector3 secondaryDiagonalStart;
    Vector3 secondaryDiagonalEnd;
    Vector3 primaryDiagonalStart;
    Vector3 primaryDiagonalEnd;

    // Start is called before the first frame update
    void Start()
    {
        primaryDiagonalStart = Quaternion.AngleAxis(45, Vector3.up) * Camera.main.transform.right;
        primaryDiagonalEnd = Quaternion.AngleAxis(-45, Vector3.up) * Camera.main.transform.right;

        secondaryDiagonalStart = Quaternion.AngleAxis(-45, Vector3.up) * Camera.main.transform.right;
        secondaryDiagonalEnd = Quaternion.AngleAxis(45, Vector3.up) * Camera.main.transform.right;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            Vector3 direction = source.position - dest.position;

            Debug.Log(((primaryDiagonalStart.z * primaryDiagonalEnd.x - primaryDiagonalStart.x * primaryDiagonalEnd.z) * (primaryDiagonalStart.z * direction.x - primaryDiagonalStart.x * direction.z)).ToString() + ", " + ((secondaryDiagonalStart.z * secondaryDiagonalEnd.x - secondaryDiagonalStart.x * secondaryDiagonalEnd.z) * (secondaryDiagonalStart.z * direction.x - secondaryDiagonalStart.x * direction.z)).ToString() + " ---- " +
                direction.magnitude);
            
                //Debug.Log("Right");
        }
    }
}
