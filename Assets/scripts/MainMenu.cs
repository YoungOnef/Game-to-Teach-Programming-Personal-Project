using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void Play()
    {
        SceneManager.LoadScene("Name of the levelscene Selector");
    }
    public void Options()
    {
        SceneManager.LoadScene("Name of the Option scene");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }


}
