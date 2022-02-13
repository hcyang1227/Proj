using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sudoku : MonoBehaviour
{
public GameObject uiText;
public GameObject InputFld;
public GameObject BtnDigHole;
Color BtnColor = new Color(149, 169, 245, 255);
int solveStep = 0;
int[,] sudoku = new int[9, 9];
int[,] solution1 = new int[9, 9];
int[,] solution2 = new int[9, 9];
int[,] n = new int[81, 2];
int nlen = 0;

static int[,] sudokuSample = new int[9, 9]
        {
        {0,0,0,7,2,8,0,0,0},
        {0,9,0,0,5,1,6,0,0},
        {0,0,0,0,6,0,0,8,2},
        {3,0,0,8,0,2,7,0,4},
        {1,7,4,0,3,0,0,2,0},
        {2,8,0,5,0,0,0,3,0},
        {0,1,0,3,0,0,2,0,0},
        {0,0,7,0,4,6,0,0,5},
        {0,0,6,1,0,0,0,4,9}};

	public void SudokuSolution1()
	{
		for(int i=0;i<9;i++){
			for(int j=0;j<9;j++){
				sudoku[i,j] = solution1[i,j];
				string str="Btn"+(i+1)+"-"+(j+1);
				GameObject Btn = GameObject.Find(str);
				Btn.GetComponent<Image>().color = BtnColor;
			}
		}
		uiText.GetComponent<Text>().text = "答案1盤面";
		Show();	
	}
	public void SudokuSolution2()
	{
		for(int i=0;i<9;i++){
			for(int j=0;j<9;j++){
				sudoku[i,j] = solution2[i,j];
				string str="Btn"+(i+1)+"-"+(j+1);
				GameObject Btn = GameObject.Find(str);
				Btn.GetComponent<Image>().color = BtnColor;
			}
		}
		uiText.GetComponent<Text>().text = "答案2盤面";
		Show();	
	}
	public void SudokuSample()
	{
		BtnDigHole.SetActive(false);
		for(int i=0;i<9;i++){
			for(int j=0;j<9;j++){
				sudoku[i,j] = sudokuSample[i,j];
				if (sudoku[i,j] == 0)
				{
				string str="Btn"+(i+1)+"-"+(j+1);
				GameObject Btn = GameObject.Find(str);
				Btn.GetComponent<Image>().color = Color.gray;
				}
				else
				{
				string str="Btn"+(i+1)+"-"+(j+1);
				GameObject Btn = GameObject.Find(str);
				Btn.GetComponent<Image>().color = BtnColor;
				}
			}
		}
		
		uiText.GetComponent<Text>().text = "使用數獨樣本";
		Show();
		
	}

	public void InitSudoku()
	{
		BtnDigHole.SetActive(false);
		for (int i=0;i<9;i++) 
		{
			for (int j=0;j<9;j++)
			{
				sudoku[i,j] = 0;
				string str="Btn"+(i+1)+"-"+(j+1);
				GameObject Btn = GameObject.Find(str);
				Btn.GetComponent<Image>().color = Color.gray;
			}
		}
		Show();
		solveStep = 0;
		uiText.GetComponent<Text>().text = "清空盤面";

		
	}

    int[] GetRandomArray()
	{
		int[] result = new int[] {1,2,3,4,5,6,7,8,9};
		int loopNum = UnityEngine.Random.Range(10,100);
		for (int i = 0; i < loopNum; i++)
		{
			int a = UnityEngine.Random.Range(0,9);
			int b = UnityEngine.Random.Range(0,9);
            while(a == b)
            {
                b = UnityEngine.Random.Range(0,9);
            }
			int temp = 0;
			temp = result[a];
			result[a] = result[b];
			result[b] = temp;
		}
		return result;
	}

	public void DiggingHole()
	{
		BtnDigHole.SetActive(false);

		int digHoleNum = 15;
		string digHoleText = InputFld.GetComponent<Text>().text;
		
		if ((digHoleText == "") || (int.Parse(digHoleText) > 50) || (int.Parse(digHoleText) <= 0))
			{
				InputFld.GetComponent<Text>().text = "15";
				digHoleNum = 15;
				uiText.GetComponent<Text>().text = "進行挖洞，共挖15個洞";
			}
		else
			digHoleNum = int.Parse(digHoleText);
			uiText.GetComponent<Text>().text = "進行挖洞，共挖"+digHoleNum.ToString()+"個洞";

		for (int i = 0; i < digHoleNum; i++)
		{
			int a = UnityEngine.Random.Range(0,9);
			int b = UnityEngine.Random.Range(0,9);
            while(sudoku[a,b] == 0)
            {
			a = UnityEngine.Random.Range(0,9);
			b = UnityEngine.Random.Range(0,9);
            }
			sudoku[a,b] = 0;
			string str="Btn"+(a+1)+"-"+(b+1);
			GameObject Btn = GameObject.Find(str);
			Btn.GetComponent<Image>().color = Color.gray;
		}
		Show();
	}

    void SolveSudoku()
	{
		nlen = 0;
		n = new int[81, 2];
		solution1 = new int[9, 9];
		solution2 = new int[9, 9];

		//將盤面上的文字存入sudoku
		for (int i = 1; i <= 9; i++)
		{
			for (int j = 1; j <= 9; j++)
			{
                string str="Btn"+i+"-"+j+"/Text";
				GameObject Btn = GameObject.Find(str);
                sudoku[i-1,j-1] = int.Parse(Btn.GetComponent<Text>().text);
				Btn.GetComponentInParent<Image>().color = BtnColor;
			}
		}
		//搜尋數獨區塊為零的地方
		for (int i = 0; i < 9; i++)
		{
			for (int j = 0; j < 9; j++)
			{
				if(sudoku[i,j]==0)
				{
					n[nlen, 0] = i;
					n[nlen, 1] = j;
					nlen++;
				}
			}
		}
		//用遞迴法搜尋可能解答
		forcefillsdk(1);


	}

	void forcefillsdk(int i)
	{
		if(solveStep>=30000)
		{
			uiText.GetComponent<Text>().text = "解題超過30000步，強制終止";
			return;		
		}
		if((i > nlen) && (solution1[0,0] == 0))
		{
			Show();
			for(int a=0;a<9;a++){
				for(int b=0;b<9;b++){
					solution1[a,b] = sudoku[a,b];
				}
			}
			uiText.GetComponent<Text>().text = "完成解題，找到答案1";
			//所有空格填回0
			for (int k = 0; k < nlen; k++)
			{
				sudoku[n[k, 0],n[k, 1]] = 0;
			}
			forcefillsdk(1);
			return;
		}
		if((i > nlen) && (solution1[0,0] != 0) && (solution2[0,0] == 0))
		{
			Show();
			for(int a=0;a<9;a++){
				for(int b=0;b<9;b++){
					solution2[a,b] = sudoku[a,b];
				}
			}
			uiText.GetComponent<Text>().text = "完成解題，找到答案1與答案2";
			return;
		}
		if(i > nlen)
		{
			uiText.GetComponent<Text>().text = "完成解題，終止程序";
			return;
		}
		for (int k = 1; k <= 9; k++)
		{
			if (solution1[0,0] == 0)
				sudoku[n[i-1, 0],n[i-1, 1]] += 1;	//解答一用正向搜尋
			else
				sudoku[n[i-1, 0],n[i-1, 1]] -= 1;	//解答二用逆向搜尋

			if (sudoku[n[i-1, 0],n[i-1, 1]] > 9)
			{
				sudoku[n[i-1, 0],n[i-1, 1]] = 1;
			}

			if (sudoku[n[i-1, 0],n[i-1, 1]] < 0)
			{
				sudoku[n[i-1, 0],n[i-1, 1]] = 9;
			}

			if(isValid(n[i-1, 0], n[i-1, 1]))
			{
				// print("第"+i+"("+n[i-1, 0]+","+n[i-1, 1]+")被填滿"+sudoku[n[i-1, 0],n[i-1, 1]]);
				solveStep++;
				forcefillsdk(i+1);
			}
			else
			{
				solveStep++;
			}
		}
	// print("第"+i+"("+n[i-1, 0]+","+n[i-1, 1]+")錯誤，數字歸零");
	sudoku[n[i-1, 0],n[i-1, 1]] = 0;
	}

    //驗證函數
	bool isValid(int i, int j)
	{
		int n = sudoku[i,j];
		int[] query = new int[9] {0,0,0,3,3,3,6,6,6};
		int t, u;
		//本身是否為零
		if (sudoku[i,j] == 0)
			return false;
        //每一行每一列是否重複
		for (t=0;t<9;t++)
		{
			if ((t != i && sudoku[t,j]==n)||(t != j && sudoku[i,t]==n))
				return false;
		}
        //每一個九宮格是否重複
        for (t = query[i]; t < query[i] + 3; t++)
        {
            for (u = query[j]; u < query[j] + 3; u++)
            {
                if ((t != i || u != j) && sudoku[t,u] == n)
                    return false;
            }
        }
		return true;
	}
	public void CheckSdkCorrectness()
	{
		//將盤面上的文字存入sudoku
		for (int i = 1; i <= 9; i++)
		{
			for (int j = 1; j <= 9; j++)
			{
                string str="Btn"+i+"-"+j+"/Text";
				GameObject Btn = GameObject.Find(str);
                sudoku[i-1,j-1] = int.Parse(Btn.GetComponent<Text>().text);
			}
		}
		bool Correctness = true;
		for (int i = 0; i < 9; i++)
		{
			for (int j = 0; j < 9; j++)
			{
                if(isValid(i,j) == false)
				{
					Correctness = false;
					string str="Btn"+(i+1)+"-"+(j+1);
					GameObject Btn = GameObject.Find(str);
					Btn.GetComponent<Image>().color = Color.red;
				}
				else
				{
					string str="Btn"+(i+1)+"-"+(j+1);
					GameObject Btn = GameObject.Find(str);
					Btn.GetComponent<Image>().color = BtnColor;
				}
			}
		}
		if (Correctness)
		{
			uiText.GetComponent<Text>().text = "數獨盤面正確無誤";
			BtnDigHole.SetActive(true);
		}
		else
			uiText.GetComponent<Text>().text = "數獨盤面有以下紅色錯誤";

	}
	void Show()
	{
		for (int i = 1; i <= 9; i++)
		{
			for (int j = 1; j <= 9; j++)
			{
                string str="Btn"+i+"-"+j+"/Text";
				GameObject Btn = GameObject.Find(str);
                Btn.GetComponent<Text>().text = ""+sudoku[i-1,j-1];
			}
		}
	}

    public void CreateGameFull()
    {
		uiText.GetComponent<Text>().text = "開始產題...";
			solveStep = 0;
			InitSudoku();
			for (int i = 0; i < 3; i++)
			{
				int[] temp = GetRandomArray();
				int p = 0;
				for (int j =0+(i*3);j<3+(i*3);j++)
				{
					for (int k =0+(i*3); k<3+(i*3);k++)
					{
						sudoku[j,k] = temp[p];
						p++;
					}
				}
			}
			Show();
			SolveSudoku();
		Show();
    }
	public void AutoSolveSudoku(int n)
	{
		//將盤面上的文字存入sudoku
		for (int i = 1; i <= 9; i++)
		{
			for (int j = 1; j <= 9; j++)
			{
                string str="Btn"+i+"-"+j+"/Text";
				GameObject Btn = GameObject.Find(str);
                sudoku[i-1,j-1] = int.Parse(Btn.GetComponent<Text>().text);
			}
		}
		solveStep = 0;
		SolveSudoku();
		solveStep = 0;
	}

    void Start()
    {
		BtnDigHole.SetActive(false);
    }

    void Update()
    {

    }
}
