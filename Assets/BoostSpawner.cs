using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSpawner : MonoBehaviour
{

    public Terrain terrain;
    public int totalBoosts;
    Vector3 mapBorder;
    Vector3 mapCenter;

    // Start is called before the first frame update
    void Start()
    {
        print(terrain.terrainData.size);
        print(terrain.terrainData.bounds);
        mapBorder = terrain.terrainData.bounds.extents;
        mapCenter = terrain.terrainData.bounds.center;
        //use terraindata.getheight to get heights for each boost spawn

        for(var i = 0; i < totalBoosts; i++) //kinda works
        {
            //for each boost spawn choose a random x,y coordinate in the terrains boundaries, look up the height on the terrain map, then add a random amount to that to put it in the air above the terrain
            Vector3 position = new Vector3(Random.Range(-mapBorder.x/2, mapBorder.x/2) + mapCenter.x, 0, Random.Range(-mapBorder.z / 2, mapBorder.z / 2) + mapCenter.z);
            position.y = terrain.terrainData.GetHeight((int)position.x, (int)position.z) + mapCenter.y;
            GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = position; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
