using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
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
    string getOrientationActivation(){
        switch(orientation){
            case "forward":
                return "w";
            case "left":
                return "a";
            case "right":
                return "d";
            case "backward":
                return "s";
            default:
                return "w";
        }
    }
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        RemoveThrustEffect();

        if(Input.GetKey(getOrientationActivation())){
            Thrust();
        }
    }

    void Thrust(){
        thrusterEffect.SetActive(true);
        Vector3 force  = transform.up * thrustPower;
        GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Force);

    }

    void RemoveThrustEffect(){
        thrusterEffect.SetActive(false);
    }
}
