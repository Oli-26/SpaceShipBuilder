using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(".")){
            Camera.main.GetComponent<Camera>(). orthographicSize--;
        }

        if(Input.GetKeyDown(",")){
            Camera.main.GetComponent<Camera>().orthographicSize++;
        }
    }
}
