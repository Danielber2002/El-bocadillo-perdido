using UnityEngine;

public class Jump : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(Vector3.up*5, ForceMode2D.Impulse);
            jumpTime = 1.0f;
        }

        if (jumpTime>0)
        {
            jumpTime -= Time.deltaTime;
        }
        else
        {
            if(rb.bodyType != RigidbodyType2D.Kinematic)
            {
                rb.linearVelocity = Vector3.zero;
            }
            rb.bodyType = RigidbodyType2D.Kinematic;
        }



    }

}
