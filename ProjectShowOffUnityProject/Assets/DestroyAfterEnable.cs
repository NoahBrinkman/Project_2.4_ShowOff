using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterEnable : MonoBehaviour
{
    [SerializeField] private float timeBeforeDestruction;

    private void OnEnable()
    {
        Destroy(gameObject, timeBeforeDestruction);
    }
}
