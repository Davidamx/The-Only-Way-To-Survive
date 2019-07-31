using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TerminalArea : MonoBehaviour
{

	[System.Serializable]
	public struct ArrowData
	{
		public GameObject arrowObj;
		public float duration;
		public Vector3 moveOffset;
		public float rotateSpeed;

		public void StartMove()
		{
			arrowObj.transform.DOLocalMove(moveOffset, duration).SetLoops(-1, LoopType.Yoyo);
			arrowObj.transform.DOLocalRotate(new Vector3(0, 0, 180), rotateSpeed).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
		}
	};

	public ArrowData _arrowData;
	
	
	void Start () {
		_arrowData.StartMove();
	}


	private void OnTriggerEnter(Collider other)
	{
		ThirdPersonInteraction player = other.GetComponent<ThirdPersonInteraction>();
		if (player != null)
		{
			UIManager.Instance.Push<UIScreenResult>(UIDepthConst.MiddleDepth, true, true);
            LWGameManager.Instance.GameOver(null);
            LevelManager.Instance.SwitchGameState(GameState.End);
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
	}
}
