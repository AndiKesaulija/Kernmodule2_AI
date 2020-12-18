using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "VariableLayerMask_", menuName = "Variables/VariableLayerMask")]
public class VariableLayerMask : BaseScriptableObject
{
    //Old value, New value
    public System.Action<LayerMask, LayerMask> OnValueChanged;

    [SerializeField] private LayerMask value;


    public LayerMask Value
    {
        get { return value; }
        set
        {
            OnValueChanged?.Invoke(this.value, value);
            this.value = value;
        }
    }

}
