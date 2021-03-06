﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneMovement : MonoBehaviour
{
    [SerializeField] Image heightImage;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject impactPrefab;
    [SerializeField] GameObject explodePrefab;

    [SerializeField] Slider boostSlider;
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider batterySlider;
    [SerializeField] Image batterySliderImage;
    [SerializeField] GameObject DroneObject;
    Rigidbody rb;
    float movex, movey, moveForward;
    float speedUp = 25f;
    float speedX = 20f;
    float speedForward = 20f;
    float tiltAngle = 25f;
    Transform[] propellers = new Transform[4];
    bool hasDroneStarted = true;
    float timeInGame;
    float boostSpeed;
    float maxboostPoints = 100f;
    float maxhp = 5f;
    float hp;
    float maxBattery = 100f;
    float battery;
    Vector3 lastPosition, actualPosition, playerDirection;

    float boostPoints;
    bool isBoostActivated = false;
    AudioSource audioSource;
    [SerializeField] AudioSource audioSource2;
    void Start()
    {
        GameManager.instance.IsDronAlive = true;
        audioSource = GetComponent<AudioSource>();
        battery = maxBattery;
        batterySlider.maxValue = maxBattery;
        batterySlider.value = battery;
        hp = maxhp;
        boostPoints = 80f;
        rb = GetComponent<Rigidbody>();
        GetPropellers();
        timeInGame = Time.time;
        boostSlider.maxValue = maxboostPoints;
        boostSlider.value = boostPoints;
        hpSlider.maxValue = maxhp;
        hpSlider.value = hp;
        lastPosition = transform.position;
        audioSource.clip = GameManager.instance.clips[4];
        audioSource.Play();
    }

    void Update()
    {
        if(GameManager.instance.IsDronAlive){
            actualPosition = transform.position;
            movex = SimpleInput.GetAxis("Horizontal");
            moveForward = SimpleInput.GetAxis("Vertical");
            movey = SimpleInput.GetAxis("VerticalLift");
            if(Input.GetKeyDown(KeyCode.Space)){
                ActivateBoost();
            }
            Move();
            lastPosition = actualPosition;
            actualPosition = transform.position;
            playerDirection = actualPosition - lastPosition;

            RotateProps();
            BoostCharging();
            MoveHeightScale();
            BatteryDischarge();      
        }else{
            if(audioSource.pitch > 0){
                audioSource.pitch -= 0.5f * Time.deltaTime;
            }
            CollapseBroken();  
        }
    }


    void CollapseBroken(){
         gameObject.transform.Rotate(0f,0.1f,0.2f,Space.Self);
         transform.position += new Vector3(playerDirection.x * 100f,-15f,playerDirection.z * 200f)*Time.deltaTime;

    }
    void BoostCharging(){
        if(boostPoints < maxboostPoints){
            boostPoints += Time.deltaTime * 10;
            boostSlider.value = boostPoints;
        }
        if(isBoostActivated){
            if(boostPoints > 1){
                boostPoints -= Time.deltaTime * 40;
                boostSlider.value = boostPoints;
                audioSource.pitch = 1.0f;
            }else{
                isBoostActivated = false;
                boostSpeed = 0f;
                audioSource.pitch = 0.8f;
            }           
        }
    }

    void BatteryDischarge(){
        if(battery > 0.1f){
            battery -= Time.deltaTime * 4;
            batterySlider.value = battery;
            if(battery > 50f){
                batterySliderImage.color = new Color32(0,186,245,255);
            }
            else if(battery > 30f){
                 batterySliderImage.color = new Color32(245,145,0,255);   
            }
            else{
                batterySliderImage.color = new Color32(245,30,0,255);
            }
        }else{
            GameManager.instance.GameOver();
        }      
    }    

    void MoveHeightScale(){
        heightImage.rectTransform.localPosition = new Vector3(heightImage.rectTransform.localPosition.x,- transform.position.y * 5f, heightImage.rectTransform.localPosition.z);
    }        
    public void ActivateBoost(){
        print("boost activated");
        isBoostActivated = true;
        boostSpeed = 2f;
    }

    void Move(){
        transform.position += new Vector3(movex * speedX * (1 + boostSpeed), movey * speedUp, moveForward * speedForward * 2 * (1 + boostSpeed)) * Time.deltaTime;
        transform.rotation = Quaternion.Euler(tiltAngle * moveForward, 0f, -tiltAngle * movex);

        //Clamping drone movement
        Vector3 clampPos = transform.position;
        clampPos.x = Mathf.Clamp (clampPos.x, -75f,75f);
        clampPos.y = Mathf.Clamp (clampPos.y, 0.5f,50f);
        transform.position = clampPos;
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
        float idleSpeed = 1000f;
        float propSpeed = 3000f;
        for(int i = 0; i < propellers.Length; i++){
            if(hasDroneStarted){
                if(movey != 0 || movex != 0 || moveForward != 0 ){
                    propellers[i].transform.Rotate(0f,0f, (idleSpeed + propSpeed) * Time.deltaTime);
                    audioSource.pitch = 0.8f;
                }else{
                    propellers[i].transform.Rotate(0f,0f, idleSpeed * Time.deltaTime);
                    audioSource.pitch = 0.6f;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.transform.CompareTag("Battery")){
            if(battery <= 70f){
                battery += 30f;
            }else{
                battery = maxBattery;
            }
            Destroy(other.gameObject);
        }

        if(other.transform.CompareTag("Finish")){
            GameManager.instance.Success();
        }

        if(other.transform.CompareTag("Obstacle")){
            HitObstacle();
        }
    }

    void HitObstacle(){
        if(GameManager.instance.IsDronAlive == false){
            GameObject h = Instantiate(explodePrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(GameManager.instance.clips[2], transform.position);
            Destroy(gameObject,0f);
            StartCoroutine(GameManager.instance.GoToMainMenu(0.5f));
        }

        hp--;
        hpSlider.value = hp;
        transform.position -= playerDirection.normalized * 10f;
        GameObject g = Instantiate(impactPrefab, transform.position, Quaternion.identity);
        Destroy(g, 1f);
        if(hp < 0){
            GameManager.instance.GameOver();
        }
        audioSource2.clip = GameManager.instance.clips[0];
        audioSource2.Play();
    }
}
