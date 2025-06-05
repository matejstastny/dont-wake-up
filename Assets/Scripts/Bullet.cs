using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private const float Speed = 20f;
    private const float MaxDistance = 20f;

    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
        GetComponent<Rigidbody>().velocity = transform.up * Speed;
    }

    private void Update()
    {
        if (!(Vector3.Distance(_startPosition, transform.position) >= MaxDistance)) return;
        Destroy(gameObject);
        Global.Log("destroyed" + name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Global.Log("Hit " + collision.gameObject.name);
        Destroy(gameObject);
    }
}