using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BasicScoutTank : MonoBehaviour
{
    private float speed;
    private float moveDelay;
    private int stepCounter;
    private float timeForSpawn;
    private float spawnDelay;
    private MapManager mapManagerScript;
    private CollisionManager collisionManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        speed = 1.0f;
        timeForSpawn = 0.1f;
        spawnDelay = 1f;
        moveDelay = 0.1f;
        stepCounter = 5;
        mapManagerScript = GameObject.Find("Manager").GetComponent<MapManager>();
        collisionManagerScript = GameObject.Find("Manager").GetComponent<CollisionManager>();
        InvokeRepeating("UpdatePosition", Constants.UpdateMethodDelay, Constants.UpdateMethodDelay);
    }

    void UpdatePosition()
    {
        CheckEdgePositions();
        CheckForPlayerOnTheWay();
        CheckForCastleOnTheWay();
        int xCoordinate = (int)Math.Round(transform.position.x);
        int zCoordinate = (int)Math.Round(transform.position.z);
        if (TankUtils.IsCenterOfField(gameObject, xCoordinate, zCoordinate))
        {
            stepCounter--;
            Vector2 nextField = FindNextField();
            if (nextField.Equals(Vector2.negativeInfinity) || mapManagerScript.obstacleMatrix[(int)nextField.x, (int)nextField.y])
            {
                RotateToFreeNeighbour(xCoordinate, zCoordinate);
                if (stepCounter == 0)
                {
                    stepCounter = 5;
                }
                    
            }
            else if (stepCounter == 0)
            {
                stepCounter = 5;
                RotateToFreeNeighbour(xCoordinate, zCoordinate);
            }
        }
        TankUtils.GoToNewPosition(gameObject);
    }

    List<Vector2> FindFreeNeighbourFields(int row, int column)
    {
        List<Vector2> fields = new List<Vector2>();
        if (column - 1 >= 0 && !mapManagerScript.obstacleMatrix[row, column - 1] && !TankUtils.FloatsAreEqual(transform.rotation.eulerAngles.y, 360, Constants.MaxDifferenceForAngles) && !TankUtils.FloatsAreEqual(transform.rotation.eulerAngles.y, 0, Constants.MaxDifferenceForAngles))
        {
            fields.Add(new Vector2(row, column - 1));
        }
        if (column + 1 < Constants.NumOfColums && !mapManagerScript.obstacleMatrix[row, column + 1] && !TankUtils.FloatsAreEqual(transform.rotation.eulerAngles.y, 180, Constants.MaxDifferenceForAngles))
        {
            fields.Add(new Vector2(row, column + 1));
        }
        if (row - 1 >= 0 && !mapManagerScript.obstacleMatrix[row - 1, column] && !TankUtils.FloatsAreEqual(transform.rotation.eulerAngles.y, 90, Constants.MaxDifferenceForAngles))
        {
            fields.Add(new Vector2(row - 1, column));
        }
        if (row + 1 < Constants.NumOfRows && !mapManagerScript.obstacleMatrix[row + 1, column] && !TankUtils.FloatsAreEqual(transform.rotation.eulerAngles.y, 270, Constants.MaxDifferenceForAngles))
        {
            fields.Add(new Vector2(row + 1, column - 1));
        }

        return fields;
    }


    void CheckEdgePositions()
    {
        if (transform.position.x > Constants.MaxCoordinate)
        {
            transform.position = new Vector3(Constants.MaxCoordinate, transform.position.y, transform.position.z);
            transform.Rotate(new Vector3(0, Constants.TurnAroundAngle, 0));
        }
        if (transform.position.x < Constants.MinCoordinate)
        {
            transform.position = new Vector3(Constants.MinCoordinate, transform.position.y, transform.position.z);
            transform.Rotate(new Vector3(0, -Constants.TurnAroundAngle, 0));
        }
        if (transform.position.z > Constants.MaxCoordinate)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, Constants.MaxCoordinate);
            transform.Rotate(new Vector3(0, -Constants.TurnAroundAngle, 0));
        }
        if (transform.position.z < Constants.MinCoordinate)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, Constants.MinCoordinate);
            transform.Rotate(new Vector3(0, Constants.TurnAroundAngle, 0));
        }
    }   

    private void OnCollisionEnter(Collision collision)
    {
        collisionManagerScript.ResolveCollision(gameObject, collision.gameObject);
    }

    void CheckForPlayerOnTheWay()
    {   
        if (mapManagerScript.CanSeePlayer(gameObject))
        {
            SpawnRocket();
        }
    }

    void CheckForCastleOnTheWay()
    {
        if (mapManagerScript.CanSeeCastle(gameObject))
        {
            SpawnRocket();
        }
    }

    void SpawnRocket()
    {
        if (Time.time >= timeForSpawn)
        {
            timeForSpawn = Time.time + spawnDelay;
            TankUtils.SpawnRocket(gameObject);
        }
    }

    Vector2 FindNextField()
    {
        Vector2 nextField = Vector2.negativeInfinity;
        float yAngle = transform.rotation.eulerAngles.y;
        int xCoordinate = (int)Math.Round(transform.position.x);
        int zCoordinate = (int)Math.Round(transform.position.z);
        int row = (xCoordinate - 1) / 2;
        int column = (zCoordinate - 1) / 2;
        if (row > 0 && TankUtils.FloatsAreEqual(yAngle, Constants.AngleUp, Constants.MaxDifferenceForAngles))
        {
            nextField = new Vector2(row - 1, column);
        }
        else if (row < 19 && TankUtils.FloatsAreEqual(yAngle, Constants.AngleDown, Constants.MaxDifferenceForAngles))
        {
            nextField = new Vector2(row + 1, column);
        }
        else if (column > 0 && TankUtils.FloatsAreEqual(yAngle, Constants.AngleLeft, Constants.MaxDifferenceForAngles))
        {
            nextField = new Vector2(row, column - 1);
        }
        else if (column < 19 && TankUtils.FloatsAreEqual(yAngle, Constants.AngleRight, Constants.MaxDifferenceForAngles))
        {
            nextField = new Vector2(row, column + 1);
        }
        return nextField;
    }

    int GetRandom(int max)
    {
        int randomNumber = UnityEngine.Random.Range(0, 1000);
        return randomNumber % (max + 1);
    }

    void RotateToFreeNeighbour(int xCoordinate, int zCoordinate)
    {
        stepCounter = 5;
        List<Vector2> freeFields = FindFreeNeighbourFields((xCoordinate - 1) / 2, (zCoordinate - 1) / 2);
        if (freeFields.Count > 0)
        {
            int randomIndex = GetRandom(freeFields.Count - 1);
            Vector3 nextRotation = TankUtils.FindNextRotation(gameObject, freeFields[randomIndex]);
            nextRotation.y -= transform.rotation.eulerAngles.y;
            transform.Rotate(nextRotation);
        }
        else
        {
            Debug.Log("Nema slobodnog komsije!");
            transform.Rotate(new Vector3(0, Constants.AngleLeft, 0));
            return;
        }
    }
}
