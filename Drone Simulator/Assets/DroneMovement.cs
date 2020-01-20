using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    Rigidbody rb;
    float movex, movey, moveForward;
    float speedUp = 12f;
    float speedX = 10f;
    float speedForward = 10f;
    float tiltAngle = 25f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        movex = Input.GetAxis("HorizontalTurn");
        moveForward = Input.GetAxis("VerticalTurn");
        movey = Input.GetAxis("Vertical");
    }

    private void FixedUpdate() {
        rb.velocity = new Vector3(movex * speedX, movey * speedUp, moveForward * speedForward);
        rb.rotation = Quaternion.Euler(tiltAngle * moveForward, 0f, -tiltAngle * movex);
    }
}
