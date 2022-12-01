using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoonSharp.Interpreter;
using TMPro;
using System;

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
        catch (ScriptRuntimeException ex)
        {
            //example of error message
            //return obj.calcHypotenuse(3, 4);
            Debug.Log("Doh! An error occured! {0}");
            Debug.Log(ex.DecoratedMessage);
            string error = ex.DecoratedMessage.ToString();
            userOutText.text = "An error occured!: \n" + error;
        }

    }    
    private void moveForward()
    {
         cube.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private static int Mul(int a, int b)
    {
        int num = a + b;
        Debug.Log("Sum: ");
        Debug.Log(num);
        return num;
    }

    public void StartLua(string rawLuaCode)
    {


        //creating a new script Object
        Script myLuaScript = new Script();

        //defining global veriable and sending the veriable
        myLuaScript.Globals["randomChannelOne"] = randomChannelOne;
        myLuaScript.Globals["randomChannelTwo"] = randomChannelTwo;
        myLuaScript.Globals["randomChannelThree"] = randomChannelThree;


        //myLuaScript.Globals["MoveForward"] = (Func<void>)moveForward;
        myLuaScript.Globals["Mul"] = (Func<int, int, int>)Mul;

        //running the script via lua
        DynValue result = myLuaScript.DoString(rawLuaCode);


        //result = myLuaScript.Call(myLuaScript.Globals["add"], 4,4);
        /* examples
function add(num1,num2)
     return Mul(num1,num2)
end
add(5,5)
     
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

}
