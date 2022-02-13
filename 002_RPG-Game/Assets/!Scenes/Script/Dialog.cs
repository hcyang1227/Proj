using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System;

public class Dialog : MonoBehaviour
{
    public int[] id;
    public int[] next;
    public string[] nametext;
    public string[] text;
    public int arrayCount;
    public bool speak = false;
    public int Count = 1;
    public int ID = 0;
    HeroKnight _HeroKnight;
    public GameObject Dialog1;
    public GameObject DlHero;
    public GameObject DlVilliger;
    public GameObject DlTxtName;
    public GameObject DlTxtText;
    public int gameHistory = 0;
    public int Monster;
    int count = 0;
    public bool SrchCrate = false;
    bool MonsterCount = false;
    void Start()
    {
        _HeroKnight = GameObject.Find("HeroKnight").GetComponent<HeroKnight>();
        LoadXml("/XmlText.xml");
        Dialog1.SetActive(false);
        
    }
    void Update()
    {
        //計算場上共有多少怪物
        if(MonsterCount == false)
        {
            Monster = GameObject.FindGameObjectsWithTag("Monster").Length;
            MonsterCount = true;
            // print("怪物一共有"+Monster+"隻");
        }

        if (gameHistory == 2 &&　speak == false)
        {
            EditorSceneManager.LoadScene("TitleScene");
        }

        if (Input.GetKeyDown("delete"))
        {
            Monster = 0;
        }


        if (speak == true)
        {
            _HeroKnight.enabled = false;
            if (Input.GetKeyDown("space") && next[count] != -1)
            {
                EventGetItem();
                ID = next[count];
                StartDialog(next[count]);
            }
            else if (Input.GetKeyDown("space") && next[count] == -1)
            {
                EventGetItem();
                ID = -1;
                speak = false;
                Dialog1.SetActive(false);
            }
            else if (ID==2004 && Input.GetKeyDown("y") && _HeroKnight.m_Money >= 100)
            {
                _HeroKnight.m_Money -= 100;
                _HeroKnight.m_Potion += 1;
                StartDialog(2005);
            }
            else if (ID==2004 && Input.GetKeyDown("y") && _HeroKnight.m_Money < 100)
            {
                StartDialog(2006);
            }
            else if (ID==2004 && Input.GetKeyDown("n"))
            {
                StartDialog(2007);
            }
        }
        else
        {
            _HeroKnight.enabled = true;
        }
    }

    void EventGetItem()
    {
        if (ID==1012) {_HeroKnight.m_Potion+=1;}
        if (ID==10002) {_HeroKnight.m_Money+=300;}
    }

    void StartDialog(int ID)
    {
        count = Array.IndexOf(id,ID);

        DlTxtName.GetComponent<Text>().text = nametext[count];
        DlTxtText.GetComponent<Text>().text = text[count];

        if (nametext[count] == "勇者")
            DlHero.SetActive(true);
        else
            DlHero.SetActive(false);

        if (nametext[count] == "村民")
            DlVilliger.SetActive(true);
        else
            DlVilliger.SetActive(false);     
    }
    public void OpenDialog(string obj)
    {
        if ((Input.GetKeyDown("up") || Input.GetKeyDown("w")) && speak == false)
        {
            if (obj == "Villiger" && gameHistory == 1 && Monster <= 0)
            {
                Dialog1.SetActive(true);
                speak = true;
                gameHistory = 2;
                StartDialog(3000);
            }
            if (obj == "Villiger" && gameHistory == 1)
            {
                Dialog1.SetActive(true);
                speak = true;
                StartDialog(2000);
            }
            if (obj == "Villiger" && gameHistory == 0)
            {
                Dialog1.SetActive(true);
                speak = true;
                gameHistory = 1;
                StartDialog(1000);
            }
            if (obj == "Board")
            {
                Dialog1.SetActive(true);
                speak = true;
                StartDialog(10000);
            }
            if (obj == "Crate" && SrchCrate == true)
            {
                Dialog1.SetActive(true);
                speak = true;
                StartDialog(10003);
            }
            if (obj == "Crate" && SrchCrate == false)
            {
                SrchCrate = true;
                Dialog1.SetActive(true);
                speak = true;
                StartDialog(10001);
            }
        }
    }
    public void LoadXml(string xmlText)
    {
        XmlDocument xml = new XmlDocument();
        xml.Load(Application.dataPath + xmlText);
        XmlElement root = xml.DocumentElement;

        XmlNodeList node = xml.SelectSingleNode("root").ChildNodes;
        arrayCount = node.Count+1;
        id = new int[arrayCount];
        next = new int[arrayCount];
        nametext = new string[arrayCount];
        text = new string[arrayCount];

        foreach (XmlElement a in node)
        {
            if (a.Name == "Sheet1")
            {
                if (a.Attributes["ID"] != null)
                    id[Count] = int.Parse(a.Attributes["ID"].Value);
                else
                    id[Count] = -1;
                if (a.Attributes["NEXT"] != null)
                    next[Count] = int.Parse(a.Attributes["NEXT"].Value);
                else
                    next[Count] = -1;
                if (a.Attributes["Name"] != null)
                    nametext[Count] = a.Attributes["Name"].Value;
                else
                    nametext[Count] = "";
                if (a.Attributes["text"] != null)
                    text[Count] = a.Attributes["text"].Value;
                else
                    text[Count] = "";

                Count++;
            }
        }


    }


}
