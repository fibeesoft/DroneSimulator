using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    float bulletSpeed = 80f;
    Vector3 bulletDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.velocity = bulletDirection * bulletSpeed;
        rb.velocity = Vector3.forward * bulletSpeed;
    }

    public void Initialize(Vector3 bulletDir){
        bulletDirection = bulletDir;
    }
}
