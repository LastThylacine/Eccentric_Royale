using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class UnitStateNavMeshMove : UnitState
{
    private NavMeshAgent _agent;
    protected bool _targetIsEnemy;
    protected Tower _nearestTower;

    public override void Constructor(Unit unit)
    {
        base.Constructor(unit);

        _targetIsEnemy = !_unit.IsEnemy;

        _agent = _unit.GetComponent<NavMeshAgent>();
        if (!_agent) Debug.LogError($"There is no NavMeshAgent found on the character {_unit.name}");

        _agent.speed = _unit.Parameters.Speed;
        _agent.radius = _unit.Parameters.ModelRadius;
        _agent.stoppingDistance = _unit.Parameters.StartAttackDistance;
    }

    public override void Init()
    {
        Vector3 unitPosition = _unit.transform.position;
        _nearestTower = MapInfo.Instance.GetNearestTower(in unitPosition, _targetIsEnemy);

        _agent.SetDestination(_nearestTower.transform.position);
    }

    public override void Run()
    {
        if (TryFindTarget(out UnitStateType changeType))
            _unit.SetState(changeType);
    }

    public override void Finish()
    {
        _agent.SetDestination(_unit.transform.position);
    }

    protected abstract bool TryFindTarget(out UnitStateType changeType);
}
