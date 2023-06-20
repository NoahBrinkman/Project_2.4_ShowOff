using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class LeaderboardShow : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> names;
    private string _fileName = "Leaderboard.txt";
    private string _folderPath;
    private string _filePath;
    bool hasSwitched = false;
    private List<HighScoreSave> sd = new List<HighScoreSave>();

    private void Start()
    {
        _folderPath = Path.Combine(Application.dataPath, "9. Data");

        if (!Directory.Exists(_folderPath))
            Directory.CreateDirectory(_folderPath);

        _filePath = Path.Combine(_folderPath, _fileName);

    }

    private void Update()
    {
        ReadData();

        for (int i = 0; i < names.Count; i++)
        {
            string name = sd[i].Name;
            names[i].text = $"{name.Substring(0,2)} & {name.Substring(name.Length-2,2)}\n{sd[i].HighScore}";
        }
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




