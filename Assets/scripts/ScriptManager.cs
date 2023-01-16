using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using TMPro;
using System;
using UnityEngine.Events;


public class ScriptManager : MonoBehaviour
{
    UIManager uIManager;
    [SerializeField]
    private GameObject player;

    [Min(0f)][SerializeField] float speed = 1f;

    [SerializeField] bool __TimerON = false;
    [SerializeField] float __TimerTime = -1f;
    [SerializeField]
    Vector3 CubeTargetPos = new Vector3(0, 0, 0);
    bool Moving = false;
    float MovingTimer = -1;
    string code;
    private Vector3 Move(Vector3 vector3, int Distance)
    {
        Wait(Distance);
        return new Vector3(0, 0, 0);
    }
    

    public void MoveForward(int Distance = 1) => CubeTargetPos += Move(Vector3.forward,Distance);
    public void MoveRight(int Distance = 1) => CubeTargetPos += Move(Vector3.right, Distance);
    public void MoveBack(int Distance = 1) => CubeTargetPos += Move(Vector3.back, Distance);
    public void MoveLeft(int Distance = 1) => CubeTargetPos += Move(Vector3.left, Distance);
    public void Wait(float seconds = 1)
    {
        __TimerON = true;
        __TimerTime = seconds;
    }
    private void Start()
    {
        uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }


    private IEnumerator _Start()
    {
        var _readyCode = "function Start()\n" + code + "\nend";
        // Load the code and get the returned function
        Script script = new Script(CoreModules.None);
        script.Globals["MoveForward"] = (Action<int>)MoveForward;
        script.Globals["MoveRight"] = (Action<int>)MoveRight;
        script.Globals["MoveBack"] = (Action<int>)MoveBack;
        script.Globals["MoveLeft"] = (Action<int>)MoveLeft;
        script.Globals["Wait"] = (Action<float>)Wait;
        //script.LoadFunction("function Wait (n) return ('Wait' ,n) end \n");
        script.DoString(_readyCode);

        // get the function
        DynValue function = script.Globals.Get("Start");//start funcion

        // Create the coroutine in C#
        DynValue coroutine = script.CreateCoroutine(function);

        coroutine.Coroutine.AutoYieldCounter = 2;//line by line yeld return ????

        DynValue result = null;

        result = coroutine.Coroutine.Resume();
        while (true)
        {

            if (result.Type != DataType.YieldRequest)
                break;


            if (__TimerON)
            {
                yield return new WaitForSeconds(__TimerTime);
                __TimerON = false;
                __TimerTime = -1;
            }
            yield return new WaitForSeconds(.1f);
            result = coroutine.Coroutine.Resume();
        }
    }


    public void InputText()
    {
        code = uIManager.inputField.GetComponent<TMP_InputField>().text;
        
        StartCoroutine(_Start());


    }
}