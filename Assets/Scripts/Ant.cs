using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{

    public enum AntState{move, sense, dead, panic}
    public AntState state;
    public Animator animator;
    public bool panic;
    public float speed = 1;
    public float panicSpeed = 1;
    public float panicTime =5;
    public SpriteRenderer blood;
    float panicTimer;
    public List<Checkpoint> checkpoints;

    public StageManager manager;
    public MeshRenderer bodyMesh;
    Rigidbody rigidbody;
    int curCheckpoint;
    public float closeEnough = 1f;
    public float wiggleFrequency = 1f;
    public float wiggleAmplitude = 2f;

    Vector3 wiggleDir => new Vector3(perlinX, perlinZ, 0) * wiggleAmplitude;
    Vector3 moveDir;
    public float pauseChance = 1f;
    public float pauseTime = 2f;
    float pauseTimer;
    float offset;

    float timeSinceSpawned;
    float perlinX;
    float perlinZ;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        offset = Random.Range(0f, 100f);
        transform.forward = (checkpoints[0].transform. position - transform.position).normalized;
    }

    public void Move(Vector3 dir){
        
        dir = Vector3.Lerp(transform.forward, dir.normalized, Time.deltaTime * 10f);
        
        transform.rotation = Quaternion.LookRotation(dir, -Vector3.forward);
        float curSpeed = state == AntState.panic ? panicSpeed : speed;
        Vector3 finalDir = dir;
        finalDir.z = 0;
        transform.position += finalDir * curSpeed * Time.deltaTime;
    }

    public void Kill(){

        if(state == AntState.dead) return;

        state = AntState.dead;
        animator.SetTrigger("Die");
        rigidbody.detectCollisions = false;
        transform.Rotate(0,0,Random.Range(0, 360));
        //blood.enabled = true;
        blood.transform.forward = Camera.main.transform.position - blood.transform.position;
        blood.transform.Rotate(0, 0, Random.Range(0, 360));
    }

    public void Panic(){
        panic = true;
        panicTimer = Time.time + panicTime * Random.Range(0.5f, 1.5f);
        state = AntState.panic;
        animator.SetBool("Sensing", false);
    }
    public void Sense(){
        state = AntState.sense;
        pauseTimer = Time.time + Random.Range(pauseTime, pauseTime * 2f);
        animator.SetBool("Sensing", true);
    }

    public void Reset(){
        rigidbody.detectCollisions = true;
        animator.SetTrigger("Reset");
        state = AntState.move;
        curCheckpoint = 0;
        animator.SetBool("Sensing" , false);
        timeSinceSpawned = 0;
        gameObject.SetActive(false);
        manager.antsSpawned.Add(this);
    }

    void Update() 
    {
        timeSinceSpawned += Time.deltaTime;

        if(!bodyMesh.isVisible && timeSinceSpawned > 5){
            Reset();
            return;
        }

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        perlinX = Mathf.PerlinNoise(Time.time * wiggleFrequency + offset, -Time.time * wiggleFrequency + offset);
        perlinZ = Mathf.PerlinNoise(-Time.time * wiggleFrequency - offset, Time.time * wiggleFrequency - offset);
        perlinX = (perlinX - 0.5f) * 2f;
        perlinZ = (perlinZ - 0.5f) * 2f;

        switch(state){
            case AntState.move:

            if(curCheckpoint < checkpoints.Count){
                moveDir = checkpoints[curCheckpoint].transform.position - transform.position;
                Move(moveDir.normalized + wiggleDir);

                if(moveDir.magnitude < closeEnough){
                    curCheckpoint ++;
                    if(curCheckpoint >= checkpoints.Count){
                         //what now
                    }
                }

                if(Random.Range(0, 100f) < pauseChance * Time.deltaTime){
                    Sense();
                }
            }

            break;

            case AntState.panic:
                if(panicTimer < Time.time){
                    state = AntState.move;
                }else{
                    Move(wiggleDir.normalized);
                }
            break;

            case AntState.sense:
                if(pauseTimer < Time.time){
                    state = AntState.move;
                    animator.SetBool("Sensing", false);
                }
            break;
        }
    }
}
