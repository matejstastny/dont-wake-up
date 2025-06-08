/*
 * Author: Matěj Šťastný
 * Date created: 6/5/2025
 * GitHub link: https://github.com/matysta/dont-wake-up
 */

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem explosionParticle;

    [Header("Constants")]
    private const float Speed = 50f;
    private const float MaxDistance = 25f;

    [Header("State")]
    private Vector3 _startPosition;

    // Start --------------------------------------------------------------------------------------------
    
    private void Start()
    {
        _startPosition = transform.position;
        GetComponent<Rigidbody>().velocity = transform.up * Speed;
    }
    
    // Update -------------------------------------------------------------------------------------------

    private void Update()
    {
        if (!(Vector3.Distance(_startPosition, transform.position) >= MaxDistance)) return;
        Destroy(gameObject);
        Global.Log("Destroyed" + name);
    }
    
    // Collision ----------------------------------------------------------------------------------------

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            AudioSource.PlayClipAtPoint(player.GetComponent<AudioSource>().clip, player.transform.position);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        }
        Global.Log("Hit " + collision.gameObject.name);
        Destroy(gameObject);
    }
}
