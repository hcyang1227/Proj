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
    public static string strAryTotal = ""; //String Array Total，將接收到的對方指令集結成同一個string，右進左出
    public static string recvStrPcs = "";
    string editString=""; //編輯框文字

    public static Socket serverSocket; //伺服器端socket
    IPAddress ip; //主機ip
    IPEndPoint ipEnd;
    string TextRecieveStr;
    public static string recvStr = ""; //接收的字串
    public static string sendStr = ""; //傳送的字串
    byte[] recvData=new byte[8192]; //接收的資料，必須為位元組
    byte[] sendData=new byte[8192]; //傳送的資料，必須為位元組
    int recvLen; //接收的資料長度
    Thread connectThread; //連線執行緒

    public static bool connectState;

    //初始化
    void InitSocket()
    {
        //定義伺服器的IP和埠，埠與伺服器對應
        TextRecieveStr = "連結到"+ipAdd+":"+"5566"+"\n"+TextRecieveStr;
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
        //連線
        try
        {
            serverSocket.Connect(ipEnd);
        }
        catch (SocketException e)
        {
            if (e.NativeErrorCode.Equals(10035))
            {
                connectState = true;
                recvStr=recvStr+"Alarm,0,10035,仍然在連線中，但送出資料被擋住;";
            }
            else
            {
                connectState = false;
                recvStr=recvStr+"Alarm,0,"+e.NativeErrorCode.ToString()+","+e.Message.Replace("\n","").Replace("\0","")+";";
            }
        }
        // SceneControl.GameNetFg = true;  //Client端不需要開啟按鈕功能
    }

    public static void StringIntegrate(string sendStrTmp)
    {
        //判斷歷史傳輸數據是否有值
        if (sendStr == "")
            sendStr = sendStrTmp;
        else
            sendStr = sendStr + sendStrTmp;
        Debug.Log("sendStr = " + sendStr);
    }

    void SocketSend()
    {
        //清空傳送快取
        sendData=new byte[8192];
        //資料型別轉換
        sendData=Encoding.UTF8.GetBytes(sendStr);
        //傳送
        try
        {
            serverSocket.Send(sendData,sendData.Length,SocketFlags.None);
        }
        catch (SocketException e)
        {
            if (e.NativeErrorCode.Equals(10035))
            {
                connectState = true;
                recvStr=recvStr+"Alarm,0,10035,仍然在連線中，但送出資料被擋住;";
            }
            else
            {
                connectState = false;
                recvStr=recvStr+"Alarm,0,"+e.NativeErrorCode.ToString()+","+e.Message.Replace("\n","").Replace("\0","")+";";
            }
        }
    }

    void SocketReceive()
    {
        SocketConnet();
        //不斷接收伺服器發來的資料
        while(true)
        {
            recvData=new byte[8192];
            try
            {
                recvLen=serverSocket.Receive(recvData);
            }
            catch (SocketException e)
            {
                if (e.NativeErrorCode.Equals(10035))
                {
                    connectState = true;
                    recvStr=recvStr+"Alarm,0,10035,仍然在連線中，但送出資料被擋住;";
                }
                else
                {
                    connectState = false;
                    recvStr=recvStr+"Alarm,0,"+e.NativeErrorCode.ToString()+","+e.Message.Replace("\n","").Replace("\0","")+";";
                }
            }
            if(recvLen==0)
            {
                SocketConnet();
                continue;
            }
            //輸出接收到的資料
            recvStr=recvStr+Encoding.UTF8.GetString(recvData,0,recvLen);
            Debug.Log("recvStr= " + recvStr);
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
        editString = IFMsg.text+";";
        StringIntegrate(editString);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneControl.GameNet == 2 && (strAryTotal != "" || recvStr != ""))
        {
            strAryTotal = strAryTotal + recvStr;
            string[] strAryLarge = strAryTotal.Split(';');
            recvStr = "";
            recvStrPcs = strAryLarge[0];
            TextRecieveStr = "收到訊息: "+recvStrPcs+"\n"+TextRecieveStr;
            if (strAryTotal.Length > 0 && strAryLarge.Length > 2)
                strAryTotal = strAryTotal.Remove(0,strAryLarge[0].Length+1);
            else
                strAryTotal = "";
            Debug.Log("strAryTotal= " + strAryTotal);
        }

        if (SceneControl.GameNet >= 1 && TextRecieveStr != "" && !SceneControl.GameStartFg)
        {
            TextRecieve.text = TextRecieveStr + TextRecieve.text;
            TextRecieveStr = "";
        }

        if (sendStr != "" && serverSocket!=null && serverSocket.Connected)
        {
            SocketSend();
            sendStr = "";
            connectState = true;
        }
        else if (connectState && serverSocket==null)
        {
            recvStr=recvStr+"Alarm,0,400,serverSocket為空值;";
            connectState = false;
        }
        else if (connectState && !serverSocket.Connected)
        {
            recvStr=recvStr+"Alarm,0,404,serverSocket未連線或斷開連線...;";
            connectState = false;
        }

    }

    //程式退出則關閉連線
    void OnApplicationQuit()
    {
        SocketQuit();
    }
}