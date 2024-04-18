using UnityEngine;

[CreateAssetMenu(fileName = "_NavMeshMeleeMove", menuName = "UnitState/NavMeshMeleeMove")]
public class NavMeshMeleeMove : UnitStateNavMeshMove
{
    protected override bool TryFindTarget(out UnitStateType changeType)
    {
        if (TryAttackTower())
        {
            changeType = UnitStateType.ATTACK;
            return true;
        }
        if (TryChaseUnit())
        {
            changeType = UnitStateType.CHASE;
            return true;
        }

        changeType = UnitStateType.NONE;
        return false;
    }

    private bool TryAttackTower()
    {
        float distaneToTarget = _nearestTower.GetDistance(_unit.transform.position);

        if (distaneToTarget <= _unit.Parameters.StartAttackDistance)
        {
            return true;
        }

        return false;
    }

    private bool TryChaseUnit()
    {
        bool hasEnemy = MapInfo.Instance.TryGetNearestWalkingUnit(_unit.transform.position, _targetIsEnemy, out Unit enemy, out float distance);
        if (!hasEnemy) return false;

        if (_unit.Parameters.StartChaseDistance >= distance)
        {
            return true;
        }

        return false;
    }
}
