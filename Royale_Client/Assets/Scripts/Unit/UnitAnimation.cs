using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    private const string STATE = "State";
    private const string ATTACK_SPEED = "AttackSpeed";

    [SerializeField] private Animator _animator;

    public void Init(Unit unit)
    {
        float damageDelay = unit.Parameters.DamageDelay;
        _animator.SetFloat(ATTACK_SPEED, 1 / damageDelay);
    }

    public void SetState(UnitStateType type)
    {
        _animator.SetInteger(STATE, (int)type);
    }
}
