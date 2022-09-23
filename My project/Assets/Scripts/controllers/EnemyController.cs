using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public GameObject self;
    public SpawnSpace spawnS;
    public float lookRadius;
    private Transform target;
    NavMeshAgent agent;
    public enum state { Return, spotted, dead };
    public state currentState = state.Return;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform; 
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != state.dead)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance <= lookRadius)
            {
                currentState = state.spotted;
                agent.SetDestination(target.position);
                
            }
            if (distance > lookRadius)
            {
                currentState = state.Return;
                agent.SetDestination(spawnS.spawn.position);
            }
        }

      /*  switch (currentState)
        {
            case state.Return:
                target = spawnS.spawn;
                agent.SetDestination(spawnS.spawn.position);
                break;
            case state.spotted:
                agent.SetDestination(target.position);
                break;
            case state.dead:
                spawnS.StartCoroutine("deadRespawn");
                break;
            default:
                break;
        }*/

    }
    public void postRespawn()
    {
        currentState = state.Return;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
