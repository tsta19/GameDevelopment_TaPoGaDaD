using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIBehaviour : MonoBehaviour
{
    // Player Reference
    public Transform player;
    
    public float wanderRadius;
    public float wanderTimer;
 
    private Transform target;
    private NavMeshAgent agent;
    private float timer;

    public Vector3 newDestination;
    
    // AI Properties
    private float _lineOfSight = 15;
    private float _playerSuspicion = 0;
    private bool _playerDetected = false;
    
    // AI States
    public AI_State ai_state;
    public enum AI_State
    {
        isIdle,
        isObserving,
        isChasing,
        isSearching
    }
    
    private bool _isIdle = false;
    private bool _isObserving = false;
    private bool _isChasing = false;
    private bool _isSearching = false;

    // Use this for initialization
    void OnEnable () {
        agent = GetComponent<NavMeshAgent> ();
        timer = wanderTimer;
    }
 
    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;
        
        Behaviour();
        LineOfSightDetection();
    }
 
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
 
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
        return navHit.position;
        
    }

    private void Behaviour()
    {
        switch (ai_state)
        {
            case AI_State.isIdle:
                Debug.Log("AI is Idle");
                if (!_playerDetected)
                {
                    if (timer >= wanderTimer) {
                        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                        agent.SetDestination(newPos);
                        newDestination = newPos;
                        timer = 0;
                    }
                }
                break;
            case AI_State.isObserving:
                Debug.Log("AI is Observing");
                if (_playerDetected)
                {
                    _isObserving = true;
                    transform.forward = player.transform.position;
                }

                _isObserving = false;
                break;
            case AI_State.isChasing:
                Debug.Log("AI is Chasing");
                break;
            case AI_State.isSearching:
                Debug.Log("AI is Searching");
                break;
            default:
                Debug.LogWarning("Something probably went");
                break;
        }
    }

    private void LineOfSightDetection()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);

        if (dist < _lineOfSight)
        {
            
            _playerDetected = true;
            ai_state = AI_State.isObserving;
            Debug.Log("IN LOS");
        }
        else
        {
            _playerDetected = false;
            ai_state = AI_State.isIdle;
            Debug.Log("NOT IN LOS");
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newDestination, 0.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _lineOfSight);
    }
}
