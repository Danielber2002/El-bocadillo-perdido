using UnityEngine;

public class Jump : MonoBehaviour
{
    public Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up*10);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.AddForce(Vector3.down*5);
        }

    }
}
