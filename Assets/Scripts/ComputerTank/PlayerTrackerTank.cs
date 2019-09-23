using System;
using UnityEngine;

public class PlayerTrackerTank : TrackerTank
{

    protected override void FindPathToDestination()
    {
        int currentRow = ((int)transform.position.x - 1) / 2;
        int currentColumn = ((int)transform.position.z - 1) / 2;
        Vector2 playerField = GetPlayerField();
        path = shortestPathAlgorithm.FindShortestPath(mapManagerScript.obstacleMatrix, currentRow, currentColumn, (int)playerField.x, (int)playerField.y);   
    }

    protected override bool ArrivedToDestination()
    {
        FindPathToDestination();
        currentIndex = 1;

        return false;
    }

    Vector2 GetPlayerField()
    {
        Vector3 playerPosition = mapManagerScript.GetPlayerCoordinates();
        int playerX = (int)Math.Round(playerPosition.x);
        int playerZ = (int)Math.Round(playerPosition.z);

        if (playerX % 2 == 1)
            playerX++;
        if (playerZ % 2 == 1)
            playerZ++;

        int playerRow = (playerX - 1) / 2;
        int playerColumn = (playerZ - 1) / 2;

        return new Vector2(playerRow, playerColumn);
    }
}
