using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Empty", menuName = "UnitState/Empty")]
public class EmptyUnitState : UnitState
{
    public override void Init()
    {
        _unit.SetState(UnitStateType.DEFAULT);
    }

    public override void Run()
    {
    }

    public override void Finish()
    {
        Debug.LogWarning($"Unit {_unit.name} was in the empty state, it was flipped to the default state");
    }
}
