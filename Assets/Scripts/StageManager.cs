using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    public delegate void HitEvent();
    public HitEvent OnHit;
    public GameObject antPrefab;

    public List<Ant> antsSpawned;
    public List<Checkpoint> checkpoints;
    public Transform[] spawns;

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
        if(Time.time > timer + spawnSpeed){
            timer = Time.time;
            spawnSpeed = Random.Range(0.2f, 0.5f);
            ResetAnt();
        }
    }
}
