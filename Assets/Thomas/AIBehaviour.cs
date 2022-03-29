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

    private FieldOfView FOVagent;

    // AI Properties
    private float _lineOfSight = 15;
    private float _playerSuspicion = 0;
    private float _suspicionThreshold = 100;
    private bool _playerDetected = false;
    private float dangerZone = 5;
    private float _suspicionTimer = 5.0f;

    private Vector3 suspicousPos;

    private bool susSound = false;
    // AI States
    public AI_State ai_state;

    
    // An Enumerator of the possible AI States
    public enum AI_State
    {
        isIdle,
        isObserving,
        isChasing,
        isSearching
        
    }

    // Booleans to determine what state the AI is currently in
    private bool _isIdle = false;
    private bool _isObserving = false;
    private bool _isChasing = false;
    private bool _isSearching = false;

    // Use this for initialization
    void OnEnable()
    {
        // Instances and timer initialization
        agent = GetComponent<NavMeshAgent>();
        FOVagent = GetComponent<FieldOfView>();
        timer = wanderTimer;
    }

    // Update is called once per frame
    void Update()
    {
        // Timer to control Idle state
        timer += Time.deltaTime;

        Behaviour();
        LineOfSightDetection();


        // Debug Input Actions
        if (ThirdPersonController.playerActionsAsset.Player.baslls.triggered)
        {
            _playerSuspicion += 10;
            print("SUSPicion increase: " + _playerSuspicion);
        }

        if (ThirdPersonController.playerActionsAsset.Player.cck.triggered)
        {
            _playerSuspicion -= 10;
            print("SUSPicion decrease: " + _playerSuspicion);
        }

    }

    // Function to randomly generate a new point that the AI wanders to
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;

    }

    // All the AI Behaviour
    private void Behaviour()
    {
        switch (ai_state)
        {
            /*
                Idle state is the state where the AI wanders around from the following parameters:
                public float wanderRadius; // Determines the radius where a new wander point will be generated within.
                public float wanderTimer; // Determines how long the AI has to move to said point before choosing a new one.
            */
            case AI_State.isIdle:
                if (!_playerDetected)
                {
                    if (!_isChasing)
                    {
                        ChaseIsDone();
                    }

                    if (timer >= wanderTimer)
                    {
                        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                        agent.SetDestination(newPos);
                        newDestination = newPos;
                        timer = 0;
                    }
                }
                break;
            
            /*
                Observering state is where the AI stands stil an "observes" the player where the AI will get increasingly
                more suspicious of the player defined from the following parameters:
                private float _playerSuspicion = 0; // Determines the players current suspicion to the AI.
                private float _suspicionThreshold = 100; // The threshold that triggers the chase state of the AI.
                private float _suspicionTimer = 5.0f; // How long the AI will observe before triggering Chase state.
            */
            case AI_State.isObserving:
                if (_playerDetected && _playerSuspicion < _suspicionThreshold)
                {
                    _isObserving = true;
                    transform.forward = player.transform.position;
                    if (_suspicionTimer > 0.0f)
                    {
                        _suspicionTimer -= Time.deltaTime;
                        print("THIS IS A TIMER OF 5 SECONDS " + _suspicionTimer);
                    }
                    else
                    {
                        _playerSuspicion = 101;
                    }
                }
                _isObserving = false;
                break;
            /*
                Chasing state is where the AI starts chasing the player as long as the player is in FOV the condition
                for this is that the player is detected and the suspicion is > 100.
            */
            case AI_State.isChasing:
                if (_playerDetected && _playerSuspicion >= _suspicionThreshold)
                {
                    Vector3 newPos = player.transform.position;
                    agent.SetDestination(newPos);
                    print("Chasing 4 real now");
                }
                break;
            /*
                Searching state is where the AI moves to the last known position of the player and searches the vicinity
                for x amount of seconds before moving back to the idle state.
            */
            case AI_State.isSearching:
                if (susSound)
                {
                    Vector3 newPos1 = suspicousPos;
                    agent.SetDestination(newPos1);
                    susSound = false;
                }
                break;
            default:
                Debug.LogWarning(this.GetType().Name + " Script Error: All states were missed (Default State Switch Case)");
                break;
        }
    }
    public void IHeardSomething()
    {
       
        print("I HEARD SOMETING");
        susSound = true;
        suspicousPos = player.position;
        ai_state = AI_State.isSearching; 
        
    }
    /*
        This is the function which first changes the AI state it checks to see whether the player is in the FOV radius.
        If the player is in FOV the player is detected and engages the observing state.
    */
    private void LineOfSightDetection()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);

        if (dist <= FOVagent.radius && FOVagent.FieldOfViewCheck())
        {
            _playerDetected = true;
            ai_state = AI_State.isObserving;
            //Debug.Log("IN LOS");
            if (_playerSuspicion > _suspicionThreshold)
            {
                ai_state = AI_State.isChasing;
            }
        }
        else
        {
            _playerDetected = false;
            ai_state = AI_State.isIdle;
            //Debug.Log("NOT IN LOS");
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newDestination, 0.5f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Collision AI, Player");
        }
    }

    private void ChaseIsDone()
    {
        _playerSuspicion = 0;
        _suspicionTimer = 5.0f;

    }

    
}
    