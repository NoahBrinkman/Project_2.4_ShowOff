using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void SwitchScene(Scene scene)
    {
        try
        {
            SceneManager.LoadScene(scene.name);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void SwitchScene(int buildIndex)
    {
        try
        {
            SceneManager.LoadScene(buildIndex);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
