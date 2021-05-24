using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StageManager : MonoBehaviour
{

    public delegate void HitEvent();

    public int nextScene;
    public float timeUntilCut;
    public float spawnRateMin, spawnRateMax;
    public HitEvent OnHit;
    public GameObject antPrefab;

    public int spawnAmount = 15;
    public List<Ant> antsSpawned;
    public List<Checkpoint> checkpoints;
    public Transform[] spawns;

    int numAntsSpawned;
    public static StageManager i;
    float spawnSpeed;
    float timer;
    void Awake()
    {
        i = this;
    }

    public void ResetAnt(){
        if(antsSpawned.Count > 0){
            antsSpawned[0].transform.position = spawns[Random.Range(0, spawns.Length)].transform.position;
            antsSpawned[0].gameObject.SetActive(true);
            antsSpawned.RemoveAt(0);
        }else{
            SpawnAnt();
        }
    }

    public void SpawnAnt(){
        GameObject newAnt = Instantiate(antPrefab, spawns[Random.Range(0, spawns.Length)].position, Quaternion.identity);
        Ant antscript = newAnt.GetComponent<Ant>();
        antscript.checkpoints = checkpoints;
        antscript.manager = this;
    }
    void Update()
    {
        if(numAntsSpawned < spawnAmount && Time.time > timer + spawnSpeed){
            
            numAntsSpawned ++;
            spawnSpeed = Random.Range(spawnRateMin, spawnRateMax);
            timer = Time.time;
            ResetAnt();
        }

        if(Time.time > timeUntilCut){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1, LoadSceneMode.Single);
        }
    }
}
