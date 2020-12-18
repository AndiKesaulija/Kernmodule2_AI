using UnityEngine;

[CreateAssetMenu(fileName = "VariableTransform_", menuName = "Variables/VariableTransform")]
public class VariableTransform : BaseScriptableObject
{
    //Old value, New value
    public System.Action<Transform, Transform> OnValueChanged;

    [SerializeField] private Transform value;


    public Transform Value
    {
        get { return value; }
        set
        {
            OnValueChanged?.Invoke(this.value, value);
            this.value = value;
        }
    }

}
