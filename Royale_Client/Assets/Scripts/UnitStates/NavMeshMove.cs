using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "_NavMeshMove", menuName = "UnitState/NavMeshMove")]
public class NavMeshMove : UnitState
{
    [SerializeField] private bool _isEnemy = false;
    [SerializeField] private float _moveOffset = 1f;
    private NavMeshAgent _agent;
    private Vector3 _targetPosition;

    public override void Init()
    {
        _agent = _unit.GetComponent<NavMeshAgent>();

        Vector3 unitPosition = _unit.transform.position;
        _targetPosition = MapInfo.Instance.GetNearestTowerPosition(in unitPosition, !_isEnemy);

        _agent.SetDestination(_targetPosition);
    }

    public override void Run()
    {
        float distaneToTarget = Vector3.Distance(_unit.transform.position, _targetPosition);

        if (distaneToTarget <= _moveOffset)
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
