using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGenerator : MonoBehaviour
{
    void Start()
    {
        GetComponent<ShipBuilding>().currentShip = GenerateShip(ShipBluePrint.GenerateShipExample1());
    }

    void Update()
    {

    }

    public GameObject GenerateShip(ShipBluePrint bluePrint){
        bool[,] shipFloor = bluePrint.GetShipFloor();
        (int width, int height) = bluePrint.GetShipDimensions();
        GameObject ship = new GameObject();
        ship.name = "ship";
        ship.tag = "PlayerShip";
        GameObject[, ] shipBody = new GameObject[height, width];
        
        Ship shipScript = ship.AddComponent<Ship>();
        shipScript.InitialiseShip(width, height);
        shipScript.bluePrint = bluePrint;

        GameObject shipBasePoint = new GameObject();
        shipBasePoint.name = "BasePoint";
        shipBasePoint.transform.parent = ship.transform;
        shipBasePoint.transform.position = -new Vector3(0f, 0f, 0f);
        shipScript.basePoint = shipBasePoint;

        GetComponent<ShipBuilding>().currentShip = ship;

        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                if(bluePrint.GetShipFloor(x, y)){
                    GetComponent<ShipBuilding>().TryAddFloor(new Vector3(x, y, 0f), true);
                }
                
            }
        }  

        foreach(ShipPart component in bluePrint.GetComponents()){
            GetComponent<ShipBuilding>().AddComponent(ship, component);
        }

        return ship;
    }

    
}
