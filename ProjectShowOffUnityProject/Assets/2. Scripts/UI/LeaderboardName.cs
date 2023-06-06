using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class LeaderboardName : MonoBehaviour
{
    [SerializeField] private float shakePower = 5f;
    [SerializeField] private float shakeTime = 0.5f;
    [SerializeField] private float scale = 1.5f;
    [SerializeField] private float timeOfColourChange = 0.2f;
    [SerializeField] private TMP_Text highScore;
    [SerializeField] private List<TMP_Text> letters;
    [SerializeField] private List<Image> arrows;
    [SerializeField] private bool horizontal;
    private string _verticalAxis = "Horizontal";
    private int _usedAlphabetLetter;
    private int _activeLetter;
    [SerializeField] private int _score;
    private readonly Dictionary<int, int> _lettersAndPosition = new Dictionary<int, int>();

    private string _fileName = "Leaderboard.txt";
    private string _folderPath;
    private string _filePath;
    bool hasSwitched = false;
    private List<HighScoreSave> sd = new List<HighScoreSave>();

    private readonly List<char> _alphabet = new List<char>()
    {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
        'W', 'X', 'Y', 'Z'
    };

    private void Start()
    {
        for (int i = 0; i < letters.Count; i++)
        {
            _lettersAndPosition.Add(i, 0);
        }

        _folderPath = Path.Combine(Application.dataPath, "9. Data");

        if (!Directory.Exists(_folderPath))
            Directory.CreateDirectory(_folderPath);

        _filePath = Path.Combine(_folderPath, _fileName);


        ReadData();
    }

    private void Update()
    {
        highScore.text = "Your score: " + _score;
        SwitchLetters();

        ChangeLetters(_activeLetter);
    }

    public void SaveData()
    {
        string newName = "";
        foreach (var letter in letters)
        {
            newName += letter.text;
        }

        sd.Add(new HighScoreSave(_score, newName));

        List<HighScoreSave> sortedList = new List<HighScoreSave>();
        sortedList = sd.OrderByDescending(o => o.HighScore).ToList();

        foreach (var sorted in sortedList)
        {
            Debug.Log(sorted);
        }

        List<string> textList = new List<string>();

        foreach (var score in sortedList)
        {
            textList.Add(score.ToString());
        }

        File.WriteAllLines(_filePath, textList);
    }

    private void ReadData()
    {
        sd.Clear();
        string[] scores = File.ReadAllLines(_filePath);

        for (int i = 0; i < 10; i++)
        {
            int newNumber = 0;
            string newName = "";
            string[] numbers = scores[i].Split(";");
            for (int j = 0; j < numbers.Length; j++)
            {
                if (j % 2 == 0)
                {
                    newNumber = int.Parse(numbers[j]);
                }
                else
                {
                    newName = numbers[j];
                }
            }

            sd.Add(new HighScoreSave(newNumber,newName));
        }
    }

    /// <summary>
    /// Switch Letter by pressing one button (SPACE), zoom the letter in and show the arrows
    /// </summary>
    private void SwitchLetters()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
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
    }

    /// <summary>
    /// Change letters on the active letter with two buttons (Q and E)
    /// </summary>
    /// <param name="activeLetter"> Which letter in the list is active</param>
    private void ChangeLetters(int activeLetter)
    {
        _usedAlphabetLetter = _lettersAndPosition[_activeLetter];
        float ver = Input.GetAxisRaw(_verticalAxis);
        if (ver > 0 && !hasSwitched)
        {
            hasSwitched = true;
            StartColorChange(arrows[activeLetter * 2]);
            if (_usedAlphabetLetter == _alphabet.Count - 1)
            {
                _usedAlphabetLetter = 0;
            }
            else
            {
                _usedAlphabetLetter++;
            }

            LetterShake(_alphabet[_usedAlphabetLetter], activeLetter);
        }
        else if (ver < 0 && !hasSwitched)
        {
            hasSwitched = true;
            StartColorChange(arrows[activeLetter * 2 + 1]);
            if (_usedAlphabetLetter == 0)
            {
                _usedAlphabetLetter = _alphabet.Count - 1;
            }
            else
            {
                _usedAlphabetLetter--;
            }

            LetterShake(_alphabet[_usedAlphabetLetter], activeLetter);
        } else if (ver == 0)
        {
            hasSwitched = false;
        }

        _lettersAndPosition[_activeLetter] = _usedAlphabetLetter;
    }

    /// <summary>
    /// Letter shake effect
    /// </summary>
    /// <param name="letterValue"> New value that the letter should have </param>
    /// <param name="usedLetter"> The letter in the row that we are changing</param>
    private void LetterShake(char letterValue, int usedLetter)
    {
        letters[usedLetter].rectTransform.DOShakePosition(shakeTime,
            horizontal ? new Vector3(shakePower, 0) : new Vector3(0, shakePower));

        letters[usedLetter].text = letterValue.ToString();
    }

    /// <summary>
    /// Method used to run the coroutine responsible for color (maybe later asset) change
    /// </summary>
    /// <param name="image"> The image that is supposed to be changed </param>
    private void StartColorChange(Image image)
    {
        StartCoroutine(ColorChangeCoroutine(image));
    }

    /// <summary>
    /// Coroutine responsible for color change for the specific amount of seconds
    /// </summary>
    /// <param name="image"> The image that is supposed to be changed </param>
    /// <returns></returns>
    private IEnumerator ColorChangeCoroutine(Image image)
    {
        image.color = Color.green;

        yield return new WaitForSeconds(timeOfColourChange);

        image.color = Color.white;
    }

    [Serializable]
    private class HighScoreSave
    {
        public int HighScore;
        public string Name;

        public override string ToString()
        {
            return String.Format("{0};{1}", HighScore, Name);
        }

        public HighScoreSave(int score, string name)
        {
            HighScore = score;
            Name = name;
        }
    }

    [Serializable]
    private class HighScoreData
    {
        public List<HighScoreSave> scores;

        public HighScoreData()
        {
            scores = new List<HighScoreSave>();
        }
    }
}