using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    Ray ray;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        {
            CamreaRay();
            RaycastPlay();
        }
    }

    /// <summary>
    /// 物体发射射线
    /// </summary>
    void RaycastPlay()
    {
        Vector3 down = transform.TransformDirection(Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, down, out hit, 2))
        {
            Debug.Log("Raycast");
            Debug.DrawLine(transform.position, hit.point, Color.green);
            Debug.Log(hit.collider.name);
        }
        Debug.Log("RaycastEnd");
    }

    /// <summary>
    /// 摄像机发射射线
    /// </summary>
    void CamreaRay()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2000))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
    }
}
