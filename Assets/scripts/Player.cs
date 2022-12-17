using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] ScriptManager level;

    bool isColliding;

    private void Awake()
    {
        level = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScriptManager>();
        
    }
    void OnTriggerEnter(Collider other)
    {

        if (isColliding) return;
        isColliding = true;

        if (other.tag == "Coin")
        {
            level.Coin(other);
            Destroy(other.gameObject);
        }

        StartCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        yield return new WaitForEndOfFrame();
        isColliding = false;
    }
}
