using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] Transform carSpawner1, carSpawner2, carSpawner3;
    [SerializeField] GameObject carPrefab;
    [SerializeField] Text txt_counter;
    float timeInGame;
    float counter = 60f;
    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if(txt_counter != null){
            txt_counter.text = counter.ToString("0");
        }
        timeInGame = Time.time;
        SpawnPrefab(carSpawner1, carPrefab);
        SpawnPrefab(carSpawner2, carPrefab, true);
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




    void SpawnPrefab(Transform spawnPoint, GameObject prefab){
        if(spawnPoint != null && prefab != null){
            GameObject car = Instantiate(prefab, spawnPoint.transform.position, Quaternion.identity);
        }
    }

    void SpawnPrefab(Transform spawnPoint, GameObject prefab, bool isRotated){
        if(spawnPoint != null && prefab != null){
            GameObject car = Instantiate(prefab, spawnPoint.transform.position, Quaternion.Euler(0f,180f,0f));
        }
    }


    public void QuitTheGame(){
        Application.Quit();
    }
    public void GoToMainMenu(){
        SceneManager.LoadScene(0);
    }
    public void StartGame(){
        SceneManager.LoadScene(1);
    }
    public void Success(){
        SceneManager.LoadScene(2);
    }
    public void GameOver(){
        SceneManager.LoadScene(3);
    }
}
