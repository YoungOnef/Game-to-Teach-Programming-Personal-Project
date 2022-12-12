using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Level1 level;

    private void Awake()
    {
        level = GameObject.FindGameObjectWithTag("GameController").GetComponent<Level1>();
    }
    void OnTriggerEnter(Collider other)
    {
        print("coin collided");
        level.Coin(other);
    }
}
