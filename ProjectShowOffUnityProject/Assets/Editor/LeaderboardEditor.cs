using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LeaderboardName))]
public class LeaderboardEditor : Editor
{
    private LeaderboardName _leaderboard;
    private bool _showHiddenFields;

    private void OnEnable()
    {
        _leaderboard = (LeaderboardName)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This is your leaderboard controller.\n" +
                                "Remember to add the arrows (sprites) to the list, together with the letters and the score field!",
            MessageType.Info);
        base.OnInspectorGUI();
        
    }
    private void ShowDebugFields()
    {
        _showHiddenFields = !_showHiddenFields;
    }
}
