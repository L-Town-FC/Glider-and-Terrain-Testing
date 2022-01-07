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

    public int xSize = 100;
    public int zSize = 100;

    public int seed;

    [Range(0, 10)]
    public float amplitude = 1f;

    [Range(10, 300)]
    public float noiseScale = 10f;
    float frequency;

    [Range(1, 8)]
    public int octaves; //number of times height is sampled at a point

    [Range(0, 1)]
    public float persistance = 0.5f;

    [Range(0, 5)]
    public float lacunarity = 2;

    Vector2[] octaveOffsets;
    public Vector2 offset;

    public Gradient gradient;

    float minTerrainHeight;
    float maxTerrainHeight;
    public enum ColorMode {NoColor, Color}; //determines how you want to color terrain
    public ColorMode colormode;

    public TerrainType[] regions;

    public void GenerateMap() //function used by editor
    {
        frequency = 1/noiseScale;
        minTerrainHeight = 0;
        maxTerrainHeight = 0;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        octaveOffsets = SeedGenerator(seed, octaves);
        print(octaveOffsets);
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
                    
                    for (int j = 0; j < octaves; j++)
                    {
                        float y = NoiseGenerator(x, z, j);

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
                if(colormode == ColorMode.NoColor)
                {
                    colors[i] = gradient.Evaluate(height);
                }
                else
                {
                    for(int j = 0; j < regions.Length; j++)
                    {
                        if(height <= regions[j].height) //checks if below a threshold height for a region. If higher than that height, check the next region
                        {
                            colors[i] = regions[j].color;
                            break;
                        }
                    }
                }
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

    float NoiseGenerator(float x, float z, int octave)
    {
        //Frequencys zoom the noise in and out/Amplitude increase max and min value of noise
        //offset.x/offset.y are how to manually scroll vertically and horizontally on the noise
        //octave offsets are generated based on the seed
        //x - xsize/2 and z-zsize/2 make it so when you zoom in on the noise you zoom in on the center of the mesh instead of a corner
        float perlinValue = Mathf.PerlinNoise( (float)(x - xSize/2) * frequency + offset.x + octaveOffsets[octave].x, (float)(z  - zSize/2) * frequency + offset.y + octaveOffsets[octave].y);
        float noiseHeight = perlinValue * amplitude;
        noiseHeight *= persistance; //octaves gradually have less and less effect on the overall terrain height
        frequency *= lacunarity; //the sampled points gradually move in/out based on lacurnity and number of octaves

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

    static Vector2[] SeedGenerator(int seed, int octaves)
    {
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        for(int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000);
            float offsetY = prng.Next(-100000, 100000);
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        return octaveOffsets;
    }

    [System.Serializable]
    public struct TerrainType //struct for creating regions beased on height
    {
        public string name;
        public Color color;
        public float height;
    }
}
