using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public ShipBluePrint bluePrint;
    public GameObject basePoint;
    public GameObject[, ] body;
    public GameObject[, ] components;
    public int width;
    public int height;

    public void InitialiseShip(int width, int height){
        body = new GameObject[height, width];
        components = new GameObject[height, width];
        this.width = width;
        this.height = height;
    }

    public bool BodyExistsAt(int x, int y){
        if(x >= 0 && x < width && y >= 0 && y < height){
            return body[y, x] != null;
        }
        return false;
    }

    public GameObject GetBodyPart(int x, int y){
        if(x >= 0 && x < width && y >= 0 && y < height){
            return body[y, x];
        }

        return null;
    }

    public void AddBodyPart(GameObject bodyPart, int x, int y){
        if(x >= 0 && x < width && y >= 0 && y < height){
            body[y, x] = bodyPart;
        }
    }

    public void RemoveBodyPart(int x, int y){
        if(x >= 0 && x < width && y >= 0 && y < height){
            Destroy(body[y, x]);
            body[y, x] = null;
        }
    }

    public bool ComponentExistsAt(int x, int y){
        if(x >= 0 && x < width && y >= 0 && y < height){
            return components[y, x] != null;
        }

        return false;
    }

    public GameObject GetComponent(int x, int y){
        if(x >= 0 && x < width && y >= 0 && y < height){
            return body[y, x];
        }

        return null;
    }

    public void AddComponent(GameObject component, int x, int y){
        if(x >= 0 && x < width && y >= 0 && y < height){
            components[y, x] = component;
        }
    }

    public void RemoveComponent(int x, int y){
        if(x >= 0 && x < width && y >= 0 && y < height){
            Destroy(components[y, x]);
            components[y, x] = null;
        }
    }
}
