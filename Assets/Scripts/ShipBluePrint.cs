using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBluePrint
{
    public int width;
    public int height;
    bool[,] ShipFloor;
    List<ShipPart> components = new List<ShipPart>();

    public static ShipBluePrint GenerateShipExample1(){
        ShipBluePrint bluePrint = new ShipBluePrint();
        bluePrint.GenerateNewShipCore(21, 21);
        bluePrint.AddShipFloor(10, 10);
        bluePrint.AddComponent(ShipParts.Heart, 10, 10, 1, 1);

        return bluePrint;
    }

    public void GenerateNewShipCore(int width, int height){
        this.width = width;
        this.height = height;
        ShipFloor = new bool[height, width]; 
    }

    public bool[,] GetShipFloor(){
        return ShipFloor;
    }

    public List<ShipPart> GetComponents(){
        return components;
    }

    public (int, int) GetShipDimensions(){
        return (width, height);
    }

    public bool GetShipFloor(int x, int y){
        if(x < 0 || y < 0 || x >= width || y >= width){
            return false;
        }
        return ShipFloor[y, x];
    }

    public void AddShipFloor(int x, int y){
        ShipFloor[y, x] = true;
    }

    public void RemoveShipFloor(int x, int y){
        ShipFloor[y, x] = false;
    }

    public void AddComponent(ShipParts partType, int x, int y, int width, int height, string orientation = "forward"){
        ShipPart part = new ShipPart(partType, x, y, width, height, orientation);
        if(!(GetComponentAt(x, y) is null)){
            return;
        }

        components.Add(part);
    }

    public ShipPart GetComponentAt(int x, int y){
        foreach(ShipPart component in components){
            if(x > component.x && x < component.x + component.width){
                if(y > component.y && y < component.y + component.height){
                    return component;
                }
            }
        }

        return null;
    }

    public Vector3 GetShipCenter(){
        int itemCount = 0;
        float x = 0;
        float y = 0;
        for(int j = 0; j < height; j++){
            for(int i = 0; i < width; i++){
                itemCount++;
                x += i;
                y += j;
            }
        }
        return new Vector3((x / itemCount) + 1, (y / itemCount) + 1, 0f);

    }
}
