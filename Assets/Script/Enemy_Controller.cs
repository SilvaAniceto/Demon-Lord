using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{
    // Varaiáveis de Vida
        public float maxHealth = 15f;
        float currentHealth;

    // Variáveis de Animação
        public Animator animator;

    // Variáveis de Movimento
        float direction = 1;
        float changeTime = 3.0f;
        float timer;
        bool flip = false;
        public bool canWalk;
        float walkTime;

    // Variáveis de detecção
        public Transform starRaycast;
        Vector2 lookDirection = new Vector2(1, 0);
        public float range = 2.0f;
        RaycastHit2D hit;

    // Variáveis de ataque
        public float damage = 1.0f;

    // Variáveis de som
    AudioSource audioSource;

    public AudioClip die;
    void Start(){
        currentHealth = maxHealth;
        timer = changeTime;
        canWalk = true;
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate(){
        hit = HasSight();

        if (canWalk == false){
            walkTime -= Time.deltaTime;
            if (walkTime < 0)
                canWalk = true;           
        }
        else{
            if (hit.collider != null){ 
                PersuePlayer();
            }
            else
                Move();
        }  
    }

    public void TakeDamage(float damage){
        canWalk = false;
        walkTime = 0.8f;
        currentHealth -= damage;
        animator.SetTrigger("Hit");
        
        if (currentHealth <= 0)
            Die();        
    }

    void Die(){
        animator.SetBool("isDead", true);

        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0f;
        GetComponent<Rigidbody2D>().velocity.Set(0f, 0f);
        audioSource.PlayOneShot(die);
        GetComponent<AudioSource>().enabled = false;
        this.enabled = false;
    }

    public void Move(){
        timer -= Time.deltaTime;
        if (timer < 0){
            direction = -direction;
            timer = changeTime;
            flip = !flip;    

            transform.Translate(Time.deltaTime * direction, 0f, 0f);
            GetComponent<SpriteRenderer>().flipX = flip;            
        }
        else       
            transform.Translate(Time.deltaTime * direction, 0f, 0f);      
    }

    void PersuePlayer(){
        transform.Translate(Time.deltaTime * direction * 2f, 0f, 0f);
    }

    RaycastHit2D HasSight(){
        if (direction < 0)
            lookDirection.Set(-1, 0);
        if (direction > 0)
            lookDirection.Set(1, 0);

        RaycastHit2D lineOfSight = Physics2D.Raycast(starRaycast.position, lookDirection, range, LayerMask.GetMask("Player"));
        return(lineOfSight);
    }

    void OnCollisionStay2D(Collision2D other){
        Character_Controller player = other.gameObject.GetComponent<Character_Controller>();        

        if (player != null && hit.collider != null){
            canWalk = false;
            walkTime = 0.8f;
            animator.SetTrigger("attack");
            player.ReceivingDamage(damage);
        }
    }
}