using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;


    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    private void Update()
    {
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];


        //creates a list of vertices for the specified length(xSize) and width(zSize)
        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x *0.3f, z * 0.3f) * 2f; //multiplying by 0.3 zooms out of noise and makes it smoother. Multiplying it by 2 increases the range of height values
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6]; //generates array of of correct size based on length and widht

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++) //when a row is complete the new row is started
        {
            for (int x = 0; x < xSize; x++) //loops over row and makes 2 sets of triangles(1 quad) out of vertices
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++; //shifts points over for next triangle
                tris += 6; //when a quad is done, incremented to work on next quad
            }

            vert++; //need to increment when a row is done or else a triangle is created between end of row and start of new row
        }
  
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    public void GenerateMap()
    {

    }
}
