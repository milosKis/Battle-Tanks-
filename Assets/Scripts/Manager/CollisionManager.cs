using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private Vector3 positionOffset;
    private GameObject explosionParticle;
    private GameObject failedExplosionParticle;
    private int collisionCounter;
    private MapManager mapManagerScript;
    private GameManager gameManagerScript;

    void Start()
    {
        positionOffset = new Vector3(0, 2, 0);
        collisionCounter = 0;
        mapManagerScript = GameObject.Find("Manager").GetComponent<MapManager>();
        gameManagerScript = GameObject.Find("Manager").GetComponent<GameManager>();
        LoadFromResources();
    }

    void LoadFromResources()
    {
        explosionParticle = Resources.Load(Constants.ExplosionParticleDest) as GameObject;
        failedExplosionParticle = Resources.Load(Constants.FailedExplosionParticleDest) as GameObject;
    }

    public void ResolveCollision(GameObject attacker, GameObject target)
    {
        if (attacker.CompareTag(Constants.RocketPlayerTag) || attacker.CompareTag(Constants.RocketCompTag))
        {
            RocketCollision(attacker, target);
        }

        if (attacker.CompareTag(Constants.PlayerTag) && target.CompareTag(Constants.ComputerTag))
        {
            PlayerComputerTankCollision(attacker, target);
        }

        if (attacker.CompareTag(Constants.ComputerTag) && target.CompareTag(Constants.PlayerTag))
        {
            PlayerComputerTankCollision(target, attacker);
        }

        if (attacker.CompareTag(Constants.ComputerTag) && target.CompareTag(Constants.ComputerTag))
        {
            ComputerComputerTankCollision(attacker, target);
        }

        if (attacker.CompareTag(Constants.ComputerTag) && (target.CompareTag(Constants.BoxTag) || target.CompareTag(Constants.BrickBlockTag) ||
                                                           target.CompareTag(Constants.WaterBlockTag)))
        {
            Debug.Log("Collision comp box water rock");
            ComputerTankBoxCollision(attacker);
        }
    }

    void RocketCollision(GameObject rocket, GameObject obj)
    {
        Vector3 position = obj.transform.position;

        if (obj.CompareTag(Constants.BoxTag) || (obj.CompareTag(Constants.ComputerTag) && rocket.CompareTag(Constants.RocketPlayerTag))
                                             || obj.CompareTag(Constants.PlayerTag))
        {
            mapManagerScript.SetFieldAsFree(((int)obj.transform.position.x - 1) / 2, ((int)obj.transform.position.z - 1) / 2);
            if (obj.CompareTag(Constants.PlayerTag))
            {
                //gameManagerScript.GameOver();
                int childCount = obj.transform.GetChild(2).childCount;
                if (childCount > 1)
                {
                    Destroy(obj.transform.GetChild(2).GetChild(0).gameObject);
                    DestroyWithExplosion(rocket, obj, position + positionOffset, false);
                    return;
                }
                gameManagerScript.GameOver();
            }
            DestroyWithExplosion(rocket, obj, position + positionOffset, true);
        }
        else
        {
            DestroyWithExplosion(rocket, obj, position + positionOffset, false);
        }
    }

    void PlayerComputerTankCollision(GameObject player, GameObject computer)
    {
        Vector3 explosionPosition = player.transform.position + new Vector3(0, 2, 0);
        Instantiate(explosionParticle, explosionPosition, Quaternion.identity);
        Destroy(player);
        gameManagerScript.GameOver();
    }

    void ComputerComputerTankCollision(GameObject computer1, GameObject computer2)
    {
        FindNewPositionAfterCollision(computer1, 1);
        FindNewPositionAfterCollision(computer2, 1);
        Vector3 rotationVector = new Vector3(0, Constants.AngleDown, 0);
        computer1.transform.Rotate(rotationVector);
        computer2.transform.Rotate(rotationVector);
    }

    void ComputerTankBoxCollision(GameObject tank)
    {
        FindNewPositionAfterCollision(tank, Constants.CollisionOffset);
        collisionCounter++;
        tank.transform.Rotate(new Vector3(0, 90, 0));
    }

    void FindNewPositionAfterCollision(GameObject obj, float collisionOffset)
    {
        Vector3 newPosition;
        float yAngle = obj.transform.rotation.eulerAngles.y;
        if (FloatsAreEqual(yAngle, Constants.AngleUp, Constants.MaxDifferenceForAngles))
        {
            newPosition = new Vector3((float)System.Math.Round(obj.transform.position.x + collisionOffset), obj.transform.position.y, obj.transform.position.z);
        }
        else if (FloatsAreEqual(yAngle, Constants.AngleDown, Constants.MaxDifferenceForAngles))
        {
            newPosition = new Vector3((float)System.Math.Round(obj.transform.position.x - collisionOffset), obj.transform.position.y, obj.transform.position.z);

        }
        else if (FloatsAreEqual(yAngle, Constants.AngleLeft, Constants.MaxDifferenceForAngles))
        {
            newPosition = new Vector3(obj.transform.position.x, obj.transform.position.y, (float)System.Math.Round(obj.transform.position.z + collisionOffset));

        }
        else
        {
            newPosition = new Vector3(obj.transform.position.x, obj.transform.position.y, (float)System.Math.Round(obj.transform.position.z - collisionOffset));
        }

        obj.transform.position = newPosition;
    }

    void DestroyWithExplosion(GameObject attacker, GameObject targetToDestroy, Vector3 explosionPosition, bool successfulExplosion)
    {
        Destroy(attacker);
        if (successfulExplosion)
        {
            Destroy(targetToDestroy);
            Instantiate(explosionParticle, explosionPosition, Quaternion.identity);
        }
        else
        {
            Instantiate(failedExplosionParticle, explosionPosition, Quaternion.identity);
        }
    }

    bool FloatsAreEqual(float float1, float float2, float maxDifference)
    {
        return System.Math.Abs(float1 - float2) <= maxDifference;
    }
}
