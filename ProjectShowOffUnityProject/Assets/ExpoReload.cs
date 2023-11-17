using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExpoReload : SingleTon<ExpoReload>
{
    private float _timer = 0.0f;
    private float _target = 3;


    private float resetTimer = 0.0f;
    private float timeToReset = 15.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.N) && Input.GetKey(KeyCode.O) &&
            Input.GetKey(KeyCode.P))
        {
            _timer += Time.deltaTime;
            //Debug.Log(_timer);
        }
        else
        {
            _timer = 0.0f;
        }

        if (_timer >= _target)
        {
            _timer = 0.0f;
            ScoreManager.Instance.Score = 0;
            SceneManager.LoadScene(0);
        }

        if (Input.anyKey)
        {
            resetTimer = 0.0f;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            resetTimer += Time.deltaTime;
            if(resetTimer >= timeToReset)
            {
                Debug.Log("RELOADING");
                resetTimer = 0;
                ScoreManager.Instance.Score = 0;
                SceneManager.LoadScene(0);
            }
        }

    }
}
