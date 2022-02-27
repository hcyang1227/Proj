using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class NetClient : MonoBehaviour
{
    public Text TextRecieve;
    public InputField IFMsg;
    public Button BtnMsg;
    public static string ipAdd = "127.0.0.1";
    string editString=""; //編輯框文字

    public static Socket serverSocket; //伺服器端socket
    IPAddress ip; //主機ip
    IPEndPoint ipEnd;
    string TextRecieveStr;
    bool TextRecieveFg = false;
    public static string recvStr; //接收的字串
    public static string sendStr = ""; //傳送的字串
    byte[] recvData=new byte[2048]; //接收的資料，必須為位元組
    public static byte[] sendData=new byte[2048]; //傳送的資料，必須為位元組
    int recvLen; //接收的資料長度
    Thread connectThread; //連線執行緒

    //初始化
    void InitSocket()
    {
        //定義伺服器的IP和埠，埠與伺服器對應
        TextRecieveStr = "連結到"+ipAdd+":"+"5566"+"\n"+TextRecieveStr;
        TextRecieveFg = true;
        ip=IPAddress.Parse(ipAdd); //可以是區域網或網際網路ip，此處是本機
        ipEnd=new IPEndPoint(ip,5566);


        //開啟一個執行緒連線，必須的，否則主執行緒卡死
        connectThread=new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
    }

    void SocketConnet()
    {
        if(serverSocket!=null)
            serverSocket.Close();
        //定義套接字型別,必須在子執行緒中定義
        serverSocket=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        TextRecieveStr = "準備連線...\n"+TextRecieveStr;
        TextRecieveFg = true;
        //連線
        serverSocket.Connect(ipEnd);

        //輸出初次連線收到的字串
        recvLen=serverSocket.Receive(recvData);
        recvStr=Encoding.UTF8.GetString(recvData,0,recvLen);
        TextRecieveStr = "收到訊息: "+recvStr+"\n"+TextRecieveStr;
        TextRecieveFg = true;
        // SceneControl.GameNetFg = true;  //Client端不需要開啟按鈕功能
    }

    public static void SocketSend(string sendStrtmp)
    {
        //清空傳送快取
        sendData=new byte[2048];
        //資料型別轉換
        sendData=Encoding.UTF8.GetBytes(sendStrtmp);
        //傳送
        serverSocket.Send(sendData,sendData.Length,SocketFlags.None);
    }

    void SocketReceive()
    {
        SocketConnet();
        //不斷接收伺服器發來的資料
        while(true)
        {
            recvData=new byte[2048];
            recvLen=serverSocket.Receive(recvData);
            if(recvLen==0)
            {
                SocketConnet();
                continue;
            }
            recvStr=Encoding.UTF8.GetString(recvData,0,recvLen);
            TextRecieveStr = "收到訊息: "+recvStr+"\n"+TextRecieveStr;
            TextRecieveFg = true;
        }
    }

    void SocketQuit()
    {
        //關閉執行緒
        if(connectThread!=null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }
        //最後關閉伺服器
        if(serverSocket!=null)
            serverSocket.Close();
        TextRecieveStr = "連線斷開\n"+TextRecieveStr;
        TextRecieveFg = true;
    }

    // Use this for initialization
    void Start()
    {
        InitSocket();
        if (SceneControl.GameNet >= 1)
		    BtnMsg.onClick.AddListener(BtnClick);
    }

    void BtnClick()
    {
        editString = IFMsg.text;
        SocketSend(editString);
    }

    // Update is called once per frame
    void Update()
    {
        if (TextRecieveFg && SceneControl.GameNet >= 1)
        {
            TextRecieve.text = TextRecieveStr + TextRecieve.text;
            TextRecieveStr = "";
            TextRecieveFg = false;
        }
    }

    //程式退出則關閉連線
    void OnApplicationQuit()
    {
        SocketQuit();
    }
}