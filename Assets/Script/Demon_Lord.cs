using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon_Lord : MonoBehaviour
{
    public GameObject charge;
    public Transform target;
    public Transform chargeOrigin;
    public Animator animator;

    bool charging = false;
    bool born = false;
    public float chargeTime;

    public float maxHealth = 95.0f;
    public float currentHealth;

    AudioSource audioSource;

    public GameObject loader;
    void Start(){
        chargeTime = 4.0f;
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate(){
        if (born == true){
            audioSource.Play();
            chargeTime -= Time.deltaTime;
            if (chargeTime <= 0){
                charging = !charging;
                animator.SetTrigger("Charge");
                if (charging == true){
                    charging = !charging;
                    chargeTime = 4.0f;
                }
            }
        }
    }

    public void ChargeShooting(){
        Vector2 dir = target.position - chargeOrigin.position;
        GameObject chargePrefab = Instantiate(charge, chargeOrigin.position, Quaternion.identity);

        Demon_Charger d = chargePrefab.GetComponent<Demon_Charger>();
        d.Charge(dir);
    }

    void OnTriggerEnter2D(Collider2D other){
        Character_Controller player = other.GetComponent<Character_Controller>();
        if (player != null) { 
            animator.SetTrigger("Born");
            born = true;
        }
        GetComponent<CircleCollider2D>().enabled = false;
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) { 
            Destroy(gameObject);
            Level_Loader load = loader.GetComponent<Level_Loader>();
            load.LoadNextLevel(0, 3);
        }
    }
}
