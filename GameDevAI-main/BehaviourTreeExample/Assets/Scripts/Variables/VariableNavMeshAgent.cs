using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "VariableNavMeshAgent_", menuName = "Variables/VariableNavMeshAgent")]
public class VariableNavMeshAgent : BaseScriptableObject
{
    //Old value, New value
    public System.Action<NavMeshAgent, NavMeshAgent> OnValueChanged;

    [SerializeField] private NavMeshAgent value;


    public NavMeshAgent Value
    {
        get { return value; }
        set
        {
            OnValueChanged?.Invoke(this.value, value);
            this.value = value;
        }
    }

}
