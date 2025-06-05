using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const float Speed = 20f;
    private const float MaxDistance = 20f;

    private Vector3 _startPosition;

    void Start()
    {
        _startPosition = transform.position;
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb)
        {
            rb.velocity = transform.up * Speed;
        }
    }

    void Update()
    {
        if (!(Vector3.Distance(_startPosition, transform.position) >= MaxDistance)) return;
        Destroy(gameObject);
        Global.Log("destroyed" + name);
    }

    void OnCollisionEnter(Collision collision)
    {
        Global.Log("Hit " + collision.gameObject.name);
        Destroy(gameObject);
    }
}