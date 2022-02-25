using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class NetHost : MonoBehaviour
{
    public Text TextRecieve;

    //以下預設都是私有的成員
    Socket serverSocket; //伺服器端socket
    Socket clientSocket; //客戶端socket
    IPEndPoint ipEnd; //偵聽埠
    int roomnum; //接收的字串
    string TextRecieveStr;
    bool TextRecieveFg = false;
    string recvStr; //接收的字串
    string sendStr; //傳送的字串
    byte[] recvData=new byte[1024]; //接收的資料，必須為位元組
    byte[] sendData=new byte[1024]; //傳送的資料，必須為位元組
    int recvLen; //接收的資料長度
    Thread connectThread; //連線執行緒

    //初始化
    void InitSocket()
    {
        //定義偵聽埠,偵聽任何IP
        ipEnd=new IPEndPoint(IPAddress.Any,5566);
        //定義套接字型別,在主執行緒中定義
        serverSocket=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        //連線
        serverSocket.Bind(ipEnd);
        //開始偵聽,最大10個連線
        serverSocket.Listen(10);

        //開啟一個執行緒連線，必須的，否則主執行緒卡死
        connectThread=new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
    }

    //連線
    void SocketConnet()
    {
        if(clientSocket!=null)
            clientSocket.Close();
        //控制檯輸出偵聽狀態
        TextRecieveStr = "已建立房號"+roomnum+"，等待客戶端連結...\n"+TextRecieveStr;
        TextRecieveFg = true;
        //一旦接受連線，建立一個客戶端
        clientSocket=serverSocket.Accept();
        //獲取客戶端的IP和埠
        IPEndPoint ipEndClient=(IPEndPoint)clientSocket.RemoteEndPoint;
        //輸出客戶端的IP和埠
        TextRecieveStr = "連結到位址"+ipEndClient.Address.ToString()+":"+ipEndClient.Port.ToString()+"\n"+TextRecieveStr;
        TextRecieveFg = true;
        //連線成功則傳送資料
        sendStr="歡迎來到房間"+roomnum.ToString();
        SocketSend(sendStr);
        TextRecieveStr = "您已連線成功\n"+TextRecieveStr;
        TextRecieveFg = true;
    }

    void SocketSend(string sendStr)
    {
        //清空傳送快取
        sendData=new byte[1024];
        //資料型別轉換
        sendData=Encoding.UTF8.GetBytes(sendStr);
        //傳送
        clientSocket.Send(sendData,sendData.Length,SocketFlags.None);
    }

    //伺服器接收
    void SocketReceive()
    {
        //連線
        SocketConnet();
        //進入接收迴圈
        while(true)
        {
            //對data清零
            recvData=new byte[1024];
            //獲取收到的資料的長度
            recvLen=clientSocket.Receive(recvData);
            //如果收到的資料長度為0，則重連並進入下一個迴圈
            if(recvLen==0)
            {
                SocketConnet();
                continue;
            }
            //輸出接收到的資料
            recvStr=Encoding.UTF8.GetString(recvData,0,recvLen);
            TextRecieveStr = "收到訊息: "+recvStr+"\n"+TextRecieveStr;
            TextRecieveFg = true;
            //將接收到的資料經過處理再發送出去
            sendStr=recvStr;
            SocketSend(sendStr);
        }
    }

    //連線關閉
    void SocketQuit()
    {
        //先關閉客戶端
        if(clientSocket!=null)
            clientSocket.Close();
        //再關閉執行緒
        if(connectThread!=null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }
        //最後關閉伺服器
        if(serverSocket!=null)
        {
            serverSocket.Close();
            TextRecieveStr = "連線斷開\n"+TextRecieveStr;
            TextRecieveFg = true;
        }
    }

    // Use this for initialization
    void Start()
    {
        System.Random rand = new System.Random(Guid.NewGuid().GetHashCode());
        roomnum = rand.Next(1000,9999);
        InitSocket(); //在這裡初始化server
    }


    // Update is called once per frame
    void Update()
    {
        if (TextRecieveFg)
        {
            TextRecieve.text = TextRecieveStr + TextRecieve.text;
            TextRecieveStr = "";
            TextRecieveFg = false;
        }
    }

    void OnApplicationQuit()
    {
        SocketQuit();
    }
}