using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Random;

public class PartSpawner : MonoBehaviour
{
    [Serializable]
    public struct WeightedSpawn
    {
        public GameObject prefab;
        public float weight;
    }
    public List<WeightedSpawn> spawnCandidates;

    public Collider spawnArea;

    public int frequency;
    public int variance;

    public float sizeVariance;

    private System.Random random = new System.Random(System.DateTime.Now.Millisecond);
    private List<GameObject> objects = new List<GameObject>();

    private float weightTotal;

    public void DestroyObjects()
    {
        for(int i = 0; i < objects.Count; i++)
        {
            Destroy(objects[i]);
        }
        objects.Clear();
    }

    public void SpawnObjects()
    {
        random = new System.Random(System.DateTime.Now.Millisecond);
        int numTrees = random.Next(frequency - variance, frequency + variance);
        MeshCollider areaCollision = spawnArea.GetComponent<MeshCollider>();

        Vector3 min = areaCollision.bounds.min, max = areaCollision.bounds.max;

        for (int i = 0; i < numTrees; i++)
        {
            Vector3 position = new Vector3(
                min.x + (float)random.NextDouble() * (max.x - min.x),
                max.y,
                min.z + (float)random.NextDouble() * (max.z - min.z)
            );

            Ray ray = new Ray(position, new Vector3(0, -1, 0));
            RaycastHit[] casts = Physics.RaycastAll(ray);
            position = casts[0].point;

            if (position.y > 0)
            {
                GameObject clone = Instantiate<GameObject>(getRandomCandidate(), position, Quaternion.identity/*Quaternion.Euler(-90, 0, 0)*/);
                clone.transform.localScale *= 1 + (float)(random.NextDouble() - 0.5) * sizeVariance;

                objects.Add(clone);
            }
        }
    }

    private GameObject getRandomCandidate()
    {
        float factor = (float)random.NextDouble() * weightTotal;
        float weightAccum = 0;

        foreach(WeightedSpawn candidate in spawnCandidates)
        {
            weightAccum += candidate.weight;
            if(factor < weightAccum)
            {
                return candidate.prefab;
            }
        }
        return null;
    }

    private void Start()
    {
        weightTotal = 0;
        foreach (WeightedSpawn candidate in spawnCandidates)
        {
            weightTotal += candidate.weight;
        }

        SpawnObjects();
    }
}
