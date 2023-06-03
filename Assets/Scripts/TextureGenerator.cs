using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{

    public static Texture2D TextureFromColorMap(Color[] colorMap, int mapWidth, int mapHeight)
    {
        Texture2D texture = new Texture2D(mapWidth, mapHeight);
        texture.filterMode = FilterMode.Point; //makes less blurry
        texture.wrapMode = TextureWrapMode.Clamp; //keeps map from wrapping around onto itself at the edges
        texture.SetPixels(colorMap);
        texture.Apply();
        return texture;
    }

    public static Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        Color[] colorMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }
        return TextureFromColorMap(colorMap, width, height);
    }

}
