using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletArea : MonoBehaviour
{

	public GameObject bulletprefab;
	public GameObject bulletFxPrefab;
	public float maxShootInterval = 0.2f;
	public float maxFlyTime = 0.5f;

	public GameObject areaOne;
	public GameObject areaTwo;

	private List<GameObject> bulletsPool;
	private Vector3 areaOnePosOne;
	private Vector3 areaOnePosTwo;
	private Vector3 areaTwoPosOne;
	private Vector3 areaTwoPosTwo;
	private float timePast;
	
	
	// Use this for initialization
	void Start ()
	{
		areaOnePosOne = areaOne.transform.TransformPoint(areaOne.GetComponent<MeshFilter>().mesh.vertices[0]);
		areaOnePosTwo = areaOne.transform.TransformPoint(areaOne.GetComponent<MeshFilter>().mesh.vertices[1]);
		areaTwoPosOne = areaTwo.transform.TransformPoint(areaTwo.GetComponent<MeshFilter>().mesh.vertices[0]);
		areaTwoPosTwo = areaTwo.transform.TransformPoint(areaTwo.GetComponent<MeshFilter>().mesh.vertices[1]);
		bulletsPool = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		timePast += Time.deltaTime;

		if (timePast > maxShootInterval)
		{
			Vector3 spawnPos = new Vector3(Random.Range(Mathf.Min(areaOnePosOne.x, areaOnePosTwo.x), Mathf.Max(areaOnePosOne.x, areaOnePosTwo.x)),
				Random.Range(Mathf.Min(areaOnePosOne.y, areaOnePosTwo.y), Mathf.Max(areaOnePosOne.y, areaOnePosTwo.y)),
				Random.Range(Mathf.Min(areaOnePosOne.z, areaOnePosTwo.z), Mathf.Max(areaOnePosOne.z, areaOnePosTwo.z)));
			Vector3 targetPos = new Vector3(Random.Range(Mathf.Min(areaTwoPosOne.x, areaTwoPosTwo.x), Mathf.Max(areaTwoPosOne.x, areaTwoPosTwo.x)),
				Random.Range(Mathf.Min(areaTwoPosOne.y, areaTwoPosTwo.y), Mathf.Max(areaTwoPosOne.y, areaTwoPosTwo.y)),
				Random.Range(Mathf.Min(areaTwoPosOne.z, areaTwoPosTwo.z), Mathf.Max(areaTwoPosOne.z, areaTwoPosTwo.z)));
			GameObject bulletGO;
			if (bulletsPool.Count > 0)
			{
				bulletGO = bulletsPool[0];
				bulletsPool.RemoveAt(0);
				bulletGO.SetActive(true);
				bulletGO.transform.position = spawnPos;
			}
			else
			{
				bulletGO = Instantiate(bulletprefab, spawnPos, Quaternion.identity);
			}

			StartBulletMove(bulletGO, targetPos);


			timePast = 0;
		}
	}

	private void StartBulletMove(GameObject bullet, Vector3 targetPos)
	{
		bullet.transform.DOMove(targetPos, maxFlyTime).SetEase(Ease.Linear).onComplete = delegate
		{
			bullet.SetActive(false);
			bulletsPool.Add(bullet);
		};
	}
	
}
