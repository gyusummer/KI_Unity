using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Study_NavAgent : MonoBehaviour
{
    public Transform Goal;
    public NavMeshAgent Agent;
    private Coroutine curCoroutine = null;

    private void Update()
    {
        if (Agent.hasPath)
        {
            Debug.Log(Agent.path.corners.Length);
            transform.forward = Agent.desiredVelocity;
        }
    }

    public void SetGoal(Vector3 goal)
    {
        if (curCoroutine != null)
        {
            StopCoroutine(curCoroutine);
        }
        Agent.SetDestination(goal);
    }
    public void SetDestinations(Vector3[] destinations)
    {
        curCoroutine = StartCoroutine(MoveToDestinations(destinations));
    }

    private IEnumerator MoveToDestinations(Vector3[] destinations)
    {
        for (int i = 0; i < destinations.Length; i++)
        {
            Agent.SetDestination(destinations[i]);
            while (Agent.pathPending)
            {
                yield return null;
            }
            while (Agent.remainingDistance > Agent.stoppingDistance + 0.05f)
            {
                yield return null;
            }
        }
        curCoroutine = null;
    }
}
