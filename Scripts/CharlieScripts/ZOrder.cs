using UnityEngine;

public class ZOrder : MonoBehaviour
{
    public Transform anchor;
    SpriteRenderer sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anchor == null)
        {
            sprite.sortingOrder = (int)(transform.position.y * -10);
        }
        else
        {
            sprite.sortingOrder = (int)(anchor.position.y * -10);
        }
    }
}
