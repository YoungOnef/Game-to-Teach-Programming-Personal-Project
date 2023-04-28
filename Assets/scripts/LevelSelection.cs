using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    // This function loads the level whose name is passed as an argument
    public void Select(string levelName)
    {
        SceneManager.LoadScene(levelName);
        
    }

    // This function loads the main menu
    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }
}
