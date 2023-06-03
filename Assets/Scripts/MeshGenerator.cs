
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class MeshGenerator{

    //[SerializeField] private Gradient gradient;
 


    // Start is called before the first frame update
       // GetComponent<MeshFilter>().mesh = mesh;


    public static MeshData GenerateTerrainMesh(float[,] heightMap){
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        MeshData meshData = new MeshData(new Vector2Int(width, height));
        meshData.PopulateData(heightMap);
        return meshData;
    }

 

    //draws vertices
  /*  void OnDrawGizmos()
    {
        if (vertices == null)
            return;
        for(int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);

        }
    }*/

}

public class MeshData {
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uv;
    private Vector2Int meshSize;

    private float halfHeight;
    private float halfWidth;

    public MeshData(Vector2Int meshSize) {
        this.meshSize = meshSize;
        vertices = new Vector3[(meshSize.x + 1) * (meshSize.y + 1)]; //need 1 more vertex than shape #
        uv = new Vector2[vertices.Length]; //1 uv per vertex
        triangles = new int[meshSize.x * meshSize.y * 6]; //2 triangles per square, 3 points per triangles -> 6, populating surface area of x * z squares
        halfHeight = (meshSize.y - 1) / 2f;
        halfWidth = (meshSize.x - 1) / 2f;
    }

    private void GenerateTriangles() {
        for (int y = 0, vertexNum = 0, triangleNum = 0; y < meshSize.y; y++) {
            for (int x = 0; x < meshSize.x; x++, vertexNum++) {
                triangles[triangleNum++] = 0 + vertexNum;
                triangles[triangleNum++] = meshSize.x + 1 + vertexNum;
                triangles[triangleNum++] = 1 + vertexNum;
                triangles[triangleNum++] = 1 + vertexNum;
                triangles[triangleNum++] = meshSize.x + 1 + vertexNum;
                triangles[triangleNum++] = meshSize.x + 2 + vertexNum;
            }
            vertexNum++; //prevents triangle being created linking new row diagonally to other end of old row
        }
    }

                //if (yMesh > maxHeight)
                  //  maxHeight = (int)yMesh;
              //  if (yMesh < minHeight)
                //    minHeight = (int)yMesh;
 

    public void GenerateVertices(float[,] heightMap) {
        int vertexIndex = 0;
        for (int y = 0; y < meshSize.y; y++) {
            for (int x = 0; x < meshSize.x; x++, vertexIndex++) {
                vertices[vertexIndex] = new Vector3(x - halfWidth, heightMap[x, y], y - halfHeight);

            }
        }
    }

    public void GenerateUVs() {
        int vertexIndex = 0;
        for(int y = 0; y < meshSize.y; y++) {
            for(int x = 0; x < meshSize.x; x++, vertexIndex++) {
                uv[vertexIndex] = new Vector2((float) x / meshSize.x, (float) y / meshSize.y); //uv values are on a scale from 0-1
            }
        }
    }

    public void PopulateData(float[,] heightMap) {
        GenerateVertices(heightMap);
        GenerateTriangles();
        GenerateUVs();
    }

    public Mesh GenerateMesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        return mesh;
    }

}
