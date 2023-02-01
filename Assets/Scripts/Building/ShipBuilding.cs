using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShipBuilding : MonoBehaviour
{
    public GameObject currentShip;
    public ShipParts buildPart = ShipParts.Floor;
    public string currentOrientation = "forward";
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Add(Vector3 worldPosition, bool ignoreGridLimits = false){
        switch(buildPart){
            case ShipParts.Floor:
                TryAddFloor(worldPosition, ignoreGridLimits);
                break;
            default:
                TryAddComponent(worldPosition, buildPart, ignoreGridLimits);
                break;
        }
    }

    public void Remove(Vector3 worldPosition){
        if(GridHelpers.WithinBuildBounds(currentShip, worldPosition)){
            (int x, int y) = GridHelpers.DetermineGridCoordinate(currentShip, worldPosition);
            if(currentShip.GetComponent<Ship>().ComponentExistsAt(x, y)){
                TryRemoveComponent(worldPosition);
                return;
            }

            if(currentShip.GetComponent<Ship>().BodyExistsAt(x, y)){
                TryRemoveFloor(worldPosition);
            }
        }
    }
    public void TryAddComponent(Vector3 worldPosition, ShipParts type, bool ignoreGridLimits){
        if(ignoreGridLimits || GridHelpers.WithinBuildBounds(currentShip, worldPosition)){
            (int x, int y) = GridHelpers.DetermineGridCoordinate(currentShip, worldPosition);
            if(currentShip.GetComponent<Ship>().BodyExistsAt(x, y)){
                ShipPart part = new ShipPart(type, x, y, 1, 1, currentOrientation);
                AddComponent(currentShip, part);
            }
        }
    }

    public void TryAddFloor(Vector3 worldPosition, bool ignoreConstraints){
        if(ignoreConstraints || GridHelpers.WithinBuildBounds(currentShip, worldPosition)){
            (int x, int y) = GridHelpers.DetermineGridCoordinate(currentShip, worldPosition);
            
            AddFloor(currentShip, x, y, ignoreConstraints);
        }
    }

    public void TryRemoveFloor(Vector3 worldPosition){
        (int x, int y) = GridHelpers.DetermineGridCoordinate(currentShip, worldPosition);
        RemoveFloor(currentShip, x, y);
    }

    public void TryRemoveComponent(Vector3 worldPosition){
        (int x, int y) = GridHelpers.DetermineGridCoordinate(currentShip, worldPosition);
        RemoveComponent(currentShip, x, y);
    }

    private void AddFloor(GameObject ship, int x, int y, bool ignoreConstraints = false){
        Ship shipScript = ship.GetComponent<Ship>();
        int width = shipScript.width;
        int height = shipScript.height;
        if(shipScript.BodyExistsAt(x,y) == false){
           (List<(int, int)> points, List<bool> pointsExist) = GetSurroundingPoints(x, y, shipScript);
            
            if(!ignoreConstraints && !pointsExist.Any(truthy => truthy)){
                return;
            }

            Quaternion rotation = Quaternion.identity;
            if(currentShip.transform.parent != null){
                rotation = currentShip.transform.parent.rotation;
            }

            GameObject floorComponent = Instantiate(GetComponent<PartHolder>().shipFloorComponentPrefab, new Vector3(0, 0, 0), rotation);
            shipScript.AddBodyPart(floorComponent, x, y);

            floorComponent.transform.parent = currentShip.transform;
            Vector3 offsetFromBase = GridHelpers.DetermineRotatedCoordinatesFromBase(currentShip, shipScript.basePoint.transform.position + new Vector3(x, y, 0f));
            floorComponent.transform.position = shipScript.basePoint.transform.position + offsetFromBase;

            for(int i = 0; i < points.Count; i++){
                if(pointsExist[i]){
                    (int tempX, int tempY) = points[i];
                    FixedJoint2D joint = floorComponent.AddComponent<FixedJoint2D>();
                    joint.connectedBody = shipScript.GetBodyPart(tempX, tempY).GetComponent<Rigidbody2D>();
                    
                    FixedJoint2D joint2 = shipScript.GetBodyPart(tempX, tempY).AddComponent<FixedJoint2D>();
                    joint2.connectedBody = floorComponent.GetComponent<Rigidbody2D>();
                }
            }
        }
    }

    private (List<(int, int)>, List<bool>) GetSurroundingPoints(int x, int y, Ship shipScript){
        List<(int, int)> points = new List<(int, int)>() {(x, y+1), (x, y-1), (x-1, y), (x+1, y)};
        List<bool> pointsExist = new List<bool>();
        foreach((int, int) point in points){
            (int tempX, int tempY) = point;
            pointsExist.Add(shipScript.BodyExistsAt(tempX, tempY));
        }
        return (points, pointsExist);
    }

    private void RemoveFloor(GameObject ship, int x, int y){
        Ship shipScript = ship.GetComponent<Ship>();
        if(x >= 0 && x < shipScript.width && y >= 0 && y < shipScript.height && shipScript.BodyExistsAt(x, y)){
            shipScript.RemoveBodyPart(x, y);
        }
    }

    public void AddComponent(GameObject ship, ShipPart component){
        Ship shipScript = ship.GetComponent<Ship>();
        int x = component.x;
        int y = component.y;
        
        Vector3 offsetFromBase;
        offsetFromBase = GridHelpers.DetermineRotatedCoordinatesFromBase(ship, shipScript.basePoint.transform.position + new Vector3(x, y, 0f));

        Quaternion rotation = Quaternion.identity;
        if(currentShip.transform.parent != null){
            rotation = currentShip.transform.parent.rotation;
        }

        RelativeJoint2D joint;
        switch(component.type){
            case ShipParts.Thruster:
                GameObject thruster = Instantiate(GetComponent<PartHolder>().shipThrusterComponentPrefab, new Vector3(x, y, 0f), rotation);
                thruster.GetComponent<Thruster>().SetOrientation(component.orientation);
                thruster.transform.parent = ship.transform;
                thruster.transform.position = shipScript.basePoint.transform.position + offsetFromBase;

                joint = thruster.AddComponent<RelativeJoint2D>();
                joint.connectedBody = shipScript.GetBodyPart(x, y).GetComponent<Rigidbody2D>();

                shipScript.AddComponent(thruster, x, y);
                break;
            case ShipParts.Heart:
                GameObject heart = Instantiate(GetComponent<PartHolder>().shipHeartComponentPrefab, new Vector3(x, y, 0f), rotation);

                ship.transform.parent = heart.transform; 

                joint = heart.AddComponent<RelativeJoint2D>();
                joint.connectedBody = shipScript.GetBodyPart(x, y).GetComponent<Rigidbody2D>();
                Camera.main.transform.position =  new Vector3(heart.transform.position.x, heart.transform.position.y, -10f);
                Camera.main.transform.parent = heart.transform;

                shipScript.AddComponent(heart, x, y);
                break;
            case ShipParts.BasicTurret:
                GameObject basicTurret = Instantiate(GetComponent<PartHolder>().shipBasicTurretPrefab, new Vector3(x, y, 0f), rotation);
                basicTurret.transform.parent = ship.transform;
                
                basicTurret.transform.position = shipScript.basePoint.transform.position + offsetFromBase;

                joint = basicTurret.AddComponent<RelativeJoint2D>();
                joint.connectedBody = ship.GetComponent<Ship>().body[component.y, component.x].GetComponent<Rigidbody2D>();

                shipScript.AddComponent(basicTurret, x, y);
                break;
            case ShipParts.BasicShield:
                GameObject basicShield = Instantiate(GetComponent<PartHolder>().shipBasicShieldPrefab, new Vector3(x, y, 0f), rotation);
                basicShield.GetComponent<BasicShield>().SetOrientation(component.orientation);
                basicShield.transform.parent = ship.transform;
                
                basicShield.transform.position = shipScript.basePoint.transform.position + offsetFromBase;

                joint = basicShield.AddComponent<RelativeJoint2D>();
                joint.connectedBody = ship.GetComponent<Ship>().body[component.y, component.x].GetComponent<Rigidbody2D>();

                shipScript.AddComponent(basicShield, x, y);
                break;
            default:
                break;
            }
    }

    private void RemoveComponent(GameObject ship, int x, int y){
        Ship shipScript = ship.GetComponent<Ship>();

        if(x >= 0 && x < shipScript.width && y >= 0 && y < shipScript.height){
            shipScript.RemoveComponent(x, y);
        }
    }

    public void ChangeOrientation(){
        switch(currentOrientation){
            case "forward":
                currentOrientation = "right";
                break;
            case "right":
                currentOrientation = "backward";
                break;
            case "backward":
                currentOrientation = "left";
                break;
            case "left":
                currentOrientation = "forward";
                break;
            default:
                break;
        }
    }

    public void SetPart(ShipParts type){
        buildPart = type;
    }
}
