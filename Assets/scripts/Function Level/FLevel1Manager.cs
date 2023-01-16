using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLevel1Manager : LevelBase
{
    public GameObject prefab; // Reference to the coin prefab
    public List<Coin> coins; // List to store the coins
    float platformLevel = 0.5f;
    public override void Setup(LevelHandler handler)
    {
        base.Setup(handler);
        print("Level1Manager Running");

        handler.spawn(prefab, new Vector3(10, platformLevel, -15));


        // Instantiate the coins and add them to the list
        coins = new List<Coin>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Coin"))
        {
            coins.Add(obj.GetComponent<Coin>());
        }
    }
    /*
     * 

    Working


distance = 2.5

for i = 1, 3 do


    distance = distance * 2
    MoveForward(distance)
    TurnRight()
    
end

    ---------------------------
distance = 2.5

for i = 1, 3 do

    MoveForwardAndTurn()
    
end

    
function MoveForwardAndTurn()

    distance = distance * 2

    MoveForward(distance)

    TurnRight()
end


        */

}