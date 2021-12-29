using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{

    //do something like this to generate seed
    static void asdf()
    {
        System.Random prng = new System.Random(1);
    }

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    public int xSize = 100;
    public int zSize = 100;


    [Range(0,10)]
    public float amplitude = 1f;

    [Range(10,100)]
    public float noiseScale = 10f;
    float frequency;

    [Range(1,8)]
    public int octaves;

    [Range(0,1)]
    public float persistance = 0.5f;

    [Range(0,5)]
    public float lacunarity = 2;

    public float xOffset;
    public float zOffset;

    public Gradient gradient;

    float minTerrainHeight;
    float maxTerrainHeight;

    public void GenerateMap() //function used by editor
    {
        frequency = 1/noiseScale;
        minTerrainHeight = 0;
        maxTerrainHeight = 0;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh(mesh, vertices, triangles, colors);
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
        
            for (int i = 0, z = 0; z <= zSize; z++)
            {
                for (int x = 0; x <= xSize; x++)
                {
                frequency = 1/noiseScale;
                    
                    for (int j = 0; j <= octaves; j++)
                    {
                        float y = NoiseGenerator(x, z);

                        if (vertices[i] == Vector3.zero)
                        {
                            vertices[i] = new Vector3(x, y, z);
                        }
                        else
                        {
                            vertices[i] += new Vector3(0, y, 0);
                        }
                    
                    }
                i++;
                }
            }

        maxTerrainHeight = minTerrainHeight = vertices[0].y; //setting min/max height values to arbitrary point in set of vertices to ensure that all heights will be compared from a known point in the set

        for (int i = 0; i < vertices.Length; i++)
        {
            
            float y = vertices[i].y;
            if (y > maxTerrainHeight)
            {
                maxTerrainHeight = y;
            }
            if (y < minTerrainHeight)
            {
                minTerrainHeight = y;
            }
        }

        TrianglesAndVertices();

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

    static void UpdateMesh(Mesh mesh, Vector3[] vertices, int[] triangles, Color[] colors) //makes sure mesh is set up correctly and points arent saved from previous mesh
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals(); //fixes lighting of mesh based on surface normals
    }

    float NoiseGenerator(float x, float z)
    {
        //Frequencys zoom the noise in and out/Amplitude increase max and min value of noise
        float perlinValue = Mathf.PerlinNoise((float)x * frequency + xOffset, (float)z * frequency + zOffset);
        float noiseHeight = perlinValue * amplitude;
        noiseHeight *= persistance;
        frequency *= lacunarity;

        return noiseHeight;
    }

    void TrianglesAndVertices()
    {
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
    }
}
