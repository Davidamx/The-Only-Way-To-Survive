using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateArea : MonoBehaviour
{


	public float camTargetX;


	private void OnTriggerEnter(Collider other)
	{
		ThirdPersonInteraction player = other.GetComponent<ThirdPersonInteraction>();
		if (player != null)
		{
			CameraManager.Instance.targetX = camTargetX;
		}
	}
}
