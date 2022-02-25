using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetConnect : MonoBehaviour
{
    public GameObject Camera;
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
    public InputField IFMsg;
    public Button BtnMsg;
    public Text TextRecieve;
    bool IPCheckFail = false;

    public void BtnHostClick()
    {
        Camera.GetComponent<NetHost>().enabled = true;
        BtnHost.GetComponent<Button>().interactable = false;
        BtnClient.GetComponent<Button>().interactable = false;
        IFIP1.interactable = false;
        IFIP2.interactable = false;
        IFIP3.interactable = false;
        IFIP4.interactable = false;
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
            Camera.GetComponent<NetClient>().enabled = true;
            NetClient.ipAdd = IFIP1.text+"."+IFIP2.text+"."+IFIP3.text+"."+IFIP4.text;
            BtnHost.GetComponent<Button>().interactable = false;
            BtnClient.GetComponent<Button>().interactable = false;
            IFIP1.interactable = false;
            IFIP2.interactable = false;
            IFIP3.interactable = false;
            IFIP4.interactable = false;
            IFMsg.interactable = true;
            BtnMsg.interactable = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
