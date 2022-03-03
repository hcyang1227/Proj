using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public GameObject Camera;
    public static int GameNet = 0; //遊戲連結角色，0:無 1:伺服端 2:客戶端
    public static int GameMode = 0; //遊戲模式，0:單人遊戲 1:合作模式 2:對戰模式(時間) 3:對戰模式(對弈)
    public static bool GameNetFg = false; //遊戲是否連線成功的flag
    public static bool GameStartFg = false; //遊戲是否開始的flag
    public static string GameName = "Player"; //玩家1的名字
    public static string GameName2 = ""; //玩家2的名字

    void Start()
    {

    }

    public void GameStart()
    {
        GameMode = 0;
        GameStartFg = true;
        SceneManager.LoadScene("MainGame");
    }

    public void GameNetCoop()
    {
        GameMode = 1;
        GameStartFg = true;
        if (SceneControl.GameNet == 1)
        {
            NetHost.StringIntegrate("<<合作模式>>;PlayerName,"+GameName+";");
        }
        if (SceneControl.GameNet == 2)
        {
            NetClient.StringIntegrate("PlayerName,"+GameName+";");
        }
        DontDestroyOnLoad(Camera);
        SceneManager.LoadScene("MainGame");
    }

    public void GameNetTime()
    {
        GameMode = 2;
        GameStartFg = true;
        if (SceneControl.GameNet == 1)
        {
            NetHost.StringIntegrate("<<競速模式>>;PlayerName,"+GameName+";");
        }
        if (SceneControl.GameNet == 2)
        {
            NetClient.StringIntegrate("PlayerName,"+GameName+";");
        }
        DontDestroyOnLoad(Camera);
        SceneManager.LoadScene("MainGame");
    }

    public void GameNetChess()
    {
        GameMode = 3;
        GameStartFg = true;
        if (SceneControl.GameNet == 1)
        {
            NetHost.StringIntegrate("<<對弈模式>>;PlayerName,"+GameName+";");
        }
        if (SceneControl.GameNet == 2)
        {
            NetClient.StringIntegrate("PlayerName,"+GameName+";");
        }
        DontDestroyOnLoad(Camera);
        SceneManager.LoadScene("MainGame");
    }

    void Update()
    {
        if (SceneControl.GameNet == 2 && NetClient.recvStrPcs == "<<合作模式>>")
            GameNetCoop();

        if (SceneControl.GameNet == 2 && NetClient.recvStrPcs == "<<競速模式>>")
            GameNetTime();

        if (SceneControl.GameNet == 2 && NetClient.recvStrPcs == "<<對弈模式>>")
            GameNetChess();
    }
}
