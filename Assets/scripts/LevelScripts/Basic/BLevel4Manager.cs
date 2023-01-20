using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BLevel5Manager : LevelBase
{
    public GameObject prefab; // Reference to the coin prefab
    public List<Coin> coins; // List to store the coins
    float platformLevel = 0.5f;
    public override void Setup(LevelHandler handler)
    {
        base.Setup(handler);
        print("Level1Manager Running");

        handler.spawn(prefab, new Vector3(40, platformLevel, 0));

        // Instantiate the coins and add them to the list
        coins = new List<Coin>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Coin"))
        {
            coins.Add(obj.GetComponent<Coin>());
        }
    }
    /*
TurnRight()
MoveForward(13)
TurnLeft()
MoveForward(6)
TurnRight()
MoveForward(11)
TurnRight()
MoveForward(6)
TurnLeft()
MoveForward(15)
        */

}