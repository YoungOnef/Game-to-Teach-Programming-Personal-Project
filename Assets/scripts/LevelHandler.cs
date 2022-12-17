using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    LevelBase level;
    Vector3 spawnPosition = new Vector3(0, 0, 0);
    int pointsNeeded = 0;
    

    private void Start()
    {

        level = GameObject.Find("Level1Manager").GetComponent<LevelBase>();

        level.Setup(this);

        Debug.Log($"Points needed = ( {pointsNeeded} )");
    }

    public void spawn(GameObject prefab, Vector3 position)
    {
        pointsNeeded++;
        // Then, instantiate the prefab
        Instantiate(prefab, position, transform.rotation);
    }
    
}
