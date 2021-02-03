using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon_Charger : MonoBehaviour
{
    Rigidbody2D rgbd2;    
    float destroyTime = 3.0f;

    
    void Awake(){
        rgbd2 = GetComponent<Rigidbody2D>();
        
    }
    void Update(){
        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0f)
            Destroy(gameObject);
    }

    public void Charge(Vector2 dir){
        rgbd2.AddForce(dir * 100.0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Character_Controller player = other.GetComponent<Character_Controller>();
        if (player != null){
            player.ReceivingDamage(2.5f);
            Destroy(gameObject);
        }
    }
}
