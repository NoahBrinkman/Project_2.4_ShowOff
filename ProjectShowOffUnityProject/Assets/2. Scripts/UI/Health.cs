using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Health : MonoBehaviour
{
    [SerializeField] private StateMachine<PlayerState> player1;
    [SerializeField] private StateMachine<PlayerState> player2;
    [SerializeField] private TMP_Text player1HealthText;
    [SerializeField] private TMP_Text player2HealthText;
    

    private void Update()
    {
        if(player1 != null)        player1HealthText.text = (player1.CurrentState.StateMachine.GodMode ? "∞" : player1.CurrentState.StateMachine.Lives)+ "/" + player1.CurrentState.StateMachine.MaxLives;
        if(player2 != null) player2HealthText.text = (player2.CurrentState.StateMachine.GodMode ? "∞" : player2.CurrentState.StateMachine.Lives)+ "/"+ player1.CurrentState.StateMachine.MaxLives;
    }
}
