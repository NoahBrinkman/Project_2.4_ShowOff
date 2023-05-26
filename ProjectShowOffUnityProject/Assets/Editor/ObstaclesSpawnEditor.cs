using System.Collections;
using System.Collections.Generic;
using LevelGeneration;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObstaclesSpawn))]
public class ObstaclesSpawnEditor : Editor
{
    private ObstaclesSpawn _obstacles;
    private void OnEnable()
    {
        _obstacles = (ObstaclesSpawn)target;
    }

    private void OnSceneGUI()
    {
        Handles.color = Color.red;

        for (int i = 2; i <= 6; i += 2)
        {
            Handles.DrawWireCube(new Vector3(_obstacles.transform.position.x+i,
                    _obstacles.GetComponent<Renderer>().bounds.size.y+0.5f/2.0f,
                    _obstacles.transform.position.z), 
                new Vector3(1,0.5f,
                    _obstacles.GetComponent<Renderer>().bounds.size.z));
        }

        Handles.color = Color.cyan;
        foreach (Vector3 point in _obstacles._areasPoints)
        {
            Handles.DrawSolidDisc(point,Vector3.up,0.05f);
        }
        
        // Handles.color = Color.cyan;
        // for (int i = 0; i <= 4; i += 2)
        // {
        //     Handles.DrawSolidDisc(new Vector3(_obstacles.transform.position.x + 1.5f + i,
        //         _obstacles.transform.position.y,
        //         _obstacles.transform.position.z), Vector3.up, 0.05f);
        // }
        // Handles.color = Color.green;
        // for (int i = 0; i <= 4; i += 2)
        // {
        //     Handles.DrawSolidDisc(new Vector3(_obstacles.transform.position.x + 2.5f + i,
        //         _obstacles.transform.position.y,
        //         _obstacles.transform.position.z), Vector3.up, 0.05f);
        // }
        // Handles.color = Color.yellow;
        // for (int i = 0; i <= 4; i += 2)
        // {
        //     Handles.DrawSolidDisc(new Vector3(_obstacles.transform.position.x + 2.0f + i,
        //         _obstacles.transform.position.y,
        //         _obstacles.transform.position.z-_obstacles.GetComponent<Renderer>().bounds.size.z/2), Vector3.up, 0.05f);
        //     Handles.DrawSolidDisc(new Vector3(_obstacles.transform.position.x + 2.0f + i,
        //         _obstacles.transform.position.y,
        //         _obstacles.transform.position.z+_obstacles.GetComponent<Renderer>().bounds.size.z/2), Vector3.up, 0.05f);
        // }
    }
}
