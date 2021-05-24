using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
   
   public Animator animator;
   public SpriteRenderer handSprite;
   public Collider handCollider;
   public GameObject handContainer;

   public float slapCooldown;
   float timer;
   public AudioSource sound;
   public AudioClip[] slapSounds;
    bool whacked;

    bool justWhacked;

   
    void Update()
    {
      if(justWhacked){
            justWhacked = false;
            handCollider.enabled = false;
        }


        if(Input.GetMouseButtonDown(0) && Time.time > timer){
            Whack();
        }

        if(whacked && (Time.time > timer && !Input.GetMouseButton(0))){
            handContainer.SetActive(false);
            whacked = false;
        }
    }

    public void OnCollisionEnter(Collision col){
        Rigidbody rb = col.collider.attachedRigidbody;
        if(rb != null){
            Ant a = rb.GetComponent<Ant>();
            if(a != null){
                a.Kill();
            }
        }
    }
    public void Whack(){
      
                // Construct a ray from the current touch coordinates
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        // Create a particle if hit
        if (Physics.Raycast(ray, out raycastHit, 100f)){

            if(raycastHit.collider.attachedRigidbody != null && raycastHit.collider.attachedRigidbody.tag == "Untagged")return;

            timer = Time.time + slapCooldown;
            transform.position = raycastHit.point;
            handContainer.SetActive(true);
            justWhacked = true;
            handCollider.enabled = true;
            whacked = true;
            animator.SetTrigger("whack");
            sound.PlayOneShot(slapSounds[Random.Range(0, slapSounds.Length)]);

            
            if(StageManager.i.OnHit != null){
                StageManager.i.OnHit.Invoke();
            }
            Collider[] antsHit = Physics.OverlapSphere(raycastHit.point, 100f);

            bool hasKilled = false;
            foreach(Collider c in antsHit){
                
                if(c.attachedRigidbody != null && c.attachedRigidbody.tag == "Ant"){
                    Ant antScript = c.attachedRigidbody.GetComponent<Ant>();
                    if(antScript.state != Ant.AntState.dead){
                        antScript.Panic();
                    }
                }
            }    
        }
    }
}
