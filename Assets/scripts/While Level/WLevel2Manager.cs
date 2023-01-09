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

    maxIterations = 100

for i = 1, maxIterations do
    if WhatsInFront("front", "Enemy", 4) then
        Wait(5)
        print("Enemy infront waiting")



    else
        print("Moving forward")
        MoveForward()
end
end
    
MoveForward()


maxIterations = 100

for i = 1, maxIterations do


    if WhatsInFront("front", "Enemy", 4) then
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