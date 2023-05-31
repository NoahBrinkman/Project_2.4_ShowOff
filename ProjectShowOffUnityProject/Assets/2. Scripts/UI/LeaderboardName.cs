using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class LeaderboardName : MonoBehaviour
{
    [SerializeField] private float fadeOutTime = 0f;
    [SerializeField] private TMP_Text letter;

    public void LetterFadeIn()
    {
        //letter.alpha = 0f;
       // letter.rectTransform.DOJump(letter.transform.position, 2f, 1, 0.5f);
        letter.rectTransform.DOShakePosition(0.5f, new Vector3(2f, 0, 0));
        letter.text = "B";
        //letter.DOFade(1, fadeOutTime);
    }
}
