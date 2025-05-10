using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy4 : MonoBehaviour, ILevelReset
{
    public float patrolSpeed = 2f;
    public float pursuitSpeed = 6f;
    public EnemyStateEnum currentState;
    public Transform[] patrolPoints;
    public int patrolPointIndex;
    private NavMeshAgent agent;
    public PursuitTrigger pursuitTrigger;

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
        if (!pursuitTrigger.playerInRange) return false;
        float zDir = patrolPoints[patrolPointIndex].position.x - transform.position.x;
        float playerZDir = GameMgr.Instance.playerController.transform.position.x - transform.position.x;
        // Debug.Log(zDir + "\t" + playerZDir);
        return zDir * playerZDir > 0;
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
        patrolPointIndex = 0;
        agent.enabled = false;
        currentState = EnemyStateEnum.Patrol;
        transform.position = patrolPoints[patrolPointIndex].position;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        agent.enabled = true;
    }
}