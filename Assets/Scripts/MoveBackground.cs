using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    public float speed;

    private Vector3 startingPosition;
    private float repeatWidth;

    // Start is called before the first frame update
    void Start()
    {
        speed = 5.0f;
        startingPosition = transform.position;
        repeatWidth = transform.lossyScale.z * gameObject.GetComponent<BoxCollider>().size.z / 2;
    }

    // Update is called once per frame
    void Update()
    {
        CheckRepeatingPosition();
        UpdatePosition();
    }

    void CheckRepeatingPosition()
    {
        if (transform.position.z < startingPosition.z - repeatWidth)
        {
            transform.position = startingPosition;
        }
    }

    void UpdatePosition()
    {
        transform.Translate(Vector3.back * Time.deltaTime * speed);
    }
}
