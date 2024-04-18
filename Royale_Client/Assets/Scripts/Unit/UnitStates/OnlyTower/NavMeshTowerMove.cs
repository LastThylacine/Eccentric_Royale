using UnityEngine;

[CreateAssetMenu(fileName = "_NavMeshTowerMove", menuName = "UnitState/NavMeshTowerMove")]
public class NavMeshTowerMove : UnitStateNavMeshMove
{
    protected override bool TryFindTarget(out UnitStateType changeType)
    {
        float distaneToTarget = _nearestTower.GetDistance(_unit.transform.position);

        if (distaneToTarget <= _unit.Parameters.StartAttackDistance)
        {
            changeType = UnitStateType.ATTACK;
            return true;
        }

        changeType = UnitStateType.NONE;
        return false;
    }
}
