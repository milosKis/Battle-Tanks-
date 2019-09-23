using System;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<Vector2> boxPositions;
    public List<Vector2> grassBlockPositions;
    public List<Vector2> waterBlockPositions;
    public List<Vector2> brickBlockPositions;
    public List<Vector2> basicScoutPositions;
    public List<Vector2> baseTrackerPositions;
    public List<Vector2> playerTrackerPositions;
    public bool[,] obstacleMatrix;

    void Start()
    {
        obstacleMatrix = new bool[Constants.NumOfRows, Constants.NumOfColums];
        for (int i = 0; i < Constants.NumOfRows; i++)
        for (int j = 0; j < Constants.NumOfColums; j++)
        {
            obstacleMatrix[i, j] = false;
        }
        SetCastleFieldsAsObstacles();
    }

    void SetCastleFieldsAsObstacles()
    {
        obstacleMatrix[0, 9] = obstacleMatrix[0, 10] = obstacleMatrix[1, 9] = obstacleMatrix[1, 10] = true;
        obstacleMatrix[18, 9] = obstacleMatrix[18, 10] = obstacleMatrix[19, 9] = obstacleMatrix[19, 10] = true;
    }

    public void FindObjectsPositionsForMap(int mapNumber)
    {
        string[] lines = File.ReadAllLines(Constants.SourcePrefix + mapNumber + Constants.FileExtension);
        InitializeLists();

        foreach (var line in lines)
        {
            ResolveObjectTypeAndPosition(line);
        }
    }

    void InitializeLists()
    {
        boxPositions = new List<Vector2>();
        grassBlockPositions = new List<Vector2>();
        brickBlockPositions = new List<Vector2>();
        basicScoutPositions = new List<Vector2>();
        baseTrackerPositions = new List<Vector2>();
        playerTrackerPositions = new List<Vector2>();
    }

    void ResolveObjectTypeAndPosition(string objectInfoLine)
    {
        string[] objectInfo = objectInfoLine.Split(Constants.SplitSign);
        string objectType = objectInfo[Constants.ObjectTypeIndex];
        int mapRow = int.Parse(objectInfo[Constants.MapRowIndex]);
        int mapColumn = int.Parse(objectInfo[Constants.MapColumnIndex]);
        Vector2 position = new Vector2(1 + mapRow * 2, 1 + mapColumn * 2);

        switch (objectType)
        {
            case Constants.BoxTag:
                boxPositions.Add(position);
                obstacleMatrix[mapRow, mapColumn] = true;
                break;

            case Constants.GrassBlockTag:
                grassBlockPositions.Add(position);
                //obstacleMatrix[mapRow, mapColumn] = false;
                break;

            case Constants.WaterBlockTag:
                waterBlockPositions.Add(position);
                obstacleMatrix[mapRow, mapColumn] = true;
                break;

            case Constants.BrickBlockTag:
                obstacleMatrix[mapRow, mapColumn] = true;
                brickBlockPositions.Add(position);
                break;

            case Constants.BasicScoutTag:
                basicScoutPositions.Add(position);
                // da li dodati da je polje zauzeto?
                break;

            case Constants.BaseTrackerTag:
                baseTrackerPositions.Add(position);
                break;

            case Constants.PlayerTrackerTag:
                playerTrackerPositions.Add(position);
                break;
        }
    }

    public void SetFieldAsFree(int row, int column)
    {
        obstacleMatrix[row, column] = false;
    }

    public void SetFieldAsTaken(int row, int column)
    {
        obstacleMatrix[row, column] = true;
    }

    public bool CanSeeCastle(GameObject attacker)
    {
        GameObject castle = GameObject.FindGameObjectWithTag(Constants.BlueCastleTag);
        if (castle != null)
        {
            return CanSeeTarget(attacker, castle);
        }

        return false;
    }

    public bool CanSeePlayer(GameObject attacker)
    {
        GameObject playerTank = GameObject.FindGameObjectWithTag(Constants.PlayerTag);
        if (playerTank != null)
        {
            return CanSeeTarget(attacker, playerTank);
        }

        return false;
    }

    bool CanSeeTarget(GameObject attacker, GameObject target)
    {
        float attackerX = attacker.transform.position.x;
        float attackerZ = attacker.transform.position.z;
        float targetX = target.transform.position.x;
        float targetZ = target.transform.position.z;
        float angleY = attacker.transform.rotation.eulerAngles.y;
        float width = target.GetComponent<BoxCollider>().bounds.size.x / 2;
        return (FloatsAreEqual(attackerZ, targetZ, width) &&
                ((attackerX < targetX && FloatsAreEqual(angleY, Constants.AngleDown, Constants.MaxDifferenceForAngles)) ||
                 (attackerX > targetX && FloatsAreEqual(angleY, Constants.AngleUp, Constants.MaxDifferenceForAngles)))) ||
               (FloatsAreEqual(attackerX, targetX, width) &&
                ((attackerZ < targetZ && (FloatsAreEqual(angleY, Constants.AngleRight, Constants.MaxDifferenceForAngles) || FloatsAreEqual(angleY, 0, Constants.MaxDifferenceForAngles))) ||
                 (attackerZ > targetZ && FloatsAreEqual(angleY, Constants.AngleLeft, Constants.MaxDifferenceForAngles))));
    }

    bool FloatsAreEqual(float float1, float float2, float maxDifference)
    {
        return Math.Abs(float1 - float2) <= maxDifference;
    }

    public Vector3 GetPlayerCoordinates()
    {
        GameObject player = GameObject.FindGameObjectWithTag(Constants.PlayerTag);
        return player.transform.position;
    }
}
