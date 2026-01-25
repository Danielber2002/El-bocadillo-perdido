using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Salud")]
    public int maxHealth = 100;
    public int currentHealth;
    public Image healthBar;

    [Header("Ajustes de Movimiento")]
    public float speed = 5f;
    public float jumpForce = 5f;
    public float gravedad = 40f; 

    [Header("Estado")]
    public bool isGrounded; 
    public bool isJumping;  

    [Header("Configuración de Ataque")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 20;

    [Header("Cooldown")]
    public float attackRate = 2f; 
    float nextAttackTime = 0f;

    private Rigidbody2D rb;
    private float velocidadVertical; 
    private float sueloY; 

    private Animator animator;

    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.fillAmount = 1f;
        }
    }

    void Update()
    {
        if (Time.time >= nextAttackTime && !isAttacking)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Return))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        if (isAttacking)
        {
            if (animator != null) animator.SetInteger("Walking", 0);

            if (!isJumping)
            {
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                velocidadVertical -= gravedad * Time.deltaTime;
                rb.linearVelocity = new Vector2(0, velocidadVertical);

                if (velocidadVertical < 0 && transform.position.y <= sueloY)
                {
                    Aterrizar();
                }
            }
            return;
        }

        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (inputX > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (inputX < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (inputX != 0 || inputY != 0)
        {
            if (animator != null) animator.SetInteger("Walking", 1);
        }
        else
        {
            if (animator != null) animator.SetInteger("Walking", 0);
        }

        Vector2 movimientoFinal;

        if (!isJumping)
        {
            movimientoFinal = new Vector2(inputX * speed, inputY * speed);
            velocidadVertical = 0f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                velocidadVertical = jumpForce;
                sueloY = transform.position.y;
            }
        }

        else
        {
            velocidadVertical -= gravedad * Time.deltaTime;
            movimientoFinal = new Vector2(inputX * speed, velocidadVertical);
        }

        rb.linearVelocity = movimientoFinal;
        LimitarPosicion();

        if (isJumping && velocidadVertical < 0 && transform.position.y <= sueloY)
        {
            Aterrizar();
        }
    }

    public void TakeDamage(int damage)
    {

        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Attack()
    {
        isAttacking = true;


        if (animator != null)
        {
            animator.SetInteger("Walking", 0);   
            animator.SetInteger("Attack", 1); 
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
            }
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyBoss enemyScript = enemy.GetComponent<EnemyBoss>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
            }
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            DestructibleObject enemyScript = enemy.GetComponent<DestructibleObject>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
            }
        }

        Invoke("ResetAttackAnim", 0.4f);
    }

    void ResetAttackAnim()
    {

        isAttacking = false;
        if (animator != null) animator.SetInteger("Attack", 0);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void Aterrizar()
    {
        isJumping = false;
        velocidadVertical = 0;

        transform.position = new Vector3(transform.position.x, sueloY, transform.position.z);
    }

    public void LimitarPosicion()
    {
        float xClamped = Mathf.Clamp(transform.position.x, -12, 32);
        float yClamped = transform.position.y;

        if (!isJumping)
        {
            yClamped = Mathf.Clamp(transform.position.y, -17, -8);
        }
        else
        {
            yClamped = Mathf.Clamp(transform.position.y, -17, 100);
        }

        transform.position = new Vector3(xClamped, yClamped, transform.position.z);
    }

    void Die()
    {
        SceneManager.LoadScene("Game Over");
    }

    public void Curar(int cantidad)
    {
        currentHealth += cantidad;


        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

      
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }

        Debug.Log("Jugador curado. Vida actual: " + currentHealth);
    }
}