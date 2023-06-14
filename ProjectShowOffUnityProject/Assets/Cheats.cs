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
    }
}
