using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : SingleTon<ScoreManager>
{
    private List<PlayerStateMachine> _players;
    public float Score { get; private set; }
    [SerializeField] private float _baseScoreModifier = 1.15f;
    
    public void AddPlayer(PlayerStateMachine p)
    {
        if(_players.Contains(p)) return;
        _players.Add(p);
    }

    public override void Awake()
    {
        destroyOnLoad = false;
        base.Awake();
        Score = 0;
        _players = new List<PlayerStateMachine>();
    }


    private void FixedUpdate()
    {
        for (int i = 0; i < _players.Count; i++)
        {
            if(_players[i].enabled) Score += _players[i].GetScore() * _baseScoreModifier;
        }
    }
}
