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

public class ObjectManager : MonoBehaviour
{

    private Color newCubeColor;
    private float randomChannelOne, randomChannelTwo, randomChannelThree;
    private float speed = 2;

    [SerializeField]
    private GameObject cube;
    private Renderer cubeRenderer;
    public GameObject ScreenButton;
    public void SetCubeColor(float r, float g, float b)
    {
        // set the cube color using the r, g, and b values provided by the user
        newCubeColor = new Color(r, g, b, 1f);
        cubeRenderer.material.SetColor("_Color", newCubeColor);

        // update the predefined color values so they match the new color of the cube
        randomChannelOne = r;
        randomChannelTwo = g;
        randomChannelThree = b;

        userOutTextFunctionDispaly.text = "SetCubeColor";
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
    private void Wait(float Time = 1f)
    {

        print($"Wait {Time}");
        listOfTasks.Add(() => { });
        listOfTime.Add(Time);

    }
    // The "move" function moves the cube by the specified amount in the specified direction.
    private void Move(double distance, string direction, double delay)
    {
        Vector3 displacement = new Vector3(0, 0, 0);

        // Set the displacement based on the specified direction
        if (direction == "up")
        {
            displacement = new Vector3(0, (float)distance * speed * Time.deltaTime, 0);
        }
        else if (direction == "down")
        {
            displacement = new Vector3(0, -(float)distance * speed * Time.deltaTime, 0);
        }
        else if (direction == "left")
        {
            displacement = new Vector3(-(float)distance * speed * Time.deltaTime, 0, 0);
        }
        else if (direction == "right")
        {
            displacement = new Vector3((float)distance * speed * Time.deltaTime, 0, 0);
        }
        else if (direction == "forward")
        {
            displacement = new Vector3(0, 0, (float)distance * speed * Time.deltaTime);
        }
        else if (direction == "backward")
        {
            displacement = new Vector3(0, 0, -(float)distance * speed * Time.deltaTime);
        }

        // Add a task to the list of tasks that moves the cube by the specified amount in the specified direction.
        // The task will wait for the specified delay plus the time delta before executing.
        listOfTasks.Add(() => cube.transform.position += displacement);
        listOfTime.Add((float)delay + Time.deltaTime);
    }

    private void Turn(string direction)
    {
        Vector3 rotation = new Vector3(0, 0, 0);

        // Set the rotation based on the specified direction
        if (direction == "left")
        {
            rotation = new Vector3(0, -90, 0);
        }
        else if (direction == "right")
        {
            rotation = new Vector3(0, 90, 0);
        }
        else if (direction == "back")
        {
            rotation = new Vector3(0, 180, 0);
        }
        cube.transform.Rotate(rotation);
    }

    private void MoveF() => cube.transform.position += Vector3.forward * speed * Time.deltaTime;
    private void MoveR() => cube.transform.position += Vector3.right * speed * Time.deltaTime;
    private void MoveB() => cube.transform.position -= Vector3.forward * speed * Time.deltaTime;
    private void MoveL() => cube.transform.position -= Vector3.right * speed * Time.deltaTime;


    // This method sets the speed at which the cube moves.
    private void SetCubeSpeed(float speed)
    {
        // Set the speed field to the specified value
        userOutTextFunctionDispaly.text = "SetCubeSpeed";
        this.speed = speed;
    }
}
