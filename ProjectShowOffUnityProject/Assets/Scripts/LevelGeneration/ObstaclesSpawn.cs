using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LevelGeneration
{
    //TODO: tool to specify what type of object needs to spawn, how many areas etc
    public class ObstaclesSpawn : MonoBehaviour
    {
        [HideInInspector]
        public List<Vector3> AreasPoints = new List<Vector3>();
        
        [Tooltip("List of obstacles that have a chance to get spawned on the road")]
        [SerializeField] private List<Obstacle> obstaclesToAvoid = new List<Obstacle>();
        
        [Tooltip("Offset of height - used to generate sliding obstacles")] [SerializeField] 
        private float heightOffset;
        [Tooltip("Offset of width - used to determine how far to the side objects can be spawned")] [SerializeField] 
        private float widthOffset;
        [Tooltip("Space from the start and between the areas")] [SerializeField] 
        private float spacing = 2.0f;
        
        private Renderer _roadRenderer;
        private Obstacle.WhichObstacle _chosenObstacle;
        

        private void OnEnable()
        {
            _roadRenderer = GetComponent<Renderer>();
            float positionX = transform.position.x;
            float positionY = _roadRenderer.bounds.max.y;
            float positionZ = transform.position.z;

            int roadRotation = ((int)transform.localRotation.eulerAngles.y + 360) % 360;

            for (int i = 0; i <= 4; i += 2)
            {
                switch (roadRotation)
                {
                    case 0:
                        GeneratePointsVertical(positionX, positionY, positionZ, 1, i);
                        break;
                    case 90:
                        GeneratePointsHorizontal(positionX, positionY, positionZ, -1, i);
                        break;
                    case 180:
                        GeneratePointsVertical(positionX, positionY, positionZ, -1, i);
                        break;
                    case 270:
                        GeneratePointsHorizontal(positionX, positionY, positionZ, 1, i);
                        break;
                }
            }

            int random = Random.Range(1, 31) % 3 + 1;

            if (obstaclesToAvoid.Count != 0)
            {
                for (int i = 0; i < random; i++)
                {
                    GameObject obstacle = Instantiate(obstaclesToAvoid[Random.Range(0, obstaclesToAvoid.Count)].gameObject,
                        transform);
                    _chosenObstacle = obstacle.GetComponent<Obstacle>().ObstacleType;
                    obstacle.transform.position = RandomPositionInTheArea(AreasPoints[4 * i], AreasPoints[4 * i + 1],
                        AreasPoints[4 * i + 3], AreasPoints[4 * i + 2], _chosenObstacle);
                }
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
        
        /// <summary>
        /// Generate areas for the obstacles to spawn - HORIZONTAL
        /// </summary>
        /// <param name="x">        Start of the area</param>
        /// <param name="y">        Y of the road</param>
        /// <param name="z">        End of the area</param>
        /// <param name="modifier"> Used in the opposite roads</param>
        /// <param name="offset">   Offset for the areas</param>
        private void GeneratePointsHorizontal(float x, float y, float z, int modifier, int offset)
        {
            var pointFront = new Vector3(x, y, z + (spacing - 0.5f) * modifier + offset * modifier);
            var pointBack = new Vector3(x, y, z + (spacing + 0.5f) * modifier + offset * modifier);
            var pointRight = new Vector3(x - widthOffset, y, z + spacing * modifier + offset * modifier);
            var pointLeft = new Vector3(x + widthOffset, y, z + spacing * modifier + offset * modifier);
            AreasPoints.Add(pointFront);
            AreasPoints.Add(pointBack);
            AreasPoints.Add(pointRight);
            AreasPoints.Add(pointLeft);
        }

        /// <summary>
        /// Generate areas for the obstacles to spawn - VERTICAL
        /// </summary>
        /// <param name="x">        Start of the area</param>
        /// <param name="y">        Y of the road</param>
        /// <param name="z">        End of the area</param>
        /// <param name="modifier"> Used in the opposite roads</param>
        /// <param name="offset">   Offset for the areas</param>
        private void GeneratePointsVertical(float x, float y, float z, int modifier, int offset)
        {
            Vector3 pointFront = new Vector3(x + (spacing - 0.5f) * modifier + offset * modifier, y, z);
            Vector3 pointBack = new Vector3(x + (spacing + 0.5f) * modifier + offset * modifier, y, z);
            Vector3 pointRight = new Vector3(x + spacing * modifier + offset * modifier, y, z - widthOffset);
            Vector3 pointLeft = new Vector3(x + spacing * modifier + offset * modifier, y, z + widthOffset);
            AreasPoints.Add(pointFront);
            AreasPoints.Add(pointBack);
            AreasPoints.Add(pointRight);
            AreasPoints.Add(pointLeft);
        }
    }
}