﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Random;

public class TreeSpawner : MonoBehaviour
{
    public Collider spawnArea;
    public GameObject spawnCandidate;

    public int frequency;
    public int variance;

    public float sizeVariance;

    private System.Random random = new System.Random();
    public List<GameObject> objects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        int numTrees = random.Next(frequency - variance, frequency + variance);
        MeshCollider areaCollision = spawnArea.GetComponent<MeshCollider>();

        Vector3 min = areaCollision.bounds.min, max = areaCollision.bounds.max;
   
        for(int i = 0; i < numTrees; i++)
        {
            Vector3 position = new Vector3(
                min.x + (float)random.NextDouble() * (max.x - min.x),
                max.y,
                min.z + (float)random.NextDouble() * (max.z - min.z)
            );

            GameObject candidate = Instantiate<GameObject>(spawnCandidate, position, Quaternion.Euler(-90, 0, 0));
            candidate.transform.localScale *= 1 + (float)(random.NextDouble() - 0.5) * sizeVariance;
            objects.Add(candidate);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
