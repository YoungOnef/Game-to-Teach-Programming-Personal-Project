using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    // This public method loads the "Level Selector" scene when called
    public void Play()
    {
        SceneManager.LoadScene("Level Selector");
    }

    // This public method loads the "Tutorial" scene when called
    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    // This public method quits the application and logs a "Quit" message to the console when called
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }



}
