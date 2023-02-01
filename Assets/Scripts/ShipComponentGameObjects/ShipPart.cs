using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPart
{
    public int x;
    public int y;
    public int width;
    public int height;
    public ShipParts type;
    public GameObject part;
    public string orientation;


    public ShipPart(ShipParts type, int x, int y, int width, int height, string orientation = "forward"){
        this.type = type;
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        this.orientation = orientation;
    }
}
