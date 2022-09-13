// MoveTo.cs
using UnityEngine;
using UnityEngine.AI;

public class UnityDocAI : MonoBehaviour
{

    public Transform goal;
    /*private NavMeshAgent agent;*/
    void start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
    }

    /*public void Update()
    {
        agent.destination = goal.position;
    }*/
}
