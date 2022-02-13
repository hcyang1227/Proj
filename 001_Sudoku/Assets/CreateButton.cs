using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateButton : MonoBehaviour
{
public GameObject Button;
public GameObject Canvas;
public GameObject BtnSudoku;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i <= 9; i++)
        {
            for (int j = 1; j <= 9; j++)
            {
                CreateBtnLoop(i,j);
            } 
        }
    }

    public void CreateBtnLoop(int i, int j)
    {
        Vector3 myVector = Canvas.transform.position + new Vector3(i*55-500, 270-j*55, 0);
        GameObject Clone;
        Clone = (GameObject)Instantiate(Button, myVector, new Quaternion(), BtnSudoku.transform);
        Clone.name = "Btn" + i + "-" + j;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
