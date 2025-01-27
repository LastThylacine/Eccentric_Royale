﻿using UnityEngine;

public abstract class UnitState : ScriptableObject
{
    protected Unit _unit;
    public virtual void Constructor(Unit unit)
    {
        _unit = unit;
    }
    public abstract void Init();
    public abstract void Run();
    public abstract void Finish();

#if UNITY_EDITOR
    public virtual void DebugDrawDistance(Unit unit) { }
#endif
}

public enum UnitStateType
{
    NONE = 0,
    DEFAULT = 1,
    CHASE = 2, 
    ATTACK = 3
}