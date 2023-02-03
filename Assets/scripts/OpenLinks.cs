using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class OpenLinks : MonoBehaviour
{
    public static void OpenURL(string url)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        OpenURL(url);
#endif
    }

    [DllImport("__Internal")]
    private static extern void OpenTab(string url);


}
