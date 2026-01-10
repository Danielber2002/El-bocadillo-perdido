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
        LimitarPosicion();

    }

    void Movimiento()
    {
        direccion = Input.GetAxis("Horizontal");
        transform.position += Vector3.right * direccion * speed * Time.deltaTime;

        direccion = Input.GetAxis("Vertical");
        transform.position += Vector3.up * direccion * speed * Time.deltaTime;

    }
    public void LimitarPosicion()
    {
        Vector3 posicionLimitada = transform.position;
        posicionLimitada.x = Mathf.Clamp(transform.position.x, -10, 10);
        posicionLimitada.y = Mathf.Clamp(transform.position.y, -9, -4);
        transform.position = posicionLimitada;
    }
}
