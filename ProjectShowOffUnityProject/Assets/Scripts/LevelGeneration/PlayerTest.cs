using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    private Rigidbody rb;
    public float moveSpeed = 5f;
    public GameObject currentRoad;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
            Debug.Log($"Player entered {collision.gameObject.name}");
            currentRoad = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Road") || collision.gameObject.CompareTag("RoadT"))
        {
            Debug.Log($"Player left {collision.gameObject.name}");
        }
    }
}
