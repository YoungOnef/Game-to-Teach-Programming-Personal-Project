using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerTemplate : LevelBase
{
    // Reference to the coin prefab
    public GameObject prefab;

    // List to store the coins
    public List<Coin> coins;

    // The level for the platform
    float platformLevel = 0.5f;

    // Overrides the Setup method from the LevelBase class
    public override void Setup(LevelHandler handler)
    {
        // Call the base class's Setup method
        base.Setup(handler);

        // Print a message to indicate that the level manager is running
        print("Level1Manager Running");

        // Call the spawn method on the handler to spawn the prefab
        handler.spawn(prefab, new Vector3(0, platformLevel, 6));
        handler.spawn(prefab, new Vector3(0, platformLevel, 12));

        // Initialize the list of coins
        coins = new List<Coin>();

        // Find all game objects with the "Coin" tag and add their Coin component to the list of coins
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Coin"))
        {
            coins.Add(obj.GetComponent<Coin>());
        }
    }
}