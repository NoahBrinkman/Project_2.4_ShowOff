using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SmoothGameplayHandler : MonoBehaviour
{
    [SerializeField] private List<PlayerWithLastRecordedPosition> players = new List<PlayerWithLastRecordedPosition>();
    [SerializeField, Tooltip("What DOT PRODUCT Result (or lower) is needed for the handler to start timing. This value ranges between -1 and 1\n0 = a 90° angle\n 1 = facing the same direction -1 is facing opposite directions")] 
    private float rotationSensitivity = -0.75f;


    private void Start()
    {
        if(players.Count == 0) Destroy(gameObject);

        for(int i = 0; i < players.Count; i++)
        {
            players[i].rotationTimer = players[i].timeBeforeSnapRotation;
            players[i].positionSnapTimer = players[i].timeBeforeSnapPosition;
        }
    }

    private void Update()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (!isCorrectlyAligned(i))
            {
                players[i].rotationTimer -= Time.deltaTime;
            }
            if(!hasMoved(i))
            {
                players[i].positionSnapTimer -= Time.deltaTime;
            }

            if (players[i].rotationTimer <= 0)
            {
     
                players[i].shouldRotate = true;
            }

            if (players[i].positionSnapTimer <= 0)
            {
           
                players[i].shouldSnap = true;
            }

            if (players[i].shouldRotate && players[i].Player.CurrentRoad.CurvePoints.Count == 0)
            {
                Vector3 directionVector = players[i].Player.CurrentRoad.transform.position - players[i].Player.transform.position;
                Vector3 rightVectorB = players[i].Player.CurrentRoad.transform.right;
                Quaternion rotation = Quaternion.LookRotation(directionVector, rightVectorB);
                players[i].Player.transform.rotation = rotation;
                players[i].rotationTimer = players[i].timeBeforeSnapRotation;
                players[i].shouldRotate = false;
            }
            if (players[i].shouldSnap)
            {
                players[i].Player.transform.position = players[i].Player.CurrentRoad.AssetEnd; // Change to worldspace if local
                players[i].positionSnapTimer = players[i].timeBeforeSnapPosition;
                players[i].shouldSnap = false;
            }
        }

    }


    bool hasMoved(int indexOfPlayer)
    {
        if (players[indexOfPlayer].Player.transform.position != players[indexOfPlayer].Position)
        {
            players[indexOfPlayer].Position = players[indexOfPlayer].Player.transform.position;
            return true;
        }
        return false;
    }

    bool isCorrectlyAligned(int indexOfPlayer)
    {
        return Vector3.Dot(players[indexOfPlayer].Player.CurrentRoad.transform.right, players[indexOfPlayer].Player.transform.forward)  > rotationSensitivity;
    }

}

[Serializable]
public class PlayerWithLastRecordedPosition
{
    public PlayerStateMachine Player;
    public Vector3 Position;
    public float timeBeforeSnapRotation = .5f;
    public float rotationTimer = .5f;
    public bool shouldRotate = false;
    
    public float timeBeforeSnapPosition = .5f;
    public float positionSnapTimer = .5f;
    public bool shouldSnap = false;

}

