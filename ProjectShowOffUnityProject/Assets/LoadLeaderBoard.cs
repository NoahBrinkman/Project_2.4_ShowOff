using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLeaderBoard : MonoBehaviour
{
    [SerializeField] private int leaderBoardSceneIndex = 3;
    // Start is called before the first frame update
    void Start()
    {
        if (Display.displays.Length > 1)
        {
            if (!SceneManager.GetSceneAt(leaderBoardSceneIndex).isLoaded)
            {
                SceneManager.LoadScene(leaderBoardSceneIndex, LoadSceneMode.Additive);
            }
        }
    }
}
