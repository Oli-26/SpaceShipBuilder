using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingControls : MonoBehaviour
{
    Camera mainCamera;
    ShipBuilding shipBuilding;
    BuildGrid buildGrid;

    void Start(){
        mainCamera = Camera.main;
        shipBuilding = GetComponent<ShipBuilding>();
        buildGrid = GetComponent<BuildGrid>();
    }

    void Update()
    {
        if(Input.GetKeyDown("b")){
            buildGrid.ToggleGrid();
        }

        if(Input.GetMouseButtonDown(0)){
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0f;
            shipBuilding.Add(worldPosition);
        }

        if(Input.GetMouseButtonDown(1)){
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0f;
            shipBuilding.Remove(worldPosition);
        }

        if(Input.GetKeyDown("r")){
            shipBuilding.ChangeOrientation();
        }

        SetBuildPart();
    }

    private void SetBuildPart(){
       if(Input.GetKeyDown("1")){
            shipBuilding.SetPart(ShipParts.Floor);
        } 

        if(Input.GetKeyDown("2")){
            shipBuilding.SetPart(ShipParts.Thruster);
        } 

        if(Input.GetKeyDown("3")){
            shipBuilding.SetPart(ShipParts.BasicTurret);
        } 

        if(Input.GetKeyDown("4")){
            shipBuilding.SetPart(ShipParts.BasicShield);
        } 
    }
}
