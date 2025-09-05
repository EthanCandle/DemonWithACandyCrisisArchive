using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomlyDOwn : MonoBehaviour
{
    public GameObject prefabToSpawn, boxGirlToSpawn;
    public int numberToSpawn = 10;
    public Vector3 spawnAreaSize = new Vector3(10f, 10f, 10f);

    public DebugStore debugStore;

    public List<GameObject> candySpawned;

    public bool shouldSpawnCandy;
    void Start()
    {
        SpawnAllCandy();
    }

    public void RemoveAllCandy()
    {
        // called by button deleting data
       // print("Remove all main candy ");
        for (int i = 0; i < candySpawned.Count; i++)
        {
            Destroy(candySpawned[i]);
        }
        candySpawned.Clear();
    }

    public void SetCandy()
    {
        print("setting candy 2");
        if (numberToSpawn > debugStore.debugStatsLocal.candyAmount)
        {
            print("setting candy 1");
            for (int i = candySpawned.Count - 1; i >= debugStore.debugStatsLocal.candyAmount; i--)
            {
                Destroy(candySpawned[i]);
                candySpawned.RemoveAt(i);
            }
        }
        else if(numberToSpawn < debugStore.debugStatsLocal.candyAmount)
        {
            print("setting candy 3");
            for (int i = numberToSpawn; i < debugStore.debugStatsLocal.candyAmount; i++)
            {
                Vector3 randomPosition = GetRandomPositionInCube();
                candySpawned.Add(Instantiate(prefabToSpawn, randomPosition, Quaternion.identity));
            }

        }
        numberToSpawn = debugStore.debugStatsLocal.candyAmount;
    }


    public void SpawnAllCandy()
    {

        StartCoroutine(DelayFrame());
    }

    public void ToggleCandy(bool state)
    {
        shouldSpawnCandy = state;

        if (shouldSpawnCandy)
        {
            SpawnAllCandy();
        }
        else
        {
            RemoveAllCandy();
        }
    }


    public IEnumerator DelayFrame()
    {
        yield return null;
        RemoveAllCandy();
        if (debugStore)
        {
            numberToSpawn = debugStore.debugStatsLocal.candyAmount;
        }

        for (int i = 0; i < numberToSpawn; i++)
        {
            //if(i >= 1000)
            //{
            //    break;
            //}
            Vector3 randomPosition = GetRandomPositionInCube();
            candySpawned.Add(Instantiate(prefabToSpawn, randomPosition, Quaternion.identity));
        }

        Vector3 randomPosition2 = GetRandomPositionInCube();
        candySpawned.Add(Instantiate(boxGirlToSpawn, randomPosition2, Quaternion.identity));
    }

    Vector3 GetRandomPositionInCube()
    {
        // Pick a point in *local space*
        Vector3 localPos = new Vector3(
            Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
            Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f),
            Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f)
        );

        // Convert local point to world space, so rotation & position are applied
        return transform.TransformPoint(localPos);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        // Save current matrix
        Matrix4x4 oldMatrix = Gizmos.matrix;

        // Apply this transform's position/rotation
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

        // Draw cube at origin of local space
        Gizmos.DrawWireCube(Vector3.zero, spawnAreaSize);

        // Restore
        Gizmos.matrix = oldMatrix;
    }
}
