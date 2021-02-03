using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Script : MonoBehaviour
{
    Rigidbody2D rgbd2d;
    public float bulletDamage = 5.0f;
    float destroyTime = 0.8f;
    
    void Awake(){
        rgbd2d = GetComponent<Rigidbody2D>();
    }

    public void Fire(Vector2 direction, float force){
        rgbd2d.AddForce(direction * force);
    }

    void OnTriggerEnter2D(Collider2D col){
        if (col.tag == "Gore Zombie"){
            col.GetComponent<Enemy_Controller>().TakeDamage(bulletDamage);
        }
        if (col.tag == "Boss")
        {
            col.GetComponent<Demon_Lord>().TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
  
    void FixedUpdate(){
        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0f)
            Destroy(gameObject);   
    }
}
