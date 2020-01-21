using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    Rigidbody rb;
    float movex, rotateDrone, movey, moveForward;
    float speedUp = 12f;
    float speedX = 10f;
    float speedForward = 10f;
    float tiltAngle = 25f;
    Transform[] propellers = new Transform[4];
    bool hasDroneStarted = true;
    float shootDelay = 0.2f;
    float timeInGame;
    float boost;
    float rotateAngle = 45f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetPropellers();
        timeInGame = Time.time;
    }


    void Update()
    {
        movex = Input.GetAxis("HorizontalTurn");
        moveForward = Input.GetAxis("VerticalTurn");
        movey = Input.GetAxis("Vertical");
        rotateDrone = Input.GetAxisRaw("Horizontal");
        boost = Input.GetAxisRaw("Boost");
        RotateProps();
        Shoot();
        //RotateDrone();
    }

    private void FixedUpdate() {
        rb.velocity = new Vector3(movex * speedX, movey * speedUp, moveForward * speedForward * 2 + boost * speedForward);
        rb.rotation = Quaternion.Euler(tiltAngle * moveForward, rotateAngle * rotateDrone, -tiltAngle * movex);
    }


    void GetPropellers(){
        Transform[] allChildren = gameObject.GetComponentsInChildren<Transform>();
        int propellerIndex = 0;
        for(int i = 0; i < allChildren.Length; i++){
            if(allChildren[i].transform.CompareTag("Prop")){
                propellers[propellerIndex] = allChildren[i];
                propellerIndex++;
            }
        }     
    }

    void RotateProps(){
        float idleSpeed = 2000f;
        float propSpeed = 3000f;
        for(int i = 0; i < propellers.Length; i++){
            if(hasDroneStarted){
                if(movey != 0 || movex != 0 || moveForward != 0 ){
                    propellers[i].transform.Rotate(0f,0f, (idleSpeed + propSpeed * movey + propSpeed * moveForward + movex * propSpeed) * Time.deltaTime);
                }else{
                    propellers[i].transform.Rotate(0f,0f, idleSpeed * Time.deltaTime);
                }
            }
        }
    }

    void Shoot(){
        if(Input.GetAxisRaw("Fire1") == 1){
            if(timeInGame + shootDelay < Time.time){
                GameObject g = Instantiate(bulletPrefab, transform.position + new Vector3(0f,0.3f,1.2f), Quaternion.identity);
                timeInGame = Time.time;
            }
        }
    }

    void RotateDrone(){
        float droneRotation = rb.rotation.y;
        droneRotation += rotateDrone * 20f;
        rb.rotation = Quaternion.Euler(0f,droneRotation, 0f);
    }
}
