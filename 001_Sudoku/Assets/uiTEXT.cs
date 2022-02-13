using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //必填

public class uiTEXT : MonoBehaviour
{
    public GameObject TESTUI;
    public GameObject Canvas2;

    // Start is called before the first frame update
    void Start()
    {
        TESTUI=GameObject.Find("uiText");
        TESTUI.GetComponent<Text>().text="你好";

        Canvas2=GameObject.Find("Canvas2");

        Instantiate(TESTUI, new Vector3(3,0,0), new Quaternion(0,90,0,0), Canvas2.transform);
        Instantiate(TESTUI, Canvas2.transform.position, Canvas2.transform.rotation, Canvas2.transform);

        Button btn = this.GetComponent<Button> (); //限定UI Button
        btn.onClick.AddListener (good);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void good(){
        print("我按下按鈕");
    }
}
