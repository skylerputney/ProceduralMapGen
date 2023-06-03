using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode {NoiseMap, ColorMap, Mesh};
    [SerializeField] DrawMode drawMode;
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;
    [SerializeField] private int seed;
    [SerializeField] private float noiseScale;
    [SerializeField] private int octaves;
    [SerializeField, Range(0, 1)] private float persistance;
    [SerializeField] private float lacunarity;
    [SerializeField] private bool autoUpdateMap;
    [SerializeField] private TerrainType[] terrainTypes;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity);
        Color[] colorMap = GetColorMap(noiseMap);

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        else if (drawMode == DrawMode.ColorMap)
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        else if (drawMode == DrawMode.Mesh)
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap), TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
    }

    public bool AutoUpdateMap()
    {
        return autoUpdateMap;
    }

    void OnValidate()
    {
        if (mapWidth < 1)
            mapWidth = 1;
        if(mapHeight < 1)
            mapHeight = 1;
        if(lacunarity < 1)
            lacunarity = 1;
        if (octaves < 0)
            octaves = 0;

    }

    private Color[] GetColorMap(float[,] noiseMap)
    {
        Color[] colorMap = new Color[mapWidth * mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < terrainTypes.Length; i++)
                {
                    if (currentHeight <= terrainTypes[i].height)
                    {
                        colorMap[y * mapWidth + x] = terrainTypes[i].color;
                        break;
                    }
                }
            }
        }
        return colorMap;
    }

}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}