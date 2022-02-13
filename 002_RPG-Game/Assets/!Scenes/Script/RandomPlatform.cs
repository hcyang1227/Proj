using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlatform : MonoBehaviour
{
    public GameObject GroundGrid;
    public GameObject Platform;
    public GameObject[] PlatformObj;
    public GameObject[] EnemyObj;
    public GameObject Eagles;
    public GameObject Eagle;
    public GameObject Eagle1;
    public GameObject Eagle2;

    public GameObject LoadGameFlag;
    void Start()
    {
        //從存讀檔的讀檔bool值，決定要讀取檔案、還是另建新地圖
        if (GameObject.Find("有讀檔") == true)
        {
            print("讀取平板位置...");
        }
        else
        {
            //亂數產生地圖初始化
            int Monster = 13;
            float x = Platform.transform.position.x;
            float y = Platform.transform.position.y;
            int PltRnd;
            int facing = 1;
            int PltCount = 0;
            int PltTotalNum = 0;
            int EnmTotalNum = 3;
            float PltWidth = 130.0f;
            float PltHeight = 100.0f;
            float LeftWall = -400.0f;
            float RightWall = 1200.0f;
            PlatformObj = new GameObject[100]; //為避免生成platform高過array，設為100
            EnemyObj = new GameObject[30]; //為避免生成enemy高過array，設為30
            EnemyObj[0] = Eagle;
            EnemyObj[1] = Eagle1;
            EnemyObj[2] = Eagle2;

            do 
            {
                //產生PltRnd整數亂數，用來選擇要在產生點的何處產生platform
                PltRnd = UnityEngine.Random.Range(0, 10);
                bool CreatePlt = false;
                //連續生成水平平台的計數器
                PltCount = UnityEngine.Random.Range(0, 4);

                //case0~9依序為生成platform於產生點的左上\左上\左\左\右上\右上\右\右\左上右上\左上右上
                switch (PltRnd)
                {
                    case 0:
                        if (x>LeftWall && PltCount <= 0 && facing == 1)
                        {
                            x -= 1.5f*PltWidth;
                            y += PltHeight;
                            PltTotalNum++;
                            PlatformObj[PltTotalNum-1] = Instantiate(Platform, new Vector3(x,y,0), Platform.transform.rotation, GroundGrid.transform);
                            PlatformObj[PltTotalNum-1].name = "Platform"+(PltTotalNum-1);
                            CreatePlt = true;
                            // print("case0 左上產生平板");
                        }
                        break;
                    case 1:  
                        if (x>LeftWall && PltCount <= 0 && facing == 1)
                        {
                            x -= 1.5f*PltWidth;
                            y += PltHeight;
                            PltTotalNum++;
                            PlatformObj[PltTotalNum-1] = Instantiate(Platform, new Vector3(x,y,0), Platform.transform.rotation, GroundGrid.transform);
                            PlatformObj[PltTotalNum-1].name = "Platform"+(PltTotalNum-1);
                            CreatePlt = true;
                            // print("case1 左上產生平板");
                        }
                        break;
                    case 2:
                        if (x>LeftWall && PltCount > 0 && facing == 1)
                        {
                            x -= 1.5f*PltWidth;
                            PltTotalNum++;
                            PlatformObj[PltTotalNum-1] = Instantiate(Platform, new Vector3(x,y,0), Platform.transform.rotation, GroundGrid.transform);
                            PlatformObj[PltTotalNum-1].name = "Platform"+(PltTotalNum-1);
                            CreatePlt = true;
                            // print("case2 左方產生平板(有空隙)");
                        }
                        break;
                    case 3:
                        if (x>LeftWall && PltCount > 0 && facing == 1)
                        {
                            x -= PltWidth;
                            PltTotalNum++;
                            PlatformObj[PltTotalNum-1] = Instantiate(Platform, new Vector3(x,y,0), Platform.transform.rotation, GroundGrid.transform);
                            PlatformObj[PltTotalNum-1].name = "Platform"+(PltTotalNum-1);
                            CreatePlt = true;
                            // print("case3 左方產生平板(無空隙)");
                        }
                        break;
                    case 4:    
                        if (x<RightWall && PltCount <= 0 && facing == -1)
                        {
                            x += 1.5f*PltWidth;
                            y += PltHeight;
                            PltTotalNum++;
                            PlatformObj[PltTotalNum-1] = Instantiate(Platform, new Vector3(x,y,0), Platform.transform.rotation, GroundGrid.transform);
                            PlatformObj[PltTotalNum-1].name = "Platform"+(PltTotalNum-1);
                            CreatePlt = true;
                            // print("case4 右上產生平板");
                        }
                        break;
                    case 5:    
                        if (x<RightWall && PltCount <= 0 && facing == -1)
                        {
                            x += 1.5f*PltWidth;
                            y += PltHeight;
                            PltTotalNum++;
                            PlatformObj[PltTotalNum-1] = Instantiate(Platform, new Vector3(x,y,0), Platform.transform.rotation, GroundGrid.transform);
                            PlatformObj[PltTotalNum-1].name = "Platform"+(PltTotalNum-1);
                            CreatePlt = true;
                            // print("case5 右上產生平板");
                        }
                        break;
                    case 6:
                        if (x<RightWall && PltCount > 0 && facing == -1)
                        {
                            x += 1.5f*PltWidth;
                            PltTotalNum++;
                            PlatformObj[PltTotalNum-1] = Instantiate(Platform, new Vector3(x,y,0), Platform.transform.rotation, GroundGrid.transform);
                            PlatformObj[PltTotalNum-1].name = "Platform"+(PltTotalNum-1);
                            CreatePlt = true;
                            // print("case6 右方產生平板(有空隙)");
                        }
                        break;
                    case 7:
                        if (x<RightWall && PltCount > 0 && facing == -1)
                        {
                            x += PltWidth;
                            PltTotalNum++;
                            PlatformObj[PltTotalNum-1] = Instantiate(Platform, new Vector3(x,y,0) + new Vector3(x,0,0), Platform.transform.rotation, GroundGrid.transform);
                            PlatformObj[PltTotalNum-1].name = "Platform"+(PltTotalNum-1);
                            CreatePlt = true;
                            // print("case7 右方產生平板(無空隙)");
                        }
                        break;
                    case 8:
                        if (x>LeftWall && x<RightWall && PltCount <= 0)
                        {
                            x += 1.5f*PltWidth;
                            y += PltHeight;
                            PltTotalNum+=2;
                            PlatformObj[PltTotalNum-2] = Instantiate(Platform, new Vector3(x,y,0), Platform.transform.rotation, GroundGrid.transform);
                            PlatformObj[PltTotalNum-1] = Instantiate(Platform, new Vector3(x,y,0) + new Vector3(-3*PltWidth,0,0), Platform.transform.rotation, GroundGrid.transform);
                            PlatformObj[PltTotalNum-2].name = "Platform"+(PltTotalNum-2);
                            PlatformObj[PltTotalNum-1].name = "Platform"+(PltTotalNum-1);
                            facing = -1;
                            PltCount = UnityEngine.Random.Range(10, 20);
                            CreatePlt = true;
                            // print("case8 左上&右上產生平板，面對右方");
                        }
                        break;
                    case 9:    
                        if (x>LeftWall && x<RightWall && PltCount <= 0)
                        {
                            x += -1.5f*PltWidth;
                            y += PltHeight;
                            PltTotalNum+=2;
                            PlatformObj[PltTotalNum-2] = Instantiate(Platform, new Vector3(x,y,0), Platform.transform.rotation, GroundGrid.transform);
                            PlatformObj[PltTotalNum-1] = Instantiate(Platform, new Vector3(x,y,0) + new Vector3(3*PltWidth,0,0), Platform.transform.rotation, GroundGrid.transform);
                            PlatformObj[PltTotalNum-2].name = "Platform"+(PltTotalNum-2);
                            PlatformObj[PltTotalNum-1].name = "Platform"+(PltTotalNum-1);
                            facing = 1;
                            PltCount = UnityEngine.Random.Range(10, 20);
                            CreatePlt = true;
                            // print("case9 左上&右上產生平板，面對左方");
                        }
                        break;
                    default:
                        break;
                }
                if (x <= LeftWall)
                {
                    x += 2.0f*PltWidth;
                    y += PltHeight;
                    PltTotalNum++;
                    PlatformObj[PltTotalNum-1] = Instantiate(Platform, new Vector3(x,y,0), Platform.transform.rotation, GroundGrid.transform);
                    PlatformObj[PltTotalNum-1].name = "Platform"+(PltTotalNum-1);
                    CreatePlt = true;
                    PltCount = UnityEngine.Random.Range(10, 20);
                    facing = -1;
                    // print("平板超越左底線，返回並產生平板");
                }

                if (x >= RightWall)
                {
                    x -= 2.0f*PltWidth;
                    y += PltHeight;
                    PltTotalNum++;
                    PlatformObj[PltTotalNum-1] = Instantiate(Platform, new Vector3(x,y,0), Platform.transform.rotation, GroundGrid.transform);
                    PlatformObj[PltTotalNum-1].name = "Platform"+(PltTotalNum-1);
                    CreatePlt = true;
                    PltCount = UnityEngine.Random.Range(10, 20);
                    facing = 1;
                    // print("平板超越右底線，返回並產生平板");
                }

                //透過亂數隨機以30%機率在平台上產生敵人
                if (UnityEngine.Random.Range(0,100)<30 && CreatePlt == true)
                {
                    EnemyObj[EnmTotalNum] = Instantiate(Eagle, new Vector3(x+PltWidth/2.0f-UnityEngine.Random.Range(0.0f,PltWidth),y+80.0f,0),Platform.transform.rotation,Eagles.transform);
                    EnemyObj[EnmTotalNum].name = "Eagle"+(EnmTotalNum);
                    EnmTotalNum++;
                    Monster--;
                    CreatePlt = false;
                }
                else
                    CreatePlt = false;

                if (PltCount <= 0)
                {
                    PltCount = UnityEngine.Random.Range(2, 10);
                }
                else
                    PltCount -= 1;

                if (y > 2400)
                {
                    // print("平板超越頂部，結束生成");
                    break;
                }
            }
            while (Monster > 0);

            //有些平板莫名其妙產生超出邊界，將其刪除
            float PltX=0.0f;
            for (int i = 0; i < PltTotalNum; i++)
            {
                PltX = GameObject.Find("Platform"+i).transform.position.x;
                if ((PltX > RightWall) || (PltX < LeftWall))
                {
                    Destroy(GameObject.Find("Platform"+i));
                    PlatformObj[i] = null;
                }
            }

            //若平台產生完，怪物還有餘未生成的話，隨機找平板生成怪物
            if (Monster > 0)
            {
                do
                {
                    PltRnd = UnityEngine.Random.Range(0, PltTotalNum);
                    if (PlatformObj[PltRnd] != null)
                    {
                        EnemyObj[EnmTotalNum] = Instantiate(Eagle, PlatformObj[PltRnd].transform.position + new Vector3(0,80.0f,0), Platform.transform.rotation,Eagles.transform);
                        EnemyObj[EnmTotalNum].name = "Eagle"+(EnmTotalNum);
                        EnmTotalNum++;
                        Monster--;
                    }
                }
                while (Monster > 0);
            }
        }
    }

    void Update()
    {
        
    }
}
