using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float turnSpeed;

    private GameObject rocketPrefab;

    // Start is called before the first frame update
    void Start()
    {
        speed = 10.0f;
        turnSpeed = 5.0f;
        LoadFromResources();
    }

    void LoadFromResources()
    {
        rocketPrefab = Resources.Load(Constants.RocketPlayerPrefabDest) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        CheckEdgePositions();
        ProcessUserInput();
    }

    void CheckEdgePositions()
    {
        if (transform.position.x > Constants.MaxCoordinate)
        {
            transform.position = new Vector3(Constants.MaxCoordinate, transform.position.y, transform.position.z);
        }
        if (transform.position.x < Constants.MinCoordinate)
        {
            transform.position = new Vector3(Constants.MinCoordinate, transform.position.y, transform.position.z);
        }
        if (transform.position.z > Constants.MaxCoordinate)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, Constants.MaxCoordinate);
        }
        if (transform.position.z < Constants.MinCoordinate)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, Constants.MinCoordinate);
        }
    }

    void ProcessUserInput()
    {
        ProcessHorizontalInput();
        ProcessVerticalInput();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 rotationVector = transform.rotation.eulerAngles;
            rotationVector.x += 90;
            Transform rocketPositionObject = transform.GetChild(1);
            Instantiate(rocketPrefab, rocketPositionObject.position, Quaternion.Euler(rotationVector));
        }
    }

    void ProcessHorizontalInput()
    {
        float horizontalInput = Input.GetAxis(Constants.HorizontalAxis);
        transform.Rotate(Vector3.up * horizontalInput * turnSpeed);
    }

    void ProcessVerticalInput()
    {
        float verticalInput = Input.GetAxis(Constants.VerticalAxis);
        transform.Translate(Vector3.forward * Time.deltaTime * verticalInput * speed);
    }
}
