using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoonSharp.Interpreter;
using TMPro;
using System;
using UnityEngine.Events;
using UnityEngine.UIElements;
using System.IO;
using UnityEngine.SceneManagement;

public class Level1 : MonoBehaviour
{
    public GameObject inputField;
    public TMP_InputField userInputField;
    public TextMeshProUGUI userOutText;
    public string ScriptText;

    [SerializeField]
    private GameObject cube;

    private Renderer CubeRenderer;

    private Color newCubeColor;

    private float randomChannelOne, randomChannelTwo, randomChannelThree;
    string sceneName;
    public float speed = 2;
    public float cubeSize = 1.0f;

    //keeping all the functions in the list
    private List<UnityAction> listOfTasks = new List<UnityAction>();
    //keeping all time in the list
    private List<float> listOfTime = new List<float>();
    private IEnumerator CurrnetTask;//CreateCorutine List 
    UnityEvent unityEvent = new UnityEvent();// UnityEvents

    // Start is called before the first frame update
    void Start()
    {
        CubeRenderer = cube.GetComponent<Renderer>();
        sceneName = SceneManager.GetActiveScene().name;
        sceneName += ".txt";

    }
    void Update()
    {
        unityEvent.Invoke();
    }

    private IEnumerator RunCodeUntil(UnityAction callfunction, float waitTime = 0)
    {
        //adding Listener to untiy event
        unityEvent.AddListener(callfunction);
        //waiting 
        yield return new WaitForSeconds(waitTime);
        //remvoing from listening list 
        unityEvent.RemoveListener(callfunction);
    }
    //grabbing whole list from the tasks and loopint thourgh it
    //Starting the timer, for the function and running it
    private IEnumerator DoTask()
    {
        //
        for (int i = 0; i < listOfTasks.Count; i++)
        {
            StartCoroutine(RunCodeUntil(listOfTasks[i], listOfTime[i]));
            yield return new WaitForSeconds(listOfTime[i]);
        }

        //cleainign the list form data
        listOfTasks.Clear();
        listOfTime.Clear();
        yield return true;
    }
    public void inputText()
    {

        //stopping all corotines
        StopAllCoroutines();

        listOfTasks = new List<UnityAction>();
        listOfTime = new List<float>();
        try
        {
            //getting text
            string script = inputField.GetComponent<TMP_InputField>().text;
            Debug.Log("script: " + script);


            StartLua(script);
            userOutText.text = "None Error messages from Lua";

            if (listOfTasks.Count == listOfTime.Count)
            {
                CurrnetTask = DoTask();
                StartCoroutine(CurrnetTask);
            }
            else
            {
                Debug.LogError($"ERROR Lists Count NotMaching : listOfTasks.Count({listOfTasks.Count}) listOfTime.Count({listOfTime.Count})");
            }
        }
        
        catch (SyntaxErrorException ex)
        {
            // if a syntax error was detected, display an error message to the user
            Debug.Log("Syntax error: " + ex.Message);
            userOutText.text = "Syntax error: " + ex.Message;
        }
        catch (ScriptRuntimeException ex)
        {
            // if a runtime error was detected, display an error message to the user
            Debug.Log("Runtime error: " + ex.DecoratedMessage);
            userOutText.text = "Runtime error: " + ex.DecoratedMessage;
        }
        catch (Exception ex)
        {
            // if any other exception was thrown, display the error message to the user
            Debug.Log("Error: " + ex.Message);
            userOutText.text = "Error: " + ex.Message;
        }
        ResetCubePosition();

    }

    public void SetCubeColor(float r, float g, float b)
    {
        // set the cube color using the r, g, and b values provided by the user
        newCubeColor = new Color(r, g, b, 1f);
        CubeRenderer.material.SetColor("_Color", newCubeColor);

        // update the predefined color values so they match the new color of the cube
        randomChannelOne = r;
        randomChannelTwo = g;
        randomChannelThree = b;
    }
    public void SetCubeSize(float size)
    {
        // set the scale of the cube to the specified size
        cube.transform.localScale = new Vector3(size, size, size);
    }
    public void MoveForward(float Time = 1f)
    {
        print($"MoveForward {Time}");
        listOfTasks.Add(MoveF);
        listOfTime.Add(Time);
    }
    private void MoveRight(float Time = 1f)
    {
        print($"MoveRight {Time}");
        listOfTasks.Add(MoveR);
        listOfTime.Add(Time);
    }
    private void MoveLeft(float Time = 1f)
    {
        print($"MoveLeft {Time}");
        listOfTasks.Add(MoveL);
        listOfTime.Add(Time);
    }
    private void MoveBack(float Time = 1f)
    {
        print($"MoveBack {Time}");
        listOfTasks.Add(MoveB);
        listOfTime.Add(Time);
    }
    private void MoveF() => cube.transform.position += Vector3.forward * speed * Time.deltaTime;
    private void MoveR() => cube.transform.position += Vector3.right * speed * Time.deltaTime;
    private void MoveB() => cube.transform.position -= Vector3.forward * speed * Time.deltaTime;
    private void MoveL() => cube.transform.position -= Vector3.right * speed * Time.deltaTime;
    public void StartLua(string rawLuaCode)
    {


        //creating a new script Object
        Script myLuaScript = new Script();

        //defining global veriable and sending the veriable
        myLuaScript.Globals["randomChannelOne"] = randomChannelOne;
        myLuaScript.Globals["randomChannelTwo"] = randomChannelTwo;
        myLuaScript.Globals["randomChannelThree"] = randomChannelThree;


        myLuaScript.Globals["setCubeColor"] = (Action<float, float, float>)((r, g, b) => SetCubeColor(r, g, b));
        myLuaScript.Globals["SetCubeSize"] = (Action<float>)SetCubeSize;
        myLuaScript.Globals["MoveForward"] = (Action<float>)MoveForward;
        myLuaScript.Globals["MoveRight"] = (Action<float>)MoveRight;
        myLuaScript.Globals["MoveLeft"] = (Action<float>)MoveLeft;
        myLuaScript.Globals["MoveBack"] = (Action<float>)MoveBack;

        //running the script via lua
        DynValue result = myLuaScript.DoString(rawLuaCode);




        //getting veriable back
        randomChannelOne = (float)myLuaScript.Globals.Get("randomChannelOne").CastToNumber();
        randomChannelTwo = (float)myLuaScript.Globals.Get("randomChannelTwo").CastToNumber();
        randomChannelThree = (float)myLuaScript.Globals.Get("randomChannelThree").CastToNumber();




        Debug.Log("randomChannelOne");
        Debug.Log(randomChannelOne);

        Debug.Log("randomChannelTwo");
        Debug.Log(randomChannelTwo);

        Debug.Log("randomChannelThree");
        Debug.Log(randomChannelThree);
        SetCubeColor(randomChannelOne, randomChannelTwo, randomChannelThree);

        //newCubeColor = new Color(randomChannelOne, randomChannelTwo, randomChannelThree, 1f);

        //CubeRenderer.material.SetColor("_Color", newCubeColor);

    }
    public void resetText()
    {
        userInputField.text = "";
    }

    public void Stop()
    {
        // Clear the list of tasks and stop the current task, if any
        listOfTasks.Clear();
        listOfTime.Clear();
        StopCoroutine(CurrnetTask);
    }

    public void ResetCubePosition()
    {
        // reset the position of the cube to its original position
        cube.transform.position = Vector3.zero;
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


}
