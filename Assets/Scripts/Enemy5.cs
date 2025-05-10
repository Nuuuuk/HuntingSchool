using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class Enemy5 : MonoBehaviour, ILevelReset
{
    public float patrolSpeed = 2f;
    public float pursuitSpeed = 6f;
    public EnemyStateEnum currentState;
    public Transform leftDownPint, rightUpPoint;
    private NavMeshAgent agent;
    public PursuitTrigger rightDoorPursuitTrigger, roomPursuitTrigger;
    public bool isWaring;
    public float warningMaxTime = 5f;
    public float warningTimer = 0f;

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

    private Vector3 GetRandomPatrolPoint()
    {
        Vector3 pos = transform.position;
        pos.x = Random.Range(leftDownPint.position.x, rightUpPoint.position.x);
        pos.z = Random.Range(leftDownPint.position.z, rightUpPoint.position.z);
        return pos;
    }

    private void SetDestination(Vector3 pos)
    {
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(pos);
        }
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

        agent.speed = patrolSpeed;
        if (CheckPatrolEnd())
        {
            if (isWaring)
            {
                agent.speed = 0f;
                warningTimer += Time.deltaTime;
                if (warningTimer >= warningMaxTime)
                {
                    isWaring = false;
                    SetDestination(GetRandomPatrolPoint());
                }
            }
            else
            {
                SetDestination(GetRandomPatrolPoint());
            }
        }
    }

    private bool IsFindPlayer()
    {
        if (isWaring)
        {
            if (roomPursuitTrigger.playerInRange)
            {
                return GameMgr.Instance.playerController.transform.position.x - transform.position.x <= 0;
            }
            else
            {
                return false;
            }
        }

        return roomPursuitTrigger.playerInRange || rightDoorPursuitTrigger.playerInRange;
    }

    private void Pursuit()
    {
        agent.speed = pursuitSpeed;
        SetDestination(GameMgr.Instance.playerController.transform.position);
    }

    public void StartWarning(Vector3 pos)
    {
        if (currentState == EnemyStateEnum.Pursuit) return;
        isWaring = true;
        currentState = EnemyStateEnum.Patrol;
        SetDestination(pos);
    }

    public void StopPursuit()
    {
        warningTimer = 0;
        currentState = EnemyStateEnum.Patrol;
        SetDestination(GetRandomPatrolPoint());
    }

    public void LevelReset()
    {
        isWaring = false;
        var pos = GetRandomPatrolPoint();
        agent.enabled = false;
        currentState = EnemyStateEnum.Patrol;
        transform.position = pos;
        SetDestination(GetRandomPatrolPoint());
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        agent.enabled = true;
    }
}