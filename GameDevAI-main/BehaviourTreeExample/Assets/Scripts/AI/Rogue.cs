using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class Rogue : MonoBehaviour
{

    private Animator animator;
    [Expandable] public VariableBool hidden;


    [Expandable] public VariableGameObject Enemy;
    public GameObject myEnemy;

    [Expandable] public VariableGameObject myPlayer;
    public GameObject myPlayerObject;

    [Expandable] public VariableGameObject Target;
    public GameObject myTarget;

    [Expandable] public VariableGameObject myAgent;
    [Expandable] public VariableNavMeshAgent myNavMesh;

    [Expandable] public VariableFloat moveSpeed;

    public GameObject hideSpots;
    public VariableGameObject hideSpot;

    [Expandable] public List<VariableGameObject> hideSpotCollection = new List<VariableGameObject>();


    public BTBaseNode tree;

    private void Awake()
    {
        myNavMesh = Instantiate(myNavMesh);
        myNavMesh.Value = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        myAgent = Instantiate(myAgent);
        moveSpeed = Instantiate(moveSpeed);

        
    }

    private void Start()
    {

        for (int i = 0; i < hideSpots.transform.childCount; i++)
        {
            VariableGameObject spot = Instantiate(hideSpot);
            spot.Value = hideSpots.transform.GetChild(i).gameObject;
            hideSpotCollection.Add(spot); 
        }

        myAgent.Value = this.gameObject;
        myPlayer.Value = myPlayerObject;
        Target.Value = myTarget;
        Enemy.Value = myEnemy;
        moveSpeed.Value = 0;


        //TODO: Create your Behaviour tree here
        BTBaseNode idle =
            new BTSequence(
                new BTSetTarget(Target, myPlayer),
                new BTInverter(new BTCheckRange(myAgent, Target, 2)),
                new BTAnimate(animator, "Crouch Idle"),
                new BTStop(myNavMesh)
                );
        idle._name = "Idle";

        BTBaseNode follow =
            new BTPrioritySelector(
                new BTSequence(
                    new BTSetTarget(Target, myPlayer),
                    new BTCheckRange(myAgent, Target, 4),
                    new BTAnimate(animator, "Run"),
                    new BTMove(2.5f, myNavMesh, Target)
                    ),
                new BTSequence(
                    new BTCheckRange(myAgent, Target, 2),
                    new BTAnimate(animator, "Walk Crouch"),
                    new BTMove(2, myNavMesh, Target)
                    ),
                new BTSequence(
                    new BTAnimate(animator, "Crouch Idle"),
                    new BTStop(myNavMesh)
                )
                );
        follow._name = "Follow";

        BTBaseNode hide =
            new BTPrioritySelector(
                new BTSequence(
                    new BTCheckBool(Player.inCombat , true),
                    new BTCheckBool(hidden, false),

                    new BTGetClosestHideSpot(hideSpotCollection, myAgent, Target, Enemy),
                    new BTCheckRange(myAgent, Target, 0.5f),

                    new BTAnimate(animator, "Run"),
                    new BTMove(2.5f, myNavMesh, Target)

                    ),
                new BTSequence(
                    new BTCheckBool(Player.inCombat, true),
                    new BTCheckBool(hidden, false),
                    new BTStop(myNavMesh),
                    new BTAnimate(animator, "Crouch Idle"),

                    new BTCheckTaskStatus(
                        new BTWait(3),
                        new BTSetBool(hidden, true)
                        )
                    ),
                new BTSequence(
                    new BTCheckBool(Player.inCombat, true),
                    new BTCheckBool(hidden, true),

                    new BTCheckTaskStatus(
                        new BTSpot(Enemy,myAgent,200,false),
                        new BTSetBool(hidden,false)
                        ),

                    new BTCheckTaskStatus(
                            new BTCheckBool(Enemy.Value.GetComponent<Guard>().isStunned, false),
                            new BTSetBool(Enemy.Value.GetComponent<Guard>().isStunned, true),
                            new BTAnimate(animator, "Throw", "Crouch Idle"),
                            new BTSetBool(hidden, false)
                            )
                    )
                 );
        hide._name = "Hide";

        tree = new BTPrioritySelector(
            hide,
            follow
            );
        tree._name = "Tree";
        
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
