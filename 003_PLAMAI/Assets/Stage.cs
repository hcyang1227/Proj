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
    int[,] sgmine = new int[100,100];
    int[,] sgnum = new int[100,100];
    int[,] sgmap = new int[100,100];
    int StageNumX;
    int StageNumY;
    int StageNumMPlus;
    int StageNumMMinus;
    int stagephase = 0;

    public void CrtBtn()
    {

        //移除所有已創建的Btn
        for (int i = 1; i <= 100; i++)
        {
            for (int j = 1; j <= 100; j++)
            {
                Destroy(GameObject.Find("Btn" + i + "-" + j));
                sgmine[i-1,j-1] = 0;
                sgnum[i-1,j-1] = 0;
                sgmap[i-1,j-1] = 0;
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
        Clone.name = "Btn" + i + "-" + j;
    }

    public void BtnPress()
    {

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
            int MineMax = Convert.ToInt32(Math.Round(StageNumX*StageNumY/3.0, 0));
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
                AddMine(1);
            }
            for (int i = 0; i < StageNumMMinus; i++)
            {
                AddMine(-1);
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

            //Btn陣列顯示所有地雷
            RevealMine();

        }

    }

    //填入地雷
    public void AddMine(int MineType)
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
            if (sgmine[RndX,RndY] == 0)
            {
            sgmine[RndX,RndY] = MineType;
            BtnEmpty = true;
            }
        }

    }

    //顯示地雷
    public void RevealMine()
    {
        for (int i = 1; i <= StageNumX; i++)
        {
            for (int j = 1; j <= StageNumY; j++)
            {

                GameObject BtnGot = GameObject.Find("Btn" + i + "-" + j);
                GameObject BtnTxtGot = GameObject.Find("Btn" + i + "-" + j + "/Text");
                if(sgnum[i-1,j-1]!=9 && sgnum[i-1,j-1]!=-9){BtnTxtGot.GetComponent<Text>().text = ""+sgnum[i-1,j-1];}
                if(sgnum[i-1,j-1]==9)
                {
                    BtnGot.GetComponent<Image>().color = Color.red;
                    BtnTxtGot.GetComponent<Text>().text = "●";
                }
                if(sgnum[i-1,j-1]==-9)
                {
                    BtnGot.GetComponent<Image>().color = Color.cyan;
                    BtnTxtGot.GetComponent<Text>().text = "●";
                }
            }
        }
    }





}
