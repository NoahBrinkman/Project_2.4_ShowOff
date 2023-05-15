using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private List<RoadPoints> roadPieces;
    [SerializeField] private int piecesGeneratedAtOnce;
    [SerializeField] private bool generateNewPiece;

    private List<GameObject> _activePieces = new List<GameObject>();

    private Vector3 _startPosition;
    private GameObject _activePiece;
    private RoadPoints _activePoints;

    private void Start()
    {
        _startPosition = transform.position;
        _activePiece = Instantiate(roadPieces[0].gameObject, _startPosition, roadPieces[0].gameObject.transform.rotation);
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
            
            _startPosition = _activePoints.AssetEnd;

            Vector3 offset = roadPieces[0].AssetStart - roadPieces[0].gameObject.transform.position;
            Debug.Log(offset);
            float yDirection = 0;
            switch (_activePoints.Direction)
            {
                case 2:
                    //left
                    yDirection = -90 + _activePiece.transform.eulerAngles.y;
                    break;
                case 3:
                    //right
                    yDirection = 90 + _activePiece.transform.eulerAngles.y;
                    break;
                case 4:
                    //tpose
                    break;
                default: //straight 🤢
                    yDirection = 0 + _activePiece.transform.eulerAngles.y;
                    break;
            }
            Quaternion rotation = Quaternion.Euler(-90,0,yDirection);
            _activePiece = Instantiate(roadPieces[0].gameObject, _startPosition-offset , rotation);
            _activePiece.name = "road1";
            _activePieces.Add(_activePiece);
            _activePoints = _activePieces[^1].GetComponent<RoadPoints>();
        }
    }
}
