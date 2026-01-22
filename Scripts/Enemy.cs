using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direccion = (player.transform.position - transform.position).normalized;
        transform.position += direccion * speed * Time.deltaTime;
    }
}
