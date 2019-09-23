using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrackerTank : MonoBehaviour
{
    protected float timeForSpawn;
    protected float spawnDelay;
    protected int currentIndex;
    protected ShortestPath shortestPathAlgorithm;
    protected MapManager mapManagerScript;
    protected CollisionManager collisionManagerScript;
    protected List<Vector2> path;

    // Start is called before the first frame update
    void Start()
    {
        timeForSpawn = 0.1f;
        spawnDelay = 1f;
        shortestPathAlgorithm = GameObject.Find("Manager").GetComponent<ShortestPath>();
        mapManagerScript = GameObject.Find("Manager").GetComponent<MapManager>();
        collisionManagerScript = GameObject.Find("Manager").GetComponent<CollisionManager>();
        FindPathToDestination();
        currentIndex = 0;
        Invoke("UpdatePosition", Constants.UpdateMethodDelay);
    }

    void UpdatePosition()
    {
        CheckForPlayerOnTheWay();
        CheckForCastleOnTheWay();
        int xCoordinate = (int)Math.Round(transform.position.x);
        int zCoordinate = (int)Math.Round(transform.position.z);
        if (TankUtils.IsCenterOfField(gameObject, xCoordinate, zCoordinate))
        {
            currentIndex++;
            if (currentIndex == path.Count)
            {
                bool stayInPlace = ArrivedToDestination();
                if (stayInPlace)
                    return;
            }

            Vector3 nextRotation = TankUtils.FindNextRotation(gameObject, path[currentIndex]);
            nextRotation.y -= transform.rotation.eulerAngles.y;
            transform.Rotate(nextRotation);
        }
        TankUtils.GoToNewPosition(gameObject);
        Invoke("UpdatePosition", Constants.UpdateMethodDelay);
    }

    protected abstract void FindPathToDestination();

    protected abstract bool ArrivedToDestination();

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
}
