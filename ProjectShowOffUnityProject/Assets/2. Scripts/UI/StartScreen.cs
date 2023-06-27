using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public Image startImage;
    public Image highlightedStartImage;
    public Image quitImage;
    public Image highlightedQuitImage;

    // Start is called before the first frame update
    void Start()
    {
        // Assign the images in the Unity editor by dragging and dropping
        // the image objects onto the script component in the Inspector.
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            startImage.enabled = false;
            highlightedStartImage.enabled = true;

            quitImage.enabled = true;
            highlightedQuitImage.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            startImage.enabled = true;
            highlightedStartImage.enabled = false;

            quitImage.enabled = false;
            highlightedQuitImage.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            if (highlightedStartImage.gameObject.activeSelf)
            {
                // Load the next scene for Start image
                SceneManager.LoadScene("GamePlayScene");
            }
            else if (highlightedQuitImage.gameObject.activeSelf)
            {
                // Load the next scene for Quit image
                SceneManager.LoadScene("NextSceneForQuitImage");
            }
        }
    }
}
