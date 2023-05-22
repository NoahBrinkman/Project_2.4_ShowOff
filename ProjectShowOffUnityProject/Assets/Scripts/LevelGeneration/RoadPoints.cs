using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelGeneration
{
    [RequireComponent(typeof(Renderer), typeof(BoxCollider))]
    public class RoadPoints : MonoBehaviour
    {
        public enum RoadType
        {
            Straight = 1,
            Right = 2,
            Left = 3,
            Crossroad = 4
        };

        [SerializeField] private RoadType roadType;
        [SerializeField] private int width = 8;
        [SerializeField] private int length = 8;


        public int Width => width;
        public int Length => length;
        public RoadType TypeOfRoad => roadType;

        public List<Vector3> CurvePoints = new List<Vector3>();

        private Vector3 _assetStart;
        private Vector3 _assetEnd;
        private Vector3 _assetLeft;
        private Bounds _bounds;

        //Used in the editor
        public Vector3 AssetStart => _assetStart;

        public Vector3 AssetEnd => _assetEnd;

        public Vector3 AssetLeft => _assetLeft;


        private void Awake()
        {
            _bounds = GetComponent<Renderer>().bounds;
        }

        private void OnEnable()
        {
            GeneratePoints();
            
            for (int i = 0; i <= 10; i++)
            {
                float t = i / (float)10;
                CurvePoints.Add(CalculateBezierPoint(t, _assetStart, _assetLeft,_assetEnd));
            }
        }

        /// <summary>
        /// Method used to generate points on correct positions
        /// </summary>
        /// <param name="xOffsetStart"> Offset of X position of the start point</param>
        /// <param name="zOffsetStart"> Offset of Z position of the start point</param>
        /// <param name="xOffsetEnd">   Offset of X position of the end point</param>
        /// <param name="zOffsetEnd">   Offset of Z position of the end point</param>
        /// <param name="isCrossroad">  Bool to check if the road is a crossroad</param>
        /// <param name="xOffsetLeft">  Offset of X position of the left point</param>
        /// <param name="zOffsetLeft">  Offset of Z position of the left point</param>
        private void SpawnPoints(float xOffsetStart, float zOffsetStart, float xOffsetEnd, float zOffsetEnd,
            bool isCrossroad = false, float xOffsetLeft = 0, float zOffsetLeft = 0)
        {
            float height = transform.position.y;
            _assetStart = new Vector3(_bounds.center.x + xOffsetStart, height,
                _bounds.center.z + zOffsetStart);
            _assetEnd = new Vector3(_bounds.center.x + xOffsetEnd, height,
                _bounds.center.z + zOffsetEnd);
            // Debug.Log("START" + _assetStart);
            // Debug.Log("END" + _assetEnd);

            if (isCrossroad)
            {
                _assetLeft = new Vector3(_bounds.center.x + xOffsetLeft, height,
                    _bounds.center.z + zOffsetLeft);
            }
        }

        /// <summary>
        /// Method used to calculate all the points' positions
        /// </summary>
        private void GeneratePoints()
        {
            int roadRotation = ((int)transform.localRotation.eulerAngles.y + 360) % 360;

            float simpleX = _bounds.size.x / 2;
            float simpleZ = _bounds.size.z / 2;
            float lengthZ = _bounds.size.z / (2 * length);
            float widthX = _bounds.size.x / (2 * width);
            float xMinusWidthX = simpleX - widthX;
            float xPlusWidthX = -simpleX + widthX;
            float zMinusLengthZ = simpleZ - lengthZ;
            float zPlusLengthZ = -simpleZ + lengthZ;
            switch (roadType)
            {
                case RoadType.Straight when roadRotation == 0:
                    SpawnPoints(-simpleX, 0, simpleX, 0);
                    break;
                case RoadType.Straight when roadRotation == 90:
                    SpawnPoints(0, simpleZ, 0, -simpleZ);
                    break;
                case RoadType.Straight when roadRotation == 180:
                    SpawnPoints(simpleX, 0, -simpleX, 0);
                    break;
                case RoadType.Straight when roadRotation == 270:
                    SpawnPoints(0, -simpleZ, 0, simpleZ);
                    break;
                //RIGHT-------------------------------------------------------------------------------------------------
                case RoadType.Right when roadRotation == 0:
                    SpawnPoints(-simpleX, zMinusLengthZ, xMinusWidthX, -simpleZ,
                        true, xMinusWidthX, zMinusLengthZ);
                    break;
                case RoadType.Right when roadRotation == 90:
                    SpawnPoints(xMinusWidthX, simpleZ, -simpleX, zPlusLengthZ,
                        true, xMinusWidthX,zPlusLengthZ);
                    break;
                case RoadType.Right when roadRotation == 180:
                    SpawnPoints(simpleX, zPlusLengthZ, xPlusWidthX, simpleZ,
                        true, xPlusWidthX,zPlusLengthZ);
                    break;
                case RoadType.Right when roadRotation == 270:
                    SpawnPoints(xPlusWidthX, -simpleZ, simpleX, zMinusLengthZ,
                        true, xPlusWidthX,zMinusLengthZ);
                    break;
                //LEFT--------------------------------------------------------------------------------------------------
                case RoadType.Left when roadRotation == 0:
                    SpawnPoints(-simpleX, zPlusLengthZ, xMinusWidthX, simpleZ,
                        true, xMinusWidthX,zPlusLengthZ);
                    break;
                case RoadType.Left when roadRotation == 90:
                    SpawnPoints(xPlusWidthX, simpleZ, simpleX, zPlusLengthZ,
                        true, xPlusWidthX,zPlusLengthZ);
                    break;
                case RoadType.Left when roadRotation == 180:
                    SpawnPoints(simpleX, zMinusLengthZ, xPlusWidthX, -simpleZ,
                        true,xPlusWidthX, zMinusLengthZ);
                    break;
                case RoadType.Left when roadRotation == 270:
                    SpawnPoints(xMinusWidthX, -simpleZ, -simpleX, zMinusLengthZ,
                        true, xMinusWidthX, zMinusLengthZ);
                    break;
                //CROSSROAD---------------------------------------------------------------------------------------------
                case RoadType.Crossroad when roadRotation == 0:
                    SpawnPoints(-simpleX, 0, xMinusWidthX, -simpleZ,
                        true, xMinusWidthX, simpleZ);
                    break;
                case RoadType.Crossroad when roadRotation == 90:
                    SpawnPoints(0, simpleZ, -simpleX, zPlusLengthZ,
                        true, simpleX, zPlusLengthZ);
                    break;
                case RoadType.Crossroad when roadRotation == 180:
                    SpawnPoints(simpleX, 0, xPlusWidthX, simpleZ,
                        true, xPlusWidthX, -simpleZ);
                    break;
                case RoadType.Crossroad when roadRotation == 270:
                    SpawnPoints(0, -simpleZ, simpleX, zMinusLengthZ,
                        true, -simpleX, zMinusLengthZ);
                    break;
            }
        }
        
        private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            Vector3 point = uu * p0 + 2 * u * t * p1 + tt * p2;
            return point;
        }
    }
}