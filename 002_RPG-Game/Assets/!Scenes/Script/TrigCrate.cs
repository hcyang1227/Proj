using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigCrate : MonoBehaviour
{
    Dialog _Dialog;

    // Start is called before the first frame update
    void Start()
    {
        _Dialog = GameObject.Find("DialogPlatform").GetComponent<Dialog>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag=="Player")
        {
             _Dialog.OpenDialog("Crate");
        }
    }
}
