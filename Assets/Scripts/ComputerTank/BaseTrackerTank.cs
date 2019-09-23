using Assets.Scripts;
using UnityEngine;

public class BaseTrackerTank : TrackerTank
{
    protected override void FindPathToDestination()
    {
        int currentRow = ((int)transform.position.x - 1) / 2;
        int currentColumn = ((int)transform.position.z - 1) / 2;
        int randomIndex = UnityEngine.Random.Range(0, Constants.CastleRows.Length);
        path = shortestPathAlgorithm.FindShortestPath(mapManagerScript.obstacleMatrix, currentRow, currentColumn, Constants.CastleRows[randomIndex], Constants.CastleColumns[randomIndex]);
    }

    protected override bool ArrivedToDestination()
    {
        RotateToCastle();
        InvokeRepeating("CheckForCastleOnTheWay", 0, spawnDelay);
        return true;
    }

    void RotateToCastle()
    {
        Vector2 castleField;
        int currentRow = ((int) transform.position.x - 1) / 2;
        int currentColumn = ((int)transform.position.z - 1) / 2;
        if (currentRow == Constants.CastleMinRow)
            castleField = new Vector2(currentRow + 1, currentColumn);
        else if (currentColumn == Constants.CastleMinColumn)
            castleField = new Vector2(currentRow, currentColumn + 1);
        else
            castleField = new Vector2(currentRow, currentColumn - 1);
        Vector3 nextRotation = TankUtils.FindNextRotation(gameObject, castleField);
        nextRotation.y -= transform.rotation.eulerAngles.y;
        transform.Rotate(nextRotation);
    }
}
