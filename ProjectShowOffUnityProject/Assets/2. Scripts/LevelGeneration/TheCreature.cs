using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class TheCreature : MonoBehaviour
{
    private RoadPoints _roadPoints;
    private bool _creatureSpawned;
    private int _roadRotation;
    private Vector3 _roadCenter;
    [SerializeField] private float arrowHeightOffset;
    [SerializeField] private float creatureHeightOffset;
    [SerializeField] private List<GameObject> arrows;
    [SerializeField] private List<GameObject> creatures;
    private void OnEnable()
    {
        _roadPoints = GetComponent<RoadPoints>();
        _roadCenter = _roadPoints.RoadCenter;
        if (creatures.Count > 0) SpawnCreatures();
        if (arrows.Count > 0) CreateArrow();
        
    }

    private void SpawnCreatures()
    {
        _roadRotation = ((int)transform.localRotation.eulerAngles.y + 360) % 360;
        int randomCreature = Random.Range(0, creatures.Count);
        GameObject theCreature = Instantiate(creatures[randomCreature], new Vector3(_roadPoints.CreatureSpawn.x, _roadPoints.CreatureSpawn.y + creatureHeightOffset, _roadPoints.CreatureSpawn.z), Quaternion.Euler(0,_roadRotation-90,0));
        theCreature.transform.parent = transform;
    }

    private void CreateArrow()
    {
        float randomDirection = Random.Range(0, 1);
        if (randomDirection == 0)
        {
            GameObject arrowLeft = Instantiate(arrows[0], new Vector3(_roadCenter.x, _roadCenter.y + arrowHeightOffset, _roadCenter.z),
                Quaternion.Euler(0, _roadRotation - 90, 0));
            arrowLeft.transform.parent = transform;
        }
        else
        {
            GameObject arrowLeft = Instantiate(arrows[1], new Vector3(_roadCenter.x, _roadCenter.y + arrowHeightOffset, _roadCenter.z),
                Quaternion.Euler(0, _roadRotation - 90, 0));
            arrowLeft.transform.parent = transform;
        }

    }
}