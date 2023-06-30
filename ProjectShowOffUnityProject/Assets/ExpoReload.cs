using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExpoReload : SingleTon<ExpoReload>
{
    private float _timer = 0.0f;
    private float _target = 3;

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
            SceneManager.LoadScene(0);
        }
    }
}
