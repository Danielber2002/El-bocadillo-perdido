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

    public bool spriteMiraIzquierda;
    private Vector3 escalaOriginal;

    [Header("Referencias")]
    public Player playerScript; 
    private Animator animator;
    private Transform miTransform;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        miTransform = transform;
        escalaOriginal = transform.localScale;

        if (playerScript == null)
            playerScript = FindFirstObjectByType<Player>();
    }

    void Update()
    {
        if (playerScript == null) return;

        float distancia = Vector2.Distance(miTransform.position, playerScript.transform.position);


        if (distancia > distanciaVision)
        {
            animator.SetInteger("Moving", 0);
        }
        else if (distancia > distanciaAtaque)
        {
            animator.SetInteger("Moving", 1);
            animator.SetInteger("Attack", 0); 

            PerseguirJugador();
        }
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
        FlipSprite(direccion.x);
    }

    void AttackPlayer()
    {

        animator.SetInteger("Attack", 1);

    }

    public void DealDamage()
    {
        if (playerScript != null && Vector2.Distance(miTransform.position, playerScript.transform.position) <= distanciaAtaque)
        {
            Debug.Log("Golpeando al jugador");
            playerScript.TakeDamage(damage);
        }
    }


    public void FinalizarAtaque()
    {
        animator.SetInteger("Attack", 0);
    }

    public void TakeDamage(int damageTaken)
    {
        currentHealth -= damageTaken;
        Debug.Log("Enemigo herido. Vida: " + currentHealth);

        animator.SetInteger("Hurt", 1);

        if (currentHealth <= 0) Die();
    }

    public void ResetHurtAnimation()
    {
        animator.SetInteger("Hurt", 0);
    }

    void Die()
    {
        animator.SetInteger("Hurt", 2); 
        GetComponent<Collider2D>().enabled = false; 

        Invoke("NextScene", 5f);
        this.enabled = false; 
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