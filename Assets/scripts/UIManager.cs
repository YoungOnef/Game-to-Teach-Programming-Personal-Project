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
    // This method updates the text displayed in the user output field
    public void UserOutTextFunctionDispaly(string text)
    {
        // Set the text of the user output field to the provided text
        userOutTextFunctionDispaly.text = text;
    }

    // This method resets the text in the user input field
    public void ResetText()
    {
        // Set the text of the user input field to an empty string
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


    // This method toggles the visibility of the HelpWindow game object
    public void HideOrViewHelpWindow()
    {
        // Check if HelpWindow is currently active (visible)
        if (HelpWindow.activeSelf == true)
        {
            // If HelpWindow is active, hide it
            HelpWindow.SetActive(false);
        }
        else
        {
            // If HelpWindow is inactive, show it
            HelpWindow.SetActive(true);
        }
    }

    // This method toggles the visibility of the FunctionWindow game object
    public void HideOrViewFunctionWindow()
    {
        // Check if FunctionWindow is currently active (visible)
        if (FunctionWindow.activeSelf == true)
        {
            // If FunctionWindow is active, hide it
            FunctionWindow.SetActive(false);
        }
        else
        {
            // If FunctionWindow is inactive, show it
            FunctionWindow.SetActive(true);
        }
    }




}
