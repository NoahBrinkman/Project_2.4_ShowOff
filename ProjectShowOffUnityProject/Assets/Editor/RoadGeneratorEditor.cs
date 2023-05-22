using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoadGenerator))]
public class RoadGeneratorEditor : Editor
{
    private RoadGenerator _generator;
    private void OnEnable()
    {
        _generator = (RoadGenerator)target;
    }
    
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Always put the crossroads at the end and specify how many of them are there!\n" +
                                "Always put the straight road first!", MessageType.Warning);
        base.OnInspectorGUI();
        EditorGUILayout.HelpBox("Generator for the infinite road. Remember to add the road pieces to the list.", MessageType.Info);
    }
    
}
