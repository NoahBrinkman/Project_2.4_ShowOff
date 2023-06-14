using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class TheCreature : MonoBehaviour
{
    private RoadPoints _roadPoints;
    private bool _creatureSpawned;
    private int _roadRotation;
    private Vector3 _roadCenter;
    [SerializeField] private float _heightOffset;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private List<GameObject> _creatures;
    private void OnEnable()
    {
        _roadPoints = GetComponent<RoadPoints>();
        _roadCenter = _roadPoints.RoadCenter;
        SpawnCreatures();
        // SpawnText();
        CreateArrow();
        Debug.Log(_roadPoints.CreatureSpawn);
    }

    private void SpawnCreatures()
    {
        _roadRotation = ((int)transform.localRotation.eulerAngles.y + 360) % 360;
        int randomCreature = Random.Range(0, _creatures.Count);
        GameObject theCreature = Instantiate(_creatures[randomCreature], _roadPoints.CreatureSpawn, Quaternion.Euler(0,_roadRotation-90,0));
    }

    private void CreateArrow()
    {
        // float randomDirection = Random.Range(0, 1);
        // if (randomDirection == 0)
        // {
            GameObject arrowLeft = Instantiate(_arrow, new Vector3(_roadCenter.x, _roadCenter.y + _heightOffset, _roadCenter.z),
                Quaternion.Euler(0, _roadRotation - 90, 0));
       // }

    }
}