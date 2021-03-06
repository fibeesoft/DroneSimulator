﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] Transform carSpawner1, carSpawner2, carSpawner3;
    [SerializeField] GameObject carPrefab, beePrefab;
    [SerializeField] GameObject drone;
    [SerializeField] GameObject drone3dmodel;
    [SerializeField] GameObject firePrefab;
    [SerializeField] Text txt_counter;
    [SerializeField] Transform lanternContainer;
    [SerializeField] GameObject lanternPrefab;
    public AudioClip[] clips;
    public AudioClip clickClip;
    AudioSource audioSource;
    float timeInGame;
    float counter = 50f;
    float movex = 0f, movey = 0f, movez = 0f;
    public bool IsDronAlive{get;set;}
    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        CreateBees();
        if(txt_counter != null){
            txt_counter.text = counter.ToString("0");
        }
        timeInGame = Time.time;
        SpawnPrefab(carSpawner1, carPrefab);
        SpawnPrefab(carSpawner2, carPrefab, true);
        SpawnLanterns();
    }

    private void Update() {
        if(Time.time > timeInGame + 8f){
            SpawnPrefab(carSpawner1, carPrefab);
            SpawnPrefab(carSpawner2, carPrefab, true);
            SpawnPrefab(carSpawner3, carPrefab, true);

            timeInGame = Time.time;
        }
        counter -= Time.deltaTime;
        if(txt_counter != null){
            txt_counter.text = counter.ToString("0");
        }
        if(counter <= 0){
            GameOver();
        }

    }


    void SpawnLanterns(){
        if(lanternPrefab != null){
            for (int i = 0; i < 17; i++){
                GameObject g = Instantiate(lanternPrefab, new Vector3(movex + 52f,movey,movez), Quaternion.identity);
                GameObject h = Instantiate(lanternPrefab, new Vector3(movex - 52f,movey,movez), Quaternion.Euler(0f,180f,0f));
                movez += 120f;
            }
        }
    }

    void SpawnPrefab(Transform spawnPoint, GameObject prefab){
        if(spawnPoint != null && prefab != null){
            GameObject car = Instantiate(prefab, spawnPoint.transform.position, Quaternion.identity);
            car.GetComponentInChildren<MeshRenderer>().materials[0].color = new Color32((byte)Random.Range(0,255),(byte)Random.Range(0,255),(byte)Random.Range(0,255),255 );        }
    }

    void SpawnPrefab(Transform spawnPoint, GameObject prefab, bool isRotated){
        if(spawnPoint != null && prefab != null){
            GameObject car = Instantiate(prefab, spawnPoint.transform.position, Quaternion.Euler(0f,180f,0f));
            car.GetComponentInChildren<MeshRenderer>().materials[0].color = new Color32((byte)Random.Range(0,255),(byte)Random.Range(0,255),(byte)Random.Range(0,255),255 );
        }
    }


    public void QuitTheGame(){
        Application.Quit();
    }
    public IEnumerator GoToMainMenu(float time){
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(0);
    }

    public IEnumerator GoToGameScene(float time){
        PlayClickSound();
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(1);
    }
    public void StartGame(){
        //SceneManager.LoadScene(1);
        PlayClickSound();
        StartCoroutine(GoToGameScene(0.3f));
        IsDronAlive = true;
    }

    

    public void GoToMainMenu(){
        PlayClickSound();
        StartCoroutine(GoToMainMenu(0.3f));
    }
    public void Success(){
        SceneManager.LoadScene(2);
    }
    public void GameOver(){
        //SceneManager.LoadScene(3);
        StartCoroutine(GameOverAnimation());
    }

    IEnumerator GameOverAnimation(){
        IsDronAlive = false;
        GameObject.FindGameObjectWithTag("PilotPanel").GetComponent<Image>().color = Color.red;
        if(firePrefab != null){
            GameObject g = Instantiate(firePrefab, drone.transform.position , Quaternion.identity);
            g.transform.SetParent(drone.transform);
            g.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
        }
        yield return new WaitForSeconds(3);
        StartCoroutine( GoToMainMenu(0f));
    }

    void CreateBees(){
        if(beePrefab != null){
            for(int i = 0; i < 100; i++){
                GameObject g = Instantiate(beePrefab, new Vector3(Random.Range(-60f,60f), Random.Range(15f,25f), Random.Range(60,1700)), Quaternion.identity);
            }
        }
    }

    public void PlayClickSound(){
        audioSource.clip = clickClip;
        audioSource.Play();
    }
}
