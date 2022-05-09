using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    public GameObject Frame2;
    public Animator Frame2Ani;
    public Text uiTextSlider;
    public GameObject GameObjectSlider2;
    public Slider Slider;
    public Slider Slider2;

    public int[,] sgmine = new int[30,25]; //stage mine，1:正電地雷 -1:負電地雷
    public int[,] sgmine2 = new int[30,25]; //stage mine 2，記憶附近是否有地雷，0:沒有 1:有
    public int[,] sgnum = new int[30,25]; //stage number，9:正電地雷 -9:負電地雷 0:附近沒地雷 其他:附近有幾個地雷(正負相消)
    public int[,] sgmap = new int[30,25]; //stage map，0:未開 1:剛踩探測是否為0 2:確認為0、其他數字或地雷 -1:猜測為正電地雷 -2:猜測為負電地雷 -3:不知道是否為地雷
    public int[,] sgopnbtn = new int[30,25]; //stage open button，記憶可以開啟的複數位置，0:沒有 1:有
    public int StageNumX; //關卡創建棋盤格X格數
    public int StageNumY; //關卡創建棋盤格Y格數
    public int StageNumMPlus; //關卡創建棋盤格正電地雷數
    public int StageNumMMinus; //關卡創建棋盤格負電地雷數
    public int GuessMPlus = 0; //預測正電地雷數量
    public int GuessMMinus = 0; //預測負電地雷數量
    public int stagephase = 0; //關卡階段，0:創建棋盤盤面 1:正常遊戲中 2:解除地雷成功或踩到地雷
    float StageTime = 0f;
    float StageTimeTmp = 0f;

    public int SelectX; //目前選擇棋盤格位置X
    public int SelectY; //目前選擇棋盤格位置Y
    public static int HoverX; //掠過棋盤格位置X
    public static int HoverY; //掠過棋盤格位置Y
    public static bool HoverCrtStage; //掠過創建關卡按鈕
    public static bool HoverCheckAns; //掠過檢查答案按鈕
    public static bool MouseDownOnBtn = false; //按下棋盤格Btn按鈕
    public static bool MouseDownOnBtnCrtStage = false; //按下創建關卡按鈕
    public static bool MouseDownOnBtnCheckAns = false; //按下檢查答案按鈕
    bool MouseDownLeft = false; //滑鼠左鍵按壓flag
    bool MouseDownRight = false; //滑鼠右鍵按壓flag
    bool KeyDownLeft = false; //鍵盤擬滑鼠左鍵按壓flag
    bool KeyDownRight = false; //鍵盤擬滑鼠右鍵按壓flag
    bool KeyDownMPlus = false; //鍵盤n鍵按壓flag
    bool KeyDownMMinus = false; //鍵盤m鍵按壓flag
    bool KeyDownMQues = false; //鍵盤,鍵按壓flag
    bool ContinueZero = false; //拓展周圍地雷總和值為0的棋盤
    int WrongAns = 0; //錯誤答案處數計數器
    bool ControllerEnable = true; //右上方可否看到輸入欄等UI控制元件

    float KeyUpTm = 0f; //鍵盤上鍵按壓時間
    float KeyDownTm = 0f; //鍵盤下鍵按壓時間
    float KeyLeftTm = 0f; //鍵盤左鍵按壓時間
    float KeyRightTm = 0f; //鍵盤右鍵按壓時間
    float KeyUpTm2 = 0.15f; //鍵盤上鍵按壓時間2
    float KeyDownTm2 = 0.15f; //鍵盤下鍵按壓時間2
    float KeyLeftTm2 = 0.15f; //鍵盤左鍵按壓時間2
    float KeyRightTm2 = 0.15f; //鍵盤右鍵按壓時間2
    bool KeyUpFg = false; //鍵盤上鍵按壓flag
    bool KeyDownFg = false; //鍵盤下鍵按壓flag
    bool KeyLeftFg = false; //鍵盤左鍵按壓flag
    bool KeyRightFg = false; //鍵盤右鍵按壓flag
    bool KeyTabFg = false; //鍵盤Tab鍵按壓flag

    public int FramePos = 1; //Frame的位置，0:棋盤格位置 1:右上設置 2:右下設置
    public int FramePos2 = 2; //Frame的右上位置，0:X格數 1:Y格數 2:創建關卡 3:正電地雷 4:負電地雷
    public int FramePosX = -1; //Frame在棋盤格中的X位置
    public int FramePosY = -1; //Frame在棋盤格中的Y位置
    public int Frame2Pos = 1; //Frame2的位置，0:棋盤格位置 1:右上設置 2:右下設置
    public int Frame2Pos2 = 2; //Frame2的右上位置，0:X格數 1:Y格數 2:創建關卡 3:正電地雷 4:負電地雷
    public int Frame2PosX = -1; //Frame2在棋盤格中的X位置
    public int Frame2PosY = -1; //Frame2在棋盤格中的Y位置
    public int NetHostCount = 0; //服務端計數量
    public int NetHostGetCount = 0; //服務端計數量(從對方網路取得)
    public int NetClientCount = 0; //客戶端計數量
    public int NetClientGetCount = 0; //客戶端計數量(從對方網路取得)

    float SliderFloat = 0f; //玩家1的完成進度或是生命值
    float SliderFloatPre = 0f; //玩家1的完成進度或是生命值
    float Slider2Float = 0f; //玩家2的完成進度或是生命值
    int NumOfOut = 0; //合作模式玩家出局數

    //--------------------------------------------------------------------------------------------------------

    void Start()
    {
        uiText.text = "視窗大小:\nWidth=" + Screen.width + ", Height=" + Screen.height + "\n" + uiText.text;

        switch (SceneControl.GameMode)
        {
            case 0:
                uiText.text = "您正在遊玩<color=magenta>單人模式</color>\n" + uiText.text;
                Frame2.SetActive(false);
                GameObjectSlider2.SetActive(false);
                uiTextSlider.text = SceneControl.GameName+"\n";
                break;
            case 1:
                uiText.text = "您正在遊玩<color=magenta>合作模式</color>\n" + uiText.text;
                GameObjectSlider2.SetActive(false);
                uiTextSlider.text = SceneControl.GameName+"\n";
                break;
            case 2:
                uiText.text = "您正在遊玩<color=magenta>競速模式</color>\n" + uiText.text;
                Frame2.SetActive(false);
                break;
            case 3:
                uiText.text = "您正在遊玩<color=magenta>對弈模式</color>\n" + uiText.text;
                GameObjectSlider2.SetActive(false);
                break;
            default:
                break;
        }

        if (SceneControl.GameNet == 2)
        {
            InputFieldX.interactable = false;
            InputFieldY.interactable = false;
            BtnCrtStage.GetComponent<Button>().interactable = false;
            InputFieldMinePlus.interactable = false;
            InputFieldMineMinus.interactable = false;
        }

        BtnCheckAns.GetComponent<Button>().interactable = false;
        uiTextMine.text = "<color=red>正電地雷</color>: 設置"+"？"+" 預測"+"？"+"\n<color=cyan>負電地雷</color>: 設置"+"？"+" 預測"+"？";
        Frame.transform.position = BtnCrtStage.transform.position;
        FrameAni.Play("FrameL");
        if (SceneControl.GameMode != 0 && SceneControl.GameMode != 2)
            Frame2Ani.Play("Frame2L");
        EventSystem.current.SetSelectedGameObject(BtnCrtStage, null);

    }

    //--------------------------------------------------------------------------------------------------------

    public void SetController(bool flag)
    {
        GameObjectX.SetActive(flag);
        GameObjectY.SetActive(flag);
        BtnCrtStage.SetActive(flag);
        GameObjectMinePlus.SetActive(flag);
        GameObjectMineMinus.SetActive(flag);
        ControllerEnable = flag;
    }

    //--------------------------------------------------------------------------------------------------------

    //移除所有已創建的Btn
    public void DestroyBtn()
    {
        for (int i = 1; i <= 30; i++)
        {
            for (int j = 1; j <= 25; j++)
            {
                Destroy(GameObject.Find("Btn" + i + "-" + j));
                sgmine[i-1,j-1] = 0;
                sgmine2[i-1,j-1] = 0;
                sgnum[i-1,j-1] = 0;
                sgmap[i-1,j-1] = 0;
                sgopnbtn[i-1,j-1] = 0;
            }
        }
    }

    //--------------------------------------------------------------------------------------------------------

    public void CrtBtn()
    {
        //回歸關卡階段到0，其餘回歸預設值
        stagephase = 0;
        WrongAns = 0;
        GuessMPlus = 0;
        GuessMMinus = 0;
        SetController(true);
        uiText.text = "<color=magenta>創建關卡</color>\n視窗大小:\nWidth=" + Screen.width + ", Height=" + Screen.height + "\n" + uiText.text;
        SliderFloat = 0f;
        Slider.value = 0f;
        Slider2Float = 0f;
        Slider2.value = 0f;
        switch (SceneControl.GameMode)
        {
            case 0:
            case 1:
            case 3:
                uiTextSlider.text = SceneControl.GameName + "\n";
                break;
            case 2:
                uiTextSlider.text = SceneControl.GameName + "\n" + SceneControl.GameName2;
                break;
            default:
                break;
        }

        //移除所有已創建的Btn
        DestroyBtn();

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

        //將Frame移到創建關卡
        FramePosX = 0;
        FramePosY = 0;
        FramePos = 1;
        FramePos2 = 2;
        Frame.transform.position = BtnCrtStage.transform.position;
        FrameAni.Play("FrameL");
        Frame2PosX = 0;
        Frame2PosY = 0;
        Frame2Pos = 1;
        Frame2Pos2 = 2;
        Frame2.transform.position = BtnCrtStage.transform.position;
        if (SceneControl.GameMode != 0 && SceneControl.GameMode != 2)
            Frame2Ani.Play("Frame2L");
        EventSystem.current.SetSelectedGameObject(BtnCrtStage, null);
        uiTextMine.text = "<color=red>正電地雷</color>: 設置"+"？"+" 預測"+"？"+"\n<color=cyan>負電地雷</color>: 設置"+"？"+" 預測"+"？";
    }

    //--------------------------------------------------------------------------------------------------------

    public void CrtBtnLp(int i, int j)
    {
        Vector3 myVector;
        if (1f*Screen.width/Screen.height >= 1578f/776f)
        {
            myVector = Canvas.transform.position + new Vector3(30f*i-(StageNumX+1)*15f, (StageNumY+1)*15f-j*30f, 0);
        }
        else
        {
            float RatioConst = (1f*Screen.width/Screen.height)/(1578f/776f);
            myVector = Canvas.transform.position + new Vector3((30f*i-(StageNumX+1)*15f)*RatioConst, ((StageNumY+1)*15f-j*30f)*RatioConst, 0);
        }
        GameObject Clone;
        Clone = (GameObject)Instantiate(Btn, myVector, new Quaternion(), BtnZone.transform);
        Clone.GetComponent<Image>().color = Color.white;
        Clone.name = "Btn" + i + "-" + j;
    }

    //--------------------------------------------------------------------------------------------------------

    public void BtnPress(int X, int Y)
    {
        SelectX = X;
        SelectY = Y;
        bool BtnPressFirst = false;
        //當所有Btn按鈕都未被按的時候，第一個Btn按下必不為地雷，同時設定所有地雷位置
        if (stagephase == 0)
        {
            BtnPressFirst = true;
            bool StageBoolMPlus = int.TryParse(InputFieldMinePlus.text, out StageNumMPlus);
            bool StageBoolMMinus = int.TryParse(InputFieldMineMinus.text, out StageNumMMinus);
            BtnCheckAns.GetComponent<Button>().interactable = true;
            FramePos = 0;
            KeyActive();

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

            //隨機填入正負地雷
            if (SceneControl.GameNet != 2)
            {
                for (int i = 0; i < StageNumMPlus; i++)
                {
                    AddMine(1,SelectX,SelectY);
                }
                for (int i = 0; i < StageNumMMinus; i++)
                {
                    AddMine(-1,SelectX,SelectY);
                }
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
            GuessMineNum();
        }

        if (BtnPressFirst)
        {
            sgmap[SelectX,SelectY] = 1;
            CheckBtnStatus(SelectX, SelectY);
            if (sgnum[SelectX,SelectY] == 0) {ContinueZero = true;}
        }
        else
        {
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
                Frame2Pos = 1;
                Frame2Pos2 = 2;
                if (SceneControl.GameMode != 0 && SceneControl.GameMode != 2)
                    Frame2Ani.Play("Frame2L");
                KeyActive();
                switch (SceneControl.GameMode)
                {
                    case 0:
                    case 1:
                    case 3:
                        uiTextSlider.text = SceneControl.GameName + "\n";
                        break;
                    case 2:
                        uiTextSlider.text = SceneControl.GameName + "\n" + SceneControl.GameName2;
                        break;
                    default:
                        break;
                }

                if (SceneControl.GameNet == 1 && SceneControl.GameMode == 2)
                {
                    uiText.text = "<color=white>" + SceneControl.GameName + "自爆，因此是" + SceneControl.GameName2 + "的勝利！</color>\n" + uiText.text;
                    NetHostCount++;
                    NetHost.StringIntegrate("SpdModeWin2," + NetHostCount + ";");
                }
                if (SceneControl.GameNet == 2 && SceneControl.GameMode == 2)
                {
                    uiText.text = "<color=white>" + SceneControl.GameName + "自爆，因此是" + SceneControl.GameName2 + "的勝利！</color>\n" + uiText.text;
                    NetClientCount++;
                    NetClient.StringIntegrate("SpdModeWin2," + NetClientCount + ";");
                }
            }
        }
    }

    //--------------------------------------------------------------------------------------------------------

    public void BtnPredict(int X, int Y, int Mode)
    {
        SelectX = X;
        SelectY = Y;

        bool renew = false;
        if (((sgmap[SelectX,SelectY] == 0 && Mode == 0) || (sgmap[SelectX,SelectY] <= 0 && sgmap[SelectX,SelectY] >= -3 && sgmap[SelectX,SelectY] != -1 && Mode == 1)) && !renew)
        {
            sgmap[SelectX,SelectY] = -1;
            // uiText.text = "我<color=green>猜</color> <color=yellow>(" + SelectX + "," + SelectY + ")</color> 是<color=red>正電地雷</color>\n" + uiText.text;
            GameObject BtnTxtGot = GameObject.Find("Btn" + (SelectX+1) + "-" + (SelectY+1) + "/Text");
            BtnTxtGot.GetComponent<Text>().text = "＋";
            BtnTxtGot.GetComponent<Text>().color = Color.red;
            renew = true;
        }
        if (((sgmap[SelectX,SelectY] == -1 && Mode == 0) || (sgmap[SelectX,SelectY] <= 0 && sgmap[SelectX,SelectY] >= -3 && sgmap[SelectX,SelectY] != -2 && Mode == 2)) && !renew)
        {
            sgmap[SelectX,SelectY] = -2;
            // uiText.text = "我<color=green>猜</color> <color=yellow>(" + SelectX + "," + SelectY + ")</color> 是<color=cyan>負電地雷</color>\n" + uiText.text;
            GameObject BtnTxtGot = GameObject.Find("Btn" + (SelectX+1) + "-" + (SelectY+1) + "/Text");
            BtnTxtGot.GetComponent<Text>().text = "－";
            BtnTxtGot.GetComponent<Text>().color = new Color(0f, 6f/8f, 6f/8f, 1f);
            renew = true;
        }
        if (((sgmap[SelectX,SelectY] == -2 && Mode == 0) || (sgmap[SelectX,SelectY] <= 0 && sgmap[SelectX,SelectY] >= -2 && Mode == 3)) && !renew)
        {
            sgmap[SelectX,SelectY] = -3;
            // uiText.text = "我<color=blue>不知道</color> <color=yellow>(" + SelectX + "," + SelectY + ")</color> 是什麼..\n" + uiText.text;
            GameObject BtnTxtGot = GameObject.Find("Btn" + (SelectX+1) + "-" + (SelectY+1) + "/Text");
            BtnTxtGot.GetComponent<Text>().text = "？";
            BtnTxtGot.GetComponent<Text>().color = Color.black;
            renew = true;
        }
        if (((sgmap[SelectX,SelectY] == -3 && (Mode == 0 || Mode == 3)) || (sgmap[SelectX,SelectY] == -1 && Mode == 1) || (sgmap[SelectX,SelectY] == -2 && Mode == 2)) && !renew)
        {
            sgmap[SelectX,SelectY] = 0;
            GameObject BtnTxtGot = GameObject.Find("Btn" + (SelectX+1) + "-" + (SelectY+1) + "/Text");
            BtnTxtGot.GetComponent<Text>().text = "";
            BtnTxtGot.GetComponent<Text>().color = Color.black;
            renew = true;
        }

        GuessMineNum();
    }

    //--------------------------------------------------------------------------------------------------------

    //計算猜測的地雷數量
    public void GuessMineNum()
    {
        GuessMPlus = 0;
        GuessMMinus = 0;
        for (int i = 0; i < StageNumX; i++)
        {
            for (int j = 0; j < StageNumY; j++)
            {
                if(sgmap[i, j] == -1) {GuessMPlus++;}
                if(sgmap[i, j] == -2) {GuessMMinus++;}
            }
        }
        uiTextMine.text = "<color=red>正電地雷</color>: 設置"+StageNumMPlus+" 預測"+GuessMPlus+"\n<color=cyan>負電地雷</color>: 設置"+StageNumMMinus+" 預測"+GuessMMinus;
    }

    //--------------------------------------------------------------------------------------------------------

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

    //--------------------------------------------------------------------------------------------------------

    //進入答案確認模式
    public void CheckBtnAnswer()
    {
        if (StageNumMPlus != GuessMPlus || StageNumMMinus != GuessMMinus)
        {
            uiText.text = "<color=magenta>預測地雷數目有誤，請重新檢查！</color>\n" + uiText.text;
        }
        else if (SceneControl.GameMode == 0)
        {
            RevealMine();
            if (WrongAns > 0)
            {
                uiText.text = "<color=magenta>糟了，解答失敗！錯誤" + WrongAns + "處</color>\n" + uiText.text;
            }
            else
            {
                SliderFloat = 1.0f;
                Slider.value = 1.0f;
                uiTextSlider.text = SceneControl.GameName+"\n";
                uiText.text = "<color=white>恭喜！您成功解除地雷了！</color>\n" + uiText.text;
            }
            SetController(true);
            stagephase = 2;
            FramePos = 1;
            FramePos2 = 2;
            FrameAni.Play("FrameL");
            Frame2Pos = 1;
            Frame2Pos2 = 2;
            if (SceneControl.GameMode != 0 && SceneControl.GameMode != 2)
                Frame2Ani.Play("Frame2L");
            KeyActive();
        }
        else if (SceneControl.GameMode == 1)
        {
            RevealMine();
            if (WrongAns > 0 && NumOfOut == 0)
            {
                uiText.text = "<color=magenta>糟了，解答失敗！錯誤" + WrongAns + "處</color>\n" + uiText.text;
                NumOfOut += 1;
                if (SceneControl.GameNet == 1)
                {
                    NetHostCount++;
                    NetHost.StringIntegrate("NumOfOut," + NetHostCount + "," + NumOfOut + ";");
                }
                if (SceneControl.GameNet == 2)
                {
                    NetClientCount++;
                    NetClient.StringIntegrate("NumOfOut," + NetClientCount + "," + NumOfOut + ";");
                }
            }
            else if (WrongAns > 0 && NumOfOut > 0)
            {
                uiText.text = "<color=magenta>糟了，解答失敗！錯誤" + WrongAns + "處</color>\n" + uiText.text;
                NumOfOut = 0;
                if (SceneControl.GameNet == 1)
                {
                    NetHostCount++;
                    NetHost.StringIntegrate("NumOfOut2," + NetHostCount + ";");
                }
                if (SceneControl.GameNet == 2)
                {
                    NetClientCount++;
                    NetClient.StringIntegrate("NumOfOut2," + NetClientCount + ";");
                }
                SetController(true);
                stagephase = 2;
                FramePos = 1;
                FramePos2 = 2;
                FrameAni.Play("FrameL");
                Frame2Pos = 1;
                Frame2Pos2 = 2;
                Frame2Ani.Play("Frame2L");
                KeyActive();
            }
            else
            {
                NumOfOut = 0;
                SliderFloat = 1.0f;
                Slider.value = 1.0f;
                uiTextSlider.text = SceneControl.GameName+"\n";
                uiText.text = "<color=white>恭喜！您成功解除地雷了！</color>\n" + uiText.text;
                SetController(true);
                stagephase = 2;
                FramePos = 1;
                FramePos2 = 2;
                FrameAni.Play("FrameL");
                Frame2Pos = 1;
                Frame2Pos2 = 2;
                Frame2Ani.Play("Frame2L");
                KeyActive();
            }

        }
        else if (SceneControl.GameMode == 2)
        {
            RevealMine();
            if (WrongAns > 0)
            {
                uiTextSlider.text = SceneControl.GameName+"\n"+SceneControl.GameName2;
                uiText.text = "<color=magenta>糟了，" + SceneControl.GameName + "解答失敗！錯誤" + WrongAns + "處</color>\n" + uiText.text;
                uiText.text = "<color=white>" + SceneControl.GameName + "自爆，因此是" + SceneControl.GameName2 + "的勝利！</color>\n" + uiText.text;

                if (SceneControl.GameNet == 1)
                {
                    NetHostCount++;
                    NetHost.StringIntegrate("SpdModeWin," + NetHostCount + "," + WrongAns + ";");
                }
                if (SceneControl.GameNet == 2)
                {
                    NetClientCount++;
                    NetClient.StringIntegrate("SpdModeWin," + NetClientCount + "," + WrongAns + ";");
                }
            }
            else
            {
                SliderFloat = 1.0f;
                Slider.value = 1.0f;
                uiTextSlider.text = SceneControl.GameName+"\n"+SceneControl.GameName2;
                uiText.text = "<color=white>恭喜！" + SceneControl.GameName + "成功解除地雷了！</color>\n" + uiText.text;
                uiText.text = "<color=white>此局由" + SceneControl.GameName + "獲得勝利！</color>\n" + uiText.text;

                if (SceneControl.GameNet == 1)
                {
                    NetHostCount++;
                    NetHost.StringIntegrate("SpdModeLose," + NetHostCount + ";");
                }
                if (SceneControl.GameNet == 2)
                {
                    NetClientCount++;
                    NetClient.StringIntegrate("SpdModeLose," + NetClientCount + ";");
                }
            }
            SetController(true);
            stagephase = 2;
            FramePos = 1;
            FramePos2 = 2;
            FrameAni.Play("FrameL");
            Frame2Pos = 1;
            Frame2Pos2 = 2;
            KeyActive();
        }
    }

    //--------------------------------------------------------------------------------------------------------

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

    //--------------------------------------------------------------------------------------------------------

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

    //--------------------------------------------------------------------------------------------------------

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

    //--------------------------------------------------------------------------------------------------------

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
                    Frame2Pos = 1;
                    Frame2Pos2 = 2;
                    if (SceneControl.GameMode != 0 && SceneControl.GameMode != 2)
                        Frame2Ani.Play("Frame2L");
                    KeyActive();
                    switch (SceneControl.GameMode)
                    {
                        case 0:
                        case 1:
                        case 3:
                            uiTextSlider.text = SceneControl.GameName + "\n";
                            break;
                        case 2:
                            uiTextSlider.text = SceneControl.GameName + "\n" + SceneControl.GameName2;
                            break;
                        default:
                            break;
                    }

                    if (SceneControl.GameNet == 1 && SceneControl.GameMode == 2)
                    {
                        uiText.text = "<color=white>" + SceneControl.GameName + "自爆，因此是" + SceneControl.GameName2 + "的勝利！</color>\n" + uiText.text;
                        NetHostCount++;
                        NetHost.StringIntegrate("SpdModeWin2," + NetHostCount + ";");
                    }
                    if (SceneControl.GameNet == 2 && SceneControl.GameMode == 2)
                    {
                        uiText.text = "<color=white>" + SceneControl.GameName + "自爆，因此是" + SceneControl.GameName2 + "的勝利！</color>\n" + uiText.text;
                        NetClientCount++;
                        NetClient.StringIntegrate("SpdModeWin2," + NetClientCount + ";");
                    }
                }
            }
        }
    }

    //--------------------------------------------------------------------------------------------------------

    public void DetectInput()
    {
        int tmpnum = 0;

        string IFX = InputFieldX.text;
        int IFXtotalnum = 0;
        for (int i = 0; i < IFX.Length; i++)
        {
            bool IFXfg = int.TryParse(IFX[i].ToString(), out tmpnum);
            if (IFXfg) {IFXtotalnum = IFXtotalnum*10 + tmpnum;}
        }
        if (IFXtotalnum == 0) {IFXtotalnum = 15;}
        InputFieldX.text = IFXtotalnum.ToString();

        string IFY = InputFieldY.text;
        int IFYtotalnum = 0;
        for (int i = 0; i < IFY.Length; i++)
        {
            bool IFYfg = int.TryParse(IFY[i].ToString(), out tmpnum);
            if (IFYfg) {IFYtotalnum = IFYtotalnum*10 + tmpnum;}
        }
        if (IFYtotalnum == 0) {IFYtotalnum = 15;}
        InputFieldY.text = IFYtotalnum.ToString();

        string IFMP = InputFieldMinePlus.text;
        int IFMPtotalnum = 0;
        for (int i = 0; i < IFMP.Length; i++)
        {
            bool IFMPfg = int.TryParse(IFMP[i].ToString(), out tmpnum);
            if (IFMPfg) {IFMPtotalnum = IFMPtotalnum*10 + tmpnum;}
        }
        if (IFMPtotalnum == 0) {IFMPtotalnum = 3;}
        InputFieldMinePlus.text = IFMPtotalnum.ToString();

        string IFMM = InputFieldMineMinus.text;
        int IFMMtotalnum = 0;
        for (int i = 0; i < IFMM.Length; i++)
        {
            bool IFMMfg = int.TryParse(IFMM[i].ToString(), out tmpnum);
            if (IFMMfg) {IFMMtotalnum = IFMMtotalnum*10 + tmpnum;}
        }
        InputFieldMineMinus.text = IFMMtotalnum.ToString();
    }

    //--------------------------------------------------------------------------------------------------------

    public void KeyActive()
    {
        if (FramePos == 0 && FramePosX != -1 && FramePosX != -1)
        {
            GameObject BtnFocus = GameObject.Find("Btn" + (FramePosX+1) + "-" + (FramePosY+1));
            Frame.transform.position = BtnFocus.transform.position;
            FrameAni.Play("FrameS");
            EventSystem.current.SetSelectedGameObject(BtnFocus, null);
            DetectInput();
        }
        if (FramePos2 == 0 && FramePos == 1)
        {
            Frame.transform.position = InputFieldX.transform.position;
            FrameAni.Play("FrameM");
            InputFieldX.ActivateInputField();
            DetectInput();
        }
        if (FramePos2 == 1 && FramePos == 1)
        {
            Frame.transform.position = InputFieldY.transform.position;
            FrameAni.Play("FrameM");
            InputFieldY.ActivateInputField();
            DetectInput();
        }
        if (FramePos2 == 2 && FramePos == 1)
        {
            Frame.transform.position = BtnCrtStage.transform.position;
            FrameAni.Play("FrameL");
            EventSystem.current.SetSelectedGameObject(BtnCrtStage, null);
            DetectInput();
        }
        if (FramePos2 == 3 && FramePos == 1)
        {
            Frame.transform.position = InputFieldMinePlus.transform.position;
            FrameAni.Play("FrameM");
            InputFieldMinePlus.ActivateInputField();
            DetectInput();
        }
        if (FramePos2 == 4 && FramePos == 1)
        {
            Frame.transform.position = InputFieldMineMinus.transform.position;
            FrameAni.Play("FrameM");
            InputFieldMineMinus.ActivateInputField();
            DetectInput();
        }
        if (FramePos == 2)
        {
            Frame.transform.position = BtnCheckAns.transform.position;
            FrameAni.Play("FrameL");
            EventSystem.current.SetSelectedGameObject(BtnCheckAns, null);
            DetectInput();
        }

        if (Frame2Pos == 0 && Frame2PosX != -1 && Frame2PosX != -1 && SceneControl.GameMode != 0 && SceneControl.GameMode != 2)
        {
            GameObject BtnFocus = GameObject.Find("Btn" + (Frame2PosX+1) + "-" + (Frame2PosY+1));
            Frame2.transform.position = BtnFocus.transform.position;
            Frame2Ani.Play("Frame2S");
        }
        if (Frame2Pos2 == 0 && Frame2Pos == 1 && SceneControl.GameMode != 0 && SceneControl.GameMode != 2)
        {
            Frame2.transform.position = InputFieldX.transform.position;
            Frame2Ani.Play("Frame2M");
        }
        if (Frame2Pos2 == 1 && Frame2Pos == 1 && SceneControl.GameMode != 0 && SceneControl.GameMode != 2)
        {
            Frame2.transform.position = InputFieldY.transform.position;
            Frame2Ani.Play("Frame2M");
        }
        if (Frame2Pos2 == 2 && Frame2Pos == 1 && SceneControl.GameMode != 0 && SceneControl.GameMode != 2)
        {
            Frame2.transform.position = BtnCrtStage.transform.position;
            Frame2Ani.Play("Frame2L");
        }
        if (Frame2Pos2 == 3 && Frame2Pos == 1 && SceneControl.GameMode != 0 && SceneControl.GameMode != 2)
        {
            Frame2.transform.position = InputFieldMinePlus.transform.position;
            Frame2Ani.Play("Frame2M");
        }
        if (Frame2Pos2 == 4 && Frame2Pos == 1 && SceneControl.GameMode != 0 && SceneControl.GameMode != 2)
        {
            Frame2.transform.position = InputFieldMineMinus.transform.position;
            Frame2Ani.Play("Frame2M");
        }
        if (Frame2Pos == 2 && SceneControl.GameMode != 0 && SceneControl.GameMode != 2)
        {
            Frame2.transform.position = BtnCheckAns.transform.position;
            Frame2Ani.Play("Frame2L");
        }
    }

    //--------------------------------------------------------------------------------------------------------

    //移除所有已創建的Btn
    void ResetBtn()
    {
        DestroyBtn();
        BtnCheckAns.GetComponent<Button>().interactable = false;
        SetController(true);

        FramePos = 1;
        FramePos2 = 2;
        Frame2Pos = 1;
        Frame2Pos2 = 2;
        KeyActive();

        if (SceneControl.GameNet == 1)
        {
            NetHostCount = 0;
            NetHost.StringIntegrate("Alarm," + NetHostCount + ",100,對方發生程序錯誤，清除盤面" + ";");
        }
        if (SceneControl.GameNet == 2)
        {
            NetClientCount = 0;
            NetClient.StringIntegrate("Alarm," + NetClientCount + ",101,對方發生程序錯誤，清除盤面" + ";");
        }
    }

    //--------------------------------------------------------------------------------------------------------

    void SliderCal()
    {
        int BtnExpand = 0;
        for (int i = 0; i < StageNumX; i++)
        {
            for (int j = 0; j < StageNumY; j++)
            {
                if (sgmap[i,j] == 1 || sgmap[i,j] == 2 || sgmap[i,j] == -1 || sgmap[i,j] == -2)
                    BtnExpand++;
            }
        }
        SliderFloat = Mathf.Pow((BtnExpand / (StageNumX * StageNumY * 1.0f)),3);
        Slider.value = SliderFloat;

        switch (SceneControl.GameMode)
        {
            case 0:
            case 1:
            case 3:
                uiTextSlider.text = SceneControl.GameName + " - " + Mathf.Round(SliderFloat*10000)*0.01 + "%\n";
                break;
            case 2:
                uiTextSlider.text = SceneControl.GameName + " - " + Mathf.Round(SliderFloat*10000)*0.01 + "%\n" + SceneControl.GameName2 + " - " + Mathf.Round(Slider2Float*10000)*0.01 + "%\n";
                Slider2.value = Slider2Float;
                break;
            default:
                break;
        }
    }

    //--------------------------------------------------------------------------------------------------------

    //遊戲中每幀更新內容
    void Update()
    {
        //踩地雷遊戲完成進度條
        StageTime += Time.deltaTime;
        if (StageTime > StageTimeTmp + 0.05f && stagephase >= 1)
        {
            SliderCal();
            if (SceneControl.GameNet == 1 && SceneControl.GameMode == 2 && SliderFloat != SliderFloatPre)
            {
                SliderFloatPre = SliderFloat;
                NetHostCount++;
                NetHost.StringIntegrate("SliderVal," + NetHostCount + "," + SliderFloat + ";");
            }
            if (SceneControl.GameNet == 2 && SceneControl.GameMode == 2 && SliderFloat != SliderFloatPre)
            {
                SliderFloatPre = SliderFloat;
                NetClientCount++;
                NetClient.StringIntegrate("SliderVal," + NetClientCount + "," + SliderFloat + ";");
            }
            StageTimeTmp += 0.05f;
        }

        //解析網路從對方收到的字串
        if (SceneControl.GameNet == 1 && NetHost.recvStrPcs != "")
        {
            string[] strAry = NetHost.recvStrPcs.Split(',');
            switch (strAry[0])
            {
                case "BtnPress":
                    NetClientGetCount++;
                    if (int.Parse(strAry[1]) == NetClientGetCount)
                    {
                        Frame2Pos = 0;
                        Frame2PosX = int.Parse(strAry[2]);
                        Frame2PosY = int.Parse(strAry[3]);
                        BtnPress(int.Parse(strAry[2]),int.Parse(strAry[3]));
                        KeyActive();
                    }
                    else {uiText.text = "<color=red>BtnPress連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "BtnPredict":
                    NetClientGetCount++;
                    if (int.Parse(strAry[1]) == NetClientGetCount)
                    {
                        Frame2Pos = 0;
                        Frame2PosX = int.Parse(strAry[2]);
                        Frame2PosY = int.Parse(strAry[3]);
                        BtnPredict(int.Parse(strAry[2]),int.Parse(strAry[3]),int.Parse(strAry[4]));
                        KeyActive();
                    }
                    else {uiText.text = "<color=red>BtnPredict連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "BtnEightCheck":
                    NetClientGetCount++;
                    if (int.Parse(strAry[1]) == NetClientGetCount)
                    {
                        Frame2Pos = 0;
                        Frame2PosX = int.Parse(strAry[2]);
                        Frame2PosY = int.Parse(strAry[3]);
                        BtnEightCheck(int.Parse(strAry[2]),int.Parse(strAry[3]));
                        KeyActive();
                    }
                    else {uiText.text = "<color=red>BtnEightCheck連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "NumOfOut":
                    NetClientGetCount++;
                    if (int.Parse(strAry[1]) == NetClientGetCount)
                    {
                        NumOfOut = int.Parse(strAry[2]);
                        uiText.text = "<color=magenta>隊友踩到地雷已陣亡...</color>\n" + uiText.text;
                    }
                    else {uiText.text = "<color=red>NumOfOut連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "NumOfOut2":
                    NetClientGetCount++;
                    if (int.Parse(strAry[1]) == NetClientGetCount)
                    {
                        NumOfOut = 0;
                        uiText.text = "<color=magenta>隊友踩到地雷已陣亡...</color>\n" + uiText.text;
                        SetController(true);
                        stagephase = 2;
                        FramePos = 1;
                        FramePos2 = 2;
                        FrameAni.Play("FrameL");
                        Frame2Pos = 1;
                        Frame2Pos2 = 2;
                        Frame2Ani.Play("Frame2L");
                        KeyActive();
                    }
                    else {uiText.text = "<color=red>NumOfOut2連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "SpdModeWin":
                    NetClientGetCount++;
                    if (int.Parse(strAry[1]) == NetClientGetCount)
                    {
                        uiTextSlider.text = SceneControl.GameName+"\n"+SceneControl.GameName2;
                        uiText.text = "<color=magenta>糟了，" + SceneControl.GameName2 + "解答失敗！錯誤" + strAry[2] + "處</color>\n" + uiText.text;
                        uiText.text = "<color=white>" + SceneControl.GameName2 + "自爆，因此是" + SceneControl.GameName + "的勝利！</color>\n" + uiText.text;
                        SetController(true);
                        stagephase = 2;
                        FramePos = 1;
                        FramePos2 = 2;
                        FrameAni.Play("FrameL");
                        Frame2Pos = 1;
                        Frame2Pos2 = 2;
                        KeyActive();
                        BtnCheckAns.GetComponent<Button>().interactable = false;
                    }
                    else {uiText.text = "<color=red>SpdModeWin連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "SpdModeWin2":
                    NetClientGetCount++;
                    if (int.Parse(strAry[1]) == NetClientGetCount)
                    {
                        uiTextSlider.text = SceneControl.GameName+"\n"+SceneControl.GameName2;
                        uiText.text = "<color=magenta>糟了，" + SceneControl.GameName2 + "踩到地雷！</color>\n" + uiText.text;
                        uiText.text = "<color=white>" + SceneControl.GameName2 + "自爆，因此是" + SceneControl.GameName + "的勝利！</color>\n" + uiText.text;
                        SetController(true);
                        stagephase = 2;
                        FramePos = 1;
                        FramePos2 = 2;
                        FrameAni.Play("FrameL");
                        Frame2Pos = 1;
                        Frame2Pos2 = 2;
                        KeyActive();
                        BtnCheckAns.GetComponent<Button>().interactable = false;
                    }
                    else {uiText.text = "<color=red>SpdModeWin2連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "SpdModeLose":
                    NetClientGetCount++;
                    if (int.Parse(strAry[1]) == NetClientGetCount)
                    {
                        Slider2Float = 1.0f;
                        Slider2.value = 1.0f;
                        uiTextSlider.text = SceneControl.GameName+"\n"+SceneControl.GameName2;
                        uiText.text = "<color=magenta>糟了！" + SceneControl.GameName2 + "成功解除地雷了！</color>\n" + uiText.text;
                        uiText.text = "<color=white>此局由" + SceneControl.GameName2 + "獲得勝利！</color>\n" + uiText.text;
                        SetController(true);
                        stagephase = 2;
                        FramePos = 1;
                        FramePos2 = 2;
                        FrameAni.Play("FrameL");
                        Frame2Pos = 1;
                        Frame2Pos2 = 2;
                        KeyActive();
                        BtnCheckAns.GetComponent<Button>().interactable = false;
                    }
                    else {uiText.text = "<color=red>SpdModeLose連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "FrameChgPos":
                    NetClientGetCount++;
                    if (int.Parse(strAry[1]) == NetClientGetCount)
                    {
                        Frame2Pos = int.Parse(strAry[2]);
                        Frame2Pos2 = int.Parse(strAry[3]);
                        Frame2PosX = int.Parse(strAry[4]);
                        Frame2PosY = int.Parse(strAry[5]);
                        KeyActive();
                    }
                    else {uiText.text = "<color=red>FrameChgPos連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "SliderVal":
                    NetClientGetCount++;
                    if (int.Parse(strAry[1]) == NetClientGetCount)
                    {
                        Slider2Float = float.Parse(strAry[2]);
                    }
                    else {uiText.text = "<color=red>SliderVal連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "PlayerName":
                    SceneControl.GameName2 = strAry[1];
                    if (SceneControl.GameMode == 2)
                        uiTextSlider.text = SceneControl.GameName+"\n"+SceneControl.GameName2;
                    else
                        uiTextSlider.text = SceneControl.GameName+"\n";
                    break;
                case "Alarm":
                    switch (strAry[2])
                    {
                        case "101":
                            NetClientGetCount = 0;
                            DestroyBtn();
                            SetController(true);
                            FramePos = 1;
                            FramePos2 = 2;
                            Frame2Pos = 1;
                            Frame2Pos2 = 2;
                            KeyActive();
                            break;
                        default:
                            NetClientGetCount = 0;
                            break;
                    }
                    uiText.text = "<color=red>錯誤" + int.Parse(strAry[2]) + ": " + strAry[3] + "</color>\n" + uiText.text;
                    break;
                default:
                    uiText.text = "<color=red>不明的指令@___@</color> " + "\n" + uiText.text;
                    break;
            }
            uiText.text = "" + NetHost.recvStrPcs + "\n" + uiText.text;
            NetHost.recvStrPcs = "";
        }

        if (SceneControl.GameNet == 2 && NetClient.recvStrPcs != "")
        {
            string[] strAry = NetClient.recvStrPcs.Split(',');
            switch (strAry[0])
            {
                case "CrtBtn":
                    NetHostGetCount++;
                    if (int.Parse(strAry[1]) == NetHostGetCount)
                    {
                        DestroyBtn();
                        InputFieldX.text = strAry[2];
                        StageNumX = int.Parse(strAry[2]);
                        InputFieldY.text = strAry[3];
                        StageNumY = int.Parse(strAry[3]);
                        uiText.text = "<color=magenta>服務端已決定棋盤大小...</color>\n" + uiText.text;
                        stagephase = 0;
                        WrongAns = 0;
                        GuessMPlus = 0;
                        GuessMMinus = 0;
                        SetController(true);
                        SliderFloat = 0f;
                        Slider.value = 0f;
                        Slider2Float = 0f;
                        Slider2.value = 0f;
                        switch (SceneControl.GameMode)
                        {
                            case 0:
                            case 1:
                            case 3:
                                uiTextSlider.text = SceneControl.GameName + "\n";
                                break;
                            case 2:
                                uiTextSlider.text = SceneControl.GameName + "\n" + SceneControl.GameName2;
                                break;
                            default:
                                break;
                        }
                    }
                    else {uiText.text = "<color=red>CrtBtn連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "BtnPressInit":
                    NetHostGetCount++;
                    if (int.Parse(strAry[1]) == NetHostGetCount)
                    {
                        CrtBtn();
                        InputFieldMinePlus.text = strAry[2];
                        StageNumMPlus = int.Parse(strAry[2]);
                        InputFieldMineMinus.text = strAry[3];
                        StageNumMPlus = int.Parse(strAry[3]);
                        for (int i = 1; i <= 30; i++)
                        {
                            for (int j = 1; j <= 25; j++)
                            {
                                sgmine[i-1,j-1] = int.Parse(strAry[i*25+j-20]);
                            }
                        }
                        Frame2Pos = 0;
                        Frame2PosX = int.Parse(strAry[4]);
                        Frame2PosY = int.Parse(strAry[5]);
                        BtnPress(int.Parse(strAry[4]),int.Parse(strAry[5]));
                        KeyActive();
                        uiText.text = "<color=magenta>服務端已決定地雷數量並開始遊戲...</color>\n" + uiText.text;
                    }
                    else {uiText.text = "<color=red>BtnPressInit連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "BtnPress":
                    NetHostGetCount++;
                    if (int.Parse(strAry[1]) == NetHostGetCount)
                    {
                        Frame2Pos = 0;
                        Frame2PosX = int.Parse(strAry[2]);
                        Frame2PosY = int.Parse(strAry[3]);
                        BtnPress(int.Parse(strAry[2]),int.Parse(strAry[3]));
                        KeyActive();
                    }
                    else {uiText.text = "<color=red>BtnPress連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "BtnPredict":
                    NetHostGetCount++;
                    if (int.Parse(strAry[1]) == NetHostGetCount)
                    {
                        Frame2Pos = 0;
                        Frame2PosX = int.Parse(strAry[2]);
                        Frame2PosY = int.Parse(strAry[3]);
                        BtnPredict(int.Parse(strAry[2]),int.Parse(strAry[3]),int.Parse(strAry[4]));
                        KeyActive();
                    }
                    else {uiText.text = "<color=red>BtnPredict連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "BtnEightCheck":
                    NetHostGetCount++;
                    if (int.Parse(strAry[1]) == NetHostGetCount)
                    {
                        Frame2Pos = 0;
                        Frame2PosX = int.Parse(strAry[2]);
                        Frame2PosY = int.Parse(strAry[3]);
                        BtnEightCheck(int.Parse(strAry[2]),int.Parse(strAry[3]));
                        KeyActive();
                    }
                    else {uiText.text = "<color=red>BtnEightCheck連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "NumOfOut":
                    NetHostGetCount++;
                    if (int.Parse(strAry[1]) == NetHostGetCount)
                    {
                        NumOfOut = int.Parse(strAry[2]);
                        uiText.text = "<color=magenta>隊友踩到地雷已陣亡...</color>\n" + uiText.text;
                    }
                    else {uiText.text = "<color=red>NumOfOut連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "NumOfOut2":
                    NetHostGetCount++;
                    if (int.Parse(strAry[1]) == NetHostGetCount)
                    {
                        NumOfOut = 0;
                        uiText.text = "<color=magenta>隊友踩到地雷已陣亡...</color>\n" + uiText.text;
                        SetController(true);
                        stagephase = 2;
                        FramePos = 1;
                        FramePos2 = 2;
                        FrameAni.Play("FrameL");
                        Frame2Pos = 1;
                        Frame2Pos2 = 2;
                        Frame2Ani.Play("Frame2L");
                        KeyActive();
                    }
                    else {uiText.text = "<color=red>NumOfOut2連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "SpdModeWin":
                    NetHostGetCount++;
                    if (int.Parse(strAry[1]) == NetHostGetCount)
                    {
                        uiTextSlider.text = SceneControl.GameName+"\n"+SceneControl.GameName2;
                        uiText.text = "<color=magenta>糟了，" + SceneControl.GameName2 + "解答失敗！錯誤" + strAry[2] + "處</color>\n" + uiText.text;
                        uiText.text = "<color=white>" + SceneControl.GameName2 + "自爆，因此是" + SceneControl.GameName + "的勝利！</color>\n" + uiText.text;
                        SetController(true);
                        stagephase = 2;
                        FramePos = 1;
                        FramePos2 = 2;
                        FrameAni.Play("FrameL");
                        Frame2Pos = 1;
                        Frame2Pos2 = 2;
                        KeyActive();
                        BtnCheckAns.GetComponent<Button>().interactable = false;
                    }
                    else {uiText.text = "<color=red>SpdModeWin連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "SpdModeWin2":
                    NetHostGetCount++;
                    if (int.Parse(strAry[1]) == NetHostGetCount)
                    {
                        uiTextSlider.text = SceneControl.GameName+"\n"+SceneControl.GameName2;
                        uiText.text = "<color=magenta>糟了，" + SceneControl.GameName2 + "踩到地雷！</color>\n" + uiText.text;
                        uiText.text = "<color=white>" + SceneControl.GameName2 + "自爆，因此是" + SceneControl.GameName + "的勝利！</color>\n" + uiText.text;
                        SetController(true);
                        stagephase = 2;
                        FramePos = 1;
                        FramePos2 = 2;
                        FrameAni.Play("FrameL");
                        Frame2Pos = 1;
                        Frame2Pos2 = 2;
                        KeyActive();
                        BtnCheckAns.GetComponent<Button>().interactable = false;
                    }
                    else {uiText.text = "<color=red>SpdModeWin2連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "SpdModeLose":
                    NetHostGetCount++;
                    if (int.Parse(strAry[1]) == NetHostGetCount)
                    {
                        Slider2Float = 1.0f;
                        Slider2.value = 1.0f;
                        uiTextSlider.text = SceneControl.GameName+"\n"+SceneControl.GameName2;
                        uiText.text = "<color=magenta>糟了！" + SceneControl.GameName2 + "成功解除地雷了！</color>\n" + uiText.text;
                        uiText.text = "<color=white>此局由" + SceneControl.GameName2 + "獲得勝利！</color>\n" + uiText.text;
                        SetController(true);
                        stagephase = 2;
                        FramePos = 1;
                        FramePos2 = 2;
                        FrameAni.Play("FrameL");
                        Frame2Pos = 1;
                        Frame2Pos2 = 2;
                        KeyActive();
                        BtnCheckAns.GetComponent<Button>().interactable = false;
                    }
                    else {uiText.text = "<color=red>SpdModeLose連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "FrameChgPos":
                    NetHostGetCount++;
                    if (int.Parse(strAry[1]) == NetHostGetCount)
                    {
                        Frame2Pos = int.Parse(strAry[2]);
                        Frame2Pos2 = int.Parse(strAry[3]);
                        Frame2PosX = int.Parse(strAry[4]);
                        Frame2PosY = int.Parse(strAry[5]);
                        KeyActive();
                    }
                    else {uiText.text = "<color=red>FrameChgPos連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "SliderVal":
                    NetHostGetCount++;
                    if (int.Parse(strAry[1]) == NetHostGetCount)
                    {
                        Slider2Float = float.Parse(strAry[2]);
                    }
                    else {uiText.text = "<color=red>SliderVal連線出錯>___<</color>\n" + uiText.text; ResetBtn();}
                    break;
                case "PlayerName":
                    SceneControl.GameName2 = strAry[1];
                    if (SceneControl.GameMode == 2)
                        uiTextSlider.text = SceneControl.GameName+"\n"+SceneControl.GameName2;
                    else
                        uiTextSlider.text = SceneControl.GameName+"\n";
                    break;
                case "Alarm":
                    switch (strAry[2])
                    {
                        case "100":
                            NetHostGetCount = 0;
                            DestroyBtn();
                            SetController(true);
                            FramePos = 1;
                            FramePos2 = 2;
                            Frame2Pos = 1;
                            Frame2Pos2 = 2;
                            KeyActive();
                            break;
                        default:
                            NetHostGetCount = 0;
                            break;
                    }
                    uiText.text = "<color=red>錯誤" + int.Parse(strAry[2]) + ": " + strAry[3] + "</color>\n" + uiText.text;
                    break;
                default:
                    uiText.text = "<color=red>不明的指令@___@</color> " + "\n" + uiText.text;
                    break;
            }
            uiText.text = "" + NetClient.recvStrPcs + "\n" + uiText.text;
            NetClient.recvStrPcs = "";
        }

        //抓取鍵盤按Space的動作
        if (Input.GetKeyDown("tab")) {KeyTabFg = true;}
        if (Input.GetKeyUp("tab"))
        {
            if (FramePos == 0 && KeyTabFg) {FramePos = 1; KeyTabFg = false;}
            if (FramePos == 1 && KeyTabFg) {FramePos = 2; KeyTabFg = false;}
            if (FramePos == 2 && KeyTabFg) {FramePos = 0; KeyTabFg = false;}
            //跑迴圈確認Frame要跑到哪裡
            bool CheckEnable = true;
            while(CheckEnable)
            {
                CheckEnable = false;
                if (FramePos == 0 && (FramePosX == -1 || FramePosY == -1)) {FramePos = 1; CheckEnable = true;}
                if (FramePos == 1 && !ControllerEnable) {FramePos = 2; CheckEnable = true;}
                if (FramePos == 2 && !BtnCheckAns.GetComponent<Button>().interactable) {FramePos = 0; CheckEnable = true;}
            }


            KeyActive();
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
            if (SceneControl.GameNet == 1 && stagephase != 0 && SceneControl.GameMode != 2)
            {
                NetHostCount++;
                NetHost.StringIntegrate("FrameChgPos," + NetHostCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            if (SceneControl.GameNet == 2 && stagephase != 0 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("FrameChgPos," + NetClientCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            KeyActive();
        }
        if (KeyDownTm > 0f && !KeyDownFg && FramePos == 0 && FramePosY < StageNumY-1)
        {
            FramePosY++;
            KeyDownFg = true;
            if (SceneControl.GameNet == 1 && stagephase != 0 && SceneControl.GameMode != 2)
            {
                NetHostCount++;
                NetHost.StringIntegrate("FrameChgPos," + NetHostCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            if (SceneControl.GameNet == 2 && stagephase != 0 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("FrameChgPos," + NetClientCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            KeyActive();
        }
        if (KeyLeftTm > 0f && !KeyLeftFg && FramePos == 0 && FramePosX > 0)
        {
            FramePosX--;
            KeyLeftFg = true;
            if (SceneControl.GameNet == 1 && stagephase != 0 && SceneControl.GameMode != 2)
            {
                NetHostCount++;
                NetHost.StringIntegrate("FrameChgPos," + NetHostCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            if (SceneControl.GameNet == 2 && stagephase != 0 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("FrameChgPos," + NetClientCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            KeyActive();
        }
        if (KeyRightTm > 0f && !KeyRightFg && FramePos == 0 && FramePosX < StageNumX-1)
        {

            FramePosX++;
            KeyRightFg = true;
            if (SceneControl.GameNet == 1 && stagephase != 0 && SceneControl.GameMode != 2)
            {
                NetHostCount++;
                NetHost.StringIntegrate("FrameChgPos," + NetHostCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            if (SceneControl.GameNet == 2 && stagephase != 0 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("FrameChgPos," + NetClientCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            KeyActive();
        }
        if (KeyUpTm > 0.15f && KeyUpTm > KeyUpTm2+0.05f && FramePos == 0 && FramePosY > 0)
        {
            FramePosY--;
            KeyUpTm2 = KeyUpTm2+0.05f;
            if (SceneControl.GameNet == 1 && stagephase != 0 && SceneControl.GameMode != 2)
            {
                NetHostCount++;
                NetHost.StringIntegrate("FrameChgPos," + NetHostCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            if (SceneControl.GameNet == 2 && stagephase != 0 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("FrameChgPos," + NetClientCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            KeyActive();
        }
        if (KeyDownTm > 0.15f && KeyDownTm > KeyDownTm2+0.05f && FramePos == 0 && FramePosY < StageNumY-1)
        {
            FramePosY++;
            KeyDownTm2 = KeyDownTm2+0.05f;
            if (SceneControl.GameNet == 1 && stagephase != 0 && SceneControl.GameMode != 2)
            {
                NetHostCount++;
                NetHost.StringIntegrate("FrameChgPos," + NetHostCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            if (SceneControl.GameNet == 2 && stagephase != 0 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("FrameChgPos," + NetClientCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            KeyActive();
        }
        if (KeyLeftTm > 0.15f && KeyLeftTm > KeyLeftTm2+0.05f && FramePos == 0 && FramePosX > 0)
        {
            FramePosX--;
            KeyLeftTm2 = KeyLeftTm2+0.05f;
            if (SceneControl.GameNet == 1 && stagephase != 0 && SceneControl.GameMode != 2)
            {
                NetHostCount++;
                NetHost.StringIntegrate("FrameChgPos," + NetHostCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            if (SceneControl.GameNet == 2 && stagephase != 0 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("FrameChgPos," + NetClientCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            KeyActive();
        }
        if (KeyRightTm > 0.15f && KeyRightTm > KeyRightTm2+0.05f && FramePos == 0 && FramePosX < StageNumX-1)
        {
            FramePosX++;
            KeyRightTm2 = KeyRightTm2+0.05f;
            if (SceneControl.GameNet == 1 && stagephase != 0 && SceneControl.GameMode != 2)
            {
                NetHostCount++;
                NetHost.StringIntegrate("FrameChgPos," + NetHostCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            if (SceneControl.GameNet == 2 && stagephase != 0 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("FrameChgPos," + NetClientCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            KeyActive();
        }
        //如果上下左右被按下，則變更Frame的位置(在右上角輸入欄時)
        if (KeyUpTm > 0f && !KeyUpFg && FramePos == 1)
        {
            FramePos2--;
            KeyUpFg = true;
            if (SceneControl.GameNet == 1 && SceneControl.GameMode != 2)
            {
                NetHostCount++;
                NetHost.StringIntegrate("FrameChgPos," + NetHostCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            if (SceneControl.GameNet == 2 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("FrameChgPos," + NetClientCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            KeyActive();
        }
        if (KeyDownTm > 0f && !KeyDownFg && FramePos == 1)
        {
            FramePos2++;
            KeyDownFg = true;
            if (SceneControl.GameNet == 1 && SceneControl.GameMode != 2)
            {
                NetHostCount++;
                NetHost.StringIntegrate("FrameChgPos," + NetHostCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            if (SceneControl.GameNet == 2 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("FrameChgPos," + NetClientCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            KeyActive();
        }
        if (KeyLeftTm > 0f && !KeyLeftFg && FramePos == 1)
        {
            FramePos2--;
            KeyLeftFg = true;
            if (SceneControl.GameNet == 1 && SceneControl.GameMode != 2)
            {
                NetHostCount++;
                NetHost.StringIntegrate("FrameChgPos," + NetHostCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            if (SceneControl.GameNet == 2 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("FrameChgPos," + NetClientCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            KeyActive();
        }
        if (KeyRightTm > 0f && !KeyRightFg && FramePos == 1)
        {
            FramePos2++;
            KeyRightFg = true;
            if (SceneControl.GameNet == 1 && SceneControl.GameMode != 2)
            {
                NetHostCount++;
                NetHost.StringIntegrate("FrameChgPos," + NetHostCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            if (SceneControl.GameNet == 2 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("FrameChgPos," + NetClientCount + "," + FramePos + "," + FramePos2 + "," + FramePosX + "," + FramePosY + ";");
            }
            KeyActive();
        }
        if (FramePos2 < 0) {FramePos2 = 4;}
        if (FramePos2 > 4) {FramePos2 = 0;}

        //滑鼠游標的位置
        // Vector3 mousePos = Input.mousePosition;
        // Debug.Log(mousePos.x+", "+mousePos.y);

        //偵測滑鼠左右鍵/鍵盤Space&Ctrl是否被按下
        if (Input.GetMouseButtonDown(0)) {MouseDownLeft = true;}
        if (Input.GetMouseButtonDown(1)) {MouseDownRight = true;}
        if (Input.GetButtonDown("Jump")||Input.GetKeyDown("h")||Input.GetKeyDown("enter")) {KeyDownLeft = true;}
        if (Input.GetButtonDown("Fire1")||Input.GetKeyDown("j")) {KeyDownRight = true;}
        if (Input.GetKeyDown("n")) {KeyDownMPlus = true;}
        if (Input.GetKeyDown("m")) {KeyDownMMinus = true;}
        if (Input.GetKeyDown(",")) {KeyDownMQues = true;}

        //滑鼠左鍵按下彈起在創建關卡按鈕上，則創建關卡
        if (Input.GetMouseButtonUp(0) && MouseDownLeft && HoverCrtStage && MouseDownOnBtnCrtStage && SceneControl.GameNet != 2)
        {
            KeyActive();
            CrtBtn();

            if (SceneControl.GameNet == 1)
            {
                NetHostCount++;
                NetHost.StringIntegrate("CrtBtn," + NetHostCount + "," + StageNumX + "," + StageNumY + ";");
            }
        }
        if ((Input.GetButtonUp("Jump")||Input.GetKeyUp("h")||Input.GetKeyUp("enter")) && FramePos == 1 && FramePos2 == 2 && SceneControl.GameNet != 2)
        {
            CrtBtn();

            if (SceneControl.GameNet == 1)
            {
                NetHostCount++;
                NetHost.StringIntegrate("CrtBtn," + NetHostCount + "," + StageNumX + "," + StageNumY + ";");
            }
        }

        //滑鼠左鍵按下彈起在檢查答案按鈕上，則檢查答案
        if (Input.GetMouseButtonUp(0) && MouseDownLeft && HoverCheckAns && MouseDownOnBtnCheckAns && stagephase == 1)
        {
            if (SceneControl.GameMode <= 2)
                CheckBtnAnswer();
        }
        if ((Input.GetButtonUp("Jump")||Input.GetKeyUp("h")||Input.GetKeyUp("enter")) && FramePos == 2 && stagephase == 1)
        {
            if (SceneControl.GameMode <= 2)
                CheckBtnAnswer();
        }


        //若單純只有滑鼠左鍵按下彈起，則點開該Btn內容物
        if (Input.GetMouseButtonUp(0) && MouseDownLeft && !MouseDownRight && MouseDownOnBtn && stagephase == 1 && HoverX != -1 && HoverY != -1)
        {
            FramePosX = HoverX;
            FramePosY = HoverY;
            KeyActive();
            BtnPress(HoverX, HoverY);

            if (SceneControl.GameNet == 1 && SceneControl.GameMode != 2)
            {
                NetHostCount++;
                NetHost.StringIntegrate("BtnPress," + NetHostCount + "," + SelectX + "," + SelectY + ";");
            }
            if (SceneControl.GameNet == 2 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("BtnPress," + NetClientCount + "," + SelectX + "," + SelectY + ";");
            }
        }
        if ((Input.GetButtonUp("Jump")||Input.GetKeyUp("h")) && KeyDownLeft && !KeyDownRight && stagephase == 1 && FramePos == 0)
        {
            KeyActive();
            BtnPress(FramePosX, FramePosY);

            if (SceneControl.GameNet == 1 && SceneControl.GameMode != 2)
            {
                NetHostCount++;
                NetHost.StringIntegrate("BtnPress," + NetHostCount + "," + SelectX + "," + SelectY + ";");
            }
            if (SceneControl.GameNet == 2 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("BtnPress," + NetClientCount + "," + SelectX + "," + SelectY + ";");
            }
        }
        if (Input.GetMouseButtonUp(0) && MouseDownLeft && !MouseDownRight && MouseDownOnBtn && stagephase == 0 && HoverX != -1 && HoverY != -1)
        {
            FramePosX = HoverX;
            FramePosY = HoverY;
            Frame2Pos = 0;
            Frame2PosX = 0;
            Frame2PosY = 0;
            KeyActive();
            BtnPress(HoverX, HoverY);

            if (SceneControl.GameNet == 1)
            {
                NetHostCount++;
                string strtmp = "BtnPressInit," + NetHostCount + "," + StageNumMPlus + "," + StageNumMMinus + "," + SelectX + "," + SelectY;
                for (int i = 1; i <= 30; i++)
                {
                    for (int j = 1; j <= 25; j++)
                    {
                        strtmp = strtmp + "," + sgmine[i-1,j-1];
                    }
                }
                NetHost.StringIntegrate(strtmp + ";");
            }
        }
        if ((Input.GetButtonUp("Jump")||Input.GetKeyUp("h")) && KeyDownLeft && !KeyDownRight && stagephase == 0 && FramePos == 0)
        {
            Frame2Pos = 0;
            Frame2PosX = 0;
            Frame2PosY = 0;
            KeyActive();
            BtnPress(FramePosX, FramePosY);

            if (SceneControl.GameNet == 1)
            {
                NetHostCount++;
                string strtmp = "BtnPressInit," + NetHostCount + "," + StageNumMPlus + "," + StageNumMMinus + "," + SelectX + "," + SelectY;
                for (int i = 1; i <= 30; i++)
                {
                    for (int j = 1; j <= 25; j++)
                    {
                        strtmp = strtmp + "," + sgmine[i-1,j-1];
                    }
                }
                NetHost.StringIntegrate(strtmp + ";");
            }
        }

        //若單純只有滑鼠右鍵按下彈起，則開關預測可能為地雷的功能
        if (Input.GetMouseButtonUp(1) && !MouseDownLeft && MouseDownRight && MouseDownOnBtn && stagephase == 1 && HoverX != -1 && HoverY != -1)
        {
            FramePosX = HoverX;
            FramePosY = HoverY;
            KeyActive();
            BtnPredict(HoverX, HoverY, 0);
            if (SceneControl.GameNet == 1 && SceneControl.GameMode != 2)
            {
                NetHostCount++;
                NetHost.StringIntegrate("BtnPredict," + NetHostCount + "," + SelectX + "," + SelectY + ",0" + ";");
            }
            if (SceneControl.GameNet == 2 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("BtnPredict," + NetClientCount + "," + SelectX + "," + SelectY + ",0" + ";");
            }
        }
        if ((Input.GetButtonUp("Fire1")||Input.GetKeyUp("j")) && !KeyDownLeft && KeyDownRight && stagephase == 1 && FramePos == 0)
        {
            KeyActive();
            BtnPredict(FramePosX, FramePosY, 0);
            if (SceneControl.GameNet == 1 && SceneControl.GameMode != 2)
            {
                NetHostCount++;
                NetHost.StringIntegrate("BtnPredict," + NetHostCount + "," + SelectX + "," + SelectY + ",0" + ";");
            }
            if (SceneControl.GameNet == 2 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("BtnPredict," + NetClientCount + "," + SelectX + "," + SelectY + ",0" + ";");
            }
        }
        if (Input.GetKeyUp("n") && KeyDownMPlus && stagephase == 1 && FramePos == 0)
        {
            KeyActive();
            BtnPredict(FramePosX, FramePosY, 1);
            if (SceneControl.GameNet == 1 && SceneControl.GameMode != 2)
            {
                NetHostCount++;
                NetHost.StringIntegrate("BtnPredict," + NetHostCount + "," + SelectX + "," + SelectY + ",1" + ";");
            }
            if (SceneControl.GameNet == 2 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("BtnPredict," + NetClientCount + "," + SelectX + "," + SelectY + ",1" + ";");
            }
        }
        if (Input.GetKeyUp("m") && KeyDownMMinus && stagephase == 1 && FramePos == 0)
        {
            KeyActive();
            BtnPredict(FramePosX, FramePosY, 2);
            if (SceneControl.GameNet == 1 && SceneControl.GameMode != 2)
            {
                NetHostCount++;
                NetHost.StringIntegrate("BtnPredict," + NetHostCount + "," + SelectX + "," + SelectY + ",2" + ";");
            }
            if (SceneControl.GameNet == 2 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("BtnPredict," + NetClientCount + "," + SelectX + "," + SelectY + ",2" + ";");
            }
        }
        if (Input.GetKeyUp(",") && KeyDownMQues && stagephase == 1 && FramePos == 0)
        {
            KeyActive();
            BtnPredict(FramePosX, FramePosY, 3);
            if (SceneControl.GameNet == 1 && SceneControl.GameMode != 2)
            {
                NetHostCount++;
                NetHost.StringIntegrate("BtnPredict," + NetHostCount + "," + SelectX + "," + SelectY + ",3" + ";");
            }
            if (SceneControl.GameNet == 2 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("BtnPredict," + NetClientCount + "," + SelectX + "," + SelectY + ",3" + ";");
            }
        }

        //若滑鼠左鍵與滑鼠右鍵同時按下並且其中一鍵彈起，則確認附近8格Btn預測地雷數與點下Btn數值是否相同，進而開拓其他位置的Btn
        if ((Input.GetMouseButtonUp(0)||Input.GetMouseButtonUp(1)) && stagephase == 1 && MouseDownLeft && MouseDownRight && MouseDownOnBtn && HoverX != -1 && HoverY != -1)
        {
            FramePosX = HoverX;
            FramePosY = HoverY;
            KeyActive();
            BtnEightCheck(HoverX, HoverY);
            if (SceneControl.GameNet == 1 && SceneControl.GameMode != 2)
            {
                NetHostCount++;
                NetHost.StringIntegrate("BtnEightCheck," + NetHostCount + "," + SelectX + "," + SelectY + ";");
            }
            if (SceneControl.GameNet == 2 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("BtnEightCheck," + NetClientCount + "," + SelectX + "," + SelectY + ";");
            }
        }
        if ((Input.GetButtonUp("Jump")||Input.GetKeyUp("h")||Input.GetButtonUp("Fire1")||Input.GetKeyUp("j")) && stagephase == 1 && KeyDownLeft && KeyDownRight && FramePos == 0)
        {
            KeyActive();
            BtnEightCheck(FramePosX, FramePosY);
            if (SceneControl.GameNet == 1 && SceneControl.GameMode != 2)
            {

                NetHostCount++;
                NetHost.StringIntegrate("BtnEightCheck," + NetHostCount + "," + SelectX + "," + SelectY + ";");
            }
            if (SceneControl.GameNet == 2 && SceneControl.GameMode != 2)
            {
                NetClientCount++;
                NetClient.StringIntegrate("BtnEightCheck," + NetClientCount + "," + SelectX + "," + SelectY + ";");
            }
        }
        if ((Input.GetButtonUp("Jump")||Input.GetKeyUp("h")||Input.GetButtonUp("Fire1")||Input.GetKeyUp("j")) && stagephase == 0 && KeyDownLeft && KeyDownRight && FramePos == 0)
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

        //偵測滑鼠左右鍵是否被彈起
        if (Input.GetMouseButtonUp(0)) {MouseDownLeft = false; MouseDownOnBtn = false; MouseDownOnBtnCrtStage = false; MouseDownOnBtnCheckAns = false;}
        if (Input.GetMouseButtonUp(1)) {MouseDownRight = false; MouseDownOnBtn = false; MouseDownOnBtnCrtStage = false; MouseDownOnBtnCheckAns = false;}
        if (Input.GetButtonUp("Jump")||Input.GetKeyUp("h")||Input.GetKeyUp("enter")) {KeyDownLeft = false;}
        if (Input.GetButtonUp("Fire1")||Input.GetKeyUp("j")) {KeyDownRight = false;}
        if (Input.GetKeyUp("n")) {KeyDownMPlus = false;}
        if (Input.GetKeyUp("m")) {KeyDownMMinus = false;}
        if (Input.GetKeyUp(",")) {KeyDownMQues = false;}

    }

}
