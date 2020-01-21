using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneMovement : MonoBehaviour
{
    [SerializeField] Image heightImage;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Slider boostSlider;
    [SerializeField] Transform crosshairTransform;
    [SerializeField] GameObject DroneObject;
    Rigidbody rb;
    float movex, movey, moveForward;
    float speedUp = 15f;
    float speedX = 20f;
    float speedForward = 15f;
    float tiltAngle = 25f;
    Transform[] propellers = new Transform[4];
    bool hasDroneStarted = true;
    float shootDelay = 0.2f;
    float timeInGame;
    float boostSpeed;
    float maxboostPoints = 100f;
    float boostPoints;
    bool isBoostActivated = false;
    void Start()
    {
        boostPoints = 60f;
        rb = GetComponent<Rigidbody>();
        GetPropellers();
        timeInGame = Time.time;
        boostSlider.maxValue = maxboostPoints;
        boostSlider.value = boostPoints;
    }


    void Update()
    {
        movex = SimpleInput.GetAxis("Horizontal");
        moveForward = SimpleInput.GetAxis("Vertical");
        movey = SimpleInput.GetAxis("VerticalLift");
        RotateProps();
        BoostCharging();
        MoveHeightScale();
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
            }else{
                isBoostActivated = false;
                boostSpeed = 0f;
            }           
        }
    }    


    void MoveHeightScale(){
        heightImage.rectTransform.localPosition = new Vector3(heightImage.rectTransform.localPosition.x,- transform.position.y * 5f, heightImage.rectTransform.localPosition.z);
    }        
    public void ActivateBoost(){
        print("boost activated");
        isBoostActivated = true;
        boostSpeed = 3f;
    }

    private void FixedUpdate() {
        rb.velocity = new Vector3(movex * speedX * (1 + boostSpeed), movey * speedUp, moveForward * speedForward * 2 * (1 + boostSpeed));
        //rb.rotation = Quaternion.Euler(tiltAngle * moveForward, 0f, -tiltAngle * movex);
        DroneObject.transform.rotation = Quaternion.Euler(tiltAngle * moveForward, 0f, -tiltAngle * movex);
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

    public void Shoot(){
        if(timeInGame + shootDelay < Time.time){
            //GameObject g = Instantiate(bulletPrefab, transform.position + new Vector3(0f,0.3f,1.2f), Quaternion.identity);
            GameObject g = Instantiate(bulletPrefab, crosshairTransform.transform.position, Quaternion.identity);
            //g.GetComponent<Bullet>().Initialize(crosshairTransform.transform.position - aimTransform.transform.position);
            timeInGame = Time.time;
        }
    }
}
