using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreText : MonoBehaviour
{
    private TMP_Text _text;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        int score = Mathf.RoundToInt(ScoreManager.Instance.Score);
        _text.text = $"{score:n0}";;
    }
}
