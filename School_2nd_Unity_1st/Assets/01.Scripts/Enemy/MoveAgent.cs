using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour
{

    public Transform wayPointsObject;
    private List<Transform> wayPoints = new List<Transform>();

    public int nextIndex = 0;
    private NavMeshAgent agent;

    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;

    private bool _isPatrol;
    public bool isPatrol
    {
        get { return _isPatrol; }
        set 
        {
            _isPatrol = value;
            if(_isPatrol)
            {
                agent.speed = patrolSpeed;
                MoveWayPoint();
            }
        }
    }

    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            TraceTarget(_traceTarget);
        }
    }

    public float speed
    {
        get
        {
            return agent.velocity.magnitude;
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
    }

    void Start()
    {
        wayPointsObject.GetComponentsInChildren<Transform>(wayPoints);
        wayPoints.RemoveAt(0);
        MoveWayPoint();

        agent.speed = patrolSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPatrol) return;
        if(agent.velocity.sqrMagnitude >= 0.04f && agent.remainingDistance <= 0.5f)
        {
            nextIndex = (++nextIndex) % wayPoints.Count;
            MoveWayPoint();
        }
    }

    public void MoveWayPoint()
    {
        if (agent.isPathStale) return;

        agent.destination = wayPoints[nextIndex].position;

        agent.isStopped = false;
    }

    private void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;
    }

    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _isPatrol = false;
    }
}
