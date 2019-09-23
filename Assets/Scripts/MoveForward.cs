using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed;

    private CollisionManager collisionManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        speed = 12.0f;
        collisionManagerScript = GameObject.Find("Manager").GetComponent<CollisionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        collisionManagerScript.ResolveCollision(gameObject, other.gameObject);
    }
}
