using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : SingleTon<ScoreManager>
{
    private List<PlayerStateMachine> _players;
    public float Score { get; private set; }
    [SerializeField] private float _baseScoreModifier = 1.15f;
    public override void Awake()
    {
        destroyOnLoad = false;
        base.Awake();
        Score = 0;
    }



    public void AddScore(PlayerStateMachine p)
    {
        if (p.enabled) Score += p.GetScore() * _baseScoreModifier;
    }
}
