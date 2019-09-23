using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    public float minCoordinate;
    public float maxCoordinate;

    private GameObject explosionParticle;

    // Start is called before the first frame update
    void Start()
    {
        minCoordinate = -15;
        maxCoordinate = 55;
        LoadFromResources();
    }

    void LoadFromResources()
    {
        explosionParticle = Resources.Load(Constants.ExplosionParticleSmokeDest) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOutOfBound())
        {
            DestroyWithExplosion(transform.position);
        }
    }

    bool IsOutOfBound()
    {
        float xPosition = transform.position.x;
        float zPosition = transform.position.z;

        return xPosition < minCoordinate || xPosition > maxCoordinate || zPosition < minCoordinate ||
               zPosition > maxCoordinate;
    }

    void DestroyWithExplosion(Vector3 explosionPosition)
    {
        Destroy(gameObject);
        Instantiate(explosionParticle, explosionPosition, Quaternion.identity);
    }
}
