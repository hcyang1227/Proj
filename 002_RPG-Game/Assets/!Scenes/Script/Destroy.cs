using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(_Destroy(0.8f));
    }

    // Update is called once per frame
    IEnumerator _Destroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
