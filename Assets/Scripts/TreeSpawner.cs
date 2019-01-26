﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Random;

public class TreeSpawner : MonoBehaviour
{
    public Collider spawnArea;
    public Object spawnCandidate;

    public int frequency;
    public int variance;

    private System.Random random = new System.Random();

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

            Instantiate(spawnCandidate, position, Quaternion.Euler(-90, 0, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}