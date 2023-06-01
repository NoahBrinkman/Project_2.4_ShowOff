using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class LeaderboardName : MonoBehaviour
{
    [SerializeField] private float shakePower;
    [SerializeField] private float shakeTime;
    [SerializeField] private float scale;
    [SerializeField] private List<TMP_Text> letters;
    [SerializeField] private List<Image> arrows;
    private int _usedAlphabetLetter;
    private int _activeLetter;

    private readonly List<char> _alphabet = new List<char>()
    {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
        'W', 'X', 'Y', 'Z'
    };

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _usedAlphabetLetter = 0;
            if (_activeLetter == letters.Count - 1)
            {
                _activeLetter = 0;
            }
            else
            {
                _activeLetter++;
            }
        }

        for (int i = 0; i < letters.Count; i++)
        {
            if (i == _activeLetter)
            {
                letters[_activeLetter].rectTransform.DOScale(scale, 0.2f);
            }
            else
            {
                letters[i].rectTransform.DOScale(1, 0.2f);
            }
        }

        for (int i = 0; i < arrows.Count; i++)
        {
            if (i == _activeLetter * 2 + 1 || i == _activeLetter * 2)
            {
                arrows[i].enabled = true;
            }
            else
            {
                arrows[i].enabled = false;
            }
        }

        ChangeLetters(_activeLetter);
    }

    private void ChangeLetters(int activeLetter)
    {
        
        if (Input.GetKeyUp(KeyCode.E))
        {
            StartColorChange(arrows[activeLetter*2 + 1]);    
            if (_usedAlphabetLetter == _alphabet.Count - 1)
            {
                _usedAlphabetLetter = 0;
            }
            else
            {
                _usedAlphabetLetter++;
            }

            LetterFadeIn(_alphabet[_usedAlphabetLetter], activeLetter);
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            StartColorChange(arrows[activeLetter*2]);    
            if (_usedAlphabetLetter == 0)
            {
                _usedAlphabetLetter = _alphabet.Count - 1;
            }
            else
            {
                _usedAlphabetLetter--;
            }

            LetterFadeIn(_alphabet[_usedAlphabetLetter], activeLetter);
        }
    }

    private void LetterFadeIn(char letterValue, int usedLetter)
    {
        letters[usedLetter].rectTransform.DOShakePosition(shakeTime, new Vector3(shakePower, 0));

        letters[usedLetter].text = letterValue.ToString();
    }

    private void StartColorChange(Image image)
    {
        StartCoroutine(ColorChangeCoroutine(image));
    }

    private IEnumerator ColorChangeCoroutine(Image image)
    {
        image.color = Color.green;

        yield return new WaitForSeconds(0.2f);

        image.color = Color.white;
    }
}