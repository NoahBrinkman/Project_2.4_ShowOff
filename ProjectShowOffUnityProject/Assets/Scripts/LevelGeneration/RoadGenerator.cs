using System;
using System.Collections;
using System.Collections.Generic;
using LevelGeneration;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private List<RoadPoints> roadPieces;
    [SerializeField] private PlayerTest player;
    [Tooltip("How many pieces the generator will keep generated at once. I recommend number smaller than 4 to prevent overlapping")]
    [SerializeField] private int piecesGeneratedAtOnce = 3;
    [Tooltip("How many crossroads variants do we have in the generator")]
    [SerializeField] private int howManyTVariants;
    [Tooltip("After how many road pieces the crossroad should be generated")]
    [SerializeField] private int whenToSpawnCross = 6;

    [SerializeField] private int borderSize = 1000;
    public int BorderSize => borderSize;
    public bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    public bool Clear
    {
        get => _clear;
        set => _clear = value;
    }

    private readonly List<GameObject> _activePieces = new List<GameObject>();

    private Vector3 _startPosition;
    private GameObject _activePiece;
    private GameObject _rightSpawn;
    private GameObject _leftSpawn;
    private RoadPoints _activePoints;
    private int _generation;
    private bool _closeToEdge;
    private bool _crossRoadGenerated;
    private bool _generateNewPiece;
    private bool _isActive;
    private bool _clear;
    [SerializeField]
    private bool _clockwise;

    

    private void Start()
    {
        _activePiece = CreateNewActivePiece(Quaternion.Euler(-90, 0, 0), transform.position);
        _activePoints = _activePiece.GetComponent<RoadPoints>();
        
        Debug.Log($"I am {name} and my position is {transform.position}");
    }

    private void Update()
    {
        if (_isActive)
        {
            if (_generateNewPiece)
            {
                GenerateStartRoads();
                _generateNewPiece = false;
            }

            if (_activePieces.Count <= piecesGeneratedAtOnce && !_crossRoadGenerated)
            {
                GenerateRoad();
            }

            if (_crossRoadGenerated)
            {
                GenerateRoadsAfterCrossRoad();
            }

            RemoveRoad();
        }

        if (_clear && !_isActive)
        {
            ClearGenerator();
        }
    }

    private void ClearGenerator()
    {
        _clear = false;
        foreach (GameObject piece in _activePieces)
        {
            Destroy(piece);
        }
        
        _activePieces.Clear();
        
        CreateNewActivePiece(Quaternion.Euler(-90, 0, 0), transform.position);
        _activePoints = _activePiece.GetComponent<RoadPoints>();
        
    }
    

    /// <summary>
    /// Makes sure to generate roads correctly after the crossroads
    /// </summary>
    private void GenerateRoadsAfterCrossRoad()
    {
        if (player.CurrentRoad == _leftSpawn)
        {
            ChooseRoad(_leftSpawn, _rightSpawn);
            _clockwise = false;

        } else if (player.CurrentRoad == _rightSpawn)
        {
            ChooseRoad(_rightSpawn, _leftSpawn);
            _clockwise = true;
        }
    }

    /// <summary>
    /// Checks which direction player chose, sets this direction as the latest road and removes roads that are on the opposite side
    /// </summary>
    /// <param name="roadToLeave">Road that player chose and is gonna leave soon</param>
    /// <param name="roadToRemove">Road that player hasn't chosen and is gonna be removed</param>
    private void ChooseRoad(GameObject roadToLeave, GameObject roadToRemove)
    {
        _activePiece = roadToLeave;
        _activePoints = roadToLeave.GetComponent<RoadPoints>();
        Destroy(roadToRemove);
        _activePieces.Remove(roadToRemove);
        _crossRoadGenerated = false;
    }

    /// <summary>
    /// Generates the set of start roads
    /// </summary>
    private void GenerateStartRoads()
    {
        for (int i = 0; i < piecesGeneratedAtOnce - 1; i++)
        {
            GenerateRoad();
        }
    }

    /// <summary>
    /// Removes the oldest road from the scene and from the list
    /// </summary>
    private void RemoveRoad()
    {
        if (player.CurrentRoad == _activePieces[1])
        {
            Destroy(_activePieces[0]);
            _activePieces.RemoveAt(0);
        }
    }
    
    /// <summary>
    /// Generates road piece depends on the previous ones rotation.
    /// </summary>
    private void GenerateRoad()
    {
        _generation++;
        _startPosition = _activePoints.AssetEnd;
        
        int randomRoad = Random.Range(0, roadPieces.Count-1);
        //int randomRoad = 0;
        bool isACrossroad = false;
        float yDirection = 0;
        float pieceYRotation = _activePiece.transform.eulerAngles.y;
        
        switch (_activePoints.TypeOfRoad)
        {
            case RoadPoints.RoadType.Right:
                yDirection = 90 + pieceYRotation;
                break;
            case RoadPoints.RoadType.Left:
                yDirection = -90 + pieceYRotation;
                break;
            case RoadPoints.RoadType.Crossroad:
                isACrossroad = true;
                break;
            case RoadPoints.RoadType.Straight:
                yDirection = 0 + pieceYRotation;
                int roadRotation = ((int)pieceYRotation + 360) % 360;
                Debug.Log($"Road rotation: {roadRotation}");
                Debug.Log("Last road position: " + _activePieces[^1].transform.position.z);
                Debug.Log("Border position: " + (transform.position.z + borderSize));
                Debug.Log($"IS it clockwise? {_clockwise}");
                if ((_activePiece.transform.position.x + _activePoints.Height * 3 >= transform.position.x + borderSize / 2.0f && roadRotation == 0)
                    || (_activePiece.transform.position.x - _activePoints.Height * 3 <= transform.position.x - borderSize / 2.0f && roadRotation == 180)
                    || (_activePiece.transform.position.z + _activePoints.RoadWidth * 3 >= transform.position.z + borderSize / 2.0f && roadRotation == 270)
                    || (_activePiece.transform.position.z - _activePoints.RoadWidth * 3 <= transform.position.z - borderSize / 2.0f && roadRotation == 90))
                {
                    Debug.Log("It will be too much");
                    _closeToEdge = true;
                    randomRoad = Random.Range(1, roadPieces.Count-1);
                    Debug.DrawLine(_activePiece.transform.position, 
                        new Vector3(_activePiece.transform.position.x + _activePoints.Height * 3, _activePiece.transform.position.y, _activePiece.transform.position.z), 
                        Color.red, 20.0f);
                    Debug.DrawLine(_activePiece.transform.position, 
                        new Vector3(_activePiece.transform.position.x - _activePoints.Height * 3, _activePiece.transform.position.y, _activePiece.transform.position.z), 
                        Color.red, 20.0f);
                    Debug.DrawLine(_activePiece.transform.position, 
                        new Vector3(_activePiece.transform.position.x, _activePiece.transform.position.y, _activePiece.transform.position.z + _activePoints.RoadWidth * 3), 
                        Color.red, 20.0f);
                    Debug.DrawLine(_activePiece.transform.position, 
                        new Vector3(_activePiece.transform.position.x, _activePiece.transform.position.y, _activePiece.transform.position.z - _activePoints.RoadWidth * 3), 
                        Color.red, 20.0f);
                }
                break;
            default:
                
                break;
        }

        if (isACrossroad)
        {
            
            GenerateCrossRoad();

        }
        else if (_closeToEdge)
        {
            int roadRotation = ((int)pieceYRotation + 360) % 360;
            if (!_clockwise)
            {
                randomRoad = 1;
            }
            else
            {
                randomRoad = 2;
            }
            Quaternion rotation = Quaternion.Euler(-90, 0, yDirection);
            _activePiece = CreateNewActivePiece(rotation,_startPosition, randomRoad);
            _activePoints = _activePieces[^1].GetComponent<RoadPoints>();
            _closeToEdge = false;
        }
        else
        {
            if (_generation % whenToSpawnCross == 0)
            {
                randomRoad = Random.Range(roadPieces.Count-howManyTVariants, roadPieces.Count-1);
            }

            Quaternion rotation = Quaternion.Euler(-90, 0, yDirection);
            _activePiece = CreateNewActivePiece(rotation,_startPosition, 0);
            _activePoints = _activePieces[^1].GetComponent<RoadPoints>();
        }
    }

    /// <summary>
    /// Generated roads on the both ends of the crossroad. The roads are ALWAYS straight
    /// </summary>
    private void GenerateCrossRoad()
    {
        _crossRoadGenerated = true;
        float pieceYRotation = _activePiece.transform.eulerAngles.y;

        float yDirection = 90 + pieceYRotation;
        float yDirectionT = -90 + pieceYRotation;
            
        Quaternion rotation = Quaternion.Euler(-90, 0, yDirection);
        Quaternion rotationT = Quaternion.Euler(-90, 0, yDirectionT);
            
        Vector3 rightPosition = _activePoints.AssetEnd;
        Vector3 leftPosition = _activePoints.AssetLeft;

        _leftSpawn = CreateNewActivePiece(rotationT, leftPosition);
        _rightSpawn = CreateNewActivePiece(rotation, rightPosition);
    }

    /// <summary>
    /// Creates new road piece and adds it to the list of active (present in the scene) pieces
    /// </summary>
    /// <param name="rotation">Rotation of the piece, important to make sure that the road is going correctly</param>
    /// <param name="startPosition">Start position of the piece</param>
    /// <param name="roadPieceNumber">Number deciding which piece of road should be generated</param>
    /// <returns></returns>
    private GameObject CreateNewActivePiece(Quaternion rotation, Vector3 startPosition, int roadPieceNumber = 0)
    {
        GameObject newPiece = Instantiate(roadPieces[roadPieceNumber].gameObject, startPosition, rotation);
        if (roadPieceNumber == 1) _clockwise = false;
        if (roadPieceNumber == 2) _clockwise = true;
        newPiece.transform.parent = transform;
        _activePieces.Add(newPiece);
        return newPiece;
    }
}