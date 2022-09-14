// MoveTo.cs
using UnityEngine;
using UnityEngine.AI;

public class UnityDocAI : MonoBehaviour
{

    public GameObject goal;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.transform.position;
    }

    private void Update()
    {
        agent.destination = goal.transform.position;
    }
}