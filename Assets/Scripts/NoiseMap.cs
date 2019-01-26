using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMap : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public int seed;
    public float scale;
    public int octaves;
    public float persistence;
    public float lacunarity;
    public Vector2 offset;
    public float height;
    public Material grass;

    private void CreateMesh(float[,] heightmap)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                vertices.Add(new Vector3(x, heightmap[x, y] * height, y));
                uvs.Add(new Vector2(0.1f * (mapWidth - x), 0.1f * (mapHeight - y)));
            }
        }
        for (int x = 0; x < mapWidth - 1; x++)
        {
            for (int y = 0; y < mapHeight - 1; y++)
            {
                triangles.Add(y * mapWidth + x);
                triangles.Add(y * mapWidth + x + 1);
                triangles.Add(y * mapWidth + x + mapWidth);

                triangles.Add(y * mapWidth + x + mapWidth + 1);
                triangles.Add(y * mapWidth + x + mapWidth);
                triangles.Add(y * mapWidth + x + 1);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshCollider>().sharedMesh = gameObject.GetComponent<MeshFilter>().mesh;
        gameObject.GetComponent<MeshRenderer>().material = grass;
    }

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.AddComponent<MeshRenderer>();
        gameObject.AddComponent<MeshFilter>();
        //gameObject.AddComponent<MeshCollider>();

        Noise noise = new Noise();
        float[,] heightmap = noise.PerlinNoiseMap2D(mapWidth, mapHeight, seed, scale, octaves, persistence, lacunarity, offset);
        CreateMesh(heightmap);
    }
}
