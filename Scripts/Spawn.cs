using UnityEngine;

public class Spawn : MonoBehaviour
{
    public float spawnTime; // 5
    public float currentTime;
    public GameObject[] enemies;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
         
            if (currentTime >= spawnTime)
            {
                int randomEnemy = Random.Range(0, enemies.Length);
                Instantiate(enemies[randomEnemy], transform.position, transform.rotation);

                currentTime = 0; 
              
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }

}
