using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Stage : MonoBehaviour
{
    public GameObject Btn;
    public GameObject BtnZone;
    public GameObject Canvas;
    public InputField InputFieldX;
    public InputField InputFieldY;
    public InputField InputFieldMinePlus;
    public InputField InputFieldMineMinus;
    public GameObject GameObjectX;
    public GameObject GameObjectY;
    public GameObject GameObjectMinePlus;
    public GameObject GameObjectMineMinus;
    public GameObject BtnCrtStage;
    public GameObject BtnCheckAns;
    public Text uiText;
    public Text uiTextMine;
    public GameObject Frame;
    public Animator FrameAni;
    public int[,] sgmine = new int[100,100]; //stage mine，1:正電地雷 -1:負電地雷
    public int[,] sgmine2 = new int[100,100]; //stage mine 2，記憶附近是否有地雷，0:沒有 1:有
    public int[,] sgnum = new int[100,100]; //stage number，9:正電地雷 -9:負電地雷 0:附近沒地雷 其他:附近有幾個地雷(正負相消)
    public int[,] sgmap = new int[100,100]; //stage map，0:未開 1:剛踩探測是否為0 2:確認為0、其他數字或地雷
    public int[,] sgopnbtn = new int[100,100]; //stage open button，記憶可以開啟的複數位置，0:沒有 1:有
    public int StageNumX;
    public int StageNumY;
    public int StageNumMPlus;
    public int StageNumMMinus;
    public int GuessMPlus = 0;
    public int GuessMMinus = 0;
    public int stagephase = 0;
    public int SelectX;
    public int SelectY;
    public int ScnX;
    public int ScnY;
    public static int HoverX;
    public static int HoverY;
    public static bool MouseDownOnBtn = false;
    bool MouseDownLeft = false;
    bool MouseDownRight = false;
    bool KeyDownLeft = false;
    bool KeyDownRight = false;
    bool CreateBtnFlag = false;
    bool ContinueZero = false;
    int WrongAns = 0;
    bool ControllerEnable = true;
    float KeyUpTm = 0f;
    float KeyDownTm = 0f;
    float KeyLeftTm = 0f;
    float KeyRightTm = 0f;
    float KeyUpTm2 = 0.15f;
    float KeyDownTm2 = 0.15f;
    float KeyLeftTm2 = 0.15f;
    float KeyRightTm2 = 0.15f;
    bool KeyUpFg = false;
    bool KeyDownFg = false;
    bool KeyLeftFg = false;
    bool KeyRightFg = false;
    bool KeySpaceFg = false;
    bool KeyActiveOnce = false;
    public int FramePos = 1; //Frame的位置，0:棋盤格位置 1:右上設置 2:右下設置
    public int FramePos2 = 2; //Frame的右上位置，0:X格數 1:Y格數 2:創建關卡 3:正電地雷 4:負電地雷
    public int FramePosX = -1;
    public int FramePosY = -1;

    void Start()
    {
        //顯示開啟時的視窗大小
        uiText.text = "視窗大小:\nWidth=" + Screen.width + ", Height=" + Screen.height + "\n" + uiText.text;
        BtnCheckAns.GetComponent<Button>().interactable = false;
        uiTextMine.text = "<color=red>正電地雷</color>: 設置"+"？"+" 預測"+"？"+"\n<color=cyan>負電地雷</color>: 設置"+"？"+" 預測"+"？";
        Frame.transform.position = BtnCrtStage.transform.position;
        FrameAni.Play("FrameL");
        EventSystem.current.SetSelectedGameObject(BtnCrtStage, null);
        KeyActiveOnce = true;
    }

    public void SetController(bool flag)
    {
        GameObjectX.SetActive(flag);
        GameObjectY.SetActive(flag);
        BtnCrtStage.SetActive(flag);
        GameObjectMinePlus.SetActive(flag);
        GameObjectMineMinus.SetActive(flag);
        ControllerEnable = flag;
    }

    public void CrtBtn()
    {
        //回歸關卡階段到0，其餘回歸預設值
        stagephase = 0;
        WrongAns = 0;
        GuessMPlus = 0;
        GuessMMinus = 0;
        SetController(true);
        uiText.text = "<color=magenta>創建關卡</color>\n視窗大小:\nWidth=" + Screen.width + ", Height=" + Screen.height;

        //移除所有已創建的Btn
        for (int i = 1; i <= 100; i++)
        {
            for (int j = 1; j <= 100; j++)
            {
                Destroy(GameObject.Find("Btn" + i + "-" + j));
                sgmine[i-1,j-1] = 0;
                sgmine2[i-1,j-1] = 0;
                sgnum[i-1,j-1] = 0;
                sgmap[i-1,j-1] = 0;
                sgopnbtn[i-1,j-1] = 0;
            }
        }

        //Btn陣列的X向個數
        bool StageBoolX = int.TryParse(InputFieldX.text, out StageNumX);
        if (StageBoolX == false)
        {
            InputFieldX.text = "15";
            StageNumX = 15;
        }
        if (StageNumX > 30)
        {
            InputFieldX.text = "30";
            StageNumX = 30;
        }
        if (StageNumX < 5)
        {
            InputFieldX.text = "5";
            StageNumX = 5;
        }

        //Btn陣列的Y向個數
        bool StageBoolY = int.TryParse(InputFieldY.text, out StageNumY);
        if (StageBoolY == false)
        {
            InputFieldY.text = "15";
            StageNumY = 15;
        }
        if (StageNumY > 25)
        {
            InputFieldY.text = "25";
            StageNumY = 25;
        }
        if (StageNumY < 5)
        {
            InputFieldY.text = "5";
            StageNumY = 5;
        }

        //呼叫CrtBtnLp功能創建Btn陣列
        for (int i = 1; i <= StageNumX; i++)
        {
            for (int j = 1; j <= StageNumY; j++)
            {
                CrtBtnLp(i,j);
            }
        }

        //豎起創建Btn的旗幟
        CreateBtnFlag = true;

        //將Frame移到創建關卡
        FramePosX = 0;
        FramePosY = 0;
        FramePos = 1;
        FramePos2 = 2;
        Frame.transform.position = BtnCrtStage.transform.position;
        FrameAni.Play("FrameL");
        EventSystem.current.SetSelectedGameObject(BtnCrtStage, null);
    }


    public void CrtBtnLp(int i, int j)
    {
        Vector3 myVector = Canvas.transform.position + new Vector3(30f*i-(StageNumX+1)*15f, (StageNumY+1)*15f-j*30f, 0);
        GameObject Clone;
        Clone = (GameObject)Instantiate(Btn, myVector, new Quaternion(), BtnZone.transform);
        Clone.GetComponent<Image>().color = Color.white;
        Clone.name = "Btn" + i + "-" + j;
    }

    public void BtnPress(int X, int Y)
    {
        SelectX = X;
        SelectY = Y;
        //當所有Btn按鈕都未被按的時候，第一個Btn按下必不為地雷，同時設定所有地雷位置
        if (stagephase == 0)
        {
            bool StageBoolMPlus = int.TryParse(InputFieldMinePlus.text, out StageNumMPlus);
            bool StageBoolMMinus = int.TryParse(InputFieldMineMinus.text, out StageNumMMinus);
            BtnCheckAns.GetComponent<Button>().interactable = true;
            FramePos = 0;
            KeyActiveOnce = true;

            //正負地雷數量設定
            if (StageBoolMPlus == false)
            {
                InputFieldMinePlus.text = "0";
                StageNumMPlus = 0;
            }
            if (StageBoolMMinus == false)
            {
                InputFieldMineMinus.text = "0";
                StageNumMMinus = 0;
            }
            if (StageNumMPlus + StageNumMMinus < 3)
            {
                StageNumMPlus = 3 - StageNumMMinus;
                InputFieldMinePlus.text = StageNumMPlus.ToString();
            }
            int MineMax = Convert.ToInt32(Math.Round(StageNumX*StageNumY/3f, 0));
            if (StageNumMPlus > MineMax)
            {
                StageNumMPlus = MineMax;
                InputFieldMinePlus.text = MineMax.ToString();
            }
            if (StageNumMMinus > MineMax)
            {
                StageNumMMinus = MineMax;
                InputFieldMineMinus.text = MineMax.ToString();
            }
            if (StageNumMPlus + StageNumMMinus > MineMax)
            {
                StageNumMMinus = MineMax - StageNumMPlus;
                InputFieldMineMinus.text = StageNumMMinus.ToString();
            }

            uiTextMine.text = "<color=red>正電地雷</color>: 設置"+StageNumMPlus+" 預測"+"0"+"\n<color=cyan>負電地雷</color>: 設置"+StageNumMMinus+" 預測"+"0";

            //隨機填入正負地雷
            for (int i = 0; i < StageNumMPlus; i++)
            {
                AddMine(1,SelectX,SelectY);
            }
            for (int i = 0; i < StageNumMMinus; i++)
            {
                AddMine(-1,SelectX,SelectY);
            }

            //計算地雷周邊數字，地雷本身為9或是-9
            int[,] EightCheck = new int[8, 3] {{1,0,1},{1,-1,1},{0,-1,1},{-1,-1,1},{-1,0,1},{-1,1,1},{0,1,1},{1,1,1}};
            for (int i = 0; i < StageNumX; i++)
            {
                for (int j = 0; j < StageNumY; j++)
                {
                    EightCheck = new int[8, 3] {{1,0,1},{1,-1,1},{0,-1,1},{-1,-1,1},{-1,0,1},{-1,1,1},{0,1,1},{1,1,1}};
                    for (int k = 0; k < 8; k++)
                    {
                        if ((EightCheck[k,0]+i < 0) || (EightCheck[k,0]+i >= StageNumX) || (EightCheck[k,1]+j < 0) || (EightCheck[k,1]+j >= StageNumY))
                        {
                            EightCheck[k,2] = 0;
                        }
                        if (EightCheck[k,2]==1)
                        {
                            if (sgmine[i+EightCheck[k,0], j+EightCheck[k,1]] == 1)
                            {
                                sgnum[i, j]++;
                                sgmine2[i, j] = 1;
                            }
                            if (sgmine[i+EightCheck[k,0], j+EightCheck[k,1]] == -1)
                            {
                                sgnum[i, j]--;
                                sgmine2[i, j] = 1;
                            }
                        }
                        //Debug.Log("i="+i+",j="+j+",k="+k+",sgmine2="+sgmine2[i, j]+",sgnum="+sgnum[i, j]);
                    }
                    if (Math.Abs(sgmine[i, j]) == 1)
                    {
                        sgnum[i, j] = sgmine[i, j]*9;
                        sgmine2[i, j] = 1;
                    }
                }
            }

            //轉換關卡階段到1
            stagephase = 1;
            SetController(false);
        }

        //關卡階段1的情況下點擊尚未開啟的Btn，則翻開該Btn
        if (stagephase == 1 && sgmap[SelectX,SelectY] == 0 && Math.Abs(sgnum[SelectX,SelectY]) != 9)
        {
            // uiText.text = "<color=green>踩開</color>了 <color=yellow>(" + SelectX + "," + SelectY + ")</color>\n" + uiText.text;
            sgmap[SelectX,SelectY] = 1;
            CheckBtnStatus(SelectX, SelectY);
            if (sgnum[SelectX,SelectY] == 0) {ContinueZero = true;}
        }

        if (stagephase == 1 && sgmap[SelectX,SelectY] == 0 && Math.Abs(sgnum[SelectX,SelectY]) == 9)
        {
            uiText.text = "<color=magenta>糟了，是地雷！</color>\n" + uiText.text;
            //Btn陣列顯示所有地雷
            RevealMine();
            SetController(true);
            stagephase = 2;
            FramePos = 1;
            FramePos2 = 2;
            FrameAni.Play("FrameL");
            KeyActiveOnce = true;
        }

    }

    public void BtnPredict(int X, int Y)
    {
        SelectX = X;
        SelectY = Y;

        bool renew = false;
        if (sgmap[SelectX,SelectY] == 0 && !renew)
        {
            sgmap[SelectX,SelectY] = -1;
            // uiText.text = "我<color=green>猜</color> <color=yellow>(" + SelectX + "," + SelectY + ")</color> 是<color=red>正電地雷</color>\n" + uiText.text;
            GameObject BtnTxtGot = GameObject.Find("Btn" + (SelectX+1) + "-" + (SelectY+1) + "/Text");
            BtnTxtGot.GetComponent<Text>().text = "＋";
            BtnTxtGot.GetComponent<Text>().color = Color.red;
            renew = true;
            GuessMPlus++;
            uiTextMine.text = "<color=red>正電地雷</color>: 設置"+StageNumMPlus+" 預測"+GuessMPlus+"\n<color=cyan>負電地雷</color>: 設置"+StageNumMMinus+" 預測"+GuessMMinus;
        }
        if (sgmap[SelectX,SelectY] == -1 && !renew)
        {
            sgmap[SelectX,SelectY] = -2;
            // uiText.text = "我<color=green>猜</color> <color=yellow>(" + SelectX + "," + SelectY + ")</color> 是<color=cyan>負電地雷</color>\n" + uiText.text;
            GameObject BtnTxtGot = GameObject.Find("Btn" + (SelectX+1) + "-" + (SelectY+1) + "/Text");
            BtnTxtGot.GetComponent<Text>().text = "－";
            BtnTxtGot.GetComponent<Text>().color = new Color(0f, 6f/8f, 6f/8f, 1f);
            renew = true;
            GuessMPlus--;
            GuessMMinus++;
            uiTextMine.text = "<color=red>正電地雷</color>: 設置"+StageNumMPlus+" 預測"+GuessMPlus+"\n<color=cyan>負電地雷</color>: 設置"+StageNumMMinus+" 預測"+GuessMMinus;
        }
        if (sgmap[SelectX,SelectY] == -2 && !renew)
        {
            sgmap[SelectX,SelectY] = -3;
            // uiText.text = "我<color=blue>不知道</color> <color=yellow>(" + SelectX + "," + SelectY + ")</color> 是什麼..\n" + uiText.text;
            GameObject BtnTxtGot = GameObject.Find("Btn" + (SelectX+1) + "-" + (SelectY+1) + "/Text");
            BtnTxtGot.GetComponent<Text>().text = "？";
            BtnTxtGot.GetComponent<Text>().color = Color.black;
            renew = true;
            GuessMMinus--;
            uiTextMine.text = "<color=red>正電地雷</color>: 設置"+StageNumMPlus+" 預測"+GuessMPlus+"\n<color=cyan>負電地雷</color>: 設置"+StageNumMMinus+" 預測"+GuessMMinus;
        }
        if (sgmap[SelectX,SelectY] == -3 && !renew)
        {
            sgmap[SelectX,SelectY] = 0;
            GameObject BtnTxtGot = GameObject.Find("Btn" + (SelectX+1) + "-" + (SelectY+1) + "/Text");
            BtnTxtGot.GetComponent<Text>().text = "";
            BtnTxtGot.GetComponent<Text>().color = Color.black;
            renew = true;
        }

    }

    public void BtnEightCheck(int X, int Y)
    {
        SelectX = X;
        SelectY = Y;
        //設定array確認的八個方位的Btn，數值分別為{x,y,check-or-not}
        int[,] EightCheck = new int[8, 3] {{1,0,1},{1,-1,1},{0,-1,1},{-1,-1,1},{-1,0,1},{-1,1,1},{0,1,1},{1,1,1}};
        int minenum = 0;
        bool guessmine = false;
        for (int i = 0; i < 8; i++)
        {
            if ((EightCheck[i,0]+SelectX < 0) || (EightCheck[i,0]+SelectX >= StageNumX) || (EightCheck[i,1]+SelectY < 0) || (EightCheck[i,1]+SelectY >= StageNumY))
            {
                EightCheck[i,2] = 0;
            }
            if (EightCheck[i,2]==1)
            {
                if (sgmap[SelectX+EightCheck[i,0], SelectY+EightCheck[i,1]] == -1)
                {
                    minenum++;
                    guessmine = true;
                }
                if (sgmap[SelectX+EightCheck[i,0], SelectY+EightCheck[i,1]] == -2)
                {
                    minenum--;
                    guessmine = true;
                }
            }
        }
        if ((sgnum[SelectX, SelectY]==minenum && sgmine2[SelectX, SelectY]==1 && guessmine==true) || (sgnum[SelectX, SelectY]==0 && sgmine2[SelectX, SelectY]==0 && guessmine==false))
        {
            // uiText.text = "<color=green>展開</color> <color=yellow>(" + SelectX + "," + SelectY + ")</color> 附近格子\n" + uiText.text;
            CheckUDLRBtn(SelectX, SelectY, true);
        }
    }

    //進入答案確認模式
    public void CheckBtnAnswer()
    {
        if (StageNumMPlus != GuessMPlus || StageNumMMinus != GuessMMinus)
        {
            uiText.text = "<color=magenta>預測地雷數目有誤，請重新檢查！</color>\n" + uiText.text;
        }
        else
        {
            RevealMine();
            if (WrongAns > 0)
            {
                uiText.text = "<color=magenta>糟了，解答失敗！錯誤" + WrongAns + "處</color>\n" + uiText.text;
            }
            else
            {
                uiText.text = "<color=white>恭喜！您成功解除地雷了！</color>\n" + uiText.text;
            }
            SetController(true);
            FramePos = 1;
            FramePos2 = 2;
            FrameAni.Play("FrameL");
            KeyActiveOnce = true;
        }
    }

    //填入地雷
    public void AddMine(int MineType, int SelectX, int SelectY)
    {
        int RndX;
        int RndY;
        bool BtnEmpty = false;
        //偵測棋盤格中是否有空位
        System.Random rand = new System.Random(Guid.NewGuid().GetHashCode());
        while(!BtnEmpty)
        {
            RndX = rand.Next(0,StageNumX);
            RndY = rand.Next(0,StageNumY);
            if (sgmine[RndX,RndY] == 0 && SelectX != RndX && SelectY != RndY)
            {
            sgmine[RndX,RndY] = MineType;
            BtnEmpty = true;
            }
        }

    }

    //顯示地雷
    public void RevealMine()
    {
        BtnCheckAns.GetComponent<Button>().interactable = false;
        for (int i = 0; i < StageNumX; i++)
        {
            for (int j = 0; j < StageNumY; j++)
            {
                CheckBtnStatus(i, j);
                sgmap[i,j] = 2;
            }
        }
    }

    public void CheckBtnStatus(int i, int j)
    {
        GameObject BtnGot = GameObject.Find("Btn" + (i+1) + "-" + (j+1));
        GameObject BtnTxtGot = GameObject.Find("Btn" + (i+1) + "-" + (j+1) + "/Text");
        if (sgnum[i,j]<9 && sgnum[i,j]>0)
        {
            BtnGot.GetComponent<Image>().color = new Color(sgnum[i,j]/8f, 0f, 0f, 1f);
            BtnTxtGot.GetComponent<Text>().text = ""+sgnum[i,j];
            BtnTxtGot.GetComponent<Text>().color = Color.white;
        }
        if (sgnum[i,j]>-9 && sgnum[i,j]<0)
        {
            BtnGot.GetComponent<Image>().color = new Color(0f, -sgnum[i,j]/8f, -sgnum[i,j]/8f, 1f);
            BtnTxtGot.GetComponent<Text>().text = ""+sgnum[i,j];
            BtnTxtGot.GetComponent<Text>().color = Color.white;
        }
        if (sgnum[i,j]==0 && sgmine2[i,j]==0)
        {
            BtnGot.GetComponent<Image>().color = Color.black;
            BtnTxtGot.GetComponent<Text>().text = "";
            BtnTxtGot.GetComponent<Text>().color = Color.white;
            ContinueZero = true;
        }
        if (sgnum[i,j]==0 && sgmine2[i,j]==1)
        {
            BtnGot.GetComponent<Image>().color = Color.black;
            BtnTxtGot.GetComponent<Text>().text = "0";
            BtnTxtGot.GetComponent<Text>().color = Color.white;
            ContinueZero = true;
        }
        if (sgnum[i,j]==9)
        {
            BtnGot.GetComponent<Image>().color = Color.red;
            BtnTxtGot.GetComponent<Text>().text = "●";
            BtnTxtGot.GetComponent<Text>().color = Color.black;
        }
        if (sgnum[i,j]==-9)
        {
            BtnGot.GetComponent<Image>().color = Color.cyan;
            BtnTxtGot.GetComponent<Text>().text = "●";
            BtnTxtGot.GetComponent<Text>().color = Color.black;
        }
        if ((sgnum[i,j]!=9 && sgmap[i,j]==-1) || (sgnum[i,j]!=-9 && sgmap[i,j]==-2))
        {
            BtnTxtGot.GetComponent<Text>().text = "╳";
            BtnTxtGot.GetComponent<Text>().color = Color.yellow;
            WrongAns++;
        }
    }

    public void CheckUDLRBtn(int CheckX, int CheckY, bool CheckEightBtn)
    {
        int[,] EightCheck = new int[8, 3] {{1,0,1},{1,-1,1},{0,-1,1},{-1,-1,1},{-1,0,1},{-1,1,1},{0,1,1},{1,1,1}};
        for (int i = 0; i < 8; i++)
        {
            if ((EightCheck[i,0]+CheckX < 0) || (EightCheck[i,0]+CheckX >= StageNumX) || (EightCheck[i,1]+CheckY < 0) || (EightCheck[i,1]+CheckY >= StageNumY))
            {
                EightCheck[i,2] = 0;
            }
            if (EightCheck[i,2]==1 && sgmap[CheckX+EightCheck[i,0],CheckY+EightCheck[i,1]]==0)
            {
                int NowBtnNum = sgnum[CheckX+EightCheck[i,0],CheckY+EightCheck[i,1]];
                if (Math.Abs(NowBtnNum)!=9)
                {
                    CheckBtnStatus(CheckX+EightCheck[i,0],CheckY+EightCheck[i,1]);
                    sgmap[CheckX+EightCheck[i,0],CheckY+EightCheck[i,1]] = 1;
                }
                else if (CheckEightBtn && Math.Abs(NowBtnNum)==9)
                {
                    uiText.text = "<color=magenta>糟了，是地雷！</color>\n" + uiText.text;
                    RevealMine();
                    SetController(true);
                    stagephase = 2;
                    FramePos = 1;
                    FramePos2 = 2;
                    FrameAni.Play("FrameL");
                    KeyActiveOnce = true;
                }
            }
        }
    }

    //遊戲中每幀更新內容
    void Update()
    {

        //抓取鍵盤按Space的動作
        if (Input.GetKeyDown("tab")) {KeySpaceFg = true;}
        if (Input.GetKeyUp("tab"))
        {
            KeyActiveOnce = true;
            if (FramePos == 0 && KeySpaceFg) {FramePos = 1; KeySpaceFg = false; FrameAni.Play("FrameL");}
            if (FramePos == 1 && KeySpaceFg) {FramePos = 2; KeySpaceFg = false; FrameAni.Play("FrameL");}
            if (FramePos == 2 && KeySpaceFg) {FramePos = 0; KeySpaceFg = false; FrameAni.Play("FrameS");}
            //跑迴圈確認Frame要跑到哪裡
            bool CheckEnable = true;
            while(CheckEnable)
            {
                CheckEnable = false;
                if (FramePos == 0 && (FramePosX == -1 || FramePosY == -1)) {FramePos = 1; CheckEnable = true; FrameAni.Play("FrameL");}
                if (FramePos == 1 && !ControllerEnable) {FramePos = 2; CheckEnable = true; FrameAni.Play("FrameL");}
                if (FramePos == 2 && !BtnCheckAns.GetComponent<Button>().interactable) {FramePos = 0; CheckEnable = true; FrameAni.Play("FrameS");}
            }
        }
        //抓取鍵盤上下左右的動作
        if (Input.GetKey("up") || Input.GetKey("w")) {KeyUpTm += Time.deltaTime;}
        if (Input.GetKey("down") || Input.GetKey("s")) {KeyDownTm += Time.deltaTime;}
        if (Input.GetKey("left") || Input.GetKey("a")) {KeyLeftTm += Time.deltaTime;}
        if (Input.GetKey("right") || Input.GetKey("d")) {KeyRightTm += Time.deltaTime;}
        if (Input.GetKeyUp("up") || Input.GetKeyUp("w")) {KeyUpTm = 0f; KeyUpTm2 = 0.15f; KeyUpFg = false;}
        if (Input.GetKeyUp("down") || Input.GetKeyUp("s")) {KeyDownTm = 0f; KeyDownTm2 = 0.15f; KeyDownFg = false;}
        if (Input.GetKeyUp("left") || Input.GetKeyUp("a")) {KeyLeftTm = 0f; KeyLeftTm2 = 0.15f; KeyLeftFg = false;}
        if (Input.GetKeyUp("right") || Input.GetKeyUp("d")) {KeyRightTm = 0f; KeyRightTm2 = 0.15f; KeyRightFg = false;}
        //如果上下左右被按下，則變更Frame的位置(在棋盤格時)
        if (KeyUpTm > 0f && !KeyUpFg && FramePos == 0 && FramePosY > 0)
        {
            FramePosY--;
            KeyUpFg = true;
            KeyActiveOnce = true;
        }
        if (KeyDownTm > 0f && !KeyDownFg && FramePos == 0 && FramePosY < StageNumY-1)
        {
            FramePosY++;
            KeyDownFg = true;
            KeyActiveOnce = true;
        }
        if (KeyLeftTm > 0f && !KeyLeftFg && FramePos == 0 && FramePosX > 0)
        {
            FramePosX--;
            KeyLeftFg = true;
            KeyActiveOnce = true;
        }
        if (KeyRightTm > 0f && !KeyRightFg && FramePos == 0 && FramePosX < StageNumX-1)
        {
            FramePosX++;
            KeyRightFg = true;
            KeyActiveOnce = true;
        }
        if (KeyUpTm > 0.15f && KeyUpTm > KeyUpTm2+0.05f && FramePos == 0 && FramePosY > 0)
        {
            FramePosY--;
            KeyUpTm2 = KeyUpTm2+0.05f;
            KeyActiveOnce = true;
        }
        if (KeyDownTm > 0.15f && KeyDownTm > KeyDownTm2+0.05f && FramePos == 0 && FramePosY < StageNumY-1)
        {
            FramePosY++;
            KeyDownTm2 = KeyDownTm2+0.05f;
            KeyActiveOnce = true;
        }
        if (KeyLeftTm > 0.15f && KeyLeftTm > KeyLeftTm2+0.05f && FramePos == 0 && FramePosX > 0)
        {
            FramePosX--;
            KeyLeftTm2 = KeyLeftTm2+0.05f;
            KeyActiveOnce = true;
        }
        if (KeyRightTm > 0.15f && KeyRightTm > KeyRightTm2+0.05f && FramePos == 0 && FramePosX < StageNumX-1)
        {
            FramePosX++;
            KeyRightTm2 = KeyRightTm2+0.05f;
            KeyActiveOnce = true;
        }
        //如果上下左右被按下，則變更Frame的位置(在右上角輸入欄時)
        if (KeyUpTm > 0f && !KeyUpFg && FramePos == 1)
        {
            FramePos2--;
            KeyUpFg = true;
            KeyActiveOnce = true;
        }
        if (KeyDownTm > 0f && !KeyDownFg && FramePos == 1)
        {
            FramePos2++;
            KeyDownFg = true;
            KeyActiveOnce = true;
        }
        if (KeyLeftTm > 0f && !KeyLeftFg && FramePos == 1)
        {
            FramePos2--;
            KeyLeftFg = true;
            KeyActiveOnce = true;
        }
        if (KeyRightTm > 0f && !KeyRightFg && FramePos == 1)
        {
            FramePos2++;
            KeyRightFg = true;
            KeyActiveOnce = true;
        }
        if (FramePos2 < 0) {FramePos2 = 4;}
        if (FramePos2 > 4) {FramePos2 = 0;}
        if (FramePos == 0 && FramePosX != -1 && FramePosX != -1 && KeyActiveOnce)
        {
            GameObject BtnFocus = GameObject.Find("Btn" + (FramePosX+1) + "-" + (FramePosY+1));
            Frame.transform.position = BtnFocus.transform.position;
            FrameAni.Play("FrameS");
            EventSystem.current.SetSelectedGameObject(BtnFocus, null);
            KeyActiveOnce = false;
        }
        if (FramePos2 == 0 && FramePos == 1 && KeyActiveOnce)
        {
            Frame.transform.position = InputFieldX.transform.position;
            FrameAni.Play("FrameM");
            InputFieldX.ActivateInputField();
            KeyActiveOnce = false;
        }
        if (FramePos2 == 1 && FramePos == 1 && KeyActiveOnce)
        {
            Frame.transform.position = InputFieldY.transform.position;
            FrameAni.Play("FrameM");
            InputFieldY.ActivateInputField();
            KeyActiveOnce = false;
        }
        if (FramePos2 == 2 && FramePos == 1 && KeyActiveOnce)
        {
            Frame.transform.position = BtnCrtStage.transform.position;
            FrameAni.Play("FrameL");
            EventSystem.current.SetSelectedGameObject(BtnCrtStage, null);
            KeyActiveOnce = false;
        }
        if (FramePos2 == 3 && FramePos == 1 && KeyActiveOnce)
        {
            Frame.transform.position = InputFieldMinePlus.transform.position;
            FrameAni.Play("FrameM");
            InputFieldMinePlus.ActivateInputField();
            KeyActiveOnce = false;
        }
        if (FramePos2 == 4 && FramePos == 1 && KeyActiveOnce)
        {
            Frame.transform.position = InputFieldMineMinus.transform.position;
            FrameAni.Play("FrameM");
            InputFieldMineMinus.ActivateInputField();
            KeyActiveOnce = false;
        }
        if (FramePos == 2 && KeyActiveOnce)
        {
            Frame.transform.position = BtnCheckAns.transform.position;
            FrameAni.Play("FrameL");
            EventSystem.current.SetSelectedGameObject(BtnCheckAns, null);
            KeyActiveOnce = false;
        }

        //滑鼠游標的位置
        // Vector3 mousePos = Input.mousePosition;
        // Debug.Log(mousePos.x+", "+mousePos.y);

        //偵測滑鼠左右鍵/鍵盤Space&Ctrl是否被按下
        if (Input.GetMouseButtonDown(0)) {MouseDownLeft = true;}
        if (Input.GetMouseButtonDown(1)) {MouseDownRight = true;}
        if (Input.GetButtonDown("Jump")) {KeyDownLeft = true;}
        if (Input.GetButtonDown("Fire1")) {KeyDownRight = true;}

        //若單純只有滑鼠左鍵按下彈起，則點開該Btn內容物
        if (Input.GetMouseButtonUp(0) && MouseDownLeft && !MouseDownRight && MouseDownOnBtn && HoverX != -1 && HoverY != -1)
        {
            BtnPress(HoverX, HoverY);
        }
        if (Input.GetButtonUp("Jump") && KeyDownLeft && !KeyDownRight && FramePos == 0)
        {
            BtnPress(FramePosX, FramePosY);
        }

        //若單純只有滑鼠右鍵按下彈起，則開關預測可能為地雷的功能
        if (Input.GetMouseButtonUp(1) && !MouseDownLeft && MouseDownRight && MouseDownOnBtn && HoverX != -1 && HoverY != -1)
        {
            BtnPredict(HoverX, HoverY);
        }
        if (Input.GetButtonUp("Fire1") && !KeyDownLeft && KeyDownRight && FramePos == 0)
        {
            BtnPredict(FramePosX, FramePosY);
        }

        //若滑鼠左鍵與滑鼠右鍵同時按下並且其中一鍵彈起，則確認附近8格Btn預測地雷數與點下Btn數值是否相同，進而開拓其他位置的Btn
        if ((Input.GetMouseButtonUp(0)||Input.GetMouseButtonUp(1)) && stagephase == 1 && MouseDownLeft && MouseDownRight && MouseDownOnBtn && HoverX != -1 && HoverY != -1)
        {
            BtnEightCheck(HoverX, HoverY);
        }
        if ((Input.GetButtonUp("Jump")||Input.GetButtonUp("Fire1")) && stagephase == 1 && KeyDownLeft && KeyDownRight && FramePos == 0)
        {
            BtnEightCheck(FramePosX, FramePosY);
        }
        if ((Input.GetButtonUp("Jump")||Input.GetButtonUp("Fire1")) && stagephase == 0 && KeyDownLeft && KeyDownRight && FramePos == 0)
        {
            KeyDownLeft = false; KeyDownRight = false;
        }

        //若ContinueZero是開啟的，表示可能仍有位置數值為0
        if (ContinueZero == true)
        {
            ContinueZero = false;
            for (int i = 0; i < StageNumX; i++)
            {
                for (int j = 0; j < StageNumY; j++)
                {
                    if (sgmap[i,j] == 1 && sgnum[i,j] == 0)
                    {
                    sgopnbtn[i,j] = 1;
                    }
                }
            }

            for (int i = 0; i < StageNumX; i++)
            {
                for (int j = 0; j < StageNumY; j++)
                {
                    if (sgopnbtn[i,j] == 1)
                    {
                    CheckUDLRBtn(i, j, false);
                    sgopnbtn[i,j] = 0;
                    sgmap[i,j] = 2;
                    }
                }
            }
        }

        //偵測螢幕大小是否變更
        if ((ScnX != Screen.width || ScnY != Screen.height) && CreateBtnFlag)
        {
            //移除所有已創建的Btn
            for (int i = 1; i <= 100; i++)
            {
                for (int j = 1; j <= 100; j++)
                {
                    Destroy(GameObject.Find("Btn" + i + "-" + j));
                    sgmine[i-1,j-1] = 0;
                    sgmine2[i-1,j-1] = 0;
                    sgnum[i-1,j-1] = 0;
                    sgmap[i-1,j-1] = 0;
                    sgopnbtn[i-1,j-1] = 0;
                }
            }
            BtnCheckAns.GetComponent<Button>().interactable = false;
            SetController(true);
            CreateBtnFlag = false;
        }

        //偵測滑鼠左右鍵是否被彈起
        if (Input.GetMouseButtonUp(0)) {MouseDownLeft = false; MouseDownOnBtn = false;}
        if (Input.GetMouseButtonUp(1)) {MouseDownRight = false; MouseDownOnBtn = false;}
        if (Input.GetButtonUp("Jump")) {KeyDownLeft = false;}
        if (Input.GetButtonUp("Fire1")) {KeyDownRight = false;}
        //儲存變更的螢幕大小
        ScnX = Screen.width;
        ScnY = Screen.height;

    }

}
