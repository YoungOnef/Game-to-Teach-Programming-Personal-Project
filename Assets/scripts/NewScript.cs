using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MoonSharp.Interpreter;

public class NewScript : MonoBehaviour
{

    public TextMeshProUGUI userOutText;
    public TMP_InputField userInputField;
    public int Input1 = 10;
    public int Input2 = 10;
    public int Output;

    private Script enviroment;
    void Start()
    {
        Debug.Log("script has started");
        Script.DefaultOptions.DebugPrint = (s) => Debug.Log(s);

        enviroment = new Script();

        string tmpstring = @"
        print('We are in MoonSharp Interpreter')
        
        Output = 10 + 10

        print('we are in a function')
        print(Output)
        
        
        ";
        //*/
        //defing the code
        // string testScript = "print('Hello world')";
        //Debug.Log("Total ", Output);
        //running the script
        DynValue ret = enviroment.DoString(tmpstring);
       // Debug.Log(ret.Type);

    }



    public void setText()
    {
        userOutText.text = userInputField.text;

    }

    public void resetText()
    {
        userInputField.text = "";
    }


}

/* working
 * 
        private Script enviroment;
    void Start()
    {
        Debug.Log("script has started");
        Script.DefaultOptions.DebugPrint = (s) => Debug.Log(s);

        enviroment = new Script();

        string testScript = "print('Hello world')";

        DynValue ret = enviroment.DoString(testScript);
        //Debug.Log(ret.Type);

	}
*/