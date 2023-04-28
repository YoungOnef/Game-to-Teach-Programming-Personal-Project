using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelManager : MonoBehaviour
{
    public int nextSceneLoad; // Integer variable that holds the index of the next scene to be loaded.

    public GameObject endingScreenButton; // Game object that represents the button to show the ending screen.

    // Start is called before the first frame update
    void Start()
    {
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1; // Sets the value of nextSceneLoad to the index of the current scene plus 1.
        endingScreenButton.SetActive(false); // Deactivates the ending screen button game object.
    }

    // Method to load the next scene based on the value of nextSceneLoad.
    public void NextSceneLoad()
    {
        SceneManager.LoadScene(nextSceneLoad);
    }

    // Method to show the ending screen by activating the ending screen button game object.
    public void ShowEndingScreen()
    {
        endingScreenButton.SetActive(true);
    }

    // Method to load the level selector scene.
    public void LevelSelector()
    {
        SceneManager.LoadScene("Level Selector");
    }

}