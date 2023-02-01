using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGrid : MonoBehaviour
{
    public GameObject currentGrid;
    public bool currentGridExists = false;

    public void ToggleGrid(){
        if(currentGridExists){
            Destroy(currentGrid);
            currentGridExists = false;
        }else{
            currentGrid = CreateBuildGrid();
            currentGridExists = true;
        }
    }

    public GameObject CreateBuildGrid(){
        GameObject currentShip = GameObject.FindWithTag("PlayerShip");

        GameObject grid = new GameObject();
        grid.transform.position = currentShip.transform.parent.position;
        Ship ship = currentShip.GetComponent<Ship>();
        for(int y = 0; y < ship.height; y++){
            for(int x = 0; x < ship.width; x++){
                GameObject gridUnit = Instantiate(GetComponent<PartHolder>().gridUnitPrefab, new Vector3(x + currentShip.transform.parent.position.x - (ship.width-1)/2, y + currentShip.transform.parent.position.y - (ship.height-1)/2, 0f), Quaternion.identity);
                gridUnit.transform.parent = grid.transform;
            }
        }
    
        
        grid.transform.parent = currentShip.transform.parent;
        grid.transform.localRotation = Quaternion.identity;
        grid.name = "BuildGrid";
        return grid;
    }
}
