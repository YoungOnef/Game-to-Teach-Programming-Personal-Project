using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    // Reference to the level base
    LevelBase level;
    // Initial spawn position
    Vector3 spawnPosition = new Vector3(0, 0, 0);
    // Total number of points needed to be collected
    int pointsNeeded = 0;
    // Player's current coin count
    public int coinCount = 0;

    private void Start()
    {
        // Get the level base component attached to the "LevelManager" GameObject
        level = GameObject.Find("LevelManager").GetComponent<LevelBase>();
        // Call the Setup method of the level base, passing the LevelHandler as a parameter
        level.Setup(this);
        // Log the total number of points needed to be collected
        Debug.Log($"Points needed = ( {pointsNeeded} )");
    }

    // Method to spawn a prefab at a specified position
    public void spawn(GameObject prefab, Vector3 position)
    {
        // Increase the total number of points needed
        pointsNeeded++;
        // Then, instantiate the prefab at the specified position
        Instantiate(prefab, position, transform.rotation);
    }

    // Method to check if all the points have been collected
    public bool AllPointsCollected()
    {
        // Return true if all the points have been collected, false otherwise
        return pointsNeeded == coinCount;
    }

}