using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankUtils
{
    private static GameObject rocketPrefab = Resources.Load(Constants.RocketCompPrefabDest) as GameObject;

    public static bool IsCenterOfField(GameObject obj, int roundedCoordinate1, int roundedCoordinate2)
    {
        return (roundedCoordinate1 - 1) % 2 == 0 && (roundedCoordinate2 - 1) % 2 == 0 &&
               FloatsAreEqual(obj.transform.position.x, roundedCoordinate1, Constants.MaxDifferenceForCoordinates) &&
               FloatsAreEqual(obj.transform.position.z, roundedCoordinate2, Constants.MaxDifferenceForCoordinates);
    }

    public static bool FloatsAreEqual(float float1, float float2, float maxDifference)
    {
        return Math.Abs(float1 - float2) <= maxDifference;
    }

    public  static Vector3 FindNextRotation(GameObject obj, Vector2 fieldToMove)
    {
        float xCoordinate = fieldToMove.x * 2 + 1;
        float zCoordinate = fieldToMove.y * 2 + 1;
        Vector3 currentPosition = obj.transform.position;
        if (xCoordinate > currentPosition.x)
        {
            return new Vector3(0, Constants.AngleDown, 0);
        }
        else if (xCoordinate < currentPosition.x)
        {
            return new Vector3(0, Constants.AngleUp, 0);
        }
        else if (zCoordinate > currentPosition.z)
        {
            return new Vector3(0, Constants.AngleRight, 0);
        }
        else //if (fieldToMove.y < transform.position.z)
        {
            return new Vector3(0, Constants.AngleLeft, 0);
        }
    }

    public static void GoToNewPosition(GameObject obj)
    {
        float yAngle = obj.transform.rotation.eulerAngles.y;
        Vector3 newPosition;
        Vector3 currentPosition = obj.transform.position;
        if (TankUtils.FloatsAreEqual(yAngle, Constants.AngleUp, Constants.MaxDifferenceForAngles))
        {
            newPosition = new Vector3(currentPosition.x - Constants.MoveLength, currentPosition.y, currentPosition.z);
        }
        else if (TankUtils.FloatsAreEqual(yAngle, Constants.AngleDown, Constants.MaxDifferenceForAngles))
        {
            newPosition = new Vector3(currentPosition.x + Constants.MoveLength, currentPosition.y, currentPosition.z);

        }
        else if (TankUtils.FloatsAreEqual(yAngle, Constants.AngleLeft, Constants.MaxDifferenceForAngles))
        {
            newPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - Constants.MoveLength);

        }
        else
        {
            newPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + Constants.MoveLength);
        }

        obj.transform.position = newPosition;
    }

    public static void SpawnRocket(GameObject obj)
    {
        Vector3 rotationVector = obj.transform.rotation.eulerAngles;
        rotationVector.x += 90;
        Transform rocketPositionObject = obj.transform.GetChild(1);
        GameObject.Instantiate(rocketPrefab, rocketPositionObject.position, Quaternion.Euler(rotationVector));
    }
}
