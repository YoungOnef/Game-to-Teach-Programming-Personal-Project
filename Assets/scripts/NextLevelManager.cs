using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelManager : MonoBehaviour
{
    public int nextSceneLoad;

    public GameObject endingScreenButton;
    // Start is called before the first frame update
    void Start()
    {
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
        endingScreenButton.SetActive(false);

    }

    public void NextSceneLoad()
    {
        SceneManager.LoadScene(nextSceneLoad);
    }

    public void ShowEndingScreen()
    {
        endingScreenButton.SetActive(true);
    }

    public void LevelSelector()
    {
        SceneManager.LoadScene("Level Selector");
    }
}