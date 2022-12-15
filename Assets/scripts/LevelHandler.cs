using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    LevelBase level;
    Vector3 spawnPosition = new Vector3(0, 0, 0);

    private void Start()
    {

        level = GameObject.Find("china").GetComponent<LevelBase>();

        level.Setup(this);
    }

    public void spawn(GameObject prefab, Vector3 position)
    {
        
        // Then, instantiate the prefab
        Instantiate(prefab, position, transform.rotation);
    }
    
}
