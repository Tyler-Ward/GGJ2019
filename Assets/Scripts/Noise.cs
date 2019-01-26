using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise
{
    public float[,] PerlinNoiseMap2D(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for(int i=0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        //clamp scale if below threshold value
        if(scale <= 0.0001f)
        {
            scale = 0.0001f;
        }

        float minNoiseHeight = float.MaxValue;
        float maxNoiseHeight = float.MinValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for(int y=0; y < mapHeight; y++)
        {
            for(int x=0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                for(int i=0; i < octaves; i++)
                {
                    float sampleX = (x-halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y-halfHeight) / scale * frequency + octaveOffsets[i].y;

                    //float perlinValue = (Mathf.PerlinNoise(sampleX, sampleY) * 2) - 1;
                    //float perlinValue = ((float)implicitFractal.Get(sampleX, sampleY) * 2) - 1;
                    float perlinValue = Noise1234.pnoise2(sampleX, sampleY, (int)(mapWidth / scale), (int)(mapHeight / scale));
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if(noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if(noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        Debug.Log("minNoiseHeight: " + minNoiseHeight + " maxNoiseHeight: " + maxNoiseHeight);

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(-1, 1, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}
