using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwirlingPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //If we can get the component store it for now in temporary variable P
        if (other.GetComponent<PlayerMovement>() is PlayerMovement p != null)
        {
            
        }
    }
}
