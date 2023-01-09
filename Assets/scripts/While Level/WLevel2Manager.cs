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



local counter = 0


while not WhatsInFront("front", "Coin", 1) and counter < 100 do
  if WhatsInFront("front", "Enemy", 3) then
    Wait(1)
    sleep(1)
    print("waiting")
  else
    Wait(1)
    sleep(1)
    print("Moving forward")
  end
  counter = counter + 1
  print("counter")

end

        */

}