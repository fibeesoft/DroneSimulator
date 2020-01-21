using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneMovement : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Slider boostSlider;
    Rigidbody rb;
    float movex, rotateDrone, movey, moveForward;
    float speedUp = 15f;
    float speedX = 20f;
    float speedForward = 15f;
    float tiltAngle = 25f;
    Transform[] propellers = new Transform[4];
    bool hasDroneStarted = true;
    float shootDelay = 0.2f;
    float timeInGame;
    float boost, boostSpeed;
    float rotateAngle = 15f;
    float maxboostPoints = 100f;
    float boostPoints;
    void Start()
    {
        boostPoints = 60f;
        rb = GetComponent<Rigidbody>();
        GetPropellers();
        timeInGame = Time.time;
        boostSpeed = 5f;
        boostSlider.maxValue = maxboostPoints;
        boostSlider.value = boostPoints;
    }


    void Update()
    {
        movex = Input.GetAxis("HorizontalTurn");
        moveForward = Input.GetAxis("VerticalTurn");
        movey = Input.GetAxis("Vertical");
        rotateDrone = Input.GetAxisRaw("Horizontal");
        if(boostPoints >= 1){
            boost = Input.GetAxisRaw("Boost");
        }else{
            boost = 0;
        }
        RotateProps();
        Shoot();
        ActivateBoost();
    }

    void ActivateBoost(){
        if(boost <= 0.2f){
            if(boostPoints < maxboostPoints){
                boostPoints += Time.deltaTime * 10;
            }
        }else{
            if(boostPoints > 1){
                boostPoints -= Time.deltaTime * 30;
            }
        }
        boostSlider.value = boostPoints;
    }

    private void FixedUpdate() {
        rb.velocity = new Vector3(movex * speedX * (1 + boost * 4/speedX * boostSpeed), movey * speedUp * (1 + boost * 2/speedUp * boostSpeed), moveForward * speedForward * 2 * (1 + boost * 4/speedForward * boostSpeed));
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
}
