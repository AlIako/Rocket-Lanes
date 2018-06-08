using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
	[SerializeField]
	GameObject projectile;

	[SerializeField]
	float timeBetweenSpawn = 1.0f;

	[SerializeField]
	bool alwaysSpawning = false;

	List<GameObject> spawners;
	float lastSpawn = 0.0f;

	void Start ()
	{
		FindSpawners();
		lastSpawn = Time.time;
	}
	
	void Update ()
	{
		AlwaysSpawn();
	}

	public GameObject GetFromPool()
	{
		int index = Random.Range(0, spawners.Count);
		return spawners[index];
	}

	void AlwaysSpawn()
	{
		if(alwaysSpawning)
		{
			if(Time.time - lastSpawn >= timeBetweenSpawn)
			{
				Spawn(GetRandomSpawnerIndex());
				lastSpawn = Time.time;
			}
		}
	}

	void FindSpawners()
	{
		spawners = new List<GameObject>();
		Transform parent = transform.parent;
		foreach (Transform child in parent)
		{
			if(child.gameObject.GetComponent<Spawner>() != null)
				spawners.Add(child.gameObject);
		}
	}

	public int GetRandomSpawnerIndex()
	{
		return Random.Range(0, spawners.Count);
	}

	public GameObject Spawn(int index)
	{
		GameObject spawner = spawners[index];
		return Instantiate(projectile, spawner.transform.position, spawner.transform.rotation);
	}
}
