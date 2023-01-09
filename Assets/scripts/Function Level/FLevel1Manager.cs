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


function MoveForwardAndTurn()
    -- Multiply the distance moved by 2 each time the function is called
    distance = distance * 2
    -- Move forward for the specified distance
    MoveForward(distance)
    -- Turn right
    Turn("right")
end

-- Set the initial distance to 1
distance = 2.5

-- Repeat the MoveForwardAndTurn function 4 times
for i = 1, 4 do
    MoveForwardAndTurn()
end


        */

}