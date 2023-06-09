using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    private Rigidbody rb;
    public float moveSpeed = 5f;

    [SerializeField] private List<RoadGenerator> biomes;
    [HideInInspector]
    public GameObject CurrentRoad { get; private set; }

    private RoadGenerator _activeRoad;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _activeRoad = biomes[0];
        _activeRoad.IsActive = true;
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Vertical");
        float moveVertical = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        rb.AddForce(movement * moveSpeed);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Road") || collision.gameObject.CompareTag("RoadT"))
        {
            CurrentRoad = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Road") || collision.gameObject.CompareTag("RoadT"))
        {
            
        }
    }
}
