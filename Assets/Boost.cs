using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    Vector3 initialPosition;
    float amplitude = 1f;
    private void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(initialPosition.x, initialPosition.y + amplitude * Mathf.Sin(initialPosition.magnitude * Time.time/400), initialPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Destroy boost
        //Increase Gliders speed by a small amount
        //Make particle explosion where boost used to be


    }
}
