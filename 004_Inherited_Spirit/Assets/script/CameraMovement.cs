using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject ObjPlayer;

    private Camera MainCamera;
    float CameraX; //Camera目前位置x
    float CameraY; //Camera目前位置y
    float CameraZoomX; //Camera放大目標位置x
    float CameraZoomY; //Camera放大目標位置y
    float CameraZoomSpeed = 2f; //Camera size放大速度
    bool camera_zoom = false; //false代表camera在巨觀模式/true代表camera在微觀模式
    bool camera_zoom_enable = false; //false代表camera不會zoom/true代表camera會zoom
    bool camera_chase = true; //false代表camera不去追主角/true代表camera會追主角

    System.Random rand = new System.Random(Guid.NewGuid().GetHashCode()); //定義亂數種子rand

    // Start is called before the first frame update
    void Awake()
    {
        MainCamera = Camera.main;
#if UNITY_EDITOR
        if (MainCamera == null)
        {
            Debug.LogWarning("Main camera is null!");
            enabled = false;
            return;
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        CameraX = MainCamera.transform.position.x;
        CameraY = MainCamera.transform.position.y;

        //!!!temp!!!
        if (Input.GetKeyDown("z"))
        {
            camera_zoom = !camera_zoom;
            camera_zoom_enable = true;
            CameraZoomSpeed = rand.Next(100,500)/100f;
            // Debug.Log("toggle camera zoom");
        }
        if (Input.GetKeyDown("a"))
        {
            camera_chase = !camera_chase;
            // Debug.Log("toggle camera chase");
        }

        //camera以指數形式追蹤主視點
        if (camera_chase)
        {
            CameraZoomX = ObjPlayer.transform.position.x;
            CameraZoomY = ObjPlayer.transform.position.y + 75;
            if (!Field.frame_unchange && (CameraX > CameraZoomX + 1))
                CameraX -= Mathf.Pow(1.2f,(CameraX - CameraZoomX)/50f)*13f-12f;
            if (!Field.frame_unchange && (CameraX < CameraZoomX - 1))
                CameraX += Mathf.Pow(1.2f,-(CameraX - CameraZoomX)/50f)*13f-12f;
            if (!Field.frame_unchange && (CameraY > CameraZoomY + 1))
                CameraY -= Mathf.Pow(1.2f,(CameraY - CameraZoomY)/50f)*13f-12f;
            if (!Field.frame_unchange && (CameraY < CameraZoomY - 1))
                CameraY += Mathf.Pow(1.2f,-(CameraY - CameraZoomY)/50f)*13f-12f;
        }

        //camera自動位移&放大縮小
        if (camera_zoom && camera_zoom_enable && !Field.frame_unchange && (MainCamera.orthographicSize > 100))
            MainCamera.orthographicSize -= CameraZoomSpeed;
        if (!camera_zoom && camera_zoom_enable && !Field.frame_unchange && (MainCamera.orthographicSize < 300))
            MainCamera.orthographicSize += CameraZoomSpeed;
        if (MainCamera.orthographicSize < 100)
            MainCamera.orthographicSize = 100;
        if (MainCamera.orthographicSize > 300)
            MainCamera.orthographicSize = 300;

        //camera上下左右邊界限制
        if (CameraX - MainCamera.orthographicSize < -300)
            CameraX = -300 + MainCamera.orthographicSize;
        if (CameraX + MainCamera.orthographicSize > 300)
            CameraX = 300 - MainCamera.orthographicSize;
        if (CameraY - MainCamera.orthographicSize < -300)
            CameraY = -300 + MainCamera.orthographicSize;
        if (CameraY + MainCamera.orthographicSize > 300)
            CameraY = 300 - MainCamera.orthographicSize;

        MainCamera.transform.position = new Vector3(CameraX, CameraY, MainCamera.transform.position.z);
    }
}
