using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level9Manager : LevelBase
{
    public GameObject prefab; // Reference to the coin prefab
    public List<Coin> coins; // List to store the coins
    float platformLevel = 0.5f;
    public override void Setup(LevelHandler handler)
    {
        base.Setup(handler);
        print("Level1Manager Running");

        handler.spawn(prefab, new Vector3(0, platformLevel, 6));
        handler.spawn(prefab, new Vector3(0, platformLevel, 12));

        // Instantiate the coins and add them to the list
        coins = new List<Coin>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Coin"))
        {
            coins.Add(obj.GetComponent<Coin>());
        }
    }
    /*
local i = 0
while i < 2 do
    local result = CheckForObjectWithTagInDirection("front", "Coin", 6)

    if result == true then
        MoveForward(6)
    else
        print("no coin found")
    end

    i = i + 1
end

        */

}