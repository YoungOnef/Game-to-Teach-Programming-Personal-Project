using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    
    public void Select(string levelName)
    {
        SceneManager.LoadScene(levelName);
        
    }

    

    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }
}
