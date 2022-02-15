using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateBG : MonoBehaviour
{
    public GameObject Btn;
    public GameObject BtnZone;
    public GameObject Canvas;
    public InputField InputFieldX;
    public InputField InputFieldY;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void CrtBtn()
    {
        for (int i = 1; i <= 100; i++)
        {
            for (int j = 1; j <= 100; j++)
            {
                Destroy(GameObject.Find("Btn" + i + "-" + j));
            }
        }

        int StageNumX;
        string StageTxtX = InputFieldX.text;
        if ( (StageTxtX == "") || (int.Parse(StageTxtX) > 30) || (int.Parse(StageTxtX) <= 4))
        {
            InputFieldX.text = "15";
            StageNumX = 15;
        }
        else
        {
            StageNumX = int.Parse(StageTxtX);
        }

        int StageNumY;
        string StageTxtY = InputFieldY.text;
        if ( (StageTxtY == "") || (int.Parse(StageTxtY) > 24) || (int.Parse(StageTxtY) <= 4))
        {
            InputFieldY.text = "15";
            StageNumY = 15;
        }
        else
        {
            StageNumY = int.Parse(StageTxtY);
        }


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
        Vector3 myVector = Canvas.transform.position + new Vector3(30*i-500, 360-30*j, 0);
        GameObject Clone;
        Clone = (GameObject)Instantiate(Btn, myVector, new Quaternion(), BtnZone.transform);
        Clone.name = "Btn" + i + "-" + j;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
