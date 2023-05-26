using LevelGeneration;
using UnityEditor;
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
        }

        private void OnSceneGUI()
        {
            Handles.color = Color.cyan;
            foreach (Vector3 point in _obstacles.AreasPoints)
            {
                Handles.DrawSolidDisc(point,Vector3.up,0.05f);
            }
        }
    }
}
