using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowControl : MonoBehaviour
{
    int minWidth = 1578;
    int minHeight = 775;
    
    void Update()
    {
        MinimumWindowSize.Set(minWidth, minHeight);
    }

    private void OnApplicationQuit(){
        MinimumWindowSize.Reset();
    }
}
