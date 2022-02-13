using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
public GameObject MainCamera;
public GameObject HeroKnight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //以主角為中心，移動主攝影機位置
        if (HeroKnight)
        {
            Vector3 MainCmr = HeroKnight.GetComponent<Transform>().position;
            MainCamera.GetComponent<Transform>().position = MainCmr + new Vector3(0, 90, 0);
        }
    }
}
