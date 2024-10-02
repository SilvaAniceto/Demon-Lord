using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : MonoBehaviour
{
    // Variável p/ movimento
    public float speed = 3f;
    Vector2 characterDirection = new Vector2(1, 0);

    // Variável p/ pulo
    public float jumpForce = 3f;
    public bool grounded = true;

    // Variável p/ animação
    public Animator animator;

    // Variável p/ ataque
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float barAttackDamage = 3.0f;
    bool canAttack = true;
    float attackTime;

    // Variáveis de Disparo
    bool trigger = true;
    bool draw = false;
    public GameObject bullet;
    float shootTime;
    int maxAmmo = 7;
    public int currentAmmo;

    // Variáveis de vida
    public float maxHealth = 10.0f;
    float currentHealth;
    bool invulnerability = false;
    float timeInvulnerable;

    // Variáveis de som
    AudioSource audioSource;

    public AudioClip pistolShot;
    public AudioClip barHit;
    public AudioClip attack;
    public AudioClip getHit;
    public AudioClip load;

    public GameObject loader;

    void Start()
    {
        currentHealth = maxHealth;
        currentAmmo = maxAmmo;
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        // Configurar Movimento Padrão
        transform.Translate(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, 0f, 0f);

        // Configurar a Orientação do Personagem
        Vector3 characterFlip = transform.localScale;
        if (Input.GetAxisRaw("Horizontal") < 0 && transform.localScale.x > 0)
        {
            characterFlip.x *= -1f;
            transform.localScale = characterFlip;
            characterDirection.Set(-1, 0);
        }
        if (Input.GetAxisRaw("Horizontal") > 0 && transform.localScale.x < 0)
        {
            characterFlip.x *= -1f;
            transform.localScale = characterFlip;
            characterDirection.Set(1, 0);
        }

        // Configurar o Pulo do Personagem
        if (grounded && Input.GetButtonDown("Jump"))
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            grounded = false;
        }

        // Configurar intervalo entre danos recebidos
        if (invulnerability == true)
        {
            timeInvulnerable -= Time.deltaTime;
            if (timeInvulnerable < 0)
                invulnerability = false;
        }

        //Configuração de Saque e Disparo
        if (Input.GetKeyDown(KeyCode.C) && grounded == true)
        {
            draw = !draw;
            animator.SetBool("isDraw", draw);
        }

        if (trigger == false){
            shootTime -= Time.deltaTime;
            if (shootTime <= 0)
                trigger = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && invulnerability == false && grounded == true && draw == true && trigger == true)
            if (currentAmmo > 0)
                Shoot();

        // Configurar Ataque Corpo-a Corpo
        if (canAttack == false){
            attackTime -= Time.deltaTime;
            if (attackTime <= 0)
                canAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && invulnerability == false && grounded == true && draw == false && canAttack == true)
            Attack();

        // CONFIGURAR ANIMAÇÕES


        // Animação de Movimento
        animator.SetFloat("isMoving", Mathf.Abs(Input.GetAxisRaw("Horizontal")));

        // Animação de Pulo
        if (gameObject.GetComponent<Rigidbody2D>().velocity.y > 0 && grounded == false)
            animator.SetBool("isJumping", true);
        else
            animator.SetBool("isJumping", false);

        // Animação de Queda
        if (gameObject.GetComponent<Rigidbody2D>().velocity.y < 0 && grounded == false)
            animator.SetBool("isFalling", true);
        else
            animator.SetBool("isFalling", false);
    }

    void OnCollisionEnter2D(){
        grounded = true;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void Attack()
    {
        canAttack = false;
        attackTime = 0.6f;
        animator.SetTrigger("isAttacking");
        audioSource.PlayOneShot(attack);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, LayerMask.GetMask("Enemies"));

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy_Controller>().TakeDamage(barAttackDamage);
            audioSource.PlayOneShot(barHit);
        }

        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, LayerMask.GetMask("Boss"));

        foreach (Collider2D enemy in hitBoss)
        {
            enemy.GetComponent<Demon_Lord>().TakeDamage(barAttackDamage);
            audioSource.PlayOneShot(barHit);
        }
    }

    public void ReceivingDamage(float damage)
    {
        if (invulnerability == false)
        {
            timeInvulnerable = 0.8f;
            invulnerability = true;
            animator.SetTrigger("isHit");
            audioSource.PlayOneShot(getHit);
            currentHealth -= damage;
            if (currentHealth <= 0)
                Died();
        }
        Health_Bar.instance.SetValue(currentHealth / maxHealth);
    }


    void Died()
    {
        animator.SetBool("isDead", true);

        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0f;

        Level_Loader load = loader.GetComponent<Level_Loader>();
        load.LoadNextLevel(0,3);

        this.enabled = false;
    }

    void Shoot(){
        ChangeAmmo(-1);
        trigger = false;
        shootTime = 0.8f;
        GameObject bulletPrefab = Instantiate(bullet, attackPoint.position, Quaternion.identity);

        Bullet_Script b = bulletPrefab.GetComponent<Bullet_Script>();
        b.Fire(characterDirection, 800);

        animator.SetTrigger("isShooting");
        audioSource.PlayOneShot(pistolShot);
    }

    public void ChangeAmmo(int changer){
        currentAmmo = Mathf.Clamp(currentAmmo + changer, 0, maxAmmo);
        audioSource.PlayOneShot(load);
    }
}