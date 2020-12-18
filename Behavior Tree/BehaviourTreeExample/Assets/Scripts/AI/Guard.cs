using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    public BTBaseNode tree;
    private Animator animator;
    [Expandable] public VariableBool isArmed;
    [Expandable] public VariableBool engaged;
    [Expandable] public VariableBool isStunned;



    [Expandable] public VariableGameObject targetPlayer;
    public GameObject myPlayer;

    [Expandable] public VariableGameObject targetWeapon;
    public GameObject myWeapon;

    [Expandable] public VariableNavMeshAgent agentNavMesh;

    [Expandable] public VariableGameObject myAgent;
    public float spotDistance;

    [Expandable] public VariableFloat moveSpeed;

    [Expandable] public VariableGameObject wayPoint;
    public List<GameObject> waypoints = new List<GameObject>();


    private void Awake()
    {
        isArmed.Value = false;
        engaged.Value = false;
        agentNavMesh = Instantiate(agentNavMesh);
        agentNavMesh.Value = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        myAgent = Instantiate(myAgent);
        moveSpeed = Instantiate(moveSpeed);
        targetPlayer.Value = myPlayer;
        targetWeapon.Value = myWeapon;

        myAgent.Value = this.gameObject;

        wayPoint.Value = waypoints[0];
    }

    private void Start()
    {
        BTBaseNode engage =
            new BTPrioritySelector(
                new BTSequence(
                    new BTCheckTaskStatus(
                        new BTCheckRange(myAgent, targetPlayer, 15),
                            new BTSetBool(engaged,false),
                            new BTSetBool(Player.inCombat, false)
                        ),

                    new BTCheckBool(isArmed,false),

                    new BTCheckTaskStatus(
                        new BTInverter(new BTCheckRange(myAgent, targetWeapon, 1.5f)),
                        new BTSetBool(isArmed, true)
                        ),

                    new BTAnimate(animator, "Run"),
                    new BTMove(5, agentNavMesh, targetWeapon)
                    ),
                new BTSequence(
                    new BTCheckRange(myAgent, targetPlayer, 1),
                    new BTAnimate(animator, "Run"),
                    new BTMove(3, agentNavMesh, targetPlayer)
                    ),
                 new BTSequence(
                    new BTStop(agentNavMesh),
                    new BTAnimate(animator, "Kick","Idle")
                    )
                );
        engage._name = "Engage";

        BTBaseNode patrol =
             new BTPrioritySelector(
                 new BTSequence(
                    new BTCheckBool(engaged, false),

                    new BTCheckTaskStatus(
                        new BTSpot(targetPlayer, myAgent, spotDistance,true),
                            new BTSetBool(Player.inCombat,true),
                            new BTSetBool(engaged, true)
                        ),

                    new BTCheckRange(myAgent, wayPoint, 1),
                    new BTAnimate(animator, "Rifle Walk"),
                    new BTMove(2, agentNavMesh, wayPoint)
                     ),
                 new BTSequence(
                    new BTCheckBool(engaged, false),

                    new BTCheckTaskStatus(
                        new BTSpot(targetPlayer, myAgent, spotDistance,true),
                            new BTSetBool(Player.inCombat, true),
                            new BTSetBool(engaged, true)
                        ),

                    new BTAnimate(animator, "Idle"),
                    new BTWait(3),
                    new BTNextWaypoint(wayPoint, waypoints)// Get new Waypoint
                    ));
        patrol._name = "Patrol";

        BTBaseNode stunned =
            new BTPrioritySelector(
                new BTSequence(
                    new BTCheckBool(isStunned, true),
                    new BTAnimate(animator, "Scared"),
                    new BTStop(agentNavMesh),
                    new BTSetBool(Player.inCombat, false),

                    new BTCheckTaskStatus(
                        new BTWait(5),
                        new BTSetBool(engaged, false),
                        new BTSetBool(isStunned, false)

                        )
                    )
                );
        stunned._name = "Stunned";
               

        tree = new BTPrioritySelector(
            stunned,
            patrol,
            engage
            );
    }

    private void FixedUpdate()
    {
        tree?.Run();
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Handles.color = Color.yellow;
    //    Vector3 endPointLeft = viewTransform.position + (Quaternion.Euler(0, -ViewAngleInDegrees.Value, 0) * viewTransform.transform.forward).normalized * SightRange.Value;
    //    Vector3 endPointRight = viewTransform.position + (Quaternion.Euler(0, ViewAngleInDegrees.Value, 0) * viewTransform.transform.forward).normalized * SightRange.Value;

    //    Handles.DrawWireArc(viewTransform.position, Vector3.up, Quaternion.Euler(0, -ViewAngleInDegrees.Value, 0) * viewTransform.transform.forward, ViewAngleInDegrees.Value * 2, SightRange.Value);
    //    Gizmos.DrawLine(viewTransform.position, endPointLeft);
    //    Gizmos.DrawLine(viewTransform.position, endPointRight);

    //}
}
