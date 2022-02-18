using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public Text uiText;
    int[,] sgmine = new int[100,100]; //stage mine，1:正電地雷 -1:負電地雷
    int[,] sgnum = new int[100,100]; //stage number，9:正電地雷 -9:負電地雷 0:附近沒地雷 其他:附近有幾個地雷(正負相消)
    int[,] sgmap = new int[100,100]; //stage map，0:未開 1:剛踩探測是否為0 2:確認為0、其他數字或地雷
    int[,] sgopnbtn = new int[100,100]; //stage open button，記憶可以開啟的複數位置，0:沒有 1:有
    int StageNumX;
    int StageNumY;
    int StageNumMPlus;
    int StageNumMMinus;
    int stagephase = 0;
    int SelectX;
    int SelectY;
    bool ContinueZero = false;
    int stageTm = 0;

    public void CrtBtn()
    {

        //回歸關卡階段到0
        stagephase = 0;

        //移除所有已創建的Btn
        for (int i = 1; i <= 100; i++)
        {
            for (int j = 1; j <= 100; j++)
            {
                Destroy(GameObject.Find("Btn" + i + "-" + j));
                sgmine[i-1,j-1] = 0;
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

    }


    public void CrtBtnLp(int i, int j)
    {
        Vector3 myVector = Canvas.transform.position + new Vector3(30*i-StageNumX*15-15, StageNumY*15-30*j+15, 0);
        GameObject Clone;
        Clone = (GameObject)Instantiate(Btn, myVector, new Quaternion(), BtnZone.transform);
        Clone.GetComponent<Image>().color = Color.gray;
        Clone.name = "Btn" + i + "-" + j;
    }

    public void BtnPress(GameObject GameObject)
    {
        //從按下按鈕的名稱擷取Btn位於array的位置
        string str = GameObject.name;
        str = str.Remove(0,3);
        string[] strAry = str.Split('-');
        SelectX = int.Parse(strAry[0])-1;
        SelectY = int.Parse(strAry[1])-1;
        uiText.text = "選取了 (" + SelectX + "," + SelectY + ")";

        //當所有Btn按鈕都未被按的時候，第一個Btn按下必不為地雷，同時設定所有地雷位置
        if (stagephase == 0)
        {

            bool StageBoolMPlus = int.TryParse(InputFieldMinePlus.text, out StageNumMPlus);
            bool StageBoolMMinus = int.TryParse(InputFieldMineMinus.text, out StageNumMMinus);

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
            for (int i = 0; i < StageNumMPlus; i++)
            {
                AddMine(1,SelectX,SelectY);
            }
            for (int i = 0; i < StageNumMMinus; i++)
            {
                AddMine(-1,SelectX,SelectY);
            }

            //計算地雷周邊數字，地雷本身為9或是-9
            for (int i = 0; i < StageNumX; i++)
            {
                for (int j = 0; j < StageNumY; j++)
                {
                    if (sgmine[i,j] != 0)
                    {
                        sgnum[i,j] = sgmine[i,j]*9;
                    }
                    else
                    {
                        if (i<=0 && j<=0) {sgnum[i,j] = sgmine[i+1,j] + sgmine[i+1,j+1] + sgmine[i,j+1];}
                        else if (i<=0 && j>0 && j<StageNumY-1) {sgnum[i,j] = sgmine[i,j-1] + sgmine[i+1,j-1] + sgmine[i+1,j] + sgmine[i+1,j+1] + sgmine[i,j+1];}
                        else if (i<=0 && j>=StageNumY-1) {sgnum[i,j] = sgmine[i,j-1] + sgmine[i+1,j-1] + sgmine[i+1,j];}
                        else if (i>0 && i<StageNumX-1 && j>=StageNumY-1) {sgnum[i,j] = sgmine[i-1,j] + sgmine[i-1,j-1] + sgmine[i,j-1] + sgmine[i+1,j-1] + sgmine[i+1,j];}
                        else if (i>=StageNumX-1 && j>=StageNumY-1) {sgnum[i,j] = sgmine[i-1,j] + sgmine[i-1,j-1] + sgmine[i,j-1];}
                        else if (i>=StageNumX-1 && j>0 && j<StageNumY-1) {sgnum[i,j] = sgmine[i,j+1] + sgmine[i-1,j+1] + sgmine[i-1,j] + sgmine[i-1,j-1] + sgmine[i,j-1];}
                        else if (i>=StageNumX-1 && j<=0) {sgnum[i,j] = sgmine[i,j+1] + sgmine[i-1,j+1] + sgmine[i-1,j];}
                        else if (i>0 && i<StageNumX-1 && j<=0) {sgnum[i,j] = sgmine[i+1,j] + sgmine[i+1,j+1] + sgmine[i,j+1] + sgmine[i-1,j+1] + sgmine[i-1,j];}
                        else {sgnum[i,j] = sgmine[i+1,j] + sgmine[i+1,j+1] + sgmine[i,j+1] + sgmine[i-1,j+1] + sgmine[i-1,j] + sgmine[i-1,j-1] + sgmine[i,j-1] + sgmine[i+1,j-1];}
                    }
                }
            }

            //轉換關卡階段到1
            stagephase = 1;
        }

        //關卡階段1的情況下點擊尚未開啟的Btn，則翻開該Btn
        if (stagephase == 1 && sgmap[SelectX,SelectY] == 0)
        {
            sgmap[SelectX,SelectY] = 1;
            CheckBtnStatus(SelectX, SelectY);
            if (sgnum[SelectX,SelectY] == 0) {ContinueZero = true;}
        }

        //Btn陣列顯示所有地雷
        //RevealMine();

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
            Color BtnColor = new Color(1f, 1f-sgnum[i,j]/8f, 1f-sgnum[i,j]/8f, 1f);
            BtnGot.GetComponent<Image>().color = BtnColor;
            BtnTxtGot.GetComponent<Text>().text = ""+sgnum[i,j];
        }
        if (sgnum[i,j]>-9 && sgnum[i,j]<0)
        {
            Color BtnColor = new Color(1f+sgnum[i,j]/8f, 1f, 1f, 1f);
            BtnGot.GetComponent<Image>().color = BtnColor;
            BtnTxtGot.GetComponent<Text>().text = ""+sgnum[i,j];
        }
        if (sgnum[i,j]==0)
        {
            BtnGot.GetComponent<Image>().color = Color.white;
            BtnTxtGot.GetComponent<Text>().text = "";
            ContinueZero = true;
        }
        if (sgnum[i,j]==9)
        {
            BtnGot.GetComponent<Image>().color = Color.red;
            BtnTxtGot.GetComponent<Text>().text = "●";
        }
        if (sgnum[i,j]==-9)
        {
            BtnGot.GetComponent<Image>().color = Color.cyan;
            BtnTxtGot.GetComponent<Text>().text = "●";
        }
    }

    public void CheckUDLRBtn(int i, int j)
    {
        if (i > 0 && Math.Abs(sgnum[i-1,j])!=9)
        {
            CheckBtnStatus(i-1,j);
            sgmap[i-1,j] = 1;
        }
        if (i < StageNumX-1 && Math.Abs(sgnum[i+1,j])!=9)
        {
            CheckBtnStatus(i+1,j);
            sgmap[i+1,j] = 1;
        }
        if (j > 0 && Math.Abs(sgnum[i,j-1])!=9)
        {
            CheckBtnStatus(i,j-1);
            sgmap[i,j-1] = 1;
        }
        if (j < StageNumY-1 && Math.Abs(sgnum[i,j+1])!=9)
        {
            CheckBtnStatus(i,j+1);
            sgmap[i,j+1] = 1;
        }

        if (i > 0 && j > 0 && Math.Abs(sgnum[i-1,j-1])!=9)
        {
            CheckBtnStatus(i-1,j-1);
            sgmap[i-1,j-1] = 1;
        }
        if (i < StageNumX-1 && j < StageNumY-1 && Math.Abs(sgnum[i+1,j+1])!=9)
        {
            CheckBtnStatus(i+1,j+1);
            sgmap[i+1,j+1] = 1;
        }
        if (i < StageNumX-1 && j > 0 && Math.Abs(sgnum[i+1,j-1])!=9)
        {
            CheckBtnStatus(i+1,j-1);
            sgmap[i+1,j-1] = 1;
        }
        if (i > 0 && j < StageNumY-1 && Math.Abs(sgnum[i-1,j+1])!=9)
        {
            CheckBtnStatus(i-1,j+1);
            sgmap[i-1,j+1] = 1;
        }
    }

    void Update()
    {
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
                    CheckUDLRBtn(i, j);
                    sgopnbtn[i,j] = 0;
                    sgmap[i,j] = 2;
                    }
                }
            }
        }

    }

}
