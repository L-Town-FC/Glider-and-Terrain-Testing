using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updraft : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<GliderMovement>().inUpdraft = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<GliderMovement>().inUpdraft = false;
        }
    }
}
