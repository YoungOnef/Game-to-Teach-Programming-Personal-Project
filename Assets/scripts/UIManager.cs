using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;


public class UIManager : MonoBehaviour
{
    // UI elements
    public GameObject inputField;
    public TMP_InputField userInputField;
    public TextMeshProUGUI userOutTextForDebug;
    public TextMeshProUGUI userOutText;
    public TextMeshProUGUI userOutTextFunctionDispaly;
    public GameObject ScreenButton;
    public GameObject HelpWindow;
    public GameObject FunctionWindow;
    // The name of the current scene
    private string sceneName;


    private void Start()
    {

        // Get the name of the current scene
        sceneName = SceneManager.GetActiveScene().name;
        sceneName += ".txt";

    }

    public void PrintToDebugLogAndTextArea(DynValue value)
    {
        // Get the type of the value
        string type = value.Type.ToString();

        // Convert the value to a string
        string message = value.ToObject().ToString();

        // Print the type and value to the debug log
        Debug.Log("Printing " + type + " value from Lua script: " + message);

        // Set the text of the Text object to the type and value
        userOutText.SetText(type + ": " + message);
    }
    public void UserOutTextFunctionDispaly(string text)
    {
        userOutTextFunctionDispaly.text = text;
    }
    public void ResetText()
    {
        userInputField.text = "";
    }

    public void RestartScene()
    {
        print("Level Restarted");
        DataInputHoldingData.instance.dataInput = userInputField.text;
        // Get the current scene name
        string sceneName = SceneManager.GetActiveScene().name;

        // Load the scene with the given name
        SceneManager.LoadScene(sceneName);
    }

    public void SaveInput()
    {

        // get the user input
        string userInput = inputField.GetComponent<TMP_InputField>().text;

        // write the user input to a file
        File.WriteAllText(sceneName, userInput);
    }

    public void LoadSave()
    {
        // read the user input from the file
        string userInput = File.ReadAllText(sceneName);

        // set the input field to the saved user input
        inputField.GetComponent<TMP_InputField>().text = userInput;
    }

    // Function to hide or view the console
    public void HideOrViewConsole()
    {
        // Check if the ScreenButton game object is active
        if (ScreenButton.activeSelf == true)
        {
            // If active, set the game object to inactive
            ScreenButton.SetActive(false);

        }
        else
        {
            // If inactive, set the game object to active
            ScreenButton.SetActive(true);
        }
    }


    public void HideOrViewHelpWindow()
    {
        if (HelpWindow.activeSelf == true)
        {
            HelpWindow.SetActive(false);

        }
        else
        {
            HelpWindow.SetActive(true);
        }
    }

    public void HideOrViewFunctionWindow()
    {
        if (FunctionWindow.activeSelf == true)
        {
            FunctionWindow.SetActive(false);

        }
        else
        {
            FunctionWindow.SetActive(true);
        }
    }



}
