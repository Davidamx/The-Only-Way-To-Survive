using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoSingleton<MenuCamera> {

    public Transform camTargetTransform;

    private bool camStartMove;
    private float t;

    private void Update()
    {
        if (camStartMove)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, camTargetTransform.position, t/2);
            transform.rotation = Quaternion.Slerp(transform.rotation, camTargetTransform.rotation, t/2);
        }
    }

    public void StartCamMove()
    {
        camStartMove = true;
    }

}
