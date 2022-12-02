using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoonSharp.Interpreter;
using TMPro;
using System;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Level2 : MonoBehaviour
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

    }
    void Update()
    {
        //Run All Added Events PerUpdate
        unityEvent.Invoke();
    }
    //passing function and time 
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

            //run lua
            StartLua(script);
            userOutText.text = "None Error messages from Lua";
            //checking if the list is in the
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



        myLuaScript.Globals["MoveForward"] = (Action<float>)MoveForward; 
        myLuaScript.Globals["MoveRight"] = (Action<float>)MoveRight;
        myLuaScript.Globals["MoveLeft"] = (Action<float>)MoveLeft;
        myLuaScript.Globals["MoveBack"] = (Action<float>)MoveBack;



        //running the script via lua
        DynValue result = myLuaScript.DoString(rawLuaCode);


    }
    public void resetText()
    {
        userInputField.text = "";
        
    }

}
