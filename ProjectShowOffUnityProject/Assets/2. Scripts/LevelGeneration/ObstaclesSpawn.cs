using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class ObstaclesSpawn : MonoBehaviour
{
    [HideInInspector] public List<Vector3> AreasPoints = new List<Vector3>();

    [Tooltip("List of obstacles that have a chance to get spawned on the road")] [SerializeField]
    private List<Obstacle> obstaclesToAvoid = new List<Obstacle>();

    [Tooltip("Offset of height - used to generate sliding obstacles")] [SerializeField]
    private float heightOffset;

    [Tooltip("Space from the start and between the areas")] [SerializeField]
    private float spacing = 2.0f;

    private Renderer _roadRenderer;
    private Obstacle.WhichObstacle _chosenObstacle;
    [HideInInspector] [SerializeField] private bool controlledSpawn;
    [HideInInspector] [SerializeField] private bool isAreaOne;
    [HideInInspector] [SerializeField] private bool isAreaTwo;
    [HideInInspector] [SerializeField] private bool isAreaThree;
    [HideInInspector] [SerializeField] private bool controlAreaOne;
    [HideInInspector] [SerializeField] private bool controlAreaTwo;
    [HideInInspector] [SerializeField] private bool controlAreaThree;

    [HideInInspector] [SerializeField] private GameObject area1Object;
    [HideInInspector] [SerializeField] private GameObject area2Object;
    [HideInInspector] [SerializeField] private GameObject area3Object;

    public bool ControlledSpawn
    {
        get => controlledSpawn;
        set => controlledSpawn = value;
    }

    public bool ControlAreaOne
    {
        get => controlAreaOne;
        set => controlAreaOne = value;
    }

    public bool ControlAreaTwo
    {
        get => controlAreaTwo;
        set => controlAreaTwo = value;
    }

    public bool ControlAreaThree
    {
        get => controlAreaThree;
        set => controlAreaThree = value;
    }

    public bool IsAreaOne
    {
        get => isAreaOne;
        set => isAreaOne = value;
    }

    public bool IsAreaTwo
    {
        get => isAreaTwo;
        set => isAreaTwo = value;
    }

    public bool IsAreaThree
    {
        get => isAreaThree;
        set => isAreaThree = value;
    }

    public GameObject Area1Object
    {
        get => area1Object;
        set => area1Object = value;
    }

    public GameObject Area2Object
    {
        get => area2Object;
        set => area2Object = value;
    }

    public GameObject Area3Object
    {
        get => area3Object;
        set => area3Object = value;
    }

    private void OnEnable()
    {
        _roadRenderer = GetComponent<Renderer>();
        float positionX = transform.position.x;
        float positionY = _roadRenderer.bounds.max.y;
        float positionZ = transform.position.z;

        int roadRotation = ((int)transform.localRotation.eulerAngles.y + 360) % 360;

        if (roadRotation == 0 || roadRotation == 180)
        {
            GeneratePointsVertical(positionX, positionY, positionZ, 0);
            GeneratePointsVertical(positionX, positionY, positionZ, -spacing);
            GeneratePointsVertical(positionX, positionY, positionZ, spacing);
        }
        else if (roadRotation == 90 || roadRotation == 270)
        {
            GeneratePointsHorizontal(positionX, positionY, positionZ, 0);
            GeneratePointsHorizontal(positionX, positionY, positionZ, -spacing);
            GeneratePointsHorizontal(positionX, positionY, positionZ, spacing);
        }


        int random = Random.Range(1, 31) % 3 + 1;

        if (obstaclesToAvoid.Count != 0 && !controlledSpawn)
        {
            for (int i = 0; i < random; i++)
            {
                GameObject obstacle = Instantiate(obstaclesToAvoid[Random.Range(0, obstaclesToAvoid.Count)].gameObject,
                    transform);
                _chosenObstacle = obstacle.GetComponent<Obstacle>().ObstacleType;

                obstacle.transform.position = CalculateRandomPosition(roadRotation,
                    AreasPoints[4 * i], AreasPoints[4 * i + 1],
                    AreasPoints[4 * i + 2], AreasPoints[4 * i + 3]);
            }
        }
        else if (obstaclesToAvoid.Count != 0 && controlledSpawn)
        {
            if (isAreaOne)
            {
                Debug.Log("Area 1 is used!");
                GameObject obstacle;
                if (controlAreaOne && area1Object != null)
                {
                    obstacle = Instantiate(area1Object, transform);
                }
                else
                {
                    obstacle = Instantiate(obstaclesToAvoid[Random.Range(0, obstaclesToAvoid.Count)].gameObject,
                        transform);
                }

                _chosenObstacle = obstacle.GetComponent<Obstacle>().ObstacleType;
                obstacle.transform.position = CalculateRandomPosition(roadRotation, AreasPoints[0], AreasPoints[1],
                    AreasPoints[2], AreasPoints[3]);
            }

            if (isAreaTwo)
            {
                Debug.Log("Area 2 is used!");
                GameObject obstacle;
                if (controlAreaTwo && area2Object != null)
                {
                    obstacle = Instantiate(area2Object, transform);
                }
                else
                {
                    obstacle = Instantiate(obstaclesToAvoid[Random.Range(0, obstaclesToAvoid.Count)].gameObject,
                        transform);
                }

                _chosenObstacle = obstacle.GetComponent<Obstacle>().ObstacleType;
                obstacle.transform.position = CalculateRandomPosition(roadRotation, AreasPoints[4], AreasPoints[4 + 1],
                    AreasPoints[4 + 2], AreasPoints[4 + 3]);
            }

            if (isAreaThree)
            {
                Debug.Log("Area 3 is used!");
                GameObject obstacle;
                if (controlAreaThree && area3Object != null)
                {
                    obstacle = Instantiate(area3Object, transform);
                }
                else
                {
                    obstacle = Instantiate(obstaclesToAvoid[Random.Range(0, obstaclesToAvoid.Count)].gameObject,
                        transform);
                }

                _chosenObstacle = obstacle.GetComponent<Obstacle>().ObstacleType;
                obstacle.transform.position = CalculateRandomPosition(roadRotation, AreasPoints[8], AreasPoints[8 + 1],
                    AreasPoints[8 + 2], AreasPoints[8 + 3]);
            }
        }
    }

    private Vector3 CalculateRandomPosition(int roadRotation, Vector3 frontPoint, Vector3 backPoint, Vector3 leftPoint,
        Vector3 rightPoint)
    {
        float positionFront = 0;
        float positionBack = 0;
        float positionRight = 0;
        float positionLeft = 0;
        switch (roadRotation)
        {
            case 0 or 180:
                positionFront = frontPoint.x;
                positionBack = backPoint.x;
                positionRight = rightPoint.z;
                positionLeft = leftPoint.z;
                break;
            case 90 or 270:
                positionFront = frontPoint.z;
                positionBack = backPoint.z;
                positionRight = rightPoint.x;
                positionLeft = leftPoint.x;
                break;
        }

        Vector3 randomPosition = GetRandomVector3Position(roadRotation, positionFront,
            positionBack, positionRight, positionLeft, _chosenObstacle, AreasPoints[0].y);
        return randomPosition;
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

    private Vector3 GetRandomVector3Position(int rotation,
        float firstPosition, float lastPosition, float rightPosition, float leftPosition,
        Obstacle.WhichObstacle obstacleType, float y)
    {
        float newLengthPosition = 0;
        float newWidthPosition = 0;
        float newY = 0;
        Vector3 newPosition = Vector3.zero;

        switch (obstacleType)
        {
            case Obstacle.WhichObstacle.RunAround:
                newLengthPosition = Random.Range(firstPosition, lastPosition);
                newWidthPosition = Random.Range(rightPosition, leftPosition);
                newY = y;
                break;
            case Obstacle.WhichObstacle.Jump:
                newLengthPosition = Random.Range(firstPosition, lastPosition);
                newWidthPosition = (rightPosition + leftPosition) / 2;
                newY = y;
                break;
            case Obstacle.WhichObstacle.Slide:
                newLengthPosition = Random.Range(firstPosition, lastPosition);
                newWidthPosition = (rightPosition + leftPosition) / 2;
                newY = y + heightOffset;
                break;
        }

        switch (rotation)
        {
            case 0 or 180:
                newPosition = new Vector3(newLengthPosition, newY, newWidthPosition);
                break;
            case 90 or 270:
                newPosition = new Vector3(newWidthPosition, newY, newLengthPosition);
                break;
        }

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
    private void GeneratePointsHorizontal(float x, float y, float z, float modifier)
    {
        Vector3 pointFront =
            new Vector3(x, y, _roadRenderer.bounds.center.z - modifier - _roadRenderer.bounds.size.x / 4);
        Vector3 pointBack =
            new Vector3(x, y, _roadRenderer.bounds.center.z - modifier + _roadRenderer.bounds.size.x / 4);
        Vector3 pointRight =
            new Vector3(x - _roadRenderer.bounds.size.x / 4, y, _roadRenderer.bounds.center.z - modifier);
        Vector3 pointLeft =
            new Vector3(x + _roadRenderer.bounds.size.x / 4, y, _roadRenderer.bounds.center.z - modifier);
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
    private void GeneratePointsVertical(float x, float y, float z, float modifier)
    {
        Vector3 pointFront =
            new Vector3(_roadRenderer.bounds.center.x - modifier - _roadRenderer.bounds.size.z / 4, y, z);
        Vector3 pointBack =
            new Vector3(_roadRenderer.bounds.center.x - modifier + _roadRenderer.bounds.size.z / 4, y, z);
        Vector3 pointRight =
            new Vector3(_roadRenderer.bounds.center.x - modifier, y, z - _roadRenderer.bounds.size.z / 4);
        Vector3 pointLeft =
            new Vector3(_roadRenderer.bounds.center.x - modifier, y, z + _roadRenderer.bounds.size.z / 4);
        AreasPoints.Add(pointFront);
        AreasPoints.Add(pointBack);
        AreasPoints.Add(pointRight);
        AreasPoints.Add(pointLeft);
    }
}