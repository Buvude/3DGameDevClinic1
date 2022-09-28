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
    public enum state { Return, spotted, Idle, dead, Attacking };
    public state currentState = state.Return;


    private bool attackCooldown = true;


    public Animator Anim;
    // Start is called before the first frame update
    void Start()
    {
        Anim = gameObject.GetComponent<Animator>();
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
                if (distance > agent.stoppingDistance)
                {

                    StopAllCoroutines();
                    attackCooldown = true;

                    Anim.Play("Running");
                    currentState = state.spotted;
                    agent.SetDestination(target.position);
                }
                else
                {
                    //do an attack

                    //play attack
                    Anim.Play("Attack");
                    //set state to currently attacking
                    currentState = state.Attacking;
                    //dont go anywhere while attacking
                    agent.SetDestination(transform.position);



                    if (attackCooldown) {
                        attackCooldown = false;
                        StartCoroutine(RaptorAttack());
                            }

                }
               
                
            }
            if (distance > lookRadius)
            {
                Anim.Play("Idle");
                currentState = state.Return;
                agent.SetDestination(spawnS.spawn.position);
            }
        }
        else if(currentState == state.dead)
        {
            spawnS.StartCoroutine("deadRespawn");
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

    public void raptorDied()
    {
        currentState = state.dead;
        GameManager.RaptorKilled(1);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }

    private IEnumerator RaptorAttack()
    {
        RaycastHit[] hit;
        yield return new WaitForSeconds(.8f);
        print("wow I ran");
        hit = Physics.BoxCastAll(transform.position, new Vector3(.3f, .3f, .3f), transform.forward);
        foreach(RaycastHit h in hit)
        {
            if(h.transform.gameObject.tag == "Player")
            {
               
                GameManager.playerTakeDamage();
            }
        }
        attackCooldown = true;
    }


}
