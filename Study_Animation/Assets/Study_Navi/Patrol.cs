using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    public List<Transform> PatrolPoints;
    public NavMeshAgent Agent;

    private void Start()
    {
        SetDestinations(PatrolPoints);
        
        IEnumerator iEnum = PatrolCoroutine(PatrolPoints);
        StartCoroutine(iEnum);
        iEnum.MoveNext();
    }
    private void Update()
    {
        if (Agent.hasPath)
        {
            // Debug.Log(Agent.path.corners.Length);
            transform.forward = Agent.desiredVelocity;
        }
    }
    public void SetDestinations(List<Transform> patrolPoints)
    {
        StartCoroutine(PatrolCoroutine(patrolPoints));
    }
    private IEnumerator PatrolCoroutine(List<Transform> patrolPoints)
    {
        while (gameObject.activeInHierarchy)
        {
            for (int i = 0; i < patrolPoints.Count; i++)
            {
                Agent.SetDestination(patrolPoints[i].position);
                // Debug.Log(patrolPoints[i].gameObject.name);
                while (Agent.pathPending)
                {
                    yield return null;
                }
                // Debug.Log("출발");
                while (Agent.remainingDistance > Agent.stoppingDistance + 0.05f)
                {
                    yield return null;
                }
                // Debug.Log("도착");
                // Debug.Log("지금부터 2초");
                yield return new WaitForSeconds(2.0f);
                // Debug.Log("2초 끝");
            }
        }
    }
}
