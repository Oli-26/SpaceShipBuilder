using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridHelpers
{
    public static Vector3 DetermineUnRotatedCoordinatesFromBase(GameObject ship, Vector3 targetPoint){
        GameObject basePoint = ship.GetComponent<Ship>().basePoint;
        
        Vector3 difference = targetPoint - basePoint.transform.position;
        float angle = Mathf.Atan(difference.y/difference.x) - ship.transform.parent.rotation.eulerAngles.z * Mathf.PI/180;

        float hypo = Vector3.Magnitude(difference);
        float unRoundedX = Mathf.Abs(hypo*Mathf.Cos(angle));
        float unRoundedY = Mathf.Abs(hypo*Mathf.Sin(angle));
        return new Vector3(unRoundedX, unRoundedY, 0);
    }

    public static Vector3 DetermineRotatedCoordinatesFromBase(GameObject ship, Vector3 targetPoint){
        GameObject basePoint = ship.GetComponent<Ship>().basePoint;
        
        Vector3 difference = targetPoint - basePoint.transform.position;
        float angle = Mathf.Atan(difference.y/difference.x);

        if(ship.transform.parent != null){
            angle += ship.transform.parent.rotation.eulerAngles.z * Mathf.PI/180;
        }

        float hypo = Vector3.Magnitude(difference);
        float unRoundedX = hypo*Mathf.Cos(angle);
        float unRoundedY = hypo*Mathf.Sin(angle);

        return new Vector3(unRoundedX, unRoundedY, 0);
    }

    public static bool WithinBuildBounds(GameObject ship, Vector3 targetPoint){
        Vector3 distanceFromHeart = targetPoint - ship.transform.parent.position;
        Ship shipScript = ship.GetComponent<Ship>();
        if(Mathf.Abs(distanceFromHeart.x) > 0.49 + (shipScript.width-1)/2 || Mathf.Abs(distanceFromHeart.y) > 0.49 + (shipScript.height-1)/2){
            return false;
        }

        return true;
    }

    public static (int, int) DetermineGridCoordinate(GameObject ship, Vector3 targetPoint){  
        if(ship.transform.parent == null){
            int initialX = Mathf.RoundToInt(targetPoint.x);
            int initialY = Mathf.RoundToInt(targetPoint.y);
            return (initialX, initialY);
        }

        Vector3 unRotatedDifference = GridHelpers.DetermineUnRotatedCoordinatesFromBase(ship, targetPoint);
        int x = Mathf.RoundToInt(unRotatedDifference.x);
        int y = Mathf.RoundToInt(unRotatedDifference.y);
        return (x, y);

    }
}
