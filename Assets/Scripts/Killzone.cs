using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    
    public void OnCollisionEnter(Collision collision){
        Rigidbody rb = collision.contacts[0].otherCollider.attachedRigidbody;
        if(rb != null){
            Ant a = rb.GetComponent<Ant>();
            if(a != null){
                //recycle the ant;
                // a.gameObject.SetActive(false);
                a.transform.forward = Vector3.Reflect(a.transform.forward, collision.contacts[0].normal);
            }
        }
    }
}
