using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int numberOctaves, float persistance, float lacunarity)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];
        Vector2[] octaveOffsets = GetOctaveOffsets(numberOctaves, seed);
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;
        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;  //Higher frequency -> Further apart sample points -> Height values change more rapidly
                float noiseHeight = 0;
                for(int i = 0; i < numberOctaves; i++)
                {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x * frequency; //halfWidth makes Scale zoom in/out of center rather than upper right corner
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; //in the range of [-1, 1] to inc. or dec. noiseHeight instead of always increase if were [0,1]
                    noiseHeight += perlinValue * amplitude;

                    if(maxNoiseHeight < noiseHeight)
                        maxNoiseHeight = noiseHeight;
                    if (minNoiseHeight > noiseHeight)
                        minNoiseHeight = noiseHeight;

                    amplitude *= persistance; //decreases every octave
                    frequency *= lacunarity; //increases each octave
                }
                noiseMap[x, y] = noiseHeight;
            }
        }

        for(int x  = 0; x < mapWidth; x++)
        {
            for(int y = 0;y < mapHeight; y++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]); //loops through noiseMap and returns a value between 0 and 1 <- Normalized
            }
        }

        return noiseMap;
    }

    private static Vector2[] GetOctaveOffsets(int numberOctaves, int seed)
    {
        Vector2[] octaveOffsets = new Vector2[numberOctaves];
        System.Random prng = new System.Random(seed); //psuedo-random number gen
        for(int i = 0; i < numberOctaves; i++)
        {
            float xOffset = prng.Next(-100000, 100000);
            float yOffset = prng.Next(-100000, 100000);
            octaveOffsets[i] = new Vector2(xOffset, yOffset);
        }
        return octaveOffsets;
    }

} 
