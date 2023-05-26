using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LevelGeneration
{
    public class ObstaclesSpawn : MonoBehaviour
    {
        [SerializeField] private List<GameObject> obstaclesToAvoid = new List<GameObject>();

        [SerializeField] private float heightOffset;
        [SerializeField] private float widthOffset;
        [SerializeField] private float spacing = 2.0f;

        private Bounds _objectBounds;
        private Dictionary<GameObject, Bounds> _objectsBoundsMap = new Dictionary<GameObject, Bounds>();
        private Renderer _roadRenderer;
        private Obstacle.WhichObstacle _chosenObstacle;

        public List<Vector3> _areasPoints = new List<Vector3>();

        private void OnEnable()
        {
            _roadRenderer = GetComponent<Renderer>();

            for (int i = 0; i <= 4; i += 2)
            {
                Vector3 point1 = new Vector3(transform.position.x + (spacing - 0.5f) + i,
                    _roadRenderer.bounds.max.y,
                    transform.position.z);
                Vector3 point2 = new Vector3(transform.position.x + (spacing + 0.5f) + i,
                    _roadRenderer.bounds.max.y,
                    transform.position.z);
                Vector3 point3 = new Vector3(transform.position.x + spacing + i,
                    _roadRenderer.bounds.max.y,
                    transform.position.z - widthOffset);
                Vector3 point4 = new Vector3(transform.position.x + spacing + i,
                    _roadRenderer.bounds.max.y,
                    transform.position.z + widthOffset);

                _areasPoints.Add(point1);
                _areasPoints.Add(point2);
                _areasPoints.Add(point3);
                _areasPoints.Add(point4);
            }

            int random = Random.Range(1, 30) % 3 + 1;

            // 0 1 2 3 || 4 5 6 7 || 8 9 10 11
            for (int i = 0; i < random; i++)
            {
                GameObject obstacle = Instantiate(obstaclesToAvoid[Random.Range(0,obstaclesToAvoid.Count-1)], transform);
                _chosenObstacle = obstacle.GetComponent<Obstacle>().ObstacleType;
                obstacle.transform.position = RandomPositionInTheArea(_areasPoints[4 * i], _areasPoints[4 * i + 1],
                    _areasPoints[4 * i + 3],
                    _areasPoints[4 * i + 2], _chosenObstacle);
            }
        }

        /// <summary>
        /// Find a random position in the area (with additional requirements like height offset)
        /// </summary>
        /// <param name="startPosition">    Start point of the area</param>
        /// <param name="endPosition">      End point of the area</param>
        /// <param name="leftPosition">     Left point of the area</param>
        /// <param name="rightPosition">    Right point of the area</param>
        /// <param name="obstacleType">     Type of obstacle - used to determine the calculations</param>
        /// <returns></returns>
        private Vector3 RandomPositionInTheArea(Vector3 startPosition, Vector3 endPosition, Vector3 leftPosition,
            Vector3 rightPosition, Obstacle.WhichObstacle obstacleType)
        {
            float newX = 0;
            float newZ = 0;
            float newY = 0;
            
            float randomX = Random.Range(startPosition.x, endPosition.x);
            switch (obstacleType)
            {
                //Randomise both X and Z
                case Obstacle.WhichObstacle.RunAround:
                    newX = randomX;
                    newZ = Random.Range(leftPosition.z, rightPosition.z);
                    newY = startPosition.y;
                    break;
                //Randomise only X
                case Obstacle.WhichObstacle.Jump:
                    newX = randomX;
                    newZ = startPosition.z;
                    newY = startPosition.y;
                    break;
                //Randomise X and move the object up
                case Obstacle.WhichObstacle.Slide:
                    newX = randomX;
                    newZ = startPosition.z;
                    newY = startPosition.y + heightOffset;
                    break;
            }
            Vector3 newPosition = new Vector3(newX, newY, newZ);
            
            return newPosition;
        }
    }
}