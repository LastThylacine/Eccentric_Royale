using UnityEngine;

public class UnitParameters : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; } = 4f;
    [field: SerializeField] public float ModelRadius { get; private set; } = 1f;
    public float StartAttackDistance { get { return ModelRadius + _startAttackDistance; } }
    public float StopAttackDistance { get { return ModelRadius + _stopAttackDistance; } }

    [SerializeField] public float _startAttackDistance = 1f;
    [SerializeField] public float _stopAttackDistance = 1f;
}
