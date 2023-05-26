using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private enum ObstacleType
    {
        RunAround = 1,
        Jump,
        Slide
    }

    [SerializeField]
    private ObstacleType obstacleType;
}
