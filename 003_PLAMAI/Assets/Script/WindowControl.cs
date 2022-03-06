using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public class WindowControl : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    int minWidth = 394;
    int minHeight = 238;
    bool WinMinimized = false;
    bool WinMiniFg = false;

    void Update()
    {
        MinimumWindowSize.Set(minWidth, minHeight);

        WinMiniFg = false;

        if (Input.GetKeyUp(KeyCode.LeftAlt) && !WinMinimized && !WinMiniFg)
        {
            ShowWindow(GetForegroundWindow(), 2); //啟用最小化
            WinMinimized = true;
            WinMiniFg = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt) && WinMinimized && !WinMiniFg)
        {
            ShowWindow(GetForegroundWindow(), 1); //啟用還原
            WinMinimized = false;
            WinMiniFg = true;
        }
    }

    private void OnApplicationQuit(){
        MinimumWindowSize.Reset();
    }
}
