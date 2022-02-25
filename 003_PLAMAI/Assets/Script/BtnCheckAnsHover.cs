using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BtnCheckAnsHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        Stage.HoverCheckAns = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Stage.HoverCheckAns = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Stage.MouseDownOnBtnCheckAns = true;
        //Debug.Log("在 (" + Stage.HoverX + "," + Stage.HoverY + ")按下");
    }

}
