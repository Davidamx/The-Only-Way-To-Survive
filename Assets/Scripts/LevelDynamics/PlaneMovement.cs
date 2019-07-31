using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMovement : MonoBehaviour {

    public Transform planeHead;

    public float smoothTime = 5.0f;

    public float flySpeed;

    public float rotateSpeed;

    private Vector3 currentDirection;

    private void Start()
    {
        currentDirection = transform.forward;
    }

    private void Update()
    {
        planeHead.Rotate(0.0f, 0.0f, rotateSpeed * Time.deltaTime, Space.Self);

        Quaternion fixedQuter = Quaternion.LookRotation(currentDirection, transform.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, fixedQuter, Time.deltaTime * smoothTime);
        transform.Translate(transform.forward * flySpeed * Time.deltaTime, Space.World);

        Vector3 temp = transform.forward;

        if (Vector3.Distance(transform.position, Vector3.zero) > 100.0f)
        {
            temp = Vector3.zero - transform.position;

        }

        if (transform.position.y < 30.0f || transform.position.y > 80.0f)
        {
            temp.y = -temp.y;
        }
        currentDirection = temp;
    }
}
