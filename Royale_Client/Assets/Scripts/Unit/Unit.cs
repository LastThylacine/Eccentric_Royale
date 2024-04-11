using System;
using UnityEngine;

[RequireComponent(typeof(UnitParameters), typeof(Health))]
public class Unit : MonoBehaviour, IHealth
{
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public bool IsEnemy { get; private set; } = false;
    [field: SerializeField] public UnitParameters Parameters;
    [SerializeField] private UnitState _defaultStateSO;
    [SerializeField] private UnitState _chaseStateSO;
    [SerializeField] private UnitState _attackStateSO;

    private UnitState _defaultState;
    private UnitState _chaseState;
    private UnitState _attackState;
    private UnitState _currentState;

    private void Start()
    {
        _defaultState = Instantiate(_defaultStateSO);
        _defaultState.Constructor(this);

        _chaseState = Instantiate(_chaseStateSO);
        _chaseState.Constructor(this);

        _attackState = Instantiate(_attackStateSO);
        _attackState.Constructor(this);

        _currentState = _defaultState;
        _currentState.Init();
    }

    private void Update()
    {
        _currentState.Run();
    }

    public void SetState(UnitStateType type)
    {
        _currentState.Finish();

        switch (type)
        {
            case UnitStateType.DEFAULT:
                _currentState = _defaultState;
                break;
            case UnitStateType.CHASE:
                _currentState = _chaseState;
                break;
            case UnitStateType.ATTACK:
                _currentState = _attackState;
                break;
            default:
                Debug.LogError($"The state {type} is not processed");
                break;
        }

        _currentState.Init();
    }

#if UNITY_EDITOR
    [Space(24)]
    [SerializeField] private bool _debug = false;
    private void OnDrawGizmos()
    {
        if (!_debug) return;
        if (_chaseStateSO) _chaseStateSO.DebugDrawDistance(this);
    }

    public void Destroy()
    {
        MapInfo.Instance.RemoveUnit(this, IsEnemy);
        Destroy(gameObject);
    }
#endif
}