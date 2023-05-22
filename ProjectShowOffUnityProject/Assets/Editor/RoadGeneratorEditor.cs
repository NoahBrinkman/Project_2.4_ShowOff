using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoadGenerator))]
public class RoadGeneratorEditor : Editor
{
    private RoadGenerator _generator;
    private bool _showBorder;

    private void OnEnable()
    {
        _generator = (RoadGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Always put the crossroads at the end and specify how many of them are there!\n" +
                                "Always put the straight road first!", MessageType.Warning);
        base.OnInspectorGUI();
        EditorGUILayout.HelpBox("Generator for the infinite road. Remember to add the road pieces to the list.",
            MessageType.Info);
        EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
        _showBorder = GUILayout.Toggle(_showBorder, "Show border");
    }

    private void OnSceneGUI()
    {
        if (_showBorder)
        {
            Handles.color = Color.red;
            Handles.DrawWireCube(_generator.transform.position, new Vector3(_generator.BorderSize, 1, _generator.BorderSize));
        }
    }
}