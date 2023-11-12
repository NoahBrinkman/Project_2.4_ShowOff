using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private Image startImage;
    [SerializeField] private Image highlightedStartImage;
    [SerializeField] private Image quitImage;
    [SerializeField] private Image highlightedQuitImage;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingScreenSlider;

    private float _loadingScreenTarget;

    // Start is called before the first frame update
    void Start()
    {
        // Assign the images in the Unity editor by dragging and dropping
        // the image objects onto the script component in the Inspector.
        loadingScreen.SetActive(false);
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
                LoadScene("GamePlayScene");
            }
            else if (highlightedQuitImage.gameObject.activeSelf)
            {
                // Load the next scene for Quit image
                SceneManager.LoadScene("NextSceneForQuitImage");
            }
        }
    }

    public async void LoadScene(string sceneName)
    {
        loadingScreenSlider.value = 0;
        _loadingScreenTarget = 0;
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        loadingScreen.SetActive(true);

        do
        {
            await Task.Delay(100);
            _loadingScreenTarget = scene.progress;

        } while (loadingScreenSlider.value < 0.9f);

        scene.allowSceneActivation = true;
    }
}
