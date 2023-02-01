using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShield : MonoBehaviour
{
    float charge = 0f;
    public GameObject thrusterEffect;
    float thrustPower = 15f;
    public string orientation = "forward";

    public void SetOrientation(string orientation){
        this.orientation = orientation;
        switch(orientation){
            case "forward":
                break;
            case "left":
                transform.Rotate(new Vector3(0, 0, 90));
                break;
            case "right":
                transform.Rotate(new Vector3(0, 0, 270));
                break;
            case "backward":
                transform.Rotate(new Vector3(0, 0, 180));
                break;
            default:
                break;
        }
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {

    }


}
