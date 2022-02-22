using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowControl : MonoBehaviour
{
    int minWidth = 394;
    int minHeight = 238;
    
    void Update()
    {
        MinimumWindowSize.Set(minWidth, minHeight);
    }

    private void OnApplicationQuit(){
        MinimumWindowSize.Reset();
    }
}
