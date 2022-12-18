using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Manager : LevelBase
{
    public GameObject prefab; // Reference to the coin prefab
    public List<Coin> coins; // List to store the coins

    public override void Setup(LevelHandler handler)
    {
        base.Setup(handler);
        print("Level1Manager Running");
        handler.spawn(prefab, new Vector3(0, 0.5f, 0));
        handler.spawn(prefab, new Vector3(0, 0.5f, 5));
        handler.spawn(prefab, new Vector3(0, 0.5f, 10));

        // Instantiate the coins and add them to the list
        coins = new List<Coin>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Coin"))
        {
            coins.Add(obj.GetComponent<Coin>());
        }
    }
}