using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class Enemy6 : MonoBehaviour, ILevelReset
{
    public float patrolSpeed = 2f;
    public float pursuitSpeed = 6f;
    public EnemyStateEnum currentState;
    private NavMeshAgent agent;
    public PursuitTrigger pursuitTrigger;
    public bool isWaring;
    public float warningMaxTime = 5f;
    public float warningTimer = 0f;
    private Vector3 initPos;

    private void Start()
    {
        initPos = transform.position;
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
                    SetDestination(initPos);
                }
            }
        }
    }

    private bool IsFindPlayer()
    {
        if (isWaring)
        {
            if (pursuitTrigger.playerInRange)
            {
                return GameMgr.Instance.playerController.transform.position.x - transform.position.x <= 0;
            }
        }

        return pursuitTrigger.playerInRange;
    }

    private void Pursuit()
    {
        agent.speed = pursuitSpeed;
        SetDestination(GameMgr.Instance.playerController.transform.position);
    }

    public void StartWarning(Vector3 pos)
    {
        if (currentState == EnemyStateEnum.Pursuit) return;
        warningTimer = 0f;
        isWaring = true;
        currentState = EnemyStateEnum.Patrol;
        SetDestination(pos);
    }

    public void LevelReset()
    {
        isWaring = false;
        agent.enabled = false;
        currentState = EnemyStateEnum.Patrol;
        transform.position = initPos;
        SetDestination(initPos);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        agent.enabled = true;
    }
}