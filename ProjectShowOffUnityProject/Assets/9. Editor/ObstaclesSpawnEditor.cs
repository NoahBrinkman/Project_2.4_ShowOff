using LevelGeneration;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ObstaclesSpawn))]
    public class ObstaclesSpawnEditor : UnityEditor.Editor
    {
        private ObstaclesSpawn _obstacles;

        private void OnEnable()
        {
            _obstacles = (ObstaclesSpawn)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Obstacle spawner.\n" +
                                    "Remember to check if all the measurements are correct and that your obstacles have assigned type!",
                MessageType.Info);
            base.OnInspectorGUI();

            if (GUILayout.Button("Control Spawning"))
            {
                ControlledSpawn();
            }

            if (_obstacles.ControlledSpawn)
            {
                EditorGUILayout.HelpBox("If you see this message, you are controlling the spawn!\n" +
                                        "If you want to go back to randomised one, click the button",
                    MessageType.Warning);
                EditorGUI.indentLevel++;
                _obstacles.IsAreaOne = EditorGUILayout.Toggle("Use Area1", _obstacles.IsAreaOne);
                _obstacles.IsAreaTwo = EditorGUILayout.Toggle("Use Area2", _obstacles.IsAreaTwo);
                _obstacles.IsAreaThree = EditorGUILayout.Toggle("Use Area3", _obstacles.IsAreaThree);
                EditorGUI.indentLevel--;
            }
            else
            {
                _obstacles.IsAreaOne = false;
                _obstacles.IsAreaTwo = false;
                _obstacles.IsAreaThree = false;
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_obstacles);
                EditorSceneManager.MarkSceneDirty(_obstacles.gameObject.scene);
            }
        }

        private void OnSceneGUI()
        {
            Handles.color = Color.cyan;
            foreach (Vector3 point in _obstacles.AreasPoints)
            {
                Handles.DrawSolidDisc(point, Vector3.up, 0.05f);
            }
        }

        private void ControlledSpawn()
        {
            _obstacles.ControlledSpawn = !_obstacles.ControlledSpawn;
        }
    }
}