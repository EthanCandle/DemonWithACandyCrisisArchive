using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomlyDOwn : MonoBehaviour
{
    public GameObject prefabToSpawn, boxGirlToSpawn;
    public int numberToSpawn = 10;
    public Vector3 spawnAreaSize = new Vector3(10f, 10f, 10f);

    void Awake()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            Vector3 randomPosition = GetRandomPositionInCube();
            Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);
        }
        Vector3 randomPosition2 = GetRandomPositionInCube();
        Instantiate(boxGirlToSpawn, randomPosition2, Quaternion.identity);
    }

    Vector3 GetRandomPositionInCube()
    {
        Vector3 center = transform.position;
        return new Vector3(
            Random.Range(center.x - spawnAreaSize.x / 2f, center.x + spawnAreaSize.x / 2f),
            Random.Range(center.y - spawnAreaSize.y / 2f, center.y + spawnAreaSize.y / 2f),
            Random.Range(center.z - spawnAreaSize.z / 2f, center.z + spawnAreaSize.z / 2f)
        );
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}
