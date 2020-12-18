using UnityEngine;

[CreateAssetMenu(fileName = "VariableVector3_", menuName = "Variables/VariableVector3")]
public class VariableVector3 : BaseScriptableObject
{
    //Old value, New value
    public System.Action<Vector3, Vector3> OnValueChanged;
    [SerializeField] private Vector3 value;
    public Vector3 Value
    {
        get { return value; }
        set
        {
            OnValueChanged?.Invoke(this.value, value);
            this.value = value;
        }
    }
}
