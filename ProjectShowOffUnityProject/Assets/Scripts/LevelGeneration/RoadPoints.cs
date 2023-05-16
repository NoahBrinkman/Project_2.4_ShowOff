using UnityEngine;

namespace LevelGeneration
{
    [RequireComponent(typeof(Renderer))]
    public class RoadPoints : MonoBehaviour
    {
        [SerializeField] private GameObject startPoint;
        [SerializeField] private GameObject endPoint;
        [SerializeField] private GameObject leftPoint;
        [SerializeField] private int direction;
        [SerializeField] private int width;
        [SerializeField] private int length;

        public int Direction => direction;
        public int Width => width;
        public int Length => length;

        [SerializeField] private Vector3 _assetStart;
        [SerializeField] private Vector3 _assetEnd;
        private Vector3 _assetLeft;
        private Bounds _bounds;

        public Vector3 AssetStart
        {
            get => _assetStart;
            set => _assetStart = value;
        }

        public Vector3 AssetEnd
        {
            get => _assetEnd;
            set => _assetEnd = value;
        }

        public Vector3 AssetLeft
        {
            get => _assetLeft;
            set => _assetLeft = value;
        }


        private void Awake()
        {
            _bounds = GetComponent<Renderer>().bounds;
        }

        private void OnEnable()
        {
            float roadRotation = transform.localRotation.eulerAngles.y;

            if (direction == 1)
        {
            if (roadRotation == 0)
            {
                SpawnPoints(-_bounds.size.x/2, 0,
                    _bounds.size.x/2, 0);
            }
            else if (roadRotation == 90)
            {
                SpawnPoints(0,-_bounds.size.z/2,
                    0,_bounds.size.z/2);
            }
            else if (roadRotation == 180)
            {
                SpawnPoints(_bounds.size.x/2, 0,
                    -_bounds.size.x/2, 0);
            }
            else if (roadRotation == 270)
            {
                SpawnPoints(0,_bounds.size.z/2,
                    0,-_bounds.size.z/2);
            }
        }
        else if (direction == 2)
        {
            if (roadRotation == 0)
            {
                SpawnPoints(-_bounds.size.x/2,_bounds.size.z/2 - _bounds.size.z/ (2 * length),
                    _bounds.size.x/2 - _bounds.size.x/ (2*width),-_bounds.size.z/2);
            }
            else if (roadRotation == 90)
            {
                SpawnPoints(_bounds.size.x / 2 - _bounds.size.x / (2 * width),_bounds.size.z / 2,
                    -_bounds.size.x / 2, -_bounds.size.z / 2 + _bounds.size.z / (2*length));
            }
            else if (roadRotation == 180)
            {
                SpawnPoints(_bounds.size.x/2,-_bounds.size.z/2 + _bounds.size.z / (2 * length),
                    -_bounds.size.x/2 + _bounds.size.x/ (2 * width),_bounds.size.z/2);
            }
            else if (roadRotation == 270)
            {
                SpawnPoints(-_bounds.size.x / 2+ _bounds.size.x/ (2 * width), -_bounds.size.z / 2,
                    _bounds.size.x / 2,_bounds.size.z / 2 - _bounds.size.z / (2*length));
                
            }
        }
        else if (direction == 3)
        {
            if (roadRotation == 0)
            {
                SpawnPoints(-_bounds.size.x / 2, -_bounds.size.z/2 + _bounds.size.z / (2 * length),
                    _bounds.size.x / 2- _bounds.size.x / (2*width), _bounds.size.z / 2);
            }
            else if (roadRotation == 90)
            {
                SpawnPoints(-_bounds.size.x/2 + _bounds.size.x/(2*width), _bounds.size.z / 2,
                    _bounds.size.x / 2, -_bounds.size.z / 2 + _bounds.size.z / (2*length));
            }
            else if (roadRotation == 180)
            {
                SpawnPoints(_bounds.size.x / 2, _bounds.size.z/2 - _bounds.size.z / (2 * length),
                    -_bounds.size.x / 2 +_bounds.size.x / (2*width), -_bounds.size.z / 2);
            }
            else if (roadRotation == 270)
            {
                SpawnPoints(_bounds.size.x / 2-_bounds.size.x/(2*width), -_bounds.size.z / 2,
                    -_bounds.size.x / 2 , _bounds.size.z / 2 - _bounds.size.z / (2*length));
            }
        }
        else if (direction == 4)
        {
            if (roadRotation == 0)
            {
                SpawnPoints(-_bounds.size.x / 2, 0,
                    _bounds.size.x / 2 - _bounds.size.x / (2*width), -_bounds.size.z / 2,
                    true, _bounds.size.x / 2 - _bounds.size.x / (2*width),_bounds.size.z / 2);
                
            }
            else if (roadRotation == 90)
            {
                SpawnPoints(0,_bounds.size.z / 2,
                    -_bounds.size.x / 2, -_bounds.size.z / 2 + _bounds.size.z / (2*length),
                    true, _bounds.size.x / 2, -_bounds.size.z / 2 + _bounds.size.z / (2*length));
            }
            else if (roadRotation == 180)
            {
                SpawnPoints(_bounds.size.x / 2,0,
                    -_bounds.size.x / 2 + _bounds.size.x / (2*width),_bounds.size.z / 2,
                    true, -_bounds.size.x / 2 + _bounds.size.x / (2*width),-_bounds.size.z / 2);
                
            }
            else if (roadRotation == 270)
            {
                SpawnPoints(0, -_bounds.size.z/2,
                    _bounds.size.x / 2, _bounds.size.z / 2 - _bounds.size.z / (2*length),
                    true, -_bounds.size.x / 2, _bounds.size.z / 2 - _bounds.size.z / (2*length));
                
            }
        }

            GameObject point1 = Instantiate(startPoint, _assetStart, transform.rotation);
            point1.name = "StartPoint";
            point1.transform.parent = transform;

            GameObject point2 = Instantiate(endPoint, _assetEnd, transform.rotation);
            point2.name = "EndPoint";
            point2.transform.parent = transform;

            if (direction == 4)
            {
                GameObject point3 = Instantiate(leftPoint, _assetLeft, transform.rotation);
                point3.name = "LeftPoint";
                point3.transform.parent = transform;
            }
        }

        private void SpawnPoints(float xOffsetStart, float zOffsetStart, float xOffsetEnd, float zOffsetEnd,
            bool isCrossroad = false, float xOffsetLeft = 0, float zOffsetLeft = 0)
        {
            _assetStart = new Vector3(_bounds.center.x + xOffsetStart, _bounds.center.y - _bounds.size.y / 2,
                _bounds.center.z + zOffsetStart);
            _assetEnd = new Vector3(_bounds.center.x + xOffsetEnd, _bounds.center.y - _bounds.size.y / 2,
                _bounds.center.z + zOffsetEnd);
            Debug.Log("START" + _assetStart);
            Debug.Log("END" + _assetEnd);

            if (isCrossroad)
            {
                _assetLeft = new Vector3(_bounds.center.x + xOffsetLeft, _bounds.center.y - _bounds.size.y / 2,
                    _bounds.center.z + zOffsetLeft);
            }
        }
    }
}