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
        if (currentTime >= spawnTime)
        {
            int randomEnemy = Random.Range(0, 2);
            Instantiate(enemies[randomEnemy], transform.position, transform.rotation);
            currentTime = 0;
        }
        else
        {
            currentTime += Time.deltaTime;
        }
    }
}
