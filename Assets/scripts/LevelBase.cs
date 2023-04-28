using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// LevelBase is an abstract class that serves as the base for different level classes. 
// It contains a virtual method named "Setup" that takes in a LevelHandler object as a parameter.
public abstract class LevelBase : MonoBehaviour
{
    // This virtual method can be overridden by a derived class to provide custom behavior.
    virtual public void Setup(LevelHandler handler) { }
}
