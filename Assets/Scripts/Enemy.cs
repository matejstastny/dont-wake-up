/*
 * Author: Matěj Šťastný
 * Date created: 6/5/2025
 * GitHub link: https://github.com/matysta/dont-wake-up
 */

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem spawnParticle;
    public ParticleSystem deathParticle;

    [Header("Private")]
    private GameManager _gameManager;
    private NavMeshAgent _agent;
    private Transform _player;
    private AudioSource _audioSource;

    [Header("State")]
    private bool _canHitPlayer = true;
    private int _hp = 60;
    
    // Start --------------------------------------------------------------------------------------------

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj) _player = playerObj.transform;

        GameObject gameManagerObj = GameObject.FindGameObjectWithTag("GameManager");
        if (gameManagerObj) _gameManager = gameManagerObj.GetComponent<GameManager>();

        _agent = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();

        spawnParticle?.Play();
    }
    
    // Update -------------------------------------------------------------------------------------------
    
    private void Update()
    {
        if (!_player || !_gameManager) return;

        float distance = Vector3.Distance(transform.position, _player.position);

        if (distance < 1.5f && _canHitPlayer)
        {
            StartCoroutine(HitPlayer());
        }

        if (distance > 1.3f && !_gameManager.IsPaused())
        {
            _agent.SetDestination(_player.position);
        }
        else
        {
            _agent.ResetPath();
        }
    }
    
    // Collision ----------------------------------------------------------------------------------------

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Bullet")) return;

        _hp -= 10;
        if (_hp > 0) return;

        if (_audioSource && _player)
        {
            AudioSource.PlayClipAtPoint(_audioSource.clip, _player.position);
        }

        if (deathParticle)
        {
            Instantiate(deathParticle, transform.position, deathParticle.transform.rotation);
        }

        Destroy(gameObject);
    }
    
    // Events -------------------------------------------------------------------------------------------

    private IEnumerator HitPlayer()
    {
        _canHitPlayer = false;
        _gameManager.TakeDamage();
        yield return new WaitForSeconds(1f);
        _canHitPlayer = true;
    }
}
