using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoonSharp.Interpreter;
using TMPro;

public class Level1 : MonoBehaviour
{
    public GameObject inputField;
    public string ScriptText;

    [SerializeField]
    private GameObject cube;

    private Renderer CubeRenderer;

    private Color newCubeColor;

    private float randomChannelOne, randomChannelTwo, RandomChannelThree;

    // Start is called before the first frame update
    void Start()
    {
        CubeRenderer = cube.GetComponent<Renderer>();
    }



    public void inputText()
    {
        //getting text
        string script = inputField.GetComponent<TMP_InputField>().text;
        Debug.Log("script: " + script);


        StartLua(script);
    }
    public void StartLua(string rawLuaCode)
    {


        //creating a new script Object
        Script myLuaScript = new Script();

        //defining global veriable and sending the veriable
        myLuaScript.Globals["randomChannelOne"] = randomChannelOne;
        myLuaScript.Globals["randomChannelTwo"] = randomChannelTwo;
        myLuaScript.Globals["RandomChannelThree"] = RandomChannelThree;


        //running the script via lua
        DynValue result = myLuaScript.DoString(rawLuaCode);

        /* examples
randomChannelOne = 1
randomChannelTwo = 0
randomChannelThree = 1


math.randomseed(os.time())
math.random()
randomChannelOne = math.random(0.0,1)
randomChannelTwo = math.random(0.0,1)
randomChannelThree = math.random(0.0,1)


math.randomseed(os.time())
math.random()
randomChannelOne = math.random()
randomChannelTwo = math.random()
randomChannelThree = math.random()
         */



        //getting veriable back
        float newrandomChannelOne = (float)myLuaScript.Globals.Get("randomChannelOne").CastToNumber();
        float newrandomChannelTwo = (float)myLuaScript.Globals.Get("randomChannelTwo").CastToNumber();
        float newRandomChannelThree = (float)myLuaScript.Globals.Get("randomChannelThree").CastToNumber();

        Debug.Log("newrandomChannelOne");
        Debug.Log(newrandomChannelOne);
        
        Debug.Log("newrandomChannelTwo");
        Debug.Log(newrandomChannelTwo);

        Debug.Log("newRandomChannelThree");
        Debug.Log(newRandomChannelThree);
        

        newCubeColor = new Color(newrandomChannelOne, newrandomChannelTwo, newRandomChannelThree, 1f);

        CubeRenderer.material.SetColor("_Color", newCubeColor);


    }

}
