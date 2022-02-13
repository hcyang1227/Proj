using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectNumber : MonoBehaviour
{
    public GameObject uiText;
    public GameObject targe; //找填入數獨的按鈕
    public GameObject NumTarge; //找選取數字的按鈕

    void Start()
    {
        NumTarge.GetComponent<Text>().text = "0";
    }
    void Update()
    {
    }
    public void SelectNumMethod(GameObject gameObject)
    {
        NumTarge = gameObject;
        uiText.GetComponent<Text>().text = "選取了" + NumTarge.GetComponent<Text>().text;
    }

    public void FillNumMethod(GameObject gameObject)
    {
        targe = gameObject;
        //顯示目前選取何數字
        targe.GetComponent<Text>().text = NumTarge.GetComponent<Text>().text;
        targe.GetComponentInParent<Image>().color = Color.yellow;
    }
}
