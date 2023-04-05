using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject prefabToSpawn;
    [SerializeField] int amountToSpawn;
    [SerializeField] float timeBetweenSpawns;

    void Awake()
    {
        StartCoroutine(SpawnObjects(prefabToSpawn, amountToSpawn, timeBetweenSpawns));
    }

    IEnumerator SpawnObjects(GameObject prefab, int amount, float gap)
    {
        float startTime = Time.time;
        int amountSpawned = 0;

        while (amountSpawned < amountToSpawn)
        {
            GameObject spawned = Instantiate(prefab, transform.position, Quaternion.identity);
            spawned.name = spawned.name + amountSpawned;
            amountSpawned++;
            yield return new WaitForSeconds(gap);
        }
        float spawningTime = Time.time - startTime;
    }
}
