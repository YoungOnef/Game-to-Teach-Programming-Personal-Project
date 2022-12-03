using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoonSharp.Interpreter;
using TMPro;
using System;
using System.IO;


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

    public float speed = 2;

    // Start is called before the first frame update
    void Start()
    {
        CubeRenderer = cube.GetComponent<Renderer>();

    }
    void Update()
    {

    }


    public void inputText()
    {
        try
        {
            //getting text
            string script = inputField.GetComponent<TMP_InputField>().text;
            Debug.Log("script: " + script);


            StartLua(script);
            userOutText.text = "None Error messages from Lua";
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

    }

    public void StartLua(string rawLuaCode)
    {


        //creating a new script Object
        Script myLuaScript = new Script();

        //defining global veriable and sending the veriable
        myLuaScript.Globals["randomChannelOne"] = randomChannelOne;
        myLuaScript.Globals["randomChannelTwo"] = randomChannelTwo;
        myLuaScript.Globals["randomChannelThree"] = randomChannelThree;



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


        newCubeColor = new Color(randomChannelOne, randomChannelTwo, randomChannelThree, 1f);

        CubeRenderer.material.SetColor("_Color", newCubeColor);

    }
    public void resetText()
    {
        userInputField.text = "";
    }

    public void SaveScript(string fileName)
    {

        // create a new StreamWriter and open the file
        StreamWriter writer = new StreamWriter(fileName);

        // write the user's script to the file
        writer.Write(userInputField.text);

        // close the file
        writer.Close();
    }
    public void LoadScript(string fileName)
    {
        // create a new StreamReader and open the file
        StreamReader reader = new StreamReader(fileName);

        // read the entire file into a string
        string script = reader.ReadToEnd();

        // close the file
        reader.Close();

        // set the user's input field to the script we just loaded
        userInputField.text = script;
    }


}
