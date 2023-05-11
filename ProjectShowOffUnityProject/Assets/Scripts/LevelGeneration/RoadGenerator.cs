using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> roadPieces;
    [SerializeField] private int piecesGeneratedAtOnce;
    [SerializeField] private bool generateNewPiece;

    private List<GameObject> activePieces;

    private void Start()
    {
        GenerateStartRoads();
    }

    private void Update()
    {
        if (generateNewPiece)
        {
            GenerateRoad();
        }
    }

    private void GenerateRoad()
    {
        throw new NotImplementedException();
    }

    private void GenerateStartRoads()
    {
        GameObject startPiece = Instantiate(roadPieces[0], transform);
        startPiece.name = "Start Road";
        startPiece.transform.SetParent(transform);

        for (int i = 1; i < piecesGeneratedAtOnce; i++)
        {
            GameObject nextPiece = Instantiate(roadPieces[0], transform.position, Quaternion.identity);
        }
    }
}
