using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public ParticleSystem spawnParticle;
    public ParticleSystem deathParticle;

    private GameManager _gameManager;
    private bool _canHitPlayer = true;
    private NavMeshAgent _agent;
    private Transform _player;
    private int _hp = 60;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _agent = GetComponent<NavMeshAgent>();
        spawnParticle.Play();
    }

    private void Update()
    {
        if (!_player) return;
        float distance = Vector3.Distance(transform.position, _player.position);
        if (distance < 2.3f && _canHitPlayer)
        {
                StartCoroutine(nameof(HitPlayer));
        }
        if (distance > 2f && !_gameManager.IsPaused())
        {
            _agent.SetDestination(_player.position);
        }
        else
        {
            _agent.ResetPath();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            _hp -= 10;
            if (_hp > 0) return;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            AudioSource.PlayClipAtPoint(GetComponent<AudioSource>().clip, player.transform.position);
            Instantiate(deathParticle, transform.position, deathParticle.transform.rotation);
            Destroy(gameObject);
        }
    }

    private IEnumerator HitPlayer()
    {
        _canHitPlayer = false;
        // TODO Hit player
        yield return new WaitForSeconds(1);
        _canHitPlayer = true;
    }
}
