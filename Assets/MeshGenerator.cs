
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{

    private Mesh mesh;
    private int[] triangles;
    private Vector3[] vertices;
    private Vector2[] uv;
    private Color[] colors;
    [SerializeField] private Gradient gradient;
    [SerializeField] private int xSize = 20;
    [SerializeField] private int zSize = 20;
    [SerializeField] private int minHeight = 0;
    [SerializeField] private int maxHeight = 100  ;
    [SerializeField] private float amplitude = .45f;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    private void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)]; //need 1 more vertex than shape #
        for (int z = 0, index = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * amplitude, z * amplitude) * 2f;
                if (y < minHeight)
                    y = minHeight;
                if(y > maxHeight)
                    y = maxHeight;
                vertices[index] = new Vector3(x, y, z);
                index++;
            }
        }

        triangles = new int[xSize * zSize * 6]; //2 triangles per square, 3 points per triangles -> 6, populating surface area of x * z squares

        for (int z = 0, vertexNum = 0, triangleNum = 0; z < zSize; z++) {
          for (int x = 0; x < xSize; x++, vertexNum++) {
            triangles[triangleNum++] = 0 + vertexNum;
            triangles[triangleNum++] = xSize + 1 + vertexNum;
            triangles[triangleNum++] = 1 + vertexNum;
            triangles[triangleNum++] = 1 + vertexNum;
            triangles[triangleNum++] = xSize + 1 + vertexNum;
            triangles[triangleNum++] = xSize + 2 + vertexNum;
            }
            vertexNum++; //prevents triangle being created linking new row diagonally to other end of old row
        }
        
        uv = new Vector2[vertices.Length];

        for(int x = 0, index = 0; x < xSize; x++){
            for(int z = 0; z < zSize; z++, index++) {
                uv[index] = new Vector2((float)x/xSize, (float)z/zSize); //uv values are on a scale from 0-1
            }
        }

        colors = new Color[vertices.Length];

        for (int x = 0, index = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++, index++)
            {
                float height = Mathf.InverseLerp(minHeight, maxHeight, vertices[index].y);
                colors[index] = gradient.Evaluate(height);
            }
        }

    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.colors = colors;
        mesh.RecalculateNormals();
    }

    //draws vertices
    void OnDrawGizmos()
    {
        if (vertices == null)
            return;
        for(int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);

        }
    }

}
