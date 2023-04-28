using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class OpenLinks : MonoBehaviour
{
    // This static method takes a URL as a parameter and opens it in a new tab in WebGL builds
    public static void OpenURL(string url)
    {
        // The code in this block will only be executed in WebGL builds, not in the Unity Editor or other platforms
#if !UNITY_EDITOR && UNITY_WEBGL
        OpenTab(url);
#endif
    }

    // This method is declared using the DllImport attribute and is used to call a function in a native library
    [DllImport("__Internal")]
    private static extern void OpenTab(string url);


}
