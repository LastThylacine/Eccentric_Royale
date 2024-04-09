using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "_NavMeshMove", menuName = "UnitState/NavMeshMove")]
public class NavMeshMove : UnitState
{
    private NavMeshAgent _agent;
    private bool _targetIsEnemy;
    private Tower _nearestTower;

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
        if (TryAttackTower()) return;
        if (TryAttackUnit()) return;
    }

    private bool TryAttackTower()
    {
        float distaneToTarget = _nearestTower.GetDistance(_unit.transform.position);

        if (distaneToTarget <= _unit.Parameters.StartAttackDistance)
        {
            _unit.SetState(UnitStateType.ATTACK);
            return true;
        }

        return false;
    }

    private bool TryAttackUnit()
    {
        bool hasEnemy = MapInfo.Instance.TryGetNearestUnit(_unit.transform.position,  _targetIsEnemy, out Unit enemy, out float distance);
        if (!hasEnemy) return false;

        if (_unit.Parameters.StartChaseDistance >= distance)
        {
            _unit.SetState(UnitStateType.CHASE);
            return true;
        }

        return false;
    }

    public override void Finish()
    {
        _agent.SetDestination(_unit.transform.position);
    }
}