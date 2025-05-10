using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Enemy7 : MonoBehaviour, ILevelReset
{
    public float patrolSpeed = 2f;
    public float pursuitSpeed = 6f;
    public EnemyStateEnum currentState;
    private NavMeshAgent agent;
    public PursuitTrigger roomPursuitTrigger;
    public PursuitTrigger[] outsidePursuitTriggers;
    public Transform[] patrolPoints;
    public int patrolPointIndex;

    public bool isWarning;
    public float warningTimer;
    public Vector3 warningPos;
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
            case EnemyStateEnum.Warning:
                Warning();
                break;
            case EnemyStateEnum.Pursuit:
                Pursuit();
                break;
        }
    }

    private void SetNextWaypoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;
        patrolPointIndex++;
        patrolPointIndex %= patrolPoints.Length;
    }

    private void SetDestination(Vector3 pos)
    {
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(pos);
        }
    }

    private bool CheckPatrolEnd()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            return true;
        }

        return false;
    }

    private void Patrol()
    {
        if (IsPlayerInRoom() && IsEnemyInRoom())
        {
            currentState = EnemyStateEnum.Pursuit;
            return;
        }


        agent.speed = patrolSpeed;
        if (CheckPatrolEnd())
        {
            SetNextWaypoint();
        }

        SetDestination(patrolPoints[patrolPointIndex].position);
    }

    private void Pursuit()
    {
        if (IsEnemyInRoom() && !IsPlayerInRoom())
        {
            if (isWarning)
            {
                currentState = EnemyStateEnum.Warning;
            }
            else
            {
                currentState = EnemyStateEnum.Patrol;
            }
            return;
        }
        if (IsEnemyOutside() && !IsPlayerOutside())
        {
            if (isWarning)
            {
                currentState = EnemyStateEnum.Warning;
            }
            else
            {
                currentState = EnemyStateEnum.Patrol;
            }
            return;
        }
        agent.speed = pursuitSpeed;
        SetDestination(GameMgr.Instance.playerController.transform.position);
    }

    public void StartWarning(Vector3 warningPos)
    {
        isWarning = true;
        currentState = EnemyStateEnum.Warning;
        warningTimer = 0f;
        agent.speed = patrolSpeed;
        this.warningPos = warningPos;
    }

    private void Warning()
    {
        if (IsEnemyOutside() && IsPlayerOutside())
        {
            currentState = EnemyStateEnum.Pursuit;
            return;
        }

        agent.speed = patrolSpeed;
        SetDestination(warningPos);

        if (CheckPatrolEnd())
        {
            isWarning = false;
            warningTimer += Time.deltaTime;
            if (warningTimer >= 8f)
            {
                currentState = EnemyStateEnum.Patrol;
            }
        }
    }

    private bool IsEnemyInRoom()
    {
        return roomPursuitTrigger.enemyInRange;
    }

    private bool IsEnemyOutside()
    {
        foreach (var pursuitTrigger in outsidePursuitTriggers)
        {
            if (pursuitTrigger.enemyInRange)
                return true;
        }

        return false;
    }

    private bool IsPlayerInRoom()
    {
        return roomPursuitTrigger.playerInRange;
    }

    private bool IsPlayerOutside()
    {
        foreach (var pursuitTrigger in outsidePursuitTriggers)
        {
            if (pursuitTrigger.playerInRange)
                return true;
        }

        return false;
    }

    public void LevelReset()
    {
        isWarning = false;
        warningTimer = 0f;
        patrolPointIndex = 0;
        agent.enabled = false;
        transform.position = patrolPoints[patrolPointIndex].position;
        currentState = EnemyStateEnum.Patrol;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        SetNextWaypoint();
        agent.enabled = true;
    }
}