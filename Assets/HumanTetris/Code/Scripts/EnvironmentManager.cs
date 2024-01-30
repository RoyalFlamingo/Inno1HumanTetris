using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
	[SerializeField] public List<GameObject> icePrefabs = new List<GameObject>();

	private List<GameObject> iceSpawner = new List<GameObject>();

	void Start()
	{
		FindIceSpawner();
		StartCoroutine(RandomIceSpawnCoroutine());
		InitializeMovingObstacles();

	}

	private void FindIceSpawner()
	{
		var objects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
		foreach (var obj in objects)
		{
			if (obj.tag == "EnvIceSpawner")
			{
				iceSpawner.Add(obj);
			}
		}
	}

	IEnumerator RandomIceSpawnCoroutine()
	{
		while (true)
		{
			SpawnIce();

			float waitTime = Random.Range(1.0f, 8.0f);
			yield return new WaitForSeconds(waitTime);
		}
	}

	private void InitializeMovingObstacles()
	{
		int initialCount = 11;
		float spacing = 75.0f;

		for (int i = 0; i < initialCount; i++)
		{
			SpawnIce(-i * spacing);
		}
	}

	private void SpawnIce()
	{
		var spawner = iceSpawner[Random.Range(0, iceSpawner.Count)];

		var ice = Instantiate(icePrefabs[Random.Range(0, icePrefabs.Count)]);
		ice.transform.position = spawner.transform.position;
		ice.transform.rotation = Quaternion.Euler(ice.transform.rotation.eulerAngles.x, Random.Range(0f, 360f), ice.transform.rotation.eulerAngles.z);
		ice.AddComponent<WorldspaceMover>().speed = 6f;
	}

	private void SpawnIce(float offset)
	{
		var spawner = iceSpawner[Random.Range(0, iceSpawner.Count)];

		var ice = Instantiate(icePrefabs[Random.Range(0, icePrefabs.Count)]);
		ice.transform.position = spawner.transform.position + new Vector3(offset, 0,0);
		ice.transform.rotation = Quaternion.Euler(ice.transform.rotation.eulerAngles.x, Random.Range(0f,360f), ice.transform.rotation.eulerAngles.z);
		ice.AddComponent<WorldspaceMover>().speed = 6f;
	}


}
