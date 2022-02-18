using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BtnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        //從掠過按鈕的名稱擷取Btn位於array的位置
        string str = name;
        str = str.Remove(0,3);
        string[] strAry = str.Split('-');
        Stage.HoverX = int.Parse(strAry[0])-1;
        Stage.HoverY = int.Parse(strAry[1])-1;
        //Debug.Log("滑鼠游標掠過 (" + Stage.HoverX + "," + Stage.HoverY + ")");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Stage.HoverX = -1;
        Stage.HoverY = -1;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Stage.MouseDownOnBtn = true;
        //Debug.Log("在 (" + Stage.HoverX + "," + Stage.HoverY + ")按下");
    }

}
