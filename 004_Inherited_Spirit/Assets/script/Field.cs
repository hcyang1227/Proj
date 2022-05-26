using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{

    public GameObject Background;

    public static bool battle_flag = true; //目前在戰鬥模式(true)還是走路模式(false)
    public static bool battle_mode = true; //目前在平常模式(false)還是副本模式(true)
    float SysFrame = 0f; //系統絕對幀數(浮點數)
    public static int SysFrameCount = 0; //系統絕對幀數(整數)
    int SysFrameCountPre = 0; //系統先前絕對幀數(整數)
    public static bool frame_unchange = false; //是否在同一幀(畫面有無改變)，是則為true


    float bg_speed = 0.1f; //背景移動的速度

    void Start()
    {
    }

    void Update()
    {
        //系統定義FPS為60
        SysFrame += Time.deltaTime * 60f;
        SysFrameCount = (int)SysFrame;
        if (SysFrameCount == SysFrameCountPre)
            frame_unchange = true;
        else
            frame_unchange = false;
        SysFrameCountPre = SysFrameCount;

        //若非戰鬥模式，則背景自動向左移動，超過某個數值就將背景圖向前位移
        if (!battle_flag && !frame_unchange)
        {
            Background.transform.position += new Vector3(-bg_speed, 0f, 0f);
            if ((Background.transform.position.x < -800))
                Background.transform.position += new Vector3(Background.GetComponent<SpriteRenderer>().size.x*140/2, 0f, 0f);
        }

    }


}
