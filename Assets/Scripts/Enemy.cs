using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    private Transform _player;
    private NavMeshAgent _agent;

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
}