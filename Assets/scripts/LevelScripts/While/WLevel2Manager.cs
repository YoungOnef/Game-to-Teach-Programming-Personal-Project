using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WLevel2Manager : LevelBase
{
    public GameObject prefab; // Reference to the coin prefab
    public List<Coin> coins; // List to store the coins
    float platformLevel = 0.5f;
    public override void Setup(LevelHandler handler)
    {
        base.Setup(handler);
        print("Level1Manager Running");
        handler.spawn(prefab, new Vector3(-6, platformLevel, 8));
        handler.spawn(prefab, new Vector3(-12, platformLevel, 16));
        handler.spawn(prefab, new Vector3(-18, platformLevel, 24));
        // Instantiate the coins and add them to the list
        coins = new List<Coin>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Coin"))
        {
            coins.Add(obj.GetComponent<Coin>());
        }
    }

    /*
while (true)
do
MoveForward(8)
MoveLeft(6)
end

    

    */
}