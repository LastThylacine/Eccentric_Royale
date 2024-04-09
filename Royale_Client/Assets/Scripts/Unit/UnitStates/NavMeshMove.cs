using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "_NavMeshMove", menuName = "UnitState/NavMeshMove")]
public class NavMeshMove : UnitState
{
    private NavMeshAgent _agent;
    private Vector3 _targetPosition;
    private bool _targetIsEnemy;
    private Tower _nearestTower;

    public override void Constructor(Unit unit)
    {
        base.Constructor(unit);

        _targetIsEnemy = !_unit.IsEnemy;

        _agent = _unit.GetComponent<NavMeshAgent>();
        if (!_agent) Debug.LogError($"There is no {_agent} found on the character {_unit.name}");

        _agent.speed = _unit.Parameters.Speed;
        _agent.radius = _unit.Parameters.ModelRadius;
        _agent.stoppingDistance = _unit.Parameters.StartAttackDistance;
    }

    public override void Init()
    {
        Vector3 unitPosition = _unit.transform.position;
        _nearestTower = MapInfo.Instance.GetNearestTower(in unitPosition, _targetIsEnemy);
        _targetPosition = _nearestTower.transform.position;

        _agent.SetDestination(_targetPosition);
    }

    public override void Run()
    {
        TryAttackTower();
    }

    private void TryAttackTower()
    {
        float distaneToTarget = _nearestTower.GetDistance(_unit.transform.position);

        if (distaneToTarget <= _unit.Parameters.StartAttackDistance)
        {
            Debug.Log("Destination Achieved");
            _unit.SetState(UnitStateType.ATTACK);
        }
    }

    public override void Finish()
    {
        _agent.isStopped = true;
    }
}
