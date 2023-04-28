using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    // This field is of type InputAction and is marked with SerializeField, which allows it to be shown in the Inspector window.
    [SerializeField] private InputAction action;

    // These fields are of type CinemachineVirtualCamera and are also marked with SerializeField.
    [SerializeField] private CinemachineVirtualCamera overWorldCamera;
    [SerializeField] private CinemachineVirtualCamera playerCamera;

    // This public float is used to control the player's movement speed.
    public float movementSpeed = 0.1f;

    // This bool is used to determine whether the world camera is active or not.
    bool WorldCamera = false;

    // This field is of type UIManager and is not marked with SerializeField since it is assigned to in code.
    UIManager uIManager;

    // The OnEnable() method is called when the object becomes enabled and enables the InputAction.
    private void OnEnable()
    {
        action.Enable();
    }

    // The OnDisable() method is called when the object becomes disabled and disables the InputAction.
    private void OnDisable()
    {
        action.Disable();
    }


    private void Start()
    {
        // Get a reference to the UIManager component
        uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        // Subscribe to the performed event of the action variable
        action.performed += _ => SwitchPriority();
    }

    private void Update()
    {
        // If the WorldCamera variable is true and the user input field is not focused
        if (WorldCamera && !uIManager.userInputField.isFocused)
        {
            // Get the horizontal and vertical inputs from the user
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Move the camera based on the user's inputs
            overWorldCamera.transform.position += new Vector3(horizontal, 0, vertical) * movementSpeed;
        }
    }

    private void SwitchPriority()
    {
        // If the user input field is not focused
        if (!uIManager.userInputField.isFocused)
        {
            // If the WorldCamera variable is true
            if (WorldCamera)
            {
                // Set the priority of the overworld camera to 0 and the player camera to 1
                overWorldCamera.Priority = 0;
                playerCamera.Priority = 1;
            }
            else
            {
                // Set the priority of the overworld camera to 1 and the player camera to 0
                overWorldCamera.Priority = 1;
                playerCamera.Priority = 0;
            }
            // Toggle the WorldCamera variable
            WorldCamera = !WorldCamera;
        }
    }
}
