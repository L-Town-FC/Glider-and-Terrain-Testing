using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSpawner : MonoBehaviour
{

    public Terrain terrain;
    public int totalBoosts;

    // Start is called before the first frame update
    void Start()
    {
        print(terrain.terrainData.size);
        print(terrain.terrainData.bounds);
        //use terraindata.getheight to get heights for each boost spawn

        for(var i = 0; i < totalBoosts; i++)
        {
            //for each boost spawn choose a random x,y coordinate in the terrains boundaries, look up the height on the terrain map, then add a random amount to that to put it in the air above the terrain

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
