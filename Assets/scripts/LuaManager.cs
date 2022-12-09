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

public class LuaManager : MonoBehaviour
{

    private void StartLua(string script)
    {
        // Create a new instance of the Lua interpreter
        Script lua = new Script();


        //lua.Globals["this"] = this;

        //lua.Globals["newCube"] = newCube;

        // Register the "print" function so that the script can print messages to the debug log
        lua.Globals["print"] = (Action<DynValue>)PrintToDebugLogAndTextArea; ;

        // Register the custom functions that the script can call
        RegisterFunctions(lua);

        // Execute the script
        lua.DoString(script);
    }
    private void RegisterFunctions(Script lua)
    {

        // Register the "SetCubeColor" function
        lua.Globals["SetCubeColor"] = (Action<float, float, float>)SetCubeColor;

        // Register the "SetCubeSize" function
        lua.Globals["SetCubeSize"] = (Action<float>)SetCubeSize;

        // Register the "SetCubeSpeed" function
        lua.Globals["SetCubeSpeed"] = (Action<float>)SetCubeSpeed;

        //functions to move the cube
        lua.Globals["MoveForward"] = (Action<float>)MoveForward;
        lua.Globals["MoveRight"] = (Action<float>)MoveRight;
        lua.Globals["MoveLeft"] = (Action<float>)MoveLeft;
        lua.Globals["MoveBack"] = (Action<float>)MoveBack;
        lua.Globals["Move"] = (Action<double, string, double>)Move;
        lua.Globals["Turn"] = (Action<string>)Turn;
        lua.Globals["Wait"] = (Action<float>)Wait;

        /*-- Set the cube's color to blue
SetCubeColor(0, 0, 1)

-- Wait for 2 seconds
Wait(1)

SetCubeSize(5)

SetCubeSpeed(4)
Wait(1)
-- Set the cube's color to red
SetCubeColor(1, 0, 0)

-- Move the cube forward by 1 unit
MoveForward(1)

-- Wait for 1 second
Wait(1)

-- Move the cube right by 1 unit
MoveRight(1)
Wait(1)
MoveBack(1)

        */
    }
}
