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

        handler.spawn(prefab, new Vector3(0, platformLevel, 12));


        // Instantiate the coins and add them to the list
        coins = new List<Coin>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Coin"))
        {
            coins.Add(obj.GetComponent<Coin>());
        }
    }
    /*
     * 



-- Set the maximum number of iterations to 100
maxIterations = 100

-- Repeat the loop until the "Coin" is in front of the player or the counter reaches the maximum number of iterations
for i = 1, maxIterations do


    -- Check if the "Enemy" is in front of the player
    Wait(1)
    if WhatsInFront("front", "Enemy", 4) then
            -- There is no "Enemy" in front of the player, so move forward for 1 second
        print("Moving forward")
        MoveForward()

    else
        -- The "Enemy" is in front of the player, so wait for 1 second
        Wait(1)
        print("waiting")
    end

end
    
MoveForward()




        -- Check if the "Coin" is in front of the player
    if WhatsInFront("front", "Coin", 1) then
        -- The "Coin" is in front of the player, so exit the loop
        break
    end




    -- Check if there is an "Enemy" in front of the player
if WhatsInFront("front", "Enemy", 6) then
    -- There is an "Enemy" in front of the player, so do something
      MoveForward()
else
    -- There is no "Enemy" in front of the player, so do something else
    Wait()
end


        */

}