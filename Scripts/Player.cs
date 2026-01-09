using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    private float direccion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Movimiento();

    }

    void Movimiento()
    {
        // El jugador se mueve con teclas de A y D.
        direccion = Input.GetAxis("Horizontal");
        transform.position += Vector3.right * direccion * speed * Time.deltaTime;

        // El jugador se mueve con teclas de W y S.
        direccion = Input.GetAxis("Vertical");
        transform.position += Vector3.up * direccion * speed * Time.deltaTime;

    }
}
