using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoadGame : MonoBehaviour
{
    HeroKnight _HeroKnight;
    Dialog _Dialog;
    RandomPlatform _RandomPlatform;
    public GameObject Platform;
    public GameObject Eagles;
    public GameObject Eagle;
    public GameObject Eagle0;
    public GameObject Eagle1;
    public GameObject Eagle2;
    public GameObject GroundGrid;
    public GameObject LoadGameFlag;
    void Start()
    {
        _HeroKnight = GameObject.Find("HeroKnight").GetComponent<HeroKnight>();
        _Dialog = GameObject.Find("DialogPlatform").GetComponent<Dialog>();
        _RandomPlatform = GameObject.Find("GroundGrid").GetComponent<RandomPlatform>();
    }

    // Update is called once per frame
    float autosave = 10.0f;
    void Update()
    {
        //假設開頭選取讀檔，則讀取json檔
        if ( GameObject.Find("有讀檔") == true)
        {
            load();
            GameObject.Find("有讀檔").name = "沒讀檔";
        }

        //10秒自動存檔機制
        if (autosave > 0 && GameObject.Find("有讀檔") == false)
            autosave -= Time.deltaTime;
        else if (autosave > 0 && GameObject.Find("有讀檔") == true)
            autosave = 10.0f;
        else
        {
            autosave = 10.0f;
            save();
            print("10秒自動儲存");
        }
    }

    public class playerState
    {
        public float m_HP;
        public int m_Money;
        public int m_Potion;
        public int m_facingDirection;
        public Vector3 m_position;
        public int gameHistory;
        public bool SrchCrate;
        public Vector3[] m_platform;
        public Vector3[] m_enemy;
        public float[] m_HPs;
    }

    public void save()
    {
        //填寫playerState格式的資料
        playerState myPlayer = new playerState();
        myPlayer.m_HP = _HeroKnight.m_HP;
        myPlayer.m_Money = _HeroKnight.m_Money;
        myPlayer.m_Potion = _HeroKnight.m_Potion;
        myPlayer.m_facingDirection = _HeroKnight.m_facingDirection;
        myPlayer.m_position = _HeroKnight.transform.position;
        myPlayer.gameHistory = _Dialog.gameHistory;
        myPlayer.SrchCrate = _Dialog.SrchCrate;
        //將myPlayer轉換成json格式的字串
        string saveString1 = JsonUtility.ToJson(myPlayer);

        playerState PlatformCrt = new playerState();
        PlatformCrt.m_platform = new Vector3[100];
        for(int i = 0; i < 100; i++)
        {
            if (GameObject.Find("Platform"+i) != null)
                PlatformCrt.m_platform[i] = GameObject.Find("Platform"+i).transform.position;
            else
                PlatformCrt.m_platform[i] = new Vector3(0,0,0);
        }
        string saveString2 =  JsonUtility.ToJson(PlatformCrt);

        playerState EnemyCrt = new playerState();
        EnemyCrt.m_enemy = new Vector3[30];
        EnemyCrt.m_HPs = new float[30];
        for(int j = 0; j < 30; j++)
        {
            if (GameObject.Find("Eagle"+j) != null)
            {
                EnemyCrt.m_enemy[j] = GameObject.Find("Eagle"+j).transform.position;
                EnemyCrt.m_HPs[j] = GameObject.Find("Eagle"+j).GetComponent<TrigEagle>().m_HP;
            }
            else
            {
                EnemyCrt.m_enemy[j] = new Vector3(0,0,0);
                EnemyCrt.m_HPs[j] = 0.0f;
            }
        }
        string saveString3 = JsonUtility.ToJson(EnemyCrt);

        //將此字串存到硬碟中
        StreamWriter file1 = new StreamWriter(System.IO.Path.Combine(Application.dataPath, "myPlayer"));
        file1.Write(saveString1);
        file1.Close();
        StreamWriter file2 = new StreamWriter(System.IO.Path.Combine(Application.dataPath, "Platform"));
        file2.Write(saveString2);
        file2.Close();
        StreamWriter file3 = new StreamWriter(System.IO.Path.Combine(Application.dataPath, "Enemy"));
        file3.Write(saveString3);
        file3.Close();
    }

    public void load()
    {
        print("讀取檔案1中...");
        //讀取HeroKnight的json檔並轉換為文字格式
        StreamReader file1 = new StreamReader(System.IO.Path.Combine(Application.dataPath, "myPlayer"));
        string loadJson1 = file1.ReadToEnd();
        file1.Close();
        //新增一個物件類型為playerState的變數loadData
        playerState loadData1 = new playerState();
        //使用JsonUtility的FromJason方法將文字轉成Json
        loadData1 = JsonUtility.FromJson<playerState>(loadJson1);
        _HeroKnight.m_HP = loadData1.m_HP;
        _HeroKnight.m_Money = loadData1.m_Money;
        _HeroKnight.m_Potion = loadData1.m_Potion;
        _HeroKnight.m_facingDirection = loadData1.m_facingDirection;
        _HeroKnight.transform.position = loadData1.m_position;
        _Dialog.gameHistory = loadData1.gameHistory;
        _Dialog.SrchCrate = loadData1.SrchCrate;

        print("讀取檔案2中...");
        //讀取平台的json檔並轉換為文字格式
        StreamReader file2 = new StreamReader(System.IO.Path.Combine(Application.dataPath, "Platform"));
        string loadJson2 = file2.ReadToEnd();
        file2.Close();
        //新增一個物件類型為playerState的變數loadData
        playerState loadData2 = new playerState();
        //使用JsonUtility的FromJason方法將文字轉成Json
        loadData2 = JsonUtility.FromJson<playerState>(loadJson2);
        int i = 0;
        foreach(Vector3 pltfrm in loadData2.m_platform)
        {
            if(loadData2.m_platform[i] != new Vector3(0,0,0))
            {
                GameObject Clone;
                Clone = Instantiate(Platform, loadData2.m_platform[i], Platform.transform.rotation, GroundGrid.transform);
                Clone.name = "Platform"+i;               
            }
            i++;
        }

        print("讀取檔案3中...");
        Destroy(Eagle0);
        Destroy(Eagle1);
        Destroy(Eagle2);
        //讀取敵人的json檔並轉換為文字格式
        StreamReader file3 = new StreamReader(System.IO.Path.Combine(Application.dataPath, "Enemy"));
        string loadJson3 = file3.ReadToEnd();
        file3.Close();
        //新增一個物件類型為playerState的變數loadData
        playerState loadData3 = new playerState();
        //使用JsonUtility的FromJason方法將文字轉成Json
        loadData3 = JsonUtility.FromJson<playerState>(loadJson3);
        int j = 0;
        foreach(Vector3 enm in loadData3.m_enemy)
        {
            if(loadData3.m_enemy[j] != new Vector3(0,0,0))
                {
                GameObject Clone;
                Clone = Instantiate(Eagle, loadData3.m_enemy[j], Eagle.transform.rotation, Eagles.transform);
                Clone.GetComponent<TrigEagle>().m_HP = loadData3.m_HPs[j];
                Clone.name = "Eagle"+j;
                }
            j++;
        }
    }
}
