using System;
using System.Collections;
using System.Collections.Generic;
using LevelGeneration;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private List<RoadPoints> roadPieces;
    [SerializeField] private GameObject startRoad;
    [SerializeField] private PlayerTest player;
    [SerializeField] private int piecesGeneratedAtOnce;
    [SerializeField] private bool generateNewPiece;

    private List<GameObject> _activePieces = new List<GameObject>();

    private Vector3 _startPosition;
    private GameObject _activePiece;
    private RoadPoints _activePoints;
    private int _generation;

    private void Start()
    {
        _activePiece = startRoad;
        _activePieces.Add(_activePiece);
        _activePoints = _activePiece.GetComponent<RoadPoints>();
    }

    private void Update()
    {
        if (generateNewPiece)
        {
            GenerateStartRoads();
            generateNewPiece = false;
        }

        if (_activePieces.Count <= piecesGeneratedAtOnce)
        {
            GenerateRoad();
        }


        RemoveRoad();
    }

    private void GenerateRoad()
    {
        _generation++;
        Debug.Log("Road generation:" + _generation);
        int randomRoad = Random.Range(0, 3);
        _startPosition = _activePoints.AssetEnd;
        float yDirection = 0;
        float yDirectionT = 0;
        bool isTPiece = false;
        switch (_activePoints.Type)
        {
            case 2:
                //left
                yDirection = 90 + _activePiece.transform.eulerAngles.y;
                break;
            case 3:
                //right
                yDirection = -90 + _activePiece.transform.eulerAngles.y;
                break;
            case 4:
                //tpose
                isTPiece = true;
                break;
            default: //straight 🤢
                yDirection = 0 + _activePiece.transform.eulerAngles.y;
                break;
        }

        if (isTPiece)
        {
            yDirection = 90 + _activePiece.transform.eulerAngles.y;
            yDirectionT = -90 + _activePiece.transform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(-90, 0, yDirection);
            Quaternion rotationT = Quaternion.Euler(-90, 0, yDirectionT);
            Vector3 rightPosition = _activePoints.AssetEnd;
            Vector3 leftPosition = _activePoints.AssetLeft;
            
            GameObject leftSpawn = Instantiate(roadPieces[0].gameObject, leftPosition, rotationT);
            _activePieces.Add(leftSpawn);
            GameObject rightSpawn = Instantiate(roadPieces[0].gameObject, rightPosition, rotation);
            _activePieces.Add(rightSpawn);
            
        }
        else
        {
            if (_generation % 6 == 0)
            {
                randomRoad = 3;
            }
            Quaternion rotation = Quaternion.Euler(-90, 0, yDirection);
            _activePiece = Instantiate(roadPieces[randomRoad].gameObject, _startPosition, rotation);
            _activePieces.Add(_activePiece);
            _activePoints = _activePieces[^1].GetComponent<RoadPoints>();
        }
    }

    private void GenerateStartRoads()
    {
        for (int i = 0; i < piecesGeneratedAtOnce - 1; i++)
        {
            GenerateRoad();
        }
    }

    private void RemoveRoad()
    {
        if (player.currentRoad == _activePieces[1])
        {
            Destroy(_activePieces[0]);
            _activePieces.RemoveAt(0);
        }
    }
}