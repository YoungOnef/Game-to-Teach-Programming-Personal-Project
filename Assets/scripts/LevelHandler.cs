using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    LevelBase level;
    Vector3 spawnPosition = new Vector3(0, 0, 0);
    int pointsNeeded = 0;
    public int coinCount = 0; // Player's coin count

    private void Start()
    {
        level = GameObject.Find("LevelManager").GetComponent<LevelBase>();
        level.Setup(this);
        Debug.Log($"Points needed = ( {pointsNeeded} )");
    }

    public void spawn(GameObject prefab, Vector3 position)
    {
        pointsNeeded++;
        // Then, instantiate the prefab
        Instantiate(prefab, position, transform.rotation);
    }

    public bool AllPointsCollected()
    {
        return pointsNeeded == coinCount;
    }

}
