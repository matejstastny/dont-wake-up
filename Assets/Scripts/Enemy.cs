using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    private Transform _player;
    private NavMeshAgent _agent;
    private int hp = 100;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!_player) return;
        float distance = Vector3.Distance(transform.position, _player.position);
        if (distance > 1.5f)
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
        Debug.Log(other.gameObject.name);
        if (!other.gameObject.CompareTag("Bullet")) return;
        hp -= 10;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}