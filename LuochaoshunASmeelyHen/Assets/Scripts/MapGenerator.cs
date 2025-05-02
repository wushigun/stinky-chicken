using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [HideInInspector]
    public float[,] noiseMap;
    public int width, height, octaves, seed;
    public float scale, lacunarity, persistance, offsetY;
	
    public void generateMap()
    {
        noiseMap = makeNoiseMap(width, height, seed, scale, octaves, lacunarity, persistance);
	}

    public void generateMap(int m_seed)
    {
        noiseMap = makeNoiseMap(width, height, m_seed, scale, octaves, lacunarity, persistance);
	}

    public static float[,] makeNoiseMap(int width, int height, int seed, float scale, int octaves, float lacunarity, float persistance)
    {
        float[,] noiseMap = new float[width, height];

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = width / 2;
        float halfHeight = height / 2;

        System.Random random = new System.Random(seed);
        Vector2[] offsetOctaves = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            offsetOctaves[i] = new Vector2(random.Next(-10000, 10000), random.Next(-10000, 10000));
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int k = 0; k < octaves; k++)
                {
                    float simpleX = (i - halfWidth) / scale * frequency + offsetOctaves[k].x;
                    float simpleY = (j - halfHeight) / scale * frequency + offsetOctaves[k].y;
                    float perlinNoise = Mathf.PerlinNoise(simpleX, simpleY) * 2 - 1;

                    noiseHeight += perlinNoise * amplitude;
                    frequency *= lacunarity;
                    amplitude *= persistance;
                }
                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;
                noiseMap[i, j] = noiseHeight;
            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                noiseMap[i, j] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[i, j]);
            }
        }

        return noiseMap;
    }

	//当值改变时
    private void OnValidate()
    {
        if (width <= 1)
        {
            width = 1;
        }
        if (height <= 1)
        {
            height = 1;
        }
        if (octaves <= 1)
        {
            octaves = 1;
        }
    }
}
