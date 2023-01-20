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
        if (other.gameObject.CompareTag("Coin"))
        {
            Coin coin = other.gameObject.GetComponent<Coin>();
            if (!coin.collected)
            {
                // Increment the coin count and set the collected flag
                LevelHandler levelHandler = GetLevelHandler();
                levelHandler.coinCount += coin.value;
                coin.collected = true;
                Destroy(other.gameObject);
            }
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            HandlePlayerDeath();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            HandlePlayerDeath();
            Destroy(other.gameObject);
        }

    }

    private void HandlePlayerDeath()
    {
        UIManager uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        uiManager.RestartScene();
        Debug.Log("You are dead!");
    }
}
