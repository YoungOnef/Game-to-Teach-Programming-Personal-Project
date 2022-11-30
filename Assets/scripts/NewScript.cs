using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MoonSharp.Interpreter;

public class NewScript : MonoBehaviour
{


    public int number = 10;

    public TextMeshProUGUI userOutText;
    public TMP_InputField userInputField;
    public GameObject inputField;

    public string ScriptText;

    public GameObject toggler;

    void Start()
    {
        //string text = inputField.GetComponent<TMP_InputField>().text;
        // string tmpstring = @"
        // return 10 + 10
        // ";
        //StartLua(tmpstring);
        /*StartLua(@"
        function test( n )
            return n * 2
        end
        ");
        */
        //print(toggler.GetComponent<Toggle>().isOn);
        //togInput1 = true;
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
        myLuaScript.Globals["number"] = number;

        //running the script via lua
        DynValue result = myLuaScript.DoString(rawLuaCode);

        //getting veriable back
        int newnumber = (int)myLuaScript.Globals.Get("number").CastToNumber();

        Debug.Log("newnumber");
        Debug.Log(newnumber);
        //call the function, send the parameter, return it as defined veriable
        //result = myLuaScript.Call(myLuaScript.Globals["test"], 2);



        Debug.Log("result.Type");
        Debug.Log(result.Type);
        //Debug.Log(result.Number);
        //userOutText.text = result.ToString();
        //displaying the result


        if (result.Type == DataType.Number)
        {
            Debug.Log(result.Number);
            double number = result.Number;
            setTextdouble(number);

        }

        if (result.Type == DataType.String)
        {
            Debug.Log(result.String);
            string word = result.String;
            //setText(word);
            userOutText.text = word.ToString();
        }
        //calling the function and passing a number
        //result = myLuaScript.Call(myLuaScript.Globals["test"],20);

        //displaying the result
        //Debug.Log(result.Number);

    }
    public void setTextdouble(double number)
    {
        userOutText.text = number.ToString();

    }
    public void setText(string word)
    {
        //Debug.Log("setText ", word);
        userOutText.text = word.ToString();

    }
}