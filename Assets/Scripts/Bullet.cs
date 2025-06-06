using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public ParticleSystem explosionParticle;
    
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().Play(0);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        }
        Global.Log("Hit " + collision.gameObject.name);
        Destroy(gameObject);
    }
}