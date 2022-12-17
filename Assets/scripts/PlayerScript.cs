using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private void Update()
    {
        LevelHandler levelHandler = GameObject.Find("LevelHandler").GetComponent<LevelHandler>();
        if (levelHandler.AllPointsCollected())
        {
            // All points have been collected
            Debug.Log("All points collected!");
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
                LevelHandler levelHandler = GameObject.Find("LevelHandler").GetComponent<LevelHandler>();
                levelHandler.coinCount += coin.value;
                coin.collected = true;
                Destroy(other.gameObject);
            }
        }
    }
}