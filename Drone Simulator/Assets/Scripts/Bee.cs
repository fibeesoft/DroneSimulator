using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    [SerializeField] GameObject[] beeWings;
    Vector3 direction;
    void Start()
    {
        ChangeDirection(); 
    }

    float wingSpeed = 16f;
    float maxRotation = 22f;
    float speed = 12f;
    float timer = 3f;
    void Update()
    {
        beeWings[0].transform.localRotation = Quaternion.Euler(0f,maxRotation * Mathf.Sin(Time.time * wingSpeed),0f);
        beeWings[1].transform.localRotation = Quaternion.Euler(0f,-maxRotation * Mathf.Sin(Time.time * wingSpeed),0f);
        transform.position += direction * speed * Time.deltaTime;

        timer -= Time.deltaTime;
        if(timer < 0){
            ChangeDirection();
            timer = 3f;
        }
    }

    void ChangeDirection(){
        //Vector3 newdirection = new Vector3(Random.Range(-0.2f,0.2f), Random.Range(-0.2f,0.2f),Random.Range(-0.2f,0.2f));
        direction = new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f),Random.Range(-1f,1f));
        transform.rotation = Quaternion.LookRotation(direction);
    }

}
