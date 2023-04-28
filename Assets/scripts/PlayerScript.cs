using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private LevelHandler GetLevelHandler()
    {
        return GameObject.Find("LevelHandler").GetComponent<LevelHandler>();
    }

    private NextLevelManager GetNextLevelManager()
    {
        return GameObject.Find("NextLevelManager").GetComponent<NextLevelManager>();
    }

    private void Update()
    {
        LevelHandler levelHandler = GetLevelHandler();
        NextLevelManager nextLevelManager = GetNextLevelManager();

        if (levelHandler.AllPointsCollected())
        {
            // All points have been collected
            Debug.Log("All points collected!");
            nextLevelManager.ShowEndingScreen();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entered the trigger is tagged as "Coin"
        if (other.gameObject.CompareTag("Coin"))
        {
            // Get the Coin component of the coin object
            Coin coin = other.gameObject.GetComponent<Coin>();

            // Check if the coin hasn't been collected yet
            if (!coin.collected)
            {
                // Get the LevelHandler component
                LevelHandler levelHandler = GetLevelHandler();

                // Increment the coin count and set the collected flag
                levelHandler.coinCount += coin.value;
                coin.collected = true;

                // Destroy the coin object
                Destroy(other.gameObject);
            }
        }
        else if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Wall"))
        {
            // Call the HandlePlayerDeath function
            HandlePlayerDeath();

            // Destroy the enemy or wall object
            Destroy(other.gameObject);
        }
    }
    private void HandlePlayerDeath()
    {
        // Get the UIManager component
        UIManager uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        // Call the RestartScene function of the UIManager component
        uiManager.RestartScene();

        // Log the message "You are dead!"
        Debug.Log("You are dead!");
    }

}
