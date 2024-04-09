using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "_NavMeshChase", menuName = "UnitState/NavMeshChase")]
public class NavMeshChase : UnitState
{
    private NavMeshAgent _agent;
    private bool _targetIsEnemy;
    private Unit _targetUnit;
    private float _startAttackDistance = 0;

    public override void Constructor(Unit unit)
    {
        base.Constructor(unit);

        _targetIsEnemy = !_unit.IsEnemy;

        _agent = _unit.GetComponent<NavMeshAgent>();
        if (!_agent) Debug.LogError($"There is no NavMeshAgent found on the character {_unit.name}");
    }

    public override void Init()
    {
        if (MapInfo.Instance.TryGetNearestUnit(_unit.transform.position, _targetIsEnemy, out _targetUnit, out float distance))
        {
            _startAttackDistance = _unit.Parameters.StartAttackDistance + _targetUnit.Parameters.ModelRadius;
        }
    }

    public override void Run()
    {
        if (!_targetUnit)
        {
            _unit.SetState(UnitStateType.DEFAULT);
            return;
        }

        float distanceToTarget = Vector3.Distance(_unit.transform.position, _targetUnit.transform.position);

        if (distanceToTarget > _unit.Parameters.StopChaseDistance) _unit.SetState(UnitStateType.DEFAULT);
        else if (distanceToTarget <= _startAttackDistance) _unit.SetState(UnitStateType.ATTACK);
        else _agent.SetDestination(_targetUnit.transform.position);
    }

    public override void Finish()
    {
        _agent.SetDestination(_unit.transform.position);
    }

#if UNITY_EDITOR
    public override void DebugDrawDistance(Unit unit)
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(unit.transform.position, Vector3.up, unit.Parameters.StartChaseDistance);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(unit.transform.position, Vector3.up, unit.Parameters.StopChaseDistance);
    }
#endif
}