using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    public int xSize = 20;
    public int zSize = 20;

    [Range(0,0.1f)]
    public float coarseScale1 = 0f;
    [Range(0, 0.1f)]
    public float coarseScale2 = 0f;

    [Range(0,10)]
    public float coarseRange = 2f;
    
    [Range(0,1f)]
    public float fineScale1 = 1f;
    [Range(0, 1f)]
    public float fineScale2 = 1f;

    [Range(0,2)]
    public float fineRange = 0.5f;

    public float xOffset;
    public float yOffset;

    public Gradient gradient;

    float minTerrainHeight;
    float maxTerrainHeight;

    public void GenerateMap() //function used by editor
    {
       
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }


    //private void Start()
    //{
    //    mesh = new Mesh();
    //    GetComponent<MeshFilter>().mesh = mesh;

    //    CreateShape();
    //    UpdateMesh();
    //}


    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];


        //creates a list of vertices for the specified length(xSize) and width(zSize)
        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {                
                float y = NoiseGenerator(x, z);

                if(y > maxTerrainHeight)
                {
                    maxTerrainHeight = y;
                }
                if (y < minTerrainHeight) {
                    minTerrainHeight = y;
                }

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
                //move this into own function
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

        colors = new Color[vertices.Length];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight,vertices[i].y); //normalizes height value  between 0 and 1 so color gradient works properly
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }

    }

    void UpdateMesh() //makes sure mesh is set up correctly and points arent saved from previous mesh
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals(); //fixes lighting of mesh based on surface normals
    }

    float NoiseGenerator(float x, float z)
    {
        //scales zoom the noise in and out/range increase max and min value of noise
        float coarseY1 = Mathf.PerlinNoise(x * coarseScale1 + xOffset, z * coarseScale1 + yOffset) * coarseRange;
        float coarseY2 = Mathf.PerlinNoise(x * coarseScale2 + xOffset, z * coarseScale2 + yOffset) * coarseRange;
        float fineY1 = Mathf.PerlinNoise(x * fineScale1, z * fineScale1) * fineRange;
        float fineY2 = Mathf.PerlinNoise(x * fineScale2, z * fineScale2) * fineRange;

        return coarseY1 + coarseY2 + fineY1 + fineY2;

    }
}
