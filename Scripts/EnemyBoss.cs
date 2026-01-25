using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBoss : MonoBehaviour
{
    [Header("Estadísticas")]
    public string nombreEnemigo = "Baryonyx";
    public int maxHealth = 100;
    public int damage = 10;
    public int currentHealth;

    [Header("Movimiento e IA")]
    public float velocidad = 2f;
    public float distanciaAtaque = 1.5f;
    public float distanciaVision = 5f;

    // Configuración de Sprite
    public bool spriteMiraIzquierda;
    private Vector3 escalaOriginal;

    [Header("Referencias")]
    public Player playerScript; // Arrastra tu Player aquí
    private Animator animator;
    private Transform miTransform;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        miTransform = transform;
        escalaOriginal = transform.localScale;

        // Buscamos al player si se nos olvidó ponerlo
        if (playerScript == null)
            playerScript = FindFirstObjectByType<Player>();
    }

    void Update()
    {
        // Si el jugador no existe (murió), no hacemos nada
        if (playerScript == null) return;

        // Calculamos distancia
        float distancia = Vector2.Distance(miTransform.position, playerScript.transform.position);

        // --- MÁQUINA DE ESTADOS SIMPLE ---

        // 1. LEJOS -> IDLE
        if (distancia > distanciaVision)
        {
            animator.SetInteger("Moving", 0);
        }
        // 2. CERCA -> PERSEGUIR
        else if (distancia > distanciaAtaque)
        {
            animator.SetInteger("Moving", 1);
            animator.SetInteger("Attack", 0); // Aseguramos que no ataque mientras corre

            PerseguirJugador();
        }
        // 3. PEGADO -> ATACAR
        else
        {
            animator.SetInteger("Moving", 0);
            AttackPlayer();
        }
    }

    void PerseguirJugador()
    {
        Vector3 direccion = (playerScript.transform.position - miTransform.position).normalized;
        miTransform.position += direccion * velocidad * Time.deltaTime;

        // Girar el sprite
        FlipSprite(direccion.x);
    }

    // Este método se llama en el Update automáticamente si estás cerca
    void AttackPlayer()
    {
        // Activamos animación (Usamos SetInteger porque así lo tienes en tu imagen)
        animator.SetInteger("Attack", 1);

        // --- GOLPE AL JUGADOR ---
        // IMPORTANTE: Esto aplica el daño en cada frame si no lo controlamos.
        // Lo ideal es usar un EVENTO DE ANIMACIÓN, pero para probar ahora:
        // Solo dañamos si la animación acaba de empezar o usamos un temporizador.

        // *TRUCO TEMPORAL*: Vamos a confiar en el Evento de Animación "FinalizarAtaque"
        // para resetear el ataque, pero el daño lo aplicamos aquí con un pequeño cooldown interno
        // o mejor aún: USAMOS EL EVENTO DE ANIMACIÓN PARA HACER EL DAÑO.
    }

    // --- ESTOS MÉTODOS LOS LLAMA EL ANIMATOR (Eventos) ---

    // 1. Añade un evento en el frame del GOLPE que llame a esta función:
    public void DealDamage()
    {
        if (playerScript != null && Vector2.Distance(miTransform.position, playerScript.transform.position) <= distanciaAtaque)
        {
            Debug.Log("¡TOMA! Golpeando al jugador...");
            playerScript.TakeDamage(damage);
        }
    }

    // 2. Añade un evento al FINAL de la animación que llame a esta función:
    public void FinalizarAtaque()
    {
        animator.SetInteger("Attack", 0);
    }

    // --- RECIBIR DAÑO ---
    public void TakeDamage(int damageTaken)
    {
        currentHealth -= damageTaken;
        Debug.Log("Enemigo herido. Vida: " + currentHealth);

        animator.SetInteger("Hurt", 1);

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        animator.SetInteger("Hurt", 2); // Animación de muerte
        GetComponent<Collider2D>().enabled = false; // Ya no se le puede pegar

        Invoke("NextScene", 5f);
        this.enabled = false; // Desactiva este script
    }

    void NextScene()
    {
        int siguienteEscena = SceneManager.GetActiveScene().buildIndex + 1;

        
        if (siguienteEscena < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(siguienteEscena);
        }
        else
        {
           
            SceneManager.LoadScene(0);
        }
    }

    void FlipSprite(float xDireccion)
    {
        float mirada = xDireccion;
        if (spriteMiraIzquierda) mirada = -xDireccion;
        float tamanoX = Mathf.Abs(escalaOriginal.x);

        if (mirada > 0) miTransform.localScale = new Vector3(tamanoX, escalaOriginal.y, escalaOriginal.z);
        else if (mirada < 0) miTransform.localScale = new Vector3(-tamanoX, escalaOriginal.y, escalaOriginal.z);
    }
}