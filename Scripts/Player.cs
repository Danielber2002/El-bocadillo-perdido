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
    public float gravedad = 40f; // Gravedad manual (cuanto más alta, más rápido cae)

    [Header("Estado")]
    public bool isGrounded; // ¿Está tocando suelo?
    public bool isJumping;  // ¿Está en la fase de salto?

    [Header("Configuración de Ataque")]
    public Transform attackPoint; // El punto desde donde sale el golpe (el puño)
    public float attackRange = 0.5f; // El radio del golpe
    public LayerMask enemyLayers; // Para que solo golpeemos enemigos, no paredes ni al suelo
    public int attackDamage = 20;

    [Header("Cooldown")]
    public float attackRate = 2f; // Cuántos ataques por segundo
    float nextAttackTime = 0f;

    private Rigidbody2D rb;
    private float velocidadVertical; // Variable para controlar la altura del salto
    private float sueloY; // Recordaremos en qué Y "profundidad" saltamos

    private Animator animator;

    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Aseguramos que sea Kinematic por código para evitar errores
        rb.bodyType = RigidbodyType2D.Kinematic;

        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        // Nos aseguramos de que la imagen empiece llena (1 = 100%)
        if (healthBar != null)
        {
            healthBar.fillAmount = 1f;
        }
    }

    void Update()
    {
        // 1. DETECTAR INTENCIÓN DE ATAQUE (Antes de nada)
        // Si tocamos el botón y ya pasó el tiempo de espera...
        if (Time.time >= nextAttackTime && !isAttacking)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Return))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        // 2. MODO COMBATE (SEMÁFORO ROJO)
        // Si estamos atacando, el código se detiene aquí y no deja moverte.
        if (isAttacking)
        {
            // Forzamos a que la animación de caminar se apague SÍ o SÍ
            if (animator != null) animator.SetInteger("Walking", 0);

            // FRENADA TOTAL:
            if (!isJumping)
            {
                // En el suelo: Quieto absoluto en X e Y (evita que resbales)
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                // En el aire: Quieto en X, pero dejamos que la gravedad baje en Y
                velocidadVertical -= gravedad * Time.deltaTime;
                rb.linearVelocity = new Vector2(0, velocidadVertical);

                // Lógica de aterrizaje (por si caes al suelo mientras atacas)
                if (velocidadVertical < 0 && transform.position.y <= sueloY)
                {
                    Aterrizar();
                }
            }

            // "return" expulsa al código de Update. 
            // Las líneas de abajo (movimiento) NO se leerán.
            return;
        }

        // ---------------------------------------------------------
        // 3. MODO MOVIMIENTO (SEMÁFORO VERDE)
        // Si llegamos aquí, es que NO estamos atacando.
        // ---------------------------------------------------------

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

        // Animación Caminar
        if (inputX != 0 || inputY != 0)
        {
            if (animator != null) animator.SetInteger("Walking", 1);
        }
        else
        {
            if (animator != null) animator.SetInteger("Walking", 0);
        }

        Vector2 movimientoFinal;

        // Movimiento en Suelo
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
        // Movimiento en Aire
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

        // CAMBIO 2: Actualizar la Imagen Filled
        if (healthBar != null)
        {
            // IMPORTANTE: Convertimos a (float) para tener decimales.
            // Ejemplo: 80 / 100 = 0.8
            healthBar.fillAmount = (float)currentHealth / maxHealth;

        }

        // Chequeo de muerte
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Attack()
    {
        // 1. ACTIVAMOS EL SEMÁFORO ROJO
        isAttacking = true;

        // 2. Configuramos animaciones manualmente para evitar conflictos
        if (animator != null)
        {
            animator.SetInteger("Walking", 0);   // Apagar caminar
            animator.SetInteger("Attack", 1); // Encender ataque
        }

        // 3. Detectar enemigos (Copiado de tu código anterior)
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Lógica de daño
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
            }
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            // Lógica de daño
            EnemyBoss enemyScript = enemy.GetComponent<EnemyBoss>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
            }
        }

        // 4. Programamos el fin del ataque
        // Ajusta 0.4f a la duración real de tu animación (ej: 0.5f o 0.3f)
        Invoke("ResetAttackAnim", 0.4f);
    }

    // ESTA FUNCIÓN ES NUEVA, ASEGÚRATE DE PEGARLA
    void ResetAttackAnim()
    {
        // Apagamos semáforo y animación
        isAttacking = false;
        if (animator != null) animator.SetInteger("Attack", 0);
    }


    // Esto dibuja el círculo en el editor para que veas el rango (Es invisible en el juego)
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

        // Corregimos la posición para que quede exacto en el suelo y no un poco abajo
        transform.position = new Vector3(transform.position.x, sueloY, transform.position.z);
    }

    public void LimitarPosicion()
    {
        // 1. Limite Horizontal (Siempre activo)
        float xClamped = Mathf.Clamp(transform.position.x, -12, 32);

        // 2. Limite Vertical (Truco del Beat Em Up)
        float yClamped = transform.position.y;

        // Solo limitamos el TECHO (-8) si estamos caminando.
        // Si saltamos, el techo desaparece (ponemos 100 o infinito).
        if (!isJumping)
        {
            yClamped = Mathf.Clamp(transform.position.y, -17, -8);
        }
        else
        {
            // Si estamos saltando, solo nos preocupa no caer al infierno (-13)
            // pero dejamos que suba todo lo que quiera.
            yClamped = Mathf.Clamp(transform.position.y, -17, 100);
        }

        transform.position = new Vector3(xClamped, yClamped, transform.position.z);
    }

    void Die()
    {
        Debug.Log("Game Over");
        // Asegúrate de que tu escena se llame EXACTAMENTE "GameOver"
        // o pon aquí el número de índice de la escena (ej: LoadScene(2))
        SceneManager.LoadScene("Game Over");
    }
}