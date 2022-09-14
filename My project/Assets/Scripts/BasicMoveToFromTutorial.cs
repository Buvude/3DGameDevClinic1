using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMoveToFromTutorial : MonoBehaviour
{
    // MoveTo.cs
    using UnityEngine;
    using UnityEngine.AI;
    
    public class MoveTo : MonoBehaviour
{

    public Transform goal;

    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
    }
}

// Update is called once per frame
void Update()
    {
        
    }
}
