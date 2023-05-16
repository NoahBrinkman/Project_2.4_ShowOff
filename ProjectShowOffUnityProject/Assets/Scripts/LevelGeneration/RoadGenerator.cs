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
    [SerializeField] private int piecesGeneratedAtOnce;
    [SerializeField] private bool generateNewPiece;

    private List<GameObject> _activePieces = new List<GameObject>();

    private Vector3 _startPosition;
    private GameObject _activePiece;
    private RoadPoints _activePoints;

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
        
    }

    private void GenerateRoad()
    {
        throw new NotImplementedException();
    }

    private void GenerateStartRoads()
    {

        for (int i = 0; i < piecesGeneratedAtOnce - 1; i++)
        {
            int randomRoad = Random.Range(0, 3);
            _startPosition = _activePoints.AssetEnd;
            Debug.Log($"Start position: {_startPosition}");
            float yDirection = 0;
            float yDirectionT = 0;
            bool isTPiece = false;
            switch (_activePoints.Direction)
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
                    yDirection = 90 + _activePiece.transform.eulerAngles.y;
                    yDirectionT = -90 + _activePiece.transform.eulerAngles.y;
                    break;
                default: //straight 🤢
                    yDirection = 0 + _activePiece.transform.eulerAngles.y;
                    break;
            }
            Quaternion rotation = Quaternion.Euler(-90,0,yDirection);
            _activePiece = Instantiate(roadPieces[randomRoad].gameObject, _startPosition , rotation);
            _activePiece.name = $"road{i}";
            
            if (isTPiece)
            {
                _startPosition = _activePoints.AssetLeft;
                Quaternion rotationT = Quaternion.Euler(-90,0,yDirectionT);
                _activePiece = Instantiate(roadPieces[randomRoad].gameObject, _startPosition , rotationT);
                _activePiece.name = $"road{i}";
                _activePieces.Add(_activePiece);
                _activePoints = _activePieces[^1].GetComponent<RoadPoints>();
            }
            
            _activePieces.Add(_activePiece);
            _activePoints = _activePieces[^1].GetComponent<RoadPoints>();

            
        }
    }
}
