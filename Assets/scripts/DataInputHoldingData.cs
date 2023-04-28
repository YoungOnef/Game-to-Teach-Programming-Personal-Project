using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataInputHoldingData : MonoBehaviour
{
    // This is a public static variable that holds the instance of the object.
    public static DataInputHoldingData instance;

    // This is a private string variable used to hold the data input.
    public string dataInput;

    // The Awake() method is called when the object is initialized.
    private void Awake()
    {
        // Check if there is already an instance of this object.
        if (instance == null)
        {
            // If there is no instance, set this as the instance and make it persistent across scenes.
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If there is already an instance, destroy this instance.
            Destroy(gameObject);
        }
    }

}
