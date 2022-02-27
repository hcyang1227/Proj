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

    void Start()
    {

    }

    public void GameStart()
    {
        GameMode = 0;
        NetHost.recvStr = "";
        NetClient.recvStr = "";
        SceneManager.LoadScene("MainGame");
    }

    public void GameNetCoop()
    {
        GameMode = 1;
        NetHost.recvStr = "";
        NetClient.recvStr = "";
        if (SceneControl.GameNet == 1)
        {
            NetHost.SocketSend("<<合作模式>>");
        }
        SceneManager.LoadScene("MainGame");
    }

    public void GameNetTime()
    {
        GameMode = 2;
        NetHost.recvStr = "";
        NetClient.recvStr = "";
        if (SceneControl.GameNet == 1)
        {
            NetHost.SocketSend("<<速度對戰>>");
        }
        SceneManager.LoadScene("MainGame");
    }

    public void GameNetChess()
    {
        GameMode = 3;
        NetHost.recvStr = "";
        NetClient.recvStr = "";
        if (SceneControl.GameNet == 1)
        {
            NetHost.SocketSend("<<對弈模式>>");
        }
        SceneManager.LoadScene("MainGame");
    }

    void Update()
    {
        //如果腳本沒有開啟則重新開啟。但是發現不用重新開啟，已開啟的程式碼也會繼續跑，故改為註解。
        // if (SceneControl.GameNet == 1 && !Camera.GetComponent<NetHost>().enabled)
        //     Camera.GetComponent<NetHost>().enabled = true;

        // if (SceneControl.GameNet == 2 && !Camera.GetComponent<NetClient>().enabled)
        //     Camera.GetComponent<NetClient>().enabled = true;

        if (SceneControl.GameNet == 2 && NetClient.recvStr == "<<合作模式>>")
            GameNetCoop();

        if (SceneControl.GameNet == 2 && NetClient.recvStr == "<<速度對戰>>")
            GameNetTime();

        if (SceneControl.GameNet == 2 && NetClient.recvStr == "<<對弈模式>>")
            GameNetChess();
    }
}
