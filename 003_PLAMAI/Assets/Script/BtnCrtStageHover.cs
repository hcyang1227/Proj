using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BtnCrtStageHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        Stage.HoverCrtStage = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Stage.HoverCrtStage = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Stage.MouseDownOnBtnCrtStage = true;
        //Debug.Log("在 (" + Stage.HoverX + "," + Stage.HoverY + ")按下");
    }

}
