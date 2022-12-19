using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.ProBuilder.Shapes;

public class XYZManager : MonoBehaviour
{
    public TextMeshProUGUI clickCoordinatesText;  // Text element to display the coordinates

    public GameObject cube;  // The cube game object
    public TextMeshProUGUI cubeCoordinatesText;  // Text element to display the coordinates
    
    // Update is called once per frame
    void Update()
    {
        // Get the position of the cube
        Vector3 position = cube.transform.position;

        // Update the coordinatesText element to display the cube's position with two decimal places
        cubeCoordinatesText.text = "Clicked Coordinates: " + position.x.ToString("F2") + ", " + position.y.ToString("F2") + ", " + position.z.ToString("F2");

        // Check if the left mouse button was clicked
        if (Input.GetMouseButtonDown(1))
        {
            // Get the ray that was cast from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Create a RaycastHit object to store the result of the raycast
            RaycastHit hit;

            // Perform the raycast and check if it hit something
            if (Physics.Raycast(ray, out hit))
            {
                // Get the position of the hit point
                Vector3 hitPoint = hit.point;

                // Update the coordinatesText element to display the hit point coordinates
                clickCoordinatesText.text = "Cube Coordinates: " + hitPoint.x.ToString("F2") + ", " + hitPoint.y.ToString("F2") + ", " + hitPoint.z.ToString("F2");

            }
        }
    }
}
