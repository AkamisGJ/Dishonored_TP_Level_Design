using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Patrol : MonoBehaviour
{

    [TextArea]
    public string MyTextArea;
    public bool startPatrol = true;
    public List<Transform> myPatrolWaypoints = new List<Transform>();

    AI_Behaviour AI_behaviour;
    NavMeshAgent myAgent;
    int currentWaypoint = 0;
    bool goNextWaypoint = true;
    Animator anim;



    void Start()
    {
        AI_behaviour = GetComponent<AI_Behaviour>();
        myAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if (startPatrol) Invoke("StartPatrol", 0.5f);
    }





    void Update()
    {
        if(!AI_behaviour.isAvailableForScripting)
        {
            CancelInvoke("StartPatrol");
        }

        //AI continue patrolling when it reaches destination
        if(AI_behaviour.isAvailableForScripting)
        {
            if(myAgent.remainingDistance < 0.2f && goNextWaypoint)
            {
                if (myPatrolWaypoints[currentWaypoint].gameObject.GetComponent<Waypoint_Behaviour>().delay == 0)
                {
                    //Next waypoint
                    currentWaypoint++;
                    if (currentWaypoint >= myPatrolWaypoints.Count)
                    {
                        currentWaypoint = 0;
                    }

                    StartPatrol();
                }

                else

                {
                    Invoke("StartPatrol", myPatrolWaypoints[currentWaypoint].gameObject.GetComponent<Waypoint_Behaviour>().delay);

                    anim.CrossFade("Idle", 0.05f);

                    //Next waypoint
                    currentWaypoint++;
                    if (currentWaypoint >= myPatrolWaypoints.Count)
                    {
                        currentWaypoint = 0;
                    }
                    goNextWaypoint = false;
                }
            }
        }
    }




    public void StartPatrol()
    {
        AI_behaviour.SetDestination(myPatrolWaypoints[currentWaypoint], false);
        goNextWaypoint = true;
    }
}
