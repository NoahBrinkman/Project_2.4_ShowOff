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
            Vector3 pointFront;
            Vector3 pointBack;
            Vector3 pointLeft;
            Vector3 pointRight;
            float positionX = transform.position.x;
            float positionY = _roadRenderer.bounds.max.y;
            float positionZ = transform.position.z;
            int modifier = 1;

            int roadRotation = ((int)transform.localRotation.eulerAngles.y + 360) % 360;

            for (int i = 0; i <= 4; i += 2)
            {
                switch (roadRotation)
                {
                    case 0:
                        GeneratePointsVertical(positionX, 1, i, positionY, positionZ);
                        break;
                    case 90:
                        GeneratePointsHorizontal(positionX, positionY, positionZ, -1, i);
                        break;
                    case 180:
                        GeneratePointsVertical(positionX, -1, i, positionY, positionZ);
                        break;
                    case 270:
                        GeneratePointsHorizontal(positionX, positionY, positionZ, 1, i);
                        break;
                }
            }

            int random = Random.Range(1, 30) % 3 + 1;

            // 0 1 2 3 || 4 5 6 7 || 8 9 10 11
            for (int i = 0; i < random; i++)
            {
                GameObject obstacle = Instantiate(obstaclesToAvoid[Random.Range(0, obstaclesToAvoid.Count - 1)],
                    transform);
                _chosenObstacle = obstacle.GetComponent<Obstacle>().ObstacleType;
                obstacle.transform.position = RandomPositionInTheArea(_areasPoints[4 * i], _areasPoints[4 * i + 1],
                    _areasPoints[4 * i + 3], _areasPoints[4 * i + 2], _chosenObstacle);
            }
        }

        private void GeneratePointsHorizontal(float x, float y, float z, int modifier, int offset)
        {
            var pointFront = new Vector3(x, y, z + (spacing - 0.5f) * modifier + offset * modifier);
            var pointBack = new Vector3(x, y, z + (spacing + 0.5f) * modifier + offset * modifier);
            var pointRight = new Vector3(x - widthOffset, y, z + spacing * modifier + offset * modifier);
            var pointLeft = new Vector3(x + widthOffset, y, z + spacing * modifier + offset * modifier);
            _areasPoints.Add(pointFront);
            _areasPoints.Add(pointBack);
            _areasPoints.Add(pointRight);
            _areasPoints.Add(pointLeft);
        }

        private void GeneratePointsVertical(float x, int modifier, int offset, float y, float z)
        {
            Vector3 pointFront = new Vector3(x + (spacing - 0.5f) * modifier + offset * modifier, y, z);
            Vector3 pointBack = new Vector3(x + (spacing + 0.5f) * modifier + offset * modifier, y, z);
            Vector3 pointRight = new Vector3(x + spacing * modifier + offset * modifier, y, z - widthOffset);
            Vector3 pointLeft = new Vector3(x + spacing * modifier + offset * modifier, y, z + widthOffset);
            _areasPoints.Add(pointFront);
            _areasPoints.Add(pointBack);
            _areasPoints.Add(pointRight);
            _areasPoints.Add(pointLeft);
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