using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Obstacle : MonoBehaviour
{
    public enum WhichObstacle
    {
        RunAround = 1,
        Jump,
        Slide
    }

    [SerializeField] private WhichObstacle obstacleType;

    public WhichObstacle ObstacleType
    {
        get => obstacleType;
        set => obstacleType = value;
    }
}