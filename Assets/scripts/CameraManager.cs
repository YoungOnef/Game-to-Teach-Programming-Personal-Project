using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private InputAction action;
    [SerializeField]
    private CinemachineVirtualCamera overWorldCamera;
    [SerializeField]
    private CinemachineVirtualCamera playerCamera;
    public float movementSpeed = 0.1f;

    bool WorldCamera = true;

    private void OnEnable()
    {
        action.Enable();
    }
    private void OnDisable()
    {
        action.Disable();
    }

    private void Start()
    {
        action.performed += _ => SwitchPriority();
    }

    private void Update()
    {
        if (WorldCamera)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            overWorldCamera.transform.position += new Vector3(horizontal, 0, vertical) * movementSpeed;
        }
    }

    private void SwitchPriority()
    {
        if (WorldCamera)
        {
            overWorldCamera.Priority = 0;
            playerCamera.Priority = 1;
        }
        else
        {
            overWorldCamera.Priority = 1;
            playerCamera.Priority = 0;
        }
        WorldCamera = !WorldCamera;
    }
}
