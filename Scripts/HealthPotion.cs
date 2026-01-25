using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public int cantidadCura = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.Curar(cantidadCura);
                Destroy(gameObject);
            }
        }
    }
}