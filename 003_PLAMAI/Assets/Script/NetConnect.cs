using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetConnect : MonoBehaviour
{
    public GameObject Camera;
    public InputField IFName;
    string IFNameStr = "Player";
    public GameObject BtnGameStart;
    public GameObject BtnGameNetCoop;
    public GameObject BtnGameNetTime;
    public GameObject BtnGameNetChess;
    public GameObject BtnHost;
    public GameObject BtnClient;
    public InputField IFIP1;
    public InputField IFIP2;
    public InputField IFIP3;
    public InputField IFIP4;
    int IFIP1Num;
    int IFIP2Num;
    int IFIP3Num;
    int IFIP4Num;
    int IFIPPos = 1;
    bool IFIPFg = false;
    public InputField IFMsg;
    public Button BtnMsg;
    public Text TextRecieve;
    bool IPCheckFail = false;

    public void BtnHostClick()
    {
        SceneControl.GameNet = 1;
        IFName.interactable = false;
        if (IFName.text != "")
        {
            SceneControl.GameName = IFName.text;
            IFNameStr = IFName.text;
        }
        else
        {
            IFName.text = "Player";
            SceneControl.GameName = "Player";
            IFNameStr = "Player";
        }
        BtnGameStart.GetComponent<Button>().interactable = false;
        BtnHost.GetComponent<Button>().interactable = false;
        BtnClient.GetComponent<Button>().interactable = false;
        IFIP1.interactable = false;
        IFIP2.interactable = false;
        IFIP3.interactable = false;
        IFIP4.interactable = false;
        IFMsg.interactable = true;
        BtnMsg.interactable = true;
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                TextRecieve.text = "此設備可能IP位址: " + ip.ToString() + "\n" + TextRecieve.text;
            }
        }
        Camera.GetComponent<NetHost>().enabled = true;
    }

    public void BtnClientClick()
    {
        IPCheckFail = false;

        bool IFIP1Bool = int.TryParse(IFIP1.text, out IFIP1Num);
        if (!IFIP1Bool)
        {
            IFIP1.text = "127";
            IFIP1Num = 127;
            TextRecieve.text = "IP第一欄有誤\n"+TextRecieve.text;
            IPCheckFail = true;
        }
        else if (IFIP1Num>255 || IFIP1Num<0)
        {
            IFIP1.text = "127";
            IFIP1Num = 127;
            TextRecieve.text = "IP第一欄數值超出\n"+TextRecieve.text;
            IPCheckFail = true;
        }
        bool IFIP2Bool = int.TryParse(IFIP2.text, out IFIP2Num);
        if (!IFIP2Bool)
        {
            IFIP2.text = "0";
            IFIP2Num = 0;
            TextRecieve.text = "IP第二欄有誤\n"+TextRecieve.text;
            IPCheckFail = true;
        }
        else if (IFIP2Num>255 || IFIP2Num<0)
        {
            IFIP2.text = "0";
            IFIP2Num = 0;
            TextRecieve.text = "IP第二欄數值超出\n"+TextRecieve.text;
            IPCheckFail = true;
        }
        bool IFIP3Bool = int.TryParse(IFIP3.text, out IFIP1Num);
        if (!IFIP3Bool)
        {
            IFIP3.text = "0";
            IFIP3Num = 0;
            TextRecieve.text = "IP第三欄有誤\n"+TextRecieve.text;
            IPCheckFail = true;
        }
        else if (IFIP3Num>255 || IFIP3Num<0)
        {
            IFIP3.text = "0";
            IFIP3Num = 0;
            TextRecieve.text = "IP第三欄數值超出\n"+TextRecieve.text;
            IPCheckFail = true;
        }
        bool IFIP4Bool = int.TryParse(IFIP4.text, out IFIP4Num);
        if (!IFIP4Bool)
        {
            IFIP4.text = "1";
            IFIP4Num = 1;
            TextRecieve.text = "IP第四欄有誤\n"+TextRecieve.text;
            IPCheckFail = true;
        }
        else if (IFIP4Num>255 || IFIP4Num<0)
        {
            IFIP4.text = "1";
            IFIP4Num = 1;
            TextRecieve.text = "IP第四欄數值超出\n"+TextRecieve.text;
            IPCheckFail = true;
        }

        if (!IPCheckFail)
        {
            SceneControl.GameNet = 2;
            IFName.interactable = false;
            if (IFName.text != "")
            {
                SceneControl.GameName = IFName.text;
                IFNameStr = IFName.text;
            }
            else
            {
                IFName.text = "Player";
                SceneControl.GameName = "Player";
                IFNameStr = "Player";
            }
            BtnGameStart.GetComponent<Button>().interactable = false;
            NetClient.ipAdd = IFIP1.text+"."+IFIP2.text+"."+IFIP3.text+"."+IFIP4.text;
            BtnHost.GetComponent<Button>().interactable = false;
            BtnClient.GetComponent<Button>().interactable = false;
            IFIP1.interactable = false;
            IFIP2.interactable = false;
            IFIP3.interactable = false;
            IFIP4.interactable = false;
            IFMsg.interactable = true;
            BtnMsg.interactable = true;
            Camera.GetComponent<NetClient>().enabled = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        IFName.text = IFNameStr;
        IFIP1.ActivateInputField();
    }

    // Update is called once per frame
    void Update()
    {
        IFIPFg = true;

        if (Input.GetKeyUp("tab") && IFIPPos == 1 && IFIPFg)
        {
            IFIP1.ActivateInputField();
            IFIPPos = 2;
            IFIPFg = false;
        }
        if (Input.GetKeyUp("tab") && IFIPPos == 2 && IFIPFg)
        {
            IFIP2.ActivateInputField();
            IFIPPos = 3;
            IFIPFg = false;
        }
        if (Input.GetKeyUp("tab") && IFIPPos == 3 && IFIPFg)
        {
            IFIP3.ActivateInputField();
            IFIPPos = 4;
            IFIPFg = false;
        }
        if (Input.GetKeyUp("tab") && IFIPPos == 4 && IFIPFg)
        {
            IFIP4.ActivateInputField();
            IFIPPos = 1;
            IFIPFg = false;
        }

        if (SceneControl.GameNetFg)
        {
            BtnGameNetCoop.GetComponent<Button>().interactable = true;
            BtnGameNetTime.GetComponent<Button>().interactable = true;
            BtnGameNetChess.GetComponent<Button>().interactable = true;
            SceneControl.GameNetFg = false;
        }
    }
}
