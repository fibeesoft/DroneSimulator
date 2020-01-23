using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    float speed = 25f;

    void Update()
    {
        if(transform.rotation.y != 0){
            transform.position -= Vector3.forward * Time.deltaTime * speed;
        }
        else{
            transform.position += Vector3.forward * Time.deltaTime * speed;
        }
    }
}
