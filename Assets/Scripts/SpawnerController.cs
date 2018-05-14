using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
	[SerializeField]
	GameObject projectile;

	[SerializeField]
	List<Spawner> spawners;

	[SerializeField]
	float timeBetweenSpawn = 1.0f;

	float lastSpawn = 0.0f;

	void Start ()
	{
		lastSpawn = Time.time;
	}
	
	void Update ()
	{
		if(Time.time - lastSpawn >= timeBetweenSpawn)
		{
			Spawn();
			lastSpawn = Time.time;
		}
	}

	void Spawn()
	{
		int index = Random.Range(0, spawners.Count);
		Spawner spawner = spawners[index];
		Instantiate(projectile, spawner.transform.position, spawner.transform.rotation);
	}
}
