using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Vector3 _spawnPoint;
    private float _distance = 20;
    private float _seconds = 10;

    private void Start()
    {
        _spawnPoint = transform.position;

        _navMeshAgent = GetComponentInChildren<NavMeshAgent>();

        StartCoroutine("NewPointTimer");
    }

    private IEnumerator NewPointTimer()
    {
        yield return new WaitForSeconds(_seconds);

        Vector3 newPoint = _spawnPoint;
        newPoint.x += Random.Range(-_distance, _distance);
        newPoint.z += Random.Range(-_distance, _distance);
        _navMeshAgent.SetDestination(newPoint);

        StartCoroutine("NewPointTimer");
    }
}
