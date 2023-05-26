using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LevelGeneration
{
    public class ObstaclesSpawn : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> obstaclesToAvoid = new List<GameObject>();

        private Bounds _objectBounds;
        private Dictionary<GameObject, Bounds> _objectsBoundsMap = new Dictionary<GameObject, Bounds>();
        private Renderer _roadRenderer;

        public List<Vector3> _areasPoints = new List<Vector3>();

        private void OnEnable()
        {
            _roadRenderer = GetComponent<Renderer>();
            // foreach (GameObject obstacle in obstaclesToAvoid)
            // {
            //     _objectsBoundsMap.Add(obstacle, obstacle.GetComponent<Renderer>().bounds);
            // }

            for (int i = 0; i <= 4; i+=2)
            {
                Vector3 point1 = new Vector3(transform.position.x + 1.5f + i,
                    transform.position.y,
                    transform.position.z);
                Vector3 point2 = new Vector3(transform.position.x + 2.5f + i,
                    transform.position.y,
                    transform.position.z);
                Vector3 point3 = new Vector3(transform.position.x + 2.0f + i,
                    transform.position.y,
                    transform.position.z-_roadRenderer.bounds.size.z/2);
                Vector3 point4 = new Vector3(transform.position.x + 2.0f + i,
                    transform.position.y,
                    transform.position.z+_roadRenderer.bounds.size.z/2);
                
                _areasPoints.Add(point1);
                _areasPoints.Add(point2);
                _areasPoints.Add(point3);
                _areasPoints.Add(point4);
            }

            int random = Random.Range(1, 3);

            // 0 1 2 3 || 4 5 6 7 || 8 9 10 11
            for (int i = 0; i < 3; i++)
            {
                GameObject obstacle = Instantiate(obstaclesToAvoid[0], transform);
                obstacle.transform.position = RandomVector3(_areasPoints[4 * i], _areasPoints[4 * i + 1], _areasPoints[4 * i + 3],
                    _areasPoints[4 * i + 2]);
            }
        }

        private Vector3 RandomVector3(Vector3 startPosition, Vector3 endPosition, Vector3 leftPosition, Vector3 rightPosition)
        {
            // startPosition = transform.TransformPoint(startPosition);
            // endPosition = transform.TransformPoint(endPosition);
            // leftPosition = transform.TransformPoint(leftPosition);
            // rightPosition = transform.TransformPoint(rightPosition);
            float newX = Random.Range(startPosition.x, endPosition.y);
            float newZ = Random.Range(leftPosition.z, rightPosition.z);

            Vector3 newPosition = new Vector3(newX, startPosition.y, newZ);
            return newPosition;
        }
    }
}
