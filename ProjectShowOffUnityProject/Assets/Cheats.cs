using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            PlayerStateMachine[] p = FindObjectsOfType<PlayerStateMachine>();
            for (int i = 0; i < p.Length; i++)
            {
                p[i].SubtractLife(-1000000);
            }
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            PlayerStateMachine[] p = FindObjectsOfType<PlayerStateMachine>();
            for (int i = 0; i < p.Length; i++)
            {
                p[i].GodMode =  !p[i].GodMode;
                Debug.LogWarning($"Godmode: {p[i].GodMode}");
            }
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            PlayerStateMachine[] p = FindObjectsOfType<PlayerStateMachine>();
            if (p.Length > 0)
            {
                p[0].SubtractLife(p[0].Lives);
            }
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            PlayerStateMachine[] p = FindObjectsOfType<PlayerStateMachine>();
            if (p.Length > 1)
            {
                p[1].SubtractLife(p[1].Lives);
            }
        }
    }
}
