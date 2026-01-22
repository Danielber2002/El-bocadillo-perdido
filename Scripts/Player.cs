using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float speed = 5f;
    public float jumpForce = 15f;
    public float gravedad = 40f; // Gravedad manual (cuanto más alta, más rápido cae)

    [Header("Estado")]
    public bool isGrounded; // ¿Está tocando suelo?
    public bool isJumping;  // ¿Está en la fase de salto?

    private Rigidbody2D rb;
    private float velocidadVertical; // Variable para controlar la altura del salto
    private float sueloY; // Recordaremos en qué Y "profundidad" saltamos

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Aseguramos que sea Kinematic por código para evitar errores
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector2 movimientoFinal;

        // --- ESTADO 1: EN EL SUELO (Caminando) ---
        if (!isJumping)
        {
            // En el suelo, nos movemos en X y en Y (profundidad)
            movimientoFinal = new Vector2(inputX * speed, inputY * speed);
            velocidadVertical = 0f;

            // Detectar Salto
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                velocidadVertical = jumpForce;
                // Guardamos la Y actual como "nuestra línea de suelo" temporal
                sueloY = transform.position.y;
            }
        }
        // --- ESTADO 2: EN EL AIRE (Saltando) ---
        else
        {
            // Aplicamos gravedad manual
            velocidadVertical -= gravedad * Time.deltaTime;

            // En el aire:
            // X = Input (puedes moverte izquierda/derecha)
            // Y = Velocidad del salto (subir/bajar)
            // NOTA: Ignoramos inputY para no "caminar al fondo" mientras volamos
            movimientoFinal = new Vector2(inputX * speed, velocidadVertical);
        }

        // Aplicamos el movimiento al Rigidbody
        rb.linearVelocity = movimientoFinal;

        // Llamamos a los límites
        LimitarPosicion();

        // --- ATERRIZAJE MANUAL (Sin Colliders) ---
        // Si estamos cayendo Y pasamos por debajo de donde empezamos a saltar...
        if (isJumping && velocidadVertical < 0 && transform.position.y <= sueloY)
        {
            Aterrizar();
        }
    }

    /*public void AttackAction
    (
      if    
    )*/

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
        float xClamped = Mathf.Clamp(transform.position.x, -32, 32);

        // 2. Limite Vertical (Truco del Beat Em Up)
        float yClamped = transform.position.y;

        // Solo limitamos el TECHO (-8) si estamos caminando.
        // Si saltamos, el techo desaparece (ponemos 100 o infinito).
        if (!isJumping)
        {
            yClamped = Mathf.Clamp(transform.position.y, -13, -8);
        }
        else
        {
            // Si estamos saltando, solo nos preocupa no caer al infierno (-13)
            // pero dejamos que suba todo lo que quiera.
            yClamped = Mathf.Clamp(transform.position.y, -13, 100);
        }

        transform.position = new Vector3(xClamped, yClamped, transform.position.z);
    }
}