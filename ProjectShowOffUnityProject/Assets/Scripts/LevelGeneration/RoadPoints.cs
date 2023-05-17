using UnityEngine;
using UnityEngine.Serialization;

namespace LevelGeneration
{
    [RequireComponent(typeof(Renderer), typeof(BoxCollider))]
    public class RoadPoints : MonoBehaviour
    {
        [SerializeField] private int type;
        [SerializeField] private int width;
        [SerializeField] private int length;

        public int Type => type;
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
            int roadRotation = ((int)transform.localRotation.eulerAngles.y + 360 ) % 360;
            Debug.Log($"Road rotation: {roadRotation}");

            if (type == 1)
            {
                if (roadRotation == 0)
                {
                    SpawnPoints(-_bounds.size.x / 2, 0,
                        _bounds.size.x / 2, 0);
                }
                else if (roadRotation == 90)
                {
                    SpawnPoints(0, _bounds.size.z / 2,
                        0, -_bounds.size.z / 2);
                }
                else if (roadRotation == 180)
                {
                    SpawnPoints(_bounds.size.x / 2, 0,
                        -_bounds.size.x / 2, 0);
                }
                else if (roadRotation == 270)
                {
                    SpawnPoints(0, -_bounds.size.z / 2,
                        0, _bounds.size.z / 2);
                }
            }
            else if (type == 2)
            {
                if (roadRotation == 0)
                {
                    SpawnPoints(-_bounds.size.x / 2, _bounds.size.z / 2 - _bounds.size.z / (2 * length),
                        _bounds.size.x / 2 - _bounds.size.x / (2 * width), -_bounds.size.z / 2);
                }
                else if (roadRotation == 90)
                {
                    SpawnPoints(_bounds.size.x / 2 - _bounds.size.x / (2 * width), _bounds.size.z / 2,
                        -_bounds.size.x / 2, -_bounds.size.z / 2 + _bounds.size.z / (2 * length));
                }
                else if (roadRotation == 180)
                {
                    SpawnPoints(_bounds.size.x / 2, -_bounds.size.z / 2 + _bounds.size.z / (2 * length),
                        -_bounds.size.x / 2 + _bounds.size.x / (2 * width), _bounds.size.z / 2);
                }
                else if (roadRotation == 270)
                {
                    SpawnPoints(-_bounds.size.x / 2 + _bounds.size.x / (2 * width), -_bounds.size.z / 2,
                        _bounds.size.x / 2, _bounds.size.z / 2 - _bounds.size.z / (2 * length));
                }
            }
            else if (type == 3)
            {
                if (roadRotation == 0)
                {
                    SpawnPoints(-_bounds.size.x / 2, -_bounds.size.z / 2 + _bounds.size.z / (2 * length),
                        _bounds.size.x / 2 - _bounds.size.x / (2 * width), _bounds.size.z / 2);
                }
                else if (roadRotation == 90)
                {
                    SpawnPoints(-_bounds.size.x / 2 + _bounds.size.x / (2 * width), _bounds.size.z / 2,
                        _bounds.size.x / 2, -_bounds.size.z / 2 + _bounds.size.z / (2 * length));
                }
                else if (roadRotation == 180)
                {
                    SpawnPoints(_bounds.size.x / 2, _bounds.size.z / 2 - _bounds.size.z / (2 * length),
                        -_bounds.size.x / 2 + _bounds.size.x / (2 * width), -_bounds.size.z / 2);
                }
                else if (roadRotation == 270)
                {
                    SpawnPoints(_bounds.size.x / 2 - _bounds.size.x / (2 * width), -_bounds.size.z / 2,
                        -_bounds.size.x / 2, _bounds.size.z / 2 - _bounds.size.z / (2 * length));
                }
            }
            else if (type == 4)
            {
                if (roadRotation == 0)
                {
                    SpawnPoints(-_bounds.size.x / 2, 0,
                        _bounds.size.x / 2 - _bounds.size.x / (2 * width), -_bounds.size.z / 2,
                        true, _bounds.size.x / 2 - _bounds.size.x / (2 * width), _bounds.size.z / 2);
                }
                else if (roadRotation == 90)
                {
                    SpawnPoints(0, _bounds.size.z / 2,
                        -_bounds.size.x / 2, -_bounds.size.z / 2 + _bounds.size.z / (2 * length),
                        true, _bounds.size.x / 2, -_bounds.size.z / 2 + _bounds.size.z / (2 * length));
                }
                else if (roadRotation == 180)
                {
                    SpawnPoints(_bounds.size.x / 2, 0,
                        -_bounds.size.x / 2 + _bounds.size.x / (2 * width), _bounds.size.z / 2,
                        true, -_bounds.size.x / 2 + _bounds.size.x / (2 * width), -_bounds.size.z / 2);
                }
                else if (roadRotation == 270)
                {
                    SpawnPoints(0, -_bounds.size.z / 2,
                        _bounds.size.x / 2, _bounds.size.z / 2 - _bounds.size.z / (2 * length),
                        true, -_bounds.size.x / 2, _bounds.size.z / 2 - _bounds.size.z / (2 * length));
                }
            }
        }

        private void SpawnPoints(float xOffsetStart, float zOffsetStart, float xOffsetEnd, float zOffsetEnd,
            bool isCrossroad = false, float xOffsetLeft = 0, float zOffsetLeft = 0)
        {
            _assetStart = new Vector3(_bounds.center.x + xOffsetStart, transform.position.y,
                _bounds.center.z + zOffsetStart); 
            _assetEnd = new Vector3(_bounds.center.x + xOffsetEnd, transform.position.y,
                _bounds.center.z + zOffsetEnd);
            Debug.Log("START" + _assetStart);
            Debug.Log("END" + _assetEnd);

            if (isCrossroad)
            {
                _assetLeft = new Vector3(_bounds.center.x + xOffsetLeft,  transform.position.y,
                    _bounds.center.z + zOffsetLeft);
            }
        }
    }
}