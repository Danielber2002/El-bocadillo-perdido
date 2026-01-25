using UnityEngine;
using static UnityEngine.InputSystem.InputRemoting;

public class FollowCamera : MonoBehaviour
{
    public float speed = 15f;
    public GameObject target;
    public Vector3 offset;
    public Transform m_leftUpLimit;
    public Transform m_rightBottomLimit;

    private Vector3 targetPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position + offset;
            Vector3 targetDirection = (target.transform.position - posNoZ);
            float interpVelocity = targetDirection.magnitude * speed;
            targetPos = (transform.position) + (targetDirection.normalized * interpVelocity * Time.deltaTime);
            Vector3 m_cameraPosition = Vector3.Lerp(transform.position, targetPos, 0.25f);
            m_cameraPosition.x = Mathf.Clamp(m_cameraPosition.x, m_leftUpLimit.position.x, m_rightBottomLimit.position.x);
            m_cameraPosition.y = Mathf.Clamp(m_cameraPosition.y, m_rightBottomLimit.position.y, m_leftUpLimit.position.y);
            transform.position = m_cameraPosition;
        }    
    }
   
}
