using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSpawner : MonoBehaviour
{
    public GameObject boost;
    public Terrain terrain;
    public int totalBoosts;
    Vector3 mapBorder;
    Vector3 mapCenter;

    public void GenerateMap()
    {
        print(terrain.terrainData.size);
        print(terrain.terrainData.bounds);
        mapBorder = terrain.terrainData.bounds.extents;
        mapCenter = terrain.terrainData.bounds.center;
        //use terraindata.getheight to get heights for each boost spawn

        GameObject boostHolder = new GameObject("BoostHolder");
        if(transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        boostHolder.transform.SetParent(transform);

        if (boost != null)
        {
            for (var i = 0; i < totalBoosts; i++) //kinda works
            {
                //for each boost spawn choose a random x,y coordinate in the terrains boundaries, look up the height on the terrain map, then add a random amount to that to put it in the air above the terrain
                Vector3 position = new Vector3(Random.Range(-mapBorder.x / 1, mapBorder.x / 1) + mapCenter.x, 0, Random.Range(-mapBorder.z / 1, mapBorder.z / 1) + mapCenter.z);
                position.y = terrain.terrainData.GetHeight((int)position.x, (int)position.z) + mapCenter.y;
                Instantiate(boost, position, Quaternion.identity, boostHolder.transform);
            }
        }
        
    }

}
