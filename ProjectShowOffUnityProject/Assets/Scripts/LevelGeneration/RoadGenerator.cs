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
    [SerializeField] private int howManyTVariants;
    [SerializeField] private bool generateNewPiece;

    private List<GameObject> _activePieces = new List<GameObject>();

    private Vector3 _startPosition;
    private Vector3 _rightPosition;
    private Vector3 _leftPosition;
    private GameObject _activePiece;
    private GameObject _rightSpawn;
    private GameObject _leftSpawn;
    private RoadPoints _activePoints;
    private int _generation;
    private bool _crossRoadGenerated;

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

        if (_activePieces.Count <= piecesGeneratedAtOnce && !_crossRoadGenerated)
        {
            GenerateRoad();
        }

        if (_crossRoadGenerated)
        {
            GenerateRoadsAfterT();
        }
        RemoveRoad();
    }

    private void GenerateRoad()
    {
        _generation++;
        Debug.Log("Road generation:" + _generation);
        int randomRoad = Random.Range(0, roadPieces.Count-1);
        _startPosition = _activePoints.AssetEnd;
        float yDirection = 0;
        float yDirectionT = 0;
        bool isTPiece = false;
        switch (_activePoints.TypeOfRoad)
        {
            case RoadPoints.RoadType.Right:
                //left
                yDirection = 90 + _activePiece.transform.eulerAngles.y;
                break;
            case RoadPoints.RoadType.Left:
                //right
                yDirection = -90 + _activePiece.transform.eulerAngles.y;
                break;
            case RoadPoints.RoadType.Crossroad:
                //tpose
                isTPiece = true;
                break;
            default: //straight 🤢
                yDirection = 0 + _activePiece.transform.eulerAngles.y;
                break;
        }

        if (isTPiece)
        {
            _crossRoadGenerated = true;
            yDirection = 90 + _activePiece.transform.eulerAngles.y;
            yDirectionT = -90 + _activePiece.transform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(-90, 0, yDirection);
            Quaternion rotationT = Quaternion.Euler(-90, 0, yDirectionT);
            _rightPosition = _activePoints.AssetEnd;
            _leftPosition = _activePoints.AssetLeft;
            
            _leftSpawn = Instantiate(roadPieces[0].gameObject, _leftPosition, rotationT);
            _activePieces.Add(_leftSpawn);
            _rightSpawn = Instantiate(roadPieces[0].gameObject, _rightPosition, rotation);
            _activePieces.Add(_rightSpawn);
            
        }
        else
        {
            if (_generation % 6 == 0)
            {
                randomRoad = Random.Range(roadPieces.Count-howManyTVariants, roadPieces.Count-1);
            }
            Quaternion rotation = Quaternion.Euler(-90, 0, yDirection);
            _activePiece = Instantiate(roadPieces[randomRoad].gameObject, _startPosition, rotation);
            _activePieces.Add(_activePiece);
            _activePoints = _activePieces[^1].GetComponent<RoadPoints>();
        }
    }

    private void GenerateRoadsAfterT()
    {
        if (player.currentRoad == _leftSpawn)
        {
            _activePiece = _leftSpawn;
            _activePoints = _leftSpawn.GetComponent<RoadPoints>();
            Destroy(_rightSpawn);
            _activePieces.Remove(_rightSpawn);
            _crossRoadGenerated = false;
            
        } else if (player.currentRoad == _rightSpawn)
        {
            _activePiece = _rightSpawn;
            _activePoints = _rightSpawn.GetComponent<RoadPoints>();
            Destroy(_leftSpawn);
            _activePieces.Remove(_leftSpawn);
            _crossRoadGenerated = false;
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