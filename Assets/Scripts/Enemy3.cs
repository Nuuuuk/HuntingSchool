using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStateEnum
{
    Patrol,
    Pursuit,
    Warning
}

public class Enemy3 : MonoBehaviour, ILevelReset
{
    public float patrolSpeed = 2f;
    public float pursuitSpeed = 6f;
    public EnemyStateEnum currentState;
    public Transform[] patrolPoints;
    public int patrolPointIndex;
    private NavMeshAgent agent;
    public List<PursuitTrigger> pursuitTriggers;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = EnemyStateEnum.Patrol;
        enabled = false;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyStateEnum.Patrol:
                Patrol();
                break;
            case EnemyStateEnum.Pursuit:
                Pursuit();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PursuitTrigger"))
        {
            pursuitTriggers.Add(other.GetComponent<PursuitTrigger>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PursuitTrigger"))
        {
            pursuitTriggers.Remove(other.GetComponent<PursuitTrigger>());
        }
    }

    private void SetDestination(Vector3 pos)
    {
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(pos);
        }
    }

    private void SetNextWaypoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;
        patrolPointIndex++;
        patrolPointIndex %= patrolPoints.Length;
    }

    bool CheckPatrolEnd()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            return true;
        }
        return false;
    }

    private void Patrol()
    {
        if (IsFindPlayer())
        {
            currentState = EnemyStateEnum.Pursuit;
            return;
        }

        if (CheckPatrolEnd())
        {
            SetNextWaypoint();
        }
        agent.speed = patrolSpeed;
        SetDestination(patrolPoints[patrolPointIndex].position);
    }

    private bool IsFindPlayer()
    {
        foreach (var trigger in pursuitTriggers)
        {
            if (trigger.playerInRange)
            {
                return true;
            }
        }

        return false;
    }

    private void Pursuit()
    {
        if (!IsFindPlayer())
        {
            currentState = EnemyStateEnum.Patrol;
            return;
        }
        agent.speed = pursuitSpeed;
        SetDestination(GameMgr.Instance.playerController.transform.position);
    }

    public void LevelReset()
    {
        agent.enabled = false;
        currentState = EnemyStateEnum.Patrol;
        patrolPointIndex = 0;
        transform.position = patrolPoints[patrolPointIndex].position;
        pursuitTriggers.Clear();
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        agent.enabled = true;
    }
}