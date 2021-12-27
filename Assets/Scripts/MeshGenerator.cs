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

    //[Range(0,0.1f)]
    //public float coarseFrequency1 = 0f;
    //[Range(0, 0.1f)]
    //public float coarseFrequency2 = 0f;

    //[Range(0,10)]
    //public float coarseAmplitude = 2f;
    
    //[Range(0,1f)]
    //public float fineFrequency1 = 1f;
    //[Range(0, 1f)]
    //public float fineFrequency2 = 1f;

    //[Range(0,2)]
    //public float fineAmplitude = 0.5f;

    [Range(0,10)]
    public float amplitude = 1f;

    public float startingFrequency = 0.1f;
    float frequency;

    public int octaves;
    public float persistance;
    [Range(0,1)]
    public float lacunarity;

    public float xOffset;
    public float zOffset;

    public Gradient gradient;

    float minTerrainHeight;
    float maxTerrainHeight;

    public void GenerateMap() //function used by editor
    {
        frequency = startingFrequency;
        minTerrainHeight = 0;
        maxTerrainHeight = 0;
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
        for (int j = 0; j < octaves; j++)
        {
            for (int i = 0, z = 0; z <= zSize; z++)
            {
                for (int x = 0; x <= xSize; x++)
                {   
                    float y = NoiseGenerator(x, z, j);

                    if(float.IsInfinity(y) == true)
                    {
                        print("Is Infinite");
                    } 
                    //if (y > maxTerrainHeight)
                    //{
                    //    maxTerrainHeight = y;
                    //}
                    //if (y < minTerrainHeight) {
                    //    minTerrainHeight = y;
                    //}

                    if (vertices[i] == Vector3.zero)
                    {
                        vertices[i] = new Vector3(x, y, z);
                    }
                    else
                    {
                        vertices[i] += new Vector3(0, y, 0);
                    }
                    i++;
                }
            }
        }

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

        print(new Vector2(minTerrainHeight, maxTerrainHeight));

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

    void UpdateMesh() //makes sure mesh is set up correctly and points arent saved from previous mesh
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals(); //fixes lighting of mesh based on surface normals
    }

    float NoiseGenerator(float x, float z, int octave)
    {
        frequency = startingFrequency;
        frequency *= lacunarity * octave;

        //Frequencys zoom the noise in and out/Amplitude increase max and min value of noise
        float perlinValue = Mathf.PerlinNoise((float)x * frequency + xOffset, (float)z * frequency + zOffset);
        float noiseHeight = perlinValue * amplitude;
        noiseHeight *= persistance * octave;
        //frequency *= lacunarity * octave;

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
