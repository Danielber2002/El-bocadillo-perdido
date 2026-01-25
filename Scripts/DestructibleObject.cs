using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public int vidaCaja = 1; 
    public GameObject objetoA_Soltar;

    public void TakeDamage(int damage)
    {
        vidaCaja -= damage;

        if (vidaCaja <= 0)
        {
            RomperCaja();
        }
    }

    void RomperCaja()
    {
        if (objetoA_Soltar != null)
        {
            Instantiate(objetoA_Soltar, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}