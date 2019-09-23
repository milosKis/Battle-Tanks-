using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CreationManager : MonoBehaviour
{
    private MapManager mapManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        mapManagerScript = GameObject.Find("Manager").GetComponent<MapManager>();
    }

    public void CreateWorldForMap(int mapNumber)
    {
        mapManagerScript.FindObjectsPositionsForMap(mapNumber);
        InstantiateObjects();
    }

    void InstantiateObjects()
    {
        InstantiateGrassBlocks();
        InstantiateWaterBlocks();
        InstantiateBrickBlocks();
        InstantiateBoxes();
        InstantiateCastles();
        InstantiateTank();
    }

    void InstantiateTank()
    {
        GameObject tank = Resources.Load(Constants.PlayerTankDest) as GameObject;
        Vector3 position = new Vector3(1 + 18 * 2, 7, 1 + 8 * 2);
        GameObject tankClone = Instantiate(tank, position, tank.transform.rotation);

        InstantiateObjects(Constants.BasicScoutTankDest, mapManagerScript.basicScoutPositions, 7.1218f);
        InstantiateObjects(Constants.BaseTrackerTankDest, mapManagerScript.baseTrackerPositions, 7.1218f);
        InstantiateObjects(Constants.PlayerTrackerTankDest, mapManagerScript.playerTrackerPositions, 7.1218f);
    }

    void InstantiateCastles()
    {
        GameObject castle = Resources.Load(Constants.BlueCastleDest) as GameObject;
        Instantiate(castle, castle.transform.position, castle.transform.rotation);
        castle = Resources.Load(Constants.RedCastleDest) as GameObject;
        Instantiate(castle, castle.transform.position, castle.transform.rotation);
    }


    void InstantiateGrassBlocks()
    {
        InstantiateObjects(Constants.GrassBlockDest, mapManagerScript.grassBlockPositions, 5);
    }

    void InstantiateWaterBlocks()
    {
        InstantiateObjects(Constants.WaterBlockDest, mapManagerScript.waterBlockPositions, 4);
    }

    void InstantiateBoxes()
    {
        InstantiateObjects(Constants.BoxDest, mapManagerScript.boxPositions, 8);
    }

    void InstantiateBrickBlocks()
    {
        InstantiateObjects(Constants.BrickBlockDest, mapManagerScript.brickBlockPositions, 8);
    }

    void InstantiateObjects(string objectDest, List<Vector2> coordinates, float yCoordinate)
    {
        GameObject obj = Resources.Load(objectDest) as GameObject;
        for (int i = 0; i < coordinates.Count; i++)
        {
            Vector3 position = new Vector3(coordinates[i].x, yCoordinate, coordinates[i].y);
            GameObject objClone = Instantiate(obj, position, obj.transform.rotation);
            switch (objectDest)
            {
                case Constants.BoxDest:
                    Renderer renderer = objClone.GetComponent<Renderer>();
                    objClone.transform.localScale = new Vector3(2 / renderer.bounds.size.y, 2 / renderer.bounds.size.y, 2 / renderer.bounds.size.z);
                    break;

                case Constants.GrassBlockDest:
                    objClone.AddComponent<BoxCollider>();
                    break;

                case Constants.WaterBlockDest:
                    objClone.transform.localScale = new Vector3(1, 2, 1);
                    break;
            }
        }
    }
}
