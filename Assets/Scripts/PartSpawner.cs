using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Random;

public class PartSpawner : MonoBehaviour
{
    public Collider spawnArea;
    public GameObject spawnCandidate;

    public int frequency;
    public int variance;

    public float sizeVariance;

    private System.Random random = new System.Random(System.DateTime.Now.Millisecond);

    // Start is called before the first frame update
    void Start()
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

            GameObject candidate = Instantiate<GameObject>(spawnCandidate, position, Quaternion.Euler(-90, 0, 0));
            candidate.transform.localScale *= 1 + (float)(random.NextDouble() - 0.5) * sizeVariance;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
